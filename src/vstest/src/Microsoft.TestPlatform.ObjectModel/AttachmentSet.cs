// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel;

/// <summary>
/// Represents a set of attachments.
/// </summary>
[DataContract]
public class AttachmentSet
{
    /// <summary>
    /// URI of the sender.
    /// If a data-collector is sending this set, then it should be uri of the data collector. Also if an
    /// executor is sending this attachment, then it should be uri of executor.
    /// </summary>
    [DataMember]
    public Uri Uri { get; private set; }

    /// <summary>
    /// Name of the sender.
    /// </summary>
    [DataMember]
    public string DisplayName { get; private set; }

    /// <summary>
    /// List of data attachments.
    /// These attachments can be things such as files that the collector/adapter wants to make available to the publishers.
    /// </summary>
    [DataMember]
    public IList<UriDataAttachment> Attachments { get; private set; }

    public AttachmentSet(Uri uri, string displayName)
    {
        Uri = uri;
        DisplayName = displayName;
        Attachments = new List<UriDataAttachment>();
    }

    public override string ToString()
        => $"{nameof(Uri)}: {Uri.AbsoluteUri}, {nameof(DisplayName)}: {DisplayName}, {nameof(Attachments)}: [{string.Join(",", Attachments)}]";
}


/// <summary>
/// Defines the data attachment.
/// Dev10 equivalent is UriDataAttachment.
/// </summary>
[DataContract]
public class UriDataAttachment
{
    /// <summary>
    /// Description of the attachment.
    /// </summary>
    [DataMember]
    public string? Description { get; }

    /// <summary>
    /// Uri of the attachment.
    /// </summary>
    [DataMember]
    public Uri Uri { get; }

    public UriDataAttachment(Uri uri, string? description)
    {
        Uri = uri;
        Description = description;
    }

    public override string ToString()
    {
        return $"{nameof(Uri)}: {Uri.AbsoluteUri}, {nameof(Description)}: {Description}";
    }

    public static UriDataAttachment CreateFrom(string localFilePath, string? description)
    {
        var uri = new UriBuilder() { Scheme = "file", Host = "", Path = localFilePath }.Uri;
        return new UriDataAttachment(uri, description);
    }
}
