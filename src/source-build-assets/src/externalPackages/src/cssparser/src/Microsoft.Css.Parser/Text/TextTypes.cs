// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;

namespace Microsoft.Css.Parser.Text
{
    [Flags]
    public enum TextTypes
    {
        None = 0,
        Razor = 1, // the text can contain Razor content, like @@import { }
    }
}
