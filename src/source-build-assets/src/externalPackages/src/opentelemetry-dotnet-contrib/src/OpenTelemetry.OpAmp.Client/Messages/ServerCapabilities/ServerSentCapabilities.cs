// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

namespace OpenTelemetry.OpAmp.Client.Messages;

/// <summary>
/// Indicates the capabilities supported by the server.
/// </summary>
[Flags]
#pragma warning disable CA1028 // Enum Storage should be Int32. ServerToAgent is using ulong backing.
public enum ServerSentCapabilities : ulong
#pragma warning restore CA1028 // Enum Storage should be Int32
{
    /// <summary>
    /// The capabilities field is unspecified.
    /// </summary>
    None = 0,

    /// <summary>
    /// The Server can accept status reports. This bit MUST be set, since all Server
    /// MUST be able to accept status reports.
    /// </summary>
    AcceptsStatus = 1,

    /// <summary>
    /// The Server can offer remote configuration to the Agent.
    /// </summary>
    OffersRemoteConfig = 2,

    /// <summary>
    /// The Server can accept EffectiveConfig in AgentToServer.
    /// </summary>
    AcceptsEffectiveConfig = 4,

    /// <summary>
    /// The Server can offer Packages.
    /// </summary>
    OffersPackages = 8,

    /// <summary>
    /// The Server can accept Packages status.
    /// </summary>
    AcceptsPackagesStatus = 16,

    /// <summary>
    /// The Server can offer connection settings.
    /// </summary>
    OffersConnectionSettings = 32,

    /// <summary>
    /// The Server can accept ConnectionSettingsRequest and respond with an offer.
    /// </summary>
    AcceptsConnectionSettingsRequest = 64,
}
