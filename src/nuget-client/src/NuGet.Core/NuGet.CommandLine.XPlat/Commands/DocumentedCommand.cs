// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.CommandLine;

namespace NuGet.CommandLine.XPlat.Commands
{
    public class DocumentedCommand : CliCommand
    {
        public DocumentedCommand(string name, string description, string helpUrl)
            : base(name, description)
        {
            HelpUrl = helpUrl;
        }

        public string HelpUrl { get; }
    }
}
