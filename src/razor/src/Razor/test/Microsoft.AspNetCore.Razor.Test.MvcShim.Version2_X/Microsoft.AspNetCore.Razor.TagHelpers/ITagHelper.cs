// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

namespace Microsoft.AspNetCore.Razor.TagHelpers;

/// <summary>
/// Contract used to filter matching HTML elements.
/// Marker interface for <see cref="TagHelper"/>s.
/// </summary>
public interface ITagHelper : ITagHelperComponent
{
}