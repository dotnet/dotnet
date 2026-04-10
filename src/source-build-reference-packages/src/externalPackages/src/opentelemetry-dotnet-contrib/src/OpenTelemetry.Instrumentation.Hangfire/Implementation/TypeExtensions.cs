// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;

namespace OpenTelemetry.Instrumentation.Hangfire.Implementation;

internal static class TypeExtensions
{
    private static readonly Regex GenericArgumentsRegex = new Regex(@"`[1-9]\d*", RegexOptions.Compiled, TimeSpan.FromSeconds(5));

    public static string ToGenericTypeString(this Type type)
    {
        if (!type.GetTypeInfo().IsGenericType)
        {
            return type.GetFullNameWithoutNamespace()
                    .ReplacePlusWithDotInNestedTypeName();
        }

        return type.GetGenericTypeDefinition()
                .GetFullNameWithoutNamespace()
                .ReplacePlusWithDotInNestedTypeName()
                .ReplaceGenericParametersInGenericTypeName(type);
    }

    public static Type[] GetAllGenericArguments(this TypeInfo type)
    {
        return type.GenericTypeArguments.Length > 0 ? type.GenericTypeArguments : type.GenericTypeParameters;
    }

    private static string GetFullNameWithoutNamespace(this Type type)
    {
        if (type.IsGenericParameter)
        {
            return type.Name;
        }

        const int dotLength = 1;

        // ReSharper disable once PossibleNullReferenceException
        return !string.IsNullOrEmpty(type.Namespace)
            ? type.FullName.Substring(type.Namespace.Length + dotLength)
            : type.FullName;
    }

    private static string ReplacePlusWithDotInNestedTypeName(this string typeName)
    {
        return typeName.Replace('+', '.');
    }

    private static string ReplaceGenericParametersInGenericTypeName(this string typeName, Type type)
    {
        var genericArguments = type.GetTypeInfo().GetAllGenericArguments();

        typeName = GenericArgumentsRegex.Replace(typeName, match =>
        {
            var currentGenericArgumentNumbers = int.Parse(match.Value.Substring(1), CultureInfo.InvariantCulture);
            var currentArguments = string.Join(",", genericArguments.Take(currentGenericArgumentNumbers).Select(ToGenericTypeString));
            genericArguments = genericArguments.Skip(currentGenericArgumentNumbers).ToArray();
            return string.Concat("<", currentArguments, ">");
        });

        return typeName;
    }
}
