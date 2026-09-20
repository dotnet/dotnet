// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.Arcade.Test.Common;

public class MockBuildEngine : IBuildEngine
{
    public bool ContinueOnError => throw new NotImplementedException();

    public int LineNumberOfTaskNode => 0;

    public int ColumnNumberOfTaskNode => 0;

    public string ProjectFileOfTaskNode => "Fake File";

    public List<CustomBuildEventArgs> CustomBuildEvents = new List<CustomBuildEventArgs>();
    public List<BuildErrorEventArgs> BuildErrorEvents = new List<BuildErrorEventArgs>();
    public List<BuildMessageEventArgs> BuildMessageEvents = new List<BuildMessageEventArgs>();
    public List<BuildWarningEventArgs> BuildWarningEvents = new List<BuildWarningEventArgs>();

    /// <summary>
    /// Errors the task logged, joined into a single string so that test failure messages can
    /// report why a task unexpectedly failed instead of only showing the boolean result.
    /// </summary>
    public string ErrorSummary => BuildErrorEvents.Count == 0
        ? "(no errors were logged)"
        : string.Join(Environment.NewLine, BuildErrorEvents.ConvertAll(e => e.Message));

    public bool BuildProjectFile(string projectFileName, string[] targetNames, IDictionary globalProperties, IDictionary targetOutputs)
    {
        throw new NotImplementedException();
    }

    public void LogCustomEvent(CustomBuildEventArgs e)
    {
        CustomBuildEvents.Add(e);
    }

    public void LogErrorEvent(BuildErrorEventArgs e)
    {
        BuildErrorEvents.Add(e);
    }

    public void LogMessageEvent(BuildMessageEventArgs e)
    {
        BuildMessageEvents.Add(e);
    }

    public void LogWarningEvent(BuildWarningEventArgs e)
    {
        BuildWarningEvents.Add(e);
    }
}
