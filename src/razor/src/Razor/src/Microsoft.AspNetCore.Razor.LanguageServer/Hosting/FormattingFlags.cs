// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Hosting;

[Flags]
internal enum FormattingFlags
{
    Disabled = 0,
    Enabled = 1,
    OnPaste = 1 << 1,
    OnType = 1 << 2,
    All = Enabled | OnPaste | OnType
};
