// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections;
using System.Linq;
using PropertyBag = Microsoft.VisualStudio.SolutionPersistence.Utilities.Lictionary<string, string>;

namespace Microsoft.VisualStudio.SolutionPersistence.Model;

/// <summary>
/// Used by Visual Studio's extensibility to determine when to notify the extensibility extensions for a property bag.
/// </summary>
public enum PropertiesScope : byte
{
    /// <summary>
    /// In Visual Studio the extensibility extensions for these properties are loaded before the solution/project is loaded.
    /// </summary>
    PreLoad,

    /// <summary>
    /// In Visual Studio the extensibility extensions for these properties are loaded after the solution/project is loaded.
    /// </summary>
    PostLoad,
}

/// <summary>
/// Represents a dictionary of property names and values that are associated
/// with a solution, project, or solution folder.
/// </summary>
public sealed class SolutionPropertyBag : IReadOnlyDictionary<string, string>
{
    private List<string> propertyNamesInOrder;
    private PropertyBag properties;

    /// <summary>
    /// Initializes a new instance of the <see cref="SolutionPropertyBag"/> class.
    /// </summary>
    /// <param name="id">The property bag id.</param>
    /// <param name="scope">The scope to create a new property bag with.</param>
    public SolutionPropertyBag(string id, PropertiesScope scope = PropertiesScope.PreLoad)
        : this(id, scope, capacity: 0)
    {
    }

    internal SolutionPropertyBag(string id, PropertiesScope scope, int capacity)
    {
        this.Id = id;
        this.Scope = scope;
        this.propertyNamesInOrder = new List<string>(capacity);
        this.properties = new PropertyBag(capacity);
    }

    // Copy constructor.
    internal SolutionPropertyBag(SolutionPropertyBag propertyBag)
    {
        Argument.ThrowIfNull(propertyBag, nameof(propertyBag));
        this.Id = propertyBag.Id;
        this.Scope = propertyBag.Scope;
        this.propertyNamesInOrder = new List<string>(propertyBag.propertyNamesInOrder);
        this.properties = new PropertyBag(propertyBag.properties);
    }

    /// <summary>
    /// Gets the unique name of the property bag.
    /// </summary>
    public string Id { get; }

    /// <summary>
    /// Gets the scope of the property bag.
    /// </summary>
    public PropertiesScope Scope { get; }

    /// <inheritdoc/>
    public int Count => this.propertyNamesInOrder.Count;

    /// <summary>
    /// Gets a list of property names in the order they were declared.
    /// </summary>
    public IReadOnlyList<string> PropertyNames => this.propertyNamesInOrder;

    /// <inheritdoc/>
    public IEnumerable<string> Keys => this.PropertyNames;

    /// <inheritdoc/>
    public IEnumerable<string> Values => this.PropertyNames.Select(x => this[x]);

    /// <inheritdoc/>
    public string this[string key] => this.properties[key];

    /// <inheritdoc/>
#if NETFRAMEWORK
#nullable disable warnings
#endif
    public bool TryGetValue(string key, [MaybeNullWhen(false)] out string value) =>
#if NETFRAMEWORK
#nullable restore
#endif
        this.properties.TryGetValue(key, out value);

    /// <inheritdoc/>
    public bool ContainsKey(string key)
    {
        return this.properties.ContainsKey(key);
    }

    /// <inheritdoc/>
    public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
    {
        foreach (string key in this.propertyNamesInOrder)
        {
            yield return new KeyValuePair<string, string>(key, this.properties[key]);
        }
    }

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }

    /// <summary>
    /// Adds a property to the property bag.
    /// </summary>
    /// <param name="name">The property name.</param>
    /// <param name="value">The property value.</param>
    public void Add(string name, string value)
    {
        Argument.ThrowIfNullOrEmpty(name, nameof(name));
        Argument.ThrowIfNull(value, nameof(value));

        if (this.properties.TryAdd(name, value))
        {
            this.propertyNamesInOrder.Add(name);
        }
        else
        {
            this.properties[name] = value;
        }
    }

    /// <summary>
    /// Adds multiple properties to the property bag.
    /// </summary>
    /// <param name="properties">The properties to add.</param>
    public void AddRange(IReadOnlyCollection<KeyValuePair<string, string>> properties)
    {
        Argument.ThrowIfNull(properties, nameof(properties));

        if (this.properties.Count == 0)
        {
            this.properties = new PropertyBag(properties);
            this.propertyNamesInOrder = properties.ToList(x => x.Key);
            return;
        }
        else
        {
            this.properties.EnsureCapacity(this.properties.Count + properties.Count);
            foreach ((string key, string value) in properties)
            {
                this.Add(key, value);
            }
        }
    }

    /// <summary>
    /// Removes a property.
    /// </summary>
    /// <param name="name">The name of the property to remove.</param>
    /// <returns><see langword="true"/> if the property was found and removed.</returns>
    public bool Remove(string name)
    {
        if (this.properties.Remove(name))
        {
            _ = this.propertyNamesInOrder.Remove(name);
            return true;
        }

        return false;
    }
}
