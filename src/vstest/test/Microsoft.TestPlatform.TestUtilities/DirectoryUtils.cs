// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;

using IO = System.IO;

namespace Microsoft.TestPlatform.TestUtilities;

public static class DirectoryUtils
{
    public static void CopyDirectory(string sourceDirectory, string targetDirectory)
    {
        CopyDirectory(new DirectoryInfo(sourceDirectory), new DirectoryInfo(targetDirectory));
    }

    public static void CopyDirectory(DirectoryInfo source, DirectoryInfo target)
    {
        Directory.CreateDirectory(target.FullName);

        // Copy each file into the new directory.
        foreach (FileInfo fi in source.GetFiles())
        {
            fi.CopyTo(IO.Path.Combine(target.FullName, fi.Name), true);
        }

        // Copy each subdirectory using recursion.
        foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
        {
            DirectoryInfo nextTargetSubDir = target.CreateSubdirectory(diSourceSubDir.Name);
            CopyDirectory(diSourceSubDir, nextTargetSubDir);
        }
    }
}
