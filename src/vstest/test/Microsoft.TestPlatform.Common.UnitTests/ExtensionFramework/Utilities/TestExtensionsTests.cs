// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

using Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework.Utilities;
using Microsoft.VisualStudio.TestPlatform.Common.Telemetry;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using FluentAssertions;

namespace TestPlatform.Common.UnitTests.ExtensionFramework.Utilities;

[TestClass]
public class TestExtensionsTests
{
    private readonly TestExtensions _testExtensions;

    public TestExtensionsTests()
    {
        _testExtensions = new TestExtensions();
    }

    [TestMethod]
    public void AddExtensionsShouldNotThrowIfExtensionsIsNull()
    {
        _testExtensions.AddExtension<TestPluginInformation>(null);

        // Validate that the default state does not change.
        Assert.IsNull(_testExtensions.TestDiscoverers);
    }

    [TestMethod]
    public void AddExtensionsShouldNotThrowIfExistingExtensionCollectionIsNull()
    {
        var testDiscoverers = new Dictionary<string, TestDiscovererPluginInformation>
        {
            {
                "td",
                new TestDiscovererPluginInformation(typeof(TestExtensionsTests))
            }
        };

        _testExtensions.AddExtension(testDiscoverers);

        Assert.IsNotNull(_testExtensions.TestDiscoverers);
        CollectionAssert.AreEqual(_testExtensions.TestDiscoverers, testDiscoverers);

        // Validate that the others remain same.
        Assert.IsNull(_testExtensions.TestExecutors);
        Assert.IsNull(_testExtensions.TestSettingsProviders);
        Assert.IsNull(_testExtensions.TestLoggers);
    }

    [TestMethod]
    public void AddExtensionsShouldAddToExistingExtensionCollection()
    {
        var testDiscoverers = new Dictionary<string, TestDiscovererPluginInformation>
        {
            { "td1", new TestDiscovererPluginInformation(typeof(TestExtensionsTests)) },
            { "td2", new TestDiscovererPluginInformation(typeof(TestExtensionsTests)) }
        };

        _testExtensions.TestDiscoverers = new Dictionary<string, TestDiscovererPluginInformation>
        {
            { "td", new TestDiscovererPluginInformation(typeof(TestExtensionsTests)) }
        };

        // Act.
        _testExtensions.AddExtension(testDiscoverers);

        // Validate.
        var expectedTestExtensions = new Dictionary<string, TestDiscovererPluginInformation>
        {
            { "td", new TestDiscovererPluginInformation(typeof(TestExtensionsTests)) },
            { "td1", new TestDiscovererPluginInformation(typeof(TestExtensionsTests)) },
            { "td2", new TestDiscovererPluginInformation(typeof(TestExtensionsTests)) }
        };

        CollectionAssert.AreEqual(_testExtensions.TestDiscoverers.Keys, expectedTestExtensions.Keys);

        // Validate that the others remain same.
        Assert.IsNull(_testExtensions.TestExecutors);
        Assert.IsNull(_testExtensions.TestSettingsProviders);
        Assert.IsNull(_testExtensions.TestLoggers);
    }

    [TestMethod]
    public void AddExtensionsShouldNotAddAnAlreadyExistingExtensionToTheCollection()
    {
        var testDiscoverers = new Dictionary<string, TestDiscovererPluginInformation>
        {
            { "td", new TestDiscovererPluginInformation(typeof(TestExtensionsTests)) }
        };

        _testExtensions.TestDiscoverers = new Dictionary<string, TestDiscovererPluginInformation>
        {
            { "td", new TestDiscovererPluginInformation(typeof(TestExtensionsTests)) }
        };

        // Act.
        _testExtensions.AddExtension(testDiscoverers);

        // Validate.
        CollectionAssert.AreEqual(_testExtensions.TestDiscoverers.Keys, testDiscoverers.Keys);

        // Validate that the others remain same.
        Assert.IsNull(_testExtensions.TestExecutors);
        Assert.IsNull(_testExtensions.TestSettingsProviders);
        Assert.IsNull(_testExtensions.TestLoggers);
    }

    [TestMethod]
    public void GetExtensionsDiscoveredFromAssemblyShouldReturnNullIfNoExtensionsPresent()
    {
        var assemblyLocation = typeof(TestExtensionsTests).Assembly.Location;

        Assert.IsNull(_testExtensions.GetExtensionsDiscoveredFromAssembly(assemblyLocation));
    }

    [TestMethod]
    public void GetExtensionsDiscoveredFromAssemblyShouldNotThrowIfExtensionAssemblyIsNull()
    {
        _testExtensions.TestDiscoverers = new Dictionary<string, TestDiscovererPluginInformation>
        {
            { "td", new TestDiscovererPluginInformation(typeof(TestExtensionsTests)) }
        };

        Assert.IsNull(_testExtensions.GetExtensionsDiscoveredFromAssembly(null));
    }

