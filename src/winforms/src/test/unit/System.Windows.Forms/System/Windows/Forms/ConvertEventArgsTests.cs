﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

namespace System.Windows.Forms.Tests;

// NB: doesn't require thread affinity
public class ConvertEventArgsTests
{
    [Theory]
    [InlineData("value", typeof(int))]
    [InlineData(null, null)]
    public void Ctor_Object_Type(object value, Type desiredType)
    {
        ConvertEventArgs e = new(value, desiredType);
        Assert.Equal(value, e.Value);
        Assert.Equal(desiredType, e.DesiredType);
    }

    [Theory]
    [InlineData(null)]
    [InlineData(1)]
    public void Value_Set_GetReturnsExpected(object value)
    {
        ConvertEventArgs e = new("value", typeof(int))
        {
            Value = value
        };
        Assert.Equal(value, e.Value);
    }
}
