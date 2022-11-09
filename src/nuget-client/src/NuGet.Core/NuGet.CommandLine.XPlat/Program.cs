// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.CommandLineUtils;
using NuGet.Common;
using NuGet.Commands;

namespace NuGet.CommandLine.XPlat
{
    internal class Program
    {
#if DEBUG
        private const string DebugOption = "--debug";
#endif
        private const string DotnetNuGetAppName = "dotnet nuget";
        private const string DotnetPackageAppName = "NuGet.CommandLine.XPlat.dll package";

        public static int Main(string[] args)
        {
            var log = new CommandOutputLogger(LogLevel.Information);
            return MainInternal(args, log);
        }

        /// <summary>
        /// Internal Main. This is used for testing.
        /// </summary>
        public static int MainInternal(string[] args, CommandOutputLogger log)
        {
#if DEBUG
            // Uncomment the following when debugging. Also uncomment the PackageReference for Microsoft.Build.Locator.
            /*try
            {
                // .NET JIT compiles one method at a time. If this method calls `MSBuildLocator` directly, the
                // try block is never entered if Microsoft.Build.Locator.dll can't be found. So, run it in a
                // lambda function to ensure we're in the try block. C# IIFE!
                ((Action)(() => Microsoft.Build.Locator.MSBuildLocator.RegisterDefaults()))();
            }
            catch
            {
                // MSBuildLocator is used only to enable Visual Studio debugging.
                // It's not needed when using a patched dotnet sdk, so it doesn't matter if it fails.
            }*/

            var debugNuGetXPlat = Environment.GetEnvironmentVariable("DEBUG_NUGET_XPLAT");

            if (args.Contains(DebugOption) || string.Equals(bool.TrueString, debugNuGetXPlat, StringComparison.OrdinalIgnoreCase))
            {
                args = args.Where(arg => !StringComparer.OrdinalIgnoreCase.Equals(arg, DebugOption)).ToArray();
                Debugger.Launch();
            }
#endif

            // Optionally disable localization.
            if (args.Any(arg => string.Equals(arg, CommandConstants.ForceEnglishOutputOption, StringComparison.OrdinalIgnoreCase)))
            {
                CultureUtility.DisableLocalization();
            }
            else
            {
                UILanguageOverride.Setup(log);
            }
            log.LogDebug(string.Format(CultureInfo.CurrentCulture, Strings.Debug_CurrentUICulture, CultureInfo.DefaultThreadCurrentUICulture));

            NuGet.Common.Migrations.MigrationRunner.Run();

            var app = InitializeApp(args, log);

            // Remove the correct item in array for "package" commands. Only do this when "add package", "remove package", etc... are being run.
            if (app.Name == DotnetPackageAppName)
            {
                // package add ...
                args[0] = null;
                args = args
                    .Where(e => e != null)
                    .ToArray();
            }

            NetworkProtocolUtility.SetConnectionLimit();

            XPlatUtility.SetUserAgent();

            app.OnExecute(() =>
            {
                app.ShowHelp();

                return 0;
            });

            log.LogVerbose(string.Format(CultureInfo.CurrentCulture, Strings.OutputNuGetVersion, app.FullName, app.LongVersionGetter()));

            int exitCode = 0;

            try
            {
                exitCode = app.Execute(args);
            }
            catch (Exception e)
            {
                bool handled = false;
                string verb = null;
                if (args.Length > 1)
                {
                    // Redirect users nicely if they do 'dotnet nuget sources add' or 'dotnet nuget add sources'
                    if (StringComparer.OrdinalIgnoreCase.Compare(args[0], "sources") == 0)
                    {
                        verb = args[1];
                    }
                    else if (StringComparer.OrdinalIgnoreCase.Compare(args[1], "sources") == 0)
                    {
                        verb = args[0];
                    }

                    if (verb != null)
                    {
                        switch (verb.ToLowerInvariant())
                        {
                            case "add":
                            case "remove":
                            case "update":
                            case "enable":
                            case "disable":
                            case "list":
                                log.LogMinimal(string.Format(CultureInfo.CurrentCulture,
                                    Strings.Sources_Redirect, $"dotnet nuget {verb} source"));
                                handled = true;
                                break;
                            default:
                                break;
                        }
                    }
                }

                if (!handled)
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

                    exitCode = 1;

                    ShowBestHelp(app, args);
                }
            }

            // Limit the exit code range to 0-255 to support POSIX
            if (exitCode < 0 || exitCode > 255)
            {
                exitCode = 1;
            }

            return exitCode;
        }


        private static CommandLineApplication InitializeApp(string[] args, CommandOutputLogger log)
        {
            // Many commands don't want prefixes output. Use this func instead of () => log to set the HidePrefix property first.
            Func<ILogger> getHidePrefixLogger = () =>
            {
                log.HidePrefixForInfoAndMinimal = true;
                return log;
            };

            // Allow commands to set the NuGet log level
            Action<LogLevel> setLogLevel = (logLevel) => log.VerbosityLevel = logLevel;

            var app = new CommandLineApplication();

            if (args.Any() && args[0] == "package")
            {
                // "dotnet * package" commands
                app.Name = DotnetPackageAppName;
                AddPackageReferenceCommand.Register(app, () => log, () => new AddPackageReferenceCommandRunner());
                RemovePackageReferenceCommand.Register(app, () => log, () => new RemovePackageReferenceCommandRunner());
                ListPackageCommand.Register(app, getHidePrefixLogger, setLogLevel, () => new ListPackageCommandRunner());
            }
            else
            {
                // "dotnet nuget *" commands
                app.Name = DotnetNuGetAppName;
                CommandParsers.Register(app, getHidePrefixLogger);
                DeleteCommand.Register(app, getHidePrefixLogger);
                PushCommand.Register(app, getHidePrefixLogger);
                LocalsCommand.Register(app, getHidePrefixLogger);
                VerifyCommand.Register(app, getHidePrefixLogger, setLogLevel, () => new VerifyCommandRunner());
                TrustedSignersCommand.Register(app, getHidePrefixLogger, setLogLevel);
                SignCommand.Register(app, getHidePrefixLogger, setLogLevel, () => new SignCommandRunner());
            }

            app.FullName = Strings.App_FullName;
            app.HelpOption(XPlatUtility.HelpOption);
            app.VersionOption("--version", typeof(Program).GetTypeInfo().Assembly.GetName().Version.ToString());

            return app;
        }

        private static void ShowBestHelp(CommandLineApplication app, string[] args)
        {
            CommandLineApplication lastCommand = null;
            List<CommandLineApplication> commands = app.Commands;
            // tunnel down into the args, and show the best help possible.
            foreach (string arg in args)
            {
                foreach (CommandLineApplication command in commands)
                {
                    if (arg == command.Name)
                    {
                        lastCommand = command;
                        commands = command.Commands;
                        break;
                    }
                }
            }

            if (lastCommand != null)
            {
                lastCommand.ShowHelp();
            }
            else
            {
                app.ShowHelp();
            }
        }
    }
}
