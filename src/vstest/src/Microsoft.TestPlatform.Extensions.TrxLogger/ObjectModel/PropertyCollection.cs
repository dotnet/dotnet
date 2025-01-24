// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;

using Microsoft.TestPlatform.Extensions.TrxLogger.Utility;
using Microsoft.VisualStudio.TestPlatform.Extensions.TrxLogger;

namespace Microsoft.TestPlatform.Extensions.TrxLogger.ObjectModel;

/// <summary>
/// A collection of strings which categorize the test.
/// </summary>
internal sealed class PropertyCollection : EqtBaseCollection<Property>
{
    /// <summary>
    /// Creates an empty PropertyCollection.
    /// </summary>
    public PropertyCollection()
    {
    }

    /// <summary>
    /// Adds the property.
    /// </summary>
    /// <param name="key">Key to be added.</param>
    /// <param name="value">Value to be added.</param>
    public void Add(string key, string value)
    {
        Add(new Property(key, value));
    }

    /// <summary>
    /// Adds the property.
    /// </summary>
    /// <param name="item">Property to be added.</param>
    public override void Add(Property item)
    {
        EqtAssert.ParameterNotNull(item, nameof(item));

        // Don't add empty items.
        if (!item.Trait.IsNullOrEmpty())
        {
            base.Add(item);
        }
    }

    /// <summary>
    /// Convert the PropertyCollection to a string.
    /// each item is surrounded by a comma (,)
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        StringBuilder returnString = new();
        if (Count > 0)
        {
            returnString.Append(',');
            foreach (Property item in this)
            {
                returnString.Append(item.Trait);
                returnString.Append(',');
            }
        }

        return returnString.ToString();
    }

    /// <summary>
    /// Convert the PropertyCollection to an array of strings.
    /// </summary>
    /// <returns>Array of strings containing the test categories.</returns>
    public string[] ToArray()
    {
        string[] result = new string[Count];

        int i = 0;
        foreach (Property item in this)
        {
            result[i++] = item.Trait;
        }

        return result;
    }

    /// <summary>
    /// Compare the collection items
    /// </summary>
    /// <param name="obj">other collection</param>
    /// <returns>true if the collections contain the same items</returns>
    public override bool Equals(object? obj)
    {
        bool result = false;

        if (obj is not PropertyCollection other)
        {
            // Other object is not a TraitItemCollection.
            result = false;
        }
        else if (ReferenceEquals(this, other))
        {
            // The other object is the same object as this one.
            result = true;
        }
        else if (Count != other.Count)
        {
            // The count of categories in the other object does not
            // match this one, so they are not equal.
            result = false;
        }
        else
        {
            // Check each item and return on the first mismatch.
            foreach (Property item in this)
            {
                if (!other.Contains(item))
                {
                    result = false;
                    break;
                }
            }
        }

        return result;
    }

    /// <summary>
    /// Return the hash code of this collection
    /// </summary>
    /// <returns>The hashcode.</returns>
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
