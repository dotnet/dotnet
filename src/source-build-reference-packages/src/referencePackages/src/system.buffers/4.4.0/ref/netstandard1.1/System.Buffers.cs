// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;

[assembly: Debuggable(DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: AllowPartiallyTrustedCallers]
[assembly: ReferenceAssembly]
[assembly: AssemblyTitle("System.Buffers")]
[assembly: AssemblyDescription("System.Buffers")]
[assembly: AssemblyDefaultAlias("System.Buffers")]
[assembly: AssemblyCompany("Microsoft Corporation")]
[assembly: AssemblyProduct("Microsoft® .NET Framework")]
[assembly: AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: AssemblyFileVersion("4.6.25519.03")]
[assembly: AssemblyInformationalVersion("4.6.25519.03 built by: SOURCEBUILD")]
[assembly: CLSCompliant(true)]
[assembly: AssemblyMetadata("", "")]
[assembly: AssemblyVersion("4.0.2.0")]




namespace System.Buffers
{
    public abstract partial class ArrayPool<T>
    {
        protected ArrayPool() { }
        public static System.Buffers.ArrayPool<T> Shared { get { throw null; } }
        public static System.Buffers.ArrayPool<T> Create() { throw null; }
        public static System.Buffers.ArrayPool<T> Create(int maxArrayLength, int maxArraysPerBucket) { throw null; }
        public abstract T[] Rent(int minimumLength);
        public abstract void Return(T[] array, bool clearArray = false);
    }
}
