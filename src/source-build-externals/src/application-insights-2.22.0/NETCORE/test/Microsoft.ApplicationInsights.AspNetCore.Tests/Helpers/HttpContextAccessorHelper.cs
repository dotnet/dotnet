namespace Microsoft.ApplicationInsights.AspNetCore.Tests.Helpers
{
    using System;
    using Microsoft.ApplicationInsights.AspNetCore.DiagnosticListeners;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Infrastructure;
    using Microsoft.Extensions.DependencyInjection;

    public static class HttpContextAccessorHelper
    {
        public static HttpContextAccessor CreateHttpContextAccessor(RequestTelemetry requestTelemetry = null, ActionContext actionContext = null, string httpContextCorrelationId = null)
        {
            var services = new ServiceCollection();

            var request = new DefaultHttpContext().Request;
            request.Method = "GET";
            request.Path = new PathString("/Test");
            if (httpContextCorrelationId != null)
            {
                HttpHeadersUtilities.SetRequestContextKeyValue(request.Headers, RequestResponseHeaders.RequestContextSourceKey, httpContextCorrelationId);
            }

            var contextAccessor = new HttpContextAccessor { HttpContext = request.HttpContext };

            services.AddSingleton<IHttpContextAccessor>(contextAccessor);

            if (actionContext != null)
            {
                var si = new ActionContextAccessor();
                si.ActionContext = actionContext;
                services.AddSingleton<IActionContextAccessor>(si);
            }

            if (requestTelemetry != null)
            {
                request.HttpContext.Features.Set(requestTelemetry);
            }

            IServiceProvider serviceProvider = services.BuildServiceProvider();
            contextAccessor.HttpContext.RequestServices = serviceProvider;

            return contextAccessor;
        }

        public static HttpContextAccessor CreateHttpContextAccessorWithoutRequest(HttpContext httpContext, RequestTelemetry requestTelemetry = null)
        {
            var services = new ServiceCollection();

            var contextAccessor = new HttpContextAccessor { HttpContext = httpContext };

            services.AddSingleton<IHttpContextAccessor>(contextAccessor);

            if (requestTelemetry != null)
            {
                services.AddSingleton<RequestTelemetry>(requestTelemetry);
            }

            IServiceProvider serviceProvider = services.BuildServiceProvider();
            contextAccessor.HttpContext.RequestServices = serviceProvider;

            return contextAccessor;
        }
    }
}
