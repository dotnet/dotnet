// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

using Microsoft.VisualStudio.TestPlatform.Common.Utilities;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework.Utilities;

/// <summary>
/// The test discoverer plugin information.
/// </summary>
internal class TestDiscovererPluginInformation : TestPluginInformation
{
    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="testDiscovererType">Data type of the test discoverer</param>
    public TestDiscovererPluginInformation(Type testDiscovererType)
        : base(testDiscovererType)
    {
        if (testDiscovererType != null)
        {
            FileExtensions = GetFileExtensions(testDiscovererType);
            DefaultExecutorUri = GetDefaultExecutorUri(testDiscovererType);
            AssemblyType = GetAssemblyType(testDiscovererType);
            IsDirectoryBased = GetIsDirectoryBased(testDiscovererType);
        }
    }

    /// <summary>
    /// Metadata for the test plugin
    /// </summary>
    public override ICollection<object?> Metadata
    {
        get
        {
            return new object?[] { FileExtensions, DefaultExecutorUri, AssemblyType, IsDirectoryBased };
        }
    }

    /// <summary>
    /// Gets collection of file extensions supported by discoverer plugin.
    /// </summary>
    public List<string>? FileExtensions
    {
        get;
        private set;
    }

    /// <summary>
    /// Gets the Uri identifying the executor
    /// </summary>
    public string? DefaultExecutorUri
    {
        get;
        private set;
    }

    /// <summary>
    /// Gets the assembly type supported by discoverer plugin.
    /// </summary>
    public AssemblyType AssemblyType
    {
        get;
        private set;
    }

    /// <summary>
    /// <c>true</c> if the discoverer plugin is decorated with <see cref="DirectoryBasedTestDiscovererAttribute"/>,
    /// <c>false</c> otherwise.
    /// </summary>
    public bool IsDirectoryBased
    {
        get;
        private set;
    }

    /// <summary>
    /// Helper to get file extensions from the <see cref="FileExtensionAttribute"/> on the discover plugin.
    /// </summary>
    /// <param name="testDiscovererType">Data type of the test discoverer</param>
    /// <returns>List of file extensions</returns>
    private static List<string> GetFileExtensions(Type testDiscovererType)
    {
        var fileExtensions = new List<string>();

        var attributes = testDiscovererType.GetTypeInfo().GetCustomAttributes(typeof(FileExtensionAttribute), inherit: false).ToArray();
        if (attributes != null && attributes.Length > 0)
        {
            foreach (var attribute in attributes)
            {
                var fileExtAttribute = (FileExtensionAttribute)attribute;
                if (!fileExtAttribute.FileExtension.IsNullOrEmpty())
                {
                    fileExtensions.Add(fileExtAttribute.FileExtension);
                }
            }
        }

        return fileExtensions;
    }

    /// <summary>
    /// Returns the value of default executor Uri on this type. <c>null</c> if not present.
    /// </summary>
    /// <param name="testDiscovererType"> The test discoverer Type. </param>
    /// <returns> The default executor URI. </returns>
    private static string GetDefaultExecutorUri(Type testDiscovererType)
    {
        var result = string.Empty;

        var attributes = testDiscovererType.GetTypeInfo().GetCustomAttributes(typeof(DefaultExecutorUriAttribute), inherit: false).ToArray();
        if (attributes != null && attributes.Length > 0)
        {
            DefaultExecutorUriAttribute executorUriAttribute = (DefaultExecutorUriAttribute)attributes[0];

            if (!executorUriAttribute.ExecutorUri.IsNullOrEmpty())
            {
                result = executorUriAttribute.ExecutorUri;
            }
        }

        return result;
    }

    /// <summary>
    /// Helper to get the supported assembly type from the <see cref="CategoryAttribute"/> on the discover plugin.
    /// </summary>
    /// <param name="testDiscovererType"> The test discoverer Type. </param>
    /// <returns> Supported assembly type. </returns>
    private static AssemblyType GetAssemblyType(Type testDiscovererType)
    {

        // Get Category
        var attribute = testDiscovererType.GetTypeInfo().GetCustomAttribute(typeof(CategoryAttribute));
        var category = (attribute as CategoryAttribute)?.Category;

        // Get assembly type from category.
        Enum.TryParse(category, true, out AssemblyType assemblyType);
        return assemblyType;
    }

    /// <summary>
    /// Returns <c>true</c> if the discoverer plugin is decorated with
    /// <see cref="DirectoryBasedTestDiscovererAttribute"/>, <c>false</c> otherwise.
    /// </summary>
    /// <param name="testDiscovererType">Data type of the test discoverer</param>
    private static bool GetIsDirectoryBased(Type testDiscovererType)
    {
        var attribute = testDiscovererType.GetTypeInfo().GetCustomAttribute(typeof(DirectoryBasedTestDiscovererAttribute), inherit: false);
        return attribute is DirectoryBasedTestDiscovererAttribute;
    }
}
