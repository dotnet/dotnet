// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

[Flags]
internal enum RazorCompilerOptions
{
    None = 0,
    ForceRuntimeCodeGeneration = 1 << 0
}
