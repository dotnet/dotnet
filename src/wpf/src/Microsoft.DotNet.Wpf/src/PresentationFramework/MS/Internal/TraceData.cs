﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#define TRACE

//
// Description: Defines TraceData class, for providing debugging information
//              for Data Binding and Styling
//

using System.Collections;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using System.Windows.Diagnostics;
using System.Windows.Markup;

namespace MS.Internal
{
    // levels for the various extended traces
    internal enum TraceDataLevel
    {
        // Binding and friends
        CreateExpression    = PresentationTraceLevel.High, // 10,
        ShowPath            = PresentationTraceLevel.High, // 11,
        ResolveDefaults     = PresentationTraceLevel.High, // 13,
        Attach              = PresentationTraceLevel.Low, // 1,
        AttachToContext     = PresentationTraceLevel.Low, // 2,
        SourceLookup        = PresentationTraceLevel.Low, // 4,
        Activate            = PresentationTraceLevel.Low, // 3,
        Transfer            = PresentationTraceLevel.Medium, // 5,
        Update              = PresentationTraceLevel.Medium, // 6,
        Validation          = PresentationTraceLevel.High, // 12,
        Events              = PresentationTraceLevel.Medium, // 7,
        GetValue            = PresentationTraceLevel.High, // 12,
        ReplaceItem         = PresentationTraceLevel.Medium, // 8,
        GetInfo             = PresentationTraceLevel.Medium, // 9,

        // Data providers
        ProviderQuery       = PresentationTraceLevel.Low, // 1,
        XmlProvider         = PresentationTraceLevel.Medium, // 2,
        XmlBuildCollection  = PresentationTraceLevel.High, // 3,
    }

    /// <summary>
    /// Provides a central mechanism for providing debugging information
    /// to aid programmers in using data binding.
    /// Helpers are defined here.
    /// The rest of the class is generated; see also: AvTraceMessage.txt and genTraceStrings.pl
    /// </summary>
    internal static partial class TraceData
    {
        // ------------------------------------------------------------------
        // Constructors
        // ------------------------------------------------------------------

        static TraceData()
        {
            _avTrace.TraceExtraMessages += new AvTraceEventHandler(OnTrace);

            // This tells tracing that IsEnabled should be true if we're in the debugger,
            // even if the registry flag isn't turned on.  By default, IsEnabled is only
            // true if the registry is set.
            _avTrace.EnabledByDebugger = true;

            // This tells the tracing code not to automatically generate the .GetType
            // and .HashCode in the trace strings.
            _avTrace.SuppressGeneratedParameters = true;
        }

        // ------------------------------------------------------------------
        // Methods
        // ------------------------------------------------------------------

        // determine whether an extended trace should be produced for the given
        // object
        public static bool IsExtendedTraceEnabled(object element, TraceDataLevel level)
        {
            if (TraceData.IsEnabled)
            {
                PresentationTraceLevel traceLevel = PresentationTraceSources.GetTraceLevel(element);
                return (traceLevel >= (PresentationTraceLevel)level);
            }
            else
                return false;
        }

        // report/describe any additional parameters passed to TraceData.Trace()
        public static void OnTrace(AvTraceBuilder traceBuilder, ReadOnlySpan<object> parameters)
        {
            for (int i = 0; i < parameters.Length; i++)
            {
                object objectParam = parameters[i];
                traceBuilder.Append(" ");

                if (objectParam is string stringValue)
                {
                    traceBuilder.Append(stringValue);
                }
                else if (objectParam is not null)
                {
                    traceBuilder.Append(objectParam.GetType().Name);
                    traceBuilder.Append(":");
                    Describe(traceBuilder, objectParam);
                }
                else
                {
                    traceBuilder.Append("null");
                }
            }
        }

        // ------------------------------------------------------------------
        // Helper functions for message string construction
        // ------------------------------------------------------------------

