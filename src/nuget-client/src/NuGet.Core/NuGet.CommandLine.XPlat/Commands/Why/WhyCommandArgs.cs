// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections.Generic;
using System.Threading;

namespace NuGet.CommandLine.XPlat.Commands.Why
{
    internal class WhyCommandArgs
    {
        public string Path { get; }
        public string Package { get; }
        public List<string> Frameworks { get; }
        public ILoggerWithColor Logger { get; }
        public CancellationToken CancellationToken { get; }

        /// <summary>
        /// A constructor for the arguments of the 'why' command.
        /// </summary>
        /// <param name="path">The path to the solution or project file.</param>
        /// <param name="package">The package for which we show the dependency graphs.</param>
        /// <param name="frameworks">The target framework(s) for which we show the dependency graphs.</param>
        /// <param name="logger"></param>
        /// <param name="cancellationToken"></param>
        public WhyCommandArgs(
            string path,
            string package,
            List<string> frameworks,
            ILoggerWithColor logger,
            CancellationToken cancellationToken)
        {
            Path = path ?? throw new ArgumentNullException(nameof(path));
            Package = package ?? throw new ArgumentNullException(nameof(package));
            Frameworks = frameworks ?? throw new ArgumentNullException(nameof(frameworks));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            CancellationToken = cancellationToken;
        }
    }
}
