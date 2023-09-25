
// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
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
                var (sourceItem, diagnostic) = source;
                if (sourceItem == null && diagnostic != null)
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
                var (sourceItem, diagnostic) = source;
                if (sourceItem == null && diagnostic != null)
                {
                    spc.ReportDiagnostic(diagnostic);
                }
            });

            return source.Select((pair, ct) => pair.Item1!);
        }

        internal static IncrementalValuesProvider<T> EmptyWhen<T>(this IncrementalValuesProvider<T> provider, IncrementalValueProvider<bool> checkProvider, bool check)
        {
            return provider.Combine(checkProvider)
                .Where(pair => pair.Right != check)
                .Select((pair, _) => pair.Left);
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
