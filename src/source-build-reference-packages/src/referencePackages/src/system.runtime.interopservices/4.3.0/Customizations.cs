// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------

// See https://github.com/dotnet/source-build/issues/4000 for why this is necessary.

#if NETCOREAPP1_1 || NETSTANDARD1_5

namespace System.Runtime.InteropServices
{
    public partial class ComAwareEventInfo : Reflection.EventInfo
    {
        public override Reflection.MethodInfo GetAddMethod(bool nonPublic) { throw null; }

        public override Reflection.MethodInfo GetRaiseMethod(bool nonPublic) { throw null; }

        public override Reflection.MethodInfo GetRemoveMethod(bool nonPublic) { throw null; }
    }
}

#endif
