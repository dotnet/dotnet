// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace NuGet.VisualStudio.OnlineEnvironment.Client
{
    internal static class GuidList
    {
        // NuGet Output window pane
        public static Guid GuidNuGetOutputWindowPaneGuid = Guid.Parse("CEC55EC8-CC51-40E7-9243-57B87A6F6BEB");
        public static readonly Guid NuGetEditorType = Guid.Parse(NuGetEditorTypeString);

        // Unique identifier of the editor factory that created an instance of the document view and document data objects.
        // Used when creating document windows of Package Manager
        private const string NuGetEditorTypeString = "95501c48-a850-47c1-a785-2aaa96637f81";

        // Online Environments in 16.4
        public const string CloudEnvironmentConnectedUIContextGuidString = "{CE73BF3D-D614-438A-9B93-24E9E9D7453A}";
        public static readonly Guid CloudEnvironmentConnectedUIContextGuid = new Guid(CloudEnvironmentConnectedUIContextGuidString);
    }
}
