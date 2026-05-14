// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

extern alias CoreV2;

using NuGet.Protocol.Core.Types;

namespace NuGet.CommandLine
{
    public class CommandLineRepositoryFactory : CoreV2.NuGet.PackageRepositoryFactory
    {
        private readonly IConsole _console;

        public CommandLineRepositoryFactory(IConsole console)
        {
            _console = console;
        }

        public override CoreV2.NuGet.IPackageRepository CreateRepository(string packageSource)
        {
            var repository = base.CreateRepository(packageSource);
            var httpClientEvents = repository as CoreV2.NuGet.IHttpClientEvents;
            if (httpClientEvents != null)
            {
                httpClientEvents.SendingRequest += (sender, args) =>
                {
                    if (sender != httpClientEvents)
                    {
                        return;
                    }

                    if (_console.Verbosity == Verbosity.Detailed)
                    {
                        _console.WriteLine(
                            System.ConsoleColor.Green,
                            "{0} {1}", args.Request.Method, args.Request.RequestUri);
                    }

                    var userAgentString = new UserAgentStringBuilder(CommandLineConstants.UserAgent).Build();
                    CoreV2.NuGet.HttpUtility.SetUserAgent(args.Request, userAgentString);
                };
            }

            return repository;
        }
    }
}
