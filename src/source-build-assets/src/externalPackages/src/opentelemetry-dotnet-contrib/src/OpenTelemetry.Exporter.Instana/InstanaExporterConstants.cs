// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

namespace OpenTelemetry.Exporter.Instana;

internal class InstanaExporterConstants
{
#pragma warning disable SA1310 // Field names should not contain underscore
    internal const string OTEL_SPAN_TYPE = "otel";
    internal const string KIND_FIELD = "kind";
    internal const string SERVER_KIND = "server";
    internal const string CLIENT_KIND = "client";
    internal const string PRODUCER_KIND = "producer";
    internal const string CONSUMER_KIND = "consumer";
    internal const string INTERNAL_KIND = "internal";
    internal const string SERVICE_FIELD = "service";
    internal const string OPERATION_FIELD = "operation";
    internal const string TRACE_STATE_FIELD = "trace_state";
    internal const string ERROR_FIELD = "error";
    internal const string ERROR_DETAIL_FIELD = "error_detail";
    internal const string EXCEPTION_FIELD = "exception";
    internal const string TAGS_FIELD = "tags";
    internal const string EVENTS_FIELD = "events";
    internal const string EVENT_NAME_FIELD = "name";
    internal const string EVENT_TIMESTAMP_FIELD = "ts";

    internal const string ENVVAR_INSTANA_ENDPOINT_URL = "INSTANA_ENDPOINT_URL";
    internal const string ENVVAR_INSTANA_AGENT_KEY = "INSTANA_AGENT_KEY";
    internal const string ENVVAR_INSTANA_TIMEOUT = "INSTANA_TIMEOUT";
    internal const int BACKEND_DEFAULT_TIMEOUT = 20000;
    internal const string ENVVAR_INSTANA_EXTRA_HTTP_HEADERS = "INSTANA_EXTRA_HTTP_HEADERS";
    internal const string ENVVAR_INSTANA_ENDPOINT_PROXY = "INSTANA_ENDPOINT_PROXY";
#pragma warning restore SA1310 // Field names should not contain underscore
}
