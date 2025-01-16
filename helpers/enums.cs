using System;

namespace TESTCS.helpers;

public static class EnumHelper
{
    public static string GetEnumName<TEnum>(TEnum value) where TEnum : struct, Enum
    {
        return value.ToString();
    }
}