﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

[Flags]
internal enum RazorCompilerOptions
{
    None = 0,
    ForceRuntimeCodeGeneration = 1 << 0
}
