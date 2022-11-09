namespace Microsoft.ApplicationInsights
{
    using System.Diagnostics;
    using System.IO;
    
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.DataContracts;

    [TestClass]
    public class AppInsightsStandaloneTests
    {
        private string tempPath;

        [TestInitialize]
        public void Initialize()
        {
            this.tempPath = Path.Combine(Path.GetTempPath(), "ApplicationInsightsTests", Guid.NewGuid().ToString());
            Directory.CreateDirectory(this.tempPath);
            File.Copy("Microsoft.ApplicationInsights.dll", $"{tempPath}\\Microsoft.ApplicationInsights.dll", true);
            File.Delete($"{tempPath}\\System.Diagnostics.DiagnosticSource.dll");
        }

        [TestCleanup]
        public void Cleanup()
        {
            Directory.Delete(this.tempPath, true);
        }

#if NETCOREAPP3_1
        [Ignore("Missing netstandard dependencies.")]
#endif
        [TestMethod]
        public void AppInsightsDllCouldRunStandalone()
        {
            // This tests if ApplicationInsights.dll can work standalone without System.DiagnosticSource (for uses like in a powershell script)
            // Its hard to mock this with plain unit tests, so we spin up a dummy application, copy just ApplicationInsights.dll
            // and run it.
            var dependencyId = RunTestApplication("guid");
            Assert.IsFalse(dependencyId.Contains("guid"));
        }

        [TestMethod]
        public void AppInsightsUsesActivityWhenDiagnosticSourceIsAvailableNonW3C()
        {
            try
            {
                // Regular use case - System.DiagnosticSource is available. Regular unit test can cover this scenario.
                var config = new TelemetryConfiguration();
                DisableW3CFormatInActivity();
                config.TelemetryInitializers.Add(new OperationCorrelationTelemetryInitializer());
                var tc = new TelemetryClient(config);
                using (var requestOperation = tc.StartOperation<RequestTelemetry>("request", "guid"))
                {
                    using (var dependencyOperation = tc.StartOperation<DependencyTelemetry>("dependency", "guid"))
                    {
                        Assert.IsTrue(dependencyOperation.Telemetry.Id.StartsWith("|guid."));
                        tc.TrackTrace("Hello World!");
                    }
                }
            }
            finally
            {
                EnableW3CFormatInActivity();
            }
        }       

        [TestMethod]
        public void AppInsightsUsesActivityWhenDiagnosticSourceIsAvailableW3C()
        {
            // Regular use case - System.DiagnosticSource is available. Regular unit test can cover this scenario.
            var config = new TelemetryConfiguration();
            config.TelemetryInitializers.Add(new OperationCorrelationTelemetryInitializer());
            var tc = new TelemetryClient(config);
            using (var requestOperation = tc.StartOperation<RequestTelemetry>("request", "guid"))
            {
                using (var dependencyOperation = tc.StartOperation<DependencyTelemetry>("dependency", "guid"))
                {
                    // "guid" is not w3c compatible. Ignored
                    Assert.IsFalse(dependencyOperation.Telemetry.Id.StartsWith("|guid."));
                    // but "guid" will be stored in custom properties
                    Assert.AreEqual("guid",dependencyOperation.Telemetry.Properties["ai_legacyRootId"]);
                    tc.TrackTrace("Hello World!");
                }
            }
        }

        private string RunTestApplication(string operationId)
        {
            var fileName = $"{this.tempPath}\\ActivityTest.exe";

            Assert.IsTrue(CreateTestApplication(fileName), "Failed to create a test application. See console output for details.");

            Process p = new Process
            {
                StartInfo =
                {
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    FileName = fileName,
                    Arguments = operationId
                }
            };
            
            p.Start();

            Assert.IsTrue(p.WaitForExit(10000));
            Trace.WriteLine(p.StandardOutput.ReadToEnd());
            Trace.WriteLine(p.StandardError.ReadToEnd());
            Assert.AreEqual(0, p.ExitCode);
           

            return p.StandardOutput.ReadToEnd();
        }

        private static void DisableW3CFormatInActivity()
        {
            Activity.DefaultIdFormat = ActivityIdFormat.Hierarchical;
            Activity.ForceDefaultIdFormat = true;
        }

        private static void EnableW3CFormatInActivity()
        {
            Activity.DefaultIdFormat = ActivityIdFormat.W3C;
            Activity.ForceDefaultIdFormat = true;
        }

        private static bool CreateTestApplication(string fileName)
        {
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(@"
                using System;
                using Microsoft.ApplicationInsights;
                using Microsoft.ApplicationInsights.DataContracts;
                using Microsoft.ApplicationInsights.Extensibility;

                class ActivityTest
                {
                    static void Main(string[] args)
                    {
                        var config = new TelemetryConfiguration();
                        config.TelemetryInitializers.Add(new OperationCorrelationTelemetryInitializer());
                        var tc = new TelemetryClient(config);
                        using (var requestOperation = tc.StartOperation<RequestTelemetry>(""request"", args[0]))
                        {
                            using (var dependencyOperation = tc.StartOperation<DependencyTelemetry>(""dependency"", args[0]))
                            {
                                Console.Write(dependencyOperation.Telemetry.Id);
                                tc.TrackTrace(""Hello World!"");
                            }
                        }
                    }
                }");

            MetadataReference[] references = new[]
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(TelemetryClient).Assembly.Location)
            };

            CSharpCompilation compilation = CSharpCompilation.Create(
                "ActivityTest",
                syntaxTrees: new[] { syntaxTree },
                references: references,
                options: new CSharpCompilationOptions(OutputKind.ConsoleApplication));

            var emitResult = compilation.Emit(fileName);
            if (emitResult.Success)
            {
                return true;
            }
            else
            {
                foreach(var d in emitResult.Diagnostics)
                {
                    Console.WriteLine(d.ToString());
                }

                return false;
            }
        }
    }
}