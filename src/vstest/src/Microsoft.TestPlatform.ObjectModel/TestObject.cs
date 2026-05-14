// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;

using Microsoft.VisualStudio.TestPlatform.CoreUtilities;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel;

/// <summary>
///  Base class for test related classes.
/// </summary>
[DataContract]
public abstract class TestObject
{
    private static readonly CustomKeyValueConverter KeyValueConverter = new();
    private static readonly CustomStringArrayConverter StringArrayConverter = new();

    /// <summary>
    /// The store for all the properties registered.
    /// </summary>
    private readonly ConcurrentDictionary<TestProperty, object?> _store = new();

    /// <summary>
    /// Property used for Json (de)serialization of store dictionary. Serialization of dictionaries
    /// by default doesn't provide the required object representation. <c>List of KeyValuePair</c> on the
    /// other hand provides a clean Key, Value entries for <c>TestProperty</c> and it's value.
    /// </summary>
    [DataMember(Name = "Properties")]
    private List<KeyValuePair<TestProperty, object?>> StoreKeyValuePairs
    {
        get => _store.ToList();

        set
        {
            // Receive the <TestProperty, String> key value pairs from deserialized entity.
            // Store each property and value in the property data store.
            foreach (var property in value)
            {
                TestProperty.Register(
                    property.Key.Id,
                    property.Key.Label,
                    property.Key.Category,
                    property.Key.Description,
                    property.Key.GetValueType(),
                    null,
                    property.Key.Attributes,
                    typeof(TestObject));

                // Do not call SetPropertyValue(TestProperty property, object value) as it does not
                // invoke ConvertPropertyFrom and does not store the properties in correct types.
                SetPropertyValue(property.Key, property.Value, CultureInfo.InvariantCulture);
            }
        }
    }

    public IEnumerable<KeyValuePair<TestProperty, object?>> GetProperties()
    {
        return _store;
    }

    [OnSerializing]
#if FullCLR
        private void CacheLazyValuesOnSerializing(StreamingContext context)
#else
    public void CacheLazyValuesOnSerializing(StreamingContext context)
#endif
    {
        var lazyValues = _store.Where(kvp => kvp.Value is ILazyPropertyValue).ToArray();

        foreach (var kvp in lazyValues)
        {
            var lazyValue = (ILazyPropertyValue?)kvp.Value;
            var value = lazyValue?.Value;
            _store.TryRemove(kvp.Key, out _);

            if (value != null)
            {
                _store.TryAdd(kvp.Key, value);
            }
        }
    }

    /// <summary>
    /// Returns the TestProperties currently specified in this TestObject.
    /// </summary>
    public virtual IEnumerable<TestProperty> Properties
    {
        get { return _store.Keys; }
    }

    /// <summary>
    /// Returns property value of the specify TestProperty
    /// </summary>
    /// <param name="property">TestObject's TestProperty</param>
    /// <returns>property value</returns>
    public object? GetPropertyValue(TestProperty property)
    {
        ValidateArg.NotNull(property, nameof(property));
        object? defaultValue = null;
        var valueType = property.GetValueType();

        if (valueType != null && valueType.IsValueType)
        {
            defaultValue = Activator.CreateInstance(valueType);
        }

        return ProtectedGetPropertyValue(property, defaultValue);
    }

