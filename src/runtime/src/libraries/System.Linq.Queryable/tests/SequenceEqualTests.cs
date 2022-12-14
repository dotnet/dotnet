// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using Xunit;

namespace System.Linq.Tests
{
    public class SequenceEqualTests : EnumerableBasedTests
    {

        [Fact]
        public void MismatchInMiddle()
        {
            int?[] first = { 1, 2, 3, 4 };
            int?[] second = { 1, 2, 6, 4 };

            Assert.False(first.AsQueryable().SequenceEqual(second.AsQueryable()));
        }

        [Fact]
        public void NullComparer()
        {
            string[] first = { "Bob", "Tim", "Chris" };
            string[] second = { "Bbo", "mTi", "rishC" };

            Assert.False(first.AsQueryable().SequenceEqual(second.AsQueryable()));
        }

        [Fact]
        public void CustomComparer()
        {
            string[] first = { "Bob", "Tim", "Chris" };
            string[] second = { "Bbo", "mTi", "rishC" };

            Assert.True(first.AsQueryable().SequenceEqual(second.AsQueryable(), new AnagramEqualityComparer()));
        }

        [Fact]
        public void FirstSourceNull()
        {
            IQueryable<int> first = null;
            int[] second = { };
            AssertExtensions.Throws<ArgumentNullException>("source1", () => first.SequenceEqual(second.AsQueryable()));
            AssertExtensions.Throws<ArgumentNullException>("source1", () => first.SequenceEqual(second.AsQueryable(), null));
        }

        [Fact]
        public void SecondSourceNull()
        {
            int[] first = { };
            IQueryable<int> second = null;
            AssertExtensions.Throws<ArgumentNullException>("source2", () => first.AsQueryable().SequenceEqual(second));
            AssertExtensions.Throws<ArgumentNullException>("source2", () => first.AsQueryable().SequenceEqual(second, null));
        }

        [Fact]
        public void SequenceEqual1()
        {
            var val = new[] { 0, 2, 1 }.AsQueryable().SequenceEqual(new[] { 0, 2, 1 });
            Assert.True(val);
        }

        [Fact]
        public void SequenceEqual2()
        {
            var val = new[] { 0, 2, 1 }.AsQueryable().SequenceEqual(new[] { 0, 2, 1 }, EqualityComparer<int>.Default);
            Assert.True(val);
        }
    }
}
