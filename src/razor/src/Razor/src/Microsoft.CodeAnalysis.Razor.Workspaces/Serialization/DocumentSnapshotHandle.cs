// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.CodeAnalysis.Razor.Serialization;

internal record DocumentSnapshotHandle(string FilePath, string TargetPath, RazorFileKind FileKind);
