// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.PackageManagement.UI
{
    internal static class IconBitmapStatusUtility
    {
        internal static bool GetIsDefaultIcon(IconBitmapStatus bitmapStatus)
        {
            return bitmapStatus == IconBitmapStatus.DefaultIcon ||
                bitmapStatus == IconBitmapStatus.DefaultIconDueToDecodingError ||
                bitmapStatus == IconBitmapStatus.DefaultIconDueToNullStream ||
                bitmapStatus == IconBitmapStatus.DefaultIconDueToRelativeUri;
        }

        internal static bool GetIsCompleted(IconBitmapStatus bitmapStatus)
        {
            switch (bitmapStatus)
            {
                case IconBitmapStatus.None:
                case IconBitmapStatus.NeedToFetch:
                case IconBitmapStatus.Fetching:
                    return false;

                default:
                    return true;
            }
        }
    }
}
