// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using System.Web;

namespace OpenTelemetry.Instrumentation.AspNet;

/// <summary>
/// Options for metric instrumentation.
/// </summary>
public sealed class AspNetMetricsInstrumentationOptions
{
    /// <summary>
    /// Delegate for enrichment of recorded metric with additional tags.
    /// </summary>
    /// <param name="context"><see cref="HttpContext"/>: the HttpContext object. Both Request and Response are available.</param>
    /// <param name="tags"><see cref="TagList"/>: List of current tags. You can add additional tags to this list. </param>
    public delegate void EnrichWithHttpContextAction(HttpContextBase context, ref TagList tags);

    /// <summary>
    /// Gets or sets a delegate to enrich a recorded metric with additional custom tags.
    /// </summary>
    public EnrichWithHttpContextAction? EnrichWithHttpContext { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether emit `server.address` and `server.port` attributes.
    /// </summary>
    internal bool EnableServerAttributesForRequestDuration { get; set; }
}
