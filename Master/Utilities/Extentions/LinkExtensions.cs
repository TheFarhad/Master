namespace Master.Utilities.Extentions;

using System;
using System.Linq;
using System.Linq.Expressions;

public static class LinkExtensions
{
    public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string sortField, bool ascending)
    {
        var param = Expression.Parameter(typeof(T), "p");
        var prop = Expression.Property(param, sortField);
        var exp = Expression.Lambda(prop, param);
        var method = ascending ? "OrderBy" : "OrderByDescending";
        var types = new Type[] { source.ElementType, exp.Body.Type };
        var mce = Expression.Call(typeof(Queryable), method, types, source.Expression, exp);
        var result = source.Provider.CreateQuery<T>(mce);
        return result;
    }
}
