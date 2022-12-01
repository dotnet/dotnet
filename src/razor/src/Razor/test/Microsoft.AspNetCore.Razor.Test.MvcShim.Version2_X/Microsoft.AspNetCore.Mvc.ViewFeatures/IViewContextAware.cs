// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using Microsoft.AspNetCore.Mvc.Rendering;

namespace Microsoft.AspNetCore.Mvc.ViewFeatures;

public interface IViewContextAware
{
    void Contextualize(ViewContext viewContext);
}
