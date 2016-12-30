using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;

namespace FoxTales.Infrastructure.RepositoryFramework
{
    public abstract class FullTextSearchBase
    {
        private readonly SqlConnection _sqlConnection;
        private readonly IDictionary<string, bool> _crawlCompleted = new Dictionary<string, bool>();
        private readonly IDictionary<string, DateTime?> _crawlCompletedDate = new Dictionary<string, DateTime?>();

        public IReadOnlyCollection<View> Views { get; set; }

        protected FullTextSearchBase(SqlConnection sqlConnection)
        {
            _sqlConnection = sqlConnection;

            var views = new List<View>();
            var viewNames = GetViewNames();
            // ReSharper disable LoopCanBeConvertedToQuery - Tak, men nej tak, jeg fatter jo ikke en lyd efterfølgende :)
            foreach (var viewName in viewNames)
            // ReSharper restore LoopCanBeConvertedToQuery
            {
                var cmd = _sqlConnection.CreateCommand();
                cmd.CommandText = "SELECT ORDINAL_POSITION, COLUMN_NAME FROM information_schema.columns WHERE table_schema = 'FullText' AND table_name = @p0";
                cmd.Parameters.Add(new SqlParameter("p0", viewName));

                var columns = new List<Column>();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var columnId = reader.GetInt32(0);
                        var columnName = reader.GetString(1);

                        columns.Add(new Column(columnId, columnName));
                    }
                }

                views.Add(new View(viewName, columns.ToArray()));
            }

