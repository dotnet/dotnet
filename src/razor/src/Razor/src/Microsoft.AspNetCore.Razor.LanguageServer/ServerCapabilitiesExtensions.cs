// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

public static class ServerCapabilitiesExtensions
{
    private static readonly IReadOnlyDictionary<string, PropertyInfo> s_propertyMappings;

    static ServerCapabilitiesExtensions()
    {
        var propertyInfos = typeof(VSInternalServerCapabilities).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        var dictionary = new Dictionary<string, PropertyInfo>();
        foreach (var propertyInfo in propertyInfos)
        {
            var dataMemberAttribute = propertyInfo.GetCustomAttribute<DataMemberAttribute>();
            Assumes.NotNull(dataMemberAttribute);
            var serverCapability = dataMemberAttribute.Name;
            Assumes.NotNull(serverCapability);
            dictionary[serverCapability] = propertyInfo;
        }

        s_propertyMappings = dictionary;
    }

    internal static void ApplyRegistrationResult(this VSInternalServerCapabilities serverCapabilities, RegistrationExtensionResult registrationExtensionResult)
    {
        var serverCapability = registrationExtensionResult.ServerCapability;
        if (s_propertyMappings.ContainsKey(serverCapability))
        {
            var propertyInfo = s_propertyMappings[serverCapability];

            propertyInfo.SetValue(serverCapabilities, registrationExtensionResult.Options);
        }
        else
        {
            serverCapabilities.Experimental ??= new Dictionary<string, object>();

            var dict = (Dictionary<string, object>)serverCapabilities.Experimental;
            dict[registrationExtensionResult.ServerCapability] = registrationExtensionResult.Options;
        }
    }
}
