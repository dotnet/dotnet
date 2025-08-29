// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Reflection;
using System.Runtime.Versioning;

namespace Microsoft.DotNet.UnifiedBuild.Tasks;

internal static class Config
{
    private static readonly string _taskAssemblyLocation = 
        Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? 
        throw new InvalidOperationException("Unable to determine task assembly location.");

    private static readonly string _staticDirectory = Path.Combine(_taskAssemblyLocation, "static");

    private const string AspNetCoreSnkFileName = "AspNetCore.snk";
    private const string EmptyProjectFileName = "empty.proj";
    public const string SigningPropsFileName = "Signing.props";

    public static string AspNetCoreSnkPath => Path.Combine(_staticDirectory, AspNetCoreSnkFileName);
    public static string EmptyProjectPath => Path.Combine(_staticDirectory, EmptyProjectFileName);
    public static string SigningPropsPath => Path.Combine(_staticDirectory, SigningPropsFileName);
}
