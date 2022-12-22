// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;

internal interface IRunSettingsHelper
{
    bool IsDefaultTargetArchitecture { get; set; }

    bool IsDesignMode { get; set; }
}
