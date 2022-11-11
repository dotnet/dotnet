// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;

namespace Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;

internal interface IEnvironmentVariableHelper
{
    /// <summary>
    /// Retrieves the value of an environment variable from the current process.
    /// </summary>
    /// <param name="variable">The name of the environment variable.</param>
    /// <returns>The value of the environment variable specified by variable, or null if the environment variable is not found.</returns>
    string? GetEnvironmentVariable(string variable);

    /// <summary>
    /// Retrieves the value of an environment variable from the current process and converts it to the given type.
    /// </summary>
    /// <typeparam name="TEnum">The type used for conversion.</typeparam>
    /// <param name="variable">The name of the environment variable.</param>
    /// <param name="defaultValue">The default value to return if the environment variable is not found.</param>
    /// <returns></returns>
    TEnum GetEnvironmentVariableAsEnum<TEnum>(string variable, TEnum defaultValue = default) where TEnum : struct, Enum;

    /// <summary>
    /// Creates, modifies, or deletes an environment variable stored in the current process.
    /// </summary>
    /// <param name="variable">The name of an environment variable.</param>
    /// <param name="value">A value to assign to variable.</param>
    void SetEnvironmentVariable(string variable, string value);

    /// <summary>
    /// Retrieves all environment variable names and their values from the current process.
    /// </summary>
    /// <returns>A dictionary that contains all environment variable names and their values; otherwise, an empty dictionary if no environment variables are found.</returns>
    IDictionary GetEnvironmentVariables();
}
