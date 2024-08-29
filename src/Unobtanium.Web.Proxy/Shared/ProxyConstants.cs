﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unobtanium.Web.Proxy.Http;

namespace Unobtanium.Web.Proxy.Shared;

/// <summary>
///     Literals shared by Proxy Server
/// </summary>
internal class ProxyConstants
{
    internal static readonly char DotSplit = '.';

    internal static readonly string NewLine = "\r\n";
    internal static readonly byte[] NewLineBytes = [(byte)'\r', (byte)'\n'];

    internal static readonly HashSet<string> ProxySupportedCompressions =
        new(StringComparer.OrdinalIgnoreCase)
        {
            KnownHeaders.ContentEncodingGzip.String,
            KnownHeaders.ContentEncodingDeflate.String,
            KnownHeaders.ContentEncodingBrotli.String
        };

    internal static readonly Regex CnRemoverRegex =
        new(@"^CN\s*=\s*", RegexOptions.IgnoreCase | RegexOptions.Compiled);
}