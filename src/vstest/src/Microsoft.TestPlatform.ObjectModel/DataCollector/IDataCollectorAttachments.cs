// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;

/// <summary>
/// Interface for data collectors add-ins that choose to handle attachment(s) generated
/// </summary>
[Obsolete("Interface is deprecated. Please use IDataCollectorAttachmentProcessor instead")]
public interface IDataCollectorAttachments
{
    /// <summary>
    /// Gets the attachment set after Test Run Session
    /// </summary>
    /// <returns>Gets the attachment set after Test Run Session</returns>
    ICollection<AttachmentSet> HandleDataCollectionAttachmentSets(ICollection<AttachmentSet> dataCollectionAttachments);

    /// <summary>
    /// Gets the attachment Uri, which is handled by current Collector
    /// </summary>
    Uri GetExtensionUri();
}
