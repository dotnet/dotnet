// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#if NETFRAMEWORK
using System.Net.Http;
#endif

using OpenTelemetry.Internal;

namespace OpenTelemetry.OpAmp.Client.Settings;

/// <summary>
/// OpAMP client settings.
/// </summary>
public sealed class OpAmpClientSettings
{
    private readonly Func<HttpClient> defaultHttpClientFactory = () => new HttpClient();

    private Uri? serverUrl;

    /// <summary>
    /// Gets or sets the unique identifier for the current client instance.
    /// </summary>
    /// <value>
    /// A <see cref="Guid"/> that uniquely identifies this instance.
    /// By default:
    /// <list type="bullet">
    ///   <item>
    ///     <description>On .NET 9.0 or greater: initialized with a Version 7 GUID.</description>
    ///   </item>
    ///   <item>
    ///     <description>On earlier versions: initialized with a randomly generated GUID.</description>
    ///   </item>
    /// </list>
    /// </value>
    public Guid InstanceUid { get; set; }
#if NET9_0_OR_GREATER
        = Guid.CreateVersion7();
#else
        = Guid.NewGuid();
#endif

    /// <summary>
    /// Gets or sets the type of connection used to communicate with the server
    /// (for example, HTTP or WebSocket).
    /// </summary>
    /// <value>
    /// A <see cref="ConnectionType"/> value that specifies the transport protocol.
    /// The default is <see cref="ConnectionType.Http"/>.
    /// </value>
    public ConnectionType ConnectionType { get; set; } = ConnectionType.Http;

    /// <summary>
    /// Gets or sets the server URL to connect to.
    /// </summary>
    /// <value>
    /// The <see cref="Uri"/> representing the server endpoint.
    /// If not explicitly set, a default URL is returned based on the <see cref="ConnectionType"/>:
    /// <list type="bullet">
    ///   <item>
    ///     <description><see cref="ConnectionType.Http"/> -> <c>https://localhost:4318/v1/opamp</c></description>
    ///   </item>
    ///   <item>
    ///     <description><see cref="ConnectionType.WebSocket"/> -> <c>wss://localhost:4318/v1/opamp</c></description>
    ///   </item>
    /// </list>
    /// </value>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the <see cref="ConnectionType"/> is not recognized.
    /// </exception>
    public Uri ServerUrl
    {
        get
        {
            if (this.serverUrl != null)
            {
                return this.serverUrl;
            }

            switch (this.ConnectionType)
            {
                case ConnectionType.Http:
                    return new("https://localhost:4318/v1/opamp");
                case ConnectionType.WebSocket:
                    return new("wss://localhost:4318/v1/opamp");
                default:
                    throw new InvalidOperationException("Unknown connection type");
            }
        }
        set => this.serverUrl = value;
    }

    /// <summary>
    /// Gets or sets the identification settings.
    /// </summary>
    public IdentificationSettings Identification { get; set; } = new();

    /// <summary>
    /// Gets or sets the heartbeat settings.
    /// </summary>
    public HeartbeatSettings Heartbeat { get; set; } = new();

    /// <summary>
    /// Gets or sets the remote configuration settings.
    /// </summary>
    public RemoteConfigSettings RemoteConfiguration { get; set; } = new();

    /// <summary>
    /// Gets or sets the factory function called to create the <see
    /// cref="HttpClient"/> instance that will be used at runtime to
    /// transmit OpAmp messages over HTTP. The returned instance will
    /// be reused for all communication.
    /// </summary>
    /// <remarks>
    /// Notes:
    /// <list type="bullet">
    /// <item>This is only invoked for the <see
    /// cref="ConnectionType.Http"/> protocol.</item>
    /// </list>
    /// </remarks>
    public Func<HttpClient> HttpClientFactory
    {
        get => field ?? this.defaultHttpClientFactory;
        set
        {
            Guard.ThrowIfNull(value);
            field = value;
        }
    }
}
