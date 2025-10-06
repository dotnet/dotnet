// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Threading;
using NuGet.Commands;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.PackageManagement.Telemetry;
using NuGet.PackageManagement.UI;
using NuGet.PackageManagement.UI.ViewModels;
using NuGet.PackageManagement.VisualStudio;
using NuGet.VisualStudio;
using NuGet.VisualStudio.Telemetry;
using NuGetVSExtension;
using Resx = NuGet.PackageManagement.UI.Resources;

namespace NuGet.Tools.Commands
{
    internal sealed class ClearNuGetLocalResourcesCommand
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandID = PkgCmdIDList.cmdidClearNuGetLocalResources;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = GuidList.guidClearNuGetLocalResourcesCmdSet;

        private readonly OleMenuCommandService _oleMenuCommandService;
        private readonly Lazy<INuGetUILogger> _outputConsoleLogger;

        public INuGetUILogger OutputConsoleLogger => _outputConsoleLogger.Value;

        public ClearNuGetLocalResourcesCommand(OleMenuCommandService oleMenuCommandService, Lazy<INuGetUILogger> outputConsoleLogger)
        {
            _oleMenuCommandService = oleMenuCommandService ?? throw new ArgumentNullException(nameof(oleMenuCommandService));
            _outputConsoleLogger = outputConsoleLogger ?? throw new ArgumentNullException(nameof(outputConsoleLogger));
        }

        public void Initialize()
        {
            var clearNuGetLocalResourcesCommandID = new CommandID(CommandSet, CommandID);
            var clearNuGetLocalResourcesCommand = new OleMenuCommand(ExecuteClearNuGetLocalResourcesCommand, clearNuGetLocalResourcesCommandID);
            _oleMenuCommandService.AddCommand(clearNuGetLocalResourcesCommand);
        }

        private void ExecuteClearNuGetLocalResourcesCommand(object sender, EventArgs e)
        {
            NuGetUIThreadHelper.JoinableTaskFactory.RunAsync(async () =>
            {
                try
                {
                    await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                    var isUserContinuing = MessageHelper.ShowQueryMessage(
                        message: Resx.VSOptions_Text_ClearLocalsPromptMessage,
                        title: Resx.VSOptions_Text_ClearLocalsPromptTitle,
                        showCancelButton: false,
                        defaultButton: OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_SECOND);

                    NavigatedTelemetryEvent evt;

                    if (isUserContinuing == true)
                    {
                        var clearNuGetLocalsViewModel = new ClearNuGetLocalsViewModel(ClearNuGetLocalsCommandExecuteAsync);
                        OutputConsoleLogger.Start();
                        var clearNuGetLocalResourcesWindow = new ClearNuGetLocalResourcesWindow(clearNuGetLocalsViewModel);
                        clearNuGetLocalResourcesWindow.ShowModal();
                        evt = NavigatedTelemetryEvent.CreateWithClearLocalsCommand(isUnifiedSettings: true, isPromptCancelled: false);
                    }
                    else
                    {
                        evt = NavigatedTelemetryEvent.CreateWithClearLocalsCommand(isUnifiedSettings: true, isPromptCancelled: true);
                    }
                    TelemetryActivity.EmitTelemetryEvent(evt);
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);
                    ActivityLog.LogError(NuGetUI.LogEntrySource, ex.ToString());
                    throw ex;
                }
                finally
                {
                    OutputConsoleLogger.End();
                }
            }).PostOnFailure(nameof(NuGetPackage), nameof(ExecuteClearNuGetLocalResourcesCommand));
        }

        public async Task ClearNuGetLocalsCommandExecuteAsync()
        {
            try
            {
                await TaskScheduler.Default;
                await ExecuteLocalsCommandRunnerAsync();
            }
            catch (Exception ex)
            {
                LogError(ex.Message);
                ActivityLog.LogError(NuGetUI.LogEntrySource, ex.ToString());
                throw ex;
            }
            finally
            {
                OutputConsoleLogger.End();
            }
        }

        private async Task ExecuteLocalsCommandRunnerAsync()
        {
            await TaskScheduler.Default;
            var arguments = new List<string> { "all" };
            var settings = await ServiceLocator.GetComponentModelServiceAsync<ISettings>();
            var logError = new LocalsArgs.Log(LogError);
            var logInformation = new LocalsArgs.Log(LogInformation);
            var localsArgs = new LocalsArgs(arguments, settings, logInformation, logError, clear: true, list: false);

            LocalsCommandRunner localsCommandRunner = new();
            localsCommandRunner.ExecuteCommand(localsArgs);
        }

        private void LogError(string message)
        {
            OutputConsoleLogger.Log(new LogMessage(LogLevel.Error, message));
        }

        private void LogInformation(string message)
        {
            OutputConsoleLogger.Log(new LogMessage(LogLevel.Information, message));
        }
    }
}
