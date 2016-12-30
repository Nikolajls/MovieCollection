using System;
using System.Transactions;
using Autofac;
using FoxTales.Infrastructure.DependencyInjection;

namespace FoxTales.Infrastructure.QueryFramework
{
    public class QueryTask
    {
        private readonly ILifetimeScope _lifetimeScope;
        private readonly IsolationLevel _isolationLevel;
        private Action _execute;

        private QueryTask(IsolationLevel isolationLevel, ILifetimeScope lifetimeScope)
        {
            _lifetimeScope = lifetimeScope;
            _isolationLevel = isolationLevel;
        }

        public static QueryTask Create<T>(IsolationLevel isolationLevel, QueryBase<T> query, Action<QueryResult<T>> completedAction)
        {
            var lifetimeScope = AutofacBootstrapper.BeginLifetimeScopeByApplicationType();

            var task = new QueryTask(isolationLevel, lifetimeScope);
            task._execute = () =>
            {
                var result = query.Execute(task._isolationLevel, task._lifetimeScope);
                completedAction(result);
            };
            return task;
        }

        internal void Execute()
        {
            _execute();
        }
    }
}