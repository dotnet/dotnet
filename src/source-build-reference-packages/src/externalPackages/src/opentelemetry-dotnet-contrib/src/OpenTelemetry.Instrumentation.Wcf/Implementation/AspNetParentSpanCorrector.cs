// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#if NETFRAMEWORK
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using OpenTelemetry.Context.Propagation;

namespace OpenTelemetry.Instrumentation.Wcf.Implementation;

/// <summary>
/// When WCF is hosted in ASP.NET and the ASP.NET telemetry instrumentation is installed, the execution context does not flow from ASP.NET into WCF
/// so the span which is created by ASP.NET is not visible to WCF. When OpenTelemetry.Instrumentation.Wcf then creates its own span it uses the parent
/// which is passed from the caller. This results in the ASP.NET span and the WCF span being siblings off the same parent when, really, the WCF span
/// should actually be a child of the ASP.NET (transport) span. This class corrects that behavior so the generated spans are correctly parented.
///
/// The way it does that is it hooks into the ASP.NET telemetry and rewrites the incoming request headers to reflect the ASP.NET parent (so, as far
/// as WCF is concerned, it appears like the ASP.NET parent span was sent from the caller). It does this all via reflection to avoid having explicit
/// dependencies on System.Web and OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule. This way this behavior is only enabled if the consumer
/// also has the ASP.NET instrumentation installed.
/// </summary>
internal static class AspNetParentSpanCorrector
{
    private const string TelemetryHttpModuleTypeName = "OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule, OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule";
    private const string TelemetryHttpModuleOptionsTypeName = "OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModuleOptions, OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule";
    private const string AspNetInstrumentationTypeName = "OpenTelemetry.Instrumentation.AspNet.AspNetInstrumentation, OpenTelemetry.Instrumentation.AspNet";

    private static readonly ReflectedInfo? ReflectedValues = Initialize();
    private static readonly PropertyFetcher<object> RequestFetcher = new("Request");
    private static readonly PropertyFetcher<NameValueCollection> HeadersFetcher = new("Headers");
    private static int isRegistered;

    public static void Register()
    {
        if (Interlocked.CompareExchange(ref isRegistered, 1, 0) == 0)
        {
            ReflectedValues?.SubscribeToOnRequestStartedCallback();
        }
    }

    private static void OnRequestStarted(Activity activity, object context)
    {
        var request = RequestFetcher.Fetch(context);
        var headers = HeadersFetcher.Fetch(request);

        var headersWereOriginallyReadOnly = ReflectedValues!.GetHeadersReadOnly(headers);

        if (headersWereOriginallyReadOnly)
        {
            ReflectedValues!.SetHeadersReadOnly(headers, false);
        }

        try
        {
            ReflectedValues.GetTelemetryHttpModulePropagator().Inject(
                new PropagationContext(activity.Context, Baggage.Current),
                headers,
                (headers, name, value) => headers[name] = value);
        }
        finally
        {
            if (headersWereOriginallyReadOnly)
            {
                ReflectedValues.SetHeadersReadOnly(headers, true);
            }
        }
    }

    private static ReflectedInfo? Initialize()
    {
        try
        {
            var isReadOnlyProp = typeof(NameValueCollection).GetProperty("IsReadOnly", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy) ?? throw new NotSupportedException("NameValueCollection.IsReadOnly property not found");

            var setHeadersReadOnly = (Action<NameValueCollection, bool>)isReadOnlyProp.SetMethod.CreateDelegate(typeof(Action<NameValueCollection, bool>));
            var getHeadersReadOnly = (Func<NameValueCollection, bool>)isReadOnlyProp.GetMethod.CreateDelegate(typeof(Func<NameValueCollection, bool>));

            return new ReflectedInfo(setHeadersReadOnly, getHeadersReadOnly, GenerateGetPropagatorLambda(), GenerateSubscribeLambda());
        }
        catch (Exception ex)
        {
            WcfInstrumentationEventSource.Log.AspNetReflectionFailedToBind(ex);
        }

        return null;
    }

    private static Func<TextMapPropagator> GenerateGetPropagatorLambda()
    {
        // this method generates this lambda:
        // () => TelemetryHttpModule.Options.TextMapPropagator

        var telemetryHttpModuleType = Type.GetType(TelemetryHttpModuleTypeName, true);
        var options = Expression.Property(null, telemetryHttpModuleType, "Options");
        var propertyReferenceExpression = Expression.Property(options, "TextMapPropagator");
        return (Func<TextMapPropagator>)Expression.Lambda(propertyReferenceExpression).Compile();
    }

