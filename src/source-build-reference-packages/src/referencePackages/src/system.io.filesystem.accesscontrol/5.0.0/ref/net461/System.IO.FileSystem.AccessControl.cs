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
using System.Security.AccessControl;

[assembly: Debuggable(DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: AllowPartiallyTrustedCallers]
[assembly: ReferenceAssembly]
[assembly: AssemblyTitle("System.IO.FileSystem.AccessControl")]
[assembly: AssemblyDescription("System.IO.FileSystem.AccessControl")]
[assembly: AssemblyDefaultAlias("System.IO.FileSystem.AccessControl")]
[assembly: AssemblyCompany("Microsoft Corporation")]
[assembly: AssemblyProduct("Microsoft® .NET Framework")]
[assembly: AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: AssemblyFileVersion("5.0.20.51904")]
[assembly: AssemblyInformationalVersion("5.0.20.51904 built by: SOURCEBUILD")]
[assembly: CLSCompliant(true)]
[assembly: AssemblyMetadata("", "")]
[assembly: AssemblyVersion("5.0.0.0")]

[assembly: TypeForwardedTo(typeof(System.Security.AccessControl.DirectoryObjectSecurity))]
[assembly: TypeForwardedTo(typeof(System.Security.AccessControl.DirectorySecurity))]
[assembly: TypeForwardedTo(typeof(System.Security.AccessControl.FileSecurity))]
[assembly: TypeForwardedTo(typeof(System.Security.AccessControl.FileSystemAccessRule))]
[assembly: TypeForwardedTo(typeof(System.Security.AccessControl.FileSystemAuditRule))]
[assembly: TypeForwardedTo(typeof(System.Security.AccessControl.FileSystemRights))]
[assembly: TypeForwardedTo(typeof(System.Security.AccessControl.FileSystemSecurity))]



namespace System.IO
{
    public static partial class FileSystemAclExtensions
    {
        public static void Create(this System.IO.DirectoryInfo directoryInfo, System.Security.AccessControl.DirectorySecurity directorySecurity) { }
        public static System.IO.FileStream Create(this System.IO.FileInfo fileInfo, System.IO.FileMode mode, System.Security.AccessControl.FileSystemRights rights, System.IO.FileShare share, int bufferSize, System.IO.FileOptions options, System.Security.AccessControl.FileSecurity fileSecurity) { throw null; }
        public static System.IO.DirectoryInfo CreateDirectory(this System.Security.AccessControl.DirectorySecurity directorySecurity, string path) { throw null; }
        public static System.Security.AccessControl.DirectorySecurity GetAccessControl(this System.IO.DirectoryInfo directoryInfo) { throw null; }
        public static System.Security.AccessControl.DirectorySecurity GetAccessControl(this System.IO.DirectoryInfo directoryInfo, System.Security.AccessControl.AccessControlSections includeSections) { throw null; }
        public static System.Security.AccessControl.FileSecurity GetAccessControl(this System.IO.FileInfo fileInfo) { throw null; }
        public static System.Security.AccessControl.FileSecurity GetAccessControl(this System.IO.FileInfo fileInfo, System.Security.AccessControl.AccessControlSections includeSections) { throw null; }
        public static System.Security.AccessControl.FileSecurity GetAccessControl(this System.IO.FileStream fileStream) { throw null; }
        public static void SetAccessControl(this System.IO.DirectoryInfo directoryInfo, System.Security.AccessControl.DirectorySecurity directorySecurity) { }
        public static void SetAccessControl(this System.IO.FileInfo fileInfo, System.Security.AccessControl.FileSecurity fileSecurity) { }
        public static void SetAccessControl(this System.IO.FileStream fileStream, System.Security.AccessControl.FileSecurity fileSecurity) { }
    }
}
