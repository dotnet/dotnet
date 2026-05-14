// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;

/// <summary>
/// Type of test case failure which occurred.
/// </summary>
public enum TestCaseFailureType
{
    None = 0,
    Assertion = 1,
    UnhandledException = 2,
    UnexpectedException = 3,
    MissingException = 4,
    Other = 5,
}
