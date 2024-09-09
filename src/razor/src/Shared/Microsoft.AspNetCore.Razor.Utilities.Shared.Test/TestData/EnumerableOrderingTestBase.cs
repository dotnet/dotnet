// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Microsoft.AspNetCore.Razor.Utilities.Shared.Test.TestData;

public abstract class EnumerableOrderingTestBase : OrderingTestBase<IEnumerable<int>, IEnumerable<ValueHolder<int>>, OrderingCaseConverters.Enumerable>
{
}
