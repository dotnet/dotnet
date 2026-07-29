// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.SolutionRestoreManager.ErrorListFixers
{
    internal static class NuGetErrorListFixerConstants
    {
        /// <summary>
        /// The exported <c>[Name]</c> of Visual Studio's built-in Copilot Error List fixer. NuGet
        /// fixers order themselves <c>[Order(Before = CopilotFixerName)]</c> so they take priority
        /// for the codes they handle. This is an external contract: it must exactly match the VS
        /// Copilot fixer's Name, and there is no compile-time link, so update it if VS renames theirs.
        /// </summary>
        internal const string CopilotFixerName = "Copilot ErrorList Fixer";
        internal const string AuditFixerName = "NuGet Audit ErrorList Fixer";
    }
}
