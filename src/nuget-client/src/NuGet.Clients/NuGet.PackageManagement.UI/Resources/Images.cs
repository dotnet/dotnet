// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace NuGet.PackageManagement.UI
{
    internal static class Images
    {
        public static readonly BitmapSource DefaultPackageIcon;

        static Images()
        {
            // in VS, look up the icon via pack://application url
            if (Application.Current != null)
            {
                var image = new BitmapImage();
                image.BeginInit();

                // If the DLL name changes, this URI would need to change to match.
                image.UriSource = new Uri("pack://application:,,,/NuGet.PackageManagement.UI;component/Resources/packageicon.png");

                // Instead of scaling larger images and keeping larger image in memory, this makes it so we scale it down, and throw away the bigger image.
                // Only need to set this on one dimension, to preserve aspect ratio
                image.DecodePixelWidth = PackageItemViewModel.DecodePixelWidth;

                image.EndInit();
                image.Freeze();
                DefaultPackageIcon = image;
            }
            else // for tests, don't actually load the icon, just use a 32x32 image.
            {
                const int size = PackageItemViewModel.DecodePixelWidth;
                var bytes = new List<byte>();
                byte[] pixel = new byte[] { 0, 0, 255, 0 };
                for (int y = 0; y < size; y++)
                {
                    for (int x = 0; x < size; x++)
                    {
                        bytes.AddRange(pixel);
                    }
                }

                BitmapSource image = BitmapSource.Create(
                    pixelWidth: size,
                    pixelHeight: size,
                    dpiX: 96.0,
                    dpiY: 96.0,
                    PixelFormats.Bgr32,
                    palette: null,
                    pixels: bytes.ToArray(),
                    stride: 4 * size);
                image.Freeze();
                DefaultPackageIcon = image;
            }
        }
    }
}
