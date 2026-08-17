// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Design;
using System.Threading;
using Microsoft.VisualStudio.Shell;
using NuGet.PackageManagement.VisualStudio;
using NuGet.VisualStudio;
using NuGet.VisualStudio.Telemetry;
using NuGetVSExtension;

namespace NuGet.Tools.Commands
{
    internal sealed class PackageSourceMapperCommand
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandID = PkgCmdIDList.cmdidReviewPackageSourceMapping;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = GuidList.guidReviewPackageSourceMappingCmdSet;

        private readonly OleMenuCommandService _oleMenuCommandService;
        private readonly Lazy<IPackageSourceMappingService> _packageSourceMappingService;
        private readonly Lazy<IVsSolutionManager> _solutionManager;

        public PackageSourceMapperCommand(
            OleMenuCommandService oleMenuCommandService,
            Lazy<IPackageSourceMappingService> packageSourceMappingService,
            Lazy<IVsSolutionManager> solutionManager)
        {
            _oleMenuCommandService = oleMenuCommandService ?? throw new ArgumentNullException(nameof(oleMenuCommandService));
            _packageSourceMappingService = packageSourceMappingService ?? throw new ArgumentNullException(nameof(packageSourceMappingService));
            _solutionManager = solutionManager ?? throw new ArgumentNullException(nameof(solutionManager));
        }

        public void Initialize()
        {
            var commandId = new CommandID(CommandSet, CommandID);
            var command = new OleMenuCommand(ExecutePackageSourceMapperCommand, changeHandler: null, BeforeQueryStatus, commandId);
            _oleMenuCommandService.AddCommand(command);
        }

        // The Review Package Source Mapping command drives GitHub Copilot and the NuGet MCP tool using the current
        // solution as context, which does not work without an open solution. Hide the button when no solution is open.
        private void BeforeQueryStatus(object sender, EventArgs e)
        {
            var command = (OleMenuCommand)sender;
            bool isSolutionOpen = _solutionManager.Value.IsSolutionOpen;
            command.Visible = isSolutionOpen;
            command.Enabled = isSolutionOpen;
        }

        private void ExecutePackageSourceMapperCommand(object sender, EventArgs e)
        {
            NuGetUIThreadHelper.JoinableTaskFactory.RunAsync(() =>
                _packageSourceMappingService.Value.LaunchReviewPackageSourceMappingAsync(CancellationToken.None))
                .PostOnFailure(nameof(NuGetPackage), nameof(ExecutePackageSourceMapperCommand));
        }
    }
}
