using System;
using Microsoft.VisualStudio.Imaging.Interop;

namespace NuGet.PackageManagement.UI
{
    public static class PackageIconMonikers
    {
        private static readonly Guid ManifestGuid = new Guid("8F5EAE8F-9892-4CE2-826C-764BEDE6D2EC");
        private const int _prefixReservedIndicator = 1;
        private const int _updateAvailableIndicator = 2;
        private const int _uninstallIndicator = 3;
        private const int _downloadIndicator = 4;

        public static ImageMoniker PrefixReservedIndicator => new ImageMoniker { Guid = ManifestGuid, Id = _prefixReservedIndicator };
        public static ImageMoniker UpdateAvailableIndicator => new ImageMoniker { Guid = ManifestGuid, Id = _updateAvailableIndicator };
        public static ImageMoniker UninstallIndicator => new ImageMoniker { Guid = ManifestGuid, Id = _uninstallIndicator };
        public static ImageMoniker DownloadIndicator => new ImageMoniker { Guid = ManifestGuid, Id = _downloadIndicator };
    }
}
