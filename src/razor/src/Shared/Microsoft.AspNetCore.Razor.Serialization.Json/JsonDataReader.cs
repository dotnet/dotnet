// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.Extensions.ObjectPool;
using Newtonsoft.Json;
using SR = Microsoft.AspNetCore.Razor.Serialization.Json.Internal.Strings;

namespace Microsoft.AspNetCore.Razor.Serialization.Json;

internal delegate T ReadValue<T>(JsonDataReader reader);
internal delegate T ReadProperties<T>(JsonDataReader reader);

/// <summary>
///  This is an abstraction used to read JSON data. Currently, this
///  wraps a <see cref="JsonReader"/> from JSON.NET.
/// </summary>
internal partial class JsonDataReader
{
    private static readonly ObjectPool<JsonDataReader> s_pool = DefaultPool.Create(Policy.Instance);

    public static JsonDataReader Get(JsonReader reader)
    {
        var dataReader = s_pool.Get();
        dataReader._reader = reader;

        return dataReader;
    }

    public static void Return(JsonDataReader dataReader)
        => s_pool.Return(dataReader);

    [AllowNull]
    private JsonReader _reader;

    private JsonDataReader()
    {
    }

    public bool IsObjectStart => _reader.TokenType == JsonToken.StartObject;

    public bool IsPropertyName(string propertyName)
        => _reader.TokenType == JsonToken.PropertyName &&
           (string?)_reader.Value == propertyName;

    public void ReadPropertyName(string propertyName)
    {
        if (!IsPropertyName(propertyName))
        {
            ThrowUnexpectedPropertyException(propertyName, (string?)_reader.Value);
        }

        _reader.Read();

        [DoesNotReturn]
        static void ThrowUnexpectedPropertyException(string expectedPropertyName, string? actualPropertyName)
        {
            throw new InvalidOperationException(
                SR.FormatExpected_JSON_property_0_but_it_was_1(expectedPropertyName, actualPropertyName));
        }
    }

    public bool TryReadPropertyName(string propertyName)
    {
        if (IsPropertyName(propertyName))
        {
            _reader.Read();
            return true;
        }

        return false;
    }

    public bool TryReadNextPropertyName([NotNullWhen(true)] out string? propertyName)
    {
        if (_reader.TokenType != JsonToken.PropertyName)
        {
            propertyName = null;
            return false;
        }

        propertyName = (string)_reader.Value.AssumeNotNull();
        _reader.Read();

        return true;
    }

    public bool TryReadNull()
    {
        if (_reader.TokenType == JsonToken.Null)
        {
            _reader.Read();
            return true;
        }

        return false;
    }

    public bool ReadBoolean()
    {
        _reader.CheckToken(JsonToken.Boolean);

        var result = Convert.ToBoolean(_reader.Value);
        _reader.Read();

        return result;
    }

    public bool ReadBoolean(string propertyName)
    {
        ReadPropertyName(propertyName);

        return ReadBoolean();
    }

    public bool ReadBooleanOrDefault(string propertyName, bool defaultValue = default)
        => TryReadPropertyName(propertyName) ? ReadBoolean() : defaultValue;

    public bool ReadBooleanOrTrue(string propertyName)
        => !TryReadPropertyName(propertyName) || ReadBoolean();

    public bool ReadBooleanOrFalse(string propertyName)
        => TryReadPropertyName(propertyName) && ReadBoolean();

    public bool TryReadBoolean(string propertyName, out bool value)
    {
        if (TryReadPropertyName(propertyName))
        {
            value = ReadBoolean();
            return true;
        }

        value = default;
        return false;
    }

    public int ReadInt32()
    {
        _reader.CheckToken(JsonToken.Integer);

        var result = Convert.ToInt32(_reader.Value);
        _reader.Read();

        return result;
    }

    public int ReadInt32OrDefault(string propertyName, int defaultValue = default)
        => TryReadPropertyName(propertyName) ? ReadInt32() : defaultValue;

