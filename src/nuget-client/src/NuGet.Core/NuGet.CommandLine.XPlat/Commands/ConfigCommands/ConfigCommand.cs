// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.CommandLine;
using System.CommandLine.Help;
using System.CommandLine.Parsing;
using System.Linq;
using System.Threading.Tasks;
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

        private static CliArgument<string> AllOrConfigKeyOption = new CliArgument<string>(name: "all-or-config-key")
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

        internal static CliCommand Register(CliCommand app, Func<ILogger> getLogger)
        {
            var ConfigCmd = new CliCommand(name: "config", description: Strings.Config_Description);
            ConfigCmd.Options.Add(HelpOption);

            // Options directly under the verb 'config'

            // noun sub-command: config paths
            var PathsCmd = new CliCommand(name: "paths", description: Strings.ConfigPathsCommandDescription);

            // Options under sub-command: config paths
            RegisterOptionsForCommandConfigPaths(PathsCmd, getLogger);

            ConfigCmd.Subcommands.Add(PathsCmd);

            // noun sub-command: config get
            var GetCmd = new CliCommand(name: "get", description: Strings.ConfigGetCommandDescription);

            // Options under sub-command: config get
            RegisterOptionsForCommandConfigGet(GetCmd, getLogger);

            ConfigCmd.Subcommands.Add(GetCmd);

            // noun sub-command: config set
            var SetCmd = new CliCommand(name: "set", description: Strings.ConfigSetCommandDescription);

            // Options under sub-command: config set
            RegisterOptionsForCommandConfigSet(SetCmd, getLogger);

            ConfigCmd.Subcommands.Add(SetCmd);

            // noun sub-command: config unset
            var UnsetCmd = new CliCommand(name: "unset", description: Strings.ConfigUnsetCommandDescription);

            // Options under sub-command: config unset
            RegisterOptionsForCommandConfigUnset(UnsetCmd, getLogger);

            ConfigCmd.Subcommands.Add(UnsetCmd);

            app.Subcommands.Add(ConfigCmd);

            return ConfigCmd;
        } // End noun method

        private static void RegisterOptionsForCommandConfigPaths(CliCommand cmd, Func<ILogger> getLogger)
        {
            cmd.Add(WorkingDirectory);
            cmd.Add(HelpOption);
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
            cmd.Add(AllOrConfigKeyOption);
            cmd.Add(WorkingDirectory);
            cmd.Add(ShowPathOption);
            cmd.Add(HelpOption);

            // Create handler delegate handler for cmd
            cmd.SetAction((parseResult, cancellationToken) =>
            {
                int exitCode;
                var args = new ConfigGetArgs()
                {
                    AllOrConfigKey = parseResult.GetValue(AllOrConfigKeyOption),
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
            cmd.Add(SetConfigKeyArgument);
            cmd.Add(ConfigValueArgument);
            cmd.Add(ConfigFileOption);
            cmd.Add(HelpOption);
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
            cmd.Add(UnsetConfigKeyArgument);
            cmd.Add(ConfigFileOption);
            cmd.Add(HelpOption);
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
