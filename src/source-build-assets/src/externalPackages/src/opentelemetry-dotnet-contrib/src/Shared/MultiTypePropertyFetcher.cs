// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Collections.Concurrent;
using System.Reflection;

namespace OpenTelemetry.Instrumentation;

/// <summary>
/// PropertyFetcher fetches a property from an object.
/// </summary>
/// <typeparam name="T">The type of the property being fetched.</typeparam>
internal sealed class MultiTypePropertyFetcher<T>
{
    private readonly string propertyName;
    private readonly ConcurrentDictionary<Type, PropertyFetch> innerFetcher = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="MultiTypePropertyFetcher{T}"/> class.
    /// </summary>
    /// <param name="propertyName">Property name to fetch.</param>
    public MultiTypePropertyFetcher(string propertyName)
    {
        this.propertyName = propertyName;
    }

    /// <summary>
    /// Fetch the property from the object.
    /// </summary>
    /// <param name="obj">Object to be fetched.</param>
    /// <returns>Property fetched.</returns>
    public T? Fetch(object? obj)
    {
        if (obj == null)
        {
            return default;
        }

        var type = obj.GetType().GetTypeInfo();
        if (!this.innerFetcher.TryGetValue(type, out var fetcher))
        {
            var property =
                type.DeclaredProperties.FirstOrDefault(p => string.Equals(p.Name, this.propertyName, StringComparison.OrdinalIgnoreCase)) ??
                type.GetProperty(this.propertyName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)!;

            fetcher = PropertyFetch.FetcherForProperty(property);

            this.innerFetcher.TryAdd(type, fetcher);
        }

        return fetcher == null ? default : fetcher.Fetch(obj);
    }

    // see https://github.com/dotnet/corefx/blob/master/src/System.Diagnostics.DiagnosticSource/src/System/Diagnostics/DiagnosticSourceEventSource.cs
    private class PropertyFetch
    {
        /// <summary>
        /// Create a property fetcher from a .NET Reflection PropertyInfo class that
        /// represents a property of a particular type.
        /// </summary>
        public static PropertyFetch FetcherForProperty(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null || !typeof(T).IsAssignableFrom(propertyInfo.PropertyType))
            {
                // returns null on any fetch.
                return new PropertyFetch();
            }

            var typedPropertyFetcher = typeof(TypedPropertyFetch<,>);
            var instantiatedTypedPropertyFetcher = typedPropertyFetcher.MakeGenericType(
                typeof(T), propertyInfo.DeclaringType!, propertyInfo.PropertyType);
            return (PropertyFetch)Activator.CreateInstance(instantiatedTypedPropertyFetcher, propertyInfo)!;
        }

        public virtual T? Fetch(object obj) => default;

#pragma warning disable CA1812
        private sealed class TypedPropertyFetch<TDeclaredObject, TDeclaredProperty> : PropertyFetch
#pragma warning restore CA1812
            where TDeclaredProperty : T
        {
            private readonly Func<TDeclaredObject, TDeclaredProperty> propertyFetch;

            public TypedPropertyFetch(PropertyInfo property)
            {
                this.propertyFetch =
#if NET
                    property.GetMethod!.CreateDelegate<Func<TDeclaredObject, TDeclaredProperty>>();
#else
                    (Func<TDeclaredObject, TDeclaredProperty>)property.GetMethod.CreateDelegate(typeof(Func<TDeclaredObject, TDeclaredProperty>));
#endif
            }

            public override T? Fetch(object obj) => obj is TDeclaredObject o ? this.propertyFetch(o) : (T?)default;
        }
    }
}
