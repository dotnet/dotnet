// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel;

internal class CustomStringArrayConverter : TypeConverter
{
    private readonly DataContractJsonSerializer _serializer = new(typeof(string[]));

    /// <inheritdoc/>
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
    {
        return sourceType == typeof(string);
    }

    /// <inheritdoc/>
    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
    {
        return value is string[] strings ? strings : base.ConvertTo(context, culture, value, destinationType);
    }

    /// <inheritdoc/>
    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object? value)
    {
        // PERF: The strings returned here can possibly be cached, but the benefits are not huge speed wise,
        // and it is unclear how many distinct strings we get, and how much memory this would consume. I was seeing around 200ms improvement on 10k tests.

        // String[] are used by adapters. E.g. TestCategory[]
        if (value is string data)
        {
            using var stream = new MemoryStream(Encoding.Unicode.GetBytes(data));
            var strings = _serializer.ReadObject(stream) as string[];
            return strings;
        }

        return null;
    }
}
