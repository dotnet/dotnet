// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Text;
using Xunit;

namespace NuGet.ProjectModel.Test
{
    public class FnvHash64FunctionTests
    {
        private const string ExpectedResult = "BSgChkz/Y68=";
        private static readonly byte[] Input = Encoding.UTF8.GetBytes("BeachClubExtraAvocadoSpread");

        [Fact]
        public void Update_SupportsIncrementalUpdates()
        {
            using var hashFunc = new FnvHash64Function();

            for (var i = 0; i < Input.Length; ++i)
            {
                hashFunc.Update(Input, i, count: 1);
            }

            var actualResult = hashFunc.GetHash();

            Assert.Equal(ExpectedResult, actualResult);
        }

        [Fact]
        public void Update_ThrowsIfDataNull()
        {
            using var hashFunc = new FnvHash64Function();

            Assert.Throws<ArgumentNullException>(() => hashFunc.Update(null, 0, count: 0));
        }

        [Fact]
        public void Update_ThrowsIfCountNegative()
        {
            using var hashFunc = new FnvHash64Function();

            Assert.Throws<ArgumentOutOfRangeException>(() => hashFunc.Update(Input, 0, count: -1));
        }
    }
}
