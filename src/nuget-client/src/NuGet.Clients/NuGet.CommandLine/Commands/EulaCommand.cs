// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace NuGet.CommandLine.Commands
{
    // The Command abstract class automatically adds some unwanted arguments, like -ConfigFile.
    // So this simple command will implement the interface directly instead.
    [Command(typeof(NuGetCommand), "eula", "EulaDescription")]
    internal class EulaCommand : ICommand
    {
        public string CurrentDirectory { get; set; }

        public CommandAttribute CommandAttribute => GetType().GetCustomAttribute<CommandAttribute>();

        public DeprecatedCommandAttribute DeprecatedCommandAttribute => null;

        public IList<string> Arguments => Array.Empty<string>();

        public void Execute()
        {
            Execute(System.Console.Out);
        }

        internal static void Execute(TextWriter writer)
        {
            using Stream resource = typeof(EulaCommand).Assembly.GetManifestResourceStream("NuGet.CommandLine.LICENSE.txt");
            using StreamReader reader = new StreamReader(resource);

            string line;
            while ((line = reader.ReadLine()) != null)
            {
                writer.WriteLine(line);
            }
        }

        public bool IncludedInHelp(string optionName) => true;
    }
}
