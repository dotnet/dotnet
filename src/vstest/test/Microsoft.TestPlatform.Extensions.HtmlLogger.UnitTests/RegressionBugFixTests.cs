// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

using Microsoft.VisualStudio.TestPlatform.Extensions.HtmlLogger;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using HtmlLoggerImpl = Microsoft.VisualStudio.TestPlatform.Extensions.HtmlLogger.HtmlLogger;
using IHtmlTransformer = Microsoft.VisualStudio.TestPlatform.Extensions.HtmlLogger.IHtmlTransformer;

namespace Microsoft.TestPlatform.Extensions.HtmlLogger.UnitTests;

/// <summary>
/// Regression test for GH-2483:
/// HtmlLogger.Initialize must create the test results directory if it doesn't exist.
/// Previously, Initialize would not create the directory, causing later file
/// operations to fail with DirectoryNotFoundException.
/// </summary>
[TestClass]
public class RegressionBugFixTests
{
    [TestMethod]
    public void Initialize_NonExistentDirectory_MustCreateIt()
    {
        // GH-2483: The fix added Directory.CreateDirectory(testResultsDirPath) in Initialize.
        // If the fix were reverted, the directory would not exist after Initialize.
        var mockFileHelper = new Mock<IFileHelper>();
        var mockHtmlTransformer = new Mock<IHtmlTransformer>();
        var mockXmlSerializer = new Mock<XmlObjectSerializer>();
        var htmlLogger = new HtmlLoggerImpl(mockFileHelper.Object, mockHtmlTransformer.Object, mockXmlSerializer.Object);

        var events = new Mock<TestLoggerEvents>();
        var nonExistentDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));

        try
        {
            // Act
            htmlLogger.Initialize(events.Object, nonExistentDir);

            // Assert: directory must have been created
            Assert.IsTrue(Directory.Exists(nonExistentDir),
                "GH-2483: Initialize must call Directory.CreateDirectory for the test results directory.");
            Assert.AreEqual(nonExistentDir, htmlLogger.TestResultsDirPath);
        }
        finally
        {
            if (Directory.Exists(nonExistentDir))
            {
                Directory.Delete(nonExistentDir, recursive: true);
            }
        }
    }

    [TestMethod]
    public void Initialize_ExistingDirectory_MustNotThrow()
    {
        // Complementary test: Initialize with an existing directory must succeed.
        var mockFileHelper = new Mock<IFileHelper>();
        var mockHtmlTransformer = new Mock<IHtmlTransformer>();
        var mockXmlSerializer = new Mock<XmlObjectSerializer>();
        var htmlLogger = new HtmlLoggerImpl(mockFileHelper.Object, mockHtmlTransformer.Object, mockXmlSerializer.Object);

        var events = new Mock<TestLoggerEvents>();
        var existingDir = Path.GetTempPath();

        htmlLogger.Initialize(events.Object, existingDir);

        Assert.AreEqual(existingDir, htmlLogger.TestResultsDirPath);
    }

    /// <summary>
    /// Regression test for GH-3136 / PR #4576:
    /// HtmlTransformer.Transform must not throw XmlException when the XML input contains
    /// invalid character references like &amp;#xFFFF;. Before the fix, the default
    /// XmlReaderSettings had CheckCharacters=true, causing XmlException on such input.
    /// The fix changed Transform to use XmlReader.Create with CheckCharacters=false.
    /// </summary>
    [TestMethod]
    public void Transform_XmlWithInvalidCharacterReferences_MustNotThrowXmlException()
    {
        // GH-3136: DCS can serialize characters into character references that are not
        // strictly valid XML (e.g. &#xFFFF;). The fix ensures the XmlReader tolerates them.
        var transformer = new HtmlTransformer();
        var xmlFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N") + ".xml");
        var htmlFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N") + ".html");

        try
        {
            // Write XML containing an invalid character reference that would cause
            // XmlException with CheckCharacters=true (the pre-fix default).
            File.WriteAllText(xmlFile,
                "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n" +
                "<TestRun><Results><Output>test&#xFFFF;value</Output></Results></TestRun>");

            // Act: must not throw XmlException. Before the fix, this line threw:
            // "System.Xml.XmlException: '', hexadecimal value 0xFFFF, is an invalid character."
            transformer.Transform(xmlFile, htmlFile);

            // Assert: output file must exist and be non-empty.
            Assert.IsTrue(File.Exists(htmlFile),
                "GH-3136: Transform must produce an output HTML file.");
            Assert.IsGreaterThan(0, new FileInfo(htmlFile).Length,
                "GH-3136: Output HTML file must be non-empty.");
        }
        finally
        {
            if (File.Exists(xmlFile))
            {
                File.Delete(xmlFile);
            }

            if (File.Exists(htmlFile))
            {
                File.Delete(htmlFile);
            }
        }
    }
}
