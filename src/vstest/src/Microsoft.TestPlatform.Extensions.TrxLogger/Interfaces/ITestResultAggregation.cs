// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Microsoft.TestPlatform.Extensions.TrxLogger.ObjectModel;

internal interface ITestResultAggregation : ITestResult
{
    List<ITestResult> InnerResults { get; }
}
