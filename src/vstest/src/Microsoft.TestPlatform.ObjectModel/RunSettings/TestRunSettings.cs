// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Xml;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel;

/// <summary>
/// Stores information about a test settings.
/// </summary>
public abstract class TestRunSettings
{
    /// <summary>
    /// Initializes with the name of the test case.
    /// </summary>
    /// <param name="name">The name of the test case.</param>
    protected TestRunSettings(string name)
    {
        ValidateArg.NotNullOrEmpty(name, nameof(name));
        Name = name;
    }

    /// <summary>
    /// Gets the name of the test settings.
    /// Do not put a private setter on this
    /// Chutzpah adapter checks for setters of all properties and it throws error if its private
    /// during RunSettings.LoadSection() call
    /// TODO: Communicate to Chutzpah and fix it
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Converter the setting to be an XmlElement.
    /// </summary>
    /// <returns>The Xml element for the run settings provided.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1059:MembersShouldNotExposeCertainConcreteTypes", Justification = "XmlElement is required in the data collector.")]
    public abstract XmlElement ToXml();
}
