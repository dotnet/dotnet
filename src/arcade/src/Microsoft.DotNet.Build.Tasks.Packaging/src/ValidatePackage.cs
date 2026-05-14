// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.Frameworks;
using NuGet.Versioning;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using PropertyNames = NuGet.Client.ManagedCodeConventions.PropertyNames;

namespace Microsoft.DotNet.Build.Tasks.Packaging
{
    public class ValidatePackage : ValidationTask
    {
        [Required]
        public string ContractName
        {
            get;
            set;
        }

        [Required]
        public string PackageId
        {
            get;
            set;
        }

        [Required]
        public string PackageVersion
        {
            get;
            set;
        }

        /// <summary>
        /// Frameworks supported by this package
        ///   Identity: name of framework, can suffix '+' to indicate all later frameworks under validation.
        ///   RuntimeIDs: Semi-colon separated list of runtime IDs.  If specified overrides the value specified in Frameworks.
        ///   Version: version of API supported
        /// </summary>
        [Required]
        public ITaskItem[] SupportedFrameworks
        {
            get;
            set;
        }

        /// <summary>
        /// Frameworks to evaluate.
        ///   Identity: Framework
        ///   RuntimeIDs: Semi-colon separated list of runtime IDs
        /// </summary>
        [Required]
        public ITaskItem[] Frameworks
        {
            get;
            set;
        }

        /// <summary>
        /// Path to runtime.json that contains the runtime graph.
        /// </summary>
        [Required]
        public string RuntimeFile
        {
            get;
            set;
        }

        public bool SkipGenerationCheck
        {
            get;
            set;
        }
        public bool SkipIndexCheck
        {
            get;
            set;
        }

        public bool SkipSupportCheck
        {
            get;
            set;
        }

        public bool UseNetPlatform
        {
            get { return _generationIdentifier == FrameworkConstants.FrameworkIdentifiers.NetPlatform; }
            set { _generationIdentifier = value ? FrameworkConstants.FrameworkIdentifiers.NetPlatform : FrameworkConstants.FrameworkIdentifiers.NetStandard; }
        }

        /// <summary>
        /// List of frameworks which were validated and determined to be supported
        ///   Identity: framework short name
        ///   Framework: framework full name
        ///   Version: assembly version of API that is supported
        ///   Inbox: true if assembly is expected to come from targeting pack
        ///   ValidatedRIDs: all RIDs that were scanned
        /// </summary>
        [Output]
        public ITaskItem[] AllSupportedFrameworks
        {
            get;
            set;
        }
        
        private Dictionary<NuGetFramework, ValidationFramework> _frameworks;
        private string _generationIdentifier = FrameworkConstants.FrameworkIdentifiers.NetStandard;

        public override bool Execute()
        {
            InitializeValidationTask();

            if (!SkipSupportCheck)
            {
                LoadSupport();

                if (!SkipGenerationCheck)
                {
                    ValidateGenerations();
                }

                // TODO: need to validate dependencies.
                ValidateSupport();
            }

            ValidateIndex();

            return !Log.HasLoggedErrors;
        }

        private void ValidateGenerations()
        {
            // get the generation of all portable implementation dlls.
            var allRuntimeGenerations = _report.Targets.Values.SelectMany(t => t.RuntimeAssets.NullAsEmpty())
                .Select(r => r.TargetFramework)
                .Where(fx => fx != null && fx.Framework == _generationIdentifier && fx.Version != null)
                .Select(fx => fx.Version);

            // get the generation of all supported frameworks (some may have framework specific implementations
            // or placeholders).
            var allSupportedGenerations = _frameworks.Values.Where(vf => vf.SupportedVersion != null && FrameworkUtilities.IsGenerationMoniker(vf.Framework) && vf.Framework.Version != null)
                .Select(vf => vf.Framework.Version);

            // find the minimum supported version as the minimum of any generation explicitly implemented 
            // with a portable implementation, or the generation of a framework with a platform specific
            // implementation.
            Version minSupportedGeneration = allRuntimeGenerations.Concat(allSupportedGenerations).Min();

            // validate API version against generation for all files
            foreach (var compileAsset in _report.Targets.Values.SelectMany(t => t.CompileAssets)
                .Where(f => IsDll(f.LocalPath) && FrameworkUtilities.IsGenerationMoniker(f.TargetFramework)))
            {
                if (compileAsset.TargetFramework.Version < minSupportedGeneration)
                {
                    Log.LogError($"Invalid generation {compileAsset.TargetFramework.Version} for {compileAsset.LocalPath}, must be at least {minSupportedGeneration} based on the implementations in the package.  If you meant to target the lower generation you may be missing an implementation for a framework on that lower generation.  If not you should raise the generation of the reference assembly to match that of the lowest supported generation of all implementations/placeholders.");
                }
            }
        }

