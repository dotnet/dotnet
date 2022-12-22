// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace NuGet.Indexing.Test
{
    public class MergeTests
    {
        [Fact]
        public void EmptyEnumerationsTest()
        {
            var x = new string[] { };
            var y = new string[] { };

            var result = x.Merge(y, StringComparer.Ordinal);

            Assert.NotNull(result);
            Assert.Equal(0, result.Count());
        }

        [Fact]
        public void EmptyRHSEnumerationTest()
        {
            var x = "acdikswxz";
            var y = string.Empty;

            var result = x.Merge(y, Comparer<char>.Default);

            Assert.NotNull(result);
            Assert.Equal(x, new string(result.ToArray()));
        }

        [Fact]
        public void EmptyLHSEnumerationTest()
        {
            var x = string.Empty;
            var y = "bjqty";

            var result = x.Merge(y, Comparer<char>.Default);

            Assert.NotNull(result);
            Assert.Equal(y, new string(result.ToArray()));
        }

        [Fact]
        public void BasicMergeTest()
        {
            var x = "acdijkswxz";
            var y = "bqty";

            var result = x.Merge(y, Comparer<char>.Default);

            Assert.NotNull(result);
            Assert.Equal("abcdijkqstwxyz", new string(result.ToArray()));
        }

        [Fact]
        public void AllTheSameTest()
        {
            var x = "aaaa";
            var y = "aaaa";

            var result = x.Merge(y, Comparer<char>.Default);

            Assert.NotNull(result);
            Assert.Equal(x + y, new string(result.ToArray()));
        }

        [Fact]
        public void AllLookTheSameTest()
        {
            var x = "aaaa";
            var y = "bbbb";

            var result = x.Merge(y, new NoopComparer<char>());

            Assert.NotNull(result);
            Assert.Equal(x + y, new string(result.ToArray()));
        }

        [Fact]
        public void MergeIsNotASortTest()
        {
            var x = "ace";
            var y = "bdf";

            var result1 = x.Merge(y, Comparer<char>.Default);

            // sorted but only because the inputs are sorted

            Assert.NotNull(result1);
            Assert.Equal("abcdef", new string(result1.ToArray()));

            var z = new string(y.Reverse().ToArray());

            var result2 = x.Merge(z, Comparer<char>.Default);

            // this should NOT be sorted

            Assert.NotNull(result2);
            Assert.Equal(x + z, new string(result2.ToArray()));
        }

        [Fact]
        public void MultiMergeTest()
        {
            var data = new[]
            {
                "einrty",
                "bfjmpz",
                "acgloux",
                "qsvw",
                "dhk"
            };

            var result = Enumerable.Empty<string>();
            foreach (var l in data)
            {
                result = result.Merge(l.Select(ch => ch.ToString()), StringComparer.Ordinal);
            }

            Assert.NotNull(result);
            Assert.Equal("abcdefghijklmnopqrstuvwxyz", string.Concat(result));
        }

        [Fact]
        public void MultiMergeTest2()
        {
            //  multiple results to merge - different lengths including zero length - this is similar to expected usage

            var data = new[]
            {
                new [] { "101", "107", "110" },
                new [] { "201", "205", "211" },
                new string [] { },
                new [] { "303", "304", "308", "309" },
                new [] { "405" },
            };

            //  create a score for each item

            var ranking = new[] { "205", "107", "110", "303", "201", "211", "405", "304", "101", "308", "309" };
            var comparer = new RankingComparer(ranking);

            var expected = "303|201|205|211|405|304|101|107|110|308|309";

            //  pair-wise merging

            var result1 = Enumerable.Empty<string>();
            foreach (var l in data)
            {
                result1 = result1.Merge(l.Select(ch => ch.ToString()), comparer);
            }

            //  check the result

            Assert.NotNull(result1);
            Assert.Equal(expected, string.Join("|", result1));

            //  pair-wise merging - but now in a different order - should be the same result

            var result2 = Enumerable.Empty<string>();
            foreach (var l in data.Reverse())
            {
                result2 = result2.Merge(l.Select(ch => ch.ToString()), comparer);
            }

            //  check the result

            Assert.NotNull(result2);
            Assert.Equal(expected, string.Join("|", result2));
        }

        [Fact]
        public void MergeUsedAsAComponent()
        {
            //  check our enumeration based merge sits comfortably in other algorithms - here a recursive Merge Sort

            var result = "MICROSOFT".Select(ch => ch.ToString()).MergeSort(StringComparer.OrdinalIgnoreCase);

            Assert.NotNull(result);
            Assert.Equal("CFIMOORST", string.Concat(result));
        }

        private class RankingComparer : Comparer<string>
        {
            IDictionary<string, int> _score;
            public RankingComparer(IEnumerable<string> ranking)
            {
                //  create a lookup. here (unlike Lucene) lowest score means better.

                _score = ranking.Zip(
                    Enumerable.Range(0, ranking.Count()), (x, y) => Tuple.Create(x, y))
                    .ToDictionary((i) => i.Item1, (j) => j.Item2);
            }

            public override int Compare(string x, string y)
            {
                return Comparer<int>.Default.Compare(_score[x], _score[y]);
            }
        }
        private class NoopComparer<T> : Comparer<T>
        {
            public override int Compare(T x, T y)
            {
                return 0;
            }
        }
    }
}
