// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Task = System.Threading.Tasks.Task;

namespace API.Test
{
    public static class VSHelper
    {
        public static string GetVSVersion()
        {
            return ThreadHelper.JoinableTaskFactory.Run(() => GetVSVersionAsync());
        }

        private static EnvDTE.Window PSWindow = null;

        public static void StorePSWindow()
        {
            ThreadHelper.JoinableTaskFactory.Run(() => StorePSWindowAsync());
        }

        private static async Task StorePSWindowAsync()
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            var dte = ServiceLocator.GetDTE();
            PSWindow = dte.ActiveWindow;
        }

        public static void FocusStoredPSWindow()
        {
            ThreadHelper.JoinableTaskFactory.Run(() => FocusStoredPSWindowAsync());
        }

        private static async Task FocusStoredPSWindowAsync()
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            PSWindow?.SetFocus();
            PSWindow = null;
        }

        private static async Task<string> GetVSVersionAsync()
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            var dte = ServiceLocator.GetDTE();
            var version = dte.Version;

            return version;
        }

        public static string GetBuildOutput()
        {
            return ThreadHelper.JoinableTaskFactory.Run(() => GetBuildOutputAsync());
        }

        private static string BuildOutputPaneName = "Build";
        private static async Task<string> GetBuildOutputAsync()
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            var dte = ServiceLocator.GetDTE();
            var dte2 = (DTE2)dte;
            var buildPane = dte2.ToolWindows.OutputWindow.OutputWindowPanes.Item(BuildOutputPaneName);
            var doc = buildPane.TextDocument;
            var sel = doc.Selection;
            sel.StartOfDocument(Extend: false);
            sel.EndOfDocument(Extend: true);
            var text = sel.Text;
            return text;
        }

        public static void NewTextFile()
        {
            ThreadHelper.JoinableTaskFactory.Run(() => NewTextFileAsync());
        }

        private static async Task NewTextFileAsync()
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            var dte = ServiceLocator.GetDTE();
            dte.ItemOperations.NewFile("General\\Text File");
            dte.ActiveDocument.Object("TextDocument");
        }

        private static string ErrorListWindowCaption = "Error List";
        private static async Task<string[]> GetErrorTasksAsync(vsBuildErrorLevel errorLevel)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            var dte = ServiceLocator.GetDTE();
            dte.ExecuteCommand("View.ErrorList", " ");

            EnvDTE.Window errorListWindow = null;
            foreach (EnvDTE.Window window in dte.Windows)
            {
                if (window.Caption.StartsWith(ErrorListWindowCaption, System.StringComparison.OrdinalIgnoreCase))
                {
                    errorListWindow = window;
                    break;
                }
            }

            if (errorListWindow == null)
            {
                throw new InvalidOperationException("Unable to locate the error list");
            }

            var errorList = errorListWindow.Object as ErrorList;
            if (errorList == null)
            {
                throw new InvalidOperationException("Unable to retrieve the error list");
            }

            errorList.ShowErrors = true;
            errorList.ShowWarnings = true;
            errorList.ShowMessages = true;

            var errorItems = errorList.ErrorItems as ErrorItems;
            if (errorItems == null)
            {
                throw new InvalidOperationException("Unable to retrieve the error list items");
            }

            var errorTasks = new List<ErrorItem>();

            for (int i = 1; i <= errorItems.Count; i++)
            {
                var errorItem = errorItems.Item(i);
                var currentErrorLevel = (vsBuildErrorLevel)errorItem.ErrorLevel;
                if (currentErrorLevel == errorLevel)
                {
                    errorTasks.Add(errorItem);
                }
            }

            var items = errorTasks.Select(e => e.Description as string).ToArray();
            return items;
        }

        public static string[] GetErrors()
        {
            return ThreadHelper.JoinableTaskFactory.Run(() => GetErrorsAsync());
        }

        private static async Task<string[]> GetErrorsAsync()
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            var errorLevel = vsBuildErrorLevel.vsBuildErrorLevelHigh;
            var errors = await GetErrorTasksAsync(errorLevel);
            return errors;
        }

        public static string[] GetWarnings()
        {
            return ThreadHelper.JoinableTaskFactory.Run(() => GetWarningsAsync());
        }

        private static async Task<string[]> GetWarningsAsync()
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            var errorLevel = vsBuildErrorLevel.vsBuildErrorLevelMedium;
            var warnings = await GetErrorTasksAsync(errorLevel);
            return warnings;
        }
    }
}
