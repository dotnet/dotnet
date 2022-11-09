// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Composition;
using System.Composition.Hosting;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Cci;
using Microsoft.Cci.Comparers;
using Microsoft.Cci.Differs;
using Microsoft.Cci.Differs.Rules;
using Microsoft.Cci.Extensions;
using Microsoft.Cci.Extensions.CSharp;
using Microsoft.Cci.Filters;
using Microsoft.Cci.Mappings;
using Microsoft.Cci.Writers;

namespace Microsoft.DotNet.ApiCompat
{
    /// <summary>
    /// The core part of ApiCompat which gets invoked by the ApiCompatRunner console frontend
    /// and the ApiCompatTask msbuild task.
    /// </summary>
    public static class Executor
    {
        /// <summary>
        /// The core part of ApiCompat which accepts a given set of arguments and
        /// performs api compatibility checks.
        /// </summary>
        public static int Run(bool usesMSBuildLog,
            bool disableAssemblyResolveTraceListener,
            IEnumerable<string> contracts,
            IEnumerable<string> implementationDirectories,
            TextWriter output,
            string rightOperand = "implementation",
            string leftOperand = "contract",
            bool listRules = false,
            IEnumerable<string> baselineFileNames = null,
            bool validateBaseline = false,
            bool resolveFramework = false,
            bool skipUnifyToLibPath = false,
            IEnumerable<string> contractDependsFileNames = null,
            string contractCoreAssembly = null,
            bool ignoreDesignTimeFacades = false,
            bool warnOnMissingAssemblies = false,
            bool respectInternals = false,
            bool warnOnIncorrectVersion = false,
            bool enforceOptionalRules = false,
            bool mdil = false,
            bool excludeNonBrowsable = false,
            bool excludeCompilerGenerated = false,
            string remapFile = null,
            bool skipGroupByAssembly = false,
            IEnumerable<string> excludeAttributes = null,
            bool allowDefaultInterfaceMethods = false)
        {
            // Clear exit code from previous runs on the same domain given this is a static property.
            DifferenceWriter.ExitCode = 0;

            if (listRules)
            {
                CompositionHost c = GetCompositionHost();
                ExportCciSettings.StaticSettings = CciComparers.Default.GetEqualityComparer<ITypeReference>();

                IEnumerable<IDifferenceRule> rules = c.GetExports<IDifferenceRule>();

                foreach (IDifferenceRule rule in rules.OrderBy(r => r.GetType().Name, StringComparer.OrdinalIgnoreCase))
                {
                    string ruleName = rule.GetType().Name;

                    if (IsOptionalRule(rule))
                        ruleName += " (optional)";

                    output.WriteLine(ruleName);
                }

                return 0;
            }

            using (output)
            {
                if (DifferenceWriter.ExitCode != 0)
                    return 0;

                if (!disableAssemblyResolveTraceListener)
                    Trace.Listeners.Add(new TextWriterTraceListener(output) { Filter = new EventTypeFilter(SourceLevels.Error | SourceLevels.Warning) });

                try
                {
                    BaselineDifferenceFilter filter = GetBaselineDifferenceFilter(baselineFileNames, validateBaseline);
                    NameTable sharedNameTable = new();
                    HostEnvironment contractHost = new(sharedNameTable);
                    contractHost.UnableToResolve += (sender, e) => Trace.TraceError($"Unable to resolve assembly '{e.Unresolved}' referenced by the {leftOperand} assembly '{e.Referrer}'.");
                    contractHost.ResolveAgainstRunningFramework = resolveFramework;
                    contractHost.UnifyToLibPath = !skipUnifyToLibPath;
                    contractHost.AddLibPaths(contractDependsFileNames);
                    IEnumerable<IAssembly> contractAssemblies = contractHost.LoadAssemblies(contracts, contractCoreAssembly);

                    if (ignoreDesignTimeFacades)
                        contractAssemblies = contractAssemblies.Where(a => !a.IsFacade());

                    HostEnvironment implHost = new(sharedNameTable);
                    implHost.UnableToResolve += (sender, e) => Trace.TraceError($"Unable to resolve assembly '{e.Unresolved}' referenced by the {rightOperand} assembly '{e.Referrer}'.");
                    implHost.ResolveAgainstRunningFramework = resolveFramework;
                    implHost.UnifyToLibPath = !skipUnifyToLibPath;
                    implHost.AddLibPaths(implementationDirectories);
                    if (warnOnMissingAssemblies)
                        implHost.LoadErrorTreatment = ErrorTreatment.TreatAsWarning;

                    // The list of contractAssemblies already has the core assembly as the first one (if _contractCoreAssembly was specified).
                    IEnumerable<IAssembly> implAssemblies = implHost.LoadAssemblies(contractAssemblies.Select(a => a.AssemblyIdentity), warnOnIncorrectVersion);

                    // Exit after loading if the code is set to non-zero
                    if (DifferenceWriter.ExitCode != 0)
                        return 0;

                    bool includeInternals = respectInternals &&
                        contractAssemblies.Any(assembly => assembly.Attributes.HasAttributeOfType(
                            "System.Runtime.CompilerServices.InternalsVisibleToAttribute"));
                    ICciDifferenceWriter writer = GetDifferenceWriter(
                        output,
                        filter,
                        enforceOptionalRules,
                        mdil,
                        excludeNonBrowsable,
                        includeInternals,
                        excludeCompilerGenerated,
                        remapFile,
                        !skipGroupByAssembly,
                        leftOperand,
                        rightOperand,
                        excludeAttributes,
                        allowDefaultInterfaceMethods,
                        usesMSBuildLog);
                    writer.Write(string.Join(",", implementationDirectories), implAssemblies, string.Join(",", contracts), contractAssemblies);

                    return 0;
                }
                catch (FileNotFoundException)
                {
                    // FileNotFoundException will be thrown by GetBaselineDifferenceFilter if it doesn't find the baseline file
                    // OR if GetComparers doesn't find the remap file.
                    return 2;
                }
            }
        }

