// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.CommandLine;
using System.CommandLine.Help;
using System.CommandLine.Parsing;
using System.Linq;
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

        private static CliArgument<string> SetConfigKeyArgument = new CliArgument<string>(name: "config-key")
        {
            Arity = ArgumentArity.ExactlyOne,
            Description = Strings.ConfigSetConfigKeyDescription,
        };

        private static CliArgument<string> UnsetConfigKeyArgument = new CliArgument<string>(name: "config-key")
        {
            Arity = ArgumentArity.ExactlyOne,
            Description = Strings.ConfigUnsetConfigKeyDescription,
        };

        private static CliArgument<string> ConfigValueArgument = new CliArgument<string>(name: "config-value")
        {
            Arity = ArgumentArity.ExactlyOne,
            Description = Strings.ConfigSetConfigValueDescription,
        };

        private static CliArgument<string> AllOrConfigKeyArgument = new CliArgument<string>(name: "all-or-config-key")
        {
            Arity = ArgumentArity.ZeroOrOne,
            HelpName = Strings.ConfigGetAllOrConfigKeyDescription,
            Description = Strings.ConfigGetAllOrConfigKeyDescription
        };

        private static CliOption<string> WorkingDirectory = new CliOption<string>(name: "--working-directory")
        {
            Arity = ArgumentArity.ZeroOrOne,
            Description = Strings.ConfigPathsWorkingDirectoryDescription
        };

        private static CliOption<bool> ShowPathOption = new CliOption<bool>(name: "--show-path")
        {
            Arity = ArgumentArity.Zero,
            Description = Strings.ConfigGetShowPathDescription,
        };

        private static CliOption<string> ConfigFileOption = new CliOption<string>(name: "--configfile")
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

        internal static void ShowHelp(ParseResult parseResult, CliCommand cmd)
        {
            var tokenList = parseResult.Tokens.TakeWhile(token => token.Type == CliTokenType.Argument || token.Type == CliTokenType.Command || token.Type == CliTokenType.Directive).Select(t => t.Value).ToList();
            tokenList.Add("-h");
            cmd.Parse(tokenList).Invoke();
        }

        internal static void Register(CommandLineApplication app)
        {
            app.Command("config", configCmd =>
            {
                configCmd.Description = Strings.Config_Description;
            });
        }

        internal static CliCommand Register(CliCommand app, Func<ILogger> getLogger)
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

        private static void RegisterOptionsForCommandConfigPaths(CliCommand cmd, Func<ILogger> getLogger)
        {
            cmd.Options.Add(WorkingDirectory);
            cmd.Options.Add(HelpOption);
            // Create handler delegate handler for cmd
            cmd.SetAction((parseResult, cancellationToken) =>
            {
                int exitCode;

                var args = new ConfigPathsArgs()
                {
                    WorkingDirectory = parseResult.GetValue(WorkingDirectory),
                };

                try
                {
                    ConfigPathsRunner.Run(args, getLogger);
                    exitCode = 0;
                }
                catch (Exception e)
                {
                    LogException(e, getLogger());
                    ShowHelp(parseResult, cmd);

                    exitCode = 1;
                }
                return Task.FromResult(exitCode);
            });
        }

        private static void RegisterOptionsForCommandConfigGet(CliCommand cmd, Func<ILogger> getLogger)
        {
            cmd.Arguments.Add(AllOrConfigKeyArgument);
            cmd.Options.Add(WorkingDirectory);
            cmd.Options.Add(ShowPathOption);
            cmd.Options.Add(HelpOption);

            // Create handler delegate handler for cmd
            cmd.SetAction((parseResult, cancellationToken) =>
            {
                int exitCode;
                var args = new ConfigGetArgs()
                {
                    AllOrConfigKey = parseResult.GetValue(AllOrConfigKeyArgument),
                    WorkingDirectory = parseResult.GetValue(WorkingDirectory),
                    ShowPath = parseResult.GetValue(ShowPathOption),
                };

                try
                {
                    ConfigGetRunner.Run(args, getLogger);
                    exitCode = 0;
                }
                catch (Exception e)
                {
                    LogException(e, getLogger());
                    ShowHelp(parseResult, cmd);

                    exitCode = 1;
                }
                return Task.FromResult(exitCode);
            });
        }

        private static void RegisterOptionsForCommandConfigSet(CliCommand cmd, Func<ILogger> getLogger)
        {
            cmd.Arguments.Add(SetConfigKeyArgument);
            cmd.Arguments.Add(ConfigValueArgument);
            cmd.Options.Add(ConfigFileOption);
            cmd.Options.Add(HelpOption);
            // Create handler delegate handler for cmd
            cmd.SetAction((parseResult, cancellationToken) =>
            {
                int exitCode;
                var args = new ConfigSetArgs()
                {
                    ConfigKey = parseResult.GetValue(SetConfigKeyArgument),
                    ConfigValue = parseResult.GetValue(ConfigValueArgument),
                    ConfigFile = parseResult.GetValue(ConfigFileOption),
                };

                try
                {
                    ConfigSetRunner.Run(args, getLogger);
                    exitCode = 0;
                }
                catch (Exception e)
                {
                    LogException(e, getLogger());
                    ShowHelp(parseResult, cmd);

                    exitCode = 1;
                }
                return Task.FromResult(exitCode);
            });
        }

        private static void RegisterOptionsForCommandConfigUnset(CliCommand cmd, Func<ILogger> getLogger)
        {
            cmd.Arguments.Add(UnsetConfigKeyArgument);
            cmd.Options.Add(ConfigFileOption);
            cmd.Options.Add(HelpOption);
            // Create handler delegate handler for cmd
            cmd.SetAction((parseResult, cancellationToken) =>
            {
                int exitCode;

                var args = new ConfigUnsetArgs()
                {
                    ConfigKey = parseResult.GetValue(UnsetConfigKeyArgument),
                    ConfigFile = parseResult.GetValue(ConfigFileOption),
                };

                try
                {
                    ConfigUnsetRunner.Run(args, getLogger);
                    exitCode = 0;
                }
                catch (Exception e)
                {
                    LogException(e, getLogger());
                    ShowHelp(parseResult, cmd);

                    exitCode = 1;
                }
                return Task.FromResult(exitCode);
            });
        }
    }
}
