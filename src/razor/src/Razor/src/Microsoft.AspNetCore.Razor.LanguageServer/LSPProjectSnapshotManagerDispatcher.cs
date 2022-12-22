// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.Extensions.Logging;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal class LSPProjectSnapshotManagerDispatcher : ProjectSnapshotManagerDispatcherBase
{
    private const string ThreadName = "Razor." + nameof(LSPProjectSnapshotManagerDispatcher);

    private readonly ILogger<LSPProjectSnapshotManagerDispatcher> _logger;

    public LSPProjectSnapshotManagerDispatcher(ILoggerFactory loggerFactory) : base(ThreadName)
    {
        if (loggerFactory is null)
        {
            throw new ArgumentNullException(nameof(loggerFactory));
        }

        _logger = loggerFactory.CreateLogger<LSPProjectSnapshotManagerDispatcher>();
    }

    public override void LogException(Exception ex) => _logger.LogError(ex, ThreadName + " encountered an exception.");
}
