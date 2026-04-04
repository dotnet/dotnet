// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics.Tracing;
using System.Globalization;
using System.Reflection;

namespace OpenTelemetry.Tests;

internal static class EventSourceTestHelper
{
    public static void MethodsAreImplementedConsistentlyWithTheirAttributes(EventSource eventSource)
    {
        foreach (var publicMethod in GetEventMethods(eventSource))
        {
            VerifyMethodImplementation(eventSource, publicMethod);
        }
    }

    private static void VerifyMethodImplementation(EventSource eventSource, MethodInfo eventMethod)
    {
        using var listener = new TestEventListener();
        listener.EnableEvents(eventSource, EventLevel.Verbose, EventKeywords.All);
        try
        {
            var eventArguments = GenerateEventArguments(eventMethod);
            eventMethod.Invoke(eventSource, eventArguments);

            var actualEvent = listener.Messages.First(q => q.EventName == eventMethod.Name);

            VerifyEventId(eventMethod, actualEvent);
            VerifyEventLevel(eventMethod, actualEvent);

            if (eventMethod.Name != "ExporterErrorResult")
            {
                VerifyEventMessage(eventMethod, actualEvent, eventArguments);
            }
        }
        catch (Exception e)
        {
            var name = eventMethod.DeclaringType == null
                ? eventMethod.Name
                : eventMethod.DeclaringType.Name + "." + eventMethod.Name;

            throw new Exception("Method '" + name + "' is implemented incorrectly.", e);
        }
        finally
        {
            listener.ClearMessages();
        }
    }

    private static object[] GenerateEventArguments(MethodInfo eventMethod)
    {
        var parameters = eventMethod.GetParameters();
        var arguments = new object[parameters.Length];
        for (var i = 0; i < parameters.Length; i++)
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

        return parameter.ParameterType.IsValueType
            ? Activator.CreateInstance(parameter.ParameterType)
              ?? throw new NotSupportedException(
                  $"Could not create an instance of the '{parameter.ParameterType}' type.")
            : throw new NotSupportedException("Complex types are not supported");
    }

    private static void VerifyEventId(MethodInfo eventMethod, EventWrittenEventArgs actualEvent)
    {
        var expectedEventId = GetEventAttribute(eventMethod).EventId;
        AssertEqual(nameof(VerifyEventId), expectedEventId, actualEvent.EventId);
    }

    private static void VerifyEventLevel(MethodInfo eventMethod, EventWrittenEventArgs actualEvent)
    {
        var expectedLevel = GetEventAttribute(eventMethod).Level;
        AssertEqual(nameof(VerifyEventLevel), expectedLevel, actualEvent.Level);
    }

    private static void VerifyEventMessage(MethodInfo eventMethod, EventWrittenEventArgs actualEvent, object[] eventArguments)
    {
        var expectedMessage = eventArguments.Length == 0
            ? GetEventAttribute(eventMethod).Message!
            : string.Format(CultureInfo.InvariantCulture, GetEventAttribute(eventMethod).Message!, eventArguments);

        var actualMessage = string.Format(
            CultureInfo.InvariantCulture,
            actualEvent.Message!,
            actualEvent.Payload?.ToArray() ?? []);

        AssertEqual(nameof(VerifyEventMessage), expectedMessage, actualMessage);
    }

    private static void AssertEqual<T>(string methodName, T expected, T actual)
        where T : notnull
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
        var methods = eventSource.GetType().GetMethods();
        return methods.Where(m => m.GetCustomAttributes(typeof(EventAttribute), false).Length != 0);
    }
}
