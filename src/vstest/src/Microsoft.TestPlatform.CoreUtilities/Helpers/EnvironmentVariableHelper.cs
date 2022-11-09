// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;

namespace Microsoft.VisualStudio.TestPlatform.CoreUtilities.Helpers;

internal class EnvironmentVariableHelper : IEnvironmentVariableHelper
{
    /// <inheritdoc />
    public string? GetEnvironmentVariable(string variable)
        => Environment.GetEnvironmentVariable(variable);

    /// <inheritdoc />
    public TEnum GetEnvironmentVariableAsEnum<TEnum>(string variable, TEnum defaultValue = default) where TEnum : struct, Enum
        => Environment.GetEnvironmentVariable(variable) is string value && !string.IsNullOrEmpty(value)
            ? Enum.TryParse<TEnum>(value, out var enumValue) ? enumValue : defaultValue
            : defaultValue;

    /// <inheritdoc />
    public void SetEnvironmentVariable(string variable, string value)
        => Environment.SetEnvironmentVariable(variable, value);
}
