// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#if NET || NETSTANDARD2_1_OR_GREATER
using System.Diagnostics.CodeAnalysis;
#endif
using System.Globalization;

namespace Microsoft.Extensions.Configuration;

internal static class OpenTelemetryConfigurationExtensions
{
    public delegate bool TryParseFunc<T>(
        string value,
#if NET || NETSTANDARD2_1_OR_GREATER
        [NotNullWhen(true)]
#endif
        out T? parsedValue);

    public static bool TryGetStringValue(
        this IConfiguration configuration,
        string key,
#if NET || NETSTANDARD2_1_OR_GREATER
        [NotNullWhen(true)]
#endif
        out string? value)
    {
        value = configuration[key];

        return !string.IsNullOrWhiteSpace(value);
    }

    public static bool TryGetUriValue(
        this IConfiguration configuration,
        IConfigurationExtensionsLogger logger,
        string key,
#if NET || NETSTANDARD2_1_OR_GREATER
        [NotNullWhen(true)]
#endif
        out Uri? value)
    {
        if (!configuration.TryGetStringValue(key, out var stringValue))
        {
            value = null;
            return false;
        }

        if (!Uri.TryCreate(stringValue, UriKind.Absolute, out value))
        {
#pragma warning disable IDE0370 // Suppression is unnecessary
            logger.LogInvalidConfigurationValue(key, stringValue!);
#pragma warning restore IDE0370 // Suppression is unnecessary
            return false;
        }

        return true;
    }

    public static bool TryGetIntValue(
        this IConfiguration configuration,
        IConfigurationExtensionsLogger logger,
        string key,
        out int value)
    {
        if (!configuration.TryGetStringValue(key, out var stringValue))
        {
            value = default;
            return false;
        }

        if (!int.TryParse(stringValue, NumberStyles.None, CultureInfo.InvariantCulture, out value))
        {
#pragma warning disable IDE0370 // Suppression is unnecessary
            logger.LogInvalidConfigurationValue(key, stringValue!);
#pragma warning restore IDE0370 // Suppression is unnecessary
            return false;
        }

        return true;
    }

    public static bool TryGetBoolValue(
        this IConfiguration configuration,
        IConfigurationExtensionsLogger logger,
        string key,
        out bool value)
    {
        if (!configuration.TryGetStringValue(key, out var stringValue))
        {
            value = default;
            return false;
        }

        if (!bool.TryParse(stringValue, out value))
        {
#pragma warning disable IDE0370 // Suppression is unnecessary
            logger.LogInvalidConfigurationValue(key, stringValue!);
#pragma warning restore IDE0370 // Suppression is unnecessary
            return false;
        }

        return true;
    }

    public static bool TryGetValue<T>(
        this IConfiguration configuration,
        IConfigurationExtensionsLogger logger,
        string key,
        TryParseFunc<T> tryParseFunc,
#if NET || NETSTANDARD2_1_OR_GREATER
        [NotNullWhen(true)]
#endif
        out T? value)
    {
        if (!configuration.TryGetStringValue(key, out var stringValue))
        {
            value = default;
            return false;
        }

#pragma warning disable IDE0370 // Suppression is unnecessary
        if (!tryParseFunc(stringValue!, out value))
        {
            logger.LogInvalidConfigurationValue(key, stringValue!);
            return false;
        }
#pragma warning restore IDE0370 // Suppression is unnecessary

        return true;
    }
}
