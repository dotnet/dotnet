//-----------------------------------------------------------------------
// <copyright file="EventSourceTests.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#define DEBUG
namespace Microsoft.ApplicationInsights.AspNetCore.Tests.Extensibility.Implementation.Tracing
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Tracing;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Event source testing helper methods.
    /// </summary>
    internal static class EventSourceTests
    {
        /// <summary>
        /// Tests event source method implementation consistency.
        /// </summary>
        /// <param name="eventSource">The event source instance to test.</param>
        public static void MethodsAreImplementedConsistentlyWithTheirAttributes(EventSource eventSource)
        {
            foreach (MethodInfo publicMethod in GetEventMethods(eventSource))
            {
                VerifyMethodImplementation(eventSource, publicMethod);
            }
        }

        /// <summary>
        /// Verifies the implementation of an event source method.
        /// </summary>
        /// <param name="eventSource">The event source instance to test.</param>
        /// <param name="eventMethod">The method to verify.</param>
        private static void VerifyMethodImplementation(EventSource eventSource, MethodInfo eventMethod)
        {
            using (var listener = new TestEventListener())
            {
                const long AllKeywords = -1;
                listener.EnableEvents(eventSource, EventLevel.Verbose, (EventKeywords)AllKeywords);
                try
                {
                    object[] eventArguments = GenerateEventArguments(eventMethod);
                    eventMethod.Invoke(eventSource, eventArguments);

                    EventWrittenEventArgs actualEvent = listener.Messages.First();
                    VerifyEventId(eventMethod, actualEvent);
                    VerifyEventLevel(eventMethod, actualEvent);
                    VerifyEventMessage(eventMethod, actualEvent, eventArguments);
                    VerifyEventApplicationName(eventMethod, actualEvent);
                }
                catch (Exception e)
                {
                    throw new Exception(eventMethod.Name + " is implemented incorrectly: " + e.Message, e);
                }
            }
        }

        /// <summary>
        /// Generates arguments for an event method.
        /// </summary>
        /// <param name="eventMethod">The method to generate arguments for.</param>
        /// <returns>The generated arguments.</returns>
        private static object[] GenerateEventArguments(MethodInfo eventMethod)
        {
            ParameterInfo[] parameters = eventMethod.GetParameters();
            var arguments = new object[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                arguments[i] = GenerateArgument(parameters[i]);
            }

            return arguments;
        }

        /// <summary>
        /// Generates an event method argument for the given parameter info instance.
        /// </summary>
        /// <param name="parameter">The parameter to generate and argument for.</param>
        /// <returns>The argument value.</returns>
        private static object GenerateArgument(ParameterInfo parameter)
        {
            if (parameter.ParameterType == typeof(string))
            {
                return "Test String";
            }

            Type parameterType = parameter.ParameterType;
            if (parameterType.GetTypeInfo().IsValueType)
            {
                return Activator.CreateInstance(parameter.ParameterType);
            }

            throw new NotSupportedException("Complex types are not suppored");
        }

        /// <summary>
        /// Verifies that the event Id is correct.
        /// </summary>
        /// <param name="eventMethod">The method to validate.</param>
        /// <param name="actualEvent">An actual event arguments to compare to.</param>
        private static void VerifyEventId(MethodInfo eventMethod, EventWrittenEventArgs actualEvent)
        {
            int expectedEventId = GetEventAttribute(eventMethod).EventId;
            AssertEqual(expectedEventId, actualEvent.EventId, "EventId");
        }

        /// <summary>
        /// Verifies that the event level is correct.
        /// </summary>
        /// <param name="eventMethod">The method to validate.</param>
        /// <param name="actualEvent">An actual event arguments to compare to.</param>
        private static void VerifyEventLevel(MethodInfo eventMethod, EventWrittenEventArgs actualEvent)
        {
            EventLevel expectedLevel = GetEventAttribute(eventMethod).Level;
            AssertEqual(expectedLevel, actualEvent.Level, "Level");
        }

        /// <summary>
        /// Verifies that the event message is correct.
        /// </summary>
        /// <param name="eventMethod">The method to validate.</param>
        /// <param name="actualEvent">An actual event arguments to compare to.</param>
        /// <param name="eventArguments">The arguments that would be passed to the event method.</param>
        private static void VerifyEventMessage(MethodInfo eventMethod, EventWrittenEventArgs actualEvent, object[] eventArguments)
        {
            string expectedMessage = eventArguments.Length == 0
                ? GetEventAttribute(eventMethod).Message
                : string.Format(CultureInfo.InvariantCulture, GetEventAttribute(eventMethod).Message, eventArguments);
            string actualMessage = string.Format(CultureInfo.InvariantCulture, actualEvent.Message, actualEvent.Payload.ToArray());
            AssertEqual(expectedMessage, actualMessage, "Message");
        }

        /// <summary>
        /// Verifies that the application name is correct.
        /// </summary>
        /// <param name="eventMethod">The method to validate.</param>
        /// <param name="actualEvent">An actual event arguments to compare to.</param>
        private static void VerifyEventApplicationName(MethodInfo eventMethod, EventWrittenEventArgs actualEvent)
        {
            string expectedApplicationName;
            try
            {
                expectedApplicationName = System.Reflection.Assembly.GetEntryAssembly().GetName().Name;
            }
            catch (Exception exp)
            {
                expectedApplicationName = "Undefined " + exp.Message;
            }

            string actualApplicationName = actualEvent.Payload.Last().ToString();
            AssertEqual(expectedApplicationName, actualApplicationName, "Application Name");
        }

        /// <summary>
        /// Dependency free equality assertion helper.
        /// </summary>
        /// <typeparam name="T">The type of the instances being compared.</typeparam>
        /// <param name="expected">The expected value.</param>
        /// <param name="actual">The actual value.</param>
        /// <param name="message">Message to show when the values are not equal.</param>
        private static void AssertEqual<T>(T expected, T actual, string message)
        {
            if (!expected.Equals(actual))
            {
                string errorMessage = string.Format(
                    CultureInfo.InvariantCulture,
                    "{0}. expected: '{1}', actual: '{2}'",
                    message,
                    expected,
                    actual);
                throw new Exception(errorMessage);
            }
        }

        /// <summary>
        /// Gets the event attribute from a given method.
        /// </summary>
        /// <param name="eventMethod">The method being tested.</param>
        /// <returns>The event attribute on the method.</returns>
        private static EventAttribute GetEventAttribute(MethodInfo eventMethod)
        {
            return (EventAttribute)eventMethod.GetCustomAttributes(typeof(EventAttribute), false).Single();
        }

        /// <summary>
        /// Gets the event methods in an event source.
        /// </summary>
        /// <param name="eventSource">The event source to get the event methods in.</param>
        /// <returns>The event methods in the specified event source.</returns>
        private static IEnumerable<MethodInfo> GetEventMethods(EventSource eventSource)
        {
            MethodInfo[] methods = eventSource.GetType().GetMethods();
            return methods.Where(m => m.GetCustomAttributes(typeof(EventAttribute), false).Any());
        }
    }
}