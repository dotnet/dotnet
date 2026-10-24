// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

public class SnapshotTest
{
    [Theory]
    [InlineData(true, 42)]
    [InlineData(false, -1)]
    public void Shadow_values_factory_reads_dictionary_indexer_or_sentinel(bool includeValue, int expected)
    {
        var modelBuilder = InMemoryTestHelpers.Instance.CreateConventionBuilder();
        modelBuilder.Entity<ShadowEntity>().Property<int>("Value").HasSentinel(-1);
        var entityType = (IRuntimeEntityType)modelBuilder.FinalizeModel().FindEntityType(typeof(ShadowEntity))!;
        IDictionary<string, object?> values = new Dictionary<string, object?>();
        if (includeValue)
        {
            values["Value"] = 42;
        }

        var snapshot = ShadowValuesFactoryFactory.Instance.Create(entityType)(values);

        Assert.Equal(expected, snapshot.GetValue<int>(entityType.FindProperty("Value")!.GetShadowIndex()));
    }

    [Fact]
    public void SetValue_sets_value()
    {
        var snapshot = new Snapshot<int, string>(1, "A");

        snapshot.SetValue(0, 2);
        snapshot.SetValue(1, "B");

        Assert.Equal(2, snapshot.GetValue<int>(0));
        Assert.Equal("B", snapshot.GetValue<string>(1));
    }

    [Fact]
    public void SetValue_sets_value_on_multi_snapshot()
    {
        var snapshot = new MultiSnapshot(
        [
            new Snapshot<int>(1),
            new Snapshot<string>("A")
        ]);

        snapshot.SetValue(0, 2);
        snapshot.SetValue(Snapshot.MaxGenericTypes, "B");

        Assert.Equal(2, snapshot.GetValue<int>(0));
        Assert.Equal("B", snapshot.GetValue<string>(Snapshot.MaxGenericTypes));
    }

    [Fact]
    public void SetValue_throws_for_empty_snapshot()
        => Assert.Throws<IndexOutOfRangeException>(() => Snapshot.Empty.SetValue(0, "A"));

    private class ShadowEntity
    {
        public int Id { get; set; }
    }
}
