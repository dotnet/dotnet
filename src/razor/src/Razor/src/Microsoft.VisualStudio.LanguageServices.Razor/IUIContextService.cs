// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;

namespace Microsoft.VisualStudio.Razor;

internal interface IUIContextService
{
    bool IsActive(Guid contextGuid);
}
