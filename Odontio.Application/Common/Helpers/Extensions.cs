using System.Linq.Expressions;

namespace Odontio.Application.Common.Helpers;

public static class Extensions
{
    /// <summary>
    /// Filtra la consulta
    /// </summary>
    /// <param name="value">Valor a filtra</param>
    /// <param name="filterProps">Lista de propiedades filtrables de la entidad</param>
    /// <typeparam name="T">Tipo EntityBase</typeparam>
    public static IQueryable<T> Filter<T>(this IQueryable<T> query, string value, IEnumerable<string> filterProps)
    {
        var parameter = Expression.Parameter(typeof(T), "e");
        var constant = Expression.Constant(value.ToLower());
        var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
        var toLowerMethod = typeof(string).GetMethod("ToLower", System.Type.EmptyTypes);
        var members = new MemberExpression[filterProps.Count()];

        for (int i = 0; i < filterProps.Count(); i++)
        {
            if (filterProps.ElementAt(i).Contains('.'))
            {
                // el filtro es una propiedad de una entidad anidada
                // ej. u => u.Rol.Nombre
                Expression nestedMember = parameter;
                foreach (var prop in filterProps.ElementAt(i).Split('.'))
                {
                    nestedMember = Expression.PropertyOrField(nestedMember, prop);
                }

                members[i] = (MemberExpression)nestedMember;
            }
            else
            {
                // el filtro es una propiedad de la entidad
                // ej. u => u.Username
                members[i] = Expression.Property(parameter, filterProps.ElementAt(i));
            }
        }

        Expression? searchExp = null;
        foreach (var member in members)
        {
            if (member.Type == typeof(string) && toLowerMethod != null && containsMethod != null)
            {
                // e => e.Member != null
                var notNullExp = Expression.NotEqual(member, Expression.Constant(null));

                // e => e.Member.ToLower() 
                var toLowerExp = Expression.Call(member, toLowerMethod);

                // e => e.Member.Contains(value)
                var containsExp = Expression.Call(toLowerExp, containsMethod, constant);

                // e => e.Member != null && e.Member.Contains(value)
                var filterExpression = Expression.AndAlso(notNullExp, containsExp);

                searchExp = searchExp == null
                    ? (Expression)filterExpression
                    : Expression.OrElse(searchExp, filterExpression);
            }
            else
            {
                // e => e.Member == value
                var equalExp = Expression.Equal(Expression.Convert(member, typeof(object)), constant);
                searchExp = searchExp == null ? (Expression)equalExp : Expression.OrElse(searchExp, equalExp);
            }
        }

        if (searchExp != null)
        {
            var predicate = Expression.Lambda<Func<T, bool>>(searchExp, parameter);

            return query.Where(predicate);
        }

        return query;
    }

    /// <summary>
    /// Aplica ordenamiento a la consulta
    /// </summary>
    /// <param name="orderByProperties">Lista de columnas a ordernar con el formato {column_name}:{asc|desc}</param>
    /// <typeparam name="T">Tipo EntityBase</typeparam>
    public static IQueryable<T> OrderBy<T>(this IQueryable<T> query, IEnumerable<string> orderByProperties)
    {
        var type = typeof(T);
        var parameter = Expression.Parameter(type, "p");

        for (var i = 0; i < orderByProperties.Count(); i++)
        {
            var splitedOrder = orderByProperties.ElementAt(i).Split(':');
            var columnName = splitedOrder[0];
            var orderType = splitedOrder.Count() > 1 ? splitedOrder[1] : "asc";
            var member = columnName.Split('.').Aggregate((Expression)parameter, Expression.PropertyOrField);
            var expression = Expression.Lambda(member, parameter);
            var orderMethod = "";

            if (i == 0)
                orderMethod = orderType.ToLower() == "asc" ? "OrderBy" : "OrderByDescending";
            else // luego es ThenBy
                orderMethod = orderType.ToLower() == "asc" ? "ThenBy" : "ThenByDescending";

            Type[] types = new Type[] { type, expression.ReturnType };

            // OrderBy*(x => x.Cassette) or Order*(x => x.SlotNumber)
            // ThenBy*(x => x.Cassette) or ThenBy*(x => x.SlotNumber)
            var callExpression = Expression.Call(typeof(Queryable), orderMethod, types,
                query.Expression, expression);

            query = query.Provider.CreateQuery<T>(callExpression);
        }

        return query;
    }
}