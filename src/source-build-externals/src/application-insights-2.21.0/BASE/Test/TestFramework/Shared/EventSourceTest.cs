#define DEBUG

namespace Microsoft.ApplicationInsights.TestFramework
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Tracing;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;

    internal static class EventSourceTest
    {
        public static void MethodsAreImplementedConsistentlyWithTheirAttributes(EventSource eventSource)
        {
            foreach (MethodInfo publicMethod in GetEventMethods(eventSource))
            {
                VerifyMethodImplementation(eventSource, publicMethod);
            }
        }

        private static void VerifyMethodImplementation(EventSource eventSource, MethodInfo eventMethod)
        {
            using (var listener = new Microsoft.ApplicationInsights.TestFramework.TestEventListener())
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

        private static object GenerateArgument(ParameterInfo parameter)
        {
            if (parameter.ParameterType == typeof(string))
            {
                return "Test String";
            }

            if (parameter.ParameterType.GetTypeInfo().IsValueType)
            {
                return Activator.CreateInstance(parameter.ParameterType);
            }

            throw new NotSupportedException("Complex types are not supported");
        }

        private static void VerifyEventId(MethodInfo eventMethod, EventWrittenEventArgs actualEvent)
        {
            int expectedEventId = GetEventAttribute(eventMethod).EventId;
            AssertEqual(expectedEventId, actualEvent.EventId, "EventId");
        }

        private static void VerifyEventLevel(MethodInfo eventMethod, EventWrittenEventArgs actualEvent)
        {
            EventLevel expectedLevel = GetEventAttribute(eventMethod).Level;
            AssertEqual(expectedLevel, actualEvent.Level, "Level");
        }

        private static void VerifyEventMessage(MethodInfo eventMethod, EventWrittenEventArgs actualEvent, object[] eventArguments)
        {
            string expectedMessage = eventArguments.Length == 0
                ? GetEventAttribute(eventMethod).Message
                : string.Format(CultureInfo.InvariantCulture, GetEventAttribute(eventMethod).Message, eventArguments);
            string actualMessage = string.Format(CultureInfo.InvariantCulture, actualEvent.Message, actualEvent.Payload.ToArray());
            AssertEqual(expectedMessage, actualMessage, "Message");
        }

        private static void VerifyEventApplicationName(MethodInfo eventMethod, EventWrittenEventArgs actualEvent)
        {
            string expectedApplicationName = AppDomain.CurrentDomain.FriendlyName;
            string actualApplicationName = actualEvent.Payload.Last().ToString();
            AssertEqual(expectedApplicationName, actualApplicationName, "Application Name");
        }

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

        private static EventAttribute GetEventAttribute(MethodInfo eventMethod)
        {
            return (EventAttribute)eventMethod.GetCustomAttributes(typeof(EventAttribute), false).Single();
        }

        private static IEnumerable<MethodInfo> GetEventMethods(EventSource eventSource)
        {
            MethodInfo[] methods = eventSource.GetType().GetMethods();

            return methods.Where(m => m.GetCustomAttributes(typeof(EventAttribute), false).Any());
        }
    }
}