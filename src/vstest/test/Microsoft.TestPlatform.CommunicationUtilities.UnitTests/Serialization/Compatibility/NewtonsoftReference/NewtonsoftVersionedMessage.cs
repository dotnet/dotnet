// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.TestPlatform.CommunicationUtilities.UnitTests.NewtonsoftReference;

/// <summary>
/// Original Newtonsoft-based VersionedMessage class, extracted from main for comparison testing.
/// </summary>
internal class NewtonsoftVersionedMessage : NewtonsoftMessage
{
    /// <summary>
    /// Gets or sets the version of the message
    /// </summary>
    public int Version { get; set; }
}
