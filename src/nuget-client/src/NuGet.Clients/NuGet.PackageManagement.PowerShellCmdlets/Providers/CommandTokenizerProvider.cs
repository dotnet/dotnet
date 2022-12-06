// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using NuGet.VisualStudio;
using NuGetConsole.Host.PowerShell.Implementation;

namespace NuGetConsole.Host
{
    [Export(typeof(ICommandTokenizerProvider))]
    [HostName(PowerShellHostProvider.HostName)]
    internal class CommandTokenizerProvider : ICommandTokenizerProvider
    {
        private readonly Lazy<CommandTokenizer> _instance = new Lazy<CommandTokenizer>(() => new CommandTokenizer());

        public ICommandTokenizer Create(IHost host)
        {
            return _instance.Value;
        }
    }
}