        private void ValidateSupport()
        {
            var runtimeFxSuppression = GetSuppressionValues(Suppression.PermitRuntimeTargetMonikerMismatch) ?? new HashSet<string>();

            // validate support for each TxM:RID
            foreach (var validateFramework in _frameworks.Values)
            {
                NuGetFramework fx = validateFramework.Framework;
                Version supportedVersion = validateFramework.SupportedVersion;

                Target compileTarget;
                if (!_report.Targets.TryGetValue(fx.ToString(), out compileTarget))
                {
                    Log.LogError($"Missing target {fx.ToString()} from validation report {ReportFile}");
                    continue;
                }

                var compileAssetPaths = compileTarget.CompileAssets.Select(ca => ca.PackagePath);
                bool hasCompileAsset, hasCompilePlaceHolder;
                NuGetAssetResolver.ExamineAssets(Log, "Compile", ContractName, fx.ToString(), compileAssetPaths, out hasCompileAsset, out hasCompilePlaceHolder);
                
                // resolve/test for each RID associated with this framework.
                foreach (string runtimeId in validateFramework.RuntimeIds)
                {
                    string target = String.IsNullOrEmpty(runtimeId) ? fx.ToString() : $"{fx}/{runtimeId}";

                    Target runtimeTarget;
                    if (!_report.Targets.TryGetValue(target, out runtimeTarget))
                    {
                        Log.LogError($"Missing target {target} from validation report {ReportFile}");
                        continue;
                    }

                    var runtimeAssetPaths = runtimeTarget.RuntimeAssets.Select(ra => ra.PackagePath);

                    bool hasRuntimeAsset, hasRuntimePlaceHolder;
                    NuGetAssetResolver.ExamineAssets(Log, "Runtime", ContractName, target, runtimeAssetPaths, out hasRuntimeAsset, out hasRuntimePlaceHolder);

                    if (null == supportedVersion)
                    {
                        // Contract should not be supported on this platform.
                        bool permitImplementation = HasSuppression(Suppression.PermitImplementation, target);

                        if (hasCompileAsset && (hasRuntimeAsset & !permitImplementation))
                        {
                            Log.LogError($"{ContractName} should not be supported on {target} but has both compile and runtime assets.");
                        }
                        else if (hasRuntimeAsset & !permitImplementation)
                        {
                            Log.LogError($"{ContractName} should not be supported on {target} but has runtime assets.");
                        }

                        if (hasRuntimePlaceHolder && hasCompilePlaceHolder)
                        {
                            Log.LogError($"{ContractName} should not be supported on {target} but has placeholders for both compile and runtime which will permit the package to install.");
                        }
                    }
                    else
                    {
                        if (validateFramework.IsInbox && !HasSuppression(Suppression.TreatAsOutOfBox, fx))
                        {
                            if (!hasCompileAsset && !hasCompilePlaceHolder)
                            {
                                Log.LogError($"Framework {fx} should support {ContractName} inbox but was missing a placeholder for compile-time.  You may need to add <InboxOnTargetFramework Include=\"{fx.GetShortFolderName()}\" /> to your project.");
                            }
                            else if (hasCompileAsset)
                            {
                                Log.LogError($"Framework {fx} should support {ContractName} inbox but contained a reference assemblies: {String.Join(", ", compileAssetPaths)}.  You may need to add <InboxOnTargetFramework Include=\"{fx.GetShortFolderName()}\" /> to your project.");
                            }

                            if (!hasRuntimeAsset && !hasRuntimePlaceHolder)
                            {
                                Log.LogError($"Framework {fx} should support {ContractName} inbox but was missing a placeholder for run-time.  You may need to add <InboxOnTargetFramework Include=\"{fx.GetShortFolderName()}\" /> to your project.");
                            }
                            else if (hasRuntimeAsset)
                            {
                                Log.LogError($"Framework {fx} should support {ContractName} inbox but contained a implementation assemblies: {String.Join(", ", runtimeAssetPaths)}.  You may need to add <InboxOnTargetFramework Include=\"{fx.GetShortFolderName()}\" /> to your project.");
                            }
                        }
                        else
                        {
                            Version referenceAssemblyVersion = null;
                            if (!hasCompileAsset)
                            {
                                if (hasCompilePlaceHolder)
                                {
                                    Log.LogError($"{ContractName} should be supported on {target} but has a compile placeholder.  You may need to remove InboxOnTargetFramework Include=\"{fx.GetShortFolderName()}\" /> from your project.");
                                }
                                else
                                {
                                    Log.LogError($"{ContractName} should be supported on {target} but has no compile assets.");
                                }

                                // skip the runtime checks
                                continue;
                            }
                            else
                            {
                                var referenceAssemblies = compileTarget.CompileAssets.Where(ca => IsDll(ca.PackagePath));

                                if (referenceAssemblies.Count() > 1)
                                {
                                    Log.LogError($"{ContractName} should only contain a single compile asset for {target}.");
                                }

                                foreach (var referenceAssembly in referenceAssemblies)
                                {
                                    referenceAssemblyVersion = referenceAssembly.Version;

                                    if (!VersionUtility.IsCompatibleApiVersion(supportedVersion, referenceAssemblyVersion))
                                    {
                                        Log.LogError($"{ContractName} should support API version {supportedVersion} on {target} but {referenceAssembly.LocalPath} was found to support {referenceAssemblyVersion?.ToString() ?? "<unknown version>"}.");
                                    }
                                }
                            }


                            if (!hasRuntimeAsset)
                            {
                                if (HasSuppression(Suppression.PermitMissingImplementation, target))
                                {
                                    Log.LogMessage($"Suppressed: {ContractName} should be supported on {target} but has no runtime assets.");
                                }
                                else
                                {
                                    // Contract should not be supported on this platform.
                                    Log.LogError($"{ContractName} should be supported on {target} but has no runtime assets.");
                                }
                            }
                            else
                            {
                                var implementationAssemblies = runtimeTarget.RuntimeAssets.Where(ra => IsDll(ra.PackagePath));

                                Dictionary<string, PackageAsset> implementationFiles = new Dictionary<string, PackageAsset>();
                                foreach (var implementationAssembly in implementationAssemblies)
                                {
                                    Version implementationVersion = implementationAssembly.Version;

                                    if (!VersionUtility.IsCompatibleApiVersion(supportedVersion, implementationVersion))
                                    {
                                        Log.LogError($"{ContractName} should support API version {supportedVersion} on {target} but {implementationAssembly.LocalPath} was found to support {implementationVersion?.ToString() ?? "<unknown version>"}.");
                                    }

                                    // Previously we only permitted compatible mismatch if Suppression.PermitHigherCompatibleImplementationVersion was specified
                                    // this is a permitted thing on every framework but desktop (which requires exact match to ensure bindingRedirects exist)
                                    // Now make this the default, we'll check desktop, where it matters, more strictly
                                    if (referenceAssemblyVersion != null &&
                                        !VersionUtility.IsCompatibleApiVersion(referenceAssemblyVersion, implementationVersion))
                                    {
                                        Log.LogError($"{ContractName} has mismatched compile ({referenceAssemblyVersion}) and runtime ({implementationVersion}) versions on {target}.");
                                    }

                                    if (fx.Framework == FrameworkConstants.FrameworkIdentifiers.Net &&
                                        referenceAssemblyVersion != null &&
                                        !referenceAssemblyVersion.Equals(implementationVersion))
                                    {
                                        Log.LogError($"{ContractName} has a higher runtime version ({implementationVersion}) than compile version ({referenceAssemblyVersion}) on .NET Desktop framework {target}.  This will break bindingRedirects.  If the live reference was replaced with a harvested reference you may need to set <Preserve>true</Preserve> on your reference assembly ProjectReference.");
                                    }

                                    string fileName = Path.GetFileName(implementationAssembly.PackagePath);
                                    
                                    if (implementationFiles.ContainsKey(fileName))
                                    {
                                        Log.LogError($"{ContractName} includes both {implementationAssembly.LocalPath} and {implementationFiles[fileName].LocalPath} an on {target} which have the same name and will clash when both packages are used.");
                                    }
                                    else
                                    {
                                        implementationFiles[fileName] = implementationAssembly;
                                    }

                                    if (!implementationAssembly.TargetFramework.Equals(fx) && !runtimeFxSuppression.Contains(fx.ToString()))
                                    {
                                        // the selected asset wasn't an exact framework match, let's see if we have an exact match in any other runtime asset.                                        
                                        var matchingFxAssets = _report.UnusedAssets.Where(i => i.TargetFramework != null && i.TargetFramework.Equals(fx) &&  // exact framework
                                                                                          // Same file
                                                                                          Path.GetFileName(i.PackagePath).Equals(fileName, StringComparison.OrdinalIgnoreCase) &&
                                                                                          // Is implementation
                                                                                          (i.PackagePath.StartsWith("lib") || i.PackagePath.StartsWith("runtimes")) &&
                                                                                          // is not the same source file as was already selected
                                                                                          i.LocalPath != implementationAssembly.LocalPath);

                                        if (matchingFxAssets.Any())
                                        {
                                            Log.LogError($"When targeting {target} {ContractName} will use {implementationAssembly.LocalPath} which targets {implementationAssembly.TargetFramework.GetShortFolderName()}  but {String.Join(";", matchingFxAssets.Select(i => i.PackagePath))} targets {fx.GetShortFolderName()} specifically.");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // Set output items
            AllSupportedFrameworks = _frameworks.Values.Where(fx => fx.SupportedVersion != null).Select(fx => fx.ToItem()).OrderBy(i => i.ItemSpec).ToArray();
        }

        private void ValidateIndex()
        {
            if (SkipIndexCheck)
            {
                return;
            }

            if (PackageIndexes == null || PackageIndexes.Length == 0)
            {
                return;
            }

            var index = PackageIndex.Load(PackageIndexes.Select(pi => pi.GetMetadata("FullPath")));

            PackageInfo info;
            if (!index.Packages.TryGetValue(PackageId, out info))
            {
                Log.LogError($"PackageIndex from {String.Join(", ", PackageIndexes.Select(i => i.ItemSpec))} is missing an entry for package {PackageId}.  Please run /t:UpdatePackageIndex on this project to commit an update.");
                return;
            }

            var allDlls = _report.Targets.Values.SelectMany(t => t.CompileAssets.NullAsEmpty().Concat(t.RuntimeAssets.NullAsEmpty()));
            var allAssemblies = allDlls.Where(f => f.Version != null);
            var assemblyVersions = new HashSet<Version>(allAssemblies.Select(f => VersionUtility.As4PartVersion(f.Version)));

            var thisPackageVersion = VersionUtility.As3PartVersion(NuGetVersion.Parse(PackageVersion).Version);

            foreach (var fileVersion in assemblyVersions)
            {
                Version packageVersion;

                // determine if we're missing a mapping for this package
                if (!info.AssemblyVersionInPackageVersion.TryGetValue(fileVersion, out packageVersion))
                {
                    Log.LogError($"PackageIndex from {String.Join(", ", PackageIndexes.Select(i => i.ItemSpec))} is missing an assembly version entry for {fileVersion} for package {PackageId}.  Please run /t:UpdatePackageIndex on this project to commit an update.");
                }
                else
                {
                    // determine if we have a mapping for an unstable package and that unstable package is not this one
                    if (!info.StableVersions.Contains(packageVersion) && packageVersion != thisPackageVersion)
                    {
                        Log.LogError($"PackageIndex from {String.Join(", ", PackageIndexes.Select(i => i.ItemSpec))} indicates that assembly version {fileVersion} is contained in non-stable package version {packageVersion} which differs from this package version {thisPackageVersion}.");
                    }
                }
            }

            var orphanedAssemblyVersions = info.AssemblyVersionInPackageVersion
                                                .Where(pair => pair.Value == thisPackageVersion && !assemblyVersions.Contains(pair.Key))
                                                .Select(pair => pair.Key);
            if (orphanedAssemblyVersions.Any())
            {
                Log.LogError($"PackageIndex from {String.Join(", ", PackageIndexes.Select(i => i.ItemSpec))} is has an assembly version entry(s) for {String.Join(", ", orphanedAssemblyVersions)} which are no longer in package {PackageId}.  Please run /t:UpdatePackageIndex on this project to commit an update.");
            }

            // if no assemblies are present in this package nor were ever present
            if (assemblyVersions.Count == 0 && 
                info.AssemblyVersionInPackageVersion.Count == 0)
            {
                // if in the native module map
                if (index.ModulesToPackages.Values.Any(p => p.Equals(PackageId)))
                {
                    // ensure the baseline is set
                    if (info.BaselineVersion != thisPackageVersion)
                    {
                        Log.LogError($"PackageIndex from {String.Join(", ", PackageIndexes.Select(i => i.ItemSpec))} is missing an baseline entry(s) for native module {PackageId}.  Please run /t:UpdatePackageIndex on this project to commit an update.");
                    }
                }
                else
                {
                    // not in the native module map, see if any of the modules in this package are present
                    // (with a different package, as would be the case for runtime-specific packages)
                    var moduleNames = allDlls.Select(d => Path.GetFileNameWithoutExtension(d.LocalPath));
                    var missingModuleNames = moduleNames.Where(m => !index.ModulesToPackages.ContainsKey(m));
                    if (missingModuleNames.Any())
                    {
                        Log.LogError($"PackageIndex from {String.Join(", ", PackageIndexes.Select(i => i.ItemSpec))} is missing ModulesToPackages entry(s) for {String.Join(", ", missingModuleNames)} to package {PackageId}.  Please add a an entry for the appropriate package.");
                    }
                }

            }
        }

        private static bool IsDll(string path)
        {
            return !String.IsNullOrWhiteSpace(path) && Path.GetExtension(path).Equals(".dll", StringComparison.OrdinalIgnoreCase);
        }
        
        private void LoadSupport()
        {
            _frameworks = new Dictionary<NuGetFramework, ValidationFramework>();

            // determine which TxM:RIDs should be considered for support based on Frameworks item
            foreach (var framework in Frameworks)
            {
                NuGetFramework fx;
                try
                {
                    fx = FrameworkUtilities.ParseNormalized(framework.ItemSpec);
                }
                catch (Exception ex)
                {
                    Log.LogError($"Could not parse Framework {framework.ItemSpec}. {ex}");
                    continue;
                }

                if (fx.Equals(NuGetFramework.UnsupportedFramework))
                {
                    Log.LogError($"Did not recognize {framework.ItemSpec} as valid Framework.");
                    continue;
                }

                string runtimeIdList = framework.GetMetadata("RuntimeIDs");

                if (_frameworks.ContainsKey(fx))
                {
                    Log.LogError($"Framework {fx} has been listed in Frameworks more than once.");
                    continue;
                }

                _frameworks[fx] = new ValidationFramework(fx);

                if (!String.IsNullOrWhiteSpace(runtimeIdList))
                {
                    _frameworks[fx].RuntimeIds = runtimeIdList.Split(';');
                }
            }

            // keep a list of explicitly listed supported frameworks so that we can check for conflicts.
            HashSet<NuGetFramework> explicitlySupportedFrameworks = new HashSet<NuGetFramework>(NuGetFramework.Comparer);

            // determine what version should be supported based on SupportedFramework items
            foreach (var supportedFramework in SupportedFrameworks)
            {
                NuGetFramework fx;
                string fxString = supportedFramework.ItemSpec;
                bool isExclusiveVersion = fxString.Length > 1 && fxString[0] == '[' && fxString[fxString.Length - 1] == ']';
                if (isExclusiveVersion)
                {
                    fxString = fxString.Substring(1, fxString.Length - 2);
                }

                try
                {
                    fx = FrameworkUtilities.ParseNormalized(fxString);
                }
                catch (Exception ex)
                {
                    Log.LogError($"Could not parse TargetFramework {fxString} as a SupportedFramework. {ex}");
                    continue;
                }

                if (fx.Equals(NuGetFramework.UnsupportedFramework))
                {
                    Log.LogError($"Did not recognize TargetFramework {fxString} as a SupportedFramework.");
                    continue;
                }

                Version supportedVersion;
                string version = supportedFramework.GetMetadata("Version");
                try
                {
                    supportedVersion = Version.Parse(version);
                }
                catch (Exception ex)
                {
                    Log.LogError($"Could not parse Version {version} on SupportedFramework item {supportedFramework.ItemSpec}. {ex}");
                    continue;
                }

                ValidationFramework validationFramework = null;
                if (!_frameworks.TryGetValue(fx, out validationFramework))
                {
                    Log.LogError($"SupportedFramework {fx} was specified but is not part of the Framework list to use for validation.");
                    continue;
                }


                if (explicitlySupportedFrameworks.Contains(fx))
                {
                    if (supportedVersion <= validationFramework.SupportedVersion)
                    {
                        // if we've already picked up a higher/equal version, prefer it
                        continue;
                    }
                }
                else
                {
                    explicitlySupportedFrameworks.Add(fx);
                }

                validationFramework.SupportedVersion = supportedVersion;

                if (!isExclusiveVersion)
                {
                    // find all frameworks of higher version, sorted by version ascending
                    IEnumerable<ValidationFramework> higherFrameworks = _frameworks.Values.Where(vf => vf.Framework.Framework == fx.Framework && vf.Framework.Version > fx.Version).OrderBy(vf => vf.Framework.Version);

                    // netcore50 is the last `netcore` framework, after that we use `uap`
                    if (fx.Framework == FrameworkConstants.FrameworkIdentifiers.NetCore)
                    {
                        var uapFrameworks = _frameworks.Values.Where(vf => vf.Framework.Framework == FrameworkConstants.FrameworkIdentifiers.UAP).OrderBy(vf => vf.Framework.Version);
                        higherFrameworks = higherFrameworks.Concat(uapFrameworks);
                    }


                    foreach (var higherFramework in higherFrameworks)
                    {
                        if (higherFramework.SupportedVersion != null && higherFramework.SupportedVersion > supportedVersion)
                        {
                            // found an higher framework version a higher API version, stop applying this supported version
                            break;
                        }

                        higherFramework.SupportedVersion = supportedVersion;
                    }
                }
            }


            // determine which Frameworks should support inbox

            PackageInfo packageInfo;
            if (_index != null && _index.Packages.TryGetValue(ContractName, out packageInfo))
            {
                foreach (var inboxPair in packageInfo.InboxOn.GetInboxVersions())
                {
                    if (!_frameworks.ContainsKey(inboxPair.Key))
                    {
                        _frameworks[inboxPair.Key] = new ValidationFramework(inboxPair.Key)
                        {
                            SupportedVersion = inboxPair.Value,
                            IsInbox = true
                        };
                    }
                }

                foreach (var validationFramework in _frameworks.Values)
                {
                    if (packageInfo.InboxOn.IsInbox(validationFramework.Framework,
                                                    validationFramework.SupportedVersion,
                                                    permitRevisions: HasSuppression(Suppression.PermitInboxRevsion)))
                    {
                        validationFramework.IsInbox = true;
                    }
                }
            }

            // validate netstandard frameworks supported by references
            foreach (var supportedFramework in _report.SupportedFrameworks)
            {
                var framework = NuGetFramework.Parse(supportedFramework.Key);

                if (framework.Framework != _generationIdentifier)
                {
                    continue;
                }

                Version supportedVersion;
                if (!Version.TryParse(supportedFramework.Value, out supportedVersion))
                {
                    continue;
                }

                if (!_frameworks.ContainsKey(framework))
                {
                    _frameworks[framework] = new ValidationFramework(framework)
                    {
                        SupportedVersion = supportedVersion
                    };
                }
            }
        }

        private class ValidationFramework
        {
            private static readonly string[] s_nullRidList = new string[] { null };
            public ValidationFramework(NuGetFramework framework)
            {
                Framework = framework;
                RuntimeIds = s_nullRidList;
            }

            public NuGetFramework Framework { get; }
            public string[] RuntimeIds { get; set; }

            // if null indicates the contract should not be supported.
            public Version SupportedVersion { get; set; }
            public bool IsInbox { get; set; }
            public string ShortName { get { return Framework.GetShortFolderName(); } }

            public ITaskItem ToItem()
            {
                ITaskItem item = new TaskItem(Framework.ToString());
                item.SetMetadata("ShortName", ShortName);
                item.SetMetadata("Version", SupportedVersion.ToString());
                item.SetMetadata("Inbox", IsInbox.ToString());
                item.SetMetadata("ValidatedRIDs", String.Join(";", RuntimeIds));
                return item;
            }
        }
    }
}
