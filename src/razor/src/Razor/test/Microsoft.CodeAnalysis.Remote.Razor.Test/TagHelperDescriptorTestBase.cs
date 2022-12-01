// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Test.Common;
using Xunit.Abstractions;

namespace Microsoft.CodeAnalysis.Remote.Razor.Test;

public class TagHelperDescriptorTestBase : TestBase
{
    protected string Project1FilePath { get; }
    protected TagHelperDescriptor TagHelper1_Project1 { get; }
    protected TagHelperDescriptor TagHelper2_Project1 { get; }
    protected IReadOnlyList<TagHelperDescriptor> Project1TagHelpers { get; }

    protected string Project2FilePath { get; }
    protected TagHelperDescriptor TagHelper1_Project2 { get; }
    protected TagHelperDescriptor TagHelper2_Project2 { get; }
    protected IReadOnlyList<TagHelperDescriptor> Project2TagHelpers { get; }
    protected IReadOnlyList<TagHelperDescriptor> Project1AndProject2TagHelpers { get; }

    public TagHelperDescriptorTestBase(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        Project1FilePath = "C:/path/to/Project1/Project1.csproj";
        TagHelper1_Project1 = TagHelperDescriptorBuilder.Create("TagHelper1", "Project1").Build();
        TagHelper2_Project1 = TagHelperDescriptorBuilder.Create("TagHelper2", "Project1").Build();
        Project1TagHelpers = new[] { TagHelper1_Project1, TagHelper2_Project1 };

        Project2FilePath = "C:/path/to/Project2/Project2.csproj";
        TagHelper1_Project2 = TagHelperDescriptorBuilder.Create("TagHelper1", "Project2").Build();
        TagHelper2_Project2 = TagHelperDescriptorBuilder.Create("TagHelper2", "Project2").Build();
        Project2TagHelpers = new[] { TagHelper1_Project2, TagHelper2_Project2 };

        Project1AndProject2TagHelpers = new[] { TagHelper1_Project1, TagHelper2_Project1, TagHelper1_Project2, TagHelper2_Project2 };
    }
}
