// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.CommandLine;
using System.Threading.Tasks;
using NuGet.Commands;

namespace NuGet.CommandLine.XPlat.Commands.NuGet.Update
{
    internal static class DotnetNuGetUpdateCommand
    {
        internal static void Register(Command parent, Func<ILoggerWithColor> getLogger)
        {
            var updateCmd = new Command("update", Strings.Update_Description);

            RegisterUpdateSource(updateCmd, getLogger);
            RegisterUpdateClientCert(updateCmd, getLogger);

            parent.Subcommands.Add(updateCmd);
        }

        private static void RegisterUpdateSource(Command parent, Func<ILoggerWithColor> getLogger)
        {
            var sourceCmd = new Command("source", Strings.UpdateSourceCommandDescription);

            var nameArg = new Argument<string>("name") { Description = Strings.SourcesCommandNameDescription };
            var source = new Option<string>("--source", "-s") { Description = Strings.SourcesCommandSourceDescription };
            var username = new Option<string>("--username", "-u") { Description = Strings.SourcesCommandUsernameDescription };
            var password = new Option<string>("--password", "-p") { Description = Strings.SourcesCommandPasswordDescription };
            var storePasswordInClearText = new Option<bool>("--store-password-in-clear-text") { Description = Strings.SourcesCommandStorePasswordInClearTextDescription };
            var validAuthenticationTypes = new Option<string>("--valid-authentication-types") { Description = Strings.SourcesCommandValidAuthenticationTypesDescription };
            var protocolVersion = new Option<string>("--protocol-version") { Description = Strings.SourcesCommandProtocolVersionDescription };
            var configfile = new Option<string>("--configfile") { Description = Strings.Option_ConfigFile };
            var allowInsecureConnections = new Option<bool>("--allow-insecure-connections") { Description = Strings.SourcesCommandAllowInsecureConnectionsDescription };

            sourceCmd.Arguments.Add(nameArg);
            sourceCmd.Options.Add(source);
            sourceCmd.Options.Add(username);
            sourceCmd.Options.Add(password);
            sourceCmd.Options.Add(storePasswordInClearText);
            sourceCmd.Options.Add(validAuthenticationTypes);
            sourceCmd.Options.Add(protocolVersion);
            sourceCmd.Options.Add(configfile);
            sourceCmd.Options.Add(allowInsecureConnections);

            sourceCmd.SetAction((parseResult, cancellationToken) =>
            {
                var args = new UpdateSourceArgs()
                {
                    Name = parseResult.GetValue(nameArg),
                    Source = parseResult.GetValue(source),
                    Username = parseResult.GetValue(username),
                    Password = parseResult.GetValue(password),
                    StorePasswordInClearText = parseResult.GetValue(storePasswordInClearText),
                    ValidAuthenticationTypes = parseResult.GetValue(validAuthenticationTypes),
                    ProtocolVersion = parseResult.GetValue(protocolVersion),
                    Configfile = parseResult.GetValue(configfile),
                    AllowInsecureConnections = parseResult.GetValue(allowInsecureConnections),
                };

                UpdateSourceRunner.Run(args, () => getLogger());
                return Task.FromResult(0);
            });

            parent.Subcommands.Add(sourceCmd);
        }

        private static void RegisterUpdateClientCert(Command parent, Func<ILoggerWithColor> getLogger)
        {
            var clientCertCmd = new Command("client-cert", Strings.UpdateClientCertCommandDescription);

            var packagesource = new Option<string>("--package-source", "-s") { Description = Strings.Option_PackageSource };
            var path = new Option<string>("--path") { Description = Strings.Option_Path };
            var password = new Option<string>("--password") { Description = Strings.Option_Password };
            var storePasswordInClearText = new Option<bool>("--store-password-in-clear-text") { Description = Strings.Option_StorePasswordInClearText };
            var storeLocation = new Option<string>("--store-location") { Description = Strings.Option_StoreLocation };
            var storeName = new Option<string>("--store-name") { Description = Strings.Option_StoreName };
            var findBy = new Option<string>("--find-by") { Description = Strings.Option_FindBy };
            var findValue = new Option<string>("--find-value") { Description = Strings.Option_FindValue };
            var force = new Option<bool>("--force", "-f") { Description = Strings.Option_Force };
            var configfile = new Option<string>("--configfile") { Description = Strings.Option_ConfigFile };

            clientCertCmd.Options.Add(packagesource);
            clientCertCmd.Options.Add(path);
            clientCertCmd.Options.Add(password);
            clientCertCmd.Options.Add(storePasswordInClearText);
            clientCertCmd.Options.Add(storeLocation);
            clientCertCmd.Options.Add(storeName);
            clientCertCmd.Options.Add(findBy);
            clientCertCmd.Options.Add(findValue);
            clientCertCmd.Options.Add(force);
            clientCertCmd.Options.Add(configfile);

            clientCertCmd.SetAction((parseResult, cancellationToken) =>
            {
                var args = new UpdateClientCertArgs()
                {
                    PackageSource = parseResult.GetValue(packagesource),
                    Path = parseResult.GetValue(path),
                    Password = parseResult.GetValue(password),
                    StorePasswordInClearText = parseResult.GetValue(storePasswordInClearText),
                    StoreLocation = parseResult.GetValue(storeLocation),
                    StoreName = parseResult.GetValue(storeName),
                    FindBy = parseResult.GetValue(findBy),
                    FindValue = parseResult.GetValue(findValue),
                    Force = parseResult.GetValue(force),
                    Configfile = parseResult.GetValue(configfile),
                };

                UpdateClientCertRunner.Run(args, () => getLogger());
                return Task.FromResult(0);
            });

            parent.Subcommands.Add(clientCertCmd);
        }
    }
}
