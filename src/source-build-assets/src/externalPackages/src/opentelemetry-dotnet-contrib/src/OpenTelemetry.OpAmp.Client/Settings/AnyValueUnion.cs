// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using OpenTelemetry.OpAmp.Client.Internal.Settings;

namespace OpenTelemetry.OpAmp.Client.Settings;

/// <summary>
/// Represents a union type that can hold a value of one of several predefined types: integer, double, boolean, or string.
/// </summary>
public readonly struct AnyValueUnion : IEquatable<AnyValueUnion>
{
    internal AnyValueUnion(AnyValueType type, int? intValue = null, bool? boolValue = null, string? stringValue = null, double? doubleValue = null, ICollection<AnyValueUnion>? arrayValue = null)
    {
        this.Type = type;
        this.IntValue = intValue;
        this.BoolValue = boolValue;
        this.StringValue = stringValue;
        this.DoubleValue = doubleValue;
        this.ArrayValue = arrayValue;

        // No value defined
        if (intValue == null && boolValue == null && stringValue == null && doubleValue == null && arrayValue == null)
        {
            switch (type)
            {
                case AnyValueType.String:
                    throw new ArgumentNullException(nameof(stringValue), "Must not be null");
                case AnyValueType.Boolean:
                    throw new ArgumentNullException(nameof(boolValue), "Must not be null");
                case AnyValueType.Integer:
                    throw new ArgumentNullException(nameof(intValue), "Must not be null");
                case AnyValueType.Double:
                    throw new ArgumentNullException(nameof(doubleValue), "Must not be null");
                case AnyValueType.Array:
                    throw new ArgumentNullException(nameof(arrayValue), "Must not be null");
                default:
                    Debug.Fail($"Missing check for AnyValueType of '{type}'");
                    return;
            }
        }
    }

    /// <summary>
    /// Gets the integer value associated with this instance.
    /// </summary>
    public int? IntValue { get; }

    /// <summary>
    /// Gets the boolean value indicating the current state or condition.
    /// </summary>
    public bool? BoolValue { get; }

    /// <summary>
    /// Gets the string value associated with this instance.
    /// </summary>
    public string? StringValue { get; }

    /// <summary>
    /// Gets the double value associated with this instance.
    /// </summary>
    public double? DoubleValue { get; }

    /// <summary>
    /// Gets the array value associated with this instance.
    /// </summary>
    public ICollection<AnyValueUnion>? ArrayValue { get; }

    /// <summary>
    /// Gets the type of the value represented by this instance.
    /// </summary>
    internal AnyValueType Type { get; }

    /// <summary>
    /// Determines whether two <see cref="AnyValueUnion"/> instances are equal.
    /// </summary>
    /// <param name="left">The first <see cref="AnyValueUnion"/> instance to compare.</param>
    /// <param name="right">The second <see cref="AnyValueUnion"/> instance to compare.</param>
    /// <returns><see langword="true"/> if the two instances are equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator ==(AnyValueUnion left, AnyValueUnion right)
        => left.Equals(right);

    /// <summary>
    /// Determines whether two <see cref="AnyValueUnion"/> instances are not equal.
    /// </summary>
    /// <param name="left">The first <see cref="AnyValueUnion"/> instance to compare.</param>
    /// <param name="right">The second <see cref="AnyValueUnion"/> instance to compare.</param>
    /// <returns><see langword="true"/> if the two instances are not equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator !=(AnyValueUnion left, AnyValueUnion right)
        => !left.Equals(right);

    /// <summary>
    /// Determines whether the specified object is equal to the current instance.
    /// </summary>
    /// <param name="obj">The object to compare with the current instance.</param>
    /// <returns><see langword="true"/> if the specified object is of type <c>AnyValueUnion</c> and has the same type and value
    /// as the current instance; otherwise, <see langword="false"/>.</returns>
    public override bool Equals(object? obj)
        => obj is AnyValueUnion other && this.Equals(other);

    /// <summary>
    /// Determines whether the current instance is equal to the specified <see cref="AnyValueUnion"/> instance.
    /// </summary>
    /// <param name="other">The <see cref="AnyValueUnion"/> instance to compare with the current instance.</param>
    /// <returns><see langword="true"/> if the current instance is equal to <paramref name="other"/>; otherwise, <see
    /// langword="false"/>. Equality is determined based on the <see cref="Type"/> and the corresponding
    /// value.</returns>
    public bool Equals(AnyValueUnion other) => this.Type == other.Type && this.Type switch
    {
        AnyValueType.Integer => this.IntValue == other.IntValue,
        AnyValueType.Boolean => this.BoolValue == other.BoolValue,
        AnyValueType.String => string.Equals(this.StringValue, other.StringValue, StringComparison.Ordinal),
        AnyValueType.Double => this.DoubleValue == other.DoubleValue,
        AnyValueType.Array => EqualsArray(this.ArrayValue, other.ArrayValue),
        _ => false,
    };

    /// <summary>
    /// Computes a hash code for the current instance based on its value type and associated data.
    /// </summary>
    /// <returns>An integer representing the hash code of the current instance.</returns>
    public override int GetHashCode() => this.Type switch
    {
        AnyValueType.Integer => this.IntValue.GetHashCode(),
        AnyValueType.Boolean => this.BoolValue.GetHashCode(),
#if NET
        AnyValueType.String => this.StringValue?.GetHashCode(StringComparison.InvariantCulture) ?? 0,
#else
        AnyValueType.String => this.StringValue?.GetHashCode() ?? 0,
#endif
        AnyValueType.Double => this.DoubleValue.GetHashCode(),
        AnyValueType.Array => GetHashCodeArray(this.ArrayValue),
        _ => 0,
    };

    internal static AnyValueUnion From(int value) => new(AnyValueType.Integer, intValue: value);

    internal static AnyValueUnion From(double value) => new(AnyValueType.Double, doubleValue: value);

    internal static AnyValueUnion From(bool value) => new(AnyValueType.Boolean, boolValue: value);

    internal static AnyValueUnion From(string value) => new(AnyValueType.String, stringValue: value);

    internal static AnyValueUnion From(ICollection<int> values) => GetArray(values);

    internal static AnyValueUnion From(ICollection<double> values) => GetArray(values);

    internal static AnyValueUnion From(ICollection<bool> values) => GetArray(values);

    internal static AnyValueUnion From(ICollection<string> values) => GetArray(values);

    private static AnyValueUnion GetArray<T>(ICollection<T> values)
    {
        var array = new AnyValueUnion[values.Count];
        var i = 0;

        foreach (var v in values)
        {
            array[i++] = v switch
            {
                int intValue => From(intValue),
                double doubleValue => From(doubleValue),
                bool boolValue => From(boolValue),
                string stringValue => From(stringValue),
                _ => throw new NotSupportedException($"Type {typeof(T).Name} is not supported."),
            };
        }

        return new AnyValueUnion(AnyValueType.Array, arrayValue: array);
    }

    private static bool EqualsArray(ICollection<AnyValueUnion>? left, ICollection<AnyValueUnion>? right)
    {
        if (ReferenceEquals(left, right))
        {
            return true;
        }

        if (left == null || right == null)
        {
            return false;
        }

        return left.SequenceEqual(right);
    }

#if NET
    private static int GetHashCodeArray(ICollection<AnyValueUnion>? arrayValue)
    {
        var hash = default(HashCode);

        if (arrayValue == null)
        {
            return hash.ToHashCode();
        }

        foreach (var v in arrayValue)
        {
            hash.Add(v);
        }

        return hash.ToHashCode();
    }
#else
    private static int GetHashCodeArray(ICollection<AnyValueUnion>? arrayValue)
    {
        var hash = 0;

        if (arrayValue == null)
        {
            return hash;
        }

        // Follows the same logic as Google.Protobuf.Collections.RepeatedField
        foreach (var v in arrayValue)
        {
            hash = (hash * 31) + v.GetHashCode();
        }

        return hash;
    }
#endif
}
