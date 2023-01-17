// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Globalization;
using System.Runtime.Serialization;
using System.Text;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

/// <summary>
/// Filter options to be passed into the Test Platform during Discovery/Execution.
/// </summary>
[DataContract]
public class FilterOptions
{
    /// <summary>
    /// Gets or sets the regular expression that will be applied on the property before matching.
    /// </summary>
    [DataMember]
    public string? FilterRegEx { get; set; }

    /// <summary>
    /// Gets or sets the optional regular expression replacement string. When this property is set, <see cref="System.Text.RegularExpressions.Regex.Replace(string)"/>
    /// will be called upon property value instead of <see cref="System.Text.RegularExpressions.Regex.Match(string)"/> before matching.
    /// </summary>
    [DataMember]
    public string? FilterRegExReplacement { get; set; }

    /// <inheritdoc/>
    public override string ToString()
    {
        StringBuilder sb = new();
        sb.AppendLine(string.Format(CultureInfo.CurrentCulture, "FilterOptions:"));
        sb.AppendLine(string.Format(CultureInfo.CurrentCulture,
            "   FilterRegEx={0}   FilterRegExReplacement={1}",
            FilterRegEx ?? string.Empty,
            FilterRegExReplacement ?? string.Empty));
        return sb.ToString();
    }

    protected bool Equals(FilterOptions? other) =>
        other != null &&
        string.Equals(FilterRegEx, other.FilterRegEx) &&
        string.Equals(FilterRegExReplacement, other.FilterRegExReplacement);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => Equals(obj as FilterOptions);

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = FilterRegEx?.GetHashCode() ?? 0;
            return (hashCode * 397) ^ (FilterRegExReplacement != null ? FilterRegExReplacement.GetHashCode() : 0);
        }
    }
}
