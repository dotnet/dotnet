// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using OpenTelemetry.OpAmp.Client.Internal.Settings;
using OpenTelemetry.OpAmp.Client.Internal.Utils;
using OpenTelemetry.OpAmp.Client.Settings;
#if NETFRAMEWORK
using OpenTelemetry.OpAmp.Client.Tests.Tools;
#endif
using Xunit;

namespace OpenTelemetry.OpAmp.Client.Tests;

public class AnyValueUnionTests
{
    [Fact]
    public void AnyValueUnion_EqualityTests()
    {
        var intValue = new AnyValueUnion(AnyValueType.Integer, intValue: 42);
        var intValue2 = new AnyValueUnion(AnyValueType.Integer, intValue: 42);
        var boolValue = new AnyValueUnion(AnyValueType.Boolean, boolValue: true);
        var boolValue2 = new AnyValueUnion(AnyValueType.Boolean, boolValue: true);
        var stringValue = new AnyValueUnion(AnyValueType.String, stringValue: "test");
        var stringValue2 = new AnyValueUnion(AnyValueType.String, stringValue: "test");
        var doubleValue = new AnyValueUnion(AnyValueType.Double, doubleValue: 3.14);
        var doubleValue2 = new AnyValueUnion(AnyValueType.Double, doubleValue: 3.14);

        Assert.Equal(intValue, intValue2);
        Assert.Equal(boolValue, boolValue2);
        Assert.Equal(stringValue, stringValue2);
        Assert.Equal(doubleValue, doubleValue2);

        Assert.NotEqual(intValue, boolValue);
        Assert.NotEqual(intValue, stringValue);
        Assert.NotEqual(intValue, doubleValue);
        Assert.NotEqual(boolValue, stringValue);
        Assert.NotEqual(boolValue, doubleValue);
        Assert.NotEqual(stringValue, doubleValue);

        // Explicit equality tests
        Assert.True(intValue.Equals(intValue));
        Assert.True(intValue.Equals(intValue2));
        Assert.False(intValue.Equals(boolValue));
        Assert.True(intValue.Equals((object)intValue));
        Assert.True(intValue.Equals((object)intValue2));
        Assert.False(intValue.Equals((object)boolValue));
        Assert.False(intValue.Equals((object)42));
#pragma warning disable CS1718 // Comparison made to same variable
        Assert.True(intValue == intValue);
#pragma warning restore CS1718 // Comparison made to same variable
        Assert.True(intValue == intValue2);
        Assert.True(intValue != doubleValue);
    }

    [Fact]
    public void AnyValueUnion_EqualityTests_NullTests()
    {
        var enumValues
#if NET
            = Enum.GetValues<AnyValueType>();
#else
            = EnumPolyfill.GetValues<AnyValueType>();
#endif

        foreach (var type in enumValues)
        {
            Assert.Throws<ArgumentNullException>(() => new AnyValueUnion(type));
        }
    }

    [Fact]
    public void AnyValueUnion_EqualityTests_UncoveredValues()
    {
        var intValue = new AnyValueUnion(AnyValueType.Integer, intValue: 42);
        var uncoveredValue = new AnyValueUnion((AnyValueType)int.MinValue, intValue: 42);

        Assert.False(intValue.Equals(uncoveredValue));
        Assert.False(uncoveredValue.Equals(uncoveredValue));
    }

    [Fact]
    public void AnyValueUnion_HashCodeTests()
    {
        var intValue = new AnyValueUnion(AnyValueType.Integer, intValue: 42);
        var boolValue = new AnyValueUnion(AnyValueType.Boolean, boolValue: true);
        var stringValue = new AnyValueUnion(AnyValueType.String, stringValue: "test");
        var doubleValue = new AnyValueUnion(AnyValueType.Double, doubleValue: 3.14);

        Assert.Equal(intValue.GetHashCode(), new AnyValueUnion(AnyValueType.Integer, intValue: 42).GetHashCode());
        Assert.Equal(boolValue.GetHashCode(), new AnyValueUnion(AnyValueType.Boolean, boolValue: true).GetHashCode());
        Assert.Equal(stringValue.GetHashCode(), new AnyValueUnion(AnyValueType.String, stringValue: "test").GetHashCode());
        Assert.Equal(doubleValue.GetHashCode(), new AnyValueUnion(AnyValueType.Double, doubleValue: 3.14).GetHashCode());

        Assert.NotEqual(intValue.GetHashCode(), boolValue.GetHashCode());
        Assert.NotEqual(intValue.GetHashCode(), stringValue.GetHashCode());
        Assert.NotEqual(intValue.GetHashCode(), doubleValue.GetHashCode());
        Assert.NotEqual(boolValue.GetHashCode(), stringValue.GetHashCode());
        Assert.NotEqual(boolValue.GetHashCode(), doubleValue.GetHashCode());
        Assert.NotEqual(stringValue.GetHashCode(), doubleValue.GetHashCode());
    }

    [Fact]
    public void AnyValueUnion_HashCodeTests_UncoveredValues()
    {
        var uncoveredValue = new AnyValueUnion((AnyValueType)int.MinValue, intValue: 42);

        Assert.Equal(0, uncoveredValue.GetHashCode());
    }

    [Theory]
    [InlineData(AnyValueType.Integer, 10, null, null, null)]
    [InlineData(AnyValueType.Boolean, null, true, null, null)]
    [InlineData(AnyValueType.String, null, null, "hello", null)]
    [InlineData(AnyValueType.Double, null, null, null, 3.14)]
    internal void AnyValueUnion_Extensions_ProtoMapTests(
        AnyValueType type,
        int? intValue,
        bool? boolValue,
        string? stringValue,
        double? doubleValue)
    {
        // Arrange
        var anyValueUnion = new AnyValueUnion(type, intValue, boolValue, stringValue, doubleValue);

        // Act
        var protoValue = anyValueUnion.ToAnyValue();

        // Assert
        switch (type)
        {
            case AnyValueType.Integer:
                Assert.Equal((long?)intValue, protoValue.IntValue);
                break;
            case AnyValueType.Boolean:
                Assert.Equal(boolValue, protoValue.BoolValue);
                break;
            case AnyValueType.String:
                Assert.Equal(stringValue, protoValue.StringValue);
                break;
            case AnyValueType.Double:
                Assert.Equal(doubleValue, protoValue.DoubleValue);
                break;
            default:
                Assert.Fail($"Unhandled type {type}");
                break;
        }
    }

    [Fact]
    internal void AnyValueUnion_Extensions_ProtoMapTests_UncoveredTests()
    {
        var uncoveredValue = new AnyValueUnion((AnyValueType)int.MinValue, intValue: 42);

        Assert.Throws<NotSupportedException>(() => uncoveredValue.ToAnyValue());
    }
}
