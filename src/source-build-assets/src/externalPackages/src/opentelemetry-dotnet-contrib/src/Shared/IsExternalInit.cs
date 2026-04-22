// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#if NETFRAMEWORK || NETSTANDARD
namespace System.Runtime.CompilerServices;

// This enabled "init" keyword in net462 + netstandard2.0 targets.
internal static class IsExternalInit
{
}
#endif
