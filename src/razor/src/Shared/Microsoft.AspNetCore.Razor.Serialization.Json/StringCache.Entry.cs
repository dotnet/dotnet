// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.AspNetCore.Razor.Serialization.Json;

internal sealed partial class StringCache
{
    private readonly struct Entry(string target) : IEquatable<Entry>
    {
        // In order to use HashSet we need a stable HashCode, so we have to cache it as soon as it comes in.
        // If the HashCode is unstable then entries in the HashSet become unreachable/unremovable.
        private readonly int _targetHashCode = target.GetHashCode();

        private readonly WeakReference<string> _weakRef = new(target);

        public bool IsAlive => _weakRef.TryGetTarget(out _);

        public bool TryGetTarget([NotNullWhen(true)] out string? target)
            => _weakRef.TryGetTarget(out target);

        public override bool Equals(object? obj)
            => obj is Entry entry &&
               Equals(entry);

        public bool Equals(Entry other)
        {
            if (TryGetTarget(out var thisTarget) && other.TryGetTarget(out var entryTarget))
            {
                return thisTarget.GetHashCode() == entryTarget.GetHashCode() &&
                       thisTarget == entryTarget;
            }

            // We lost the reference, but we need to check RefEquals to ensure that HashSet can successfully Remove items.
            // We can't compare the Entries themselves because as structs they would get Value-Boxed an RefEquals would always be false.
            return ReferenceEquals(_weakRef, other._weakRef);
        }

        public override int GetHashCode()
            => _targetHashCode;
    }
}
