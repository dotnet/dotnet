// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Build", "CA1031:Modify 'InitializeTypes' to catch a more specific allowed exception type, or rethrow the exception.", Justification = "<Pending>", Scope = "member", Target = "~M:NuGet.VisualStudio.ExtensionManagerShim.InitializeTypes(System.Action{System.String})")]
[assembly: SuppressMessage("Build", "CA1031:Modify 'CreateMetadata' to catch a more specific allowed exception type, or rethrow the exception.", Justification = "<Pending>", Scope = "member", Target = "~M:NuGet.VisualStudio.PackageManagementHelpers.CreateMetadata(System.String,NuGet.Packaging.Core.PackageIdentity)~NuGet.VisualStudio.IVsPackageMetadata")]
[assembly: SuppressMessage("Build", "CA1822:Member GetExtensionRepositoryPath does not access instance data and can be marked as static (Shared in VisualBasic)", Justification = "<Pending>", Scope = "member", Target = "~M:NuGet.VisualStudio.PreinstalledPackageInstaller.GetExtensionRepositoryPath(System.String,System.Object,System.Action{System.String})~System.String")]
[assembly: SuppressMessage("Build", "CA1822:Member GetRegistryRepositoryPath does not access instance data and can be marked as static (Shared in VisualBasic)", Justification = "<Pending>", Scope = "member", Target = "~M:NuGet.VisualStudio.PreinstalledPackageInstaller.GetRegistryRepositoryPath(System.String,System.Collections.Generic.IEnumerable{NuGet.VisualStudio.IRegistryKey},System.Action{System.String})~System.String")]
[assembly: SuppressMessage("Build", "CA1801:Parameter provider of method AddFromExtension is never used. Remove the parameter or use it in the method body.", Justification = "<Pending>", Scope = "member", Target = "~M:NuGet.VisualStudio.PreinstalledRepositoryProvider.AddFromExtension(NuGet.Protocol.Core.Types.ISourceRepositoryProvider,System.String)")]
[assembly: SuppressMessage("Build", "CA1031:Modify 'RestorePackages' to catch a more specific allowed exception type, or rethrow the exception.", Justification = "<Pending>", Scope = "member", Target = "~M:NuGet.VisualStudio.Implementation.Extensibility.VsPackageRestorer.RestorePackages(EnvDTE.Project)")]
[assembly: SuppressMessage("Build", "CA1031:Modify 'MigrateProjectToPackageRefAsync' to catch a more specific allowed exception type, or rethrow the exception.", Justification = "<Pending>", Scope = "member", Target = "~M:NuGet.VisualStudio.Implementation.Extensibility.VsProjectJsonToPackageReferenceMigrator.MigrateProjectToPackageRefAsync(System.String)~System.Threading.Tasks.Task{System.Object}")]
[assembly: SuppressMessage("Build", "CA1822:Member RunDesignTimeBuildAsync does not access instance data and can be marked as static (Shared in VisualBasic)", Justification = "<Pending>", Scope = "member", Target = "~M:NuGet.VisualStudio.VsTemplateWizard.RunDesignTimeBuildAsync(EnvDTE.Project)")]
[assembly: SuppressMessage("Build", "CA1822:Member ErrorHandler does not access instance data and can be marked as static (Shared in VisualBasic)", Justification = "<Pending>", Scope = "member", Target = "~P:NuGet.VisualStudio.Implementation.Extensibility.VsPackageInstaller.ErrorHandler")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "<Pending>", Scope = "member", Target = "~M:NuGet.VisualStudio.Implementation.Extensibility.VsPackageInstallerEvents.#ctor(NuGet.ProjectManagement.IPackageEventsProvider,NuGet.VisualStudio.Telemetry.INuGetTelemetryProvider)")]
