// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Microsoft.AspNetCore.Mvc.Razor;

public abstract class RazorPage<TModel> : RazorPage
{
    public TModel Model { get; }

    public ViewDataDictionary<TModel> ViewData { get; set; }
}