            Views = views.AsReadOnly();
        }

        private IEnumerable<string> GetViewNames()
        {
            _sqlConnection.Open();
            var cmd = _sqlConnection.CreateCommand();
            cmd.CommandText = "SELECT v.name, has_crawl_completed, crawl_end_date FROM sys.views v JOIN sys.schemas s ON s.schema_id = v.schema_id JOIN sys.fulltext_indexes fi ON fi.object_id = v.object_id WHERE s.name = 'FullText' AND v.type = 'V' AND fi.is_enabled = 1";

            using (var reader = cmd.ExecuteReader())
            {
                var viewNames = new List<string>();
                while (reader.Read())
                {
                    var viewName = reader.GetString(0);
                    viewNames.Add(viewName);
                    _crawlCompleted.Add(viewName, reader.GetBoolean(1));

                    var completedDate = reader.GetSqlDateTime(2);
                    _crawlCompletedDate.Add(viewName, completedDate.IsNull ? new DateTime?() : completedDate.Value);
                }
                return viewNames;
            }
        }

        public IEnumerable<Result> Search(SearchModel searchModel)
        {
            try
            {
                searchModel.Results = Search(searchModel.Query, searchModel.MaxNumberOfResults, searchModel.Advanced).ToList();
                return searchModel.Results;
            }
            catch (Exception ex)
            {
                searchModel.Error = ex.Message;
                return new List<Result>();
            }
        }

        public IEnumerable<Result> Search(string query, int count = 1000, bool advanced = false)
        {
            if (!advanced) query = string.Format("\"{0}\"", query.Trim().Replace("\"", "\"\""));

            foreach (var view in Views)
            {
                var columnList = string.Join(", ", view.Columns.Where(c => c.IsForTable).Select(c => c.Name));

                var cmd = _sqlConnection.CreateCommand();
                cmd.CommandText = string.Format("SELECT TOP {2} DetailsText, DetailsURL, {0} FROM FullText.{1} WHERE Contains(*, @p0)", columnList, view.Name, count);

                var whereSql = GetWhereClause(view.Name);
                if (!string.IsNullOrWhiteSpace(whereSql)) cmd.CommandText = string.Format("{0} AND {1}", cmd.CommandText, whereSql);

                var orderBySql = GetOrderByClause(view.Name);
                if (!string.IsNullOrWhiteSpace(orderBySql)) cmd.CommandText = string.Format("{0} ORDER BY {1}", cmd.CommandText, orderBySql);

                cmd.Parameters.Add(new SqlParameter("p0", query));

                var stopwatch = Stopwatch.StartNew();
                var reader = cmd.ExecuteReader();
                stopwatch.Stop();
                using (reader)
                {
                    var columns = new List<Column>();
                    var rows = new List<Result.Row>();
                    while (reader.Read())
                    {
                        if (!columns.Any())
                        {
                            for (var i = 2; i < reader.FieldCount; i++)
                            {
                                columns.Add(view.Columns.Single(v => v.Name == reader.GetName(i)));
                            }
                        }

                        var detailsText = reader.GetString(0);
                        var detailsURL = reader.GetString(1);
                        var values = new List<object>();
                        for (var i = 2; i < reader.FieldCount; i++)
                        {
                            values.Add(reader.GetValue(i));
                        }
                        rows.Add(new Result.Row(detailsText, detailsURL, values));
                    }
                    yield return new Result(view, columns, rows, GetDetailsLinkHeader(view.Name), stopwatch.Elapsed, _crawlCompleted[view.Name], _crawlCompletedDate[view.Name]);
                }
            }
        }

        /// <summary>
        /// Returns an SQL string to be inserted after WHERE when doing full text search. Be careful not to introduce dangerous SQL injections using this function!
        /// </summary>
        /// <param name="viewName">The name of the view where the WHERE clause will be appended.</param>
        /// <returns>The SQL string to place after WHERE or an empty string if no WHERE clause is needed.</returns>
        public virtual string GetWhereClause(string viewName)
        {
            return string.Empty;
        }

        /// <summary>
        /// Returns an SQL string to be inserted after ORDER BY when doing full text search. Be careful not to introduce dangerous SQL injections using this function!
        /// </summary>
        /// <param name="viewName">The name of the view where the ORDER BY clause will be appended.</param>
        /// <returns>The SQL string to place after ORDER BY or an empty string if no ORDER BY clause is needed.</returns>
        public virtual string GetOrderByClause(string viewName)
        {
            return string.Empty;
        }

        /// <summary>
        /// Returns the header for the details link of the specified view name.
        /// </summary>
        /// <param name="viewName"></param>
        /// <returns></returns>
        public virtual string GetDetailsLinkHeader(string viewName)
        {
            return string.Empty;
        }

        public class Result
        {
            public View View { get; private set; }
            public IReadOnlyCollection<Column> Columns { get; private set; }
            public IReadOnlyCollection<Row> Rows { get; private set; }
            public string DetailsLinkHeader { get; private set; }
            public TimeSpan ElapsedTime { get; set; }
            public bool IsCrawlCompleted { get; set; }
            public DateTime? LastCrawlFinishedDate { get; set; }

            public Result(View view, IEnumerable<Column> columns, IEnumerable<Row> rows, string detailsLinkHeader, TimeSpan elapsedTime, bool isCrawlCompleted, DateTime? lastCrawlFinishedDate)
            {
                View = view;
                DetailsLinkHeader = detailsLinkHeader;
                ElapsedTime = elapsedTime;
                IsCrawlCompleted = isCrawlCompleted;
                LastCrawlFinishedDate = lastCrawlFinishedDate;
                Columns = columns.ToList();
                Rows = rows.ToList();
            }

            public class Row
            {
                public string DetailsText { get; set; }
                public string DetailsURL { get; set; }
                public IReadOnlyCollection<object> ColumnValues { get; set; }

                public Row(string detailsText, string detailsURL, IEnumerable<object> columnValues)
                {
                    DetailsText = detailsText;
                    DetailsURL = detailsURL;
                    ColumnValues = columnValues.ToArray();
                }
            }
        }

        public class View
        {
            public string Name { get; set; }
            public string Title { get; set; }
            public IReadOnlyCollection<Column> Columns { get; private set; }

            public View(string name, Column[] columns)
            {
                Name = name;
                Title = name.Replace("_", " ");
                Columns = columns.ToList().AsReadOnly();
            }
        }

        public class Column
        {
            public int Id { get; private set; }
            public string Name { get; private set; }
            public string Title { get; private set; }
            public bool IsForTable { get; private set; }

            public Column(int id, string name)
            {
                Id = id;
                Name = name;
                if (Name.StartsWith("UI_", StringComparison.InvariantCultureIgnoreCase))
                {
                    Title = Name.Substring(3).Replace("_", " ");
                    IsForTable = Name.StartsWith("UI_", StringComparison.InvariantCultureIgnoreCase);
                }
                else
                {
                    Title = Name;
                }
            }
        }

        public enum SortDirection
        {
            Ascending,
            Descending
        }

        public class SearchModel
        {
            public string Query { get; set; }
            public int MaxNumberOfResults { get; set; }
            public bool Advanced { get; set; }
            public string Error { get; set; }
            public IEnumerable<Result> Results { get; set; }

            public SearchModel()
            {
                MaxNumberOfResults = 1000;
            }
        }
    }
}
