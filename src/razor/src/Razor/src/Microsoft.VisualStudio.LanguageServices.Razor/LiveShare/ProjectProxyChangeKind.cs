// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace Microsoft.VisualStudio.Razor.LiveShare;

// This type must be public because it is exposed by a public interface that is implemented as
// an RPC proxy by live share.
public enum ProjectProxyChangeKind
{
    ProjectAdded,
    ProjectRemoved,
    ProjectChanged,
}
