// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Globalization;
using System.Reflection;

namespace PresentationCore.Tests.MS.Internal.Text.TextInterface;

public class LocalizedStringsTests
{
    [Fact]
    public void EmptyDictionary_ReadOnlyViewsShareKeysAndValues()
    {
        Type type = Assembly.Load("DirectWriteForwarder")
            .GetType("MS.Internal.Text.TextInterface.LocalizedStrings", throwOnError: true)!;
        object instance = Activator.CreateInstance(type, nonPublic: true)!;
        IDictionary<CultureInfo, string> dictionary = Assert.IsAssignableFrom<IDictionary<CultureInfo, string>>(instance);
        IReadOnlyDictionary<CultureInfo, string> readOnlyDictionary = Assert.IsAssignableFrom<IReadOnlyDictionary<CultureInfo, string>>(instance);

        Assert.Empty(dictionary);
        Assert.Equal(dictionary.Count, readOnlyDictionary.Count);
        Assert.Empty(readOnlyDictionary);
        Assert.Empty(readOnlyDictionary.Keys);
        Assert.Empty(readOnlyDictionary.Values);
        Assert.Same(dictionary.Keys, readOnlyDictionary.Keys);
        Assert.Same(dictionary.Values, readOnlyDictionary.Values);
    }
}
