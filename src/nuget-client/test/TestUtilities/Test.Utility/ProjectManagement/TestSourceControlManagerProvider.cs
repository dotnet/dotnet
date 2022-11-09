// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.Configuration;
using NuGet.ProjectManagement;

namespace Test.Utility
{
    public class TestSourceControlManagerProvider : ISourceControlManagerProvider
    {
        private TestSourceControlManager TestSourceControlManager { get; }

        public TestSourceControlManagerProvider(TestSourceControlManager testSourceControlManager)
        {
            if (testSourceControlManager == null)
            {
                throw new ArgumentNullException(nameof(testSourceControlManager));
            }

            TestSourceControlManager = testSourceControlManager;
        }

        public SourceControlManager GetSourceControlManager()
        {
            return TestSourceControlManager;
        }
    }
}
