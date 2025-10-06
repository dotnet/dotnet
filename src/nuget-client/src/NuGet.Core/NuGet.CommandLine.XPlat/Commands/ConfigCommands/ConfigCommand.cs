// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.CommandLine;
using System.CommandLine.Help;
using System.Threading.Tasks;
using Microsoft.Extensions.CommandLineUtils;
using NuGet.CommandLine.XPlat.Commands;
using NuGet.Common;

namespace NuGet.CommandLine.XPlat
{
    internal class ConfigCommand
    {
        private static HelpOption HelpOption = new HelpOption()
        {
            Arity = ArgumentArity.Zero
        };

        private static Argument<string> SetConfigKeyArgument = new Argument<string>(name: "config-key")
        {
            Arity = ArgumentArity.ExactlyOne,
            Description = Strings.ConfigSetConfigKeyDescription,
        };

        private static Argument<string> UnsetConfigKeyArgument = new Argument<string>(name: "config-key")
        {
            Arity = ArgumentArity.ExactlyOne,
            Description = Strings.ConfigUnsetConfigKeyDescription,
        };

        private static Argument<string> ConfigValueArgument = new Argument<string>(name: "config-value")
        {
            Arity = ArgumentArity.ExactlyOne,
            Description = Strings.ConfigSetConfigValueDescription,
        };

        private static Argument<string> AllOrConfigKeyArgument = new Argument<string>(name: "all-or-config-key")
        {
            Arity = ArgumentArity.ExactlyOne,
            Description = Strings.ConfigGetAllOrConfigKeyDescription
        };

        private static Option<string> WorkingDirectory = new Option<string>(name: "--working-directory")
        {
            Arity = ArgumentArity.ZeroOrOne,
            Description = Strings.ConfigPathsWorkingDirectoryDescription
        };

        private static Option<bool> ShowPathOption = new Option<bool>(name: "--show-path")
        {
            Arity = ArgumentArity.Zero,
            Description = Strings.ConfigGetShowPathDescription,
        };

        private static Option<string> ConfigFileOption = new Option<string>(name: "--configfile")
        {
            Arity = ArgumentArity.ZeroOrOne,
            Description = Strings.Option_ConfigFile,
        };

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

        internal static void Register(CommandLineApplication app)
        {
            app.Command("config", configCmd =>
            {
                configCmd.Description = Strings.Config_Description;
            });
        }

        internal static Command Register(Command app, Func<ILogger> getLogger)
        {
            var ConfigCmd = new DocumentedCommand(name: "config", description: Strings.Config_Description, "https://aka.ms/dotnet/nuget/config");
            ConfigCmd.Options.Add(HelpOption);

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
            cmd.Options.Add(WorkingDirectory);
            cmd.Options.Add(HelpOption);
            // Create handler delegate handler for cmd
            cmd.SetAction((parseResult, cancellationToken) =>
            {
                var args = new ConfigPathsArgs()
                {
                    WorkingDirectory = parseResult.GetValue(WorkingDirectory),
                };

                int exitCode = ConfigPathsRunner.Run(args, getLogger);
                return Task.FromResult(exitCode);
            });
        }

        private static void RegisterOptionsForCommandConfigGet(Command cmd, Func<ILogger> getLogger)
        {
            cmd.Arguments.Add(AllOrConfigKeyArgument);
            cmd.Options.Add(WorkingDirectory);
            cmd.Options.Add(ShowPathOption);
            cmd.Options.Add(HelpOption);

            // Create handler delegate handler for cmd
            cmd.SetAction((parseResult, cancellationToken) =>
            {
                var args = new ConfigGetArgs()
                {
                    AllOrConfigKey = parseResult.GetValue(AllOrConfigKeyArgument),
                    WorkingDirectory = parseResult.GetValue(WorkingDirectory),
                    ShowPath = parseResult.GetValue(ShowPathOption),
                };

                int exitCode = ConfigGetRunner.Run(args, getLogger);
                return Task.FromResult(exitCode);
            });
        }

        private static void RegisterOptionsForCommandConfigSet(Command cmd, Func<ILogger> getLogger)
        {
            cmd.Arguments.Add(SetConfigKeyArgument);
            cmd.Arguments.Add(ConfigValueArgument);
            cmd.Options.Add(ConfigFileOption);
            cmd.Options.Add(HelpOption);
            // Create handler delegate handler for cmd
            cmd.SetAction((parseResult, cancellationToken) =>
            {
                var args = new ConfigSetArgs()
                {
                    ConfigKey = parseResult.GetValue(SetConfigKeyArgument),
                    ConfigValue = parseResult.GetValue(ConfigValueArgument),
                    ConfigFile = parseResult.GetValue(ConfigFileOption),
                };

                int exitCode = ConfigSetRunner.Run(args, getLogger);
                return Task.FromResult(exitCode);
            });
        }

        private static void RegisterOptionsForCommandConfigUnset(Command cmd, Func<ILogger> getLogger)
        {
            cmd.Arguments.Add(UnsetConfigKeyArgument);
            cmd.Options.Add(ConfigFileOption);
            cmd.Options.Add(HelpOption);
            // Create handler delegate handler for cmd
            cmd.SetAction((parseResult, cancellationToken) =>
            {
                var args = new ConfigUnsetArgs()
                {
                    ConfigKey = parseResult.GetValue(UnsetConfigKeyArgument),
                    ConfigFile = parseResult.GetValue(ConfigFileOption),
                };

                int exitCode = ConfigUnsetRunner.Run(args, getLogger);
                return Task.FromResult(exitCode);
            });
        }
    }
}
