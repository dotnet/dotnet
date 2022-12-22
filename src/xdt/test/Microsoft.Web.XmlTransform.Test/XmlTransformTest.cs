using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Xunit;
using System.Reflection;
using System.Xml;
using Microsoft.Web.XmlTransform.Test.Properties;

namespace Microsoft.Web.XmlTransform.Test
{
    public class XmlTransformTest
    {

        [Fact]
        public void XmlTransform_Support_WriteToStream()
        {
            string src = CreateATestFile("Web.config", Resources.Web);
            string transformFile = CreateATestFile("Web.Release.config", Resources.Web_Release);
            string destFile = GetTestFilePath("MyWeb.config");

            //execute
            Microsoft.Web.XmlTransform.XmlTransformableDocument x = new Microsoft.Web.XmlTransform.XmlTransformableDocument();
            x.PreserveWhitespace = true;
            x.Load(src);

            Microsoft.Web.XmlTransform.XmlTransformation transform = new Microsoft.Web.XmlTransform.XmlTransformation(transformFile);

            bool succeed = transform.Apply(x);

            FileStream fsDestFile = new FileStream(destFile, FileMode.OpenOrCreate);
            x.Save(fsDestFile);

            //verify, we have a success transform
            Assert.True(succeed);

            //verify, the stream is not closed
            Assert.True(fsDestFile.CanWrite);

            //sanity verify the content is right, (xml was transformed)
            fsDestFile.Close();
            string content = File.ReadAllText(destFile);
            Assert.DoesNotContain("debug=\"true\"", content);

            List<string> lines = new List<string>(File.ReadLines(destFile));
            //sanity verify the line format is not lost (otherwsie we will have only one long line)
            Assert.True(lines.Count > 10);

            //be nice 
            transform.Dispose();
            x.Dispose();
        }

        [Fact]
        public void XmlTransform_AttibuteFormatting()
        {
            Transform_TestRunner_ExpectSuccess(Resources.AttributeFormating_source,
                    Resources.AttributeFormating_transform,
                    Resources.AttributeFormating_destination,
                    Resources.AttributeFormatting_log);
        }

        [Fact]
        public void XmlTransform_TagFormatting()
        {
            Transform_TestRunner_ExpectSuccess(Resources.TagFormatting_source,
                   Resources.TagFormatting_transform,
                   Resources.TagFormatting_destination,
                   Resources.TagFormatting_log);
        }

        [Fact]
        public void XmlTransform_HandleEdgeCase()
        {
            //2 edge cases we didn't handle well and then fixed it per customer feedback.
            //    a. '>' in the attribute value
            //    b. element with only one character such as <p>
            Transform_TestRunner_ExpectSuccess(Resources.EdgeCase_source,
                    Resources.EdgeCase_transform,
                    Resources.EdgeCase_destination,
                    Resources.EdgeCase_log);
        }

        [Fact]
        public void XmlTransform_ErrorAndWarning()
        {
            Transform_TestRunner_ExpectFail(Resources.WarningsAndErrors_source,
                    Resources.WarningsAndErrors_transform,
                    Resources.WarningsAndErrors_log);
        }

        [Fact]
        public void XmlTransform_Support_CommentOut()
        {
            string src = CreateATestFile("Web.config", Resources.Web);
            string transformFile = CreateATestFile("Web.Test.config", Resources.Web_Test);
            string destFile = GetTestFilePath("MyWeb.config");
            string keyName = "keyAppSettings1";

            //execute
            Microsoft.Web.XmlTransform.XmlTransformableDocument x = new Microsoft.Web.XmlTransform.XmlTransformableDocument();
            x.PreserveWhitespace = true;
            x.Load(src);

            // Verify the XML content has the requested XML node
            var xPathString = $"/configuration/appSettings/add[@key='{keyName}']";
            System.Xml.XmlNodeList nodesFound = x.SelectNodes(xPathString);
            Assert.NotNull(nodesFound);
            Assert.NotEmpty(nodesFound);

            Microsoft.Web.XmlTransform.XmlTransformation transform = new Microsoft.Web.XmlTransform.XmlTransformation(transformFile);

            bool succeed = transform.Apply(x);

            FileStream fsDestFile = new FileStream(destFile, FileMode.OpenOrCreate);
            x.Save(fsDestFile);

            //verify, we have a success transform
            Assert.True(succeed);

            //sanity verify the content is right, (xml was transformed)
            fsDestFile.Close();
            x.Dispose();
            x = new Microsoft.Web.XmlTransform.XmlTransformableDocument();
            x.PreserveWhitespace = true;
            x.Load(destFile);

            // Verify the XML content does not expose the requested (already commented out) XML node
            nodesFound = x.SelectNodes(xPathString);
            Assert.NotNull(nodesFound);
            Assert.Empty(nodesFound);

            List<string> lines = new List<string>(File.ReadLines(destFile));
            //sanity verify the line format is not lost (otherwise we will have only one long line)
            Assert.True(lines.Count > 10);

            //be nice 
            transform.Dispose();
            x.Dispose();
        }

