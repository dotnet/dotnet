// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace Microsoft.VisualStudio.TestPlatform.Utilities;

public static class CommandLineUtilities
{
    public static bool SplitCommandLineIntoArguments(string args, out string[] arguments)
    {
        bool hadError = false;
        var argArray = new List<string>();
        var currentArg = new StringBuilder();
        bool inQuotes = false;
        int index = 0;

        try
        {
            while (index < args.Length)
            {
                // Skip whitespace.
                while (char.IsWhiteSpace(args[index]))
                {
                    index++;
                }

                // # - comment to end of line
                if (args[index] == '#')
                {
                    index++;
                    while (index < args.Length && args[index] != '\n')
                    {
                        index++;
                    }

                    // We are done processing comment move to next statement.
                    continue;
                }

                // Read argument until next whitespace (not in quotes).
                do
                {
                    if (args[index] == '\\')
                    {
                        // Move to next char.
                        index++;

                        // If this was the last char then output the slash.
                        if (index == args.Length)
                        {
                            currentArg.Append('\\');

                            index++;
                            continue;
                        }
                        else
                        {
                            // If the char after '\' is also a '\', output the second '\' and skip over to the next char.
                            if (args[index] == '\\')
                            {
                                currentArg.Append('\\');

                                // We processed the escaped \, move to next char.
                                index++;
                                continue;
                            }

                            // If the char after '\' is a '"', output '"' and skip over to the next char.
                            if (index <= args.Length && args[index] == '"')
                            {
                                currentArg.Append('"');

                                // We processed the escaped " move to next char.
                                index++;
                                continue;
                            }

                            // If the char after '\' is anything else, output the slash. And continue processing the next char.
                            if (index <= args.Length)
                            {
                                currentArg.Append('\\');

                                // Don't skip to the next char. We outputted the \ because it was not escaping \ or ". Let the next character to be processed by the loop.
                                // index++;
                                continue;
                            }
                        }
                    }
                    // Unescaped quote enters and leaves quoted mode.
                    else if (args[index] == '"')
                    {
                        inQuotes = !inQuotes;
                        index++;
                    }
                    else
                    {
                        // Collect all other characters.
                        currentArg.Append(args[index]);
                        index++;
                    }
                } while (!char.IsWhiteSpace(args[index]) || inQuotes);
                argArray.Add(currentArg.ToString());
                currentArg.Clear();
            }
        }
        catch (IndexOutOfRangeException)
        {
            // got EOF
            if (inQuotes)
            {
                EqtTrace.Verbose("Executor.Execute: Exiting with exit code of {0}", 1);
                EqtTrace.Error("Error: Unbalanced '\"' in command line argument file");
                hadError = true;
            }
            else if (currentArg.Length > 0)
            {
                // valid argument can be terminated by EOF
                argArray.Add(currentArg.ToString());
            }
        }

        arguments = argArray.ToArray();
        return hadError;
    }
}
