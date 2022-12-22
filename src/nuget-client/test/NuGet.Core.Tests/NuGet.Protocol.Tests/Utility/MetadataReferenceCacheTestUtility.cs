// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using NuGet.Packaging.Core;
using Xunit;

namespace NuGet.Protocol.Tests
{
    public static class MetadataReferenceCacheTestUtility
    {
        private class ReferenceEqualityComparer<T> : IEqualityComparer, IEqualityComparer<T>
        {
            public bool Equals(T x, T y) => Equals((object)x, y);

            public int GetHashCode(T obj) => GetHashCode((object)obj);

            bool IEqualityComparer.Equals(object x, object y) => ReferenceEquals(x, y);

            public int GetHashCode(object obj) => RuntimeHelpers.GetHashCode(obj);
        }

        public static void AssertPackagesHaveSameReferences<T>(T first, T second)
        {
            Assert.NotNull(first);
            Assert.NotNull(second);

            // Get all properties that can be cached and are not Version (because the version of the two packages is different).
            var properties =
                typeof(T).GetTypeInfo()
                    .DeclaredProperties.Where(
                        p =>
                            p.Name != nameof(PackageIdentity.Version) &&
                            MetadataReferenceCache.CachableTypes.Contains(p.PropertyType) && p.GetMethod != null);

            // Check that all cached references between the two packages are identical.
            foreach (var property in properties)
            {
                var firstValue = property.GetMethod.Invoke(first, null);
                var secondValue = property.GetMethod.Invoke(second, null);
                Assert.Same(firstValue, secondValue);
            }

            // Get all properties that are IEnumerables of a type that can be cached.
            var enumerableProperties =
                typeof(T).GetTypeInfo()
                    .DeclaredProperties.Where(
                        p =>
                            p.PropertyType.IsConstructedGenericType &&
                            p.PropertyType.GetGenericTypeDefinition() == typeof(IEnumerable<>) &&
                            p.PropertyType.GenericTypeArguments.Length == 1 &&
                            MetadataReferenceCache.CachableTypes.Contains(p.PropertyType.GenericTypeArguments[0]) &&
                            p.GetMethod != null);

            // Check that all cached references stored in IEnumerables between the two packages are identical.
            foreach (var enumerableProperty in enumerableProperties)
            {
                var firstEnumerable = enumerableProperty.GetMethod.Invoke(first, null);
                var secondEnumerable = enumerableProperty.GetMethod.Invoke(second, null);
                Assert.Equal((IEnumerable<string>)firstEnumerable, (IEnumerable<string>)secondEnumerable,
                    new ReferenceEqualityComparer<string>());
            }
        }
    }
}
