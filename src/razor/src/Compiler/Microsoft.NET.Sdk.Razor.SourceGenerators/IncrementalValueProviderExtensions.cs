
// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Razor;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Microsoft.NET.Sdk.Razor.SourceGenerators
{
    internal static class IncrementalValuesProviderExtensions
    {
        internal static IncrementalValueProvider<T> WithLambdaComparer<T>(this IncrementalValueProvider<T> source, Func<T, T, bool> equal)
        {
            var comparer = new LambdaComparer<T>(equal);
            return source.WithComparer(comparer);
        }

        internal static IncrementalValuesProvider<T> WithLambdaComparer<T>(this IncrementalValuesProvider<T> source, Func<T, T, bool> equal)
        {
            var comparer = new LambdaComparer<T>(equal);
            return source.WithComparer(comparer);
        }

        internal static IncrementalValuesProvider<TSource> ReportDiagnostics<TSource>(this IncrementalValuesProvider<(TSource?, Diagnostic?)> source, IncrementalGeneratorInitializationContext context)
        {
            context.RegisterSourceOutput(source, (spc, source) =>
            {
                var (_, diagnostic) = source;
                if (diagnostic != null)
                {
                    spc.ReportDiagnostic(diagnostic);
                }
            });

            return source.Where((pair) => pair.Item1 != null).Select((pair, ct) => pair.Item1!);
        }

        internal static IncrementalValueProvider<TSource> ReportDiagnostics<TSource>(this IncrementalValueProvider<(TSource?, Diagnostic?)> source, IncrementalGeneratorInitializationContext context)
        {
            context.RegisterSourceOutput(source, (spc, source) =>
            {
                var (_, diagnostic) = source;
                if (diagnostic != null)
                {
                    spc.ReportDiagnostic(diagnostic);
                }
            });

            return source.Select((pair, ct) => pair.Item1!);
        }

        internal static IncrementalValuesProvider<T> EmptyOrCachedWhen<T>(this IncrementalValuesProvider<T> provider, IncrementalValueProvider<bool> checkProvider, bool check)
        {
            // This code is a little hard to understand:
            // Basically you can think about this provider having two states: 'on' and 'off'.
            // When the checkProvider equals the check value, the provider is 'on'
            // When in the 'on' state, data flows through it as usual.
            // When in the 'off' state you get either: an empty provider, if it has never been 'on' before, or the last 'on' data, but all cached.

            // First we create a check provider that 'latches'. That is, once it has been flipped into the 'on' state, it never goes back to 'off'
            var latchedCheckProvider = checkProvider.WithLambdaComparer((old, @new) => IsOn(old) || old == @new);

            // Next we filter on the latched provider. When the provider is off we return an empty array, when it's on we allow data to flow through.
            var dataProvider = provider.Combine(latchedCheckProvider)
                .Where(pair => IsOn(pair.Right))
                .Select((pair, _) => pair.Left);

            // Now, we compare against the real value of the check provider. If the real provider is 'on' we allow the data through. When the provider is
            // 'off' we set all the data to cached. This allows the caches to remain full, but still disable any further downstream processing.
            var realProvider = dataProvider.Combine(checkProvider)
                
                // We have to group and ungroup the data before comparing to ensure that we correctly handle added and removed cases which would otherwise
                // not get compared and would be processed downstream
                .Collect()
                
                // When the real value is 'on', always say the data is modified. When the value is 'off' say it's cached
                .WithLambdaComparer((old, @new) => !IsOn(@new.FirstOrDefault().Right))

                // When 'on' the data will be re-evaluated item-wise here, ensuring only things that have actually changed will be marked as such.
                // When 'off' the previous data was cached so nothing downstream runs.
                .SelectMany((arr, _) => arr)
                .Select((pair, _) => pair.Left);

            return realProvider;

            bool IsOn(bool value) => value != check;
        }

        /// <summary>
        /// Ensures that <paramref name="input"/> reports as up to date if <paramref name="suppressionCheck"/> returns <see langword="true"/>.
        /// </summary>
        internal static IncrementalValueProvider<(T, bool)> SuppressIfNeeded<T>(this IncrementalValueProvider<T> input, IncrementalValueProvider<bool> suppressionCheck)
        {
            return input
                .Combine(suppressionCheck)
                // when the suppression check is true, we always say its up to date. Otherwise we perform the default comparison on the item itself.
                .WithLambdaComparer((old, @new) => @new.Right || EqualityComparer<T>.Default.Equals(old.Left, @new.Left));
        }

        internal static IncrementalValueProvider<bool> CheckGlobalFlagSet(this IncrementalValueProvider<AnalyzerConfigOptionsProvider> optionsProvider, string flagName)
        {
            return optionsProvider.Select((provider, _) => provider.GlobalOptions.TryGetValue($"build_property.{flagName}", out var flagValue) && flagValue == "true");
        }
    }

    internal sealed class LambdaComparer<T> : IEqualityComparer<T>
    {
        private readonly Func<T, T, bool> _equal;

        public LambdaComparer(Func<T, T, bool> equal)
        {
            _equal = equal;
        }

        public bool Equals(T x, T y) => _equal(x, y);

        public int GetHashCode(T obj) => Assumed.Unreachable<int>();
    }
}
