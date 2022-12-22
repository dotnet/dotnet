// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;

using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;

namespace Microsoft.VisualStudio.TestPlatform.Common.Utilities;

public class InstallationContext
{
    private const string DevenvExe = "devenv.exe";
    private const string PrivateAssembliesDirName = "PrivateAssemblies";
    private const string PublicAssembliesDirName = "PublicAssemblies";

    private readonly IFileHelper _fileHelper;

    public InstallationContext(IFileHelper fileHelper)
    {
        _fileHelper = fileHelper;
    }

    public bool TryGetVisualStudioDirectory(out string visualStudioDirectory)
    {
        var vsInstallPath = new DirectoryInfo(typeof(InstallationContext).GetTypeInfo().Assembly.GetAssemblyLocation()).Parent?.Parent?.Parent?.FullName;
        if (!vsInstallPath.IsNullOrEmpty())
        {
            var pathToDevenv = Path.Combine(vsInstallPath, DevenvExe);
            if (!pathToDevenv.IsNullOrEmpty() && _fileHelper.Exists(pathToDevenv))
            {
                visualStudioDirectory = vsInstallPath;
                return true;
            }
        }

        visualStudioDirectory = string.Empty;
        return false;
    }

    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Part of the public API")]
    public string GetVisualStudioPath(string visualStudioDirectory)
    {
        return Path.Combine(visualStudioDirectory, DevenvExe);
    }

    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Part of the public API")]
    public string[] GetVisualStudioCommonLocations(string visualStudioDirectory)
    {
        return new[]
        {
            Path.Combine(visualStudioDirectory, PrivateAssembliesDirName),
            Path.Combine(visualStudioDirectory, PublicAssembliesDirName),
            Path.Combine(visualStudioDirectory, "CommonExtensions", "Microsoft", "TestWindow"),
            Path.Combine(visualStudioDirectory, "CommonExtensions", "Microsoft", "TeamFoundation", "Team Explorer"),
            visualStudioDirectory
        };
    }
}
