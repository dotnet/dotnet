// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.Linq;
using NuGet.Commands;

namespace NuGet.CommandLine
{
    [Command(typeof(NuGetCommand), "sources", "SourcesCommandDescription", UsageSummaryResourceName = "SourcesCommandUsageSummary",
        MinArgs = 0, MaxArgs = 1)]
    public class SourcesCommand : Command
    {
        [Option(typeof(NuGetCommand), "SourcesCommandNameDescription")]
        public string Name { get; set; }

        [Option(typeof(NuGetCommand), "SourcesCommandSourceDescription", AltName = "src")]
        public string Source { get; set; }

        [Option(typeof(NuGetCommand), "SourcesCommandUserNameDescription")]
        public string Username { get; set; }

        [Option(typeof(NuGetCommand), "SourcesCommandPasswordDescription")]
        public string Password { get; set; }

        [Option(typeof(NuGetCommand), "SourcesCommandStorePasswordInClearTextDescription")]
        public bool StorePasswordInClearText { get; set; }

        [Option(typeof(NuGetCommand), "SourcesCommandValidAuthenticationTypesDescription")]
        public string ValidAuthenticationTypes { get; set; }

        [Option(typeof(NuGetCommand), "SourcesCommandProtocolVersionDescription")]
        public string ProtocolVersion { get; set; }

        [Option(typeof(NuGetCommand), "SourcesCommandFormatDescription")]
        public SourcesListFormat Format { get; set; }


        public override void ExecuteCommand()
        {
            if (SourceProvider == null)
            {
                throw new InvalidOperationException(LocalizedResourceManager.GetString("Error_SourceProviderIsNull"));
            }

            SourcesAction action = SourcesAction.None;
            var actionArg = Arguments.FirstOrDefault();
            if (string.IsNullOrEmpty(actionArg))
            {
                action = SourcesAction.List;
            }
            else
            {
                if (!Enum.TryParse<SourcesAction>(actionArg, ignoreCase: true, out action))
                {
                    Console.WriteLine(LocalizedResourceManager.GetString("SourcesCommandUsageSummary"));
                }
            }

            switch (action)
            {
                case SourcesAction.Add:
                    var addArgs = new AddSourceArgs() { Name = Name, Source = Source, Username = Username, Password = Password, StorePasswordInClearText = StorePasswordInClearText, ValidAuthenticationTypes = ValidAuthenticationTypes, Configfile = ConfigFile, ProtocolVersion = ProtocolVersion };
                    AddSourceRunner.Run(addArgs, () => Console);
                    break;
                case SourcesAction.Update:
                    var updateSourceArgs = new UpdateSourceArgs() { Name = Name, Source = Source, Username = Username, Password = Password, StorePasswordInClearText = StorePasswordInClearText, ValidAuthenticationTypes = ValidAuthenticationTypes, Configfile = ConfigFile, ProtocolVersion = ProtocolVersion };
                    UpdateSourceRunner.Run(updateSourceArgs, () => Console);
                    break;
                case SourcesAction.Remove:
                    var removeSourceArgs = new RemoveSourceArgs() { Name = Name, Configfile = ConfigFile };
                    RemoveSourceRunner.Run(removeSourceArgs, () => Console);
                    break;
                case SourcesAction.Disable:
                    var disableSourceArgs = new DisableSourceArgs() { Name = Name, Configfile = ConfigFile };
                    DisableSourceRunner.Run(disableSourceArgs, () => Console);
                    break;
                case SourcesAction.Enable:
                    var enableSourceArgs = new EnableSourceArgs() { Name = Name, Configfile = ConfigFile };
                    EnableSourceRunner.Run(enableSourceArgs, () => Console);
                    break;
                case SourcesAction.List:
                    if (Format == SourcesListFormat.None)
                    {
                        Format = SourcesListFormat.Detailed;
                    }
                    var listSourceArgs = new ListSourceArgs() { Configfile = ConfigFile, Format = Format.ToString() };
                    ListSourceRunner.Run(listSourceArgs, () => Console);
                    break;
            }
        }
    }
}
