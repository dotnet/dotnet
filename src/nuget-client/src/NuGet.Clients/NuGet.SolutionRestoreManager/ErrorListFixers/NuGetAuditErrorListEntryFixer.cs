// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading;
using Microsoft.Internal.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.TableControl;
using Microsoft.VisualStudio.Shell.TableManager;
using Microsoft.VisualStudio.Utilities;
using NuGet.Common;
using NuGet.VisualStudio;
using NuGet.VisualStudio.Telemetry;

namespace NuGet.SolutionRestoreManager.ErrorListFixers
{
#pragma warning disable CS0618 // Obsolete in VS because it "may change without warning". It remains the only Error List fixer extensibility point today.
    [Export(typeof(IErrorListEntryFixer))]
#pragma warning restore CS0618
    [DataSource(StandardTableDataSources.ErrorTableDataSource)]
    [Name(NuGetErrorListFixerConstants.AuditFixerName)]
    [Order(Before = NuGetErrorListFixerConstants.CopilotFixerName)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    internal sealed class NuGetAuditErrorListEntryFixer : NuGetErrorListEntryFixerBase
    {
        private static readonly HashSet<string> SupportedAuditCodes = new(StringComparer.OrdinalIgnoreCase)
        {
            NuGetLogCode.NU1901.ToString(),
            NuGetLogCode.NU1902.ToString(),
            NuGetLogCode.NU1903.ToString(),
            NuGetLogCode.NU1904.ToString(),
        };

        [Import(typeof(IFixVulnerabilitiesService), AllowDefault = true)]
        public Lazy<IFixVulnerabilitiesService>? FixVulnerabilitiesService { get; set; }

        public override string Tooltip => Resources.Title_FixVulnerabilitiesWithCopilot;

        protected override IErrorListEntryInspector EntryInspector { get; } = new SupportedCodesErrorListInspector(SupportedAuditCodes);

        protected override bool TryFixCore(ITableEntryHandle entry)
        {
            if (FixVulnerabilitiesService == null)
            {
                return false;
            }

            NuGetUIThreadHelper.JoinableTaskFactory.RunAsync(async () =>
            {
                await FixVulnerabilitiesService.Value.LaunchFixVulnerabilitiesAsync(
                    FixVulnerabilitiesSource.ErrorList,
                    CancellationToken.None);
            }).PostOnFailure(nameof(NuGetAuditErrorListEntryFixer), nameof(TryFixCore));

            return true;
        }
    }
}
