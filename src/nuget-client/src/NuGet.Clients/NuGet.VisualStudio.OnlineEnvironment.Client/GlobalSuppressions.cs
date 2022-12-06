// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Build", "CA1031:Modify 'RunSolutionRestoreAsync' to catch a more specific allowed exception type, or rethrow the exception.", Justification = "<Pending>", Scope = "member", Target = "~M:NuGet.VisualStudio.OnlineEnvironment.Client.RestoreCommandHandler.RunSolutionRestoreAsync~System.Threading.Tasks.Task")]
[assembly: SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "ToolWindow can't be disposed while the window is active. Dispose is handled in OnClose.", Scope = "member", Target = "~M:NuGet.VisualStudio.OnlineEnvironment.Client.PackageManagerUICommandHandler.CreateToolWindowAsync(System.String,Microsoft.VisualStudio.Shell.Interop.IVsHierarchy,System.UInt32)~System.Threading.Tasks.Task{Microsoft.VisualStudio.Shell.Interop.IVsWindowFrame}")]
