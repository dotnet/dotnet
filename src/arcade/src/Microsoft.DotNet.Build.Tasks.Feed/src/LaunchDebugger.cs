// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using  Microsoft.Build.Utilities;
using System.Diagnostics;

namespace Microsoft.DotNet.Build.Tasks.Feed
{
    public class LaunchDebugger : Microsoft.Build.Utilities.Task
    {
        public override bool Execute()
        {
            Debugger.Launch();
            return true;
        }
    }
}
