// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.CommandLine;
using System.Threading.Tasks;
using NuGet.CommandLine.XPlat.Commands;
using NuGet.Common;

namespace NuGet.CommandLine.XPlat
{
    internal class ConfigCommand
    {
        internal static void LogException(Exception e, ILogger log)
        {
            // Log the error
            if (ExceptionLogger.Instance.ShowStack)
            {
                log.LogError(e.ToString());
            }
            else
            {
                log.LogError(ExceptionUtilities.DisplayMessage(e));
            }

            // Log the stack trace as verbose output.
            log.LogVerbose(e.ToString());
        }

        internal static Command Register(Command app, Func<ILoggerWithColor> getLogger)
        {
            var ConfigCmd = new DocumentedCommand(name: "config", description: Strings.Config_Description, "https://aka.ms/dotnet/nuget/config");

            // Options directly under the verb 'config'

            // noun sub-command: config paths
            var PathsCmd = new DocumentedCommand(name: "paths", description: Strings.ConfigPathsCommandDescription, "https://aka.ms/dotnet/nuget/config/paths");

            // Options under sub-command: config paths
            RegisterOptionsForCommandConfigPaths(PathsCmd, getLogger);

            ConfigCmd.Subcommands.Add(PathsCmd);

            // noun sub-command: config get
            var GetCmd = new DocumentedCommand(name: "get", description: Strings.ConfigGetCommandDescription, "https://aka.ms/dotnet/nuget/config/get");

            // Options under sub-command: config get
            RegisterOptionsForCommandConfigGet(GetCmd, getLogger);

            ConfigCmd.Subcommands.Add(GetCmd);

            // noun sub-command: config set
            var SetCmd = new DocumentedCommand(name: "set", description: Strings.ConfigSetCommandDescription, "https://aka.ms/dotnet/nuget/config/set");

            // Options under sub-command: config set
            RegisterOptionsForCommandConfigSet(SetCmd, getLogger);

            ConfigCmd.Subcommands.Add(SetCmd);

            // noun sub-command: config unset
            var UnsetCmd = new DocumentedCommand(name: "unset", description: Strings.ConfigUnsetCommandDescription, "https://aka.ms/dotnet/nuget/config/unset");

            // Options under sub-command: config unset
            RegisterOptionsForCommandConfigUnset(UnsetCmd, getLogger);

            ConfigCmd.Subcommands.Add(UnsetCmd);

            app.Subcommands.Add(ConfigCmd);

            return ConfigCmd;
        } // End noun method

        private static void RegisterOptionsForCommandConfigPaths(Command cmd, Func<ILogger> getLogger)
        {
            var workingDirectory = new Option<string>(name: "--working-directory")
            {
                Arity = ArgumentArity.ZeroOrOne,
                Description = Strings.ConfigPathsWorkingDirectoryDescription
            };

            cmd.Options.Add(workingDirectory);
            // Create handler delegate handler for cmd
            cmd.SetAction((parseResult, cancellationToken) =>
            {
                var args = new ConfigPathsArgs()
                {
                    WorkingDirectory = parseResult.GetValue(workingDirectory),
                };

                int exitCode = ConfigPathsRunner.Run(args, getLogger);
                return Task.FromResult(exitCode);
            });
        }

        private static void RegisterOptionsForCommandConfigGet(Command cmd, Func<ILogger> getLogger)
        {
            var allOrConfigKeyArgument = new Argument<string>(name: "all-or-config-key")
            {
                Arity = ArgumentArity.ExactlyOne,
                Description = Strings.ConfigGetAllOrConfigKeyDescription
            };

            var workingDirectory = new Option<string>(name: "--working-directory")
            {
                Arity = ArgumentArity.ZeroOrOne,
                Description = Strings.ConfigPathsWorkingDirectoryDescription
            };

            var showPathOption = new Option<bool>(name: "--show-path")
            {
                Arity = ArgumentArity.Zero,
                Description = Strings.ConfigGetShowPathDescription,
            };

            cmd.Arguments.Add(allOrConfigKeyArgument);
            cmd.Options.Add(workingDirectory);
            cmd.Options.Add(showPathOption);

            // Create handler delegate handler for cmd
            cmd.SetAction((parseResult, cancellationToken) =>
            {
                var args = new ConfigGetArgs()
                {
                    AllOrConfigKey = parseResult.GetValue(allOrConfigKeyArgument),
                    WorkingDirectory = parseResult.GetValue(workingDirectory),
                    ShowPath = parseResult.GetValue(showPathOption),
                };

                int exitCode = ConfigGetRunner.Run(args, getLogger);
                return Task.FromResult(exitCode);
            });
        }

        private static void RegisterOptionsForCommandConfigSet(Command cmd, Func<ILogger> getLogger)
        {
            var setConfigKeyArgument = new Argument<string>(name: "config-key")
            {
                Arity = ArgumentArity.ExactlyOne,
                Description = Strings.ConfigSetConfigKeyDescription,
            };

            var configValueArgument = new Argument<string>(name: "config-value")
            {
                Arity = ArgumentArity.ExactlyOne,
                Description = Strings.ConfigSetConfigValueDescription,
            };

            var configFileOption = new Option<string>(name: "--configfile")
            {
                Arity = ArgumentArity.ZeroOrOne,
                Description = Strings.Option_ConfigFile,
            };

            cmd.Arguments.Add(setConfigKeyArgument);
            cmd.Arguments.Add(configValueArgument);
            cmd.Options.Add(configFileOption);
            // Create handler delegate handler for cmd
            cmd.SetAction((parseResult, cancellationToken) =>
            {
                var args = new ConfigSetArgs()
                {
                    ConfigKey = parseResult.GetValue(setConfigKeyArgument),
                    ConfigValue = parseResult.GetValue(configValueArgument),
                    ConfigFile = parseResult.GetValue(configFileOption),
                };

                int exitCode = ConfigSetRunner.Run(args, getLogger);
                return Task.FromResult(exitCode);
            });
        }

        private static void RegisterOptionsForCommandConfigUnset(Command cmd, Func<ILogger> getLogger)
        {
            var unsetConfigKeyArgument = new Argument<string>(name: "config-key")
            {
                Arity = ArgumentArity.ExactlyOne,
                Description = Strings.ConfigUnsetConfigKeyDescription,
            };

            var configFileOption = new Option<string>(name: "--configfile")
            {
                Arity = ArgumentArity.ZeroOrOne,
                Description = Strings.Option_ConfigFile,
            };

            cmd.Arguments.Add(unsetConfigKeyArgument);
            cmd.Options.Add(configFileOption);
            // Create handler delegate handler for cmd
            cmd.SetAction((parseResult, cancellationToken) =>
            {
                var args = new ConfigUnsetArgs()
                {
                    ConfigKey = parseResult.GetValue(unsetConfigKeyArgument),
                    ConfigFile = parseResult.GetValue(configFileOption),
                };

                int exitCode = ConfigUnsetRunner.Run(args, getLogger);
                return Task.FromResult(exitCode);
            });
        }
    }
}
