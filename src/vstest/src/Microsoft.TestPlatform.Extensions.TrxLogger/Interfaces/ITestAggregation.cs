// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Microsoft.TestPlatform.Extensions.TrxLogger.ObjectModel;

internal interface ITestAggregation : ITestElement
{
    /// <summary>
    /// Gets a snapshot of the test links.
    /// </summary>
    IReadOnlyDictionary<Guid, TestLink> TestLinks { get; }

    /// <summary>
    /// Adds a test link, unless a link with the same id was already added.
    /// </summary>
    void AddTestLink(TestLink testLink);
}
