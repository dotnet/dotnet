// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.TestPlatform.Extensions.EventLogCollector;

/// <summary>
/// Constants used by Event Log Data Collector.
/// </summary>
internal static class EventLogConstants
{
    // Supported configuration setting names
    public const string SettingEventLogs = "EventLogs";
    public const string SettingEventSources = "EventSources";
    public const string SettingEntryTypes = "EntryTypes";
    public const string SettingMaxEntries = "MaxEventLogEntriesToCollect";

    // default values
    public const int DefaultMaxEntries = 50000;

    public const int TypeColumnMaxLength = 64;
    public const int SourceColumnMaxLength = 212;
}