        /// <summary>
        /// Construct a string that describes data and debugging information about the object.
        /// A title is appended in front if provided.
        /// If object o is not a recognized object, it will be ToString()'ed.
        /// </summary>
        /// <param name="traceBuilder">description will be appended to this builder</param>
        /// <param name="o">object to be described;
        /// currently recognized types: BindingExpression, Binding, DependencyObject, Exception</param>
        /// <returns>a string that describes the object</returns>
        public static void Describe(AvTraceBuilder traceBuilder, object o)
        {
            if (o == null)
            {
                traceBuilder.Append("null");
            }

            else if (o is BindingExpression)
            {
                BindingExpression bindingExpr = o as BindingExpression;

                Describe(traceBuilder, bindingExpr.ParentBinding);
                traceBuilder.Append("; DataItem=");
                DescribeSourceObject(traceBuilder, bindingExpr.DataItem);
                traceBuilder.Append("; ");
                DescribeTarget(traceBuilder, bindingExpr.TargetElement, bindingExpr.TargetProperty);
            }

            else if (o is Binding)
            {
                Binding binding = o as Binding;
                if (binding.Path != null)
                    traceBuilder.AppendFormat("Path={0}", binding.Path.Path );
                else if (binding.XPath != null)
                    traceBuilder.AppendFormat("XPath={0}", binding.XPath );
                else
                    traceBuilder.Append("(no path)");
            }

            else if (o is BindingExpressionBase)
            {
                BindingExpressionBase beb = o as BindingExpressionBase;
                DescribeTarget(traceBuilder, beb.TargetElement, beb.TargetProperty);
            }

            else if (o is DependencyObject)
            {
               DescribeSourceObject(traceBuilder, o);
            }

            else
            {
                traceBuilder.AppendFormat("'{0}'", AvTrace.ToStringHelper(o));
            }
        }

        /// <summary>
        /// Produces a string that describes a source object:
        /// e.g. element in a Binding Path, DataItem in BindingExpression, ContextElement
        /// </summary>
        /// <param name="traceBuilder">description will be appended to this builder</param>
        /// <param name="o">a source object (e.g. element in a Binding Path, DataItem in BindingExpression, ContextElement)</param>
        /// <returns>a string that describes the object</returns>
        public static void DescribeSourceObject(AvTraceBuilder traceBuilder, object o)
        {
            if (o == null)
            {
                traceBuilder.Append("null");
            }
            else
            {
                FrameworkElement fe = o as FrameworkElement;
                if (fe != null)
                {
                    traceBuilder.AppendFormat("'{0}' (Name='{1}')", fe.GetType().Name, fe.Name);
                }
                else
                {
                    traceBuilder.AppendFormat("'{0}' (HashCode={1})", o.GetType().Name, o.GetHashCode());
                }
            }
        }

        /// <summary>
        /// </summary>
        public static string DescribeSourceObject(object o)
        {
            AvTraceBuilder atb = new AvTraceBuilder(null);
            DescribeSourceObject(atb, o);
            return atb.ToString();
        }

        /// <summary>
        /// Produces a string that describes TargetElement and TargetProperty
        /// </summary>
        /// <param name="traceBuilder">description will be appended to this builder</param>
        /// <param name="targetElement">TargetElement</param>
        /// <param name="targetProperty">TargetProperty</param>
        /// <returns>a string that describes TargetElement and TargetProperty</returns>
        public static void DescribeTarget(AvTraceBuilder traceBuilder, DependencyObject targetElement, DependencyProperty targetProperty)
        {
            if (targetElement != null)
            {
                traceBuilder.Append("target element is ");
                DescribeSourceObject(traceBuilder, targetElement);
                if (targetProperty != null)
                {
                    traceBuilder.Append("; ");
                }
            }

            if (targetProperty != null)
            {
                traceBuilder.AppendFormat("target property is '{0}' (type '{1}')", targetProperty.Name, targetProperty.PropertyType.Name);
            }
        }

        /// <summary>
        /// </summary>
        public static string DescribeTarget(DependencyObject targetElement, DependencyProperty targetProperty)
        {
            AvTraceBuilder atb = new AvTraceBuilder(null);
            DescribeTarget(atb, targetElement, targetProperty);
            return atb.ToString();
        }

        public static string Identify(object o)
        {
            if (o == null)
                return "<null>";

            Type type = o.GetType();

            if (type.IsPrimitive || type.IsEnum)
            {
                return string.Create(TypeConverterHelper.InvariantEnglishUS, $"'{o}'");
            }

            return o switch
            {
                string s => $"'{AvTrace.AntiFormat(s)}'",
                NamedObject n => AvTrace.AntiFormat(n.ToString()),
                ICollection ic => string.Create(TypeConverterHelper.InvariantEnglishUS, $"{type.Name} (hash={AvTrace.GetHashCodeHelper(o)} Count={ic.Count})"),
                _ => string.Create(TypeConverterHelper.InvariantEnglishUS, $"{type.Name} (hash={AvTrace.GetHashCodeHelper(o)})")
            };
        }

