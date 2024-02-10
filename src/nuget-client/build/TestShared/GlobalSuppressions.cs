// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

// This file is used by Code Analysis to maintain SuppressMessage 
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given 
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Test case methods follow different naming conventions.", Scope = "namespaceanddescendants", Target = "Dotnet.Integration.Test")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Test case methods follow different naming conventions.", Scope = "namespaceanddescendants", Target = "NuGet.PackageManagement.VisualStudio.Test")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Test case methods follow different naming conventions.", Scope = "namespaceanddescendants", Target = "NuGet.Test")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Test case methods follow different naming conventions.", Scope = "namespaceanddescendants", Target = "NuGet.XPlat.FuncTest")]
[assembly: SuppressMessage("Usage", "xUnit1041:Fixture arguments to test classes must have fixture sources", Justification = "This is a partial classes and the analyzer fails to detect it.", Scope = "member", Target = "~M:NuGet.VisualStudio.Common.Test.OutputConsoleLoggerTests.Constructor.#ctor(Microsoft.VisualStudio.Sdk.TestFramework.GlobalServiceProvider)")]
[assembly: SuppressMessage("Usage", "xUnit1041:Fixture arguments to test classes must have fixture sources", Justification = "This is a partial classes and the analyzer fails to detect it.", Scope = "member", Target = "~M:NuGet.VisualStudio.Common.Test.OutputConsoleLoggerTests.Dispose.#ctor(Microsoft.VisualStudio.Sdk.TestFramework.GlobalServiceProvider)")]
[assembly: SuppressMessage("Usage", "xUnit1041:Fixture arguments to test classes must have fixture sources", Justification = "This is a partial classes and the analyzer fails to detect it.", Scope = "member", Target = "~M:NuGet.VisualStudio.Common.Test.OutputConsoleLoggerTests.End.#ctor(Microsoft.VisualStudio.Sdk.TestFramework.GlobalServiceProvider)")]
[assembly: SuppressMessage("Usage", "xUnit1041:Fixture arguments to test classes must have fixture sources", Justification = "This is a partial classes and the analyzer fails to detect it.", Scope = "member", Target = "~M:NuGet.VisualStudio.Common.Test.OutputConsoleLoggerTests.ImportingConstructor.#ctor(Microsoft.VisualStudio.Sdk.TestFramework.GlobalServiceProvider)")]
[assembly: SuppressMessage("Usage", "xUnit1041:Fixture arguments to test classes must have fixture sources", Justification = "This is a partial classes and the analyzer fails to detect it.", Scope = "member", Target = "~M:NuGet.VisualStudio.Common.Test.OutputConsoleLoggerTests.Log.#ctor(Microsoft.VisualStudio.Sdk.TestFramework.GlobalServiceProvider)")]
[assembly: SuppressMessage("Usage", "xUnit1041:Fixture arguments to test classes must have fixture sources", Justification = "This is a partial classes and the analyzer fails to detect it.", Scope = "member", Target = "~M:NuGet.VisualStudio.Common.Test.OutputConsoleLoggerTests.ReportError.#ctor(Microsoft.VisualStudio.Sdk.TestFramework.GlobalServiceProvider)")]
[assembly: SuppressMessage("Usage", "xUnit1041:Fixture arguments to test classes must have fixture sources", Justification = "This is a partial classes and the analyzer fails to detect it.", Scope = "member", Target = "~M:NuGet.VisualStudio.Common.Test.OutputConsoleLoggerTests.Start.#ctor(Microsoft.VisualStudio.Sdk.TestFramework.GlobalServiceProvider)")]
