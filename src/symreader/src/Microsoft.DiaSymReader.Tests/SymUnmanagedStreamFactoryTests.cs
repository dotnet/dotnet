// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the License.txt file in the project root for more information.

using System;
using Xunit;

namespace Microsoft.DiaSymReader.UnitTests
{
    public class SymUnmanagedStreamFactoryTests
    {
        [Fact]
        public void Errors()
        {
            Assert.Throws<ArgumentNullException>(() => SymUnmanagedStreamFactory.CreateStream(null));
        }
    }
}
