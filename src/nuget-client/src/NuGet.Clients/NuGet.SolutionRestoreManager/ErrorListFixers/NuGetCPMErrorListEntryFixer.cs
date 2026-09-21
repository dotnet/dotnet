// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;
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
    [Name(NuGetErrorListFixerConstants.CPMFixerName)]
    [Order(Before = NuGetErrorListFixerConstants.CopilotFixerName)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    internal sealed class NuGetCPMErrorListEntryFixer : NuGetErrorListEntryFixerBase
    {
        private static readonly HashSet<string> SupportedCodes = new(StringComparer.OrdinalIgnoreCase)
        {
            NuGetLogCode.NU1507.ToString(),
        };

        [Import(typeof(IResolveSupplyChainSecurityService), AllowDefault = true)]
        public Lazy<IResolveSupplyChainSecurityService>? ResolveSupplyChainSecurityService { get; set; }

        public override string Tooltip => Resources.Title_AskCopilotForFix;

        protected override IErrorListEntryInspector EntryInspector { get; } = new SupportedCodesErrorListInspector(SupportedCodes);

        protected override bool TryFixCore(ITableEntryHandle entry)
        {
            if (ResolveSupplyChainSecurityService == null
                || !SupportedCodesErrorListInspector.TryGetErrorCode(entry, out string errorCode))
            {
                return false;
            }

            NuGetUIThreadHelper.JoinableTaskFactory.RunAsync(async () =>
            {
                await ResolveSupplyChainSecurityService.Value.LaunchResolveAsync(
                    source: ResolveSupplyChainSecuritySource.ErrorList,
                    prompt: string.Format(
                        CultureInfo.CurrentCulture,
                        Resources.Prompt_ResolveSupplyChainSecurityNUCode,
                        errorCode),
                    cancellationToken: CancellationToken.None);
            }).PostOnFailure(nameof(NuGetCPMErrorListEntryFixer), nameof(TryFixCore));

            return true;
        }
    }
}
