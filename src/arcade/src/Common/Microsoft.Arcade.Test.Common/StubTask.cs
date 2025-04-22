// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;

namespace Microsoft.Arcade.Test.Common
{
    public class StubTask : ITask
    {
        public StubTask(IBuildEngine buildEngine)
        {
            BuildEngine = buildEngine;
        }

        public StubTask() : this(new MockBuildEngine()) { }

        public IBuildEngine BuildEngine { get; set; }
        public ITaskHost HostObject { get; set; }

        public bool Execute()
        {
            return true;
        }
    }
}
