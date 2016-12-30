using System.Linq.Expressions;

namespace FoxTales.Infrastructure.Extensions.Expressions
{
    public static class ExpressionExtensions
    {
        public static string GetName<T>(this Expression<T> me)
        {
            var body = me.Body as MemberExpression;

            if (body == null)
            {
                var ubody = (UnaryExpression) me.Body;
                body = ubody.Operand as MemberExpression;
            }

            return body == null ? string.Empty : body.Member.Name;
        }
    }
}