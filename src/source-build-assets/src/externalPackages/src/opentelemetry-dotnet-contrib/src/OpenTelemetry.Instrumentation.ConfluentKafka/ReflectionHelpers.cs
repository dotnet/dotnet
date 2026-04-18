// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

namespace OpenTelemetry.Instrumentation.ConfluentKafka;

internal static class ReflectionHelpers
{
    public static void SetProperty<T>(T instance, string fieldName, object? value)
    {
        var property = typeof(T).GetProperty(fieldName, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        if (property == null)
        {
            throw new InvalidOperationException($"Could not find property '{fieldName}' on type '{typeof(T).FullName}'.");
        }

        property.SetValue(instance, value);
    }

    public static object? GetProperty<T>(T instance, string fieldName)
    {
        var property = typeof(T).GetProperty(fieldName, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        if (property == null)
        {
            throw new InvalidOperationException($"Could not find property '{fieldName}' on type '{typeof(T).FullName}'.");
        }

        return property.GetValue(instance);
    }

    public static void SetField<T>(T instance, string fieldName, object? value)
    {
        var field = typeof(T).GetField(fieldName, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        if (field == null)
        {
            throw new InvalidOperationException($"Could not find field '{fieldName}' on type '{typeof(T).FullName}'.");
        }

        field.SetValue(instance, value);
    }

    public static object? GetField<T>(T instance, string fieldName)
    {
        var field = typeof(T).GetField(fieldName, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        if (field == null)
        {
            throw new InvalidOperationException($"Could not find field '{fieldName}' on type '{typeof(T).FullName}'.");
        }

        return field.GetValue(instance);
    }
}
