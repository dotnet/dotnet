// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace NuGet.Common.Test
{
    public class AggregateEnumerableAsyncTest
    {
        [Fact]
        public async Task TestAllEmptyEnums()
        {
            var enum1 = new IntegerEnumerableAsync(new List<int>() { });
            var enum2 = new IntegerEnumerableAsync(new List<int>() { });
            var enum3 = new IntegerEnumerableAsync(new List<int>() { });

            HashSet<int> blaa = new HashSet<int>();

            var aggregateEnumerable = new AggregateEnumerableAsync<int>(new List<IEnumerableAsync<int>> { enum1, enum2, enum3 }, null, null);
            var enumerator = aggregateEnumerable.GetEnumeratorAsync();

            Assert.False(await enumerator.MoveNextAsync());
        }

        [Fact]
        public async Task TestAllButOneEmptyEnums()
        {
            var enum1 = new IntegerEnumerableAsync(new List<int>() { 1, 2, 3, 4, 5 });
            var enum2 = new IntegerEnumerableAsync(new List<int>() { });
            var enum3 = new IntegerEnumerableAsync(new List<int>() { });

            var aggregateEnumerable = new AggregateEnumerableAsync<int>(new List<IEnumerableAsync<int>> { enum1, enum2, enum3 }, null, null);
            var enumerator = aggregateEnumerable.GetEnumeratorAsync();

            var expectedUniqueCount = 5;
            var actualCount = 0;
            var last = int.MinValue;
            while (await enumerator.MoveNextAsync())
            {
                actualCount++;
                var current = enumerator.Current;
                Assert.True(last < current, "Incorrent order: " + last + " should be before " + current);
                last = current;
            }

            Assert.Equal(expectedUniqueCount, actualCount);

        }
        [Fact]
        public async Task TestTwoNonOverllapingEnums()
        {
            var enum1 = new IntegerEnumerableAsync(new List<int>() { 1, 2, 3, 4, 5 });
            var enum2 = new IntegerEnumerableAsync(new List<int>() { 6, 7, 8, 9, 10 });

            var aggregateEnumerable = new AggregateEnumerableAsync<int>(new List<IEnumerableAsync<int>> { enum1, enum2 }, null, null);
            var enumerator = aggregateEnumerable.GetEnumeratorAsync();

            var expectedUniqueCount = 10;
            var actualCount = 0;
            var last = int.MinValue;

            while (await enumerator.MoveNextAsync())
            {
                actualCount++;
                var current = enumerator.Current;
                Assert.True(last < current, "Incorrent order: " + last + " should be before " + current);
                last = current;
            }

            Assert.Equal(expectedUniqueCount, actualCount);
            // Assert that whole sorting process is commutative
            aggregateEnumerable = new AggregateEnumerableAsync<int>(new List<IEnumerableAsync<int>> { enum2, enum1 }, null, null);
            enumerator = aggregateEnumerable.GetEnumeratorAsync();

            expectedUniqueCount = 10;
            actualCount = 0;
            last = int.MinValue;
            while (await enumerator.MoveNextAsync())
            {
                actualCount++;
                var current = enumerator.Current;
                Assert.True(last < current, "Incorrent order: " + last + " should be before " + current);
                last = current;
            }

            Assert.Equal(expectedUniqueCount, actualCount);
        }

        [Fact]
        public async Task TestTwoOverlappingEnums()
        {
            var enum1 = new IntegerEnumerableAsync(new List<int>() { 1, 4, 5, 8, 9 });
            var enum2 = new IntegerEnumerableAsync(new List<int>() { 2, 3, 6, 7, 10 });

            var aggregateEnumerable = new AggregateEnumerableAsync<int>(new List<IEnumerableAsync<int>> { enum1, enum2 }, null, null);
            var enumerator = aggregateEnumerable.GetEnumeratorAsync();

            var expectedUniqueCount = 10;
            var actualCount = 0;
            var last = int.MinValue;
            while (await enumerator.MoveNextAsync())
            {
                actualCount++;
                var current = enumerator.Current;
                Assert.True(last < current, "Incorrent order: " + last + " should be before " + current);
                last = current;
            }

            Assert.Equal(expectedUniqueCount, actualCount);
            // Assert that whole sorting process is commutative
            aggregateEnumerable = new AggregateEnumerableAsync<int>(new List<IEnumerableAsync<int>> { enum2, enum1 }, null, null);
            enumerator = aggregateEnumerable.GetEnumeratorAsync();

            expectedUniqueCount = 10;
            actualCount = 0;
            last = int.MinValue;
            while (await enumerator.MoveNextAsync())
            {
                actualCount++;
                var current = enumerator.Current;
                Assert.True(last < current, "Incorrent order: " + last + " should be before " + current);
                last = current;
            }

            Assert.Equal(expectedUniqueCount, actualCount);
        }

        [Fact]
        public async Task TestMultipleOverlappingEnums()
        {
            var enum1 = new IntegerEnumerableAsync(new List<int>() { 1, 4, 5, 8, 9 });
            var enum2 = new IntegerEnumerableAsync(new List<int>() { 2, 3, 6, 7, 10 });
            var enum3 = new IntegerEnumerableAsync(new List<int>() { 3, 5, 7, 12, 15 });


            var aggregateEnumerable = new AggregateEnumerableAsync<int>(new List<IEnumerableAsync<int>> { enum1, enum2, enum3 }, null, null);
            var enumerator = aggregateEnumerable.GetEnumeratorAsync();

            var expectedUniqueCount = 12;
            var actualCount = 0;
            var last = int.MinValue;
            while (await enumerator.MoveNextAsync())
            {
                actualCount++;
                var current = enumerator.Current;
                Assert.True(last < current, "Incorrent order: " + last + " should be before " + current);
                last = current;
            }

            Assert.Equal(expectedUniqueCount, actualCount);
            // Assert that whole sorting process is commutative
            aggregateEnumerable = new AggregateEnumerableAsync<int>(new List<IEnumerableAsync<int>> { enum3, enum2, enum1 }, null, null);
            enumerator = aggregateEnumerable.GetEnumeratorAsync();

            expectedUniqueCount = 12;
            actualCount = 0;
            last = int.MinValue;
            while (await enumerator.MoveNextAsync())
            {
                actualCount++;
                var current = enumerator.Current;
                Assert.True(last <= current, "Incorrent order: " + last + " should be before " + current);
                last = current;
            }

            Assert.Equal(expectedUniqueCount, actualCount);
        }
    }

    class IntegerEnumerableAsync : IEnumerableAsync<int>
    {

        private readonly IList<int> _enumInts;
        public IntegerEnumerableAsync(IList<int> enumInts)
        {
            _enumInts = enumInts;
        }

        public IEnumeratorAsync<int> GetEnumeratorAsync()
        {
            return new IntegerEnumeratorAsync(_enumInts);
        }

        class IntegerEnumeratorAsync : IEnumeratorAsync<int>
        {
            private readonly IList<int> _enumInts;
            private IEnumerator<int> enumerator;

            public int Current
            {
                get
                {
                    return enumerator.Current;
                }
            }

            public IntegerEnumeratorAsync(IList<int> enumInts)
            {
                _enumInts = enumInts;
                enumerator = _enumInts.GetEnumerator();

            }

            public Task<bool> MoveNextAsync()
            {
                return Task.FromResult(enumerator.MoveNext());
            }
        }


    }
}
