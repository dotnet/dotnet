// -----------------------------------------------------------------------
// <copyright file="XdtTests.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. 
// All rights reserved.  2014
// </copyright>
// -----------------------------------------------------------------------

namespace Xdt.Tests
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Xml;
    using System.Xml.Linq;
    using System.Xml.XPath;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.Web.XmlTransform;

    [TestClass]
    public class XdtTests
    {
        [TestMethod]
        [TestCategory("XdtTests")]
        public void XdtTraceListenerAppTest()
        {
            this.ValidateTransform(
                ".TraceListener.app.config.install.xdt",
                ".TraceListener.app.config.uninstall.xdt",
                ".TraceListener.TestDataSet.xml");
        }

        [TestMethod]
        [TestCategory("XdtTests")]
        public void XdtTraceListenerWebTest()
        {
            this.ValidateTransform(
                ".TraceListener.web.config.install.xdt",
                ".TraceListener.web.config.uninstall.xdt",
                ".TraceListener.TestDataSet.xml");
        }

        [TestMethod]
        [TestCategory("XdtTests")]
        public void XdtNlogAppTest()
        {
            this.ValidateTransform(
                ".NLog.app.config.install.xdt",
                ".NLog.app.config.uninstall.xdt",
                ".NLog.TestDataSet.xml");
        }

        [TestMethod]
        [TestCategory("XdtTests")]
        public void XdtNlogWebTest()
        {
            this.ValidateTransform(
                ".NLog.web.config.install.xdt",
                ".NLog.web.config.uninstall.xdt",
                ".NLog.TestDataSet.xml");
        }

        [TestMethod]
        [TestCategory("XdtTests")]
        public void XdtLog4NetWebTest()
        {
            this.ValidateTransform(
               ".Log4Net.web.config.install.xdt",
               ".Log4Net.web.config.uninstall.xdt",
               ".Log4Net.TestDataSet.xml");
        }

        [TestMethod]
        [TestCategory("XdtTests")]
        public void XdtLog4NetAppTest()
        {
            this.ValidateTransform(
               ".Log4Net.app.config.install.xdt",
               ".Log4Net.app.config.uninstall.xdt",
               ".Log4Net.TestDataSet.xml");
        }

        [TestMethod]
        [TestCategory("XdtTests")]
        public void DiagnosticSourceListenerTest()
        {
            this.ValidateTransform(
               ".DiagnosticSourceListener.ApplicationInsights.config.install.xdt",
               ".DiagnosticSourceListener.ApplicationInsights.config.uninstall.xdt",
               ".DiagnosticSourceListener.TestDataSet.xml");
        }

        [TestMethod]
        [TestCategory("XdtTests")]
        public void EventSourceListenerTest()
        {
            this.ValidateTransform(
               ".EventSourceListener.ApplicationInsights.config.install.xdt",
               ".EventSourceListener.ApplicationInsights.config.uninstall.xdt",
               ".EventSourceListener.TestDataSet.xml");
        }

        [TestMethod]
        [TestCategory("XdtTests")]
        public void EtwCollectorTest()
        {
            this.ValidateTransform(
               ".EtwCollector.ApplicationInsights.config.install.xdt",
               ".EtwCollector.ApplicationInsights.config.uninstall.xdt",
               ".EtwCollector.TestDataSet.xml");
        }

        private static string GetInnerXml(XElement element)
        {
            XmlReader reader = element.CreateReader();
            reader.MoveToContent();
            return reader.ReadInnerXml();
        }

        private void ValidateTransform(string installTransformationName, string uninstallTransformationName, string testDataSetName)
        {
            // load all relevant XDTs and the test data set XML
            var installXdtName =
                Assembly.GetExecutingAssembly()
                    .GetManifestResourceNames()
                    .Single(name => name.EndsWith(installTransformationName));

            var uninstallXdtName =
                Assembly.GetExecutingAssembly()
                    .GetManifestResourceNames()
                    .Single(name => name.EndsWith(uninstallTransformationName));

            var dataSetName =
                Assembly.GetExecutingAssembly()
                    .GetManifestResourceNames()
                    .Single(name => name.EndsWith(testDataSetName));

            var installXdtStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(installXdtName);
            var uninstallXdtStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(uninstallXdtName);

            var dataSetStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(dataSetName);
            var dataSetXml = XElement.Load(dataSetStream);

            using (var installXdtStreamReader = new StreamReader(installXdtStream))
            {
                using (var uninstallXdtStreamReader = new StreamReader(uninstallXdtStream))
                {
                    var installTransformation = new XmlTransformation(installXdtStreamReader.ReadToEnd(), false, null);
                    var uninstallTransformation = new XmlTransformation(uninstallXdtStreamReader.ReadToEnd(), false, null);

                    int i = 0;
                    foreach (var item in dataSetXml.XPathSelectElements("./item"))
                    {
                        Trace.WriteLine("Item #" + i++);

                        var original = GetInnerXml(item.XPathSelectElement("./original"));
                        var expectedPostTransform = GetInnerXml(item.XPathSelectElement("./expectedPostTransform"));

                        var targetDocument = new XmlDocument();
                        targetDocument.LoadXml(original);

                        bool success = installTransformation.Apply(targetDocument);
                        Assert.IsTrue(success, "Transformation (install) has failed. XDT: {0}, XML: {1}", installXdtName, item);

                        // validate the transformation result
                        Assert.IsTrue(
                            string.Equals(
                                expectedPostTransform,
                                targetDocument.OuterXml,
                                StringComparison.InvariantCulture),
                            "Unexpected transform (install) result. Expected:{0}{0}{1}{0}{0} Actual:{0}{2}{0}{0}",
                            Environment.NewLine,
                            expectedPostTransform,
                            targetDocument.OuterXml);

                        var transformedDocument = targetDocument.OuterXml;

                        // apply uninstall transformation
                        success = uninstallTransformation.Apply(targetDocument);
                        Assert.IsTrue(success, "Transformation (uninstall) has failed. XDT: {0}, XML: {1}", uninstallXdtName, transformedDocument);

                        // validate the transformation result
                        Assert.IsTrue(
                            string.Equals(
                                original,
                                targetDocument.OuterXml,
                                StringComparison.InvariantCulture),
                            "Unexpected transform (uninstall) result. Expected:{0}{1}{0}{0} Actual:{0}{2}",
                            Environment.NewLine,
                            original,
                            targetDocument.OuterXml);
                    }
                }
            }
        }
    }
}
