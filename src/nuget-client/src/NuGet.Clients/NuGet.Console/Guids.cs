// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace NuGetConsole
{
    public static class GuidList
    {
        // IMPORTANT: this GUID has to match the one declared in NuGet.Tools\Guid.cs
        internal const string guidNuGetConsoleCmdSetString = "1E8A55F6-C18D-407F-91C8-94B02AE1CED6";
        internal static readonly Guid guidNuGetCmdSet = new Guid(guidNuGetConsoleCmdSetString);

        // GUID for the Package Manager Console category in the Font and Colors options page
        public const string GuidPackageManagerConsoleFontAndColorCategoryString = "{F9D6BCE6-C669-41DB-8EE7-DD953828685B}";
        internal static readonly Guid guidPackageManagerConsoleFontAndColorCategory = new Guid(GuidPackageManagerConsoleFontAndColorCategoryString);

        // NuGet Output window pane
        private static Guid NuGetOutputWindowPane = Guid.Parse("CEC55EC8-CC51-40E7-9243-57B87A6F6BEB");

        public static Guid NuGetOutputWindowPaneGuid
        {
            get => NuGetOutputWindowPane;
            set => NuGetOutputWindowPane = value;
        }

        // Visual Studio output tool window (Copied from EnvDTE interop)
        internal static Guid guidVsWindowKindOutput = Guid.Parse("34E76E81-EE4A-11D0-AE2E-00A0C90FFFC3");
    }
}