    public int ReadInt32OrZero(string propertyName)
        => TryReadPropertyName(propertyName) ? ReadInt32() : 0;

    public bool TryReadInt32(string propertyName, out int value)
    {
        if (TryReadPropertyName(propertyName))
        {
            value = ReadInt32();
            return true;
        }

        value = default;
        return false;
    }

    public int ReadInt32(string propertyName)
    {
        ReadPropertyName(propertyName);

        return ReadInt32();
    }

    public long ReadInt64()
    {
        _reader.CheckToken(JsonToken.Integer);

        var result = Convert.ToInt64(_reader.Value);
        _reader.Read();

        return result;
    }

    public long ReadInt64OrDefault(string propertyName, int defaultValue = default)
        => TryReadPropertyName(propertyName) ? ReadInt64() : defaultValue;

    public long ReadInt64OrZero(string propertyName)
        => TryReadPropertyName(propertyName) ? ReadInt64() : 0;

    public bool TryReadInt64(string propertyName, out long value)
    {
        if (TryReadPropertyName(propertyName))
        {
            value = ReadInt64();
            return true;
        }

        value = default;
        return false;
    }

    public long ReadInt64(string propertyName)
    {
        ReadPropertyName(propertyName);

        return ReadInt64();
    }

    public string? ReadString()
    {
        if (TryReadNull())
        {
            return null;
        }

        _reader.CheckToken(JsonToken.String);

        var result = Convert.ToString(_reader.Value);
        _reader.Read();

        return result;
    }

    public string? ReadString(string propertyName)
    {
        ReadPropertyName(propertyName);

        return ReadString();
    }

    public string? ReadStringOrDefault(string propertyName, string? defaultValue = default)
        => TryReadPropertyName(propertyName) ? ReadString() : defaultValue;

    public string? ReadStringOrNull(string propertyName)
        => TryReadPropertyName(propertyName) ? ReadString() : null;

    public bool TryReadString(string propertyName, out string? value)
    {
        if (TryReadPropertyName(propertyName))
        {
            value = ReadString();
            return true;
        }

        value = null;
        return false;
    }

    public string ReadNonNullString()
    {
        _reader.CheckToken(JsonToken.String);

        var result = Convert.ToString(_reader.Value).AssumeNotNull();
        _reader.Read();

        return result;
    }

    public string ReadNonNullString(string propertyName)
    {
        ReadPropertyName(propertyName);

        return ReadNonNullString();
    }

    public object? ReadValue()
    {
        return _reader.TokenType switch
        {
            JsonToken.String => ReadString(),
            JsonToken.Integer => ReadInt32(),
            JsonToken.Boolean => ReadBoolean(),

            var token => ThrowNotSupported(token)
        };

        [DoesNotReturn]
        static object? ThrowNotSupported(JsonToken token)
        {
            throw new NotSupportedException(
                SR.FormatCould_not_read_value_JSON_token_was_0(token));
        }
    }

    public Uri? ReadUri(string propertyName)
    {
        ReadPropertyName(propertyName);

        return ReadUri();
    }

    public Uri? ReadUri()
    {
        return ReadString() is string uriString
            ? new Uri(uriString)
            : null;
    }

    public Uri ReadNonNullUri(string propertyName)
    {
        ReadPropertyName(propertyName);

        return ReadNonNullUri();
    }

    public Uri ReadNonNullUri()
    {
        var uriString = ReadNonNullString();
        return new Uri(uriString);
    }

    [return: MaybeNull]
    public T ReadObject<T>(ReadProperties<T> readProperties)
    {
        if (TryReadNull())
        {
            return default;
        }

        return ReadNonNullObject(readProperties);
    }

    [return: MaybeNull]
    public T ReadObject<T>(string propertyName, ReadProperties<T> readProperties)
    {
        ReadPropertyName(propertyName);

        return ReadObject(readProperties);
    }

    [return: MaybeNull]
    public T ReadObjectOrDefault<T>(string propertyName, ReadProperties<T> readProperties, T defaultValue)
        => TryReadPropertyName(propertyName) ? ReadObject(readProperties) : defaultValue;

