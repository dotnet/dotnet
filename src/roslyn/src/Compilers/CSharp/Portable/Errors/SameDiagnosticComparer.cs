﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Roslyn.Utilities;

namespace Microsoft.CodeAnalysis.CSharp;

internal sealed class SameDiagnosticComparer : EqualityComparer<Diagnostic>
{
    public static readonly SameDiagnosticComparer Instance = new SameDiagnosticComparer();
    public override bool Equals(Diagnostic? x, Diagnostic? y) => x is null ? y is null : x.Equals(y);
    public override int GetHashCode(Diagnostic obj) => obj.GetHashCode();
}
