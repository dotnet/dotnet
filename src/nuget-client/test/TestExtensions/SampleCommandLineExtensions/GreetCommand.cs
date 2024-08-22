// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.ComponentModel.Composition;
using NuGet;
using NuGet.CommandLine;

namespace SampleCommandLineExtensions
{
    [Export]
    [Command("greet", "Prints greetings")]
    [DeprecatedCommand(typeof(HelloCommand))]
    public class GreetCommand : Command
    {
        public override void ExecuteCommand()
        {
            Console.WriteLine("Greetings");
        }
    }
}
