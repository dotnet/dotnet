// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.Build.Framework;
using Newtonsoft.Json;
using NuGet.Commands;
using NuGet.Common;
using NuGet.ProjectModel;
using Task = Microsoft.Build.Utilities.Task;


namespace NuGet.Build.Tasks
{
    public class CheckForDuplicateNuGetItemsTask : Task
    {
        [Required]
        public ITaskItem[] Items { get; set; }

        [Required]
        public string ItemName { get; set; }

        [Required]
        public string LogCode { get; set; }

        [Required]
        public string MSBuildProjectFullPath { get; set; }

        public string TreatWarningsAsErrors { get; set; }

        public string WarningsAsErrors { get; set; }

        public string WarningsNotAsErrors { get; set; }

        public string NoWarn { get; set; }

        [Output]
        public ITaskItem[] DeduplicatedItems { get; set; }

        public override bool Execute()
        {
            DeduplicatedItems = Array.Empty<ITaskItem>();
            var itemGroups = Items.GroupBy(i => i.ItemSpec, StringComparer.OrdinalIgnoreCase);
            var duplicateItems = itemGroups.Where(g => g.Count() > 1).ToList();

            if (duplicateItems.Any())
            {
                var logger = new PackCollectorLogger(
                    new MSBuildLogger(Log),
                    EvaluateWarningProperties(WarningsAsErrors, NoWarn, TreatWarningsAsErrors, WarningsNotAsErrors)
                    );
                string duplicateItemsFormatted = string.Join("; ", duplicateItems.Select(d => string.Join(", ", d.Select(e => $"{e.ItemSpec} {e.GetMetadata("version")}"))));
                NuGetLogCode logCode = (NuGetLogCode)Enum.Parse(typeof(NuGetLogCode), LogCode);

                logger.Log(new RestoreLogMessage(
                    LogLevel.Warning,
                    logCode,
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Strings.Error_DuplicateItems,
                        ItemName,
                        duplicateItemsFormatted))
                {
                    FilePath = MSBuildProjectFullPath,
                });

                // Set Output
                DeduplicatedItems = itemGroups.Select(g => g.First()).ToArray();
            }

            return !Log.HasLoggedErrors;
        }

        private WarningProperties EvaluateWarningProperties(string warningsAsErrors, string noWarn, string treatWarningsAsErrors, string warningsNotAsErrors)
        {
            var warnAsErrorCodes = new HashSet<NuGetLogCode>();
            ReadNuGetLogCodes(warningsAsErrors, warnAsErrorCodes);
            var noWarnCodes = new HashSet<NuGetLogCode>();
            ReadNuGetLogCodes(noWarn, noWarnCodes);
            _ = bool.TryParse(treatWarningsAsErrors, out bool allWarningsAsErrors);
            var warningNotAsErrorsCodes = new HashSet<NuGetLogCode>();
            ReadNuGetLogCodes(warningsNotAsErrors, warningNotAsErrorsCodes);

            return new WarningProperties(warnAsErrorCodes, noWarnCodes, allWarningsAsErrors, warningNotAsErrorsCodes);
        }

        private static void ReadNuGetLogCodes(string str, HashSet<NuGetLogCode> hashCodes)
        {
            foreach (var code in MSBuildStringUtility.Split(str))
            {
                if (Enum.TryParse(code, out NuGetLogCode logCode))
                {
                    hashCodes.Add(logCode);
                }
            }
        }
    }
}
