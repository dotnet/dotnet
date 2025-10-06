// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Xunit;
using static Microsoft.AspNetCore.Razor.Test.Common.ProjectSystem.TestProjectSnapshotManager.Listener;

namespace Microsoft.AspNetCore.Razor.Test.Common.ProjectSystem;

internal static class NotificationAssertions
{
    public static void AssertNoNotifications(this IEnumerable<ProjectChangeEventArgs> notifications)
    {
        Assert.Empty(notifications);
    }

    public static void AssertNotifications(this IEnumerable<ProjectChangeEventArgs> notifications, params Action<Inspector>[] inspectors)
    {
        Assert.Equal(notifications.Count(), inspectors.Length);

        var i = 0;
        foreach (var notification in notifications)
        {
            var inspector = inspectors[i];
            inspector(new(notification));

            i++;
        }
    }
}
