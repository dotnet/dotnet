// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;

using Newtonsoft.Json.Serialization;

namespace Microsoft.TestPlatform.CommunicationUtilities.UnitTests.NewtonsoftReference.Serialization;

/// <summary>
/// Original Newtonsoft-based TestPlatformContractResolver1 (v1 protocol), extracted from main for comparison testing.
/// </summary>
internal class NewtonsoftTestPlatformContractResolver1 : NewtonsoftDefaultTestPlatformContractResolver
{
    /// <inheritdoc/>
    protected override JsonContract CreateContract(Type objectType)
    {
        var contract = base.CreateContract(objectType);
        if (typeof(TestCase) == objectType)
        {
            contract.Converter = new NewtonsoftTestCaseConverter();
        }
        else if (typeof(TestResult) == objectType)
        {
            contract.Converter = new NewtonsoftTestResultConverter();
        }

        return contract;
    }
}
