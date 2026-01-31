// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel;

/// <summary>
/// Class that holds collection of traits
/// </summary>
[Serializable]
public class TraitCollection : IEnumerable<Trait>
{
    internal const string TraitPropertyId = "TestObject.Traits";
    private static readonly TestProperty TraitsProperty = TestProperty.Register(
        TraitPropertyId,
        // REVIEW ME: We should make some test to see if that's causing any crash on WinUI/UWP
        Resources.Resources.TestCasePropertyTraitsLabel,
        typeof(KeyValuePair<string, string>[]),
#pragma warning disable 618
        TestPropertyAttributes.Hidden | TestPropertyAttributes.Trait,
#pragma warning restore 618
        typeof(TestObject));

    [NonSerialized]
    private readonly TestObject _testObject;

    internal TraitCollection(TestObject testObject)
    {
        _testObject = testObject ?? throw new ArgumentNullException(nameof(testObject));
    }

    public void Add(Trait trait)
    {
        ValidateArg.NotNull(trait, nameof(trait));
        AddRange(new[] { trait });
    }

    public void Add(string name, string value)
    {
        ValidateArg.NotNull(name, nameof(name));
        Add(new Trait(name, value));
    }

    public void AddRange(IEnumerable<Trait> traits)
    {
        ValidateArg.NotNull(traits, nameof(traits));
        var existingTraits = GetTraits();
        Add(existingTraits, traits);
    }

    public IEnumerator<Trait> GetEnumerator()
    {
        var enumerable = GetTraits();
        return enumerable.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private IEnumerable<Trait> GetTraits()
    {
        if (!_testObject.Properties.Contains(TraitsProperty, EqualityComparer<TestProperty>.Default))
        {
            return Array.Empty<Trait>();
        }

        var traitsKvp = _testObject.GetPropertyValue(TraitsProperty, Enumerable.Empty<KeyValuePair<string, string>>().ToArray());
        var traits = new Trait[traitsKvp.Length];
        for (int i = 0; i < traits.Length; i++)
        {
            traits[i] = new Trait(traitsKvp[i]);
        }

        return traits;
    }

    private void Add(IEnumerable<Trait> traits, IEnumerable<Trait> newTraits)
    {
        var newValue = traits.Union(newTraits);
        var newPairs = newValue.Select(t => new KeyValuePair<string, string>(t.Name, t.Value)).ToArray();
        _testObject.SetPropertyValue(TraitsProperty, newPairs);
    }
}
