// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Microsoft.TestPlatform.Extensions.BlameDataCollector.UnitTests;

/// <summary>
/// The xml reader writer tests.
/// </summary>
[TestClass]
public class XmlReaderWriterTests
{
    private readonly TestableXmlReaderWriter _xmlReaderWriter;
    private readonly Mock<IFileHelper> _mockFileHelper;
    private readonly Mock<Stream> _mockStream;
    private readonly List<Guid> _testCaseList;
    private readonly Dictionary<Guid, BlameTestObject> _testObjectDictionary;
    private readonly BlameTestObject _blameTestObject;

    /// <summary>
    /// Initializes a new instance of the <see cref="XmlReaderWriterTests"/> class.
    /// </summary>
    public XmlReaderWriterTests()
    {
        _mockFileHelper = new Mock<IFileHelper>();
        _xmlReaderWriter = new TestableXmlReaderWriter(_mockFileHelper.Object);
        _mockStream = new Mock<Stream>();
        _testCaseList = new List<Guid>();
        _testObjectDictionary = new Dictionary<Guid, BlameTestObject>();
        var testcase = new TestCase
        {
            ExecutorUri = new Uri("test:/abc"),
            FullyQualifiedName = "TestProject.UnitTest.TestMethod",
            Source = "abc.dll"
        };
        _blameTestObject = new BlameTestObject(testcase);
    }

    /// <summary>
    /// The write test sequence should throw exception if file path is null.
    /// </summary>
    [TestMethod]
    public void WriteTestSequenceShouldThrowExceptionIfFilePathIsNull()
    {
        _testCaseList.Add(_blameTestObject.Id);
        _testObjectDictionary.Add(_blameTestObject.Id, _blameTestObject);

        Assert.ThrowsException<ArgumentNullException>(() => _xmlReaderWriter.WriteTestSequence(_testCaseList, _testObjectDictionary, null!));
    }

    /// <summary>
    /// The write test sequence should throw exception if file path is empty.
    /// </summary>
    [TestMethod]
    public void WriteTestSequenceShouldThrowExceptionIfFilePathIsEmpty()
    {
        _testCaseList.Add(_blameTestObject.Id);
        _testObjectDictionary.Add(_blameTestObject.Id, _blameTestObject);

        Assert.ThrowsException<ArgumentNullException>(() => _xmlReaderWriter.WriteTestSequence(_testCaseList, _testObjectDictionary, string.Empty));
    }

    /// <summary>
    /// The read test sequence should throw exception if file path is null.
    /// </summary>
    [TestMethod]
    public void ReadTestSequenceShouldThrowExceptionIfFilePathIsNull()
    {
        Assert.ThrowsException<ArgumentNullException>(() => _xmlReaderWriter.ReadTestSequence(null!));
    }

    /// <summary>
    /// The read test sequence should throw exception if file not found.
    /// </summary>
    [TestMethod]
    public void ReadTestSequenceShouldThrowExceptionIfFileNotFound()
    {
        _mockFileHelper.Setup(m => m.Exists(It.IsAny<string>())).Returns(false);

        Assert.ThrowsException<FileNotFoundException>(() => _xmlReaderWriter.ReadTestSequence(string.Empty));
    }

    /// <summary>
    /// The write test sequence should write file stream.
    /// </summary>
    [TestMethod]
    public void WriteTestSequenceShouldWriteFileStream()
    {
        // Setup
        _mockFileHelper.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);
        using var stream = new MemoryStream();
        _mockFileHelper.Setup(m => m.GetStream("path.xml", FileMode.Create, FileAccess.ReadWrite)).Returns(stream);
        _mockStream.Setup(x => x.CanWrite).Returns(true);
        _mockStream.Setup(x => x.Write(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<int>()));

        _xmlReaderWriter.WriteTestSequence(_testCaseList, _testObjectDictionary, "path");

        // Verify Call to fileHelper
        _mockFileHelper.Verify(x => x.GetStream("path.xml", FileMode.Create, FileAccess.ReadWrite));

        // Assert it has some data
        var data = Encoding.UTF8.GetString(stream.ToArray());
        Assert.IsTrue(data.Length > 0, "Stream should have some data.");
    }

    /// <summary>
    /// Verify Write and Read test sequence to check file contents if test completed is false.
    /// </summary>
    [TestMethod]
    public void WriteTestSequenceShouldWriteCorrectFileContentsIfTestCompletedIsFalse()
    {
        var xmlReaderWriter = new XmlReaderWriter();
        var testObject = new BlameTestObject(new TestCase("Abc.UnitTest1", new Uri("test:/abc"), "Abc.dll"));
        testObject.DisplayName = "UnitTest1";
        var testSequence = new List<Guid>
        {
            testObject.Id
        };
        var testObjectDictionary = new Dictionary<Guid, BlameTestObject>
        {
            { testObject.Id, testObject }
        };

        var filePath = xmlReaderWriter.WriteTestSequence(testSequence, testObjectDictionary, Path.GetTempPath());
        var testCaseList = xmlReaderWriter.ReadTestSequence(filePath);
        File.Delete(filePath);

        Assert.AreEqual("Abc.UnitTest1", testCaseList.First().FullyQualifiedName);
        Assert.AreEqual("UnitTest1", testCaseList.First().DisplayName);
        Assert.AreEqual("Abc.dll", testCaseList.First().Source);
        Assert.IsFalse(testCaseList.First().IsCompleted);
    }

    /// <summary>
    /// Verify Write and Read test sequence to check file contents if test completed is true.
    /// </summary>
    [TestMethod]
    public void WriteTestSequenceShouldWriteCorrectFileContentsIfTestCompletedIsTrue()
    {
        var xmlReaderWriter = new XmlReaderWriter();
        var testObject = new BlameTestObject(new TestCase("Abc.UnitTest1", new Uri("test:/abc"), "Abc.dll"));
        testObject.DisplayName = "UnitTest1";
        var testSequence = new List<Guid>
        {
            testObject.Id
        };
        var testObjectDictionary = new Dictionary<Guid, BlameTestObject>
        {
            { testObject.Id, testObject }
        };

        testObjectDictionary[testObject.Id].IsCompleted = true;
        var filePath = xmlReaderWriter.WriteTestSequence(testSequence, testObjectDictionary, Path.GetTempPath());
        var testCaseList = xmlReaderWriter.ReadTestSequence(filePath);
        File.Delete(filePath);

        Assert.AreEqual("Abc.UnitTest1", testCaseList.First().FullyQualifiedName);
        Assert.AreEqual("UnitTest1", testCaseList.First().DisplayName);
        Assert.AreEqual("Abc.dll", testCaseList.First().Source);
        Assert.IsTrue(testCaseList.First().IsCompleted);
    }

    /// <summary>
    /// The testable xml reader writer.
    /// </summary>
    internal class TestableXmlReaderWriter : XmlReaderWriter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestableXmlReaderWriter"/> class.
        /// </summary>
        /// <param name="fileHelper">
        /// The file helper.
        /// </param>
        internal TestableXmlReaderWriter(IFileHelper fileHelper)
            : base(fileHelper)
        {
        }
    }
}
