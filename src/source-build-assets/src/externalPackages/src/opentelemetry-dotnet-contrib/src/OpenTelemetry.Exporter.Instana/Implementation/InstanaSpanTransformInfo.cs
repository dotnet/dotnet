// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using OpenTelemetry.Internal;

namespace OpenTelemetry.Exporter.Instana.Implementation;

internal class InstanaSpanTransformInfo
{
    private string statusCode = string.Empty;
    private string statusDesc = string.Empty;

    public string StatusCode
    {
        get => this.statusCode;
        set
        {
            Guard.ThrowIfNull(value);
            this.statusCode = value;
        }
    }

    public string StatusDesc
    {
        get => this.statusDesc;
        set
        {
            Guard.ThrowIfNull(value);
            this.statusDesc = value;
        }
    }

    public bool HasExceptionEvent { get; internal set; }

    public bool IsEntrySpan { get; internal set; }
}
