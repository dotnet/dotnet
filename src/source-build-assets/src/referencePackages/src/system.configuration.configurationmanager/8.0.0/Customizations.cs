// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

// Manual addition to address missing System.Security.PermissionSet type in netstandard2.0
// This type is used by IInternalConfigHost interface but not available in netstandard2.0
#if NETSTANDARD2_0
namespace System.Security
{
    // Stub type to allow compilation - this is just a reference assembly
    public partial class PermissionSet
    {
    }
}
#endif
