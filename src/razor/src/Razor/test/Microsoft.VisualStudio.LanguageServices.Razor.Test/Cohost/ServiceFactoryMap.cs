// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.Remote.Razor;
using Microsoft.ServiceHub.Framework;
using System.Collections.Generic;
using System.Linq;
using System;
using Xunit;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

internal static class ServiceFactoryMap
{
    private static readonly Dictionary<Type, IServiceHubServiceFactory> s_factoryMap = BuildFactoryMap();

    private static Dictionary<Type, IServiceHubServiceFactory> BuildFactoryMap()
    {
        var result = new Dictionary<Type, IServiceHubServiceFactory>();

        foreach (var type in typeof(RazorServiceFactoryBase<>).Assembly.GetTypes())
        {
            if (!type.IsAbstract &&
                typeof(IServiceHubServiceFactory).IsAssignableFrom(type))
            {
                Assert.Equal(typeof(RazorServiceFactoryBase<>), type.BaseType.GetGenericTypeDefinition());

                var genericType = type.BaseType.GetGenericArguments().FirstOrDefault();
                if (genericType != null)
                {
                    // ServiceHub requires a parameterless constructor, so we can safely rely on it existing too
                    var factory = (IServiceHubServiceFactory)Activator.CreateInstance(type);
                    result.Add(genericType, factory);
                }
            }
        }

        return result;
    }

    public static RazorServiceFactoryBase<TService> GetServiceFactory<TService>()
        where TService : class
    {
        Assert.True(s_factoryMap.TryGetValue(typeof(TService), out var factory));

        return (RazorServiceFactoryBase<TService>)factory;
    }
}
