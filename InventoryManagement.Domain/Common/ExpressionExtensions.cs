using System.Linq.Expressions;

namespace InventoryManagement.Domain.Common
{
    public static class ExpressionExtensions
    {
        /// <summary>
        /// Combines two boolean expressions using AndAlso logic with proper parameter replacement
        /// </summary>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            var parameter = Expression.Parameter(typeof(T), "t");

            //var body1 = ReplaceParameter(expr1.Body, expr1.Parameters[0], parameter);
            //var body2 = ReplaceParameter(expr2.Body, expr2.Parameters[0], parameter);
            //var combinedBody = Expression.AndAlso(body1, body2);

            var combinedBody = Expression.AndAlso(
                Expression.Invoke(expr1, parameter),
                Expression.Invoke(expr2, parameter)
            );

            return Expression.Lambda<Func<T, bool>>(combinedBody, parameter);
        }

        /// <summary>
        /// Combines two boolean expressions using OrElse logic with proper parameter replacement
        /// </summary>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            var parameter = Expression.Parameter(typeof(T), "t");

            //var body1 = ReplaceParameter(expr1.Body, expr1.Parameters[0], parameter);
            //var body2 = ReplaceParameter(expr2.Body, expr2.Parameters[0], parameter);
            //var combinedBody = Expression.OrElse(body1, body2);
            var combinedBody = Expression.OrElse(
                Expression.Invoke(expr1, parameter),
                Expression.Invoke(expr2, parameter)
            );

            return Expression.Lambda<Func<T, bool>>(combinedBody, parameter);
        }

        private static Expression ReplaceParameter(Expression expression, ParameterExpression oldParameter, ParameterExpression newParameter)
        {
            return new ParameterReplacerVisitor(oldParameter, newParameter).Visit(expression);
        }
    }
}
