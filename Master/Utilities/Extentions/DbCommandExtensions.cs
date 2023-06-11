namespace Master.Utilities.Extentions;

using System.Data;
using System.Data.Common;

public static class DbCommandExtensions
{
    public static void ApplyCorrectYeKe(this DbCommand source)
    {
        source.CommandText = source.CommandText.ApplyCorrectYeKe();
        foreach (DbParameter item in source.Parameters)
        {
            switch (item.DbType)
            {
                case DbType.AnsiString:
                case DbType.AnsiStringFixedLength:
                case DbType.String:
                case DbType.StringFixedLength:
                case DbType.Xml:
                    item.Value = item.Value is DBNull ? item.Value : item.Value.ApplyCorrectYeKe();
                    break;
            }
        }
    }
}
