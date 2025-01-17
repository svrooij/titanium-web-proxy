﻿using System;
using Unobtanium.Web.Proxy.Models;

namespace Unobtanium.Web.Proxy.Helpers;

internal struct RequestStatusInfo
{
    public string Method { get; set; }

    public ByteString RequestUri { get; set; }

    public Version Version { get; set; }

    public readonly bool IsEmpty ()
    {
        return Method == null && RequestUri.Length == 0 && Version == null;
    }
}
