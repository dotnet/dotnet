#define DEBUG

namespace Microsoft.ApplicationInsights.Web.TestFramework
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
                    var name = eventMethod.DeclaringType.Name + "." + eventMethod.Name;

                    throw new Exception("Method '" + name + "' is implemented incorrectly.", e);
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

            if (parameter.ParameterType.IsValueType)
            {
                return Activator.CreateInstance(parameter.ParameterType);
            }

            throw new NotSupportedException("Complex types are not supported");
        }

        private static void VerifyEventId(MethodInfo eventMethod, EventWrittenEventArgs actualEvent)
        {
            int expectedEventId = GetEventAttribute(eventMethod).EventId;
            AssertEqual(nameof(VerifyEventId), expectedEventId, actualEvent.EventId);
        }

        private static void VerifyEventLevel(MethodInfo eventMethod, EventWrittenEventArgs actualEvent)
        {
            EventLevel expectedLevel = GetEventAttribute(eventMethod).Level;
            AssertEqual(nameof(VerifyEventLevel), expectedLevel, actualEvent.Level);
        }

        private static void VerifyEventMessage(MethodInfo eventMethod, EventWrittenEventArgs actualEvent, object[] eventArguments)
        {
            string expectedMessage = eventArguments.Length == 0
                ? GetEventAttribute(eventMethod).Message
                : string.Format(CultureInfo.InvariantCulture, GetEventAttribute(eventMethod).Message, eventArguments);
            string actualMessage = string.Format(CultureInfo.InvariantCulture, actualEvent.Message, actualEvent.Payload.ToArray());
            AssertEqual(nameof(VerifyEventMessage), expectedMessage, actualMessage);
        }

        private static void VerifyEventApplicationName(MethodInfo eventMethod, EventWrittenEventArgs actualEvent)
        {
            string expectedApplicationName = AppDomain.CurrentDomain.FriendlyName;
            string actualApplicationName = actualEvent.Payload.Last().ToString();
            AssertEqual(nameof(VerifyEventApplicationName), expectedApplicationName, actualApplicationName);
        }

        private static void AssertEqual<T>(string methodName, T expected, T actual)
        {
            if (!expected.Equals(actual))
            {
                var errorMessage = string.Format(
                    CultureInfo.InvariantCulture, 
                    "{0} Failed: expected: '{1}' actual: '{2}'", 
                    methodName, 
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
