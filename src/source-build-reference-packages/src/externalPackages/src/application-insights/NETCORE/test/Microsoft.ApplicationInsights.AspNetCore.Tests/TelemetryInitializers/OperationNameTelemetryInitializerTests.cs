using Microsoft.ApplicationInsights.AspNetCore.DiagnosticListeners;

namespace Microsoft.ApplicationInsights.AspNetCore.Tests.TelemetryInitializers
{
    using System;
    using System.Diagnostics;
    using Microsoft.ApplicationInsights.AspNetCore.TelemetryInitializers;
    using Microsoft.ApplicationInsights.AspNetCore.Tests.Helpers;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Routing;
    using Xunit;
    using Microsoft.AspNetCore.Routing.Tree;
    using Microsoft.AspNetCore.Http;
    using Microsoft.ApplicationInsights.AspNetCore.Implementation;

    public class OperationNameTelemetryInitializerTests
    {
        private const string TestListenerName = "TestListener";

        private HostingDiagnosticListener CreateHostingListener(AspNetCoreMajorVersion aspNetCoreMajorVersion)
        {
            var hostingListener = new HostingDiagnosticListener(
                CommonMocks.MockTelemetryClient(telemetry => {}),
                CommonMocks.GetMockApplicationIdProvider(),
                injectResponseHeaders: true,
                trackExceptions: true,
                enableW3CHeaders: false,
                aspNetCoreMajorVersion: aspNetCoreMajorVersion);
            hostingListener.OnSubscribe();

            return hostingListener;
        }

        [Fact]
        public void InitializeThrowIfHttpContextAccessorIsNull()
        {
            Assert.ThrowsAny<ArgumentNullException>(() =>
            {
                var initializer = new OperationNameTelemetryInitializer(null);
            });
        }

        [Fact]
        public void InitializeDoesNotThrowIfHttpContextIsUnavailable()
        {
            var ac = new HttpContextAccessor() { HttpContext = null };

            var initializer = new OperationNameTelemetryInitializer(ac);

            initializer.Initialize(new RequestTelemetry());
        }

        [Fact]
        public void InitializeDoesNotThrowIfRequestTelemetryIsUnavailable()
        {
            var ac = new HttpContextAccessor() { HttpContext = new DefaultHttpContext() };

            var initializer = new OperationNameTelemetryInitializer(ac);

            initializer.Initialize(new RequestTelemetry());
        }

        [Fact]
        public void InitializeDoesNotOverrideOperationNameProvidedInline()
        {
            var contextAccessor = HttpContextAccessorHelper.CreateHttpContextAccessor(new RequestTelemetry(), null);

            var initializer = new OperationNameTelemetryInitializer(contextAccessor);

            var telemetry = new EventTelemetry();
            telemetry.Context.Operation.Name = "Name";
            initializer.Initialize(telemetry);

            Assert.Equal("Name", telemetry.Context.Operation.Name);
        }

        [Fact]
        public void InitializeSetsTelemetryOperationNameToMethodAndPath()
        {
            var contextAccessor = HttpContextAccessorHelper.CreateHttpContextAccessor(new RequestTelemetry(), null);

            var initializer = new OperationNameTelemetryInitializer(contextAccessor);

            var telemetry = new EventTelemetry();
            initializer.Initialize(telemetry);

            Assert.Equal("GET /Test", telemetry.Context.Operation.Name);
        }

        [Fact]
        public void InitializeSetsTelemetryOperationNameToControllerFromActionContext()
        {
            var actionContext = new ActionContext();
            actionContext.RouteData = new RouteData();
            actionContext.RouteData.Values.Add("controller", "home");

            var contextAccessor = HttpContextAccessorHelper.CreateHttpContextAccessor(new RequestTelemetry(), actionContext);

            var telemetryListener = new DiagnosticListener(TestListenerName);
            using (var listener = CreateHostingListener(AspNetCoreMajorVersion.One))
            {
                telemetryListener.Subscribe(listener);
                telemetryListener.Write("Microsoft.AspNetCore.Mvc.BeforeAction",
                    new { httpContext = contextAccessor.HttpContext, routeData = actionContext.RouteData });
            }
            var telemetry = contextAccessor.HttpContext.Features.Get<RequestTelemetry>();

            Assert.Equal("GET home", telemetry.Name);
        }

        [Fact]
        public void InitializeSetsTelemetryOperationNameToControllerAndActionFromActionContext()
        {
            var actionContext = new ActionContext();
            actionContext.RouteData = new RouteData();
            actionContext.RouteData.Values.Add("controller", "account");
            actionContext.RouteData.Values.Add("action", "login");

            var contextAccessor = HttpContextAccessorHelper.CreateHttpContextAccessor(new RequestTelemetry(), actionContext);

            var telemetryListener = new DiagnosticListener(TestListenerName);
            using (var listener = CreateHostingListener(AspNetCoreMajorVersion.One))
            {
                telemetryListener.Subscribe(listener);
                telemetryListener.Write("Microsoft.AspNetCore.Mvc.BeforeAction",
                    new { httpContext = contextAccessor.HttpContext, routeData = actionContext.RouteData });
            }
            var telemetry = contextAccessor.HttpContext.Features.Get<RequestTelemetry>();

            Assert.Equal("GET account/login", telemetry.Name);
        }