    public T? ReadObjectOrNull<T>(string propertyName, ReadProperties<T> readProperties)
        where T : class
        => ReadObjectOrDefault(propertyName, readProperties!, defaultValue: null);

    public T ReadNonNullObject<T>(ReadProperties<T> readProperties)
    {
        _reader.ReadToken(JsonToken.StartObject);
        var result = readProperties(this);
        _reader.ReadToken(JsonToken.EndObject);

        return result;
    }

    public T ReadNonNullObject<T>(string propertyName, ReadProperties<T> readProperties)
    {
        ReadPropertyName(propertyName);

        return ReadNonNullObject(readProperties);
    }

    public T[]? ReadArray<T>(ReadValue<T> readElement)
    {
        if (TryReadNull())
        {
            return null;
        }

        _reader.ReadToken(JsonToken.StartArray);

        // First special case, is this an empty array?
        if (_reader.TokenType == JsonToken.EndArray)
        {
            _reader.Read();
            return [];
        }

        // Second special case, is this an array of one element?
        var firstElement = readElement(this);

        if (_reader.TokenType == JsonToken.EndArray)
        {
            _reader.Read();
            return [firstElement];
        }

        // There's more than one element, so we need to acquire a pooled list to
        // read the rest of the array elements.
        using var _ = ListPool<T>.GetPooledObject(out var elements);

        // Be sure to add the element that we already read.
        elements.Add(firstElement);

        do
        {
            var element = readElement(this);
            elements.Add(element);
        }
        while (_reader.TokenType != JsonToken.EndArray);

        _reader.Read();

        return [.. elements];
    }

    public T[]? ReadArray<T>(string propertyName, ReadValue<T> readElement)
    {
        ReadPropertyName(propertyName);
        return ReadArray(readElement);
    }

    public T[] ReadArrayOrEmpty<T>(string propertyName, ReadValue<T> readElement)
        => TryReadPropertyName(propertyName) ? ReadArray(readElement) ?? [] : [];

    public ImmutableArray<T> ReadImmutableArray<T>(ReadValue<T> readElement)
    {
        _reader.ReadToken(JsonToken.StartArray);

        // First special case, is this an empty array?
        if (_reader.TokenType == JsonToken.EndArray)
        {
            _reader.Read();
            return ImmutableArray<T>.Empty;
        }

        // Second special case, is this an array of one element?
        var firstElement = readElement(this);

        if (_reader.TokenType == JsonToken.EndArray)
        {
            _reader.Read();
            return ImmutableArray.Create(firstElement);
        }

        // There's more than one element, so we need to acquire a pooled list to
        // read the rest of the array elements.
        using var _ = ArrayBuilderPool<T>.GetPooledObject(out var elements);

        // Be sure to add the element that we already read.
        elements.Add(firstElement);

        do
        {
            var element = readElement(this);
            elements.Add(element);
        }
        while (_reader.TokenType != JsonToken.EndArray);

        _reader.Read();

        return elements.ToImmutable();
    }

    public ImmutableArray<T> ReadImmutableArray<T>(string propertyName, ReadValue<T> readElement)
    {
        ReadPropertyName(propertyName);
        return ReadImmutableArray(readElement);
    }

    public ImmutableArray<T> ReadImmutableArrayOrEmpty<T>(string propertyName, ReadValue<T> readElement)
        => TryReadPropertyName(propertyName) ? ReadImmutableArray(readElement) : ImmutableArray<T>.Empty;

    public void ReadToEndOfCurrentObject()
    {
        var nestingLevel = 0;

        while (_reader.Read())
        {
            switch (_reader.TokenType)
            {
                case JsonToken.StartObject:
                    nestingLevel++;
                    break;

                case JsonToken.EndObject:
                    nestingLevel--;

                    if (nestingLevel == -1)
                    {
                        return;
                    }

                    break;
            }
        }

        throw new JsonSerializationException(SR.Encountered_end_of_stream_before_end_of_object);
    }
}
