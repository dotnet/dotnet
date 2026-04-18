namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Tests.Helpers
{
#if NET452
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Threading;
    using System.Web;
    using System.Web.Hosting;
#endif

    public static class HttpContextHelper
    {
#if NET452
        /// <summary>
        /// Sets the static HttpContext.Current for use in unit tests.
        /// Request URL is set specifically to evaluate httpContext.Request.Url during tests.
        /// </summary>
        public static HttpContext SetFakeHttpContext(IDictionary<string, string> headers = null, Func<string> remoteAddr = null)
        {
            string urlPath = "/SeLog.svc/EventData";
            string urlQueryString = "eventDetail=2";

            Thread.GetDomain().SetData(".appPath", string.Empty);
            Thread.GetDomain().SetData(".appVPath", string.Empty);

            var workerRequest = new SimpleWorkerRequestWithHeaders(urlPath, urlQueryString, new StringWriter(CultureInfo.InvariantCulture), headers, remoteAddr);

            var context = new HttpContext(workerRequest);
            HttpContext.Current = context;

            return context;
        }

        private class SimpleWorkerRequestWithHeaders : SimpleWorkerRequest
        {
            private readonly IDictionary<string, string> headers;

            private readonly Func<string> getRemoteAddress;

            public SimpleWorkerRequestWithHeaders(string page, string query, TextWriter output, IDictionary<string, string> headers, Func<string> getRemoteAddress = null)
                : base(page, query, output)
            {
                if (headers != null)
                {
                    this.headers = headers;
                }
                else
                {
                    this.headers = new Dictionary<string, string>();
                }

                this.getRemoteAddress = getRemoteAddress;
            }

            public override string[][] GetUnknownRequestHeaders()
            {
                List<string[]> result = new List<string[]>();

                foreach (var header in this.headers)
                {
                    result.Add(new string[] { header.Key, header.Value });
                }

                var baseResult = base.GetUnknownRequestHeaders();
                if (baseResult != null)
                {
                    result.AddRange(baseResult);
                }

                return result.ToArray();
            }

            public override string GetUnknownRequestHeader(string name)
            {
                if (this.headers.ContainsKey(name))
                {
                    return this.headers[name];
                }

                return base.GetUnknownRequestHeader(name);
            }

            public override string GetKnownRequestHeader(int index)
            {
                var name = HttpWorkerRequest.GetKnownRequestHeaderName(index);

                if (this.headers.ContainsKey(name))
                {
                    return this.headers[name];
                }

                return base.GetKnownRequestHeader(index);
            }

            public override string GetRemoteAddress()
            {
                if (this.getRemoteAddress != null)
                {
                    return this.getRemoteAddress();
                }

                return base.GetRemoteAddress();
            }
        }
#endif
    }
}
