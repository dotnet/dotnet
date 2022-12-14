// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Linq.Expressions;
using Xunit;

namespace System.Linq.Tests
{
    public class SingleOrDefaultTests : EnumerableBasedTests
    {
        [Fact]
        public void ManyMatch()
        {
            int[] source = { 4, 4, 4, 4, 4 };

            Assert.Throws<InvalidOperationException>(() => source.AsQueryable().SingleOrDefault());
        }

        [Fact]
        public void Empty()
        {
            Assert.Null(Enumerable.Empty<int?>().AsQueryable().SingleOrDefault());
        }

        [Fact]
        public void EmptyDefault()
        {
            int[] source = { };
            int defaultValue = 5;
            Assert.Equal(defaultValue, source.AsQueryable().SingleOrDefault(5));
        }

        [Fact]
        public void EmptySourceWithPredicate()
        {
            Assert.Null(Enumerable.Empty<int?>().AsQueryable().SingleOrDefault(i => i % 2 == 0));
        }

        [Fact]
        public void EmptySourceWithPredicateDefault()
        {
            Assert.Equal(5, Enumerable.Empty<int?>().AsQueryable().SingleOrDefault(i => i % 2 == 0, 5));
        }

        [Theory]
        [InlineData(1, 100)]
        [InlineData(42, 100)]
        public void FindSingleMatch(int target, int range)
        {
            Assert.Equal(target, Enumerable.Range(0, range).AsQueryable().SingleOrDefault(i => i == target));
        }

        [Fact]
        public void ThrowsOnNullSource()
        {
            IQueryable<int> source = null;
            AssertExtensions.Throws<ArgumentNullException>("source", () => source.SingleOrDefault());
            AssertExtensions.Throws<ArgumentNullException>("source", () => source.SingleOrDefault(i => i % 2 == 0));
        }

        [Fact]
        public void ThrowsOnNullPredicate()
        {
            int[] source = { };
            Expression<Func<int, bool>> nullPredicate = null;
            AssertExtensions.Throws<ArgumentNullException>("predicate", () => source.AsQueryable().SingleOrDefault(nullPredicate));
        }

        [Fact]
        public void SingleOrDefault1()
        {
            var val = new[] { 2 }.AsQueryable().SingleOrDefault();
            Assert.Equal(2, val);
        }

        [Fact]
        public void SingleOrDefault2()
        {
            var val = new[] { 2 }.AsQueryable().SingleOrDefault(n => n > 1);
            Assert.Equal(2, val);
        }

        [Fact]
        public void SingleOrDefault_OverloadResolution_Regression()
        {
            // Regression test for https://github.com/dotnet/runtime/issues/65419
            object? result = new object[] { 1, "" }.AsQueryable().SingleOrDefault(x => x is string);
            Assert.IsType<string>(result);

            result = Array.Empty<object>().AsQueryable().SingleOrDefault(1);
            Assert.IsType<int>(result);
        }
    }
}
