// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Razor.LanguageServer.Formatting;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.AutoInsert;

internal abstract class RazorOnAutoInsertProvider
{
    internal readonly ILogger Logger;

    public RazorOnAutoInsertProvider(ILoggerFactory loggerFactory)
    {
        if (loggerFactory is null)
        {
            throw new ArgumentNullException(nameof(loggerFactory));
        }

        Logger = loggerFactory.CreateLogger<RazorOnAutoInsertProvider>();
    }

    public abstract string TriggerCharacter { get; }

    public abstract bool TryResolveInsertion(Position position, FormattingContext context, [NotNullWhen(true)] out TextEdit? edit, out InsertTextFormat format);
}
