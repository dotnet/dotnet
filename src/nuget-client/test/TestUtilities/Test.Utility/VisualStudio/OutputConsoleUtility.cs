// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using Moq;
using NuGet.VisualStudio;

namespace Test.Utility.VisualStudio
{
    public static class OutputConsoleUtility
    {
        public static (Mock<IOutputConsoleProvider> mockIOutputConsoleProvider, Mock<IOutputConsole> mockIOutputConsole) GetMock()
        {
            var mockIOutputConsole = new Mock<IOutputConsole>();
            var mockIOutputConsoleProvider = new Mock<IOutputConsoleProvider>();
            mockIOutputConsoleProvider.Setup(ocp => ocp.CreatePackageManagerConsoleAsync())
                                 .ReturnsAsync(mockIOutputConsole.Object);

            return (mockIOutputConsoleProvider, mockIOutputConsole);
        }
    }
}
