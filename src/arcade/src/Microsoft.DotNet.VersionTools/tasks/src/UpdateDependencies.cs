// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.DotNet.VersionTools.Automation;
using System.Linq;

namespace Microsoft.DotNet.Build.Tasks.VersionTools
{
    public class UpdateDependencies : BaseDependenciesTask
    {
        protected override void TraceListenedExecute()
        {
            DependencyUpdateUtils.Update(
                CreateUpdaters().ToArray(),
                CreateLocalDependencyInfos().ToArray());
        }
    }
}
