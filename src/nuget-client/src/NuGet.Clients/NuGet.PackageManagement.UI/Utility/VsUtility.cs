using System.Collections.Generic;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace NuGet.PackageManagement.UI
{
    public static class VsUtility
    {
        public static IEnumerable<IVsWindowFrame> GetDocumentWindows(IVsUIShell uiShell)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            IEnumWindowFrames documentWindowEnumerator;
            var hr = uiShell.GetDocumentWindowEnum(out documentWindowEnumerator);
            if (documentWindowEnumerator == null)
            {
                yield break;
            }

            var windowFrames = new IVsWindowFrame[1];
            uint frameCount;
            while (documentWindowEnumerator.Next(1, windowFrames, out frameCount) == VSConstants.S_OK &&
                   frameCount == 1)
            {
                yield return windowFrames[0];
            }
        }

        // Gets the package manager control hosted in the window frame.
        public static PackageManagerControl GetPackageManagerControl(IVsWindowFrame windowFrame)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            object property;
            var hr = windowFrame.GetProperty(
                (int)__VSFPROPID.VSFPROPID_DocView,
                out property);

            var windowPane = property as PackageManagerWindowPane;
            if (windowPane == null)
            {
                return null;
            }

            var packageManagerControl = windowPane.Content as PackageManagerControl;
            return packageManagerControl;
        }
    }
}
