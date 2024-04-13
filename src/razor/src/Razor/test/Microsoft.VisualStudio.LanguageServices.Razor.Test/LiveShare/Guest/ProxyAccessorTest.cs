// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.VisualStudio.LiveShare;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.Razor.LiveShare.Guest;

public class ProxyAccessorTest(ITestOutputHelper testOutput) : ToolingTestBase(testOutput)
{
    [UIFact]
    public void GetProjectHierarchyProxy_Caches()
    {
        // Arrange
        var projectHierarchyProxy = StrictMock.Of<IProjectHierarchyProxy>();

        var collaborationSessionMock = new StrictMock<CollaborationSession>();
        collaborationSessionMock
            .Setup(x => x.GetRemoteServiceAsync<IProjectHierarchyProxy>(typeof(IProjectHierarchyProxy).Name, CancellationToken.None))
            .ReturnsAsync(projectHierarchyProxy);

        var liveShareSessionAccessorMock = new StrictMock<ILiveShareSessionAccessor>();
        liveShareSessionAccessorMock
            .SetupGet(x => x.Session)
            .Returns(collaborationSessionMock.Object);

        var proxyAccessor = new ProxyAccessor(liveShareSessionAccessorMock.Object, JoinableTaskContext);

        // Act
        var proxy1 = proxyAccessor.GetProjectHierarchyProxy();
        var proxy2 = proxyAccessor.GetProjectHierarchyProxy();

        // Assert
        Assert.Same(proxy1, proxy2);
    }
}
