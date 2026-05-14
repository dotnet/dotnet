// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;

/// <summary>
/// Severity levels at which a DataCollectionTextMessage can be logged.
/// </summary>
public enum DataCollectorMessageLevel
{
    Error = 0,
    Warning = 1,
    Info = 2,
    Data = 3,
}
