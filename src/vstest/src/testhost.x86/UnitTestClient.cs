// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;

using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace Microsoft.VisualStudio.TestPlatform.TestExecutor;

/// <summary>
/// Initialization point for Old UWP application
/// </summary>
public static class UnitTestClient
{
    /// <summary>
    /// Create default UI for UWP app
    /// </summary>
    public static void CreateDefaultUI()
    {
    }

    /// <summary>
    /// Entry point for testhost, in App Model(UWP) scenario
    /// </summary>
    /// <param name="arguments">testhost initialization arguments</param>
    public static void Run(string arguments)
    {
        Task.Run(() => Program.Run(SplitArguments(arguments)));
    }

    /// <summary>
    /// Split Arguments on space, if they are not inside Single/Double Quotes
    /// </summary>
    /// <param name="commandLine"></param>
    /// <returns></returns>
    internal static string[] SplitArguments(string commandLine)
    {
        var parmChars = commandLine.ToCharArray();
        var inDoubleQuote = false;
        var inSingleQuote = false;

        for (var index = 0; index < parmChars.Length; index++)
        {
            if (parmChars[index] == '"' && !inSingleQuote)
            {
                inDoubleQuote = !inDoubleQuote;
            }

            if (parmChars[index] == '\'' && !inDoubleQuote)
            {
                inSingleQuote = !inSingleQuote;
            }

            if (!inSingleQuote && !inDoubleQuote && parmChars[index] == ' ')
                parmChars[index] = '\n';
        }

        return (new string(parmChars)).Split(new[] { '\n' });
    }
}
