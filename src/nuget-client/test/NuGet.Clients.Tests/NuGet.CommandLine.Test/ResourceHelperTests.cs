// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Xunit;

namespace NuGet.CommandLine.Test
{
    public class ResourceHelperTests
    {
        [Theory]
        [InlineData(typeof(NuGetCommand), "SignCommandCertificatePathDescription")]
        [InlineData(typeof(NuGetResources), "UnableToConvertTypeError")]
        public void GetLocalizedString_ExistingResource_ReturnsStringResource(Type type, string resourceName)
        {
            // Arrange & Act
            string resource = ResourceHelper.GetLocalizedString(type, resourceName);

            // Assert
            Assert.NotNull(resource);
        }

        [Fact]
        public void GetLocalizedString_MultipleStringResources_ReturnsMultilineStringResources()
        {
            // Arrange & Act
            string resource = ResourceHelper.GetLocalizedString(typeof(NuGetCommand), "CommandNoServiceEndpointDescription;ConfigCommandDesc");

            string[] rep = resource.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            // Assert
            Assert.Equal(2, rep.Length);
            Assert.All(rep, e => Assert.NotEmpty(e));
        }

        [Fact]
        public void GetLocalizedString_ExistingResource_ReturnsSameValueFromResourceClass()
        {
            // Arrange & Act
            string resource = ResourceHelper.GetLocalizedString(typeof(NuGetResources), nameof(NuGetResources.UpdateCommandNuGetUpToDate));

            // Assert
            Assert.Equal(NuGetResources.UpdateCommandNuGetUpToDate, resource);
        }

        [Fact]
        public void GetLocalizedString_TypeWithoutResourceManagerMember_Throws()
        {
            // Arrange, Act & Assert
            Assert.Throws<InvalidOperationException>(() => ResourceHelper.GetLocalizedString(typeof(string), "A_String_Resource"));
        }

        [Fact]
        public void GetLocalizedString_NonExistingResourceInType_Throws()
        {
            // Arrange, Act & Assert
            Assert.Throws<InvalidOperationException>(() => ResourceHelper.GetLocalizedString(typeof(NuGetCommand), "A_non_existing_string"));
        }

        [Fact]
        public void GetLocalizedString_NullOrEmptyArgument_Throws()
        {
            // Arrange, Act & Assert
            Assert.Throws<ArgumentNullException>(() => ResourceHelper.GetLocalizedString(resourceType: null, resourceNames: null));
            Assert.Throws<ArgumentNullException>(() => ResourceHelper.GetLocalizedString(resourceType: null, resourceNames: "aaaa"));
            Assert.Throws<ArgumentNullException>(() => ResourceHelper.GetLocalizedString(resourceType: null, resourceNames: ""));
            Assert.Throws<ArgumentException>(() => ResourceHelper.GetLocalizedString(resourceType: typeof(NuGetCommand), resourceNames: null));
            Assert.Throws<ArgumentException>(() => ResourceHelper.GetLocalizedString(resourceType: typeof(NuGetCommand), resourceNames: ""));
        }
    }
}
