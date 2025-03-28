// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.IO.Pipes;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace Microsoft.VisualStudioCode.RazorExtension;

internal sealed class RazorWorkspaceListener : RazorWorkspaceListenerBase
{
    public RazorWorkspaceListener(ILoggerFactory loggerFactory) : base(loggerFactory.CreateLogger<RazorWorkspaceListener>())
    {
    }

    /// <summary>
    /// Initializes the workspace and begins hooking up to workspace events. This is not thread safe
    /// but may be called multiple times.
    /// </summary>
    public void EnsureInitialized(Workspace workspace, string pipeName)
    {
        // Configuration of the server stream is very important. Please
        // be _very_ careful if changing any of the options used to initialize this.
        EnsureInitialized(workspace, () => new NamedPipeServerStream(
                pipeName,
                PipeDirection.Out,
                maxNumberOfServerInstances: 1,
                PipeTransmissionMode.Byte,
                PipeOptions.CurrentUserOnly | PipeOptions.Asynchronous));
    }

    private protected override Task CheckConnectionAsync(Stream stream, CancellationToken cancellationToken)
    {
        if (stream is NamedPipeServerStream { IsConnected: false } namedPipe)
        {
            return namedPipe.WaitForConnectionAsync(cancellationToken);
        }

        return Task.CompletedTask;
    }
}
