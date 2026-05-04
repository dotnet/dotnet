// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

using Newtonsoft.Json.Serialization;

namespace Microsoft.TestPlatform.CommunicationUtilities.UnitTests.NewtonsoftReference.Serialization;

/// <summary>
/// Original Newtonsoft-based DefaultTestPlatformContractResolver, extracted from main for comparison testing.
/// </summary>
internal class NewtonsoftDefaultTestPlatformContractResolver : DefaultContractResolver
{
    /// <inheritdoc/>
    protected override JsonContract CreateContract(Type objectType)
    {
        var contract = base.CreateContract(objectType);

        if (typeof(List<KeyValuePair<TestProperty, object>>) == objectType)
        {
            contract.Converter = new NewtonsoftTestObjectConverter();
        }
        else if (objectType == typeof(ITestRunStatistics))
        {
            contract.Converter = new NewtonsoftTestRunStatisticsConverter();
        }

        return contract;
    }
}
