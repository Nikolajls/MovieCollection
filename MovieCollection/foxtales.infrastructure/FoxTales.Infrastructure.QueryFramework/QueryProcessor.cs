using System.Threading.Tasks;

namespace FoxTales.Infrastructure.QueryFramework
{
    public static class QueryProcessor
    {
        public static void StartInParallel(params QueryTask[] tasks)
        {
            Parallel.ForEach(tasks, t => t.Execute());
        }
    }
}
