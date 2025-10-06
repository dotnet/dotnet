namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing.SelfDiagnostics
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class SelfDiagnosticsConfigParserTest
    {
        [TestMethod]
        public void SelfDiagnosticsConfigParser_TryParseFilePath_Success()
        {
            string configJson = "{ \t \n "
                                + "\t    \"LogDirectory\" \t : \"Diagnostics\", \n"
                                + "FileSize \t : \t \n"
                                + " 1024 \n}\n";
            Assert.IsTrue(SelfDiagnosticsConfigParser.TryParseLogDirectory(configJson, out string logDirectory));
            Assert.AreEqual("Diagnostics", logDirectory);
        }

        [TestMethod]
        public void SelfDiagnosticsConfigParser_TryParseFilePath_MissingField()
        {
            string configJson = @"{
                    ""path"": ""Diagnostics"",
                    ""FileSize"": 1024
                    }";
            Assert.IsFalse(SelfDiagnosticsConfigParser.TryParseLogDirectory(configJson, out string logDirectory));
        }

        [TestMethod]
        public void SelfDiagnosticsConfigParser_TryParseFileSize()
        {
            string configJson = @"{
                    ""LogDirectory"": ""Diagnostics"",
                    ""FileSize"": 1024
                    }";
            Assert.IsTrue(SelfDiagnosticsConfigParser.TryParseFileSize(configJson, out int fileSize));
            Assert.AreEqual(1024, fileSize);
        }

        [TestMethod]
        public void SelfDiagnosticsConfigParser_TryParseFileSize_CaseInsensitive()
        {
            string configJson = @"{
                    ""LogDirectory"": ""Diagnostics"",
                    ""fileSize"" :
                                   2048
                    }";
            Assert.IsTrue(SelfDiagnosticsConfigParser.TryParseFileSize(configJson, out int fileSize));
            Assert.AreEqual(2048, fileSize);
        }

        [TestMethod]
        public void SelfDiagnosticsConfigParser_TryParseFileSize_MissingField()
        {
            string configJson = @"{
                    ""LogDirectory"": ""Diagnostics"",
                    ""size"": 1024
                    }";
            Assert.IsFalse(SelfDiagnosticsConfigParser.TryParseFileSize(configJson, out int fileSize));
        }

        [TestMethod]
        public void SelfDiagnosticsConfigParser_TryParseLogLevel()
        {
            string configJson = @"{
                    ""LogDirectory"": ""Diagnostics"",
                    ""FileSize"": 1024,
                    ""LogLevel"": ""Error""
                    }";
            Assert.IsTrue(SelfDiagnosticsConfigParser.TryParseLogLevel(configJson, out string logLevelString));
            Assert.AreEqual("Error", logLevelString);
        }
    }
}
