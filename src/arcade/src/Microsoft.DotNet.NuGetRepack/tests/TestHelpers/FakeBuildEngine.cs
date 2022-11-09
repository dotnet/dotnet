// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Xml.Linq;

namespace Microsoft.DotNet.Tools.Tests.Utilities
{
    internal sealed class FakeBuildEngine : IBuildEngine
    {
        // It's just a test helper so public fields is fine.
        public List<BuildErrorEventArgs> LogErrorEvents = new List<BuildErrorEventArgs>();

        public List<BuildMessageEventArgs> LogMessageEvents =
            new List<BuildMessageEventArgs>();

        public List<CustomBuildEventArgs> LogCustomEvents =
            new List<CustomBuildEventArgs>();

        public List<BuildWarningEventArgs> LogWarningEvents =
            new List<BuildWarningEventArgs>();

        public readonly List<ImmutableArray<XElement>> FilesToSign = new List<ImmutableArray<XElement>>();

        public bool BuildProjectFile(string projectFileName, string[] targetNames, IDictionary globalProperties, IDictionary targetOutputs)
        {
            return true;
        }

        public int ColumnNumberOfTaskNode
        {
            get { return 0; }
        }

        public bool ContinueOnError
        {
            get; set;
        }

        public int LineNumberOfTaskNode
        {
            get { return 0; }
        }

        public void LogCustomEvent(CustomBuildEventArgs e)
        {
            LogCustomEvents.Add(e);
        }

        public void LogErrorEvent(BuildErrorEventArgs e)
        {
            LogErrorEvents.Add(e);
        }

        public void LogMessageEvent(BuildMessageEventArgs e)
        {
            LogMessageEvents.Add(e);
        }

        public void LogWarningEvent(BuildWarningEventArgs e)
        {
            LogWarningEvents.Add(e);
        }

        public string ProjectFileOfTaskNode
        {
            get { return "fake ProjectFileOfTaskNode"; }
        }

    }
}
