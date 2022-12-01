// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace Microsoft.VisualStudio.Editor.Razor;

internal enum BlockKind
{
    // Code
    Statement,
    Directive,
    Functions,
    Expression,
    Helper,

    // Markup
    Markup,
    Section,
    Template,

    // Special
    Comment,
    Tag,

    HtmlComment
}
