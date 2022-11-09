// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using FluentAssertions;
using Microsoft.DotNet.Build.Tasks.VersionTools;
using Microsoft.DotNet.Internal.DependencyInjection.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Microsoft.DotNet.VersionTools.Tasks.Tests
{
    public class LocalUpdatePublishedVersionsTests
    {
        [Fact]
        public void AreDependenciesRegistered()
        {
            LocalUpdatePublishedVersions task = new LocalUpdatePublishedVersions();

            var collection = new ServiceCollection();
            task.ConfigureServices(collection);
            var provider = collection.BuildServiceProvider();

            DependencyInjectionValidation.IsDependencyResolutionCoherent(
                    s =>
                    {
                        task.ConfigureServices(s);
                    },
                    out string message,
                    additionalSingletonTypes: task.GetExecuteParameterTypes()
                )
                .Should()
                .BeTrue(message);
        }
    }
}
