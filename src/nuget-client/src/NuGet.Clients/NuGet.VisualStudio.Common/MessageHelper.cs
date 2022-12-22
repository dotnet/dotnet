// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using NuGet.Common;

namespace NuGet.VisualStudio
{
    public static class MessageHelper
    {
        public static void ShowWarningMessage(string message, string title)
        {
            VsShellUtilities.ShowMessageBox(
                GetServiceProvider(),
                message,
                title,
                OLEMSGICON.OLEMSGICON_WARNING,
                OLEMSGBUTTON.OLEMSGBUTTON_OK,
                OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
        }

        public static void ShowInfoMessage(string message, string title)
        {
            VsShellUtilities.ShowMessageBox(
                GetServiceProvider(),
                message,
                title,
                OLEMSGICON.OLEMSGICON_INFO,
                OLEMSGBUTTON.OLEMSGBUTTON_OK,
                OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
        }

        public static void ShowErrorMessage(Exception exception, string title)
        {
            ShowErrorMessage(ExceptionUtilities.Unwrap(exception).Message, title);
        }

        public static void ShowErrorMessage(string message, string title)
        {
            VsShellUtilities.ShowMessageBox(
                GetServiceProvider(),
                message,
                title,
                OLEMSGICON.OLEMSGICON_CRITICAL,
                OLEMSGBUTTON.OLEMSGBUTTON_OK,
                OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
        }

        public static bool? ShowQueryMessage(string message, string title, bool showCancelButton)
        {
            int result = VsShellUtilities.ShowMessageBox(
                GetServiceProvider(),
                message,
                title,
                OLEMSGICON.OLEMSGICON_QUERY,
                showCancelButton ? OLEMSGBUTTON.OLEMSGBUTTON_YESNOCANCEL : OLEMSGBUTTON.OLEMSGBUTTON_YESNO,
                OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);

            if (result == NativeMethods.IDCANCEL)
            {
                return null;
            }
            return (result == NativeMethods.IDYES);
        }

        public static void ShowError(ErrorListProvider errorListProvider, TaskErrorCategory errorCategory, TaskPriority priority, string errorText, IVsHierarchy hierarchyItem)
        {
            ErrorTask errorTask = new ErrorTask();
            errorTask.Text = errorText;
            errorTask.ErrorCategory = errorCategory;
            errorTask.Category = TaskCategory.BuildCompile;
            errorTask.Priority = priority;
            errorTask.HierarchyItem = hierarchyItem;
            errorListProvider.Tasks.Add(errorTask);
            errorListProvider.BringToFront();
            errorListProvider.ForceShowErrors();
        }

        private static IServiceProvider GetServiceProvider()
        {
            return NuGetUIThreadHelper.JoinableTaskFactory.Run(() => ServiceLocator.GetServiceProviderAsync());
        }

        internal static class NativeMethods
        {
            public const int IDCANCEL = 2;
            public const int IDYES = 6;
            public const int IDNO = 7;
        }
    }
}
