// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel;

/// <summary>
/// Represents a lazy value calculation for a TestObject
/// </summary>
internal interface ILazyPropertyValue
{
    /// <summary>
    /// Forces calculation of the value
    /// </summary>
    object Value { get; }
}

/// <summary>
/// Represents a lazy value calculation for a TestObject
/// </summary>
/// <typeparam name="T">The type of the value to be calculated</typeparam>
public sealed class LazyPropertyValue<T> : ILazyPropertyValue
{
    private readonly Func<T> _getValue;
    private T? _value;
    private bool _isValueCreated;

    public LazyPropertyValue(Func<T> getValue)
    {
        _isValueCreated = false;
        _value = default;
        _getValue = getValue;
    }

    /// <summary>
    /// Forces calculation of the value
    /// </summary>
    public T Value
    {
        get
        {
            if (!_isValueCreated)
            {
                _value = _getValue();
                _isValueCreated = true;
            }

            return _value!;
        }
    }

    object ILazyPropertyValue.Value
    {
        get
        {
            return Value!;
        }
    }
}
