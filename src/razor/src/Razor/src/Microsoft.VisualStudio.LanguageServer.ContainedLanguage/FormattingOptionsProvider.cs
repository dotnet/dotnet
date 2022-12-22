// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.VisualStudio.LanguageServer.ContainedLanguage;

public abstract class FormattingOptionsProvider
{
    public abstract FormattingOptions? GetOptions(Uri uri);
}
