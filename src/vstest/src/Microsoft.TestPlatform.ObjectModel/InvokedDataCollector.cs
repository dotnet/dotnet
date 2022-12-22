// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.Serialization;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel;

public sealed class InvokedDataCollector : IEquatable<InvokedDataCollector>
{
    /// <summary>
    /// Initialize an InvokedDataCollector
    /// </summary>
    /// <param name="uri">Data collector Uri</param>
    /// <param name="assemblyQualifiedName">Data collector assembly qualified name</param>
    /// <param name="filePath">Data collector file path</param>
    /// <param name="hasAttachmentProcessor">True if data collector registers an attachment processor</param>
    public InvokedDataCollector(Uri uri, string friendlyName, string assemblyQualifiedName, string filePath, bool hasAttachmentProcessor)
    {
        Uri = uri ?? throw new ArgumentNullException(nameof(uri));
        FriendlyName = friendlyName ?? throw new ArgumentNullException(nameof(friendlyName));
        AssemblyQualifiedName = assemblyQualifiedName ?? throw new ArgumentNullException(nameof(assemblyQualifiedName));
        FilePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
        HasAttachmentProcessor = hasAttachmentProcessor;
    }

    /// <summary>
    /// DataCollector uri.
    /// </summary>
    [DataMember]
    public Uri Uri { get; private set; }

    /// <summary>
    /// DataCollector FriendlyName.
    /// </summary>
    [DataMember]
    public string FriendlyName { get; private set; }

    /// <summary>
    /// AssemblyQualifiedName of data collector.
    /// </summary>
    [DataMember]
    public string AssemblyQualifiedName { get; private set; }

    /// <summary>
    /// Data collector file path.
    /// </summary>
    [DataMember]
    public string FilePath { get; private set; }

    /// <summary>
    /// True if the collector registers an attachments processor.
    /// </summary>
    [DataMember]
    public bool HasAttachmentProcessor { get; private set; }

    /// <summary>
    /// Compares InvokedDataCollector instances for equality.
    /// </summary>
    /// <param name="other">InvokedDataCollector instance</param>
    /// <returns>True if objects are equal</returns>
    public bool Equals(InvokedDataCollector? other)
        => other is not null
        && HasAttachmentProcessor == other.HasAttachmentProcessor
        && Uri.AbsoluteUri == other.Uri.AbsoluteUri
        && FriendlyName == other.FriendlyName
        && AssemblyQualifiedName == other.AssemblyQualifiedName
        && FilePath == other.FilePath;

    /// <summary>
    /// Compares InvokedDataCollector instances for equality.
    /// </summary>
    /// <param name="obj">InvokedDataCollector instance</param>
    /// <returns>True if objects are equal</returns>
    public override bool Equals(object? obj)
        => Equals(obj as InvokedDataCollector);

    /// <summary>
    /// Returns the object hashcode
    /// </summary>
    /// <returns>Hashcode value</returns>
    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = Uri.GetHashCode();
            hashCode = (hashCode * 397) ^ FriendlyName.GetHashCode();
            hashCode = (hashCode * 397) ^ AssemblyQualifiedName.GetHashCode();
            hashCode = (hashCode * 397) ^ (FilePath != null ? FilePath.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ HasAttachmentProcessor.GetHashCode();
            return hashCode;
        }
    }

    /// <summary>
    /// Return string representation for the current object
    /// </summary>
    /// <returns>String representation</returns>
    public override string ToString()
        => $"Uri: '{Uri}' FriendlyName: '{FriendlyName}' AssemblyQualifiedName: '{AssemblyQualifiedName}' FilePath: '{FilePath}' HasAttachmentProcessor: '{HasAttachmentProcessor}'";
}
