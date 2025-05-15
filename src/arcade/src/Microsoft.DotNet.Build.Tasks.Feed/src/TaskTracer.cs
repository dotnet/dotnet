// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if NET
using Microsoft.Build.Framework;
using MsBuildUtils = Microsoft.Build.Utilities;

namespace Microsoft.DotNet.Build.Tasks.Feed;

sealed internal class TaskTracer : Microsoft.SymbolStore.ITracer
{
    readonly MsBuildUtils.TaskLoggingHelper _log;
    readonly bool _verbose;

    public TaskTracer(MsBuildUtils.TaskLoggingHelper log, bool verbose)
    {
        _log = log;
        _verbose = verbose;
    }

    public void WriteLine(string message)
    {
        WriteLine("{0}", message);
    }

    public void WriteLine(string format, params object[] arguments)
    {
        _log.LogMessage(MessageImportance.Low, format, arguments);
    }

    public void Information(string message)
    {
        Information("{0}", message);
    }

    public void Information(string format, params object[] arguments)
    {
        _log.LogMessage(MessageImportance.Normal, format, arguments);
    }

    public void Warning(string message)
    {
        Warning("{0}", message);
    }

    public void Warning(string format, params object[] arguments)
    {
        _log.LogWarning(format, arguments);
    }

    public void Error(string message)
    {
        Error("{0}", message);
    }

    public void Error(string format, params object[] arguments)
    {
        _log.LogError(format, arguments);
    }

    public void Verbose(string message)
    {
        Verbose("{0}", message);
    }

    public void Verbose(string format, params object[] arguments)
    {
        _log.LogMessage(_verbose ? MessageImportance.Normal : MessageImportance.Low, format, arguments);
    }
}
#endif
