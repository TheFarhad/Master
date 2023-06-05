namespace Master.Utilities.Extentions;

using System;
using System.Linq;
using System.ComponentModel;

public static class EnumExtensions
{
    public static string Description(this Enum source)
    {
        var fieldInfo = source.Type().GetField(source.ToString());
        var attributes = fieldInfo?.GetCustomAttributes(typeof(DescriptionAttribute), false);
        var result = attributes.IsNotNull() && attributes.Any() ? ((DescriptionAttribute)attributes.First()).Description : source.ToString();
        return result;
    }
}
