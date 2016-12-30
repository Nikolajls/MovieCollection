using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace FoxTales.Infrastructure.CommandFramework
{
    public static class CommandProcessor
    {
        public static void StartInParallel(params CommandTask[] tasks)
        {
            Parallel.ForEach(tasks, t => t.Execute());
        }

        public static void StartInParallel(IsolationLevel isolationLevel, params AbstractCommand[] tasks)
        {
            Parallel.ForEach(tasks, t => t.DoExecute(isolationLevel));
        }

        public static void Start(IsolationLevel isolationLevel, params AbstractCommand[] tasks)
        {
            foreach (var abstractCommand in tasks)
            {
                abstractCommand.DoExecute(isolationLevel);
            }
        }

        public static void Start(params CommandTask[] tasks)
        {
            foreach (var command in tasks)
            {
                command.Execute();
            }
        }
    }
}