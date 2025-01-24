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

namespace Microsoft.DotNet.MacOsPkg;

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
            return UnpackCommand(result, unpackSrcArgument, unpackDestinationArgument);
        });

        var packSrcArgument = new CliArgument<string>("src") { Description = "Source path to pack." };
        var packDstArgument = new CliArgument<string>("dst") { Description = "Destination path of the .pkg or .app file." };
        var packCommand = new CliCommand("pack", "Pack a directory into a .pkg or .app file.")
        {
            Arguments = { packSrcArgument, packDstArgument }
        };
        packCommand.SetAction(result =>
        {
            return PackCommand(result, packSrcArgument, packDstArgument);
        });

        var pkgOrAppArgument = new CliArgument<string>("src") { Description = "Input pkg or app to verify." };
        var verifyCommand = new CliCommand("verify", "Verify that a pkg or app is signed.")
        {
            Arguments = { pkgOrAppArgument }
        };
        verifyCommand.SetAction(result =>
        {
            return VerifyCommand(result, pkgOrAppArgument);
        });

        rootCommand.Subcommands.Add(unpackCommand);
        rootCommand.Subcommands.Add(packCommand);
        rootCommand.Subcommands.Add(verifyCommand);
        return rootCommand;
    }

    private static int VerifyCommand(ParseResult result, CliArgument<string> pkgOrAppArgument)
    {
        var srcPath = result.GetValue(pkgOrAppArgument) ?? throw new Exception("src must be non-empty");
        try
        {
            if (!File.Exists(srcPath) || !Utilities.IsPkg(srcPath) && !Utilities.IsAppBundle(srcPath))
            {
                throw new Exception("Input path must be a .pkg or .app (zipped) file.");
            }

            if (Utilities.IsPkg(srcPath))
            {
                Package.VerifySignature(srcPath);
            }
            else if (Utilities.IsAppBundle(srcPath))
            {
                AppBundle.VerifySignature(srcPath);
            }
        }
        catch (Exception e)
        {
            Console.Error.WriteLine(e);
            return 1;
        }

        return 0;
    }

    private static int PackCommand(ParseResult result, CliArgument<string> packSrcArgument, CliArgument<string> packDstArgument)
    {
        var srcPath = result.GetValue(packSrcArgument) ?? throw new Exception("src must be non-empty");
        var dstPath = result.GetValue(packDstArgument) ?? throw new Exception("dst must be non-empty");

        try
        {
            if (!Directory.Exists(srcPath))
            {
                throw new Exception("Input path must be a valid directory.");
            }

            if (!Utilities.IsPkg(dstPath) && !Utilities.IsAppBundle(dstPath))
            {
                throw new Exception("Output path must be a .pkg or .app (zipped) file.");
            }

            Utilities.CleanupPath(dstPath);
            Utilities.CreateParentDirectory(dstPath);

            if (Utilities.IsPkg(dstPath))
            {
                Package.Pack(srcPath, dstPath);
            }
            else if (Utilities.IsAppBundle(dstPath))
            {
                AppBundle.Pack(srcPath, dstPath);
            }
        }
        catch (Exception e)
        {
            Console.Error.WriteLine(e);
            return 1;
        }

        return 0;
    }

    private static int UnpackCommand(ParseResult result, CliArgument<string> unpackSrcArgument, CliArgument<string> unpackDestinationArgument)
    {
        var srcPath = result.GetValue(unpackSrcArgument) ?? throw new Exception("src must be non-empty");
        var dstPath = result.GetValue(unpackDestinationArgument) ?? throw new Exception("dst must be non-empty");

        try
        {
            if (!File.Exists(srcPath) || (!Utilities.IsPkg(srcPath) && !Utilities.IsAppBundle(srcPath)))
            {
                throw new Exception("Input path must be an existing .pkg or .app (zipped) file.");
            }
            Utilities.CleanupPath(dstPath);
            Utilities.CreateParentDirectory(dstPath);
            if (Utilities.IsPkg(srcPath))
            {
                Package.Unpack(srcPath, dstPath);
            }
            else if (Utilities.IsAppBundle(srcPath))
            {
                AppBundle.Unpack(srcPath, dstPath);
            }
        }
        catch (Exception e)
        {
            Console.Error.WriteLine(e);
            return 1;
        }
        return 0;
    }
}
#endif