    [TestMethod]
    public void GetExtensionsDiscoveredFromAssemblyShouldReturnTestDiscoverers()
    {
        var assemblyLocation = typeof(TestExtensionsTests).Assembly.Location;

        _testExtensions.TestDiscoverers = new Dictionary<string, TestDiscovererPluginInformation>
        {
            { "td", new TestDiscovererPluginInformation(typeof(TestExtensionsTests)) },
            { "td1", new TestDiscovererPluginInformation(typeof(TestExtensions)) }
        };

        var extensions = _testExtensions.GetExtensionsDiscoveredFromAssembly(assemblyLocation);

        var expectedExtensions = new Dictionary<string, TestDiscovererPluginInformation>
        {
            { "td", new TestDiscovererPluginInformation(typeof(TestExtensionsTests)) }
        };
        CollectionAssert.AreEqual(expectedExtensions.Keys, extensions!.TestDiscoverers!.Keys);
    }

    [TestMethod]
    public void GetExtensionsDiscoveredFromAssemblyShouldReturnTestExecutors()
    {
        var assemblyLocation = typeof(TestExtensionsTests).Assembly.Location;

        _testExtensions.TestExecutors = new Dictionary<string, TestExecutorPluginInformation>
        {
            { "te", new TestExecutorPluginInformation(typeof(TestExtensionsTests)) },
            { "te1", new TestExecutorPluginInformation(typeof(TestExtensions)) }
        };

        var extensions = _testExtensions.GetExtensionsDiscoveredFromAssembly(assemblyLocation);

        var expectedExtensions = new Dictionary<string, TestExecutorPluginInformation>
        {
            { "te", new TestExecutorPluginInformation(typeof(TestExtensionsTests)) }
        };
        CollectionAssert.AreEqual(expectedExtensions.Keys, extensions!.TestExecutors!.Keys);
    }

    [TestMethod]
    public void GetExtensionsDiscoveredFromAssemblyShouldReturnTestSettingsProviders()
    {
        var assemblyLocation = typeof(TestExtensionsTests).Assembly.Location;

        _testExtensions.TestSettingsProviders = new Dictionary<string, TestSettingsProviderPluginInformation>
        {
            { "tsp", new TestSettingsProviderPluginInformation(typeof(TestExtensionsTests)) },
            { "tsp1", new TestSettingsProviderPluginInformation(typeof(TestExtensions)) }
        };

        var extensions = _testExtensions.GetExtensionsDiscoveredFromAssembly(assemblyLocation);

        var expectedExtensions = new Dictionary<string, TestSettingsProviderPluginInformation>
        {
            { "tsp", new TestSettingsProviderPluginInformation(typeof(TestExtensionsTests)) }
        };
        CollectionAssert.AreEqual(expectedExtensions.Keys, extensions!.TestSettingsProviders!.Keys);
    }

    [TestMethod]
    public void GetExtensionsDiscoveredFromAssemblyShouldReturnTestLoggers()
    {
        var assemblyLocation = typeof(TestExtensionsTests).Assembly.Location;

        _testExtensions.TestLoggers = new Dictionary<string, TestLoggerPluginInformation>
        {
            { "tl", new TestLoggerPluginInformation(typeof(TestExtensionsTests)) },
            { "tl1", new TestLoggerPluginInformation(typeof(TestExtensions)) }
        };

        var extensions = _testExtensions.GetExtensionsDiscoveredFromAssembly(assemblyLocation);

        var expectedExtensions = new Dictionary<string, TestLoggerPluginInformation>
        {
            { "tl", new TestLoggerPluginInformation(typeof(TestExtensionsTests)) }
        };
        CollectionAssert.AreEqual(expectedExtensions.Keys, extensions!.TestLoggers!.Keys);
    }

    [TestMethod]
    public void GetExtensionsDiscoveredFromAssemblyShouldReturnTestDiscoveresAndLoggers()
    {
        var assemblyLocation = typeof(TestExtensionsTests).Assembly.Location;

        _testExtensions.TestDiscoverers = new Dictionary<string, TestDiscovererPluginInformation>
        {
            { "td", new TestDiscovererPluginInformation(typeof(TestExtensionsTests)) }
        };

        _testExtensions.TestLoggers = new Dictionary<string, TestLoggerPluginInformation>
        {
            { "tl", new TestLoggerPluginInformation(typeof(TestExtensionsTests)) }
        };

        var extensions = _testExtensions.GetExtensionsDiscoveredFromAssembly(assemblyLocation);

        var expectedDiscoverers = new Dictionary<string, TestDiscovererPluginInformation>
        {
            { "td", new TestDiscovererPluginInformation(typeof(TestExtensionsTests)) }
        };
        CollectionAssert.AreEqual(expectedDiscoverers.Keys, extensions!.TestDiscoverers!.Keys);

        var expectedLoggers = new Dictionary<string, TestLoggerPluginInformation>
        {
            { "tl", new TestLoggerPluginInformation(typeof(TestExtensionsTests)) }
        };
        CollectionAssert.AreEqual(expectedLoggers.Keys, extensions.TestLoggers!.Keys);
    }

