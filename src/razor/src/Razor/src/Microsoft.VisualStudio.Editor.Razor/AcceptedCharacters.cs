// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;

namespace Microsoft.VisualStudio.Editor.Razor;

[Flags]
internal enum AcceptedCharacters
{
    None = 0,
    NewLine = 1,
    WhiteSpace = 2,

    NonWhiteSpace = 4,

    AllWhiteSpace = NewLine | WhiteSpace,
    Any = AllWhiteSpace | NonWhiteSpace,

    AnyExceptNewline = NonWhiteSpace | WhiteSpace
}
