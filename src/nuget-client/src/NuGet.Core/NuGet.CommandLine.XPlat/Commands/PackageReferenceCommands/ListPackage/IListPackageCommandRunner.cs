// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace NuGet.CommandLine.XPlat
{
    internal interface IListPackageCommandRunner
    {
        [RequiresUnreferencedCode("In-process MSBuild execution loads task assemblies and loggers via reflection and is not trim-safe.")]
        Task<int> ExecuteCommandAsync(ListPackageArgs packageRefArgs);
    }
}
