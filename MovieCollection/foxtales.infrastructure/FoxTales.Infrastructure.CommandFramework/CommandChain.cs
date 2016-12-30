using FoxTales.Infrastructure.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace FoxTales.Infrastructure.CommandFramework
{
    public static class CommandChain
    {
        public static void Start(IsolationLevel isolationLevel, params AbstractCommand[] commands)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions {IsolationLevel = isolationLevel}))
            {
                foreach (var command in commands)
                {
                    try
                    {
                        command.IsChained = true;
                        command.DoExecute(isolationLevel);
                        if (!command.IsValid)
                        {
                            return;
                        }
                    }
                    finally
                    {
                        command.IsChained = false;
                    }
                }
                scope.Complete();
            }
        }

        public static void StartAsync(IsolationLevel isolationLevel, params CommandBase[] commands)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions {IsolationLevel = isolationLevel}))
            {
                try
                {
                    commands.ToList().ForEach(c => c.IsChained = true);
                    Parallel.ForEach(commands, c =>
                    {
                        var lifetimeScope = AutofacBootstrapper.BeginLifetimeScopeByApplicationType();
                        c.Run(isolationLevel, lifetimeScope);
                    });
                }
                finally
                {
                    commands.ToList().ForEach(c => c.IsChained = false);
                }
                scope.Complete();
            }
        }
    }
}