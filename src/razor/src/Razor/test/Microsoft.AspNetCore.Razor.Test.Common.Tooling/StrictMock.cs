// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Linq.Expressions;
using Moq;

namespace Microsoft.AspNetCore.Razor.Test.Common;

public static class StrictMock
{
    public static T Of<T>()
        where T : class
        => Mock.Of<T>(MockBehavior.Strict);

    public static T Of<T>(Expression<Func<T, bool>> predicate)
        where T : class
        => Mock.Of<T>(predicate, MockBehavior.Strict);
}
