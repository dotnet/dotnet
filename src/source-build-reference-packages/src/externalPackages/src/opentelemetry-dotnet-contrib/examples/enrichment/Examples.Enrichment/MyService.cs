// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

namespace Examples.Enrichment;

internal sealed class MyService : IMyService
{
    private readonly List<string> statuses =
    [
        "Blocked",
        "No blockers",
        "Out of office"
    ];

    /// <summary>
    /// Returns daily status.
    /// </summary>
    /// <returns>A tuple with service name and status.</returns>
    public (string Service, string Status) MyDailyStatus()
    {
        var statusNumber = Random.Shared.Next(0, this.statuses.Count);
        return new(nameof(MyService), this.statuses[statusNumber]);
    }
}
