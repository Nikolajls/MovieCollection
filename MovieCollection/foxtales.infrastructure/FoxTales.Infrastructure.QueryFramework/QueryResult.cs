using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;
using Autofac;
using FoxTales.Infrastructure.DTOFramework;
using FoxTales.Infrastructure.Extensions.Queryable;

namespace FoxTales.Infrastructure.QueryFramework
{
    public class QueryResult<T>
    {
        private ILifetimeScope LifetimeScope { get; set; }
        private IsolationLevel IsolationLevel { get; set; }
        private IQueryable<T> OriginalQuery { get; set; }

        private QueryResult()
        { }

        internal static QueryResult<T> Create(ILifetimeScope lifetimeScope, IsolationLevel isolationLevel, IQueryable<T> query, int pageNumber = 1, int? pageSize = null)
        {
            var result = new QueryResult<T>();
            result.IsolationLevel = IsolationLevel.ReadUncommitted;
            result.OriginalQuery = query;
            result.LifetimeScope = lifetimeScope;
            return result;
        }

        public IReadOnlyCollection<TDTO> GetAll<TDTO, TMapper>() where TMapper : IDTOMapper<T, TDTO>,  new() where TDTO : IDTO, new()
        {
            return OriginalQuery.Select(new TMapper().ToDTO(LifetimeScope)).ToList(IsolationLevel);
        }

        public IReadOnlyCollection<TDTO> GetAll<TDTO>() where TDTO : IDTO, new()
        {
            return GetAll<TDTO, AutoDTOMapper<T, TDTO>>();
        }

        public IReadOnlyCollection<TKey> GetAll<TKey>(Expression<Func<T, TKey>> projector)
        {
            return OriginalQuery.Select(projector).ToList(IsolationLevel);
        }

        public ResultPage<TDTO> GetPage<TDTO, TMapper>(int page, int pageSize) where TMapper : IDTOMapper<T, TDTO>, new() where TDTO : IDTO, new()
        {
            var mapper = new TMapper();
            return new ResultPage<TDTO>(IsolationLevel, OriginalQuery.Select(mapper.ToDTO(LifetimeScope)).Skip((page-1)*pageSize).Take(pageSize), page, pageSize);
        }

        public ResultPage<TDTO> GetPage<TDTO>(int page, int pageSize) where TDTO : IDTO, new()
        {
            return GetPage<TDTO, AutoDTOMapper<T, TDTO>>(page, pageSize);
        }

        public ResultPage<TKey> GetPage<TKey>(Expression<Func<T, TKey>> projector, int page, int pageSize)
        {
            return new ResultPage<TKey>(IsolationLevel, OriginalQuery.Select(projector).Skip((page - 1)*pageSize).Take(pageSize), page, pageSize);
        }
    }

    public class ResultPage<TDTO> : IReadOnlyCollection<TDTO>
    {
        private IReadOnlyCollection<TDTO> Page { get; set; }
        public int EntityCount { get; private set; }
        public int PageCount { get; private set; }
        public int CurrentPage { get; private set; }
        public int PageSize { get; private set; }

        public int Count
        {
            get { return this.Count(); }
        }

        public ResultPage(IsolationLevel isolationLevel, IQueryable<TDTO> query, int page, int pageSize)
        {
            EntityCount = query.Count();
            PageCount = EntityCount/pageSize;
            Page = query.ToList(isolationLevel);
            CurrentPage = page;
            PageSize = pageSize;
        }

        public IEnumerator<TDTO> GetEnumerator()
        {
            return Page.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }
}