// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Linq;
using Microsoft.DotNet.VersionTools.Automation;

namespace Microsoft.DotNet.VersionTools.Cli;


/// <summary>
/// Utilize Microsoft.DotNet.VersionTool to remove the version from the nuget asset file name.
/// </summary>
public class VersionTrimmingOperation : IOperation
{
    public struct Context
    {
        public INupkgInfoFactory NupkgInfoFactory { get; set; }
        public IFileProxy FileProxy { get; set; }
        public IDirectoryProxy DirectoryProxy { get; set; }
        public ILogger Logger { get; set; }

        public string AssetsDirectory { get; set; }
        public string SearchPattern { get; set; }
        public bool Recursive { get; set; }
    }

    private readonly Context _context;

    public VersionTrimmingOperation(Context context) => _context = context;

    public IOperation.ExitCode Execute()
    {
        if (!_context.DirectoryProxy.Exists(_context.AssetsDirectory))
        {
            _context.Logger.WriteError($"{_context.AssetsDirectory} does not exist");
            return IOperation.ExitCode.ErrorFileNotFount;
        }

        var assets = _context.DirectoryProxy.GetFiles(
            _context.AssetsDirectory,
            _context.SearchPattern,
            _context.Recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
        if (assets.Any(a => Path.GetExtension(a) != ".nupkg"))
        {
            throw new NotImplementedException("Version trimming applies only to NuGet assets. " +
                $"The search pattern `{_context.SearchPattern}` includes non-NuGet assets.");
        }

        foreach (var assetFileName in assets)
        {
            NupkgInfo info = null;
            try
            {
                info = _context.NupkgInfoFactory.CreateNupkgInfo(assetFileName);
            }
            catch (Exception e)
            {
                _context.Logger.WriteError($"Asset {assetFileName} in not a valid nuget package: {e.Message}");
                continue;
            }

            string newAssetFileName = assetFileName.Replace($".{info.Version}", null);
            if (assetFileName == newAssetFileName) continue;

            _context.Logger.WriteMessage($"Processing {assetFileName} ...");

            try
            {
                _context.FileProxy.Move(assetFileName, newAssetFileName);
            }
            catch (Exception e)
            {
                _context.Logger.WriteError($"Version trimming of {assetFileName} failed: {e.Message}");
                continue;
            }
        }

        return IOperation.ExitCode.Success;
    }
}
