using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;

namespace FoxTales.Infrastructure.Repository.EntityFramework6.Extensions
{
    public static class DbContextExtensions
    {
        public static DbRawSqlQuery<T> CallStoredProcedure<T>(this DbContext me, string storedProcedureName, params object[] parameters)
        {
            var sql = BuildStoredProcedureSql(storedProcedureName, parameters);
            return me.Database.SqlQuery<T>(sql, parameters);
        }

        public static void CallStoredProcedure(this DbContext me, string storedProcedureName, params object[] parameters)
        {
            var sql = BuildStoredProcedureSql(storedProcedureName, parameters);
            me.Database.ExecuteSqlCommand(sql, parameters);
        }

        public static T CallStoredProcedureWithReturn<T>(this DbContext me, string storedProcedureName, params object[] parameters)
        {
            var sql = BuildStoredProcedureSql(storedProcedureName, parameters);
            var sqlQuery = me.Database.SqlQuery<T>(sql, parameters);
            var resultTask = sqlQuery.FirstAsync();
            resultTask.Wait();
            return resultTask.Result;
        }

        private static string BuildStoredProcedureSql(string storedProcedureName, params object[] parameters)
        {
            var sqlBuilder = new StringBuilder();
            sqlBuilder.Append("EXEC ");
            sqlBuilder.Append(storedProcedureName);
            for (var i = 0; i < parameters.Length; i++)
            {
                sqlBuilder.AppendFormat(" @p{0}{1}", i, i != parameters.Length - 1 ? ", " : "");
            }
            return sqlBuilder.ToString();
        }
    }
}