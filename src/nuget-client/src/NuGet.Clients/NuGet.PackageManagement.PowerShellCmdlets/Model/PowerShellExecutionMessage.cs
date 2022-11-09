// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Concurrent;
using NuGet.ProjectManagement;

namespace NuGet.PackageManagement.PowerShellCmdlets
{
    public class Message
    {
    }

    public class ExecutionCompleteMessage : Message
    {
    }

    public class LogMessage : Message
    {
        public LogMessage(MessageLevel level, string content)
        {
            Level = level;
            Content = content;
        }

        public MessageLevel Level { get; set; }

        public string Content { get; set; }
    }

    public class ScriptMessage : Message
    {
        public ScriptMessage(string scriptPath)
        {
            ScriptPath = scriptPath;
        }

        public string ScriptPath { get; set; }
    }

    /// <summary>
    /// A message inserted into the Package Manager Console to signal that all messages in front of
    /// this message in the <see cref="BlockingCollection{T}"/> should be flushed before
    /// continuing.
    /// </summary>
    public class FlushMessage : Message
    {
    }
}
