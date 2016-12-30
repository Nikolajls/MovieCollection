using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using FoxTales.Infrastructure.DTOFramework;
using FoxTales.Infrastructure.DTOFramework.Attributes;
using FoxTales.Infrastructure.Extensions.PropertyInfos;
using FoxTales.Infrastructure.QueryFramework;

namespace FoxTales.Infrastructure.MVCFramework
{
    public class Grid<TIdentity> where TIdentity : struct
    {
        public Type ViewModelType { get; private set; }
        public WebGrid WebGrid { get; set; }
        public List<WebGridColumn> Columns { get; set; }
        public IReadOnlyCollection<TIdentity> Selections { get; private set; }

        internal Grid()
        { }

        public static Grid<TIdentity> Create<T, TDTO, TMapper>(QueryResult<T> result, IEnumerable<TIdentity> selections)
            where TDTO : ILinkedDTO<TIdentity>, new()
            where TMapper : IDTOMapper<T, TDTO>, new()
        {
            selections = selections.ToList();

            var grid = new Grid<TIdentity>();
            grid.ViewModelType = typeof(TDTO);
            grid.Selections = selections.ToList().AsReadOnly();
            grid.Gridify<T, TDTO, TMapper>(result, selections);

            return grid;
        }

        public IHtmlString GetHtml()
        {
            return WebGrid.GetHtml(columns: Columns, firstText: "«", lastText: "»", mode: WebGridPagerModes.FirstLast | WebGridPagerModes.NextPrevious | WebGridPagerModes.Numeric);
        }

        private void Gridify<T, TDTO, TMapper>(QueryResult<T> result, IEnumerable<TIdentity> selections) where TDTO : ILinkedDTO<TIdentity>, new() where TMapper : IDTOMapper<T, TDTO>, new()
        {
            int page;
            int.TryParse(HttpContext.Current.Request["page"], out page);
            page = page == 0 ? 1 : page;

            int pageSize;
            int.TryParse(HttpContext.Current.Request["pageSize"], out pageSize);
            pageSize = pageSize == 0 ? 30 : pageSize;

            string columnNamesStr = HttpContext.Current.Request["columns"] ?? string.Empty;
            var columnNames = columnNamesStr.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            var excludeColumnNamesStr = HttpContext.Current.Request["excludeColumns"] ?? string.Empty;
            var excludeColumnNames = excludeColumnNamesStr.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            var grid = new WebGrid(rowsPerPage: pageSize);
            Columns = new List<WebGridColumn>();
            Columns.Add(new WebGridColumn
            {
                CanSort = false,
                ColumnName = "Id",
                Header = string.Empty,
                Format = a =>
                {
                    TIdentity id = a.Id;
                    string viewModel = typeof(T).FullName;
                    return new HtmlString(string.Format("<i class='chk{2}' data-id='{0}' data-view-model='{1}'></i>", id, viewModel, selections.Contains(id) ? " checked" : ""));
                },
                Style = "selection"
            });
            Columns.AddRange(
                typeof(TDTO).GetProperties()
                    .Where(p => p.Name != "Id" && p.GetAttribute<GridHiddenColumnAttribute, GridHiddenColumnAttribute>(h => h) == null)
                    .OrderBy(p => p.GetAttribute<GridColumnSetupAttribute, int>(a => a.Order, int.MaxValue))
                    .Select(
                        p => grid.Column(
                            p.Name,
                            style: p.GetAttribute<GridColumnSetupAttribute, string>(a => string.Format("column-width-{0} column-textalign-{2} minimum-window-width-{1}", a.CSSWidth, a.MinimumWindowWidth, a.TextAlignment.ToString().ToLower()), "autohide"),
                            format: x =>
                            {
                                var attribute = p.GetAttribute<DisplayFormatAttribute, string>(a => a.DataFormatString, "{0}");
                                return string.Format(attribute, x[p.Name] ?? string.Empty);
                            },
                            canSort: !string.IsNullOrWhiteSpace(p.GetAttribute<GridColumnSetupAttribute, string>(a => a.SortProperty, string.Empty)),
                            header: p.GetAttribute<DisplayAttribute, string>(a => a.Name))));

            if (columnNames.Any())
            {
                columnNames = new[] { "Id" }.Concat(columnNames).ToArray();
                Columns = Columns.Where(c => columnNames.Contains(c.ColumnName)).ToList();
            }

            if (excludeColumnNames.Any())
            {
                Columns = Columns.Where(c => !excludeColumnNames.Contains(c.ColumnName)).ToList();
            }

            var pageRows = result.GetPage<TDTO, TMapper>(page, pageSize);
            grid.Bind((IEnumerable<dynamic>)pageRows, Columns.Select(c => c.ColumnName), rowCount: pageRows.EntityCount, autoSortAndPage: false);

            WebGrid = grid;
        }
    }

    public class Grid : Grid<Guid>
    { }
}
