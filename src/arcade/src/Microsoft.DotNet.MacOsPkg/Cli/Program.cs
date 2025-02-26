// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

#if !NET

// This code is unreachable. Here to keep the compiler happy.
throw new PlatformNotSupportedException("This tool is only supported on .NET Core.");

#else

using System.Runtime.InteropServices;
using System.CommandLine;
using System.IO;
using Microsoft.DotNet.MacOsPkg.Core;

namespace Microsoft.DotNet.MacOsPkg.Cli;

public class Program
{
    public static int Main(string[] args)
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            Console.Error.WriteLine("This tool is only supported on macOS.");
            return 1;
        }

        CliRootCommand rootCommand = Setup();
        return new CliConfiguration(rootCommand).Invoke(args);
    }

    /// <summary>
    /// Set up the command line interface and associated actions.
    /// </summary>
    /// <returns>Root cli command</returns>
    private static CliRootCommand Setup()
    {
        var rootCommand = new CliRootCommand();
        var unpackSrcArgument = new CliArgument<string>("src") { Description = "Source path of the .pkg or .app file" };
        var unpackDestinationArgument = new CliArgument<string>("dst") { Description = "Destination path to unpack the file" };
        var unpackCommand = new CliCommand("unpack", "Unpack a .pkg or .app file")
        {
            Arguments = { unpackSrcArgument, unpackDestinationArgument }
        };
        unpackCommand.SetAction(result =>
        {
            var srcPath = result.GetValue(unpackSrcArgument);
            var dstPath = result.GetValue(unpackDestinationArgument);
            return MacOsPkgCore.Unpack(srcPath, dstPath);
        });

        var packSrcArgument = new CliArgument<string>("src") { Description = "Source path to pack." };
        var packDstArgument = new CliArgument<string>("dst") { Description = "Destination path of the .pkg or .app file." };
        var packCommand = new CliCommand("pack", "Pack a directory into a .pkg or .app file.")
        {
            Arguments = { packSrcArgument, packDstArgument }
        };
        packCommand.SetAction(result =>
        {
            var srcPath = result.GetValue(packSrcArgument);
            var dstPath = result.GetValue(packDstArgument);
            return MacOsPkgCore.Pack(srcPath, dstPath);
        });

        var pkgOrAppArgument = new CliArgument<string>("src") { Description = "Input pkg or app to verify." };
        var verifyCommand = new CliCommand("verify", "Verify that a pkg or app is signed.")
        {
            Arguments = { pkgOrAppArgument }
        };
        verifyCommand.SetAction(result =>
        {
            var srcPath = result.GetValue(pkgOrAppArgument);
            return MacOsPkgCore.VerifySignature(srcPath);
        });

        rootCommand.Subcommands.Add(unpackCommand);
        rootCommand.Subcommands.Add(packCommand);
        rootCommand.Subcommands.Add(verifyCommand);
        return rootCommand;
    }
}
#endif
