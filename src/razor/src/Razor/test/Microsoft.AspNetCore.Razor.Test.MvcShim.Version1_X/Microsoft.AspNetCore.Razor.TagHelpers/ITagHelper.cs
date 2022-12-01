// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Razor.TagHelpers;

public interface ITagHelper
{
    int Order { get; }

    void Init(TagHelperContext context);

    Task ProcessAsync(TagHelperContext context, TagHelperOutput output);
}