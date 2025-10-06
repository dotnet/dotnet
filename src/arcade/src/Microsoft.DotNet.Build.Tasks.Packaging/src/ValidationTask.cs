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
    public abstract class ValidationTask : BuildTask
    {
        /// <summary>
        /// Suppressions
        ///     Identity: suppression name
        ///     Value: optional semicolon-delimited list of values for the suppression
        /// </summary>
        public ITaskItem[] Suppressions { get;set; }
        

        /// <summary>
        /// JSON file describing results of validation
        /// </summary>
        public string ReportFile
        {
            get;
            set;
        }

        /// <summary>
        /// Package index files used to define package version mapping.
        /// </summary>
        public ITaskItem[] PackageIndexes { get; set; }

        /// <summary>
        /// property bag of error suppressions
        /// </summary>
        private Dictionary<Suppression, HashSet<string>> _suppressions;
        protected PackageIndex _index;
        protected PackageReport _report;

        protected void InitializeValidationTask()
        {
            LoadSuppressions();
            LoadReport();
            LoadIndex();
        }

        protected HashSet<string> GetSuppressionValues(Suppression key)
        {
            HashSet<string> values;
            _suppressions.TryGetValue(key, out values);
            return values;
        }

        protected string GetSingleSuppressionValue(Suppression key)
        {
            var values = GetSuppressionValues(key);
            return (values != null && values.Count == 1) ? values.Single() : null;
        }


        protected bool HasSuppression(Suppression key)
        {
            return _suppressions.ContainsKey(key);
        }

        protected bool HasSuppression(Suppression key, string value)
        {
            HashSet<string> values;
            if (_suppressions.TryGetValue(key, out values) && values != null)
            {
                return values.Contains(value);
            }
            return false;
        }

        protected bool HasSuppression(Suppression key, NuGetFramework framework)
        {
            HashSet<string> values;
            if (_suppressions.TryGetValue(key, out values) && values != null)
            {
                var frameworkValues = new[] { framework.DotNetFrameworkName, framework.Framework, framework.GetShortFolderName() };
                return frameworkValues.Any(fx => values.Contains(fx));
            }
            return false;
        }

        private void LoadSuppressions()
        {
            _suppressions = new Dictionary<Suppression, HashSet<string>>();

            if (Suppressions != null)
            {
                foreach(var suppression in Suppressions)
                {
                    AddSuppression(suppression.ItemSpec, suppression.GetMetadata("Value"));
                }
            }
        }

        private void AddSuppression(string keyString, string valueString)
        {
            Suppression key;
            HashSet<string> values = null;

            if (!Enum.TryParse<Suppression>(keyString, out key))
            {
                Log.LogError($"Unknown suppression {keyString}");
                return;
            }

            _suppressions.TryGetValue(key, out values);

            if (valueString != null)
            {
                var valuesToAdd = valueString.Split(';').Select(v => v.Trim());

                if (values == null)
                {
                    values = new HashSet<string>(valuesToAdd);
                }
                else
                {
                    foreach(var valueToAdd in valuesToAdd)
                    {
                        values.Add(valueToAdd);
                    }
                }
            }

            _suppressions[key] = values;
        }

        private void LoadReport()
        {
            _report = PackageReport.Load(ReportFile);
        }
        private void LoadIndex()
        {
            _index = PackageIndexes != null && PackageIndexes.Length > 0 ?
                PackageIndex.Load(PackageIndexes.Select(pi => pi.GetMetadata("FullPath"))) :
                null;
        }
    }

    public enum Suppression
    {
        /// <summary>
        /// Permits a runtime asset of the targets specified, semicolon delimited
        /// </summary>
        PermitImplementation,
        /// <summary>
        /// Permits an absent runtime asset on the targets specified, semicolon delimited
        /// </summary>
        PermitMissingImplementation,
        /// <summary>
        /// Permits a higher revision/build to still be considered as a match for an inbox assembly
        /// </summary>
        PermitInboxRevsion,
        /// <summary>
        /// Permits a lower version on specified frameworks, semicolon delimited, than the generation supported by that framework
        /// </summary>
        PermitPortableVersionMismatch,
        /// <summary>
        /// Permits a compatible API version match between ref and impl, rather than exact match
        /// </summary>
        PermitHigherCompatibleImplementationVersion,
        /// <summary>
        /// Permits a non-matching targetFramework asset to be used even when a matching one exists.
        /// </summary>
        PermitRuntimeTargetMonikerMismatch,
        /// <summary>
        /// Permits an assembly to be inbox even if it is missing inbox data from the PackageIndex.  This is used for cases where we specifically want implementation-only assemblies.
        /// </summary>
        PermitInbox,
        /// <summary>
        /// Permits an inbox assembly to be missing from framework package.  This is used for cases where the assembly is part of the framework itself (eg: in desktop).
        /// </summary>
        PermitMissingInbox,
        /// <summary>
        /// Treats an assembly as out of box on the given frameworks, but it must provide an assembly version that is compatible with the inbox version (>=)
        /// </summary>
        TreatAsOutOfBox
    }
}
