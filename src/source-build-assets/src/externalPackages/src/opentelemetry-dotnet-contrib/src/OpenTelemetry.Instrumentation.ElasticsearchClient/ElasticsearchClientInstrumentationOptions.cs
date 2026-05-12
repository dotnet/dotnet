// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;

namespace OpenTelemetry.Instrumentation.ElasticsearchClient;

/// <summary>
/// Options for Elasticsearch client instrumentation.
/// </summary>
public class ElasticsearchClientInstrumentationOptions
{
    /// <summary>
    /// Gets or sets a value indicating whether down stream instrumentation (HttpClient) is suppressed (disabled).
    /// </summary>
    public bool SuppressDownstreamInstrumentation { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the JSON request body should be parsed out of the request debug information and formatted as indented JSON.
    /// </summary>
    public bool ParseAndFormatRequest { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether or not the OpenTelemetry.Instrumentation.ElasticsearchClient
    /// should add the request information as db.statement attribute tag. Default value: True.
    /// </summary>
    public bool SetDbStatementForRequest { get; set; } = true;

    /// <summary>
    /// Gets or sets a max length allowed for the db.statement attribute. Default value: 4096.
    /// </summary>
    public int MaxDbStatementLength { get; set; } = 4096;

    /// <summary>
    /// Gets or sets an action to enrich an Activity.
    /// </summary>
    /// <remarks>
    /// <para><see cref="Activity"/>: the activity being enriched.</para>
    /// <para>string: the name of the event.</para>
    /// <para>object: the raw object from which additional information can be extracted to enrich the activity.
    /// The type of this object depends on the event, which is given by the above parameter.</para>
    /// </remarks>
    public Action<Activity, string, object?>? Enrich { get; set; }
}
