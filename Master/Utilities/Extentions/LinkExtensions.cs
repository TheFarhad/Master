namespace Master.Utilities.Extentions;

using System;
using System.Data;
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

    public static List<T> ToList<T>(this DataTable dataTable)
    {
        var result = new List<T>();

        foreach (DataRow item in dataTable.Rows)
            result.Add(GetItem<T>(item));

        return result;
    }

    private static T GetItem<T>(DataRow dr)
    {
        var temp = typeof(T);
        T result = Activator.CreateInstance<T>();

        foreach (DataColumn column in dr.Table.Columns)
        {
            foreach (var pro in temp.GetProperties())
            {
                if (pro.Name == column.ColumnName)
                    pro.SetValue(result, Convert.ChangeType(dr[column.ColumnName], pro.PropertyType), null);

                else
                    continue;
            }
        }
        return result;
    }
}