    /// <summary>
    /// Returns property value of the specify TestProperty
    /// </summary>
    /// <typeparam name="T">Property value type</typeparam>
    /// <param name="property">TestObject's TestProperty</param>
    /// <param name="defaultValue">default property value if property is not present</param>
    /// <returns>property value</returns>
    [return: NotNullIfNotNull("defaultValue")]
    public T? GetPropertyValue<T>(TestProperty property, T? defaultValue)
    {
        return GetPropertyValue(property, defaultValue, CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Set TestProperty's value
    /// </summary>
    /// <typeparam name="T">Property value type</typeparam>
    /// <param name="property">TestObject's TestProperty</param>
    /// <param name="value">value to be set</param>
    public void SetPropertyValue<T>(TestProperty property, T value)
    {
        SetPropertyValue(property, value, CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Set TestProperty's value
    /// </summary>
    /// <typeparam name="T">Property value type</typeparam>
    /// <param name="property">TestObject's TestProperty</param>
    /// <param name="value">value to be set</param>
    public void SetPropertyValue<T>(TestProperty property, LazyPropertyValue<T> value)
    {
        SetPropertyValue(property, value, CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Set TestProperty's value
    /// </summary>
    /// <param name="property">TestObject's TestProperty</param>
    /// <param name="value">value to be set</param>
    public void SetPropertyValue(TestProperty property, object? value)
    {
        ProtectedSetPropertyValue(property, value);
    }

    /// <summary>
    ///  Remove test property from the current TestObject.
    /// </summary>
    /// <param name="property"></param>
    public void RemovePropertyValue(TestProperty property)
    {
        ValidateArg.NotNull(property, nameof(property));
        _store.TryRemove(property, out _);
    }

    /// <summary>
    /// Returns TestProperty's value
    /// </summary>
    /// <returns>property's value. default value is returned if the property is not present</returns>
    [return: NotNullIfNotNull("defaultValue")]
    public T? GetPropertyValue<T>(TestProperty property, T? defaultValue, CultureInfo culture)
    {
        ValidateArg.NotNull(property, nameof(property));
        ValidateArg.NotNull(culture, nameof(culture));
        object? objValue = ProtectedGetPropertyValue(property, defaultValue);

        return ConvertPropertyTo<T>(property, culture, objValue);
    }

    /// <summary>
    /// Set TestProperty's value to the specified value T.
    /// </summary>
    public void SetPropertyValue<T>(TestProperty property, T value, CultureInfo culture)
    {
        ValidateArg.NotNull(property, nameof(property));
        ValidateArg.NotNull(culture, nameof(culture));
        object? objValue = ConvertPropertyFrom<T>(property, culture, value);

        ProtectedSetPropertyValue(property, objValue);
    }

    /// <summary>
    /// Set TestProperty's value to the specified value T.
    /// </summary>
    public void SetPropertyValue<T>(TestProperty property, LazyPropertyValue<T> value, CultureInfo culture)
    {
        ValidateArg.NotNull(property, nameof(property));
        ValidateArg.NotNull(culture, nameof(culture));
        object? objValue = ConvertPropertyFrom<T>(property, culture, value);

        ProtectedSetPropertyValue(property, objValue);
    }

    /// <summary>
    /// Return TestProperty's value
    /// </summary>
    /// <returns></returns>
    [return: NotNullIfNotNull("defaultValue")]
    protected virtual object? ProtectedGetPropertyValue(TestProperty property, object? defaultValue)
    {
        ValidateArg.NotNull(property, nameof(property));
        if (!_store.TryGetValue(property, out var value) || value == null)
        {
            value = defaultValue;
        }

        return value;
    }

    /// <summary>
    /// Set TestProperty's value
    /// </summary>
    protected virtual void ProtectedSetPropertyValue(TestProperty property, object? value)
    {
        ValidateArg.NotNull(property, nameof(property));
        _store[property] = property.ValidateValueCallback == null || property.ValidateValueCallback(value)
            ? value
            : throw new ArgumentException(property.Label);
    }

    /// <summary>
    /// Convert passed in value from TestProperty's specified value type.
    /// </summary>
    /// <returns>Converted object</returns>
    private static object? ConvertPropertyFrom<T>(TestProperty property, CultureInfo culture, object? value)
    {
        ValidateArg.NotNull(property, nameof(property));
        ValidateArg.NotNull(culture, nameof(culture));
        var valueType = property.GetValueType();

        // Do not try conversion if the object is already of the type we're trying to convert.
        // Note that typeof(T) may be object in case the value is getting deserialized via the StoreKvpList, however
        // the de-serializer could have converted it already, hence the runtime type check.
        if (valueType != null && (valueType.IsAssignableFrom(typeof(T)) || valueType.IsAssignableFrom(value?.GetType())))
        {
            return value;
        }

        // Traits are KeyValuePair based. Use the custom converter in that case.
        if (valueType == typeof(KeyValuePair<string, string>[]))
        {
            return KeyValueConverter.ConvertFrom(null, culture, (string?)value);
        }

        // Use a custom string array converter for string[] types.
        if (valueType == typeof(string[]))
        {
            return StringArrayConverter.ConvertFrom(null, culture, (string?)value);
        }

        TPDebug.Assert(valueType is not null, "valueType is null");
        TypeConverter converter = TypeDescriptor.GetConverter(valueType);
        if (converter == null)
        {
            throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, Resources.Resources.ConverterNotSupported, valueType.Name));
        }

        try
        {
            return converter.ConvertFrom(null, culture, value!);
        }
        catch (FormatException)
        {
            throw;
        }
        catch (Exception e)
        {
            // some type converters throw strange exceptions (e.g.: System.Exception by Int32Converter)
            throw new FormatException(e.Message, e);
        }
    }

    /// <summary>
    /// Convert passed in value into the specified type when property is registered.
    /// </summary>
    /// <returns>Converted object</returns>
    [return: NotNullIfNotNull("value")]
    private static T? ConvertPropertyTo<T>(TestProperty property, CultureInfo culture, object? value)
    {
        ValidateArg.NotNull(property, nameof(property));
        ValidateArg.NotNull(culture, nameof(culture));

        if (value == null)
        {
            return default;
        }
        else if (value is T t)
        {
            return t;
        }
        else if (value is LazyPropertyValue<T> lazyValue)
        {
            return lazyValue.Value!;
        }

        var valueType = property.GetValueType();

        TypeConverter converter = TypeDescriptor.GetConverter(valueType);

        if (converter == null)
        {
            throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, Resources.Resources.ConverterNotSupported, valueType.Name));
        }

        try
        {
            return (T?)converter.ConvertTo(null, culture, value, typeof(T))!;
        }
        catch (FormatException)
        {
            throw;
        }
        catch (Exception e)
        {
            // some type converters throw strange exceptions (e.g.: System.Exception by Int32Converter)
            throw new FormatException(e.Message, e);
        }
    }

    private TraitCollection? _traits;

    public TraitCollection Traits
    {
        get
        {
            _traits ??= new TraitCollection(this);

            return _traits;
        }
    }
}
