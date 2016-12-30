using System;
using System.Linq;
using System.Transactions;
using Autofac;
using FoxTales.Infrastructure.DependencyInjection;

namespace FoxTales.Infrastructure.QueryFramework
{
    public abstract class QueryBase<T>
    {
        private readonly ILifetimeScope _lifetimeScope;

        protected QueryBase()
        {
            _lifetimeScope = AutofacBootstrapper.BeginLifetimeScopeByApplicationType();
        }

        public QueryResult<T> Execute(IsolationLevel isolationLevel)
        {
            return Execute(isolationLevel, _lifetimeScope);
        }

        internal QueryResult<T> Execute(IsolationLevel isolationLevel, ILifetimeScope lifetimeScope)
        {
            return QueryResult<T>.Create(_lifetimeScope, isolationLevel, OnExecuting(lifetimeScope));
        } 

        protected abstract IOrderedQueryable<T> OnExecuting(ILifetimeScope lifetimeScope);

        public QueryTask ToQueryTask(IsolationLevel isolationLevel, Action<QueryResult<T>> completedAction)
        {
            return QueryTask.Create(isolationLevel, this, completedAction);
        }
    }
}
