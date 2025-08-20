using System.Linq.Expressions;

namespace InventoryManagement.Domain.Common
{
    /// <summary>
    /// Visitor pattern implementation for replacing parameter expressions
    /// </summary>
    internal class ParameterReplacerVisitor(ParameterExpression oldParameter, ParameterExpression newParameter) : ExpressionVisitor
    {
        protected override Expression VisitParameter(ParameterExpression node)
        {
            return node == oldParameter ? newParameter : base.VisitParameter(node);
        }
    }
}
