// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;

namespace Microsoft.AspNetCore.Razor.Utilities.Shared.Test.TestData;

public abstract class ImmutableArrayOrderingTestBase : OrderingTestBase<ImmutableArray<int>, ImmutableArray<ValueHolder<int>>, OrderingCaseConverters.ImmutableArray>
{
}
