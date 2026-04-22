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
    internal AnyValueUnion(AnyValueType type, int? intValue = null, bool? boolValue = null, string? stringValue = null, double? doubleValue = null)
    {
        this.Type = type;
        this.IntValue = intValue;
        this.BoolValue = boolValue;
        this.StringValue = stringValue;
        this.DoubleValue = doubleValue;

        // No value defined
        if (intValue == null && boolValue == null && stringValue == null && doubleValue == null)
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
    {
        return left.Equals(right);
    }

    /// <summary>
    /// Determines whether two <see cref="AnyValueUnion"/> instances are not equal.
    /// </summary>
    /// <param name="left">The first <see cref="AnyValueUnion"/> instance to compare.</param>
    /// <param name="right">The second <see cref="AnyValueUnion"/> instance to compare.</param>
    /// <returns><see langword="true"/> if the two instances are not equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator !=(AnyValueUnion left, AnyValueUnion right)
    {
        return !left.Equals(right);
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current instance.
    /// </summary>
    /// <param name="obj">The object to compare with the current instance.</param>
    /// <returns><see langword="true"/> if the specified object is of type <c>AnyValueUnion</c> and has the same type and value
    /// as the current instance; otherwise, <see langword="false"/>.</returns>
    public override bool Equals(object? obj)
    {
        if (obj is AnyValueUnion other)
        {
            return this.Equals(other);
        }

        return false;
    }

    /// <summary>
    /// Determines whether the current instance is equal to the specified <see cref="AnyValueUnion"/> instance.
    /// </summary>
    /// <param name="other">The <see cref="AnyValueUnion"/> instance to compare with the current instance.</param>
    /// <returns><see langword="true"/> if the current instance is equal to <paramref name="other"/>; otherwise, <see
    /// langword="false"/>. Equality is determined based on the <see cref="Type"/> and the corresponding
    /// value.</returns>
    public bool Equals(AnyValueUnion other)
    {
        if (this.Type != other.Type)
        {
            return false;
        }

        return this.Type switch
        {
            AnyValueType.Integer => this.IntValue == other.IntValue,
            AnyValueType.Boolean => this.BoolValue == other.BoolValue,
            AnyValueType.String => string.Equals(this.StringValue, other.StringValue, StringComparison.Ordinal),
            AnyValueType.Double => this.DoubleValue == other.DoubleValue,
            _ => false,
        };
    }

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
        _ => 0,
    };

    internal static AnyValueUnion From(int value) => new(AnyValueType.Integer, intValue: value);

    internal static AnyValueUnion From(double value) => new(AnyValueType.Double, doubleValue: value);

    internal static AnyValueUnion From(bool value) => new(AnyValueType.Boolean, boolValue: value);

    internal static AnyValueUnion From(string value) => new(AnyValueType.String, stringValue: value);
}
