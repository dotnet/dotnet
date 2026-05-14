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
    public class ValidateFrameworkPackage : ValidationTask
    {
        [Required]
        public string Framework
        {
            get;
            set;
        }
        
        public string Runtime
        {
            get;
            set;
        }

        public override bool Execute()
        {
            InitializeValidationTask();

            var fx = NuGetFramework.Parse(Framework);

            var testAssets = GetTestAssets(fx, Runtime);
            var testAssetsByName = testAssets.Where(a => Path.GetExtension(a.PackagePath) == ".dll")
                                             .ToDictionary(a => Path.GetFileNameWithoutExtension(a.PackagePath), a => a);

            var permittedInbox = GetSuppressionValues(Suppression.PermitInbox) ?? new HashSet<string>();

            foreach (var testAssetByName in testAssetsByName)
            {
                var name = testAssetByName.Key;
                var testAsset = testAssetByName.Value;

                var logMissingInbox = permittedInbox.Contains(name) ? 
                    new Action<string>(s => Log.LogMessage(LogImportance.Low, $"Suppressed: {s}")) :
                    new Action<string>(s => Log.LogError(s));

                PackageInfo packageInfo;
                if (!_index.Packages.TryGetValue(name, out packageInfo))
                {
                    logMissingInbox($"File {name} was included framework package {_report.Id}/{_report.Version} but that file is missing from package index {string.Join(";", _index.IndexSources)}.  Please add it with appropriate {nameof(PackageInfo.InboxOn)} entry for {Framework} or suppress this message with {nameof(Suppression.PermitInbox)} suppression.");
                    continue;
                }

                if (!packageInfo.InboxOn.IsInbox(fx, testAsset.Version))
                {
                    logMissingInbox($"File {name}, version {testAsset.Version} was included framework package {_report.Id}/{_report.Version} but that version is not considered inbox in package index {string.Join(";", _index.IndexSources)}.  Please add it with appropriate {nameof(PackageInfo.InboxOn)} entry for {Framework} or suppress this message with {nameof(Suppression.PermitInbox)} suppression.");
                    continue;
                }
            }

            var permittedMissingInbox = GetSuppressionValues(Suppression.PermitMissingInbox) ?? new HashSet<string>();
            
            var missingInboxAssemblies = _index.Packages.Where(packageInfo => packageInfo.Value.InboxOn.IsAnyVersionInbox(fx) && !testAssetsByName.ContainsKey(packageInfo.Key));

            foreach(var missingInboxAssembly in missingInboxAssemblies)
            {
                var logMissingPackage = permittedMissingInbox.Contains(missingInboxAssembly.Key) ?
                    new Action<string>(s => Log.LogMessage(LogImportance.Low, $"Suppressed: {s}")) :
                    new Action<string>(s => Log.LogError(s));

                logMissingPackage($"File {missingInboxAssembly.Key}.dll is marked as inbox for framework {Framework} but was missing from framework package {_report.Id}/{_report.Version}.  Either add the file or update {nameof(PackageInfo.InboxOn)} entry in {string.Join(";", _index.IndexSources)}.   This may be suppressed with {nameof(Suppression.PermitMissingInbox)} suppression");
            }
            
            return !Log.HasLoggedErrors;
        }

        private IEnumerable<PackageAsset> GetTestAssets(NuGetFramework fx, string runtimeId)
        {

            var targetKey = string.IsNullOrEmpty(runtimeId) ? fx.ToString() : $"{fx}/{runtimeId}";

            Target target = null;
            if (!_report.Targets.TryGetValue(targetKey, out target))
            {
                Log.LogError($"Could not find target {targetKey} in {ReportFile}.");
                return Enumerable.Empty<PackageAsset>();
            }

            return string.IsNullOrEmpty(Runtime) ? target.CompileAssets : target.RuntimeAssets;
        }
    }

}
