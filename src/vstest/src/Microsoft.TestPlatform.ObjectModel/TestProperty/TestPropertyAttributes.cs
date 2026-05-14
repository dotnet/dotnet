// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel;

[Flags]
public enum TestPropertyAttributes
{
    None = 0x00, // Default
    Hidden = 0x01,
    Immutable = 0x02,
    [Obsolete("Use TestObject.Traits collection to create traits")]
    Trait = 0x04,
}
