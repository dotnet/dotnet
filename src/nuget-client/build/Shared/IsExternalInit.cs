// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#if NETFRAMEWORK || NETSTANDARD

using System.Diagnostics;

namespace System.Runtime.CompilerServices;

// This class allows the compiler to emit record structs in earlier versions of .NET

[DebuggerNonUserCode]
internal static class IsExternalInit
{
}

#endif
