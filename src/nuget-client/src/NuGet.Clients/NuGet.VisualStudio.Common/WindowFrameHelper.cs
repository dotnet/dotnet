// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace NuGet.VisualStudio
{
    public static class WindowFrameHelper
    {
        public static void AddF1HelpKeyword(IVsWindowFrame windowFrame, string keywordValue)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            // Set F1 help keyword
            object varUserContext = null;
            if (ErrorHandler.Succeeded(windowFrame.GetProperty((int)__VSFPROPID.VSFPROPID_UserContext, out varUserContext)))
            {
                var userContext = varUserContext as IVsUserContext;
                if (userContext != null)
                {
                    userContext.AddAttribute(VSUSERCONTEXTATTRIBUTEUSAGE.VSUC_Usage_LookupF1, "keyword", keywordValue);
                }
            }
        }

        public static void DisableWindowAutoReopen(IVsWindowFrame windowFrame)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            ErrorHandler.ThrowOnFailure(windowFrame.SetProperty((int)__VSFPROPID5.VSFPROPID_DontAutoOpen, true));
        }

        public static void DockToolWindow(IVsWindowFrame windowFrame)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            ErrorHandler.ThrowOnFailure(windowFrame.SetProperty((int)__VSFPROPID.VSFPROPID_FrameMode, VSFRAMEMODE.VSFM_MdiChild));
        }
    }
}
