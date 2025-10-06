namespace Microsoft.ApplicationInsights.Web
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Web;

    using Microsoft.ApplicationInsights.Common;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Web.Helpers;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class SyntheticUserAgentTelemetryInitializerTest
    {
        private string botSubstrings = "search|spider|crawl|Bot|Monitor|AlwaysOn";

        [TestCleanup]
        public void Cleanup()
        {
            while (Activity.Current != null)
            {
                Activity.Current.Stop();
            }
        }

        [TestMethod]
        public void SyntheticSourceIsNotSetIfUserProvidedValue()
        {
            var eventTelemetry = new EventTelemetry("name");
            eventTelemetry.Context.Operation.SyntheticSource = "SOURCE";
            var source = new TestableSyntheticUserAgentTelemetryInitializer(new Dictionary<string, string>
                {
                    { "User-Agent", "YandexBot" }
                });

            source.Filters = this.botSubstrings;

            source.Initialize(eventTelemetry);

            Assert.AreEqual("SOURCE", eventTelemetry.Context.Operation.SyntheticSource);
        }

        [TestMethod]
        public void SyntheticSourceIsNotSetIfNoMatch()
        {
            var eventTelemetry = new EventTelemetry("name");
            var source = new TestableSyntheticUserAgentTelemetryInitializer(new Dictionary<string, string>
                {
                    { "User-Agent", "Yan23232dexBooot" }
                });

            source.Filters = this.botSubstrings;

            source.Initialize(eventTelemetry);

            Assert.IsNull(eventTelemetry.Context.Operation.SyntheticSource);
        }

        [TestMethod]
        public void SyntheticSourceIsNotSetIfContextIsNull()
        {
            var eventTelemetry = new EventTelemetry("name");
            var source = new SyntheticUserAgentTelemetryInitializer();

            source.Filters = this.botSubstrings;

            source.Initialize(eventTelemetry);

            Assert.IsNull(eventTelemetry.Context.Operation.SyntheticSource);
        }

        [TestMethod]
        public void SyntheticSourceIsSetForAllBots()
        {
            this.AssertSyntheticSourceIsSet("YottaaMonitor 123");
            this.AssertSyntheticSourceIsSet("HttpMonitor 123");
            this.AssertSyntheticSourceIsSet("YandexBot 123");
            this.AssertSyntheticSourceIsSet("ThumbShotsBot 123");
            this.AssertSyntheticSourceIsSet("Catchpoint bot 123");
            this.AssertSyntheticSourceIsSet("Willow Internet Crawler 123");
            this.AssertSyntheticSourceIsSet("AlwaysOn 123");
            this.AssertSyntheticSourceIsSet("bingbot 123");
            this.AssertSyntheticSourceIsSet("DBot 123");
            this.AssertSyntheticSourceIsSet("CCBot 123");
            this.AssertSyntheticSourceIsSet("crawl 123");
            this.AssertSyntheticSourceIsSet("spider 123");
            this.AssertSyntheticSourceIsSet("msnbot 123");
            this.AssertSyntheticSourceIsSet("msrbot 123");
            this.AssertSyntheticSourceIsSet("crawler 123");
            this.AssertSyntheticSourceIsSet("bingbot 123");
            this.AssertSyntheticSourceIsSet("openbot 123");
            this.AssertSyntheticSourceIsSet("gigabot 123");
            this.AssertSyntheticSourceIsSet("furlbot 123");
            this.AssertSyntheticSourceIsSet("polybot 123");
            this.AssertSyntheticSourceIsSet("EtaoSpider 123");
            this.AssertSyntheticSourceIsSet("PaperLiBot 123");
            this.AssertSyntheticSourceIsSet("SputnikBot 123");
            this.AssertSyntheticSourceIsSet("baiduspider 123");
            this.AssertSyntheticSourceIsSet("YisouSpider 123");
            this.AssertSyntheticSourceIsSet("ICC-Crawler 123");
            this.AssertSyntheticSourceIsSet("converacrawler 123");
            this.AssertSyntheticSourceIsSet("Sogou Pic Spider 123");
            this.AssertSyntheticSourceIsSet("Innovazion Crawler 123");
        }

        [TestMethod]
        public void SyntheticSourceIsSetForMultipleItemsFromSameContext()
        {
            string userAgent = "YottaaMonitor 123";

            var eventTelemetry1 = new EventTelemetry("name1");
            var eventTelemetry2 = new EventTelemetry("name2");
            var source = new TestableSyntheticUserAgentTelemetryInitializer(new Dictionary<string, string>
                {
                    { "User-Agent", userAgent }
                });
            source.Filters = this.botSubstrings;            
            source.Initialize(eventTelemetry1);
            source.Initialize(eventTelemetry2);
            Assert.AreEqual("Bot", eventTelemetry2.Context.Operation.SyntheticSource, "Incorrect result for " + userAgent);
            Assert.AreEqual("Bot", eventTelemetry2.Context.Operation.SyntheticSource, "Incorrect result for " + userAgent);
        }

        private void AssertSyntheticSourceIsSet(string userAgent)
        {
            var eventTelemetry = new EventTelemetry("name");
            var source = new TestableSyntheticUserAgentTelemetryInitializer(new Dictionary<string, string>
                {
                    { "User-Agent", userAgent }
                });

            source.Filters = this.botSubstrings;

            source.Initialize(eventTelemetry);

            Assert.AreEqual("Bot", eventTelemetry.Context.Operation.SyntheticSource, "Incorrect result for " + userAgent);
        }

        private class TestableSyntheticUserAgentTelemetryInitializer : SyntheticUserAgentTelemetryInitializer
        {
            private readonly HttpContext fakeContext;

            public TestableSyntheticUserAgentTelemetryInitializer(IDictionary<string, string> headers = null)
            {
                this.fakeContext = HttpModuleHelper.GetFakeHttpContext(headers);
            }

            protected override HttpContext ResolvePlatformContext()
            {
                return this.fakeContext;
            }
        }
    }
}