    private static Func<object> GenerateSubscribeLambda()
    {
        // this method effectively generates this lambda:
        // () => TelemetryHttpModule.Options.OnRequestStartedCallback = CreateCombinedCallback(TelemetryHttpModule.Options.OnRequestStartedCallback)
        // The callback signature is Func<HttpContextBase, ActivityContext, Activity?>

        var telemetryHttpModuleType = Type.GetType(TelemetryHttpModuleTypeName, true);
        var telemetryHttpModuleOptionsType = Type.GetType(TelemetryHttpModuleOptionsTypeName, true);
        var onRequestStartedProp = telemetryHttpModuleOptionsType?.GetProperty("OnRequestStartedCallback") ?? throw new NotSupportedException("TelemetryHttpModuleOptions.OnRequestStartedCallback property not found");

        // ensure that HttpModuleTelemetry callbacks are initialized by the AspNet instrumentation
        var aspNetInstrumentationType = Type.GetType(AspNetInstrumentationTypeName, true);
        var aspNetInstrumentationInstance = aspNetInstrumentationType?.GetField("Instance", BindingFlags.Static | BindingFlags.Public);
        _ = aspNetInstrumentationInstance?.GetValue(null);

        // Get the parameter types from the callback property type itself to avoid hardcoded type loading
        var callbackType = onRequestStartedProp.PropertyType;
        var invokeMethod = callbackType.GetMethod("Invoke");
        var parameterTypes = invokeMethod!.GetParameters().Select(p => p.ParameterType).ToArray();
        var returnType = invokeMethod.ReturnType;

        // Parameters for the new combined callback (use actual parameter types from the callback)
        var httpContextParam = Expression.Parameter(parameterTypes[0], "httpContext");
        var activityContextParam = Expression.Parameter(parameterTypes[1], "activityContext");

        // Get the existing callback value
        var options = Expression.Property(null, telemetryHttpModuleType, "Options");

        // Capture the existing callback as a constant value at lambda creation time
        // This prevents infinite recursion by storing the original callback value before assignment
        var captureCallback = Expression.Lambda<Func<object>>(
            Expression.Convert(Expression.Property(options, onRequestStartedProp), typeof(object))).Compile();
        var existingCallbackValue = captureCallback();
        var existingCallback = Expression.Constant(existingCallbackValue, callbackType);

        // Create conditional logic: if existingCallback != null, call it, otherwise return null
        var nullCheck = Expression.NotEqual(existingCallback, Expression.Constant(null, callbackType));

        // Call existing callback if it exists
        var callExistingCallback = Expression.Call(
            existingCallback,
            invokeMethod,
            httpContextParam,
            activityContextParam);

        // If no existing callback, return null
        var nullActivity = Expression.Constant(null, returnType);

        // Choose between calling existing callback or returning null
        var activityResult = Expression.Condition(nullCheck, callExistingCallback, nullActivity);

        // Store the activity result in a variable
        var activityVariable = Expression.Variable(returnType, "activity");
        var assignActivity = Expression.Assign(activityVariable, activityResult);

        // Check if activity is not null before calling OnRequestStarted
        var activityNotNull = Expression.NotEqual(activityVariable, Expression.Constant(null, returnType));

        // Call OnRequestStarted method - convert HttpContextBase to object for compatibility
        var onRequestStartedMethod = typeof(AspNetParentSpanCorrector).GetMethod(nameof(OnRequestStarted), BindingFlags.Static | BindingFlags.NonPublic)!;
        var callOnRequestStarted = Expression.Call(onRequestStartedMethod, activityVariable, Expression.Convert(httpContextParam, typeof(object)));

        // Conditional call to OnRequestStarted
        var conditionalCall = Expression.IfThen(activityNotNull, callOnRequestStarted);

        // Return the activity
        var returnActivity = activityVariable;

        // Create the method body
        var methodBody = Expression.Block(
            [activityVariable],
            assignActivity,
            conditionalCall,
            returnActivity);

        // Create the combined callback lambda
        var combinedCallbackLambda = Expression.Lambda(callbackType, methodBody, httpContextParam, activityContextParam);

        // Create assignment: Options.OnRequestStartedCallback = combinedCallback
        var callbackProperty = Expression.Property(options, onRequestStartedProp);
        var subscribeExpression = Expression.Assign(callbackProperty, combinedCallbackLambda);

        return (Func<object>)Expression.Lambda(subscribeExpression).Compile();
    }

    private sealed class ReflectedInfo
    {
        public readonly Action<NameValueCollection, bool> SetHeadersReadOnly;
        public readonly Func<NameValueCollection, bool> GetHeadersReadOnly;
        public readonly Func<TextMapPropagator> GetTelemetryHttpModulePropagator;
        public readonly Func<object> SubscribeToOnRequestStartedCallback;

        public ReflectedInfo(
            Action<NameValueCollection, bool> setHeadersReadOnly,
            Func<NameValueCollection, bool> getHeadersReadOnly,
            Func<TextMapPropagator> getTelemetryHttpModulePropagator,
            Func<object> subscribeToOnRequestStartedCallback)
        {
            this.SetHeadersReadOnly = setHeadersReadOnly;
            this.GetHeadersReadOnly = getHeadersReadOnly;
            this.GetTelemetryHttpModulePropagator = getTelemetryHttpModulePropagator;
            this.SubscribeToOnRequestStartedCallback = subscribeToOnRequestStartedCallback;
        }
    }
}
#endif
