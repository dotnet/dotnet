// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Xml;

using Microsoft.TestPlatform.Extensions.TrxLogger.XML;
using Microsoft.VisualStudio.TestPlatform.Extensions.TrxLogger;

namespace Microsoft.TestPlatform.Extensions.TrxLogger.ObjectModel;

/// <summary>
/// TestMethod contains information about a unit test method that needs to be executed
/// </summary>
internal sealed class TestMethod : IXmlTestStore
{
    public TestMethod(string name, string className)
    {
        TPDebug.Assert(!name.IsNullOrEmpty(), "name is null");
        TPDebug.Assert(!className.IsNullOrEmpty(), "className is null");
        Name = name;
        ClassName = className;
    }

    /// <summary>
    /// Gets the name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the class name.
    /// </summary>
    public string ClassName { get; }

    /// <summary>
    /// Gets or sets a value indicating whether is valid.
    /// </summary>
    public bool IsValid { get; set; }

    #region Override

    /// <summary>
    /// Override function for Equals.
    /// </summary>
    /// <param name="obj">
    /// The object to compare.
    /// </param>
    /// <returns>
    /// The <see cref="bool"/>.
    /// </returns>
    public override bool Equals(object? obj)
    {
        return obj is TestMethod otherTestMethod && Name == otherTestMethod.Name
                                                 && ClassName == otherTestMethod.ClassName && IsValid == otherTestMethod.IsValid;
    }

    /// <summary>
    /// Override function for GetHashCode.
    /// </summary>
    /// <returns>
    /// The <see cref="int"/>.
    /// </returns>
    public override int GetHashCode()
    {
        return Name?.GetHashCode() ?? 0;
    }

    #endregion Override

    #region IXmlTestStore Members

    /// <summary>
    /// Saves the class under the XmlElement..
    /// </summary>
    /// <param name="element">
    /// The parent xml.
    /// </param>
    /// <param name="parameters">
    /// The parameter
    /// </param>
    public void Save(XmlElement element, XmlTestStoreParameters? parameters)
    {
        XmlPersistence helper = new();
        helper.SaveSimpleField(element, "@className", ClassName, string.Empty);
        helper.SaveSimpleField(element, "@name", Name, string.Empty);
        helper.SaveSimpleField(element, "isValid", IsValid, false);
    }

    #endregion
}
