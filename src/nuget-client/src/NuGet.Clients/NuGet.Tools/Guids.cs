// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace NuGetVSExtension
{
    internal static class GuidList
    {
        public const string guidNuGetPkgString = "5fcc8577-4feb-4d04-ad72-d6c629b083cc";

        private const string guidNuGetConsoleCmdSetString = "1E8A55F6-C18D-407F-91C8-94B02AE1CED6";
        private const string guidNuGetDialogCmdSetString = "25fd982b-8cae-4cbd-a440-e03ffccde106";
        private const string guidNuGetToolsGroupString = "C0D88179-5D25-4982-BFE6-EC5FD59AC103";
        private const string guidNuGetDebugConsoleCmdSetString = "DDC61543-6CA7-4A6F-A5B7-984BE723C52F";
        private const string guidClearNuGetLocalResourcesCmdSetString = "54A0AC88-A025-4A62-8D48-6C1848E4F545";

        // any project system that wants to load NuGet when its project opens needs to activate a UI context with this GUID
        public const string guidAutoLoadNuGetString = "65B1D035-27A5-4BBA-BAB9-5F61C1E2BC4A";

        // GUID for UI Context rule that is active when a project that might be upgradeable (from packages.config to PackageReference)
        // is loaded. This will autoload our package, so we can dynamically control the visibility of the appropriate menu items.
        public const string guidUpgradeableProjectLoadedString = "1837160D-723F-43CD-8185-97758295A859";

        // Unique identifier of the editor factory that created an instance of the document view and document data objects.
        // Used when creating document windows of Package Manager
        private const string guidNuGetEditorTypeString = "95501c48-a850-47c1-a785-2aaa96637f81";

        public static readonly Guid guidNuGetConsoleCmdSet = new Guid(guidNuGetConsoleCmdSetString);
        public static readonly Guid guidNuGetDialogCmdSet = new Guid(guidNuGetDialogCmdSetString);
        public static readonly Guid guidNuGetToolsGroupCmdSet = new Guid(guidNuGetToolsGroupString);
        public static readonly Guid guidNuGetDebugConsoleCmdSet = new Guid(guidNuGetDebugConsoleCmdSetString);
        public static readonly Guid guidNuGetEditorType = Guid.Parse(guidNuGetEditorTypeString);
        public static readonly Guid guidClearNuGetLocalResourcesCmdSet = Guid.Parse(guidClearNuGetLocalResourcesCmdSetString);

        // Visual Studio output tool window (Copied from EnvDTE interop)
        public static Guid guidVsWindowKindOutput = Guid.Parse("34E76E81-EE4A-11D0-AE2E-00A0C90FFFC3");
    }
}
