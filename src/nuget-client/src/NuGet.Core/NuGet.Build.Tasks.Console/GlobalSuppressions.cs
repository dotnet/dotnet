// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Build", "CA1031:Modify 'RestoreAsync' to catch a more specific allowed exception type, or rethrow the exception.", Justification = "<Pending>", Scope = "member", Target = "~M:NuGet.Build.Tasks.Console.MSBuildStaticGraphRestore.RestoreAsync(System.String,System.Collections.Generic.IDictionary{System.String,System.String},System.Collections.Generic.IReadOnlyDictionary{System.String,System.String})~System.Threading.Tasks.Task{System.Boolean}")]
[assembly: SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "<Pending>", Scope = "member", Target = "~M:NuGet.Build.Tasks.Console.MSBuildStaticGraphRestore.WriteDependencyGraphSpec(System.String,System.Collections.Generic.IDictionary{System.String,System.String},System.Collections.Generic.IReadOnlyDictionary{System.String,System.String})~System.Boolean")]
[assembly: SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "<Pending>", Scope = "member", Target = "~M:NuGet.Build.Tasks.Console.MSBuildStaticGraphRestore.GetDependencyGraphSpec(System.String,System.Collections.Generic.IDictionary{System.String,System.String},System.Boolean,System.String)~NuGet.ProjectModel.DependencyGraphSpec")]
[assembly: SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "<Pending>", Scope = "member", Target = "~M:NuGet.Build.Tasks.Console.MSBuildStaticGraphRestore.LoadProjects(System.Collections.Generic.IEnumerable{Microsoft.Build.Graph.ProjectGraphEntryPoint},System.Boolean,System.String)~System.Collections.Generic.ICollection{NuGet.Build.Tasks.Console.ProjectWithInnerNodes}")]
[assembly: SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "<Pending>", Scope = "member", Target = "~M:NuGet.Build.Tasks.Console.Program.MainInternal(System.String[],NuGet.Common.IEnvironmentVariableReader)~System.Threading.Tasks.Task{System.Int32}")]
[assembly: SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "<Pending>", Scope = "member", Target = "~M:NuGet.Build.Tasks.Console.Program.TryDeserializeGlobalProperties(System.IO.TextWriter,System.IO.BinaryReader,System.Collections.Generic.Dictionary{System.String,System.String}@)~System.Boolean")]
[assembly: SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "<Pending>", Scope = "member", Target = "~M:NuGet.Build.Tasks.Console.Program.TryParseArguments(System.String[],System.Func{System.IO.Stream},System.IO.TextWriter,System.ValueTuple{System.Collections.Generic.Dictionary{System.String,System.String},System.IO.FileInfo,System.String,System.Collections.Generic.Dictionary{System.String,System.String}}@)~System.Boolean")]
