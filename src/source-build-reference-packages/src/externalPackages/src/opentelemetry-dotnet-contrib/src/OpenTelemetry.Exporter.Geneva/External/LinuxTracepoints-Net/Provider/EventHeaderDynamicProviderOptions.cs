#nullable enable

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.LinuxTracepoints.Provider;

using System;

/// <summary>
/// Options used when creating an <see cref="EventHeaderDynamicProvider"/>.
/// </summary>
internal class EventHeaderDynamicProviderOptions
{
    private string groupName = "";

    /// <summary>
    /// Gets or sets the provider group name. This name will appear in the "G"
    /// suffix of the tracepoint names, e.g. a group name of "mygroup" would
    /// result in a tracepoint name like "MyProvider_L5K1fGmygroup".
    /// <br/>
    /// The group name must not be null and may contain only lowercase ASCII
    /// letters ('a'..'z') and ASCII digits ('0'..'9'). Default group is "",
    /// resulting in no "G" suffix.
    /// </summary>
    public string GroupName
    {
        get
        {
            return this.groupName;
        }

        set
        {
            foreach (var ch in value)
            {
                if ((ch < 'a' || ch > 'z') && (ch < '0' || ch > '9'))
                {
                    throw new ArgumentException(
                        "Invalid char '" + ch + "' in " + nameof(GroupName) + " (must be 'a'..'z' or '0'..'9').",
                        nameof(GroupName));
                }
            }

            this.groupName = value;
        }
    }
}
