// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using OpenTelemetry.Internal;

namespace OpenTelemetry.Exporter.Instana.Implementation;

internal enum SpanKind
{
#pragma warning disable SA1602 // Enumeration items should be documented
    ENTRY,
#pragma warning restore SA1602 // Enumeration items should be documented
#pragma warning disable SA1602 // Enumeration items should be documented
    EXIT,
#pragma warning restore SA1602 // Enumeration items should be documented
#pragma warning disable SA1602 // Enumeration items should be documented
    INTERMEDIATE,
#pragma warning restore SA1602 // Enumeration items should be documented
#pragma warning disable SA1602 // Enumeration items should be documented
    NOT_SET,
#pragma warning restore SA1602 // Enumeration items should be documented
}

internal class InstanaSpan
{
    private InstanaSpanTransformInfo transformInfo = new();
    private string n = string.Empty;
    private string t = string.Empty;
    private string lt = string.Empty;
    private From f = new();
    private string p = string.Empty;
    private string s = string.Empty;
    private SpanKind k = SpanKind.NOT_SET;
    private long ts;
    private long d;
    private bool tp;
    private int ec;
    private Data data = new()
    {
        data = new Dictionary<string, object>(8),
        Events = new List<SpanEvent>(8),
        Tags = new Dictionary<string, string>(2),
    };

    public InstanaSpanTransformInfo TransformInfo
    {
        get => this.transformInfo;
        set
        {
            Guard.ThrowIfNull(value);
            this.transformInfo = value;
        }
    }

    public string N
    {
        get => this.n;
        internal set
        {
            Guard.ThrowIfNull(value);
            this.n = value;
        }
    }

    public string T
    {
        get => this.t;
        internal set
        {
            Guard.ThrowIfNull(value);
            this.t = value;
        }
    }

    public string Lt
    {
        get => this.lt;
        internal set
        {
            Guard.ThrowIfNull(value);
            this.lt = value;
        }
    }

    public From F
    {
        get => this.f;
        set
        {
            Guard.ThrowIfNull(value);
            this.f = value;
        }
    }

    public string P
    {
        get => this.p;
        internal set
        {
            Guard.ThrowIfNull(value);
            this.p = value;
        }
    }

    public string S
    {
        get => this.s;
        internal set
        {
            Guard.ThrowIfNull(value);
            this.s = value;
        }
    }

    public SpanKind K
    {
        get => this.k;
        internal set
        {
            Guard.ThrowIfNull(value);
            this.k = value;
        }
    }

    public Data Data
    {
        get => this.data;
        internal set
        {
            Guard.ThrowIfNull(value);
            this.data = value;
        }
    }

    public long Ts
    {
        get => this.ts;
        internal set
        {
            Guard.ThrowIfNull(value);
            this.ts = value;
        }
    }

    public long D
    {
        get => this.d;
        internal set
        {
            Guard.ThrowIfNull(value);
            this.d = value;
        }
    }

    public bool Tp
    {
        get => this.tp;
        internal set
        {
            Guard.ThrowIfNull(value);
            this.tp = value;
        }
    }

    public int Ec
    {
        get => this.ec;
        internal set
        {
            Guard.ThrowIfNull(value);
            this.ec = value;
        }
    }
}

#pragma warning disable SA1402 // File may only contain a single type
internal class From
#pragma warning restore SA1402 // File may only contain a single type
{
    internal From()
    {
        this.E = string.Empty;
        this.H = string.Empty;
    }

    public string E { get; internal set; }

    public string H { get; internal set; }

    internal bool IsEmpty()
    {
        return string.IsNullOrEmpty(this.E) && string.IsNullOrEmpty(this.H);
    }
}

#pragma warning disable SA1402 // File may only contain a single type
internal class Data
#pragma warning restore SA1402 // File may only contain a single type
{
    private List<SpanEvent> events = new(8);
    private Dictionary<string, object> dataField = new(8);
    private Dictionary<string, string> tags = new(2);

#pragma warning disable SA1300 // Element should begin with upper-case letter

    public Dictionary<string, object> data
    {
        get => this.dataField;
        internal set
        {
            Guard.ThrowIfNull(value);
            this.dataField = value;
        }
    }

#pragma warning restore SA1300 // Element should begin with upper-case letter

    public Dictionary<string, string> Tags
    {
        get => this.tags;
        internal set
        {
            Guard.ThrowIfNull(value);
            this.tags = value;
        }
    }

    public List<SpanEvent> Events
    {
        get => this.events;
        internal set
        {
            Guard.ThrowIfNull(value);
            this.events = value;
        }
    }
}

#pragma warning disable SA1402 // File may only contain a single type
internal class SpanEvent
#pragma warning restore SA1402 // File may only contain a single type
{
    private string name = string.Empty;
    private Dictionary<string, string> tags = [];

    public string Name
    {
        get => this.name;
        internal set
        {
            Guard.ThrowIfNull(value);
            this.name = value;
        }
    }

    public long Ts { get; internal set; }

    public Dictionary<string, string> Tags
    {
        get => this.tags;
        internal set
        {
            Guard.ThrowIfNull(value);
            this.tags = value;
        }
    }
}
