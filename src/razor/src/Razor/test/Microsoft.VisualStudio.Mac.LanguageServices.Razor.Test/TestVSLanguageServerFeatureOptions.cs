// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.VisualStudio.Editor.Razor;
using Moq;

namespace Microsoft.VisualStudio.Mac.LanguageServices.Razor.ProjectSystem;

internal class TestVSLanguageServerFeatureOptions : VisualStudioMacLanguageServerFeatureOptions
{
#pragma warning disable CS0618 // Type or member is obsolete
    public static readonly TestVSLanguageServerFeatureOptions Instance = new();
#pragma warning restore CS0618 // Type or member is obsolete

    [Obsolete("Use static Instance member")]
    public TestVSLanguageServerFeatureOptions()
        : base(Mock.Of<LSPEditorFeatureDetector>(MockBehavior.Strict))
    {
    }
}
