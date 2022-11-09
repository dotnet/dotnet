// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Microsoft.VisualStudio.TestPlatform.CoreUtilities.Helpers;

/// <summary>
/// Helper class for processing arguments passed to a process.
/// </summary>
public class CommandLineArgumentsHelper
{
    /// <summary>
    /// Parse command line arguments to a dictionary.
    /// </summary>
    /// <param name="args">Command line arguments. Ex: <c>{ "--port", "12312", "--parentprocessid", "2312", "--testsourcepath", "C:\temp\1.dll" }</c></param>
    /// <returns>Dictionary of arguments keys and values.</returns>
    public static IDictionary<string, string?> GetArgumentsDictionary(string[]? args)
    {
        var argsDictionary = new Dictionary<string, string?>();
        if (args == null)
        {
            return argsDictionary;
        }

        for (int i = 0; i < args.Length; i++)
        {
            if (args[i].StartsWith("-"))
            {
                if (i < args.Length - 1 && !args[i + 1].StartsWith("-"))
                {
                    argsDictionary.Add(args[i], args[i + 1]);
                    i++;
                }
                else
                {
                    argsDictionary.Add(args[i], null);
                }
            }
        }

        return argsDictionary;
    }

    /// <summary>
    /// Parse the value of an argument as an integer.
    /// </summary>
    /// <param name="argsDictionary">Dictionary of all arguments Ex: <c>{ "--port":"12312", "--parentprocessid":"2312" }</c></param>
    /// <param name="fullname">The full name for required argument. Ex: "--port"</param>
    /// <returns>Value of the argument.</returns>
    /// <exception cref="ArgumentException">Thrown if value of an argument is not an integer.</exception>
    public static int GetIntArgFromDict(IDictionary<string, string?> argsDictionary, string fullname)
    {
        var found = TryGetIntArgFromDict(argsDictionary, fullname, out var value);
        return found ? value : 0;
    }

    /// <summary>
    /// Try get the argument and parse the value of an argument as an integer.
    /// </summary>
    /// <param name="argsDictionary">Dictionary of all arguments Ex: <c>{ "--port":"12312", "--parentprocessid":"2312" }</c></param>
    /// <param name="fullname">The full name for required argument. Ex: "--port"</param>
    /// <returns>Value of the argument.</returns>
    /// <exception cref="ArgumentException">Thrown if value of an argument is not an integer.</exception>
    public static bool TryGetIntArgFromDict(IDictionary<string, string?> argsDictionary, string fullname, out int value)
    {
        var found = argsDictionary.TryGetValue(fullname, out var optionValue);
        if (!found)
        {
            value = default;
            return false;
        }

        return int.TryParse(optionValue, out value);
    }


    /// <summary>
    /// Parse the value of an argument as a string.
    /// </summary>
    /// <param name="argsDictionary">Dictionary of all arguments Ex: <c>{ "--port":"12312", "--parentprocessid":"2312" }</c></param>
    /// <param name="fullname">The full name for required argument. Ex: "--port"</param>
    /// <returns>Value of the argument.</returns>
    /// <exception cref="ArgumentException">Thrown if value of an argument is not an integer.</exception>
    public static string? GetStringArgFromDict(IDictionary<string, string?> argsDictionary, string fullname)
    {
        return argsDictionary.TryGetValue(fullname, out var optionValue) ? optionValue : string.Empty;
    }
}
