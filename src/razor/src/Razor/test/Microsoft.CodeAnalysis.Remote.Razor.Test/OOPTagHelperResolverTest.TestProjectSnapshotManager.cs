// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System.Linq;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Moq;

namespace Microsoft.CodeAnalysis.Remote.Razor;

public partial class OOPTagHelperResolverTest
{
    private class TestProjectSnapshotManager(Workspace workspace) : DefaultProjectSnapshotManager(
        Mock.Of<IErrorReporter>(MockBehavior.Strict),
        Enumerable.Empty<IProjectSnapshotChangeTrigger>(),
        workspace)
    {
    }
}
