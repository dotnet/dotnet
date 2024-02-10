// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Moq;

namespace Microsoft.AspNetCore.Razor.Test.Common;

public class StrictMock<T> : Mock<T>
    where T : class
{
    public StrictMock()
        : base(MockBehavior.Strict)
    {
    }
}
