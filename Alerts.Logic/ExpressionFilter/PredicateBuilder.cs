using System;
using System.Linq.Expressions;

namespace Alerts.Logic.ExpressionFilter
{
    /// <summary>
    /// Clase estática que proporciona métodos para construir expresiones de predicado.
    /// </summary>
    public static class PredicateBuilder
    {
        /// <summary>
        /// Combina dos expresiones de predicado con lógica "AND".
        /// </summary>
        /// <typeparam name="T">Tipo de entidad sobre la que se evalúan las expresiones de predicado.</typeparam>
        /// <param name="expr1">Primera expresión de predicado.</param>
        /// <param name="expr2">Segunda expresión de predicado.</param>
        /// <returns>Expresión de predicado resultante de la combinación de las dos expresiones originales con lógica "AND".</returns>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1,
                                                   Expression<Func<T, bool>> expr2)
        {
            // Invoca la segunda expresión con los parámetros de la primera
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters);
            // Combina las dos expresiones con lógica "AND"
            return Expression.Lambda<Func<T, bool>>(
                Expression.AndAlso(expr1.Body, invokedExpr), expr1.Parameters);
        }
    }
}
