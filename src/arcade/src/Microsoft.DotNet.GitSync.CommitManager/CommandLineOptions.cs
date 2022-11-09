// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CommandLine;

namespace Microsoft.DotNet.GitSync.CommitManager
{
    internal class CommandLineOptions
    {
        [Option('k', "azureKey", Required = true, HelpText = "Azure Account Key")]
        public string Key { get; set; }

        [Option('u', "azureAccount", Required = true, HelpText = "Azure Account Name")]
        public string Username { get; set; }

        [Option('r', "repo", Required = true, HelpText = "Repo to which commit was made")]
        public string Repository { get; set; }

        [Option('b', "branch", Required = true, HelpText = "Branch to Mirror")]
        public string Branch { get; set; }

        [Option('c', "commits", Required = true, HelpText = "Sha of commit(s) concatenated by ;")]
        public string Commit { get; set; }
    }
}
