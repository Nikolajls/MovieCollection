using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Transactions;

namespace FoxTales.Infrastructure.Extensions.Queryable
{
    public static class QueryableExtensions
    {
        public static List<T> ToList<T>(this IQueryable<T> source, IsolationLevel isolationLevel)
        {
            return Transaction(source.ToList, isolationLevel);
        }

        public static Dictionary<TKey, TValue> ToDictionary<T, TKey, TValue>(this IQueryable<T> source,
            Func<T, TKey> keySelector, Func<T, TValue> elementSelector, IsolationLevel isolationLevel)
        {
            return Transaction(() => source.ToDictionary(keySelector, elementSelector), isolationLevel);
        }

        public static Dictionary<TKey, T> ToDictionary<T, TKey>(this IQueryable<T> source,
            Func<T, TKey> keySelector, IsolationLevel isolationLevel)
        {
            return Transaction(() => source.ToDictionary(keySelector), isolationLevel);
        }

        public static int Count<T>(this IQueryable<T> source, IsolationLevel isolationLevel, Func<T, bool> predicate = null)
        {
            return Transaction(() => predicate == null ? source.Count() : source.Count(predicate), isolationLevel);
        }

        public static T First<T>(this IQueryable<T> source, IsolationLevel isolationLevel, Func<T, bool> predicate = null)
        {
            return Transaction(() => predicate == null ? source.First() : source.First(predicate), isolationLevel);
        }

        public static T FirstOrDefault<T>(this IQueryable<T> source, IsolationLevel isolationLevel, Func<T, bool> predicate = null)
        {
            return Transaction(() => predicate == null ? source.FirstOrDefault() : source.FirstOrDefault(predicate), isolationLevel);
        }

        public static T Single<T>(this IQueryable<T> source, IsolationLevel isolationLevel, Func<T, bool> predicate = null)
        {
            return Transaction(() => predicate == null ? source.Single() : source.Single(predicate), isolationLevel);
        }

        public static T SingleOrDefault<T>(this IQueryable<T> source, IsolationLevel isolationLevel, Func<T, bool> predicate = null)
        {
            return Transaction(() => predicate == null ? source.SingleOrDefault() : source.SingleOrDefault(predicate), isolationLevel);
        }

        public static bool Any<T>(this IQueryable<T> source, IsolationLevel isolationLevel, Func<T, bool> predicate = null)
        {
            return Transaction(() => predicate == null ? source.Any() : source.Any(predicate), isolationLevel);
        }

        public static bool All<T>(this IQueryable<T> source, IsolationLevel isolationLevel, Func<T, bool> predicate)
        {
            return Transaction(() => source.All(predicate), isolationLevel);
        }

        public static bool Contains<T>(this IQueryable<T> source, T value, IsolationLevel isolationLevel)
        {
            return Transaction(() => source.Contains(value), isolationLevel);
        }

        private static T Transaction<T>(Func<T> method, IsolationLevel isolationLevel)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = isolationLevel }))
            {
                var result = method.Invoke();
                scope.Complete();
                return result;
            }
        }

        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string property)
        {
            return ApplyOrder(source, property, "OrderBy");
        }
        public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string property)
        {
            return ApplyOrder(source, property, "OrderByDescending");
        }
        public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, string property)
        {
            return ApplyOrder(source, property, "ThenBy");
        }
        public static IOrderedQueryable<T> ThenByDescending<T>(this IOrderedQueryable<T> source, string property)
        {
            return ApplyOrder(source, property, "ThenByDescending");
        }
        static IOrderedQueryable<T> ApplyOrder<T>(IQueryable<T> source, string property, string methodName)
        {
            string[] props = property.Split('.');
            Type type = typeof(T);
            ParameterExpression arg = Expression.Parameter(type, "x");
            Expression expr = arg;
            foreach (string prop in props)
            {
                // use reflection (not ComponentModel) to mirror LINQ
                PropertyInfo pi = type.GetProperty(prop);
                expr = Expression.Property(expr, pi);
                type = pi.PropertyType;
            }
            Type delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
            LambdaExpression lambda = Expression.Lambda(delegateType, expr, arg);

            object result = typeof(System.Linq.Queryable).GetMethods().Single(
                    method => method.Name == methodName
                            && method.IsGenericMethodDefinition
                            && method.GetGenericArguments().Length == 2
                            && method.GetParameters().Length == 2)
                    .MakeGenericMethod(typeof(T), type)
                    .Invoke(null, new object[] { source, lambda });
            return (IOrderedQueryable<T>)result;
        }
    }
}
