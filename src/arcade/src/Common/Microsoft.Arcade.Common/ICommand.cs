// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;

namespace Microsoft.Arcade.Common
{
    public interface ICommand
    {
        CommandResult Execute();

        ICommand CaptureStdErr();
        ICommand CaptureStdOut();
        ICommand EnvironmentVariable(string name, string value);
        ICommand ForwardStatus(TextWriter to = null);
        ICommand ForwardStdErr(TextWriter to = null);
        ICommand ForwardStdOut(TextWriter to = null);
        ICommand OnErrorLine(Action<string> handler);
        ICommand OnOutputLine(Action<string> handler);
        ICommand QuietBuildReporter();
        ICommand WorkingDirectory(string projectDirectory);
    }
}