        private static ICciDifferenceWriter GetDifferenceWriter(TextWriter writer,
            IDifferenceFilter filter,
            bool enforceOptionalRules,
            bool mdil,
            bool excludeNonBrowsable,
            bool includeInternals,
            bool excludeCompilerGenerated,
            string remapFile,
            bool groupByAssembly,
            string leftOperand,
            string rightOperand,
            IEnumerable<string> excludeAttributes,
            bool allowDefaultInterfaceMethods,
            bool usesMSBuildLog)
        {
            CompositionHost container = GetCompositionHost();

            bool RuleFilter(IDifferenceRuleMetadata ruleMetadata)
            {
                if (ruleMetadata.OptionalRule && !enforceOptionalRules)
                    return false;

                if (ruleMetadata.MdilServicingRule && !mdil)
                    return false;
                return true;
            }

            if (mdil && excludeNonBrowsable)
            {
                Trace.TraceWarning("Enforcing MDIL servicing rules and exclusion of non-browsable types are both enabled, but they are not compatible so non-browsable types will not be excluded.");
            }

            if (includeInternals && (mdil || excludeNonBrowsable))
            {
                Trace.TraceWarning("Enforcing MDIL servicing rules or exclusion of non-browsable types are enabled " +
                    "along with including internals -- an incompatible combination. Internal members will not be included.");
            }

            ICciFilter cciFilter = GetCciFilter(mdil, excludeNonBrowsable, includeInternals, excludeCompilerGenerated);
            var settings = new MappingSettings
            {
                Comparers = GetComparers(remapFile),
                DiffFactory = new ElementDifferenceFactory(container, RuleFilter),
                DiffFilter = GetDiffFilter(cciFilter),
                Filter = cciFilter,
                GroupByAssembly = groupByAssembly,
                IncludeForwardedTypes = true,
            };

            if (filter == null)
            {
                filter = new DifferenceFilter<IncompatibleDifference>();
            }

            var diffWriter = new DifferenceWriter(writer, settings, filter, usesMSBuildLog);
            ExportCciSettings.StaticSettings = settings.TypeComparer;
            ExportCciSettings.StaticOperands = new DifferenceOperands()
            {
                Contract = leftOperand,
                Implementation = rightOperand
            };

            ExportCciSettings.StaticAttributeFilter = GetAttributeFilter(excludeAttributes);
            ExportCciSettings.StaticRuleSettings = new RuleSettings { AllowDefaultInterfaceMethods = allowDefaultInterfaceMethods };

            // Always compose the diff writer to allow it to import or provide exports
            container.SatisfyImports(diffWriter);

            return diffWriter;
        }

