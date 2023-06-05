namespace Master.Utilities.Extentions;

using System;
using System.Globalization;

public static class ObjectExtensions
{
    public static Type Type(this object source) =>
      source.GetType();

    public static bool IsNull(this object source) =>
        Equals(source, null) || source == default;

    public static bool IsNotNull(this object source) =>
       source is not null;

    public static bool IsNull(this object[] source) =>
       Equals(source, null) || source == default;

    public static bool IsNotNull(this object[] source) =>
      source is not null;

    public static bool Is<Type>(this object source) =>
      source is Type;

    public static T ChangeType<T>(this object source, CultureInfo culture) =>
        (T)Convert.ChangeType(source, typeof(T), culture);
}
