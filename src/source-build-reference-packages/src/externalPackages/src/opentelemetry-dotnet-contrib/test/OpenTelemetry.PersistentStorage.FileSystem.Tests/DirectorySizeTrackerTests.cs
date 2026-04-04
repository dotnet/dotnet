// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Xunit;

namespace OpenTelemetry.PersistentStorage.FileSystem.Tests;

public class DirectorySizeTrackerTests
{
    [Fact]
    public void VerifyDirectorySizeTracker()
    {
        var testDirectory = new DirectoryInfo(Path.Combine(Path.GetTempPath(), Path.GetRandomFileName()));
        testDirectory.Create();

        var directorySizeTracker = new DirectorySizeTracker(maxSizeInBytes: 100, path: testDirectory.FullName);

        // new directory, expected to have space
        Assert.True(directorySizeTracker.IsSpaceAvailable(out var currentSize1));
        Assert.Equal(0, currentSize1);

        // add a file and check current space.
        directorySizeTracker.FileAdded(30);
        Assert.True(directorySizeTracker.IsSpaceAvailable(out var currentSize2));
        Assert.Equal(30, currentSize2);

        // add a file and check current space. Here we've exceeded the configured max size
        directorySizeTracker.FileAdded(100);
        Assert.False(directorySizeTracker.IsSpaceAvailable(out var currentSize3));
        Assert.Equal(130, currentSize3);

        // remove a file and check current space.
        directorySizeTracker.FileRemoved(50);
        Assert.True(directorySizeTracker.IsSpaceAvailable(out var currentSize4));
        Assert.Equal(80, currentSize4);

        // since we haven't actually written any files to disk,
        // Recount will reset to zero.
        directorySizeTracker.RecountCurrentSize();
        Assert.True(directorySizeTracker.IsSpaceAvailable(out var currentSize5));
        Assert.Equal(0, currentSize5);

        // cleanup
        testDirectory.Delete();
    }
}
