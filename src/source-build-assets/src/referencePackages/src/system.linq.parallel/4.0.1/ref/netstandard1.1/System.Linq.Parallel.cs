// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Security.AllowPartiallyTrustedCallers]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyTitle("System.Linq.Parallel")]
[assembly: System.Reflection.AssemblyDescription("System.Linq.Parallel")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Linq.Parallel")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET Framework")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: System.Reflection.AssemblyFileVersion("1.0.24212.01")]
[assembly: System.Reflection.AssemblyInformationalVersion("1.0.24212.01. Commit Hash: 9688ddbb62c04189cac4c4a06e31e93377dccd41")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyMetadata(".NETFrameworkAssembly", "")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyVersionAttribute("4.0.0.0")]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace System.Linq
{
    public partial class OrderedParallelQuery<TSource> : ParallelQuery<TSource>
    {
        internal OrderedParallelQuery() { }

        public override Collections.Generic.IEnumerator<TSource> GetEnumerator() { throw null; }
    }

    public static partial class ParallelEnumerable
    {
        public static TSource Aggregate<TSource>(this ParallelQuery<TSource> source, Func<TSource, TSource, TSource> func) { throw null; }

        public static TAccumulate Aggregate<TSource, TAccumulate>(this ParallelQuery<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func) { throw null; }

        public static TResult Aggregate<TSource, TAccumulate, TResult>(this ParallelQuery<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> updateAccumulatorFunc, Func<TAccumulate, TAccumulate, TAccumulate> combineAccumulatorsFunc, Func<TAccumulate, TResult> resultSelector) { throw null; }

        public static TResult Aggregate<TSource, TAccumulate, TResult>(this ParallelQuery<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func, Func<TAccumulate, TResult> resultSelector) { throw null; }

        public static TResult Aggregate<TSource, TAccumulate, TResult>(this ParallelQuery<TSource> source, Func<TAccumulate> seedFactory, Func<TAccumulate, TSource, TAccumulate> updateAccumulatorFunc, Func<TAccumulate, TAccumulate, TAccumulate> combineAccumulatorsFunc, Func<TAccumulate, TResult> resultSelector) { throw null; }

        public static bool All<TSource>(this ParallelQuery<TSource> source, Func<TSource, bool> predicate) { throw null; }

        public static bool Any<TSource>(this ParallelQuery<TSource> source, Func<TSource, bool> predicate) { throw null; }

        public static bool Any<TSource>(this ParallelQuery<TSource> source) { throw null; }

        public static Collections.Generic.IEnumerable<TSource> AsEnumerable<TSource>(this ParallelQuery<TSource> source) { throw null; }

        public static ParallelQuery AsOrdered(this ParallelQuery source) { throw null; }

        public static ParallelQuery<TSource> AsOrdered<TSource>(this ParallelQuery<TSource> source) { throw null; }

        public static ParallelQuery AsParallel(this Collections.IEnumerable source) { throw null; }

        public static ParallelQuery<TSource> AsParallel<TSource>(this Collections.Concurrent.Partitioner<TSource> source) { throw null; }

        public static ParallelQuery<TSource> AsParallel<TSource>(this Collections.Generic.IEnumerable<TSource> source) { throw null; }

        public static Collections.Generic.IEnumerable<TSource> AsSequential<TSource>(this ParallelQuery<TSource> source) { throw null; }

        public static ParallelQuery<TSource> AsUnordered<TSource>(this ParallelQuery<TSource> source) { throw null; }

        public static decimal Average(this ParallelQuery<decimal> source) { throw null; }

        public static double Average(this ParallelQuery<double> source) { throw null; }

        public static double Average(this ParallelQuery<int> source) { throw null; }

        public static double Average(this ParallelQuery<long> source) { throw null; }

        public static decimal? Average(this ParallelQuery<decimal?> source) { throw null; }

        public static double? Average(this ParallelQuery<double?> source) { throw null; }

        public static double? Average(this ParallelQuery<int?> source) { throw null; }

        public static double? Average(this ParallelQuery<long?> source) { throw null; }

        public static float? Average(this ParallelQuery<float?> source) { throw null; }

        public static float Average(this ParallelQuery<float> source) { throw null; }

        public static decimal Average<TSource>(this ParallelQuery<TSource> source, Func<TSource, decimal> selector) { throw null; }

        public static double Average<TSource>(this ParallelQuery<TSource> source, Func<TSource, double> selector) { throw null; }

        public static double Average<TSource>(this ParallelQuery<TSource> source, Func<TSource, int> selector) { throw null; }

        public static double Average<TSource>(this ParallelQuery<TSource> source, Func<TSource, long> selector) { throw null; }

        public static decimal? Average<TSource>(this ParallelQuery<TSource> source, Func<TSource, decimal?> selector) { throw null; }

        public static double? Average<TSource>(this ParallelQuery<TSource> source, Func<TSource, double?> selector) { throw null; }

        public static double? Average<TSource>(this ParallelQuery<TSource> source, Func<TSource, int?> selector) { throw null; }

        public static double? Average<TSource>(this ParallelQuery<TSource> source, Func<TSource, long?> selector) { throw null; }

        public static float? Average<TSource>(this ParallelQuery<TSource> source, Func<TSource, float?> selector) { throw null; }

        public static float Average<TSource>(this ParallelQuery<TSource> source, Func<TSource, float> selector) { throw null; }

        public static ParallelQuery<TResult> Cast<TResult>(this ParallelQuery source) { throw null; }

        [Obsolete("The second data source of a binary operator must be of type System.Linq.ParallelQuery<T> rather than System.Collections.Generic.IEnumerable<T>. To fix this problem, use the AsParallel() extension method to convert the right data source to System.Linq.ParallelQuery<T>.")]
        public static ParallelQuery<TSource> Concat<TSource>(this ParallelQuery<TSource> first, Collections.Generic.IEnumerable<TSource> second) { throw null; }

        public static ParallelQuery<TSource> Concat<TSource>(this ParallelQuery<TSource> first, ParallelQuery<TSource> second) { throw null; }

        public static bool Contains<TSource>(this ParallelQuery<TSource> source, TSource value, Collections.Generic.IEqualityComparer<TSource> comparer) { throw null; }

        public static bool Contains<TSource>(this ParallelQuery<TSource> source, TSource value) { throw null; }

        public static int Count<TSource>(this ParallelQuery<TSource> source, Func<TSource, bool> predicate) { throw null; }

        public static int Count<TSource>(this ParallelQuery<TSource> source) { throw null; }

        public static ParallelQuery<TSource> DefaultIfEmpty<TSource>(this ParallelQuery<TSource> source, TSource defaultValue) { throw null; }

        public static ParallelQuery<TSource> DefaultIfEmpty<TSource>(this ParallelQuery<TSource> source) { throw null; }

        public static ParallelQuery<TSource> Distinct<TSource>(this ParallelQuery<TSource> source, Collections.Generic.IEqualityComparer<TSource> comparer) { throw null; }

        public static ParallelQuery<TSource> Distinct<TSource>(this ParallelQuery<TSource> source) { throw null; }

        public static TSource ElementAt<TSource>(this ParallelQuery<TSource> source, int index) { throw null; }

        public static TSource ElementAtOrDefault<TSource>(this ParallelQuery<TSource> source, int index) { throw null; }

        public static ParallelQuery<TResult> Empty<TResult>() { throw null; }

        [Obsolete("The second data source of a binary operator must be of type System.Linq.ParallelQuery<T> rather than System.Collections.Generic.IEnumerable<T>. To fix this problem, use the AsParallel() extension method to convert the right data source to System.Linq.ParallelQuery<T>.")]
        public static ParallelQuery<TSource> Except<TSource>(this ParallelQuery<TSource> first, Collections.Generic.IEnumerable<TSource> second, Collections.Generic.IEqualityComparer<TSource> comparer) { throw null; }

        [Obsolete("The second data source of a binary operator must be of type System.Linq.ParallelQuery<T> rather than System.Collections.Generic.IEnumerable<T>. To fix this problem, use the AsParallel() extension method to convert the right data source to System.Linq.ParallelQuery<T>.")]
        public static ParallelQuery<TSource> Except<TSource>(this ParallelQuery<TSource> first, Collections.Generic.IEnumerable<TSource> second) { throw null; }

        public static ParallelQuery<TSource> Except<TSource>(this ParallelQuery<TSource> first, ParallelQuery<TSource> second, Collections.Generic.IEqualityComparer<TSource> comparer) { throw null; }

        public static ParallelQuery<TSource> Except<TSource>(this ParallelQuery<TSource> first, ParallelQuery<TSource> second) { throw null; }

        public static TSource First<TSource>(this ParallelQuery<TSource> source, Func<TSource, bool> predicate) { throw null; }

        public static TSource First<TSource>(this ParallelQuery<TSource> source) { throw null; }

        public static TSource FirstOrDefault<TSource>(this ParallelQuery<TSource> source, Func<TSource, bool> predicate) { throw null; }

        public static TSource FirstOrDefault<TSource>(this ParallelQuery<TSource> source) { throw null; }

        public static void ForAll<TSource>(this ParallelQuery<TSource> source, Action<TSource> action) { }

        public static ParallelQuery<IGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this ParallelQuery<TSource> source, Func<TSource, TKey> keySelector, Collections.Generic.IEqualityComparer<TKey> comparer) { throw null; }

        public static ParallelQuery<IGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this ParallelQuery<TSource> source, Func<TSource, TKey> keySelector) { throw null; }

        public static ParallelQuery<IGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this ParallelQuery<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Collections.Generic.IEqualityComparer<TKey> comparer) { throw null; }

        public static ParallelQuery<IGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this ParallelQuery<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector) { throw null; }

        public static ParallelQuery<TResult> GroupBy<TSource, TKey, TResult>(this ParallelQuery<TSource> source, Func<TSource, TKey> keySelector, Func<TKey, Collections.Generic.IEnumerable<TSource>, TResult> resultSelector, Collections.Generic.IEqualityComparer<TKey> comparer) { throw null; }

        public static ParallelQuery<TResult> GroupBy<TSource, TKey, TResult>(this ParallelQuery<TSource> source, Func<TSource, TKey> keySelector, Func<TKey, Collections.Generic.IEnumerable<TSource>, TResult> resultSelector) { throw null; }

        public static ParallelQuery<TResult> GroupBy<TSource, TKey, TElement, TResult>(this ParallelQuery<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<TKey, Collections.Generic.IEnumerable<TElement>, TResult> resultSelector, Collections.Generic.IEqualityComparer<TKey> comparer) { throw null; }

        public static ParallelQuery<TResult> GroupBy<TSource, TKey, TElement, TResult>(this ParallelQuery<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<TKey, Collections.Generic.IEnumerable<TElement>, TResult> resultSelector) { throw null; }

        [Obsolete("The second data source of a binary operator must be of type System.Linq.ParallelQuery<T> rather than System.Collections.Generic.IEnumerable<T>. To fix this problem, use the AsParallel() extension method to convert the right data source to System.Linq.ParallelQuery<T>.")]
        public static ParallelQuery<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this ParallelQuery<TOuter> outer, Collections.Generic.IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, Collections.Generic.IEnumerable<TInner>, TResult> resultSelector, Collections.Generic.IEqualityComparer<TKey> comparer) { throw null; }

        [Obsolete("The second data source of a binary operator must be of type System.Linq.ParallelQuery<T> rather than System.Collections.Generic.IEnumerable<T>. To fix this problem, use the AsParallel() extension method to convert the right data source to System.Linq.ParallelQuery<T>.")]
        public static ParallelQuery<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this ParallelQuery<TOuter> outer, Collections.Generic.IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, Collections.Generic.IEnumerable<TInner>, TResult> resultSelector) { throw null; }

        public static ParallelQuery<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this ParallelQuery<TOuter> outer, ParallelQuery<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, Collections.Generic.IEnumerable<TInner>, TResult> resultSelector, Collections.Generic.IEqualityComparer<TKey> comparer) { throw null; }

        public static ParallelQuery<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this ParallelQuery<TOuter> outer, ParallelQuery<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, Collections.Generic.IEnumerable<TInner>, TResult> resultSelector) { throw null; }

        [Obsolete("The second data source of a binary operator must be of type System.Linq.ParallelQuery<T> rather than System.Collections.Generic.IEnumerable<T>. To fix this problem, use the AsParallel() extension method to convert the right data source to System.Linq.ParallelQuery<T>.")]
        public static ParallelQuery<TSource> Intersect<TSource>(this ParallelQuery<TSource> first, Collections.Generic.IEnumerable<TSource> second, Collections.Generic.IEqualityComparer<TSource> comparer) { throw null; }

        [Obsolete("The second data source of a binary operator must be of type System.Linq.ParallelQuery<T> rather than System.Collections.Generic.IEnumerable<T>. To fix this problem, use the AsParallel() extension method to convert the right data source to System.Linq.ParallelQuery<T>.")]
        public static ParallelQuery<TSource> Intersect<TSource>(this ParallelQuery<TSource> first, Collections.Generic.IEnumerable<TSource> second) { throw null; }

        public static ParallelQuery<TSource> Intersect<TSource>(this ParallelQuery<TSource> first, ParallelQuery<TSource> second, Collections.Generic.IEqualityComparer<TSource> comparer) { throw null; }

        public static ParallelQuery<TSource> Intersect<TSource>(this ParallelQuery<TSource> first, ParallelQuery<TSource> second) { throw null; }

        [Obsolete("The second data source of a binary operator must be of type System.Linq.ParallelQuery<T> rather than System.Collections.Generic.IEnumerable<T>. To fix this problem, use the AsParallel() extension method to convert the right data source to System.Linq.ParallelQuery<T>.")]
        public static ParallelQuery<TResult> Join<TOuter, TInner, TKey, TResult>(this ParallelQuery<TOuter> outer, Collections.Generic.IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector, Collections.Generic.IEqualityComparer<TKey> comparer) { throw null; }

        [Obsolete("The second data source of a binary operator must be of type System.Linq.ParallelQuery<T> rather than System.Collections.Generic.IEnumerable<T>. To fix this problem, use the AsParallel() extension method to convert the right data source to System.Linq.ParallelQuery<T>.")]
        public static ParallelQuery<TResult> Join<TOuter, TInner, TKey, TResult>(this ParallelQuery<TOuter> outer, Collections.Generic.IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector) { throw null; }

        public static ParallelQuery<TResult> Join<TOuter, TInner, TKey, TResult>(this ParallelQuery<TOuter> outer, ParallelQuery<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector, Collections.Generic.IEqualityComparer<TKey> comparer) { throw null; }

        public static ParallelQuery<TResult> Join<TOuter, TInner, TKey, TResult>(this ParallelQuery<TOuter> outer, ParallelQuery<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector) { throw null; }

        public static TSource Last<TSource>(this ParallelQuery<TSource> source, Func<TSource, bool> predicate) { throw null; }

        public static TSource Last<TSource>(this ParallelQuery<TSource> source) { throw null; }

        public static TSource LastOrDefault<TSource>(this ParallelQuery<TSource> source, Func<TSource, bool> predicate) { throw null; }

        public static TSource LastOrDefault<TSource>(this ParallelQuery<TSource> source) { throw null; }

        public static long LongCount<TSource>(this ParallelQuery<TSource> source, Func<TSource, bool> predicate) { throw null; }

        public static long LongCount<TSource>(this ParallelQuery<TSource> source) { throw null; }

        public static decimal Max(this ParallelQuery<decimal> source) { throw null; }

        public static double Max(this ParallelQuery<double> source) { throw null; }

        public static int Max(this ParallelQuery<int> source) { throw null; }

        public static long Max(this ParallelQuery<long> source) { throw null; }

        public static decimal? Max(this ParallelQuery<decimal?> source) { throw null; }

        public static double? Max(this ParallelQuery<double?> source) { throw null; }

        public static int? Max(this ParallelQuery<int?> source) { throw null; }

        public static long? Max(this ParallelQuery<long?> source) { throw null; }

        public static float? Max(this ParallelQuery<float?> source) { throw null; }

        public static float Max(this ParallelQuery<float> source) { throw null; }

        public static decimal Max<TSource>(this ParallelQuery<TSource> source, Func<TSource, decimal> selector) { throw null; }

        public static double Max<TSource>(this ParallelQuery<TSource> source, Func<TSource, double> selector) { throw null; }

        public static int Max<TSource>(this ParallelQuery<TSource> source, Func<TSource, int> selector) { throw null; }

        public static long Max<TSource>(this ParallelQuery<TSource> source, Func<TSource, long> selector) { throw null; }

        public static decimal? Max<TSource>(this ParallelQuery<TSource> source, Func<TSource, decimal?> selector) { throw null; }

        public static double? Max<TSource>(this ParallelQuery<TSource> source, Func<TSource, double?> selector) { throw null; }

        public static int? Max<TSource>(this ParallelQuery<TSource> source, Func<TSource, int?> selector) { throw null; }

        public static long? Max<TSource>(this ParallelQuery<TSource> source, Func<TSource, long?> selector) { throw null; }

        public static float? Max<TSource>(this ParallelQuery<TSource> source, Func<TSource, float?> selector) { throw null; }

        public static float Max<TSource>(this ParallelQuery<TSource> source, Func<TSource, float> selector) { throw null; }

        public static TSource Max<TSource>(this ParallelQuery<TSource> source) { throw null; }

        public static TResult Max<TSource, TResult>(this ParallelQuery<TSource> source, Func<TSource, TResult> selector) { throw null; }

        public static decimal Min(this ParallelQuery<decimal> source) { throw null; }

        public static double Min(this ParallelQuery<double> source) { throw null; }

        public static int Min(this ParallelQuery<int> source) { throw null; }

        public static long Min(this ParallelQuery<long> source) { throw null; }

        public static decimal? Min(this ParallelQuery<decimal?> source) { throw null; }

        public static double? Min(this ParallelQuery<double?> source) { throw null; }

        public static int? Min(this ParallelQuery<int?> source) { throw null; }

        public static long? Min(this ParallelQuery<long?> source) { throw null; }

        public static float? Min(this ParallelQuery<float?> source) { throw null; }

        public static float Min(this ParallelQuery<float> source) { throw null; }

        public static decimal Min<TSource>(this ParallelQuery<TSource> source, Func<TSource, decimal> selector) { throw null; }

        public static double Min<TSource>(this ParallelQuery<TSource> source, Func<TSource, double> selector) { throw null; }

        public static int Min<TSource>(this ParallelQuery<TSource> source, Func<TSource, int> selector) { throw null; }

        public static long Min<TSource>(this ParallelQuery<TSource> source, Func<TSource, long> selector) { throw null; }

        public static decimal? Min<TSource>(this ParallelQuery<TSource> source, Func<TSource, decimal?> selector) { throw null; }

        public static double? Min<TSource>(this ParallelQuery<TSource> source, Func<TSource, double?> selector) { throw null; }

        public static int? Min<TSource>(this ParallelQuery<TSource> source, Func<TSource, int?> selector) { throw null; }

        public static long? Min<TSource>(this ParallelQuery<TSource> source, Func<TSource, long?> selector) { throw null; }

        public static float? Min<TSource>(this ParallelQuery<TSource> source, Func<TSource, float?> selector) { throw null; }

        public static float Min<TSource>(this ParallelQuery<TSource> source, Func<TSource, float> selector) { throw null; }

        public static TSource Min<TSource>(this ParallelQuery<TSource> source) { throw null; }

        public static TResult Min<TSource, TResult>(this ParallelQuery<TSource> source, Func<TSource, TResult> selector) { throw null; }

        public static ParallelQuery<TResult> OfType<TResult>(this ParallelQuery source) { throw null; }

        public static OrderedParallelQuery<TSource> OrderBy<TSource, TKey>(this ParallelQuery<TSource> source, Func<TSource, TKey> keySelector, Collections.Generic.IComparer<TKey> comparer) { throw null; }

        public static OrderedParallelQuery<TSource> OrderBy<TSource, TKey>(this ParallelQuery<TSource> source, Func<TSource, TKey> keySelector) { throw null; }

        public static OrderedParallelQuery<TSource> OrderByDescending<TSource, TKey>(this ParallelQuery<TSource> source, Func<TSource, TKey> keySelector, Collections.Generic.IComparer<TKey> comparer) { throw null; }

        public static OrderedParallelQuery<TSource> OrderByDescending<TSource, TKey>(this ParallelQuery<TSource> source, Func<TSource, TKey> keySelector) { throw null; }

        public static ParallelQuery<int> Range(int start, int count) { throw null; }

        public static ParallelQuery<TResult> Repeat<TResult>(TResult element, int count) { throw null; }

        public static ParallelQuery<TSource> Reverse<TSource>(this ParallelQuery<TSource> source) { throw null; }

        public static ParallelQuery<TResult> Select<TSource, TResult>(this ParallelQuery<TSource> source, Func<TSource, TResult> selector) { throw null; }

        public static ParallelQuery<TResult> Select<TSource, TResult>(this ParallelQuery<TSource> source, Func<TSource, int, TResult> selector) { throw null; }

        public static ParallelQuery<TResult> SelectMany<TSource, TResult>(this ParallelQuery<TSource> source, Func<TSource, Collections.Generic.IEnumerable<TResult>> selector) { throw null; }

        public static ParallelQuery<TResult> SelectMany<TSource, TResult>(this ParallelQuery<TSource> source, Func<TSource, int, Collections.Generic.IEnumerable<TResult>> selector) { throw null; }

        public static ParallelQuery<TResult> SelectMany<TSource, TCollection, TResult>(this ParallelQuery<TSource> source, Func<TSource, Collections.Generic.IEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector) { throw null; }

        public static ParallelQuery<TResult> SelectMany<TSource, TCollection, TResult>(this ParallelQuery<TSource> source, Func<TSource, int, Collections.Generic.IEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector) { throw null; }

        [Obsolete("The second data source of a binary operator must be of type System.Linq.ParallelQuery<T> rather than System.Collections.Generic.IEnumerable<T>. To fix this problem, use the AsParallel() extension method to convert the right data source to System.Linq.ParallelQuery<T>.")]
        public static bool SequenceEqual<TSource>(this ParallelQuery<TSource> first, Collections.Generic.IEnumerable<TSource> second, Collections.Generic.IEqualityComparer<TSource> comparer) { throw null; }

        [Obsolete("The second data source of a binary operator must be of type System.Linq.ParallelQuery<T> rather than System.Collections.Generic.IEnumerable<T>. To fix this problem, use the AsParallel() extension method to convert the right data source to System.Linq.ParallelQuery<T>.")]
        public static bool SequenceEqual<TSource>(this ParallelQuery<TSource> first, Collections.Generic.IEnumerable<TSource> second) { throw null; }

        public static bool SequenceEqual<TSource>(this ParallelQuery<TSource> first, ParallelQuery<TSource> second, Collections.Generic.IEqualityComparer<TSource> comparer) { throw null; }

        public static bool SequenceEqual<TSource>(this ParallelQuery<TSource> first, ParallelQuery<TSource> second) { throw null; }

        public static TSource Single<TSource>(this ParallelQuery<TSource> source, Func<TSource, bool> predicate) { throw null; }

        public static TSource Single<TSource>(this ParallelQuery<TSource> source) { throw null; }

        public static TSource SingleOrDefault<TSource>(this ParallelQuery<TSource> source, Func<TSource, bool> predicate) { throw null; }

        public static TSource SingleOrDefault<TSource>(this ParallelQuery<TSource> source) { throw null; }

        public static ParallelQuery<TSource> Skip<TSource>(this ParallelQuery<TSource> source, int count) { throw null; }

        public static ParallelQuery<TSource> SkipWhile<TSource>(this ParallelQuery<TSource> source, Func<TSource, bool> predicate) { throw null; }

        public static ParallelQuery<TSource> SkipWhile<TSource>(this ParallelQuery<TSource> source, Func<TSource, int, bool> predicate) { throw null; }

        public static decimal Sum(this ParallelQuery<decimal> source) { throw null; }

        public static double Sum(this ParallelQuery<double> source) { throw null; }

        public static int Sum(this ParallelQuery<int> source) { throw null; }

        public static long Sum(this ParallelQuery<long> source) { throw null; }

        public static decimal? Sum(this ParallelQuery<decimal?> source) { throw null; }

        public static double? Sum(this ParallelQuery<double?> source) { throw null; }

        public static int? Sum(this ParallelQuery<int?> source) { throw null; }

        public static long? Sum(this ParallelQuery<long?> source) { throw null; }

        public static float? Sum(this ParallelQuery<float?> source) { throw null; }

        public static float Sum(this ParallelQuery<float> source) { throw null; }

        public static decimal Sum<TSource>(this ParallelQuery<TSource> source, Func<TSource, decimal> selector) { throw null; }

        public static double Sum<TSource>(this ParallelQuery<TSource> source, Func<TSource, double> selector) { throw null; }

        public static int Sum<TSource>(this ParallelQuery<TSource> source, Func<TSource, int> selector) { throw null; }

        public static long Sum<TSource>(this ParallelQuery<TSource> source, Func<TSource, long> selector) { throw null; }

        public static decimal? Sum<TSource>(this ParallelQuery<TSource> source, Func<TSource, decimal?> selector) { throw null; }

        public static double? Sum<TSource>(this ParallelQuery<TSource> source, Func<TSource, double?> selector) { throw null; }

        public static int? Sum<TSource>(this ParallelQuery<TSource> source, Func<TSource, int?> selector) { throw null; }

        public static long? Sum<TSource>(this ParallelQuery<TSource> source, Func<TSource, long?> selector) { throw null; }

        public static float? Sum<TSource>(this ParallelQuery<TSource> source, Func<TSource, float?> selector) { throw null; }

        public static float Sum<TSource>(this ParallelQuery<TSource> source, Func<TSource, float> selector) { throw null; }

        public static ParallelQuery<TSource> Take<TSource>(this ParallelQuery<TSource> source, int count) { throw null; }

        public static ParallelQuery<TSource> TakeWhile<TSource>(this ParallelQuery<TSource> source, Func<TSource, bool> predicate) { throw null; }

        public static ParallelQuery<TSource> TakeWhile<TSource>(this ParallelQuery<TSource> source, Func<TSource, int, bool> predicate) { throw null; }

        public static OrderedParallelQuery<TSource> ThenBy<TSource, TKey>(this OrderedParallelQuery<TSource> source, Func<TSource, TKey> keySelector, Collections.Generic.IComparer<TKey> comparer) { throw null; }

        public static OrderedParallelQuery<TSource> ThenBy<TSource, TKey>(this OrderedParallelQuery<TSource> source, Func<TSource, TKey> keySelector) { throw null; }

        public static OrderedParallelQuery<TSource> ThenByDescending<TSource, TKey>(this OrderedParallelQuery<TSource> source, Func<TSource, TKey> keySelector, Collections.Generic.IComparer<TKey> comparer) { throw null; }

        public static OrderedParallelQuery<TSource> ThenByDescending<TSource, TKey>(this OrderedParallelQuery<TSource> source, Func<TSource, TKey> keySelector) { throw null; }

        public static TSource[] ToArray<TSource>(this ParallelQuery<TSource> source) { throw null; }

        public static Collections.Generic.Dictionary<TKey, TSource> ToDictionary<TSource, TKey>(this ParallelQuery<TSource> source, Func<TSource, TKey> keySelector, Collections.Generic.IEqualityComparer<TKey> comparer) { throw null; }

        public static Collections.Generic.Dictionary<TKey, TSource> ToDictionary<TSource, TKey>(this ParallelQuery<TSource> source, Func<TSource, TKey> keySelector) { throw null; }

        public static Collections.Generic.Dictionary<TKey, TElement> ToDictionary<TSource, TKey, TElement>(this ParallelQuery<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Collections.Generic.IEqualityComparer<TKey> comparer) { throw null; }

        public static Collections.Generic.Dictionary<TKey, TElement> ToDictionary<TSource, TKey, TElement>(this ParallelQuery<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector) { throw null; }

        public static Collections.Generic.List<TSource> ToList<TSource>(this ParallelQuery<TSource> source) { throw null; }

        public static ILookup<TKey, TSource> ToLookup<TSource, TKey>(this ParallelQuery<TSource> source, Func<TSource, TKey> keySelector, Collections.Generic.IEqualityComparer<TKey> comparer) { throw null; }

        public static ILookup<TKey, TSource> ToLookup<TSource, TKey>(this ParallelQuery<TSource> source, Func<TSource, TKey> keySelector) { throw null; }

        public static ILookup<TKey, TElement> ToLookup<TSource, TKey, TElement>(this ParallelQuery<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Collections.Generic.IEqualityComparer<TKey> comparer) { throw null; }

        public static ILookup<TKey, TElement> ToLookup<TSource, TKey, TElement>(this ParallelQuery<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector) { throw null; }

        [Obsolete("The second data source of a binary operator must be of type System.Linq.ParallelQuery<T> rather than System.Collections.Generic.IEnumerable<T>. To fix this problem, use the AsParallel() extension method to convert the right data source to System.Linq.ParallelQuery<T>.")]
        public static ParallelQuery<TSource> Union<TSource>(this ParallelQuery<TSource> first, Collections.Generic.IEnumerable<TSource> second, Collections.Generic.IEqualityComparer<TSource> comparer) { throw null; }

        [Obsolete("The second data source of a binary operator must be of type System.Linq.ParallelQuery<T> rather than System.Collections.Generic.IEnumerable<T>. To fix this problem, use the AsParallel() extension method to convert the right data source to System.Linq.ParallelQuery<T>.")]
        public static ParallelQuery<TSource> Union<TSource>(this ParallelQuery<TSource> first, Collections.Generic.IEnumerable<TSource> second) { throw null; }

        public static ParallelQuery<TSource> Union<TSource>(this ParallelQuery<TSource> first, ParallelQuery<TSource> second, Collections.Generic.IEqualityComparer<TSource> comparer) { throw null; }

        public static ParallelQuery<TSource> Union<TSource>(this ParallelQuery<TSource> first, ParallelQuery<TSource> second) { throw null; }

        public static ParallelQuery<TSource> Where<TSource>(this ParallelQuery<TSource> source, Func<TSource, bool> predicate) { throw null; }

        public static ParallelQuery<TSource> Where<TSource>(this ParallelQuery<TSource> source, Func<TSource, int, bool> predicate) { throw null; }

        public static ParallelQuery<TSource> WithCancellation<TSource>(this ParallelQuery<TSource> source, Threading.CancellationToken cancellationToken) { throw null; }

        public static ParallelQuery<TSource> WithDegreeOfParallelism<TSource>(this ParallelQuery<TSource> source, int degreeOfParallelism) { throw null; }

        public static ParallelQuery<TSource> WithExecutionMode<TSource>(this ParallelQuery<TSource> source, ParallelExecutionMode executionMode) { throw null; }

        public static ParallelQuery<TSource> WithMergeOptions<TSource>(this ParallelQuery<TSource> source, ParallelMergeOptions mergeOptions) { throw null; }

        [Obsolete("The second data source of a binary operator must be of type System.Linq.ParallelQuery<T> rather than System.Collections.Generic.IEnumerable<T>. To fix this problem, use the AsParallel() extension method to convert the right data source to System.Linq.ParallelQuery<T>.")]
        public static ParallelQuery<TResult> Zip<TFirst, TSecond, TResult>(this ParallelQuery<TFirst> first, Collections.Generic.IEnumerable<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector) { throw null; }

        public static ParallelQuery<TResult> Zip<TFirst, TSecond, TResult>(this ParallelQuery<TFirst> first, ParallelQuery<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector) { throw null; }
    }

    public enum ParallelExecutionMode
    {
        Default = 0,
        ForceParallelism = 1
    }

    public enum ParallelMergeOptions
    {
        Default = 0,
        NotBuffered = 1,
        AutoBuffered = 2,
        FullyBuffered = 3
    }

    public partial class ParallelQuery : Collections.IEnumerable
    {
        internal ParallelQuery() { }

        Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }
    }

    public partial class ParallelQuery<TSource> : ParallelQuery, Collections.Generic.IEnumerable<TSource>, Collections.IEnumerable
    {
        internal ParallelQuery() { }

        public virtual Collections.Generic.IEnumerator<TSource> GetEnumerator() { throw null; }
    }
}