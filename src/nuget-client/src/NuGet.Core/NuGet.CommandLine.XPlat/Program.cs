// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.CommandLine;
using System.Globalization;
using System.Linq;
using NuGet.CommandLine.XPlat.Commands;
using NuGet.CommandLine.XPlat.Commands.NuGet.Add;
using NuGet.CommandLine.XPlat.Commands.NuGet.Disable;
using NuGet.CommandLine.XPlat.Commands.NuGet.Enable;
using NuGet.CommandLine.XPlat.Commands.NuGet.List;
using NuGet.CommandLine.XPlat.Commands.NuGet.Remove;
using NuGet.CommandLine.XPlat.Commands.NuGet.Update;
using NuGet.CommandLine.XPlat.Commands.Why;
using NuGet.Commands;
using NuGet.Common;

#if DEBUG
using NuGet.CommandLine.XPlat.Commands.Package.Update;
using NuGet.CommandLine.XPlat.Commands.Package.PackageDownload;
#endif

namespace NuGet.CommandLine.XPlat
{
    public static class Program
    {
#if DEBUG
        private const string DebugOption = "--debug";
#endif

        internal static int Main(string[] args)
        {
            return MainInternal(args, virtualProjectBuilder: null);
        }

        public static int Run(string[] args, IVirtualProjectBuilder virtualProjectBuilder)
        {
            return MainInternal(args, virtualProjectBuilder);
        }

        private static int MainInternal(string[] args, IVirtualProjectBuilder? virtualProjectBuilder)
        {
            var log = new CommandOutputLogger(LogLevel.Information);
            return MainInternal(args, log, EnvironmentVariableWrapper.Instance, virtualProjectBuilder);
        }

        /// <summary>
        /// Internal Main. This is used for testing.
        /// </summary>
        internal static int MainInternal(string[] args, CommandOutputLogger log, IEnvironmentVariableReader environmentVariableReader, IVirtualProjectBuilder? virtualProjectBuilder = null)
        {
#if USEMSBUILDLOCATOR
            try
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
            }
#endif

#if DEBUG
            string? debugNuGetXPlat = environmentVariableReader.GetEnvironmentVariable("DEBUG_NUGET_XPLAT");

            if (args.Contains(DebugOption) || string.Equals(bool.TrueString, debugNuGetXPlat, StringComparison.OrdinalIgnoreCase))
            {
                args = args.Where(arg => !StringComparer.OrdinalIgnoreCase.Equals(arg, DebugOption)).ToArray();
                System.Diagnostics.Debugger.Launch();
            }
#endif

            // Optionally disable localization.
            if (args.Any(arg => string.Equals(arg, CommandConstants.ForceEnglishOutputOption, StringComparison.OrdinalIgnoreCase)))
            {
                CultureUtility.DisableLocalization();
            }
            else
            {
                UILanguageOverride.Setup(log, environmentVariableReader);
            }
            log.LogDebug(string.Format(CultureInfo.CurrentCulture, Strings.Debug_CurrentUICulture, CultureInfo.DefaultThreadCurrentUICulture));

            NuGet.Common.Migrations.MigrationRunner.Run();

            Func<ILoggerWithColor> getHidePrefixLogger = () =>
            {
                log.HidePrefixForInfoAndMinimal = true;
                return log;
            };

            Action<LogLevel> setLogLevel = (logLevel) => log.VerbosityLevel = logLevel;

            RootCommand rootCommand = new RootCommand();
            rootCommand.Options.Add(new Option<bool>(CommandConstants.ForceEnglishOutputOption)
            {
                Description = Strings.ForceEnglishOutput_Description,
                Arity = ArgumentArity.Zero,
                Recursive = true
            });
            Option<bool> interactiveOption = new Option<bool>("--interactive")
            {
                Description = Strings.AddPkg_InteractiveDescription,
                DefaultValueFactory = _ => Console.IsOutputRedirected
            };

            if (args.Length > 0 && args[0] == "package")
            {
                var packageCommand = new Command("package");
                rootCommand.Subcommands.Add(packageCommand);

                var msbuild = new MSBuildAPIUtility(log, virtualProjectBuilder);

                PackageSearchCommand.Register(packageCommand, getHidePrefixLogger);
                AddPackageReferenceCommand.Register(packageCommand, () => log, () => new AddPackageReferenceCommandRunner(), () => msbuild.VirtualProjectBuilder);
                RemovePackageReferenceCommand.Register(packageCommand, () => log, () => new RemovePackageReferenceCommandRunner(), () => msbuild.VirtualProjectBuilder);
                ListPackageCommand.Register(packageCommand, getHidePrefixLogger, setLogLevel, () => new ListPackageCommandRunner(msbuild));
#if DEBUG
                PackageUpdateCommand.Register(packageCommand, interactiveOption, virtualProjectBuilder);
                PackageDownloadCommand.Register(packageCommand, interactiveOption);
#endif
            }
            else
            {
                var nugetCommand = new Command("nuget");
                rootCommand.Subcommands.Add(nugetCommand);

                var lazyConsole = new Lazy<Spectre.Console.IAnsiConsole>(() => Spectre.Console.AnsiConsole.Console);

                ConfigCommand.Register(rootCommand, getHidePrefixLogger);
                WhyCommand.Register(rootCommand, lazyConsole, virtualProjectBuilder);
                DeleteCommand.Register(rootCommand, getHidePrefixLogger);
                PushCommand.Register(rootCommand, getHidePrefixLogger);
                LocalsCommand.Register(rootCommand, getHidePrefixLogger);
                VerifyCommand.Register(rootCommand, getHidePrefixLogger, setLogLevel, () => new VerifyCommandRunner());
                SignCommand.Register(rootCommand, getHidePrefixLogger, setLogLevel, () => new SignCommandRunner());
                TrustedSignersCommand.Register(rootCommand, getHidePrefixLogger, setLogLevel);

                // Source/client-cert verb commands
                DotnetNuGetAddCommand.Register(rootCommand, getHidePrefixLogger);
                DotnetNuGetDisableCommand.Register(rootCommand, getHidePrefixLogger);
                DotnetNuGetEnableCommand.Register(rootCommand, getHidePrefixLogger);
                DotnetNuGetListCommand.Register(rootCommand, getHidePrefixLogger);
                DotnetNuGetRemoveCommand.Register(rootCommand, getHidePrefixLogger);
                DotnetNuGetUpdateCommand.Register(rootCommand, getHidePrefixLogger);

                // These commands have the same parser as the dotnet CLI, so they can be used interchangeably with "dotnet nuget *"
                ConfigCommand.Register(nugetCommand, getHidePrefixLogger);
                WhyCommand.Register(nugetCommand, lazyConsole, virtualProjectBuilder);
            }

            NetworkProtocolUtility.SetConnectionLimit();
            XPlatUtility.SetUserAgent();

            int exitCode = 0;
            ParseResult parseResult = rootCommand.Parse(args);
            var invocationConfig = new InvocationConfiguration
            {
                EnableDefaultExceptionHandler = false
            };

            try
            {
                exitCode = parseResult.Invoke(invocationConfig);
            }
            catch (Exception e)
            {
                LogException(e, log);
                // Commands that let exceptions propagate to here (e.g. push, source, client-cert)
                // have historically returned exit code 1 on failure. Preserve that contract rather
                // than returning ExitCodes.Error (2), which is reserved for commands that set it explicitly.
                exitCode = 1;
            }

            // Limit the exit code range to 0-255 to support POSIX
            if (exitCode < 0 || exitCode > 255)
            {
                exitCode = 1;
            }

            return exitCode;
        }

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
    }
}
