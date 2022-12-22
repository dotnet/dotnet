// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Text;
using NuGet.Packaging;
using Xunit;

namespace NuGet.ProjectModel.Test
{
    public class Sha512HashFunctionTests
    {
        private const string _expectedResult = "NJAwUJVdN8HOjha9VNbopjFMaPVZlAPYFef4CpiYGvVEYmafbYo5CB9KtPFXF5pG7Tj7jBb4/axBJpxZKGEY2Q==";
        private static readonly byte[] _input = Encoding.UTF8.GetBytes("peach");

        [Fact]
        public void Update_SupportsIncrementalUpdates()
        {
            using (var hashFunc = new Sha512HashFunction())
            {
                for (var i = 0; i < _input.Length; ++i)
                {
                    hashFunc.Update(_input, i, count: 1);
                }

                var actualResult = hashFunc.GetHash();

                Assert.Equal(_expectedResult, actualResult);
            }
        }

        [Fact]
        public void Update_ThrowsAfterGetHashCalled()
        {
            using (var hashFunc = new Sha512HashFunction())
            {
                hashFunc.Update(_input, 0, count: 1);
                hashFunc.GetHash();

                Assert.Throws<InvalidOperationException>(() => hashFunc.Update(_input, 1, count: 1));
            }
        }
    }
}
