// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.DotNet.MacOsPkg;

if (!RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
{
    Console.Error.WriteLine("This tool is only supported on macOS.");
    return 1;
}

if (args.Length != 3)
{
    Console.Error.WriteLine("Usage: <src path> <dst path> <unpack|pack>");
    return 1;
}

string srcPath = args[0];
string dstPath = args[1];
string op = args[2];

var cleanTarget = () =>
{
    Utilities.CleanupPath(dstPath);
    Utilities.CreateParentDirectory(dstPath);
};

try
{
    if (op == "unpack")
    {
        if (!File.Exists(srcPath) || (!Utilities.IsPkg(srcPath) && !Utilities.IsAppBundle(srcPath)))
        {
            throw new Exception("Input path must be an existing .pkg or .app (zipped) file.");
        }

        cleanTarget();

        if (Utilities.IsPkg(srcPath))
        {
            Package.Unpack(srcPath, dstPath);
        }
        else if (Utilities.IsAppBundle(srcPath))
        {
            AppBundle.Unpack(srcPath, dstPath);
        }
    }
    else if(op == "pack")
    {
        if (!Directory.Exists(srcPath))
        {
            throw new Exception("Input path must be a valid directory.");
        }

        if (!Utilities.IsPkg(dstPath) && !Utilities.IsAppBundle(dstPath))
        {
            throw new Exception("Output path must be a .pkg or .app (zipped) file.");
        }

        cleanTarget();

        if (Utilities.IsPkg(dstPath))
        {
            Package.Pack(srcPath, dstPath);
        }
        else if (Utilities.IsAppBundle(dstPath))
        {
            AppBundle.Pack(srcPath, dstPath);
        }
    }
    else
    {
        Console.Error.WriteLine($"Invalid operation {op}.");
        return 1;
    }
}
catch (Exception e)
{
    Console.Error.WriteLine(e);
    return 1;
}

return 0;
