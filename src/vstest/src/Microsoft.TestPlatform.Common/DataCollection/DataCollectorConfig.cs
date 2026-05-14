// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework.Utilities;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;

namespace Microsoft.VisualStudio.TestPlatform.Common.DataCollector;

/// <summary>
/// The data collector config.
/// </summary>
internal class DataCollectorConfig : TestExtensionPluginInformation
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DataCollectorConfig"/> class.
    /// </summary>
    /// <param name="type">
    /// The type.
    /// </param>
    public DataCollectorConfig(Type type)
        : base(type)
    {
        DataCollectorType = type ?? throw new ArgumentNullException(nameof(type));
        TypeUri = GetTypeUri(type);
        FriendlyName = GetFriendlyName(type);
        AttachmentsProcessorType = GetAttachmentsProcessors(type);
    }

    /// <summary>
    /// Gets the data collector type.
    /// </summary>
    public Type DataCollectorType { get; private set; }

    /// <summary>
    /// Gets the type uri.
    /// </summary>
    public Uri? TypeUri { get; private set; }

    /// <summary>
    /// Gets the friendly name.
    /// </summary>
    public string FriendlyName { get; private set; }

    /// <inheritdoc />
    public override string? IdentifierData
    {
        get
        {
            return TypeUri?.ToString();
        }
    }

    /// <inheritdoc />
    public override ICollection<object?> Metadata
    {
        get
        {
            return new object?[] { TypeUri?.ToString(), FriendlyName, AttachmentsProcessorType != null };
        }
    }

    /// <summary>
    /// Gets attachments processor
    /// </summary>
    public Type? AttachmentsProcessorType { get; private set; }

    /// <summary>
    /// Check if collector registers an attachment processor.
    /// </summary>
    /// <returns>True if collector registers an attachment processor.</returns>
    public bool HasAttachmentsProcessor() => AttachmentsProcessorType != null;

    /// <summary>
    /// Gets the Type Uri for the data collector.
    /// </summary>
    /// <param name="dataCollectorType">The data collector to get the Type URI for.</param>
    /// <returns>Type Uri of the data collector.</returns>
    private static Uri? GetTypeUri(Type dataCollectorType)
    {
        var typeUriAttributes = GetAttributes(dataCollectorType, typeof(DataCollectorTypeUriAttribute));
        if (typeUriAttributes.Length > 0)
        {
            var typeUriAttribute = (DataCollectorTypeUriAttribute)typeUriAttributes[0];
            if (!typeUriAttribute.TypeUri.IsNullOrWhiteSpace())
            {
                return new Uri(typeUriAttribute.TypeUri);
            }
        }

        return null;
    }

    /// <summary>
    /// Gets the attachment processor for the data collector.
    /// </summary>
    /// <param name="dataCollectorType">The data collector to get the attachment processor for.</param>
    /// <returns>Type of the attachment processor.</returns>
    private static Type? GetAttachmentsProcessors(Type dataCollectorType)
    {
        var attachmentsProcessor = GetAttributes(dataCollectorType, typeof(DataCollectorAttachmentProcessorAttribute));
        if (attachmentsProcessor.Length > 0)
        {
            var attachmenstProcessorsAttribute = (DataCollectorAttachmentProcessorAttribute)attachmentsProcessor[0];
            if (attachmenstProcessorsAttribute.Type != null)
            {
                return attachmenstProcessorsAttribute.Type;
            }
        }

        return null;
    }

    /// <summary>
    /// Gets the friendly name for the data collector.
    /// </summary>
    /// <param name="dataCollectorType">The data collector to get the Type URI for.</param>
    /// <returns>Friendly name of the data collector.</returns>
    private static string GetFriendlyName(Type dataCollectorType)
    {
        // Get the friendly name from the attribute.
        var friendlyNameAttributes = GetAttributes(dataCollectorType, typeof(DataCollectorFriendlyNameAttribute));
        if (friendlyNameAttributes != null && friendlyNameAttributes.Length > 0)
        {
            var friendlyNameAttribute = (DataCollectorFriendlyNameAttribute)friendlyNameAttributes[0];
            if (!friendlyNameAttribute.FriendlyName.IsNullOrEmpty())
            {
                return friendlyNameAttribute.FriendlyName;
            }
        }

        return string.Empty;
    }

    /// <summary>
    /// Gets the attributes of the specified type from the data collector type.
    /// </summary>
    /// <param name="dataCollectorType">
    /// Data collector type to get attribute from.
    /// </param>
    /// <param name="attributeType">
    /// The type of attribute to look for.
    /// </param>
    /// <returns>
    /// Array of attributes matching the type provided.  Will be an empty array if none were found.
    /// </returns>
    private static object[] GetAttributes(Type dataCollectorType, Type attributeType)
    {
        TPDebug.Assert(dataCollectorType != null, "null dataCollectorType");
        TPDebug.Assert(attributeType != null, "null attributeType");

        // If any attribute constructor on the type throws, the exception will bubble up through
        // the "GetCustomAttributes" method.
        return dataCollectorType.GetCustomAttributes(attributeType, true).ToArray();
    }
}
