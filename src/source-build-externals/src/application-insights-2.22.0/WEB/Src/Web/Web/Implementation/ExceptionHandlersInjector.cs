namespace Microsoft.ApplicationInsights.Web.Implementation
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Reflection;
    using System.Reflection.Emit;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;

    internal static class ExceptionHandlersInjector
    {
        // we only support ASP.NET 5 for now 
        // and may extend list of supported versions if there will be a business need 
        private const int MinimumMvcVersion = 5;
        private const int MinimumWebApiVersion = 5;

        private const string AssemblyName = "Microsoft.ApplicationInsights.ExceptionTracking";
        private const string MvcHandlerName = AssemblyName + ".MvcExceptionFilter";
        private const string WebApiHandlerName = AssemblyName + ".WebApiExceptionLogger";

        private const string TelemetryClientFieldName = "telemetryClient";
        private const string IsAutoInjectedFieldName = "IsAutoInjected";
        private const string OnExceptionMethodName = "OnException";
        private const string OnLogMethodName = "Log";

        /// <summary>
        /// Forces injection of MVC5 exception filter and WebAPI2 exception logger into the global configurations.
        /// </summary>
        /// <remarks>
        /// <para>Injection is attempted each time method is called. However if the filter/logger was injected already, injection is skipped.</para>
        /// <para>Use this method only when you can guarantee it's called once per AppDomain.</para>
        /// </remarks>
        internal static void Inject(TelemetryClient telemetryClient)
        {
            try
            {
                WebEventSource.Log.InjectionStarted();

                var trackExceptionMethod = GetMethodOrFail(typeof(TelemetryClient), "TrackException", new[] { typeof(ExceptionTelemetry) });
                var exceptionTelemetryCtor = GetConstructorOrFail(typeof(ExceptionTelemetry), new[] { typeof(Exception) });

                var assemblyName = new AssemblyName(AssemblyName);
                var assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);

                var moduleBuilder = assemblyBuilder.DefineDynamicModule(AssemblyName);

                AddMvcFilter(
                    telemetryClient,
                    moduleBuilder,
                    exceptionTelemetryCtor,
                    trackExceptionMethod);

                AddWebApiExceptionLogger(
                    telemetryClient,
                    moduleBuilder,
                    exceptionTelemetryCtor,
                    trackExceptionMethod);
            }
            catch (Exception e)
            {
                WebEventSource.Log.InjectionUnknownError(e.ToString());
            }

            WebEventSource.Log.InjectionCompleted();
        }

        #region Mvc

        /// <summary>
        /// Generates new MVC5 filter class implementing HandleErrorAttribute and adds instance of it to the GlobalFilterCollection.
        /// </summary>
        /// <param name="telemetryClient"><see cref="TelemetryClient"/> instance.</param>
        /// <param name="moduleBuilder"><see cref="ModuleBuilder"/> to define type in.</param>
        /// <param name="exceptionTelemetryCtor"><see cref="ConstructorInfo"/> of default constructor of <see cref="ExceptionTelemetry"/>.</param>
        /// <param name="trackExceptionMethod"><see cref="MethodInfo"/> of <see cref="TelemetryClient.TrackException(ExceptionTelemetry)"/>.</param>
        private static void AddMvcFilter(
            TelemetryClient telemetryClient,
            ModuleBuilder moduleBuilder,
            ConstructorInfo exceptionTelemetryCtor,
            MethodInfo trackExceptionMethod)
        {
            // This method emits following code:
            // [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
            // public class MvcExceptionFilter : HandleErrorAttribute
            // {
            //     public const bool IsAutoInjected = true;
            //     private readonly TelemetryClient telemetryClient = new TelemetryClient();
            //     
            //     public MvcExceptionFilter(TelemetryClient tc) : base()
            //     {
            //         telemetryClient = tc;
            //     }
            //     
            //     public override void OnException(ExceptionContext filterContext)
            //     {
            //        if (filterContext != null && filterContext.HttpContext != null && filterContext.Exception != null && filterContext.HttpContext.IsCustomErrorEnabled)
            //            telemetryClient.TrackException(new ExceptionTelemetry(filterContext.Exception));
            //     }
            // }
            // 
            // GlobalFilters.Filters.Add(new MvcExceptionFilter(new TelemetryClient()));
            try
            {
                // Get HandleErrorAttribute, make sure it's resolved and MVC version is supported
                var handleErrorType = GetTypeOrFail("System.Web.Mvc.HandleErrorAttribute, System.Web.Mvc");
                if (handleErrorType.Assembly.GetName().Version.Major < MinimumMvcVersion)
                {
                    WebEventSource.Log.InjectionVersionNotSupported(handleErrorType.Assembly.GetName().Version.ToString(), "MVC");
                    return;
                }

                // get global filter collection
                GetMvcGlobalFiltersOrFail(out dynamic globalFilters, out Type globalFilterCollectionType);

                if (!NeedToInjectMvc(globalFilters))
                {
                    // there is another filter in the collection that has IsAutoInjected const field set to true - stop injection.
                    return;
                }

                var exceptionContextType = GetTypeOrFail("System.Web.Mvc.ExceptionContext, System.Web.Mvc");
                var exceptionGetter = GetMethodOrFail(exceptionContextType, "get_Exception");

                var controllerContextType = GetTypeOrFail("System.Web.Mvc.ControllerContext, System.Web.Mvc");
                var httpContextGetter = GetMethodOrFail(controllerContextType, "get_HttpContext");

                var addFilter = GetMethodOrFail(globalFilterCollectionType, "Add", new[] { typeof(object) });

                // HttpContextBase requires full assembly name to be resolved
                // even though version 4.0.0.0 (CLR version) is specified, it will be resolved to the latest .NET System.Web installed
                var httpContextBaseType = GetTypeOrFail("System.Web.HttpContextBase, System.Web, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
                var isCustomErrorEnabled = GetMethodOrFail(httpContextBaseType, "get_IsCustomErrorEnabled");

                // build a type for exception filter.
                TypeBuilder typeBuilder = moduleBuilder.DefineType(MvcHandlerName, TypeAttributes.Public | TypeAttributes.Class, handleErrorType);
                typeBuilder.SetCustomAttribute(GetUsageAttributeOrFail());
                var telemetryClientField = typeBuilder.DefineField(TelemetryClientFieldName, typeof(TelemetryClient), FieldAttributes.Private);

                DefineAutoInjectedField(typeBuilder);

                // emit constructor that assigns telemetry client field
                var handleErrorBaseCtor = GetConstructorOrFail(handleErrorType, new Type[0]);
                EmitConstructor(typeBuilder, typeof(TelemetryClient), telemetryClientField, handleErrorBaseCtor);

                // emit OnException method that handles exception
                EmitMvcOnException(
                    typeBuilder,
                    exceptionContextType,
                    telemetryClientField,
                    exceptionGetter,
                    trackExceptionMethod,
                    exceptionTelemetryCtor,
                    httpContextBaseType,
                    httpContextGetter,
                    isCustomErrorEnabled);

                // create error handler type
                var handlerType = typeBuilder.CreateType();

                // add handler to global filters
                var mvcFilter = Activator.CreateInstance(handlerType, telemetryClient);
                addFilter.Invoke(globalFilters, new[] { mvcFilter });
            }
            catch (ResolutionException e)
            {
                // some of the required types/methods/properties/etc were not found.
                // it may indicate we are dealing with a new version of MVC library
                // handle it and log here, we may still succeed with WebApi injection
                WebEventSource.Log.InjectionFailed("MVC", e.ToInvariantString());
            }
        }

        /// <summary>
        /// Emits OnException method.
        /// </summary>
        /// <param name="typeBuilder">MVCExceptionFilter type builder.</param>
        /// <param name="exceptionContextType">Type of ExceptionContext.</param>
        /// <param name="telemetryClientField">FieldInfo of MVCExceptionFilter.telemetryClient.</param>
        /// <param name="exceptionGetter">MethodInfo to get ExceptionContext.Exception.</param>
        /// <param name="trackException">MethodInfo of TelemetryClient.TrackException(ExceptionTelemetry).</param>
        /// <param name="exceptionTelemetryCtor">ConstructorInfo of ExceptionTelemetry.</param>
        /// <param name="httpContextBaseType">Type of HttpContextBase.</param>
        /// <param name="httpContextGetter">MethodInfo to get ExceptionContext.HttpContext.</param>
        /// <param name="isCustomErrorEnabled">MethodInfo to get ExceptionContext.HttpContextBase.IsCustomErrorEnabled.</param>
        private static void EmitMvcOnException(
            TypeBuilder typeBuilder,
            Type exceptionContextType,
            FieldInfo telemetryClientField,
            MethodInfo exceptionGetter,
            MethodInfo trackException,
            ConstructorInfo exceptionTelemetryCtor,
            Type httpContextBaseType,
            MethodInfo httpContextGetter,
            MethodInfo isCustomErrorEnabled)
        {
            // This method emits following code:
            // public override void OnException(ExceptionContext filterContext)
            // {
            //    if (filterContext != null && filterContext.HttpContext != null && filterContext.Exception != null && filterContext.HttpContext.IsCustomErrorEnabled)
            //        telemetryClient.TrackException(new ExceptionTelemetry(filterContext.Exception));
            // }  
             
            // defines public override void OnException(ExceptionContext filterContext)
            var onException = typeBuilder.DefineMethod(
                OnExceptionMethodName,
                MethodAttributes.Public | MethodAttributes.ReuseSlot | MethodAttributes.Virtual | MethodAttributes.HideBySig,
                null,
                new[] { exceptionContextType });
            var il = onException.GetILGenerator();

            Label track = il.DefineLabel();
            Label end = il.DefineLabel();
            Label n1 = il.DefineLabel();
            Label n2 = il.DefineLabel();
            Label n3 = il.DefineLabel();

            var httpContext = il.DeclareLocal(httpContextBaseType);
            var exception = il.DeclareLocal(typeof(Exception));
            var v2 = il.DeclareLocal(typeof(bool));
            var v3 = il.DeclareLocal(typeof(bool));
            var v4 = il.DeclareLocal(typeof(bool));
            var v5 = il.DeclareLocal(typeof(bool));

            // if filterContext is null, goto the end
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Ldnull);
            il.Emit(OpCodes.Ceq);
            il.Emit(OpCodes.Stloc, v2);
            il.Emit(OpCodes.Ldloc, v2);
            il.Emit(OpCodes.Brfalse_S, n1);
            il.Emit(OpCodes.Br_S, end);

            // if filterContext.HttpContext is null, goto the end
            il.MarkLabel(n1);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Callvirt, httpContextGetter);
            il.Emit(OpCodes.Stloc, httpContext);
            il.Emit(OpCodes.Ldloc, httpContext);
            il.Emit(OpCodes.Ldnull);
            il.Emit(OpCodes.Ceq);
            il.Emit(OpCodes.Stloc, v3);
            il.Emit(OpCodes.Ldloc, v3);
            il.Emit(OpCodes.Brfalse_S, n2);
            il.Emit(OpCodes.Br_S, end);

            // if filterContext.Exception is null, goto the end
            il.MarkLabel(n2);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Callvirt, exceptionGetter);
            il.Emit(OpCodes.Stloc, exception);
            il.Emit(OpCodes.Ldloc, exception);
            il.Emit(OpCodes.Ldnull);
            il.Emit(OpCodes.Ceq);
            il.Emit(OpCodes.Stloc_S, v4);
            il.Emit(OpCodes.Ldloc_S, v4);
            il.Emit(OpCodes.Brfalse_S, n3);
            il.Emit(OpCodes.Br_S, end);

            // if filterContext.HttpContext.IsCustomErrorEnabled is false, goto the end
            il.MarkLabel(n3);
            il.Emit(OpCodes.Ldloc, httpContext);
            il.Emit(OpCodes.Callvirt, isCustomErrorEnabled);
            il.Emit(OpCodes.Ldc_I4_0);
            il.Emit(OpCodes.Ceq);
            il.Emit(OpCodes.Stloc_S, v5);
            il.Emit(OpCodes.Ldloc_S, v5);
            il.Emit(OpCodes.Brfalse_S, track);
            il.Emit(OpCodes.Br_S, end);

            // telemetryClient.TrackException(new ExceptionTelemetry(filterContext.Exception))
            il.MarkLabel(track);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, telemetryClientField);
            il.Emit(OpCodes.Ldloc, exception);
            il.Emit(OpCodes.Newobj, exceptionTelemetryCtor);
            il.Emit(OpCodes.Callvirt, trackException);
            il.Emit(OpCodes.Br_S, end);

            il.MarkLabel(end);
            il.Emit(OpCodes.Ret);
        }

        /// <summary>
        /// Gets GlobalFilters.Filters property value. If not resolved, or is null, fails with <see cref="ResolutionException"/>.
        /// </summary>
        /// <param name="globalFilters">Resolved GlobalFilters.Filters instance.</param>
        /// <param name="globalFilterCollectionType">Resolved GlobalFilterCollection type.</param>
        private static void GetMvcGlobalFiltersOrFail(out dynamic globalFilters, out Type globalFilterCollectionType)
        {
            globalFilterCollectionType = GetTypeOrFail("System.Web.Mvc.GlobalFilterCollection, System.Web.Mvc");

            var globalFiltersType = GetTypeOrFail("System.Web.Mvc.GlobalFilters, System.Web.Mvc");
            globalFilters = GetStaticPropertyValueOrFail(globalFiltersType, "Filters");
        }

        /// <summary>
        /// Checks if another auto injected filter was already added into the filter collection.
        /// </summary>
        /// <param name="globalFilters">GlobalFilters.Filters value of GlobalFilterCollection type.</param>
        /// <returns>True if injection needs to be done, false when collection already contains another auto injected filter.</returns>
        private static bool NeedToInjectMvc(dynamic globalFilters)
        {
            var filters = (IEnumerable)globalFilters;

            // GlobalFilterCollection must implements IEnumerable, if not - fail.
            if (filters == null)
            {
                throw new ResolutionException(string.Format(CultureInfo.InvariantCulture, "Unexpected type of GlobalFilterCollection {0}", globalFilters.GetType()));
            }

            var mvcFilterType = GetTypeOrFail("System.Web.Mvc.Filter, System.Web.Mvc");
            var mvcFilterInstanceProp = GetPropertyOrFail(mvcFilterType, "Instance");

            // iterate over the filters
            foreach (var filter in filters)
            {
                if (filter.GetType() != mvcFilterType)
                {
                    throw new ResolutionException(string.Format(CultureInfo.InvariantCulture, "Unexpected type of MVC Filter {0}", filter.GetType()));
                }

                var instance = mvcFilterInstanceProp.GetValue(filter);
                if (instance == null)
                {
                    throw new ResolutionException($"MVC Filter does not have Instance property");
                }

                var isAutoInjectedField = instance.GetType().GetField(IsAutoInjectedFieldName, BindingFlags.Public | BindingFlags.Static);
                if (isAutoInjectedField != null && (bool)isAutoInjectedField.GetValue(null))
                {
                    // isAutoInjected public const field (when true) indicates that our filter is already injected by other component.
                    // return false and stop MVC injection
                    WebEventSource.Log.InjectionSkipped(instance.GetType().AssemblyQualifiedName, "MVC");
                    return false;
                }
            }

            return true;
        }

        #endregion

        #region WebApi

        /// <summary>
        /// Generates new WebAPI2 exception logger class implementing ExceptionLogger and adds instance of it to the GlobalConfiguration.Configuration.Services of IExceptionLogger type.
        /// </summary>
        /// <param name="telemetryClient"><see cref="TelemetryClient"/> instance.</param>
        /// <param name="moduleBuilder"><see cref="ModuleBuilder"/> to define type in.</param>
        /// <param name="exceptionTelemetryCtor"><see cref="ConstructorInfo"/> of default constructor of <see cref="ExceptionTelemetry"/>.</param>
        /// <param name="trackExceptionMethod"><see cref="MethodInfo"/> of <see cref="TelemetryClient.TrackException(ExceptionTelemetry)"/>.</param>
        private static void AddWebApiExceptionLogger(
            TelemetryClient telemetryClient,
            ModuleBuilder moduleBuilder,
            ConstructorInfo exceptionTelemetryCtor,
            MethodInfo trackExceptionMethod)
        {
            // This method emits following code:
            // public class WebApiExceptionLogger : ExceptionLogger
            // {
            //     public const bool IsAutoInjected = true;
            //     private readonly TelemetryClient telemetryClient = new TelemetryClient();
            //     
            //     public WebApiExceptionLogger(TelemetryClient tc) : base()
            //     {
            //         telemetryClient = tc;
            //     }
            //     
            //     public override void OnLog(ExceptionLoggerContext context)
            //     {
            //        if (context != null && context.Exception != null)
            //            telemetryClient.TrackException(new ExceptionTelemetry(context.Exception));
            //     }
            // }
            // 
            // GlobalConfiguration.Configuration.Services.Add(typeof(IExceptionLogger), new WebApiExceptionFilter(new TelemetryClient()));
            try
            {
                // try to get all types/methods/properties/fields
                // and if something is not available, fail fast
                var baseExceptionLoggerType = GetTypeOrFail("System.Web.Http.ExceptionHandling.ExceptionLogger, System.Web.Http");
                if (baseExceptionLoggerType.Assembly.GetName().Version.Major < MinimumWebApiVersion)
                {
                    WebEventSource.Log.InjectionVersionNotSupported(baseExceptionLoggerType.Assembly.GetName().Version.ToString(), "WebApi");
                    return;
                }

                var exceptionContextType = GetTypeOrFail("System.Web.Http.ExceptionHandling.ExceptionLoggerContext, System.Web.Http");
                var iexceptionLoggerType = GetTypeOrFail("System.Web.Http.ExceptionHandling.IExceptionLogger, System.Web.Http");

                // get GlobalConfiguration.Configuration.Services
                GetServicesContainerWebApiOrFail(out dynamic servicesContainer, out Type servicesContainerType);
                if (!NeedToInjectWebApi(servicesContainer, servicesContainerType, iexceptionLoggerType))
                {
                    return;
                }

                var addLogger = GetMethodOrFail(servicesContainerType, "Add", new[] { typeof(Type), typeof(object) });
                var exceptionGetter = GetMethodOrFail(exceptionContextType, "get_Exception");
                var exceptionLoggerBaseCtor = baseExceptionLoggerType.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, CallingConventions.Standard, new Type[0], null);
                if (exceptionLoggerBaseCtor == null)
                {
                    throw new ResolutionException(string.Format(CultureInfo.InvariantCulture, "Failed to get default constructor for type {0}", baseExceptionLoggerType.AssemblyQualifiedName));
                }

                // define 'public class WebApiExceptionLogger : ExceptionLogger' type
                TypeBuilder typeBuilder = moduleBuilder.DefineType(WebApiHandlerName, TypeAttributes.Public | TypeAttributes.Class, baseExceptionLoggerType);
                DefineAutoInjectedField(typeBuilder);
                var telemetryClientField = typeBuilder.DefineField(TelemetryClientFieldName, typeof(TelemetryClient), FieldAttributes.Private | FieldAttributes.InitOnly);

                // emit constructor that assigns telemetry client field
                EmitConstructor(typeBuilder, typeof(TelemetryClient), telemetryClientField, exceptionLoggerBaseCtor);

                // emit Log method 
                EmitWebApiLog(typeBuilder, exceptionContextType, exceptionGetter, telemetryClientField, exceptionTelemetryCtor, trackExceptionMethod);

                // create error WebApiExceptionLogger type
                var exceptionLoggerType = typeBuilder.CreateType();

                // add WebApiExceptionLogger to list of services
                var exceptionLogger = Activator.CreateInstance(exceptionLoggerType, telemetryClient);
                addLogger.Invoke(servicesContainer, new[] { iexceptionLoggerType, exceptionLogger });
            }
            catch (ResolutionException e)
            {
                // some of the required types/methods/properties/etc were not found.
                // it may indicate we are dealing with a new version of WebApi library
                // handle it and log here, we may still succeed with MVC injection
                WebEventSource.Log.InjectionFailed("WebApi", e.ToInvariantString());
            }
        }

        /// <summary>
        /// Emits OnLog method.
        /// </summary>
        /// <param name="typeBuilder">MVCExceptionFilter type builder.</param>
        /// <param name="exceptionContextType">Type of ExceptionContext.</param>
        /// <param name="exceptionGetter">MethodInfo to get ExceptionLoggerContext.Exception.</param>
        /// <param name="telemetryClientField">FieldInfo of WebAPIExceptionFilter.telemetryClient.</param>
        /// <param name="exceptionTelemetryCtor">ConstructorInfo of ExceptionTelemetry.</param>
        /// <param name="trackException">MethodInfo of TelemetryClient.TrackException(ExceptionTelemetry).</param>
        private static void EmitWebApiLog(TypeBuilder typeBuilder, Type exceptionContextType, MethodInfo exceptionGetter, FieldInfo telemetryClientField, ConstructorInfo exceptionTelemetryCtor, MethodInfo trackException)
        {
            // This method emits following code:
            // public override void OnLog(ExceptionLoggerContext context)
            // {
            //    if (context != null && context.Exception != null)
            //        telemetryClient.TrackException(new ExceptionTelemetry(context.Exception));
            // }
            // public override void OnLog(ExceptionLoggerContext context)
            var log = typeBuilder.DefineMethod(OnLogMethodName, MethodAttributes.Public | MethodAttributes.ReuseSlot | MethodAttributes.Virtual | MethodAttributes.HideBySig, null, new[] { exceptionContextType });
            var il = log.GetILGenerator();

            Label track = il.DefineLabel();
            Label end = il.DefineLabel();
            Label n1 = il.DefineLabel();

            var exception = il.DeclareLocal(typeof(Exception));
            var v1 = il.DeclareLocal(typeof(bool));
            var v2 = il.DeclareLocal(typeof(bool));

            // is context is null, goto the end
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Ldnull);
            il.Emit(OpCodes.Ceq);
            il.Emit(OpCodes.Stloc, v1);
            il.Emit(OpCodes.Ldloc, v1);
            il.Emit(OpCodes.Brfalse_S, n1);
            il.Emit(OpCodes.Br_S, end);

            // is context.Exception is null, goto the end
            il.MarkLabel(n1);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Callvirt, exceptionGetter);
            il.Emit(OpCodes.Stloc, exception);
            il.Emit(OpCodes.Ldloc, exception);
            il.Emit(OpCodes.Ldnull);
            il.Emit(OpCodes.Ceq);
            il.Emit(OpCodes.Stloc, v2);
            il.Emit(OpCodes.Ldloc, v2);
            il.Emit(OpCodes.Brfalse_S, track);
            il.Emit(OpCodes.Br_S, end);

            // telemetryClient.TrackException(new ExceptionTelemetry(context.Exception))
            il.MarkLabel(track);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, telemetryClientField);
            il.Emit(OpCodes.Ldloc, exception);
            il.Emit(OpCodes.Newobj, exceptionTelemetryCtor);
            il.Emit(OpCodes.Callvirt, trackException);
            il.Emit(OpCodes.Br_S, end);

            il.MarkLabel(end);
            il.Emit(OpCodes.Ret);
        }

        /// <summary>
        /// Gets GlobalConfiguration.Configuration.Services value and type.
        /// </summary>
        /// <param name="serviceContaner">Services collection.</param>
        /// <param name="servicesContainerType">ServicesContainer type of Services.</param>
        private static void GetServicesContainerWebApiOrFail(out dynamic serviceContaner, out Type servicesContainerType)
        {
            var globalConfigurationType = GetTypeOrFail("System.Web.Http.GlobalConfiguration, System.Web.Http.WebHost");
            var httpConfigurationType = GetTypeOrFail("System.Web.Http.HttpConfiguration, System.Web.Http");
            servicesContainerType = GetTypeOrFail("System.Web.Http.Controllers.ServicesContainer, System.Web.Http");

            var configuration = GetStaticPropertyValueOrFail(globalConfigurationType, "Configuration");
            serviceContaner = GetPropertyValueOrFail(httpConfigurationType, configuration, "Services");
        }

        /// <summary>
        /// Checks if another auto injected logger was already added into the Services collection.
        /// </summary>
        /// <param name="servicesContainer">GlobalConfiguration.Configuration.Services value.</param>
        /// <param name="servicesContainerType">ServicesContainer type.</param>
        /// <param name="iexceptionLoggerType">IExceptionLogger type.</param>
        /// <returns>True if injection needs to be done, false when collection already contains another auto injected logger.</returns>
        private static bool NeedToInjectWebApi(dynamic servicesContainer, Type servicesContainerType, Type iexceptionLoggerType)
        {
            // call ServicesContainer.GetServices(Type) to get collection of all exception loggers.
            var getServicesMethod = GetMethodOrFail(servicesContainerType, "GetServices", new[] { typeof(Type) });

            var exceptionLoggersObj = getServicesMethod.Invoke(servicesContainer, new object[] { iexceptionLoggerType });
            var exceptionLoggers = (IEnumerable<object>)exceptionLoggersObj;
            if (exceptionLoggers == null)
            {
                throw new ResolutionException(string.Format(CultureInfo.InvariantCulture, "Unexpected type of {0}", exceptionLoggersObj.GetType()));
            }

            foreach (var filter in exceptionLoggers)
            {
                var isAutoInjectedField = filter.GetType().GetField(IsAutoInjectedFieldName, BindingFlags.Public | BindingFlags.Static);
                if (isAutoInjectedField != null)
                {
                    var isAutoInjectedFilter = (bool)isAutoInjectedField.GetValue(null);
                    if (isAutoInjectedFilter)
                    {
                        // if logger defines isAutoInjected property, stop WebApi injection.
                        WebEventSource.Log.InjectionSkipped(filter.GetType().AssemblyQualifiedName, "WebApi");
                        return false;
                    }
                }
            }

            return true;
        }
        #endregion

        /// <summary>
        /// Emits constructor for MVC Filter and WebAPI logger.
        /// </summary>
        /// <param name="typeBuilder">TypeBuilder of MVC filter or WebAPI logger.</param>
        /// <param name="telemetryClientType">Type of TelemetryClient.</param>
        /// <param name="field">FieldInfo to assign TelemetryClient instance to.</param>
        /// <param name="baseCtorInfo">ConstructorInfo of the base class.</param>
        private static void EmitConstructor(TypeBuilder typeBuilder, Type telemetryClientType, FieldInfo field, ConstructorInfo baseCtorInfo)
        {
            // this method emits following code:
            // public ClassName(TelemetryClient tc) : base()
            // {
            //     telemetryClient = tc;
            // }

            // public <TypeName>(TelemetryClient tc)
            var ctor = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, new[] { telemetryClientType });
            var il = ctor.GetILGenerator();

            // call base constructor 
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Call, baseCtorInfo);

            // assign telemetryClient field
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Stfld, field);
            il.Emit(OpCodes.Ret);
        }

        /// <summary>
        /// Emits IsAutoInjected field. The field is used to mark injected filter/logger and prevent double-injection.
        /// </summary>
        /// <param name="exceptionHandlerType">Type of the exception handler.</param>
        private static void DefineAutoInjectedField(TypeBuilder exceptionHandlerType)
        {
            // This method emits following code:
            //     public const bool IsAutoInjected = true;
            // we mark our types by using IsAutoInjected field - this is prevention mechanism.
            // If this code is shipped as a standalone nuget, the Web SDK and lightup might both register filters.
            // all components re-using this code must check for IsAutoInjected on the filter/handler
            // and do not inject itself if there is already a filter with such field
            var field = exceptionHandlerType.DefineField(IsAutoInjectedFieldName, typeof(bool), FieldAttributes.Public | FieldAttributes.Static | FieldAttributes.Literal | FieldAttributes.HasDefault);
            field.SetConstant(true);
        }

        #region Helpers

        /// <summary>
        /// Gets attribute builder for AttributeUsageAttribute with AllowMultiple set to true.
        /// </summary>
        /// <returns>CustomAttributeBuilder for the AttributeUsageAttribute.</returns>
        private static CustomAttributeBuilder GetUsageAttributeOrFail()
        {
            // This method emits following code:
            //    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
            var attributeUsageCtor = GetConstructorOrFail(typeof(AttributeUsageAttribute), new[] { typeof(AttributeTargets) });
            var allowMultipleInfo = typeof(AttributeUsageAttribute).GetProperty("AllowMultiple", BindingFlags.Instance | BindingFlags.Public);
            if (attributeUsageCtor == null || allowMultipleInfo == null)
            {
                // must not ever happen 
                throw new ResolutionException("Failed to get AttributeUsageAttribute ctor or AllowMultiple property");
            }

            return new CustomAttributeBuilder(attributeUsageCtor, new object[] { AttributeTargets.Class | AttributeTargets.Method }, new[] { allowMultipleInfo }, new object[] { true });
        }

        /// <summary>
        /// Gets type by it's name and throws <see cref="ResolutionException"/> if type is not found.
        /// </summary>
        /// <param name="typeName">Name of the type to be found. It could be a short namespace qualified name or assembly qualified name, as appropriate.</param>
        /// <returns>Resolved <see cref="Type"/>.</returns>
        private static Type GetTypeOrFail(string typeName)
        {
            var type = Type.GetType(typeName);
            if (type == null)
            {
                throw new ResolutionException(string.Format(CultureInfo.InvariantCulture, "Failed to get {0} type", typeName));
            }

            return type;
        }

        /// <summary>
        /// Gets public instance method info from the given type with the given of parameters. Throws <see cref="ResolutionException"/> if method is not found.
        /// </summary>
        /// <param name="type">Type to get method from.</param>
        /// <param name="methodName">Method name.</param>
        /// <param name="paramTypes">Array of method parameters. Optional (empty array by default).</param>
        /// <returns>Resolved <see cref="MethodInfo"/>.</returns>
        private static MethodInfo GetMethodOrFail(Type type, string methodName, Type[] paramTypes = null)
        {
            var method = type.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public, null, paramTypes ?? new Type[0], null);
            if (method == null)
            {
                throw new ResolutionException(string.Format(CultureInfo.InvariantCulture, "Failed to get {0} method info from type {1}", methodName, type));
            }

            return method;
        }

        /// <summary>
        /// Gets public instance constructor info from the given type with the given of parameters. Throws <see cref="ResolutionException"/> if constructor is not found.
        /// </summary>
        /// <param name="type">Type to get constructor from.</param>
        /// <param name="paramTypes">Array of constructor parameters.</param>
        /// <returns>Resolved <see cref="ConstructorInfo"/>.</returns>
        private static ConstructorInfo GetConstructorOrFail(Type type, Type[] paramTypes)
        {
            var ctor = type.GetConstructor(paramTypes);
            if (ctor == null)
            {
                throw new ResolutionException(string.Format(CultureInfo.InvariantCulture, "Failed to get constructor info from type {0}", type));
            }

            return ctor;
        }

        /// <summary>
        /// Gets public instance property info from the given type. Throws <see cref="ResolutionException"/> if property is not found.
        /// </summary>
        /// <param name="type">Type to get property from.</param>
        /// <param name="propertyName">Name or the property to get.</param>
        /// <returns>Resolved <see cref="PropertyInfo"/>.</returns>
        private static PropertyInfo GetPropertyOrFail(Type type, string propertyName)
        {
            var prop = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
            if (prop == null)
            {
                throw new ResolutionException(string.Format(CultureInfo.InvariantCulture, "Failed to get {0} property info from type {1}", propertyName, type));
            }

            return prop;
        }

        /// <summary>
        /// Gets public instance property value from the given type. Throws <see cref="ResolutionException"/> if property is not found.
        /// </summary>
        /// <param name="type">Type to get property from.</param>
        /// <param name="instance">Instance of type to get property value from.</param>
        /// <param name="propertyName">Name or the property to get.</param>
        /// <returns>Value of the property.</returns>
        private static dynamic GetPropertyValueOrFail(Type type, dynamic instance, string propertyName)
        {
            var prop = GetPropertyOrFail(type, propertyName);

            var value = prop.GetValue(instance);
            if (value == null)
            {
                throw new ResolutionException(string.Format(CultureInfo.InvariantCulture, "Failed to get {0} property info from type {1}", propertyName, type));
            }

            return value;
        }

        /// <summary>
        /// Gets public static property value from the given type. Throws <see cref="ResolutionException"/> if property is not found.
        /// </summary>
        /// <param name="type">Type to get property from.</param>
        /// <param name="propertyName">Name of the property to get.</param>
        /// <returns>Value of the property.</returns>
        private static dynamic GetStaticPropertyValueOrFail(Type type, string propertyName)
        {
            var prop = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Static);
            if (prop == null)
            {
                throw new ResolutionException(string.Format(CultureInfo.InvariantCulture, "Failed to get {0} property info from type {1}", propertyName, type));
            }

            var value = prop.GetValue(null);
            if (value == null)
            {
                throw new ResolutionException(string.Format(CultureInfo.InvariantCulture, "Failed to get {0} property value from type {1}", propertyName, type));
            }

            return value;
        }

        /// <summary>
        /// Represents specific resolution exception.
        /// </summary>
        [Serializable]
        [SuppressMessage("Microsoft.Design", "CA1064:ExceptionsShouldBePublic", Justification = "We expect that this exception will be caught within the internal scope and should never be exposed to an end user.")]
        [SuppressMessage("Microsoft.Design", "CA1032:ImplementStandardExceptionConstructors", Justification = "Internal use only, additional constructors aren't necessary.")]
        private class ResolutionException : Exception
        {
            public ResolutionException(string message) : base(message)
            {
            }
        }

        #endregion
    }
}