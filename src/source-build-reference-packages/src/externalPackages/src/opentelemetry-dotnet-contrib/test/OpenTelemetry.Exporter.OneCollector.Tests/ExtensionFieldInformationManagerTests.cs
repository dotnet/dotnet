// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Xunit;

namespace OpenTelemetry.Exporter.OneCollector.Tests;

public class ExtensionFieldInformationManagerTests
{
    [Fact]
    public void FieldInformationIsCachedTest()
    {
        var extensionFieldInformationManager = new ExtensionFieldInformationManager();

        var result = extensionFieldInformationManager.TryResolveExtensionFieldInformation("ext.something.fieldName1", out var fieldInformation);

        Assert.True(result);
        Assert.NotNull(fieldInformation);
        Assert.Equal("something", fieldInformation.ExtensionName);
        Assert.Equal("fieldName1", fieldInformation.FieldName);

        Assert.Equal(1, extensionFieldInformationManager.CountOfCachedExtensionFields);

        result = extensionFieldInformationManager.TryResolveExtensionFieldInformation("ext.something.fieldName1", out fieldInformation);

        Assert.Equal(1, extensionFieldInformationManager.CountOfCachedExtensionFields);

        Assert.True(result);
        Assert.NotNull(fieldInformation);
        Assert.Equal("something", fieldInformation.ExtensionName);
        Assert.Equal("fieldName1", fieldInformation.FieldName);

        result = extensionFieldInformationManager.TryResolveExtensionFieldInformation("ext.something.field.Name2", out fieldInformation);

        Assert.Equal(2, extensionFieldInformationManager.CountOfCachedExtensionFields);

        Assert.True(result);
        Assert.NotNull(fieldInformation);
        Assert.Equal("something", fieldInformation.ExtensionName);
        Assert.Equal("field.Name2", fieldInformation.FieldName);
    }

    [Fact]
    public void InvalidFieldNamesIgnoredTest()
    {
        var extensionFieldInformationManager = new ExtensionFieldInformationManager();
        Assert.False(extensionFieldInformationManager.TryResolveExtensionFieldInformation("ext.", out _));

        Assert.Equal(1, extensionFieldInformationManager.CountOfCachedExtensionFields);

        Assert.False(extensionFieldInformationManager.TryResolveExtensionFieldInformation("EXT.", out _));

        Assert.Equal(1, extensionFieldInformationManager.CountOfCachedExtensionFields);

        Assert.False(extensionFieldInformationManager.TryResolveExtensionFieldInformation("ext..", out _));
        Assert.False(extensionFieldInformationManager.TryResolveExtensionFieldInformation("ext. .", out _));
        Assert.False(extensionFieldInformationManager.TryResolveExtensionFieldInformation("ext..field", out _));
        Assert.False(extensionFieldInformationManager.TryResolveExtensionFieldInformation("ext.something", out _));
        Assert.False(extensionFieldInformationManager.TryResolveExtensionFieldInformation("ext.something.", out _));
        Assert.False(extensionFieldInformationManager.TryResolveExtensionFieldInformation("ext.SOMETHING.", out _));
        Assert.False(extensionFieldInformationManager.TryResolveExtensionFieldInformation("ext.something. ", out _));

        Assert.Equal(7, extensionFieldInformationManager.CountOfCachedExtensionFields);
    }

    [Fact]
    public void FieldInformationCacheLimitTest()
    {
        var extensionFieldInformationManager = new ExtensionFieldInformationManager();

        for (var i = 0; i < ExtensionFieldInformationManager.MaxNumberOfCachedFieldInformations + 128; i++)
        {
            var fieldName = $"fieldName{i}";

            var result = extensionFieldInformationManager.TryResolveExtensionFieldInformation($"ext.something.{fieldName}", out var fieldInformation);

            Assert.True(result);
            Assert.NotNull(fieldInformation);
            Assert.Equal("something", fieldInformation.ExtensionName);
            Assert.Equal(fieldName, fieldInformation.FieldName);
        }

        Assert.Equal(ExtensionFieldInformationManager.MaxNumberOfCachedFieldInformations, extensionFieldInformationManager.CountOfCachedExtensionFields);
    }
}