        [Fact]
        public void InitializeSetsTelemetryOperationNameToPageFromActionContext()
        {
            var actionContext = new ActionContext();
            actionContext.RouteData = new RouteData();
            actionContext.RouteData.Values.Add("page", "/Index");

            var contextAccessor = HttpContextAccessorHelper.CreateHttpContextAccessor(new RequestTelemetry(), actionContext);

            var telemetryListener = new DiagnosticListener(TestListenerName);
            using (var listener = CreateHostingListener(AspNetCoreMajorVersion.One))
            {
                telemetryListener.Subscribe(listener);
                telemetryListener.Write("Microsoft.AspNetCore.Mvc.BeforeAction",
                    new { httpContext = contextAccessor.HttpContext, routeData = actionContext.RouteData });
            }
            var telemetry = contextAccessor.HttpContext.Features.Get<RequestTelemetry>();

            Assert.Equal("GET /Index", telemetry.Name);
        }

        [Fact]
        public void InitializeSetsTelemetryOperationNameToControllerAndActionAndParameterFromActionContext()
        {
            var actionContext = new ActionContext();
            actionContext.RouteData = new RouteData();
            actionContext.RouteData.Values.Add("controller", "account");
            actionContext.RouteData.Values.Add("action", "login");
            actionContext.RouteData.Values.Add("parameter", "myName");

            var contextAccessor = HttpContextAccessorHelper.CreateHttpContextAccessor(new RequestTelemetry(), actionContext);

            var telemetryListener = new DiagnosticListener(TestListenerName);
            using (var listener = CreateHostingListener(AspNetCoreMajorVersion.One))
            {
                telemetryListener.Subscribe(listener);
                telemetryListener.Write("Microsoft.AspNetCore.Mvc.BeforeAction",
                    new { httpContext = contextAccessor.HttpContext, routeData = actionContext.RouteData });
            }

            var telemetry = contextAccessor.HttpContext.Features.Get<RequestTelemetry>();

            Assert.Equal("GET account/login [parameter]", telemetry.Name);
        }

        [Fact]
        public void InitializeSortsParameters()
        {
            var actionContext = new ActionContext();
            actionContext.RouteData = new RouteData();
            actionContext.RouteData.Values.Add("controller", "account");
            actionContext.RouteData.Values.Add("action", "login");
            actionContext.RouteData.Values.Add("parameterZ", "myName1");
            actionContext.RouteData.Values.Add("parameterA", "myName2");
            actionContext.RouteData.Values.Add("parameterN", "myName1");

            var contextAccessor = HttpContextAccessorHelper.CreateHttpContextAccessor(new RequestTelemetry(), actionContext);

            var telemetryListener = new DiagnosticListener(TestListenerName);
            using (var listener = CreateHostingListener(AspNetCoreMajorVersion.One))
            {
                telemetryListener.Subscribe(listener);
                telemetryListener.Write("Microsoft.AspNetCore.Mvc.BeforeAction",
                    new { httpContext = contextAccessor.HttpContext, routeData = actionContext.RouteData });
            }
            var telemetry = contextAccessor.HttpContext.Features.Get<RequestTelemetry>();

            Assert.Equal("GET account/login [parameterA/parameterN/parameterZ]", telemetry.Name);
        }

        [Fact]
        public void InitializeDoesNotIncludeRouteGroupKeyInParametersList()
        {
            var actionContext = new ActionContext();
            actionContext.RouteData = new RouteData();
            actionContext.RouteData.Values.Add("controller", "account");
            actionContext.RouteData.Values.Add("action", "login");
            actionContext.RouteData.Values.Add(TreeRouter.RouteGroupKey, "RouteGroupKey");

            var contextAccessor = HttpContextAccessorHelper.CreateHttpContextAccessor(new RequestTelemetry(), actionContext);
            var telemetryListener = new DiagnosticListener(TestListenerName);
            using (var listener = CreateHostingListener(AspNetCoreMajorVersion.One))
            {
                telemetryListener.Subscribe(listener);
                telemetryListener.Write("Microsoft.AspNetCore.Mvc.BeforeAction",
                    new { httpContext = contextAccessor.HttpContext, routeData = actionContext.RouteData });
            }
            var telemetry = contextAccessor.HttpContext.Features.Get<RequestTelemetry>();
            
            Assert.Equal("GET account/login", telemetry.Name);
        }

        [Fact]
        public void InitializeSetsRequestContextOperationNameToRequestName()
        {
            var telemetry = new RequestTelemetry();
            telemetry.Name = "POST /Test";

            var contextAccessor = HttpContextAccessorHelper.CreateHttpContextAccessor(telemetry);
            var initializer = new OperationNameTelemetryInitializer(contextAccessor);

            initializer.Initialize(telemetry);

            Assert.Equal("POST /Test", telemetry.Context.Operation.Name);
        }

    }
}