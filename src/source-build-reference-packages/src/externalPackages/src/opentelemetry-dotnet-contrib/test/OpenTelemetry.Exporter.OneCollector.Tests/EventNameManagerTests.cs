// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Collections;
using System.Text;
using Xunit;

namespace OpenTelemetry.Exporter.OneCollector.Tests;

public class EventNameManagerTests
{
    [Theory]
    [InlineData("Company.Product")]
    [InlineData("Company")]
    [InlineData("company.product")]
    [InlineData("company99.1product")]
    [InlineData("c")]
    public void ValidEventNamespaceTest(string eventNamespace)
    {
        Assert.True(EventNameManager.IsEventNamespaceValid(eventNamespace));
    }

    [Theory]
    [InlineData("9")]
    [InlineData("Company..Product")]
    [InlineData("Company.")]
    [InlineData(".Company")]
    [InlineData("")]
    public void InvalidEventNamespaceTest(string eventNamespace)
    {
        Assert.False(EventNameManager.IsEventNamespaceValid(eventNamespace));
    }

    [Theory]
    [InlineData("Opened")]
    [InlineData("closed")]
    [InlineData("c")]
    [InlineData("event9")]
    public void ValidEventNameTest(string eventNamespace)
    {
        Assert.True(EventNameManager.IsEventNameValid(eventNamespace));
    }

    [Theory]
    [InlineData("9")]
    [InlineData("Some.Event")]
    [InlineData("Event.")]
    [InlineData(".Event")]
    [InlineData("")]
    public void InvalidEventNameTest(string eventNamespace)
    {
        Assert.False(EventNameManager.IsEventNameValid(eventNamespace));
    }

    [Theory]
    [InlineData(null, null, "DefaultNamespace.DefaultName")]
    [InlineData("myNamespace", null, "MyNamespace.DefaultName")]
    [InlineData(null, "myEvent", "DefaultNamespace.MyEvent")]
    [InlineData("", " ", "DefaultNamespace.DefaultName")]
    [InlineData("9", "[]", "DefaultNamespace.DefaultName")]
    public void DefaultEventNamespaceAndNameUsedToGenerateFullNameTest(string? eventNamespace, string? eventName, string expectedEventFullName)
    {
        var eventNameManager = BuildEventNameManagerWithDefaultOptions();

        var resolveEventFullName = eventNameManager.ResolveEventFullName(eventNamespace, eventName);

        Assert.Equal(Encoding.ASCII.GetBytes($"\"{expectedEventFullName}\""), resolveEventFullName.EventFullName);
    }

    [Fact]
    public void DefaultEventNamespaceAndNameUsedToGenerateFullNameLengthTest()
    {
        var eventNameManager = BuildEventNameManagerWithDefaultOptions();

        var resolveEventFullName = eventNameManager.ResolveEventFullName("N", "N");

        Assert.Equal(Encoding.ASCII.GetBytes("\"DefaultNamespace.DefaultName\""), resolveEventFullName.EventFullName);

        resolveEventFullName = eventNameManager.ResolveEventFullName(new string('N', 99), "N");

        Assert.Equal(Encoding.ASCII.GetBytes("\"DefaultNamespace.DefaultName\""), resolveEventFullName.EventFullName);

        resolveEventFullName = eventNameManager.ResolveEventFullName("N", new string('N', 99));

        Assert.Equal(Encoding.ASCII.GetBytes("\"DefaultNamespace.DefaultName\""), resolveEventFullName.EventFullName);
    }

    [Fact]
    public void EventNameCacheTest()
    {
        var eventNameManager = BuildEventNameManagerWithDefaultOptions();

        Assert.Empty(eventNameManager.EventNamespaceCache);

        eventNameManager.ResolveEventFullName("Test", "Test");

        Assert.Single(eventNameManager.EventNamespaceCache);
        Assert.Single((eventNameManager.EventNamespaceCache["Test"] as Hashtable)!);

        eventNameManager.ResolveEventFullName("test", "test");

        Assert.Single(eventNameManager.EventNamespaceCache);
        Assert.Single((eventNameManager.EventNamespaceCache["Test"] as Hashtable)!);
    }