    [TestMethod]
    public void MergedDictionaryOfEmptyDictionariesShouldBeAnEmptyDictionary()
    {
        var first = new Dictionary<string, HashSet<string>>();
        var second = new Dictionary<string, HashSet<string>>();
        var merged = TestExtensions.CreateMergedDictionary(first, second);

        // Merging two empty dictionaries should result in an empty dictionary.
        merged.Should().HaveCount(0);

        // Make sure the method is "pure" and returns a new reference.
        merged.Should().NotBeSameAs(first);
        merged.Should().NotBeSameAs(second);
    }

    [TestMethod]
    public void MergedDictionaryOfOneEmptyAndOneNonEmptyDictionaryShouldContainAllTheItemsOfTheNonEmptyDictionary()
    {
        var first = new Dictionary<string, HashSet<string>>();
        var second = new Dictionary<string, HashSet<string>>
        {
            { "aaa", new HashSet<string>() }
        };

        var merged1 = TestExtensions.CreateMergedDictionary(first, second);
        var merged2 = TestExtensions.CreateMergedDictionary(second, first);

        // Merging one empty dictionary with a not empty one should result in a not empty
        // dictionary.
        merged1.Should().HaveCount(1);
        merged2.Should().HaveCount(1);
        merged1.Should().ContainKey("aaa");
        merged2.Should().ContainKey("aaa");

        // Make sure the method stays "pure" and returns a new reference regardless of the input.
        merged1.Should().NotBeSameAs(first);
        merged1.Should().NotBeSameAs(second);
        merged2.Should().NotBeSameAs(first);
        merged2.Should().NotBeSameAs(second);
        merged1.Should().NotBeSameAs(merged2);
    }

    [TestMethod]
    public void MergedDictionaryShouldBeEquivalentToTheExpectedDictionary()
    {
        var first = new Dictionary<string, HashSet<string>>
        {
            // Merged with "key1" from the next set.
            { "key1", new HashSet<string>(new List<string>() { "ext1", "ext2", "ext3" }) },
            // Empty hashset, will be removed from the result.
            { "key2", new HashSet<string>() },
            // Added as is.
            { "key5", new HashSet<string>(new List<string>() { "ext1", "ext2" }) }
        };
        var second = new Dictionary<string, HashSet<string>>
        {
            // Merged with "key1" from the previous set.
            { "key1", new HashSet<string>(new List<string>() { "ext2", "ext3", "ext3", "ext4", "ext5" }) },
            // Empty hashset, will be removed from the result.
            { "key2", new HashSet<string>() },
            // Empty hashset, will be removed from the result.
            { "key3", new HashSet<string>() },
            // Added as is.
            { "key4", new HashSet<string>(new List<string>() { "ext1" }) }
        };
        var expected = new Dictionary<string, HashSet<string>>
        {
            { "key1", new HashSet<string>(new List<string>() { "ext1", "ext2", "ext3", "ext4", "ext5" }) },
            { "key4", new HashSet<string>(new List<string>() { "ext1" }) },
            { "key5", new HashSet<string>(new List<string>() { "ext1", "ext2" }) }
        };

        // Merge the two dictionaries.
        var merged = TestExtensions.CreateMergedDictionary(first, second);

        // Make sure the merged dictionary has the exact same keys as the expected dictionary.
        merged.Should().HaveCount(expected.Count);
        merged.Should().ContainKeys(expected.Keys);
        expected.Should().ContainKeys(merged.Keys);

        // Make sure the hashsets for each key are equal.
        merged.Values.Should().BeEquivalentTo(expected.Values);
    }

    [TestMethod]
    public void AddExtensionTelemetryShouldAddJsonFormattedDiscoveredExtensionsTelemetry()
    {
        var telemetryData = new Dictionary<string, object>();
        var extensions = new Dictionary<string, HashSet<string>>
        {
            { "key1", new HashSet<string>(new List<string>() { "ext1", "ext2", "ext3", "ext4", "ext5" }) },
            { "key4", new HashSet<string>(new List<string>() { "ext1" }) },
            { "key5", new HashSet<string>(new List<string>() { "ext1", "ext2" }) }
        };

        var expectedTelemetry =
            @"{""key1"":[""ext1"",""ext2"",""ext3"",""ext4"",""ext5""],"
            + @"""key4"":[""ext1""],"
            + @"""key5"":[""ext1"",""ext2""]}";

        TestExtensions.AddExtensionTelemetry(telemetryData, extensions);

        telemetryData.Should().ContainKey(TelemetryDataConstants.DiscoveredExtensions);
        telemetryData[TelemetryDataConstants.DiscoveredExtensions].Should().BeEquivalentTo(expectedTelemetry);
    }
}
