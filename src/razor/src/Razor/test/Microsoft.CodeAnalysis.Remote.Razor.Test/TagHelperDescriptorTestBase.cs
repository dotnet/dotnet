// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Utilities;
using Xunit.Abstractions;
using Checksum = Microsoft.AspNetCore.Razor.Utilities.Checksum;

namespace Microsoft.CodeAnalysis.Remote.Razor.Test;

public class TagHelperDescriptorTestBase : TestBase
{
    protected string Project1FilePath { get; }
    internal ProjectId Project1Id { get; }
    protected TagHelperDescriptor TagHelper1_Project1 { get; }
    protected TagHelperDescriptor TagHelper2_Project1 { get; }
    protected ImmutableArray<TagHelperDescriptor> Project1TagHelpers { get; }
    private protected ImmutableArray<Checksum> Project1TagHelperChecksums { get; }

    protected string Project2FilePath { get; }
    internal ProjectId Project2Id { get; }
    protected TagHelperDescriptor TagHelper1_Project2 { get; }
    protected TagHelperDescriptor TagHelper2_Project2 { get; }
    protected ImmutableArray<TagHelperDescriptor> Project2TagHelpers { get; }
    private protected ImmutableArray<Checksum> Project2TagHelperChecksums { get; }
    protected ImmutableArray<TagHelperDescriptor> Project1AndProject2TagHelpers { get; }
    private protected ImmutableArray<Checksum> Project1AndProject2TagHelperChecksums { get; }

    public TagHelperDescriptorTestBase(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        Project1FilePath = "C:/path/to/Project1/Project1.csproj";
        Project1Id = ProjectId.CreateNewId();
        TagHelper1_Project1 = TagHelperDescriptorBuilder.Create("TagHelper1", "Project1").Build();
        TagHelper2_Project1 = TagHelperDescriptorBuilder.Create("TagHelper2", "Project1").Build();
        Project1TagHelpers = ImmutableArray.Create(TagHelper1_Project1, TagHelper2_Project1);
        Project1TagHelperChecksums = Project1TagHelpers.SelectAsArray(t => t.GetChecksum());

        Project2FilePath = "C:/path/to/Project2/Project2.csproj";
        Project2Id = ProjectId.CreateNewId();
        TagHelper1_Project2 = TagHelperDescriptorBuilder.Create("TagHelper1", "Project2").Build();
        TagHelper2_Project2 = TagHelperDescriptorBuilder.Create("TagHelper2", "Project2").Build();
        Project2TagHelpers = ImmutableArray.Create(TagHelper1_Project2, TagHelper2_Project2);
        Project2TagHelperChecksums = Project2TagHelpers.SelectAsArray(t => t.GetChecksum());

        Project1AndProject2TagHelpers = ImmutableArray.Create(TagHelper1_Project1, TagHelper2_Project1, TagHelper1_Project2, TagHelper2_Project2);
        Project1AndProject2TagHelperChecksums = Project1AndProject2TagHelpers.SelectAsArray(t => t.GetChecksum());
    }
}
