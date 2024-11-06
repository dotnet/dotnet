// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Hosting;

[Flags]
internal enum FormattingFlags
{
    Disabled = 0,
    Enabled  = 1,
    OnPaste  = 1 << 1,
    OnType   = 1 << 2,
    All      = Enabled | OnPaste | OnType
};
