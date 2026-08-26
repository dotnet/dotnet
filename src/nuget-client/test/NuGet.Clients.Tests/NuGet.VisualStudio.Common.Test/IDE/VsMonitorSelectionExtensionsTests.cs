// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.VisualStudio.Sdk.TestFramework;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Moq;
using Xunit;

namespace NuGet.VisualStudio.Common.Test.IDE
{
    [Collection(MockedVS.Collection)]
    public class VsMonitorSelectionExtensionsTests
    {
        public VsMonitorSelectionExtensionsTests(GlobalServiceProvider globalServiceProvider)
        {
            globalServiceProvider.Reset();
        }

        [Fact]
        public async System.Threading.Tasks.Task GetActiveProject_WhenSelectionHasNoHierarchy_ReturnsNull()
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            var monitorSelection = Mock.Of<IVsMonitorSelection>();

            Assert.Null(monitorSelection.GetActiveProject());
        }
    }
}