    [Fact]
    public void EventFullNameCacheTest()
    {
        var eventNameManager = BuildEventNameManagerWithDefaultOptions();

        Assert.Empty(eventNameManager.EventFullNameCache);

        eventNameManager.ResolveEventFullName("Company_Product_EventName");

        Assert.Single(eventNameManager.EventFullNameCache);

        eventNameManager.ResolveEventFullName("company_product_eventName");

        Assert.Single(eventNameManager.EventFullNameCache);
    }

    [Fact]
    public void EventFullNameMappedWhenEventNamespaceMatchesTest()
    {
        var eventNameManager = BuildEventNameManagerWithEventFullNameMappings(
            new("*", "WildcardEventName"),
            new("MyNamespace", "NewEventName1"),
            new("mynamespace.match.in.full.MyEventName", "NewEventName2"));

        var resolveEventFullName = eventNameManager.ResolveEventFullName("MyNamespace.Match.In.Full", "MyEventName");

        Assert.Equal(Encoding.ASCII.GetBytes("\"NewEventName2\""), resolveEventFullName.EventFullName);
    }

    [Fact]
    public void EventFullNameMappedWhenEventNamespaceStartsWithPrefixTest()
    {
        var eventNameManager = BuildEventNameManagerWithEventFullNameMappings(
            new("*", "WildcardEventName"),
            new("MyNamespace", "NewEventName1"),
            new("MyNamespace.NonMatch", "NewEventName2"),
            new("MyNamespace.MyChild", "NewEventName3"),
            new("mynamespace.mychild.namesp", "NewEventName4"));

        var resolveEventFullName = eventNameManager.ResolveEventFullName("MyNamespace.MyChild.Namespace", "MyEventName");

        Assert.Equal(Encoding.ASCII.GetBytes("\"NewEventName4\""), resolveEventFullName.EventFullName);
    }

    [Fact]
    public void EventFullNameMappedUsingDefaultRuleTest()
    {
        var eventNameManager = BuildEventNameManagerWithEventFullNameMappings(
            new("MyNamespace1", "NewEventName1"),
            new("MyNamespace2", "NewEventName2"),
            new("*", "defaultEventName"));

        var resolveEventFullName = eventNameManager.ResolveEventFullName("MyNamespace", "MyEventName");

        Assert.Equal(Encoding.ASCII.GetBytes("\"DefaultEventName\""), resolveEventFullName.EventFullName);
    }

    [Theory]
    [InlineData("DefaultNamespace")]
    [InlineData("")]
    public void EventFullNameMappedUsingDefaultsWhenNoDefaultRuleDefinedTest(string defaultNamespace)
    {
        var eventNameManager = BuildEventNameManagerWithEventFullNameMappings(
            defaultNamespace,
            [
                new("MyNamespace1", "NewEventName1"),
                new("MyNamespace2", "NewEventName2")
            ]);

        var resolveEventFullName = eventNameManager.ResolveEventFullName("MyNamespace", "MyEventName");

        Assert.Equal(Encoding.ASCII.GetBytes($"\"{(defaultNamespace.Length > 0 ? $"{defaultNamespace}." : string.Empty)}DefaultName\""), resolveEventFullName.EventFullName);
    }

    [Fact]
    public void EventFullNameMappedUsingPassthroughTest()
    {
        var eventNameManager = BuildEventNameManagerWithEventFullNameMappings(
        [
            new("*", "*")
        ]);

        var resolveEventFullName = eventNameManager.ResolveEventFullName("MyNamespace", "MyEventName");

        Assert.Equal(Encoding.ASCII.GetBytes("\"MyNamespace.MyEventName\""), resolveEventFullName.EventFullName);
    }

    private static EventNameManager BuildEventNameManagerWithDefaultOptions()
    {
        return new EventNameManager("defaultNamespace", "defaultName");
    }

    private static EventNameManager BuildEventNameManagerWithEventFullNameMappings(
        params KeyValuePair<string, string>[] mappings)
    {
        return BuildEventNameManagerWithEventFullNameMappings(
            "defaultNamespace",
            mappings);
    }

    private static EventNameManager BuildEventNameManagerWithEventFullNameMappings(
        string defaultNamespace,
        KeyValuePair<string, string>[] mappings)
    {
        var options = new OneCollectorLogExporterOptions()
        {
            ConnectionString = "InstrumentationKey=token-extrainformation",
            EventFullNameMappings = mappings.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value),
        };

        options.Validate();

        return new EventNameManager(
            defaultNamespace,
            "defaultName",
            eventFullNameMappings: options.ParsedEventFullNameMappings);
    }
}
