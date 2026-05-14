// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.TestPlatform.Extensions.TrxLogger.XML;

internal class XmlFilePersistence : XmlPersistence
{
    /// <summary>
    /// Type of the object that is persisted to the file, the object that represents the root element
    /// </summary>
    public const string RootObjectType = "RootObjectType";

    /// <summary>
    /// The directory to where the file is being saved, or from where the file is being loaded
    /// </summary>
    public const string DirectoryPath = "DirectoryPath";

}