        public static string IdentifyWeakEvent(Type type)
        {
            const string suffix = "EventManager";
            string name = type.Name;
            if (name.EndsWith(suffix, StringComparison.Ordinal))
            {
                name = name.Substring(0, name.Length - suffix.Length);
            }

            return name;
        }

        public static string IdentifyAccessor(object accessor)
        {
            return accessor switch
            {
                DependencyProperty dp => $"{dp.GetType().Name}({dp.Name})",
                PropertyInfo pi => $"{pi.GetType().Name}({pi.Name})",
                PropertyDescriptor pd => $"{pd.GetType().Name}({pd.Name})",
                _ => Identify(accessor)
            };
        }

        public static string IdentifyException(Exception ex)
        {
            if (ex == null)
                return "<no error>";

            return $"{ex.GetType().Name} ({AvTrace.AntiFormat(ex.Message)})";
        }

        /// <summary>
        /// Writes trace output for a binding failure plus triggers the event <see cref="BindingDiagnostics.BindingFailed"/>.
        /// The event will not be triggered if the TraceEventType is filtered out.
        /// </summary>
        /// <param name="binding">The binding is used as a trace parameter, so it get appended to the end of the trace message.</param>
        /// <param name="exception">If not null, used as both a trace and event parameter.</param>
        public static void TraceAndNotify(TraceEventType eventType, AvTraceDetails traceDetails, BindingExpressionBase binding, Exception exception = null)
        {
            object[] traceParameters = (exception != null) ? new object[] { binding, exception } : new object[] { binding };
            string traceOutput = _avTrace.Trace(eventType, traceDetails.Id, traceDetails.Message, traceDetails.Labels, traceParameters);

            if (traceOutput != null && BindingDiagnostics.IsEnabled)
            {
                object[] eventParameters = (exception != null) ? new object[] { exception } : null;
                BindingDiagnostics.NotifyBindingFailed(new BindingFailedEventArgs(eventType, traceDetails.Id, traceOutput, binding, eventParameters));
            }
        }

        /// <summary>
        /// Writes trace output for a data failure (with no BindingExpression context available) plus triggers the event <see cref="BindingDiagnostics.BindingFailed"/>.
        // The event will not be triggered if the TraceEventType is filtered out.
        /// </summary>
        /// <param name="exception">If not null, used as both a trace and event parameter.</param>
        public static void TraceAndNotify(TraceEventType eventType, AvTraceDetails traceDetails, Exception exception = null)
        {
            object[] parameters = (exception != null) ? new object[] { exception } : null;
            TraceData.TraceAndNotify(eventType, traceDetails, null, parameters, parameters);
        }

        /// <summary>
        /// Writes trace output for a binding failure plus triggers the event <see cref="BindingDiagnostics.BindingFailed"/>.
        /// The event will not be triggered if the TraceEventType is filtered out. This overload allows specific trace and event parameters to be included.
        /// </summary>
        /// <param name="binding">The binding is only part of the event, not the trace message.</param>
        public static void TraceAndNotify(TraceEventType eventType, AvTraceDetails traceDetails, BindingExpressionBase binding, object[] traceParameters, object[] eventParameters = null)
        {
            string traceOutput = _avTrace.Trace(eventType, traceDetails.Id, traceDetails.Message, traceDetails.Labels, traceParameters);

            if (traceOutput != null && BindingDiagnostics.IsEnabled)
            {
                BindingDiagnostics.NotifyBindingFailed(new BindingFailedEventArgs(eventType, traceDetails.Id, traceOutput, binding, eventParameters));
            }
        }

        /// <summary>
        /// Writes trace output for a binding failure plus triggers the event <see cref="BindingDiagnostics.BindingFailed"/>.
        /// The event will not be triggered if the TraceEventType is filtered out. No extra parameters are sent to the trace message or the event.
        /// </summary>
        /// <param name="binding">The binding is only included in the the event, not the trace message.</param>
        public static void TraceAndNotifyWithNoParameters(TraceEventType eventType, AvTraceDetails traceDetails, BindingExpressionBase binding)
        {
            TraceData.TraceAndNotify(eventType, traceDetails, binding, null, null);
        }
    }
}
