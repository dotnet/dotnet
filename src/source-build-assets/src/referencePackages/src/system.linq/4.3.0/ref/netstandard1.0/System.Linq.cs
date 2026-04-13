// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyInformationalVersion("4.0.30319.17929")]
[assembly: System.Security.AllowPartiallyTrustedCallers]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Reflection.AssemblyTitle("System.Linq.dll")]
[assembly: System.Reflection.AssemblyDescription("System.Linq.dll")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Linq.dll")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET Framework")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: System.Reflection.AssemblyFileVersion("4.0.30319.17929")]
[assembly: System.Reflection.AssemblyVersionAttribute("4.0.0.0")]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace System.Linq
{
    public static partial class Enumerable
    {
        public static TSource Aggregate<TSource>(this Collections.Generic.IEnumerable<TSource> source, Func<TSource, TSource, TSource> func) { throw null; }

        public static TAccumulate Aggregate<TSource, TAccumulate>(this Collections.Generic.IEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func) { throw null; }

        public static TResult Aggregate<TSource, TAccumulate, TResult>(this Collections.Generic.IEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func, Func<TAccumulate, TResult> resultSelector) { throw null; }

        public static bool All<TSource>(this Collections.Generic.IEnumerable<TSource> source, Func<TSource, bool> predicate) { throw null; }

        public static bool Any<TSource>(this Collections.Generic.IEnumerable<TSource> source, Func<TSource, bool> predicate) { throw null; }

        public static bool Any<TSource>(this Collections.Generic.IEnumerable<TSource> source) { throw null; }

        public static Collections.Generic.IEnumerable<TSource> AsEnumerable<TSource>(this Collections.Generic.IEnumerable<TSource> source) { throw null; }

        public static decimal Average(this Collections.Generic.IEnumerable<decimal> source) { throw null; }

        public static double Average(this Collections.Generic.IEnumerable<double> source) { throw null; }

        public static double Average(this Collections.Generic.IEnumerable<int> source) { throw null; }

        public static double Average(this Collections.Generic.IEnumerable<long> source) { throw null; }

        public static decimal? Average(this Collections.Generic.IEnumerable<decimal?> source) { throw null; }

        public static double? Average(this Collections.Generic.IEnumerable<double?> source) { throw null; }

        public static double? Average(this Collections.Generic.IEnumerable<int?> source) { throw null; }

        public static double? Average(this Collections.Generic.IEnumerable<long?> source) { throw null; }

        public static float? Average(this Collections.Generic.IEnumerable<float?> source) { throw null; }

        public static float Average(this Collections.Generic.IEnumerable<float> source) { throw null; }

        public static decimal Average<TSource>(this Collections.Generic.IEnumerable<TSource> source, Func<TSource, decimal> selector) { throw null; }

        public static double Average<TSource>(this Collections.Generic.IEnumerable<TSource> source, Func<TSource, double> selector) { throw null; }

        public static double Average<TSource>(this Collections.Generic.IEnumerable<TSource> source, Func<TSource, int> selector) { throw null; }

        public static double Average<TSource>(this Collections.Generic.IEnumerable<TSource> source, Func<TSource, long> selector) { throw null; }

        public static decimal? Average<TSource>(this Collections.Generic.IEnumerable<TSource> source, Func<TSource, decimal?> selector) { throw null; }

        public static double? Average<TSource>(this Collections.Generic.IEnumerable<TSource> source, Func<TSource, double?> selector) { throw null; }

        public static double? Average<TSource>(this Collections.Generic.IEnumerable<TSource> source, Func<TSource, int?> selector) { throw null; }

        public static double? Average<TSource>(this Collections.Generic.IEnumerable<TSource> source, Func<TSource, long?> selector) { throw null; }

        public static float? Average<TSource>(this Collections.Generic.IEnumerable<TSource> source, Func<TSource, float?> selector) { throw null; }

        public static float Average<TSource>(this Collections.Generic.IEnumerable<TSource> source, Func<TSource, float> selector) { throw null; }

        public static Collections.Generic.IEnumerable<TResult> Cast<TResult>(this Collections.IEnumerable source) { throw null; }

        public static Collections.Generic.IEnumerable<TSource> Concat<TSource>(this Collections.Generic.IEnumerable<TSource> first, Collections.Generic.IEnumerable<TSource> second) { throw null; }

        public static bool Contains<TSource>(this Collections.Generic.IEnumerable<TSource> source, TSource value, Collections.Generic.IEqualityComparer<TSource> comparer) { throw null; }

        public static bool Contains<TSource>(this Collections.Generic.IEnumerable<TSource> source, TSource value) { throw null; }

        public static int Count<TSource>(this Collections.Generic.IEnumerable<TSource> source, Func<TSource, bool> predicate) { throw null; }

        public static int Count<TSource>(this Collections.Generic.IEnumerable<TSource> source) { throw null; }

        public static Collections.Generic.IEnumerable<TSource> DefaultIfEmpty<TSource>(this Collections.Generic.IEnumerable<TSource> source, TSource defaultValue) { throw null; }

        public static Collections.Generic.IEnumerable<TSource> DefaultIfEmpty<TSource>(this Collections.Generic.IEnumerable<TSource> source) { throw null; }

        public static Collections.Generic.IEnumerable<TSource> Distinct<TSource>(this Collections.Generic.IEnumerable<TSource> source, Collections.Generic.IEqualityComparer<TSource> comparer) { throw null; }

        public static Collections.Generic.IEnumerable<TSource> Distinct<TSource>(this Collections.Generic.IEnumerable<TSource> source) { throw null; }

        public static TSource ElementAt<TSource>(this Collections.Generic.IEnumerable<TSource> source, int index) { throw null; }

        public static TSource ElementAtOrDefault<TSource>(this Collections.Generic.IEnumerable<TSource> source, int index) { throw null; }

        public static Collections.Generic.IEnumerable<TResult> Empty<TResult>() { throw null; }

        public static Collections.Generic.IEnumerable<TSource> Except<TSource>(this Collections.Generic.IEnumerable<TSource> first, Collections.Generic.IEnumerable<TSource> second, Collections.Generic.IEqualityComparer<TSource> comparer) { throw null; }

        public static Collections.Generic.IEnumerable<TSource> Except<TSource>(this Collections.Generic.IEnumerable<TSource> first, Collections.Generic.IEnumerable<TSource> second) { throw null; }

        public static TSource First<TSource>(this Collections.Generic.IEnumerable<TSource> source, Func<TSource, bool> predicate) { throw null; }

        public static TSource First<TSource>(this Collections.Generic.IEnumerable<TSource> source) { throw null; }

        public static TSource FirstOrDefault<TSource>(this Collections.Generic.IEnumerable<TSource> source, Func<TSource, bool> predicate) { throw null; }

        public static TSource FirstOrDefault<TSource>(this Collections.Generic.IEnumerable<TSource> source) { throw null; }

        public static Collections.Generic.IEnumerable<IGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this Collections.Generic.IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Collections.Generic.IEqualityComparer<TKey> comparer) { throw null; }

        public static Collections.Generic.IEnumerable<IGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this Collections.Generic.IEnumerable<TSource> source, Func<TSource, TKey> keySelector) { throw null; }

        public static Collections.Generic.IEnumerable<IGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this Collections.Generic.IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Collections.Generic.IEqualityComparer<TKey> comparer) { throw null; }

        public static Collections.Generic.IEnumerable<IGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this Collections.Generic.IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector) { throw null; }

        public static Collections.Generic.IEnumerable<TResult> GroupBy<TSource, TKey, TResult>(this Collections.Generic.IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TKey, Collections.Generic.IEnumerable<TSource>, TResult> resultSelector, Collections.Generic.IEqualityComparer<TKey> comparer) { throw null; }

        public static Collections.Generic.IEnumerable<TResult> GroupBy<TSource, TKey, TResult>(this Collections.Generic.IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TKey, Collections.Generic.IEnumerable<TSource>, TResult> resultSelector) { throw null; }

        public static Collections.Generic.IEnumerable<TResult> GroupBy<TSource, TKey, TElement, TResult>(this Collections.Generic.IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<TKey, Collections.Generic.IEnumerable<TElement>, TResult> resultSelector, Collections.Generic.IEqualityComparer<TKey> comparer) { throw null; }

        public static Collections.Generic.IEnumerable<TResult> GroupBy<TSource, TKey, TElement, TResult>(this Collections.Generic.IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<TKey, Collections.Generic.IEnumerable<TElement>, TResult> resultSelector) { throw null; }

        public static Collections.Generic.IEnumerable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this Collections.Generic.IEnumerable<TOuter> outer, Collections.Generic.IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, Collections.Generic.IEnumerable<TInner>, TResult> resultSelector, Collections.Generic.IEqualityComparer<TKey> comparer) { throw null; }

        public static Collections.Generic.IEnumerable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this Collections.Generic.IEnumerable<TOuter> outer, Collections.Generic.IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, Collections.Generic.IEnumerable<TInner>, TResult> resultSelector) { throw null; }

        public static Collections.Generic.IEnumerable<TSource> Intersect<TSource>(this Collections.Generic.IEnumerable<TSource> first, Collections.Generic.IEnumerable<TSource> second, Collections.Generic.IEqualityComparer<TSource> comparer) { throw null; }

        public static Collections.Generic.IEnumerable<TSource> Intersect<TSource>(this Collections.Generic.IEnumerable<TSource> first, Collections.Generic.IEnumerable<TSource> second) { throw null; }

        public static Collections.Generic.IEnumerable<TResult> Join<TOuter, TInner, TKey, TResult>(this Collections.Generic.IEnumerable<TOuter> outer, Collections.Generic.IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector, Collections.Generic.IEqualityComparer<TKey> comparer) { throw null; }

        public static Collections.Generic.IEnumerable<TResult> Join<TOuter, TInner, TKey, TResult>(this Collections.Generic.IEnumerable<TOuter> outer, Collections.Generic.IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector) { throw null; }

        public static TSource Last<TSource>(this Collections.Generic.IEnumerable<TSource> source, Func<TSource, bool> predicate) { throw null; }

        public static TSource Last<TSource>(this Collections.Generic.IEnumerable<TSource> source) { throw null; }

        public static TSource LastOrDefault<TSource>(this Collections.Generic.IEnumerable<TSource> source, Func<TSource, bool> predicate) { throw null; }

        public static TSource LastOrDefault<TSource>(this Collections.Generic.IEnumerable<TSource> source) { throw null; }

        public static long LongCount<TSource>(this Collections.Generic.IEnumerable<TSource> source, Func<TSource, bool> predicate) { throw null; }

        public static long LongCount<TSource>(this Collections.Generic.IEnumerable<TSource> source) { throw null; }

        public static decimal Max(this Collections.Generic.IEnumerable<decimal> source) { throw null; }

        public static double Max(this Collections.Generic.IEnumerable<double> source) { throw null; }

        public static int Max(this Collections.Generic.IEnumerable<int> source) { throw null; }

        public static long Max(this Collections.Generic.IEnumerable<long> source) { throw null; }

        public static decimal? Max(this Collections.Generic.IEnumerable<decimal?> source) { throw null; }

        public static double? Max(this Collections.Generic.IEnumerable<double?> source) { throw null; }

        public static int? Max(this Collections.Generic.IEnumerable<int?> source) { throw null; }

        public static long? Max(this Collections.Generic.IEnumerable<long?> source) { throw null; }

        public static float? Max(this Collections.Generic.IEnumerable<float?> source) { throw null; }

        public static float Max(this Collections.Generic.IEnumerable<float> source) { throw null; }

        public static decimal Max<TSource>(this Collections.Generic.IEnumerable<TSource> source, Func<TSource, decimal> selector) { throw null; }

        public static double Max<TSource>(this Collections.Generic.IEnumerable<TSource> source, Func<TSource, double> selector) { throw null; }

        public static int Max<TSource>(this Collections.Generic.IEnumerable<TSource> source, Func<TSource, int> selector) { throw null; }

        public static long Max<TSource>(this Collections.Generic.IEnumerable<TSource> source, Func<TSource, long> selector) { throw null; }

        public static decimal? Max<TSource>(this Collections.Generic.IEnumerable<TSource> source, Func<TSource, decimal?> selector) { throw null; }

        public static double? Max<TSource>(this Collections.Generic.IEnumerable<TSource> source, Func<TSource, double?> selector) { throw null; }

        public static int? Max<TSource>(this Collections.Generic.IEnumerable<TSource> source, Func<TSource, int?> selector) { throw null; }

        public static long? Max<TSource>(this Collections.Generic.IEnumerable<TSource> source, Func<TSource, long?> selector) { throw null; }

        public static float? Max<TSource>(this Collections.Generic.IEnumerable<TSource> source, Func<TSource, float?> selector) { throw null; }

        public static float Max<TSource>(this Collections.Generic.IEnumerable<TSource> source, Func<TSource, float> selector) { throw null; }

        public static TSource Max<TSource>(this Collections.Generic.IEnumerable<TSource> source) { throw null; }

        public static TResult Max<TSource, TResult>(this Collections.Generic.IEnumerable<TSource> source, Func<TSource, TResult> selector) { throw null; }

        public static decimal Min(this Collections.Generic.IEnumerable<decimal> source) { throw null; }

        public static double Min(this Collections.Generic.IEnumerable<double> source) { throw null; }

        public static int Min(this Collections.Generic.IEnumerable<int> source) { throw null; }

        public static long Min(this Collections.Generic.IEnumerable<long> source) { throw null; }

        public static decimal? Min(this Collections.Generic.IEnumerable<decimal?> source) { throw null; }

        public static double? Min(this Collections.Generic.IEnumerable<double?> source) { throw null; }

        public static int? Min(this Collections.Generic.IEnumerable<int?> source) { throw null; }

        public static long? Min(this Collections.Generic.IEnumerable<long?> source) { throw null; }

        public static float? Min(this Collections.Generic.IEnumerable<float?> source) { throw null; }

        public static float Min(this Collections.Generic.IEnumerable<float> source) { throw null; }

        public static decimal Min<TSource>(this Collections.Generic.IEnumerable<TSource> source, Func<TSource, decimal> selector) { throw null; }

        public static double Min<TSource>(this Collections.Generic.IEnumerable<TSource> source, Func<TSource, double> selector) { throw null; }

        public static int Min<TSource>(this Collections.Generic.IEnumerable<TSource> source, Func<TSource, int> selector) { throw null; }

        public static long Min<TSource>(this Collections.Generic.IEnumerable<TSource> source, Func<TSource, long> selector) { throw null; }

        public static decimal? Min<TSource>(this Collections.Generic.IEnumerable<TSource> source, Func<TSource, decimal?> selector) { throw null; }

        public static double? Min<TSource>(this Collections.Generic.IEnumerable<TSource> source, Func<TSource, double?> selector) { throw null; }

        public static int? Min<TSource>(this Collections.Generic.IEnumerable<TSource> source, Func<TSource, int?> selector) { throw null; }

        public static long? Min<TSource>(this Collections.Generic.IEnumerable<TSource> source, Func<TSource, long?> selector) { throw null; }

        public static float? Min<TSource>(this Collections.Generic.IEnumerable<TSource> source, Func<TSource, float?> selector) { throw null; }

        public static float Min<TSource>(this Collections.Generic.IEnumerable<TSource> source, Func<TSource, float> selector) { throw null; }

        public static TSource Min<TSource>(this Collections.Generic.IEnumerable<TSource> source) { throw null; }

        public static TResult Min<TSource, TResult>(this Collections.Generic.IEnumerable<TSource> source, Func<TSource, TResult> selector) { throw null; }

        public static Collections.Generic.IEnumerable<TResult> OfType<TResult>(this Collections.IEnumerable source) { throw null; }

        public static IOrderedEnumerable<TSource> OrderBy<TSource, TKey>(this Collections.Generic.IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Collections.Generic.IComparer<TKey> comparer) { throw null; }

        public static IOrderedEnumerable<TSource> OrderBy<TSource, TKey>(this Collections.Generic.IEnumerable<TSource> source, Func<TSource, TKey> keySelector) { throw null; }

        public static IOrderedEnumerable<TSource> OrderByDescending<TSource, TKey>(this Collections.Generic.IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Collections.Generic.IComparer<TKey> comparer) { throw null; }

        public static IOrderedEnumerable<TSource> OrderByDescending<TSource, TKey>(this Collections.Generic.IEnumerable<TSource> source, Func<TSource, TKey> keySelector) { throw null; }

        public static Collections.Generic.IEnumerable<int> Range(int start, int count) { throw null; }

        public static Collections.Generic.IEnumerable<TResult> Repeat<TResult>(TResult element, int count) { throw null; }

        public static Collections.Generic.IEnumerable<TSource> Reverse<TSource>(this Collections.Generic.IEnumerable<TSource> source) { throw null; }

        public static Collections.Generic.IEnumerable<TResult> Select<TSource, TResult>(this Collections.Generic.IEnumerable<TSource> source, Func<TSource, TResult> selector) { throw null; }

        public static Collections.Generic.IEnumerable<TResult> Select<TSource, TResult>(this Collections.Generic.IEnumerable<TSource> source, Func<TSource, int, TResult> selector) { throw null; }

        public static Collections.Generic.IEnumerable<TResult> SelectMany<TSource, TResult>(this Collections.Generic.IEnumerable<TSource> source, Func<TSource, Collections.Generic.IEnumerable<TResult>> selector) { throw null; }

        public static Collections.Generic.IEnumerable<TResult> SelectMany<TSource, TResult>(this Collections.Generic.IEnumerable<TSource> source, Func<TSource, int, Collections.Generic.IEnumerable<TResult>> selector) { throw null; }

        public static Collections.Generic.IEnumerable<TResult> SelectMany<TSource, TCollection, TResult>(this Collections.Generic.IEnumerable<TSource> source, Func<TSource, Collections.Generic.IEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector) { throw null; }

        public static Collections.Generic.IEnumerable<TResult> SelectMany<TSource, TCollection, TResult>(this Collections.Generic.IEnumerable<TSource> source, Func<TSource, int, Collections.Generic.IEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector) { throw null; }

        public static bool SequenceEqual<TSource>(this Collections.Generic.IEnumerable<TSource> first, Collections.Generic.IEnumerable<TSource> second, Collections.Generic.IEqualityComparer<TSource> comparer) { throw null; }

        public static bool SequenceEqual<TSource>(this Collections.Generic.IEnumerable<TSource> first, Collections.Generic.IEnumerable<TSource> second) { throw null; }

        public static TSource Single<TSource>(this Collections.Generic.IEnumerable<TSource> source, Func<TSource, bool> predicate) { throw null; }

        public static TSource Single<TSource>(this Collections.Generic.IEnumerable<TSource> source) { throw null; }

        public static TSource SingleOrDefault<TSource>(this Collections.Generic.IEnumerable<TSource> source, Func<TSource, bool> predicate) { throw null; }

        public static TSource SingleOrDefault<TSource>(this Collections.Generic.IEnumerable<TSource> source) { throw null; }

        public static Collections.Generic.IEnumerable<TSource> Skip<TSource>(this Collections.Generic.IEnumerable<TSource> source, int count) { throw null; }

        public static Collections.Generic.IEnumerable<TSource> SkipWhile<TSource>(this Collections.Generic.IEnumerable<TSource> source, Func<TSource, bool> predicate) { throw null; }

        public static Collections.Generic.IEnumerable<TSource> SkipWhile<TSource>(this Collections.Generic.IEnumerable<TSource> source, Func<TSource, int, bool> predicate) { throw null; }

        public static decimal Sum(this Collections.Generic.IEnumerable<decimal> source) { throw null; }

        public static double Sum(this Collections.Generic.IEnumerable<double> source) { throw null; }

        public static int Sum(this Collections.Generic.IEnumerable<int> source) { throw null; }

        public static long Sum(this Collections.Generic.IEnumerable<long> source) { throw null; }

        public static decimal? Sum(this Collections.Generic.IEnumerable<decimal?> source) { throw null; }

        public static double? Sum(this Collections.Generic.IEnumerable<double?> source) { throw null; }

        public static int? Sum(this Collections.Generic.IEnumerable<int?> source) { throw null; }

        public static long? Sum(this Collections.Generic.IEnumerable<long?> source) { throw null; }

        public static float? Sum(this Collections.Generic.IEnumerable<float?> source) { throw null; }

        public static float Sum(this Collections.Generic.IEnumerable<float> source) { throw null; }

        public static decimal Sum<TSource>(this Collections.Generic.IEnumerable<TSource> source, Func<TSource, decimal> selector) { throw null; }

        public static double Sum<TSource>(this Collections.Generic.IEnumerable<TSource> source, Func<TSource, double> selector) { throw null; }

        public static int Sum<TSource>(this Collections.Generic.IEnumerable<TSource> source, Func<TSource, int> selector) { throw null; }

        public static long Sum<TSource>(this Collections.Generic.IEnumerable<TSource> source, Func<TSource, long> selector) { throw null; }

        public static decimal? Sum<TSource>(this Collections.Generic.IEnumerable<TSource> source, Func<TSource, decimal?> selector) { throw null; }

        public static double? Sum<TSource>(this Collections.Generic.IEnumerable<TSource> source, Func<TSource, double?> selector) { throw null; }

        public static int? Sum<TSource>(this Collections.Generic.IEnumerable<TSource> source, Func<TSource, int?> selector) { throw null; }

        public static long? Sum<TSource>(this Collections.Generic.IEnumerable<TSource> source, Func<TSource, long?> selector) { throw null; }

        public static float? Sum<TSource>(this Collections.Generic.IEnumerable<TSource> source, Func<TSource, float?> selector) { throw null; }

        public static float Sum<TSource>(this Collections.Generic.IEnumerable<TSource> source, Func<TSource, float> selector) { throw null; }

        public static Collections.Generic.IEnumerable<TSource> Take<TSource>(this Collections.Generic.IEnumerable<TSource> source, int count) { throw null; }

        public static Collections.Generic.IEnumerable<TSource> TakeWhile<TSource>(this Collections.Generic.IEnumerable<TSource> source, Func<TSource, bool> predicate) { throw null; }

        public static Collections.Generic.IEnumerable<TSource> TakeWhile<TSource>(this Collections.Generic.IEnumerable<TSource> source, Func<TSource, int, bool> predicate) { throw null; }

        public static IOrderedEnumerable<TSource> ThenBy<TSource, TKey>(this IOrderedEnumerable<TSource> source, Func<TSource, TKey> keySelector, Collections.Generic.IComparer<TKey> comparer) { throw null; }

        public static IOrderedEnumerable<TSource> ThenBy<TSource, TKey>(this IOrderedEnumerable<TSource> source, Func<TSource, TKey> keySelector) { throw null; }

        public static IOrderedEnumerable<TSource> ThenByDescending<TSource, TKey>(this IOrderedEnumerable<TSource> source, Func<TSource, TKey> keySelector, Collections.Generic.IComparer<TKey> comparer) { throw null; }

        public static IOrderedEnumerable<TSource> ThenByDescending<TSource, TKey>(this IOrderedEnumerable<TSource> source, Func<TSource, TKey> keySelector) { throw null; }

        public static TSource[] ToArray<TSource>(this Collections.Generic.IEnumerable<TSource> source) { throw null; }

        public static Collections.Generic.Dictionary<TKey, TSource> ToDictionary<TSource, TKey>(this Collections.Generic.IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Collections.Generic.IEqualityComparer<TKey> comparer) { throw null; }

        public static Collections.Generic.Dictionary<TKey, TSource> ToDictionary<TSource, TKey>(this Collections.Generic.IEnumerable<TSource> source, Func<TSource, TKey> keySelector) { throw null; }

        public static Collections.Generic.Dictionary<TKey, TElement> ToDictionary<TSource, TKey, TElement>(this Collections.Generic.IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Collections.Generic.IEqualityComparer<TKey> comparer) { throw null; }

        public static Collections.Generic.Dictionary<TKey, TElement> ToDictionary<TSource, TKey, TElement>(this Collections.Generic.IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector) { throw null; }

        public static Collections.Generic.List<TSource> ToList<TSource>(this Collections.Generic.IEnumerable<TSource> source) { throw null; }

        public static ILookup<TKey, TSource> ToLookup<TSource, TKey>(this Collections.Generic.IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Collections.Generic.IEqualityComparer<TKey> comparer) { throw null; }

        public static ILookup<TKey, TSource> ToLookup<TSource, TKey>(this Collections.Generic.IEnumerable<TSource> source, Func<TSource, TKey> keySelector) { throw null; }

        public static ILookup<TKey, TElement> ToLookup<TSource, TKey, TElement>(this Collections.Generic.IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Collections.Generic.IEqualityComparer<TKey> comparer) { throw null; }

        public static ILookup<TKey, TElement> ToLookup<TSource, TKey, TElement>(this Collections.Generic.IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector) { throw null; }

        public static Collections.Generic.IEnumerable<TSource> Union<TSource>(this Collections.Generic.IEnumerable<TSource> first, Collections.Generic.IEnumerable<TSource> second, Collections.Generic.IEqualityComparer<TSource> comparer) { throw null; }

        public static Collections.Generic.IEnumerable<TSource> Union<TSource>(this Collections.Generic.IEnumerable<TSource> first, Collections.Generic.IEnumerable<TSource> second) { throw null; }

        public static Collections.Generic.IEnumerable<TSource> Where<TSource>(this Collections.Generic.IEnumerable<TSource> source, Func<TSource, bool> predicate) { throw null; }

        public static Collections.Generic.IEnumerable<TSource> Where<TSource>(this Collections.Generic.IEnumerable<TSource> source, Func<TSource, int, bool> predicate) { throw null; }

        public static Collections.Generic.IEnumerable<TResult> Zip<TFirst, TSecond, TResult>(this Collections.Generic.IEnumerable<TFirst> first, Collections.Generic.IEnumerable<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector) { throw null; }
    }

    public partial interface IGrouping<out TKey, out TElement> : Collections.Generic.IEnumerable<TElement>, Collections.IEnumerable
    {
        TKey Key { get; }
    }

    public partial interface ILookup<TKey, TElement> : Collections.Generic.IEnumerable<IGrouping<TKey, TElement>>, Collections.IEnumerable
    {
        int Count { get; }

        Collections.Generic.IEnumerable<TElement> this[TKey key] { get; }

        bool Contains(TKey key);
    }

    public partial interface IOrderedEnumerable<TElement> : Collections.Generic.IEnumerable<TElement>, Collections.IEnumerable
    {
        IOrderedEnumerable<TElement> CreateOrderedEnumerable<TKey>(Func<TElement, TKey> keySelector, Collections.Generic.IComparer<TKey> comparer, bool descending);
    }

    public partial class Lookup<TKey, TElement> : ILookup<TKey, TElement>, Collections.Generic.IEnumerable<IGrouping<TKey, TElement>>, Collections.IEnumerable
    {
        internal Lookup() { }

        public int Count { get { throw null; } }

        public Collections.Generic.IEnumerable<TElement> this[TKey key] { get { throw null; } }

        public Collections.Generic.IEnumerable<TResult> ApplyResultSelector<TResult>(Func<TKey, Collections.Generic.IEnumerable<TElement>, TResult> resultSelector) { throw null; }

        public bool Contains(TKey key) { throw null; }

        public Collections.Generic.IEnumerator<IGrouping<TKey, TElement>> GetEnumerator() { throw null; }

        Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }
    }
}