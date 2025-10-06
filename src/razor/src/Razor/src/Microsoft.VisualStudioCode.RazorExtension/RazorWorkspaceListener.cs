// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.IO;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;
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
