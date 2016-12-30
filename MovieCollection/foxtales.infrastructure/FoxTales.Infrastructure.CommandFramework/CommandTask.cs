using Autofac;
using FoxTales.Infrastructure.DependencyInjection;
using System.Transactions;

namespace FoxTales.Infrastructure.CommandFramework
{
    public class CommandTask
    {
        private readonly AbstractCommand _command;
        private readonly IsolationLevel _isolationLevel;
        private readonly ILifetimeScope _lifetimeScope;

        private CommandTask(IsolationLevel isolationLevel, AbstractCommand command)
        {
            _isolationLevel = isolationLevel;
            _command = command;
            _lifetimeScope = AutofacBootstrapper.BeginLifetimeScopeByApplicationType();
        }

        public static CommandTask Create(IsolationLevel isolationLevel, AbstractCommand query)
        {
            return new CommandTask(isolationLevel, query);
        }

        public void Execute()
        {
            _command.Run(_isolationLevel, _lifetimeScope);
        }
    }
}