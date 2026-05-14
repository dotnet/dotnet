// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace NuGetConsole
{
    [ComImport]
    [Guid("4EF0C34E-BD1F-473F-93D8-E1F90F9B3D63")]
    public interface IConsoleInitializer
    {
        Task<Action> Initialize();
    }
}
