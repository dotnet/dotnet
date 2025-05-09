namespace Microsoft.ApplicationInsights.Web.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Net;
    using System.Threading;
    using System.Web;
    using System.Web.Hosting;
    using System.Web.Mvc;

    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.Web.Implementation;
    using Moq;
    using VisualStudio.TestTools.UnitTesting;

    internal static class HttpModuleHelper
    {
        public const string UrlHost = "http://test.microsoft.com";
        public const string UrlPath = "/SeLog.svc/EventData";
        public const string UrlQueryString = "eventDetail=2";

        public static PrivateObject CreateTestModule(RequestStatus requestStatus = RequestStatus.Success)
        {
            InitializeTelemetryConfiguration();

            switch (requestStatus)
            {
                case RequestStatus.Success:
                    {
                        HttpContext.Current = GetFakeHttpContext();
                        break;
                    }

                case RequestStatus.RequestFailed:
                    {
                        HttpContext.Current = GetFakeHttpContextForFailedRequest();
                        break;
                    }

                case RequestStatus.ApplicationFailed:
                    {
                        HttpContext.Current = GetFakeHttpContextForFailedApplication();
                        break;
                    }
            }

            PrivateObject moduleWrapper = new PrivateObject(typeof(ApplicationInsightsHttpModule));

            return moduleWrapper;
        }

        public static HttpApplication GetFakeHttpApplication()
        {
            var httpContext = GetFakeHttpContext();
            var httpApplicationWrapper = new PrivateObject(typeof(HttpApplication), null);

            httpApplicationWrapper.SetField("_context", httpContext);

            return (HttpApplication)httpApplicationWrapper.Target;
        }

        public static HttpContext GetFakeHttpContext(IDictionary<string, string> headers = null, Func<string> remoteAddr = null)
        {
            Thread.GetDomain().SetData(".appPath", string.Empty);
            Thread.GetDomain().SetData(".appVPath", string.Empty);

            var workerRequest = new SimpleWorkerRequestWithHeaders(UrlPath, UrlQueryString, new StringWriter(CultureInfo.InvariantCulture), headers, remoteAddr);
            
            var context = new HttpContext(workerRequest);
            HttpContext.Current = context;

            return context;
        }

        public static ControllerContext GetFakeControllerContext(bool isCustomErrorEnabled = false)
        {
            var mock = new Mock<HttpContextWrapper>(GetFakeHttpContext());

            using (var writerResponse = new StringWriter(CultureInfo.InvariantCulture))
            {
                var response = new HttpResponseWrapper(new HttpResponse(writerResponse));
                mock.SetupGet(ctx => ctx.IsCustomErrorEnabled).Returns(isCustomErrorEnabled);
                mock.SetupGet(ctx => ctx.Response).Returns(response);
            }

            var controllerCtx = new ControllerContext
            {
                HttpContext = mock.Object
            };

            controllerCtx.RouteData.Values["controller"] = "controller";
            controllerCtx.RouteData.Values["action"] = "action";
            controllerCtx.Controller = new DefaultController();

            return controllerCtx;
        }

        public static HttpContextBase GetFakeHttpContextBase(IDictionary<string, string> headers = null)
        {
            return new HttpContextWrapper(GetFakeHttpContext(headers));
        }

        public static HttpContext GetFakeHttpContextForFailedRequest()
        {
            var httpContext = GetFakeHttpContext();
            httpContext.Response.StatusCode = 500;
            return httpContext;
        }

        public static HttpContext GetFakeHttpContextForFailedApplication()
        {
            var httpContext = GetFakeHttpContextForFailedRequest();
            httpContext.AddError(new WebException("Exception1", new ApplicationException("Exception1")));
            httpContext.AddError(new ApplicationException("Exception2"));

            return httpContext;
        }

        public static HttpContext AddRequestTelemetry(this HttpContext context, RequestTelemetry requestTelemetry)
        {
            context.Items["Microsoft.ApplicationInsights.RequestTelemetry"] = requestTelemetry;
            return context;
        }

        public static HttpContext AddRequestCookie(this HttpContext context, HttpCookie cookie)
        {
            context.Request.Cookies.Add(cookie);
            return context;
        }

        private static void InitializeTelemetryConfiguration()
        {
            TelemetryModules.Instance.Modules.Clear();
            TelemetryModules.Instance.Modules.Add(new RequestTrackingTelemetryModule());
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

        private class DefaultController : Controller
        {
        }
    }
}