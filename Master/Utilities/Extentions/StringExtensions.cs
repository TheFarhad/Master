namespace Master.Utilities.Extentions;

using Microsoft.Extensions.DependencyModel;
using System.Reflection;
using System.Text;

public static class StringExtensions
{
    public const char ArabicYeChar = (char)1610;
    public const char PersianYeChar = (char)1740;

    public const char ArabicKeChar = (char)1603;
    public const char PersianKeChar = (char)1705;

    public static string PlaceHolder(this string source, string[] parameters)
    {
        var result = source;
        if (parameters?.Length > 0)
        {
            for (int i = 0; i < parameters.Length; i++)
                result = result.Replace($"{{{i}}}", parameters[i]);
        }
        return result;
    }

    public static bool IsNull(this string source) =>
         string.IsNullOrWhiteSpace(source);

    public static bool IsGuid(this string source, out Guid result) =>
        Guid.TryParse(source, out result);

    public static byte[] ToBytes(this string source) =>
       Encoding.UTF8.GetBytes(source);

    public static string ToBytes(this byte[] source) =>
       Encoding.UTF8.GetString(source);

    public static string ApplyCorrectYeKe(this object data) =>
         data.IsNull() ? null : data.ToString().ApplyCorrectYeKe();

    public static string ApplyCorrectYeKe(this string data)
    {
        return data.IsNull() ?
                    string.Empty :
                    data.Replace(ArabicYeChar, PersianYeChar).Replace(ArabicKeChar, PersianKeChar).Trim();
    }
}
