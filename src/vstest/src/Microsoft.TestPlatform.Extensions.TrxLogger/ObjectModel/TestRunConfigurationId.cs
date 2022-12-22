// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.TestPlatform.Extensions.TrxLogger.ObjectModel;

/// <summary>
/// The test run configuration id.
/// </summary>
internal sealed class TestRunConfigurationId
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TestRunConfigurationId"/> class.
    /// </summary>
    public TestRunConfigurationId()
    {
        Id = Guid.NewGuid();
    }

    /// <summary>
    /// Gets the id.
    /// </summary>
    public Guid Id { get; }
}
