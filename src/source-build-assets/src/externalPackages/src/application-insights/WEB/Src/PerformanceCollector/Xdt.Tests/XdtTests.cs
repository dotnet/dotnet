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
        public void XdtTest()
        {
            this.ValidateTransform(
                ".ApplicationInsights.config.install.xdt",
                ".ApplicationInsights.config.uninstall.xdt",
                ".TestDataSet.xml");
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
                    .Single(name => name.EndsWith(installTransformationName, StringComparison.OrdinalIgnoreCase));

            var uninstallXdtName =
                Assembly.GetExecutingAssembly()
                    .GetManifestResourceNames()
                    .Single(name => name.EndsWith(uninstallTransformationName, StringComparison.OrdinalIgnoreCase));

            var dataSetName =
                Assembly.GetExecutingAssembly()
                    .GetManifestResourceNames()
                    .Single(name => name.EndsWith(testDataSetName, StringComparison.OrdinalIgnoreCase));

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
                        Trace.WriteLine("Item #" + (i++).ToString(CultureInfo.InvariantCulture));

                        var original = GetInnerXml(item.XPathSelectElement("./original"));
                        var expectedPostTransform = GetInnerXml(item.XPathSelectElement("./expectedPostTransform"));
                        var expectedPostUninstallElement = item.XPathSelectElement("./expectedPostUninstall");
                        var expectedPostUninstall = expectedPostUninstallElement != null ? GetInnerXml(expectedPostUninstallElement) : original;

                        var targetDocument = new XmlDocument();
                        targetDocument.LoadXml(original);
                        
                        bool success = installTransformation.Apply(targetDocument);
                        Assert.IsTrue(
                            success,
                            "Transformation (install) has failed. XDT: {0}, XML: {1}",
                            installXdtName,
                            item);

                        // validate the transformation result
                        Assert.IsTrue(
                            string.Equals(
                                expectedPostTransform,
                                targetDocument.OuterXml.Replace("\r", null),
                                StringComparison.Ordinal),
                            "Unexpected transform (install) result. Expected:{0}{0}{1}{0}{0} Actual:{0}{2}{0}{0}",
                            Environment.NewLine,
                            expectedPostTransform,
                            targetDocument.OuterXml);

                        var transformedDocument = targetDocument.OuterXml;

                        // apply uninstall transformation
                        success = uninstallTransformation.Apply(targetDocument);
                        Assert.IsTrue(
                            success,
                            "Transformation (uninstall) has failed. XDT: {0}, XML: {1}",
                            uninstallXdtName,
                            transformedDocument);

                        // validate the transformation result
                        Assert.IsTrue(
                            string.Equals(
                                expectedPostUninstall,
                                targetDocument.OuterXml.Replace("\r", null),
                                StringComparison.Ordinal),
                            "Unexpected transform (uninstall) result. Expected:{0}{1}{0}{0} Actual:{0}{2}",
                            Environment.NewLine,
                            expectedPostUninstall,
                            targetDocument.OuterXml);
                    }
                }
            }
        }
    }
}
