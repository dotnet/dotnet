// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;
using System.Linq;

using Microsoft.VisualStudio.TestPlatform.Common.Utilities;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Microsoft.TestPlatform.Common.UnitTests.Utilities;

[TestClass]
public class InstallationContextTests
{
    private readonly Mock<IFileHelper> _mockFileHelper;
    private readonly InstallationContext _installationContext;

    public InstallationContextTests()
    {
        _mockFileHelper = new Mock<IFileHelper>();
        _installationContext = new InstallationContext(_mockFileHelper.Object);
    }

    [TestMethod]
    public void TryGetVisualStudioDirectoryShouldReturnTrueIfVsIsFound()
    {
        _mockFileHelper.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

        Assert.IsTrue(_installationContext.TryGetVisualStudioDirectory(out string visualStudioDirectory), "VS Install Directory returned false");

        Assert.IsTrue(Directory.Exists(visualStudioDirectory), "VS Install Directory doesn't exist");
    }

    [TestMethod]
    public void TryGetVisualStudioDirectoryShouldReturnFalseIfVsIsNotFound()
    {
        _mockFileHelper.Setup(m => m.Exists(It.IsAny<string>())).Returns(false);

        Assert.IsFalse(_installationContext.TryGetVisualStudioDirectory(out string visualStudioDirectory), "VS Install Directory returned true");

        Assert.IsTrue(string.IsNullOrEmpty(visualStudioDirectory), "VS Install Directory is not empty");
    }

    [TestMethod]
    public void GetVisualStudioPathShouldReturnPathToDevenvExecutable()
    {
        var devenvPath = _installationContext.GetVisualStudioPath(@"C:\temp");

        Assert.AreEqual(@"C:\temp\devenv.exe", devenvPath.Replace("/", "\\"));
    }

    [TestMethod]
    public void GetVisualStudioCommonLocationShouldReturnWellKnownLocations()
    {
        var expectedLocations = new[]
        {
            @"C:\temp\PrivateAssemblies",
            @"C:\temp\PublicAssemblies",
            @"C:\temp\CommonExtensions\Microsoft\TestWindow",
            @"C:\temp\CommonExtensions\Microsoft\TeamFoundation\Team Explorer",
            @"C:\temp"
        };
        var commonLocations = _installationContext.GetVisualStudioCommonLocations(@"C:\temp").Select(p => p.Replace("/", "\\")).ToArray();

        CollectionAssert.AreEquivalent(expectedLocations, commonLocations);
    }
}