        private static BaselineDifferenceFilter GetBaselineDifferenceFilter(IEnumerable<string> baselineFileNames, bool validateBaseline)
        {
            if (baselineFileNames == null)
            {
                return null;
            }

            BaselineDifferenceFilter baselineDifferenceFilter = null;

            AddFiles(baselineFileNames, (file) =>
                (baselineDifferenceFilter ??= new BaselineDifferenceFilter(new DifferenceFilter<IncompatibleDifference>(), validateBaseline)).AddBaselineFile(file));

            return baselineDifferenceFilter;
        }

        private static AttributeFilter GetAttributeFilter(IEnumerable<string> ignoreAttributeFileNames)
        {
            AttributeFilter attributeFilter = new();

            if (ignoreAttributeFileNames != null)
            {
                AddFiles(ignoreAttributeFileNames, (file) => attributeFilter.AddIgnoreAttributeFile(file));
            }

            return attributeFilter;
        }

        private static void AddFiles(IEnumerable<string> files, System.Action<string> addFile)
        {
            foreach (string file in files)
            {
                if (!string.IsNullOrEmpty(file))
                {
                    if (!File.Exists(file))
                    {
                        throw new FileNotFoundException($"File {file} was not found!", file);
                    }

                    addFile(file);
                }
            }
        }

        private static CompositionHost GetCompositionHost()
        {
            ContainerConfiguration configuration = new ContainerConfiguration().WithAssembly(typeof(Executor).GetTypeInfo().Assembly);
            return configuration.CreateContainer();
        }

        private static ICciComparers GetComparers(string remapFile)
        {
            if (!string.IsNullOrEmpty(remapFile))
            {
                if (!File.Exists(remapFile))
                {
                    throw new FileNotFoundException("ERROR: RemapFile {0} was not found!", remapFile);
                }
                return new NamespaceRemappingComparers(remapFile);
            }
            return CciComparers.Default;
        }

        private static ICciFilter GetCciFilter(
            bool enforcingMdilRules,
            bool excludeNonBrowsable,
            bool includeInternals,
            bool excludeCompilerGenerated)
        {
            ICciFilter includeFilter;
            if (enforcingMdilRules)
            {
                includeFilter = new MdilPublicOnlyCciFilter()
                {
                    IncludeForwardedTypes = true
                };
            }
            else if (excludeNonBrowsable)
            {
                includeFilter = new PublicEditorBrowsableOnlyCciFilter()
                {
                    IncludeForwardedTypes = true
                };
            }
            else if (includeInternals)
            {
                includeFilter = new InternalsAndPublicCciFilter();
            }
            else
            {
                includeFilter = new PublicOnlyCciFilter()
                {
                    IncludeForwardedTypes = true
                };
            }

            if (excludeCompilerGenerated)
            {
                includeFilter = new IntersectionFilter(includeFilter, new ExcludeCompilerGeneratedCciFilter());
            }

            return includeFilter;
        }

        private static IMappingDifferenceFilter GetDiffFilter(ICciFilter filter) =>
            new MappingDifferenceFilter(GetIncludeFilter(), filter);

        private static Func<DifferenceType, bool> GetIncludeFilter() => (d => d != DifferenceType.Unchanged);

        private static bool IsOptionalRule(IDifferenceRule rule) =>
            rule.GetType().GetTypeInfo().GetCustomAttribute<ExportDifferenceRuleAttribute>().OptionalRule;
    }
}
