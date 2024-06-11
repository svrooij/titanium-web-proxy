﻿using System;
using System.Buffers;
using System.Buffers.Text;
using System.Globalization;
using System.Text;

namespace Titanium.Web.Proxy.Extensions;

internal static class StringExtensions
{
    internal static bool EqualsIgnoreCase ( this string str, string? value )
    {
        return str.Equals(value, StringComparison.CurrentCultureIgnoreCase);
    }

    internal static bool EqualsIgnoreCase ( this ReadOnlySpan<char> str, ReadOnlySpan<char> value )
    {
        return str.Equals(value, StringComparison.CurrentCultureIgnoreCase);
    }

    internal static bool ContainsIgnoreCase ( this string str, string? value )
    {
        if (value is null)
        {
            return false;
        }
        return CultureInfo.CurrentCulture.CompareInfo.IndexOf(str, value, CompareOptions.IgnoreCase) >= 0;
    }

    internal static int IndexOfIgnoreCase ( this string str, string? value )
    {
        if (value is null)
        {
            return -1;
        }
        return CultureInfo.CurrentCulture.CompareInfo.IndexOf(str, value, CompareOptions.IgnoreCase);
    }

    internal static unsafe string ByteArrayToHexString ( this ReadOnlySpan<byte> data )
    {
        if (data.Length == 0)
        {
            return string.Empty;
        }

        int length = data.Length * 3;
        Span<byte> buf = stackalloc byte[length];
        var buf2 = buf;
        foreach (var b in data)
        {
            Utf8Formatter.TryFormat(b, buf2, out _, new StandardFormat('X', 2));
            buf2[2] = 32; // space
            buf2 = buf2[3..];
        }

        return Encoding.UTF8.GetString(buf[..(length - 1)]);
    }
}
