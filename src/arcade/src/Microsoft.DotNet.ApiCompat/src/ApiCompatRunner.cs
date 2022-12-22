// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using McMaster.Extensions.CommandLineUtils;

namespace Microsoft.DotNet.ApiCompat
{
    /// <summary>
    /// The console app's entry point which invokes Executor.Run in Microsoft.DotNet.ApiCompat.Core.
    /// The other entry point is an msbuild task, ApiCompatTask.
    /// </summary>
    public class ApiCompatRunner
    {
        private readonly CommandLineApplication _app;
        private TextWriter _outputStream;

        public ApiCompatRunner(TextWriter outputStream = null)
        {
            _outputStream = outputStream;
            _app = CreateApp();
        }

        public int Run(string[] args) => _app.Execute(args);

        private CommandLineApplication CreateApp()
        {
            var app = new CommandLineApplication
            {
                Name = "ApiCompat",
                FullName = "A command line tool to verify that two sets of APIs are compatible.",
                ResponseFileHandling = ResponseFileHandling.ParseArgsAsSpaceSeparated,
                Out = _outputStream ?? Console.Out,
                Error = _outputStream ?? Console.Error
            };
            app.HelpOption("-?|-h|--help");
            app.VersionOption("-v|--version", typeof(Program).Assembly.GetName().Version.ToString());

            CommandArgument contracts = app.Argument("contracts", "Comma delimited list of assemblies or directories of assemblies for all the contract assemblies.");
            contracts.IsRequired();
            CommandOption implDirs = app.Option("-i|--impl-dirs", "Comma delimited list of directories to find the implementation assemblies for each contract assembly.", CommandOptionType.SingleValue);
            implDirs.IsRequired(allowEmptyStrings: true);
            CommandOption baseline = app.Option("-b|--baseline", "Comma delimited list of files to skip known diffs.", CommandOptionType.SingleValue);
            CommandOption validateBaseline = app.Option("--validate-baseline", "Validates that baseline files don't have invalid/unused diffs.", CommandOptionType.NoValue);
            CommandOption mdil = app.Option("-m|--mdil", "Enforce MDIL servicing rules in addition to IL rules.", CommandOptionType.NoValue);
            CommandOption outFilePath = app.Option("-o|--out", "Output file path. Default is the console.", CommandOptionType.SingleValue);
            CommandOption leftOperand = app.Option("-l|--left-operand", "Name for left operand in comparison, default is 'contract'.", CommandOptionType.SingleValue);
            CommandOption rightOperand = app.Option("-r|--right-operand", "Name for right operand in comparison, default is 'implementation'.", CommandOptionType.SingleValue);
            CommandOption listRules = app.Option("--list-rules", "Outputs all the rules. If this options is supplied all other options are ignored.", CommandOptionType.NoValue);
            CommandOption remapFile = app.Option("--remap-file", "File with a list of type and/or namespace remappings to consider apply to names while diffing.", CommandOptionType.SingleValue);
            CommandOption skipGroupByAssembly = app.Option("--skip-group-by-assembly", "Skip grouping the differences by assembly instead of flattening the namespaces.", CommandOptionType.NoValue);
            CommandOption skipUnifyToLibPath = app.Option("--skip-unify-to-lib-path", "Skip unifying the assembly references to the loaded assemblies and the assemblies found in the given directories (contractDepends and implDirs).", CommandOptionType.NoValue);
            CommandOption resolveFx = app.Option("--resolve-fx", "If a contract or implementation dependency cannot be found in the given directories, fallback to try to resolve against the framework directory on the machine.", CommandOptionType.NoValue);
            CommandOption contractDepends = app.Option("--contract-depends", "Comma delimited list of directories used to resolve the dependencies of the contract assemblies.", CommandOptionType.SingleValue);
            CommandOption contractCoreAssembly = app.Option("--contract-core-assembly", "Simple name for the core assembly to use.", CommandOptionType.SingleValue);
            CommandOption ignoreDesignTimeFacades = app.Option("--ignore-design-time-facades", "Ignore design time facades in the contract set while analyzing.", CommandOptionType.NoValue);
            CommandOption warnOnIncorrectVersion = app.Option("--warn-on-incorrect-version", "Warn if the contract version number doesn't match the found implementation version number.", CommandOptionType.NoValue);
            CommandOption warnOnMissingAssemblies = app.Option("--warn-on-missing-assemblies", "Warn if the contract assembly cannot be found in the implementation directories. Default is to error and not do analysis.", CommandOptionType.NoValue);
            CommandOption excludeNonBrowsable = app.Option("--exclude-non-browsable", "When MDIL servicing rules are not being enforced, exclude validation on types that are marked with EditorBrowsable(EditorBrowsableState.Never).", CommandOptionType.NoValue);
            CommandOption excludeAttributes = app.Option("--exclude-attributes", "Comma delimited list of files with types in DocId format of which attributes to exclude.", CommandOptionType.SingleValue);
            CommandOption enforceOptionalRules = app.Option("--enforce-optional-rules", "Enforce optional rules, in addition to the mandatory set of rules.", CommandOptionType.NoValue);
            CommandOption allowDefaultInterfaceMethods = app.Option("--allow-default-interface-methods", "Allow default interface methods additions to not be considered breaks. This flag should only be used if you know your consumers support DIM", CommandOptionType.NoValue);
            CommandOption respectInternals = app.Option(
                "--respect-internals",
                "Include both internal and public APIs if assembly contains an InternalsVisibleTo attribute. Otherwise, include only public APIs.",
                CommandOptionType.NoValue);

            // --exclude-compiler-generated is recommended if the same option was passed to GenAPI.
            //
            // For one thing, comparing compiler-generated attributes, especially `CompilerGeneratedAttribute` itself,
            // on members leads to numerous false incompatibilities e.g. { get; set; } properties result in two
            // compiler-generated methods but GenAPI produces `{ get { throw null; } set { } }` i.e. explicit methods.
            CommandOption excludeCompilerGenerated = app.Option(
                "--exclude-compiler-generated",
                "Exclude APIs marked with a CompilerGenerated attribute.",
                CommandOptionType.NoValue);

            app.OnExecute(() =>
            {
                bool disableAssemblyResolveTraceListener = false;
                // Use Console.Out if no output file path is passed in or
                // when the file cannot be opened or created.
                if (_outputStream == null && (string.IsNullOrWhiteSpace(outFilePath.Value()) ||
                    !OutputHelper.TryGetOutput(outFilePath.Value(), out _outputStream)))
                {
                    _outputStream = Console.Out;
                    disableAssemblyResolveTraceListener = true;
                }

                Executor.Run(usesMSBuildLog: false,
                    disableAssemblyResolveTraceListener,
                    SplitPaths(contracts.Value),
                    SplitPaths(implDirs.Value()),
                    _outputStream,
                    rightOperand.Value(),
                    leftOperand.Value(),
                    listRules.HasValue(),
                    SplitPaths(baseline.Value()),
                    validateBaseline.HasValue(),
                    resolveFx.HasValue(),
                    skipUnifyToLibPath.HasValue(),
                    SplitPaths(contractDepends.Value()),
                    contractCoreAssembly.Value(),
                    ignoreDesignTimeFacades.HasValue(),
                    warnOnMissingAssemblies.HasValue(),
                    respectInternals.HasValue(),
                    warnOnIncorrectVersion.HasValue(),
                    enforceOptionalRules.HasValue(),
                    mdil.HasValue(),
                    excludeNonBrowsable.HasValue(),
                    excludeCompilerGenerated.HasValue(),
                    remapFile.Value(),
                    skipGroupByAssembly.HasValue(),
                    SplitPaths(excludeAttributes.Value()),
                    allowDefaultInterfaceMethods.HasValue());
            });

            return app;
        }

        private static string[] SplitPaths(string pathSet) =>
            pathSet?.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries) ?? new string[0];
    }
}
