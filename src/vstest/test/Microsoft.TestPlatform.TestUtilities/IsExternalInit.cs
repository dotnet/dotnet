// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if !NET5_0_OR_GREATER

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace System.Runtime.CompilerServices;

/// <summary>
/// Reserved for use by the compiler for records.
/// /!\ Do not use directly. /!\
/// </summary>
[ExcludeFromCodeCoverage, DebuggerNonUserCode]
internal static class IsExternalInit
{
}

#endif
