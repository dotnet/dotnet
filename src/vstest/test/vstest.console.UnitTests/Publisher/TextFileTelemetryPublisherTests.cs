// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

using Microsoft.VisualStudio.TestPlatform.CommandLine.Publisher;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace vstest.console.UnitTests.Publisher;

[TestClass]
public class TextFileTelemetryPublisherTests
{
    [TestMethod]
    public void LogToFileShouldCreateDirectoryIfNotExists()
    {
        var dummyDictionary = new Dictionary<string, object?>();
        var mockFileHelper = new Mock<IFileHelper>();
        mockFileHelper.Setup(fh => fh.DirectoryExists(It.IsAny<string>())).Returns(false);
        dummyDictionary.Add("DummyMessage://", "DummyValue");
        dummyDictionary.Add("Dummy2", "DummyValue2");

        // Act.
        TextFileTelemetryPublisher.LogToFile("dummyevent", dummyDictionary, mockFileHelper.Object);

        // Verify.
        mockFileHelper.Verify(fh => fh.CreateDirectory(It.IsAny<string>()), Times.Once);
    }

    [TestMethod]
    public void LogToFileShouldWriteAllText()
    {
        var dummyDictionary = new Dictionary<string, object?>();
        var mockFileHelper = new Mock<IFileHelper>();
        dummyDictionary.Add("DummyMessage://", "DummyValue");
        dummyDictionary.Add("Dummy2", "DummyValue2");

        // Act.
        TextFileTelemetryPublisher.LogToFile("dummyevent", dummyDictionary, mockFileHelper.Object);

        // Verify.
        mockFileHelper.Verify(fh => fh.WriteAllTextToFile(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }
}
