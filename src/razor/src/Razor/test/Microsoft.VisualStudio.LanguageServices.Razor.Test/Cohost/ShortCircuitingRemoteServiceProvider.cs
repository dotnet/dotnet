// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Razor.Remote;
using Microsoft.CodeAnalysis.Remote.Razor;
using Microsoft.ServiceHub.Framework;
using Nerdbank.Streams;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.LanguageServices.Razor.Test.Cohost;

/// <summary>
/// An implementation of IRemoteServiceProvider that doesn't actually do anything remote, but rather directly calls service methods
/// </summary>
internal class ShortCircuitingRemoteServiceProvider(ITestOutputHelper testOutputHelper) : IRemoteServiceProvider
{
    private static readonly Dictionary<Type, IServiceHubServiceFactory> s_factoryMap = BuildFactoryMap();

    private readonly IServiceProvider _serviceProvider = new TestTraceSourceProvider(testOutputHelper);

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

    public async ValueTask<TResult?> TryInvokeAsync<TService, TResult>(
        Solution solution,
        Func<TService, RazorPinnedSolutionInfoWrapper, CancellationToken, ValueTask<TResult>> invocation,
        CancellationToken cancellationToken,
        [CallerFilePath] string? callerFilePath = null,
        [CallerMemberName] string? callerMemberName = null)
        where TService : class
    {
        Assert.True(s_factoryMap.TryGetValue(typeof(TService), out var factory));

        var testServiceBroker = new InterceptingServiceBroker(solution);

        // We don't ever use this stream, because we never really use ServiceHub, but going through its factory method means the
        // remote services under test are using their full MEF composition etc. so we get excellent coverage.
        var (stream, _) = FullDuplexStream.CreatePair();
        var service = (TService)await factory.CreateAsync(stream, _serviceProvider, serviceActivationOptions: default, testServiceBroker, authorizationServiceClient: default!);

        // This is never used, we short-circuited things by passing the solution direct to the InterceptingServiceBroker
        var solutionInfo = new RazorPinnedSolutionInfoWrapper();

        testOutputHelper.WriteLine($"Pretend OOP call for {typeof(TService).Name}, invocation: {Path.GetFileNameWithoutExtension(callerFilePath)}.{callerMemberName}");
        return await invocation(service, solutionInfo, cancellationToken);
    }
}
