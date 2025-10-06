namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing
{
    using System;
    using System.Globalization;
    using System.Threading;
    using Microsoft.ApplicationInsights.TestFramework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    

    public class ExtensionsTest
    {
        [TestClass]
        public class ToInvariantString
        {
            private CultureInfo originalUICulture = Thread.CurrentThread.CurrentUICulture;

            [TestCleanup]
            public void Cleanup()
            {
                Thread.CurrentThread.CurrentUICulture = this.originalUICulture;
            }

            [TestMethod]
            public void ExtractsStackTraceWithInvariantCultureToHelpOurTelemetryToolsMatchSimilarErrorsReportedByOSsWithDifferentLanguages()
            {
                CultureInfo stackTraceCulture = null;
                var exception = new StubException();
                exception.OnToString = () =>
                {
                    stackTraceCulture = Thread.CurrentThread.CurrentUICulture;
                    return string.Empty;
                };

                Extensions.ToInvariantString(exception);

                Assert.AreSame(CultureInfo.InvariantCulture, stackTraceCulture);
            }

            [TestMethod]
            public void RestoresOriginalUICultureToPreserveGlobalStateOfApplication()
            {
                Extensions.ToInvariantString(new Exception());
                Assert.AreSame(this.originalUICulture, Thread.CurrentThread.CurrentUICulture);
            }
        }
    }
}
