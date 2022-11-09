// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

using Newtonsoft.Json.Serialization;

namespace Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Serialization;

/// <summary>
/// JSON contract resolver for mapping test platform types.
/// </summary>
public class DefaultTestPlatformContractResolver : DefaultContractResolver
{
    /// <inheritdoc/>
    protected override JsonContract CreateContract(Type objectType)
    {
        var contract = base.CreateContract(objectType);

        if (typeof(List<KeyValuePair<TestProperty, object>>) == objectType)
        {
            // ObjectModel.TestObject provides a custom TestProperty based data store for all
            // inherited objects. This converter helps with serialization of TestProperty and values
            // over the wire.
            // Each object inherited from TestObject handles it's own serialization. Most of them use
            // this TestProperty data store for members as well. In such cases, we just ignore those
            // properties. E.g. TestCase object's CodeFilePath is ignored for serialization since the
            // actual data is already getting serialized by this converter.
            // OTOH, TestResult has members that are not based off this store.
            contract.Converter = new TestObjectConverter();
        }
        else if (objectType == typeof(ITestRunStatistics))
        {
            // This converter is required to hint json.net to use a concrete class for serialization
            // of ITestRunStatistics. We can't remove ITestRunStatistics since it is a breaking change.
            contract.Converter = new TestRunStatisticsConverter();
        }

        return contract;
    }
}

/// TODO: This is not used now, but I was experimenting with this quite a bit for performance, leaving it here in case I was wrong
/// and the serializer settings actually have signigicant impact on the speed.
/// <summary>
/// JSON contract resolver for mapping test platform types.
/// </summary>
internal class DefaultTestPlatformContractResolver7 : DefaultContractResolver
{
    public DefaultTestPlatformContractResolver7()
    {
    }
    /// <inheritdoc/>
    protected override JsonContract CreateContract(Type objectType)
    {
        var contract = base.CreateContract(objectType);

        if (typeof(List<KeyValuePair<TestProperty, object>>) == objectType)
        {
            // ObjectModel.TestObject provides a custom TestProperty based data store for all
            // inherited objects. This converter helps with serialization of TestProperty and values
            // over the wire.
            // Each object inherited from TestObject handles it's own serialization. Most of them use
            // this TestProperty data store for members as well. In such cases, we just ignore those
            // properties. E.g. TestCase object's CodeFilePath is ignored for serialization since the
            // actual data is already getting serialized by this converter.
            // OTOH, TestResult has members that are not based off this store.
            contract.Converter = new TestObjectConverter7();
        }
        else if (objectType == typeof(ITestRunStatistics))
        {
            // This converter is required to hint json.net to use a concrete class for serialization
            // of ITestRunStatistics. We can't remove ITestRunStatistics since it is a breaking change.
            contract.Converter = new TestRunStatisticsConverter();
        }

        return contract;
    }
}
