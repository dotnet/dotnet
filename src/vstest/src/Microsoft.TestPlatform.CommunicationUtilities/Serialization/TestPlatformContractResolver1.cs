// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;

using Newtonsoft.Json.Serialization;

namespace Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Serialization;

/// <summary>
/// JSON contract resolver for mapping test platform types for v1 serialization.
/// </summary>
public class TestPlatformContractResolver1 : DefaultTestPlatformContractResolver
{
    /// <inheritdoc/>
    protected override JsonContract CreateContract(Type objectType)
    {
        var contract = base.CreateContract(objectType);
        if (typeof(TestCase) == objectType)
        {
            contract.Converter = new TestCaseConverter();
        }
        else if (typeof(TestResult) == objectType)
        {
            contract.Converter = new TestResultConverter();
        }

        return contract;
    }
}