        /// <summary>
        /// This is a result of getting errors from VSTS / Azure Devops pipeline transforms failing.
        /// The error is caused by having RemoveAttributes in a transform when the source doesn't have the attribute.
        /// There is no error when using a transform to change the value of a non existing attribute so it felt wrong.
        /// The pipelines use ConfigTransformationTool ctt.exe (https://ctt.codeplex.com) to do the transforms which refs this lib.
        /// The issue is caused by a typo in XmlTransformationLogger.ConvertUriToFileName.
        /// </summary>
        [Fact]
        public void XmlTransformWithLogger_Expect_Success_RemoveAttributes_When_Attribute_Not_Present()
        {
            //============================================================================ robs ==
            // Needs to have a logger as that's where the exception this is to highlight originates
            //====================================================================== 2019-11-29 ==
            var logger = new TestTransformationLogger();

            //============================================================================ robs ==
            // This HAS to be XmlDocument, if it's XmlTransformableDocument it won't hit the
            // exception path this test is for.
            //====================================================================== 2019-11-30 ==
            var sourceConfig = new System.Xml.XmlDocument();

            //============================================================================ robs ==
            // Load original source web.config - with debug="true"
            //====================================================================== 2019-11-30 ==
            sourceConfig.LoadXml(Resources.Web);

            //============================================================================ robs ==
            // Create two transforms, one to replicate a release build config transform and another
            // to replicate a deploy transform - they are both the same but it doesn't matter as long as
            // the result is that one transform has RemoveAttributes when the source does not have
            // the attribute.
            //====================================================================== 2019-11-30 ==
            using (var transformRelease = new XmlTransformation(Resources.Web_Release, false, logger))
            using (var transformDeploy = new XmlTransformation(Resources.Web_Release, false, logger))
            {
                bool transformReleaseSucceed = transformRelease.Apply(sourceConfig);
                Assert.True(transformReleaseSucceed);

                bool transformDeploySucceed = transformDeploy.Apply(sourceConfig);
                Assert.True(transformDeploySucceed);

                string transformedContent = sourceConfig.OuterXml;
                Assert.DoesNotContain("debug=\"true\"", transformedContent);
            }
        }

        private void Transform_TestRunner_ExpectSuccess(string source, string transform, string baseline, string expectedLog)
        {
            string src = CreateATestFile("source.config", source);
            string transformFile = CreateATestFile("transform.config", transform);
            string baselineFile = CreateATestFile("baseline.config", baseline);
            string destFile = GetTestFilePath("result.config");
            TestTransformationLogger logger = new TestTransformationLogger();

            XmlTransformableDocument x = new XmlTransformableDocument();
            x.PreserveWhitespace = true;
            x.Load(src);

            Microsoft.Web.XmlTransform.XmlTransformation xmlTransform = new Microsoft.Web.XmlTransform.XmlTransformation(transformFile, logger);

            //execute
            bool succeed = xmlTransform.Apply(x);
            x.Save(destFile);
            xmlTransform.Dispose();
            x.Dispose();
            //test
            Assert.True(succeed);
            CompareFiles(destFile, baselineFile);
            CompareMultiLines(expectedLog, logger.LogText);
        }

        private void Transform_TestRunner_ExpectFail(string source, string transform, string expectedLog)
        {
            string src = CreateATestFile("source.config", source);
            string transformFile = CreateATestFile("transform.config", transform);
            string destFile = GetTestFilePath("result.config");
            TestTransformationLogger logger = new TestTransformationLogger();

            XmlTransformableDocument x = new XmlTransformableDocument();
            x.PreserveWhitespace = true;
            x.Load(src);

            Microsoft.Web.XmlTransform.XmlTransformation xmlTransform = new Microsoft.Web.XmlTransform.XmlTransformation(transformFile, logger);

            //execute
            bool succeed = xmlTransform.Apply(x);
            x.Save(destFile);
            xmlTransform.Dispose();
            x.Dispose();
            //test
            Assert.False(succeed);
            CompareMultiLines(expectedLog, logger.LogText);
        }

        private void CompareFiles(string baseLinePath, string resultPath)
        {
            string bsl;
            using (StreamReader sr = new StreamReader(baseLinePath))
            {
                bsl = sr.ReadToEnd();
            }

            string result;
            using (StreamReader sr = new StreamReader(resultPath))
            {
                result = sr.ReadToEnd();
            }

            CompareMultiLines(bsl, result);
        }

        private void CompareMultiLines(string baseline, string result)
        {
            string[] baseLines = baseline.Split(new string[] { System.Environment.NewLine }, StringSplitOptions.None);
            string[] resultLines = result.Split(new string[] { System.Environment.NewLine }, StringSplitOptions.None);

            for (int i = 0; i < baseLines.Length; i++)
            {
                Assert.Equal(baseLines[i], resultLines[i]);
            }
        }

        private string CreateATestFile(string filename, string contents)
        {
            string file = GetTestFilePath(filename);
            File.WriteAllText(file, contents);
            return file;
        }

        private string GetTestFilePath(string filename)
        {
            Uri asm = new Uri(typeof(XmlTransformTest).GetTypeInfo().Assembly.Location, UriKind.Absolute);
            string dir = Path.GetDirectoryName(asm.LocalPath);
            string folder = Path.Combine(dir, "testfiles");
            Directory.CreateDirectory(folder);
            string file = Path.Combine(folder, filename);
            return file;
        }
    }
}
