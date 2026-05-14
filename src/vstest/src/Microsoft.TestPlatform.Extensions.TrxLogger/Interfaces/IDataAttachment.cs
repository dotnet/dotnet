// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.TestPlatform.Extensions.TrxLogger.ObjectModel;

/// <summary>
/// Interface used to define a data attachment.
/// </summary>
internal interface IDataAttachment
{
    /// <summary>
    /// Gets the description for the attachment.
    /// </summary>
    string Description { get; }
}
