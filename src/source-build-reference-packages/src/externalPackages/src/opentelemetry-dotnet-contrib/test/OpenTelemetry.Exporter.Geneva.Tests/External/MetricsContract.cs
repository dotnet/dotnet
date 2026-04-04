using Kaitai;

namespace OpenTelemetry.Exporter.Geneva.Tests;

public partial class MetricsContract : KaitaiStruct
{
    public static MetricsContract FromFile(string fileName)
    {
        return new MetricsContract(new KaitaiStream(fileName));
    }


    public enum MetricEventType
    {
        Old = 0,
        Uint64Metric = 50,
        DoubleScaledToLongMetric = 51,
        BatchMetric = 52,
        ExternallyAggregatedUlongMetric = 53,
        ExternallyAggregatedDoubleMetric = 54,
        DoubleMetric = 55,
        ExternallyAggregatedUlongDistributionMetric = 56,
        ExternallyAggregatedDoubleDistributionMetric = 57,
        ExternallyAggregatedDoubleScaledToLongDistributionMetric = 58,
        Tlv = 70,
    }

    public enum DistributionType
    {
        Bucketed = 0,
        MonBucketed = 1,
        ValueCountPairs = 2,
    }

    public enum PayloadTypes
    {
        AccountName = 1,
        NamespaceName = 2,
        MetricName = 3,
        Dimensions = 4,
        SingleUint64Value = 5,
        SingleDoubleValue = 6,
        SingleDoubleScaledToUint64Value = 7,
        ExtAggregatedUint64Value = 8,
        ExtAggregatedDoubleValue = 9,
        ExtAggregatedDoubleScaledToUint64Value = 10,
        HistogramUint16Bucketed = 11,
        HistogramUint64ValueCountPairs = 12,
        HistogramDoubleScaledToUint64ValueCountPairs = 13,
        ReservedForDoubleHistogram = 14,
        Exemplars = 15,
    }
    public MetricsContract(KaitaiStream p__io, KaitaiStruct p__parent = null, MetricsContract p__root = null) : base(p__io)
    {
        m_parent = p__parent;
        m_root = p__root ?? this;
        f_eventType = false;
        _read();
    }
    private void _read()
    {
        _eventId = m_io.ReadU2le();
        _lenBody = m_io.ReadU2le();
        switch (EventType)
        {
            case MetricEventType.Tlv:
                {
                    __raw_body = m_io.ReadBytes(LenBody);
                    var io___raw_body = new KaitaiStream(__raw_body);
                    _body = new UserdataV2(io___raw_body, this, m_root);
                    break;
                }
            default:
                {
                    __raw_body = m_io.ReadBytes(LenBody);
                    var io___raw_body = new KaitaiStream(__raw_body);
                    _body = new Userdata(EventId, io___raw_body, this, m_root);
                    break;
                }
        }
    }

    /// <summary>
    /// This type represents "UserData" or "body" portion of Metrics message.
    /// </summary>
    public partial class Userdata : KaitaiStruct
    {
        public Userdata(ushort p_eventId, KaitaiStream p__io, MetricsContract p__parent = null, MetricsContract p__root = null) : base(p__io)
        {
            m_parent = p__parent;
            m_root = p__root;
            _eventId = p_eventId;
            _read();
        }
        private void _read()
        {
            _numDimensions = m_io.ReadU2le();
            _padding = m_io.ReadBytes(2);
            switch (M_Parent.EventType)
            {
                case MetricsContract.MetricEventType.ExternallyAggregatedDoubleScaledToLongDistributionMetric:
                    {
                        _valueSection = new ExtAggregatedDoubleValue(m_io, this, m_root);
                        break;
                    }
                case MetricsContract.MetricEventType.DoubleMetric:
                    {
                        _valueSection = new SingleDoubleValue(m_io, this, m_root);
                        break;
                    }
                case MetricsContract.MetricEventType.ExternallyAggregatedUlongMetric:
                    {
                        _valueSection = new ExtAggregatedUint64Value(m_io, this, m_root);
                        break;
                    }
                case MetricsContract.MetricEventType.ExternallyAggregatedUlongDistributionMetric:
                    {
                        _valueSection = new ExtAggregatedUint64Value(m_io, this, m_root);
                        break;
                    }
                case MetricsContract.MetricEventType.ExternallyAggregatedDoubleDistributionMetric:
                    {
                        _valueSection = new ExtAggregatedDoubleValue(m_io, this, m_root);
                        break;
                    }
                case MetricsContract.MetricEventType.DoubleScaledToLongMetric:
                    {
                        _valueSection = new SingleDoubleValue(m_io, this, m_root);
                        break;
                    }
                case MetricsContract.MetricEventType.Uint64Metric:
                    {
                        _valueSection = new SingleUint64Value(m_io, this, m_root);
                        break;
                    }
                case MetricsContract.MetricEventType.ExternallyAggregatedDoubleMetric:
                    {
                        _valueSection = new ExtAggregatedDoubleValue(m_io, this, m_root);
                        break;
                    }
                case MetricsContract.MetricEventType.Old:
                    {
                        _valueSection = new SingleUint64Value(m_io, this, m_root);
                        break;
                    }
            }
            _metricAccount = new LenString(m_io, this, m_root);
            _metricNamespace = new LenString(m_io, this, m_root);
            _metricName = new LenString(m_io, this, m_root);
            _dimensionsNames = new List<LenString>((int)(NumDimensions));
            for (var i = 0; i < NumDimensions; i++)
            {
                _dimensionsNames.Add(new LenString(m_io, this, m_root));
            }
            _dimensionsValues = new List<LenString>((int)(NumDimensions));
            for (var i = 0; i < NumDimensions; i++)
            {
                _dimensionsValues.Add(new LenString(m_io, this, m_root));
            }
            if (!(M_Io.IsEof))
            {
                _apContainer = new LenString(m_io, this, m_root);
            }
            if (((((M_Parent.EventType == MetricsContract.MetricEventType.ExternallyAggregatedUlongDistributionMetric) || (M_Parent.EventType == MetricsContract.MetricEventType.ExternallyAggregatedDoubleDistributionMetric) || (M_Parent.EventType == MetricsContract.MetricEventType.ExternallyAggregatedDoubleScaledToLongDistributionMetric))) && (!(M_Io.IsEof))))
            {
                _histogram = new Histogram(m_io, this, m_root);
            }
        }
        private ushort _numDimensions;
        private byte[] _padding;
        private KaitaiStruct _valueSection;
        private LenString _metricAccount;
        private LenString _metricNamespace;
        private LenString _metricName;
        private List<LenString> _dimensionsNames;
        private List<LenString> _dimensionsValues;
        private LenString _apContainer;
        private Histogram _histogram;
        private ushort _eventId;
        private MetricsContract m_root;
        private MetricsContract m_parent;

        /// <summary>
        /// Number of dimensions specified in this event.
        /// </summary>
        public ushort NumDimensions { get { return _numDimensions; } }
        public byte[] Padding { get { return _padding; } }

        /// <summary>
        /// Value section of the body, stores fixed numeric metric value(s), as per event type.
        /// </summary>
        public KaitaiStruct ValueSection { get { return _valueSection; } }

        /// <summary>
        /// Geneva Metrics account name to be used for this metric.
        /// </summary>
        public LenString MetricAccount { get { return _metricAccount; } }

        /// <summary>
        /// Geneva Metrics namespace name to be used for this metric.
        /// </summary>
        public LenString MetricNamespace { get { return _metricNamespace; } }

        /// <summary>
        /// Geneva Metrics metric name to be used.
        /// </summary>
        public LenString MetricName { get { return _metricName; } }

        /// <summary>
        /// Dimension names strings ("key" parts of key-value pairs). Must be sorted,
        /// unless MetricsExtenion's option `enableDimensionSortingOnIngestion` is
        /// enabled.
        /// </summary>
        public List<LenString> DimensionsNames { get { return _dimensionsNames; } }

        /// <summary>
        /// Dimension values strings ("value" parts of key-value pairs).
        /// </summary>
        public List<LenString> DimensionsValues { get { return _dimensionsValues; } }

        public LenString ApContainer { get { return _apContainer; } }
        public Histogram Histogram { get { return _histogram; } }

        /// <summary>
        /// Type of message, affects format of the body.
        /// </summary>
        public ushort EventId { get { return _eventId; } }
        public MetricsContract M_Root { get { return m_root; } }
        public MetricsContract M_Parent { get { return m_parent; } }
    }

    /// <summary>
    /// Bucket with an explicitly-defined value coordinate `value`, claiming to
    /// hold `count` hits. Normally used to represent non-linear (e.g. exponential)
    /// histograms payloads.
    /// </summary>
    public partial class PairValueCount : KaitaiStruct
    {
        public static PairValueCount FromFile(string fileName)
        {
            return new PairValueCount(new KaitaiStream(fileName));
        }

        public PairValueCount(KaitaiStream p__io, MetricsContract.HistogramValueCountPairs p__parent = null, MetricsContract p__root = null) : base(p__io)
        {
            m_parent = p__parent;
            m_root = p__root;
            _read();
        }
        private void _read()
        {
            _value = m_io.ReadU8le();
            _count = m_io.ReadU4le();
        }
        private ulong _value;
        private uint _count;
        private MetricsContract m_root;
        private MetricsContract.HistogramValueCountPairs m_parent;
        public ulong Value { get { return _value; } }
        public uint Count { get { return _count; } }
        public MetricsContract M_Root { get { return m_root; } }
        public MetricsContract.HistogramValueCountPairs M_Parent { get { return m_parent; } }
    }
    public partial class ExemplarFlags : KaitaiStruct
    {
        public static ExemplarFlags FromFile(string fileName)
        {
            return new ExemplarFlags(new KaitaiStream(fileName));
        }

        public ExemplarFlags(KaitaiStream p__io, MetricsContract.SingleExemplarBody p__parent = null, MetricsContract p__root = null) : base(p__io)
        {
            m_parent = p__parent;
            m_root = p__root;
            _read();
        }
        private void _read()
        {
            __unnamed0 = m_io.ReadBitsIntBe(3);
            _sampleCountExists = m_io.ReadBitsIntBe(1) != 0;
            _traceIdExists = m_io.ReadBitsIntBe(1) != 0;
            _spanIdExists = m_io.ReadBitsIntBe(1) != 0;
            _isTimestampAvailable = m_io.ReadBitsIntBe(1) != 0;
            _isMetricValueDoubleStoredAsLong = m_io.ReadBitsIntBe(1) != 0;
        }
        private ulong __unnamed0;
        private bool _sampleCountExists;
        private bool _traceIdExists;
        private bool _spanIdExists;
        private bool _isTimestampAvailable;
        private bool _isMetricValueDoubleStoredAsLong;
        private MetricsContract m_root;
        private MetricsContract.SingleExemplarBody m_parent;
        public ulong Unnamed_0 { get { return __unnamed0; } }
        public bool SampleCountExists { get { return _sampleCountExists; } }
        public bool TraceIdExists { get { return _traceIdExists; } }
        public bool SpanIdExists { get { return _spanIdExists; } }
        public bool IsTimestampAvailable { get { return _isTimestampAvailable; } }
        public bool IsMetricValueDoubleStoredAsLong { get { return _isMetricValueDoubleStoredAsLong; } }
        public MetricsContract M_Root { get { return m_root; } }
        public MetricsContract.SingleExemplarBody M_Parent { get { return m_parent; } }
    }
    public partial class SingleUint64ValueV2 : KaitaiStruct
    {
        public static SingleUint64ValueV2 FromFile(string fileName)
        {
            return new SingleUint64ValueV2(new KaitaiStream(fileName));
        }

        public SingleUint64ValueV2(KaitaiStream p__io, MetricsContract.TlvField p__parent = null, MetricsContract p__root = null) : base(p__io)
        {
            m_parent = p__parent;
            m_root = p__root;
            _read();
        }
        private void _read()
        {
            _timestamp = m_io.ReadU8le();
            _value = m_io.ReadU8le();
        }
        private ulong _timestamp;
        private ulong _value;
        private MetricsContract m_root;
        private MetricsContract.TlvField m_parent;

        /// <summary>
        /// Timestamp in Windows FILETIME format, i.e. number of 100 ns ticks passed since 1601-01-01 00:00:00 UTC.
        /// </summary>
        public ulong Timestamp { get { return _timestamp; } }

        /// <summary>
        /// Metric value as 64-bit unsigned integer.
        /// </summary>
        public ulong Value { get { return _value; } }
        public MetricsContract M_Root { get { return m_root; } }
        public MetricsContract.TlvField M_Parent { get { return m_parent; } }
    }
    public partial class WrappedString : KaitaiStruct
    {
        public static WrappedString FromFile(string fileName)
        {
            return new WrappedString(new KaitaiStream(fileName));
        }

        public WrappedString(KaitaiStream p__io, MetricsContract.TlvField p__parent = null, MetricsContract p__root = null) : base(p__io)
        {
            m_parent = p__parent;
            m_root = p__root;
            _read();
        }
        private void _read()
        {
            _value = System.Text.Encoding.GetEncoding("UTF-8").GetString(m_io.ReadBytesFull());
        }
        private string _value;
        private MetricsContract m_root;
        private MetricsContract.TlvField m_parent;
        public string Value { get { return _value; } }
        public MetricsContract M_Root { get { return m_root; } }
        public MetricsContract.TlvField M_Parent { get { return m_parent; } }
    }

    /// <summary>
    /// Payload of a histogram with linear distribution of buckets. Such histogram
    /// is defined by the parameters specified in `min`, `bucket_size` and
    /// `bucket_count`. It is modelled as a series of buckets. First (index 0) and
    /// last (indexed `bucket_count - 1`) buckets are special and are supposed to
    /// catch all "underflow" and "overflow" values. Buckets with indexes 1 up to
    /// `bucket_count - 2` are regular buckets of size `bucket_size`.
    /// </summary>
    public partial class HistogramUint16Bucketed : KaitaiStruct
    {
        public static HistogramUint16Bucketed FromFile(string fileName)
        {
            return new HistogramUint16Bucketed(new KaitaiStream(fileName));
        }

        public HistogramUint16Bucketed(KaitaiStream p__io, KaitaiStruct p__parent = null, MetricsContract p__root = null) : base(p__io)
        {
            m_parent = p__parent;
            m_root = p__root;
            _read();
        }
        private void _read()
        {
            _min = m_io.ReadU8le();
            _bucketSize = m_io.ReadU4le();
            _bucketCount = m_io.ReadU4le();
            _distributionSize = m_io.ReadU2le();
            _columns = new List<PairUint16>((int)(DistributionSize));
            for (var i = 0; i < DistributionSize; i++)
            {
                _columns.Add(new PairUint16(m_io, this, m_root));
            }
        }
        private ulong _min;
        private uint _bucketSize;
        private uint _bucketCount;
        private ushort _distributionSize;
        private List<PairUint16> _columns;
        private MetricsContract m_root;
        private KaitaiStruct m_parent;
        public ulong Min { get { return _min; } }
        public uint BucketSize { get { return _bucketSize; } }
        public uint BucketCount { get { return _bucketCount; } }
        public ushort DistributionSize { get { return _distributionSize; } }
        public List<PairUint16> Columns { get { return _columns; } }
        public MetricsContract M_Root { get { return m_root; } }
        public KaitaiStruct M_Parent { get { return m_parent; } }
    }
    public partial class HistogramValueCountPairs : KaitaiStruct
    {
        public static HistogramValueCountPairs FromFile(string fileName)
        {
            return new HistogramValueCountPairs(new KaitaiStream(fileName));
        }

        public HistogramValueCountPairs(KaitaiStream p__io, KaitaiStruct p__parent = null, MetricsContract p__root = null) : base(p__io)
        {
            m_parent = p__parent;
            m_root = p__root;
            _read();
        }
        private void _read()
        {
            _distributionSize = m_io.ReadU2le();
            _columns = new List<PairValueCount>((int)(DistributionSize));
            for (var i = 0; i < DistributionSize; i++)
            {
                _columns.Add(new PairValueCount(m_io, this, m_root));
            }
        }
        private ushort _distributionSize;
        private List<PairValueCount> _columns;
        private MetricsContract m_root;
        private KaitaiStruct m_parent;
        public ushort DistributionSize { get { return _distributionSize; } }
        public List<PairValueCount> Columns { get { return _columns; } }
        public MetricsContract M_Root { get { return m_root; } }
        public KaitaiStruct M_Parent { get { return m_parent; } }
    }

    /// <summary>
    /// Recorded values that associates trace signals to a metric event within a metric.
    /// </summary>
    public partial class Exemplars : KaitaiStruct
    {
        public static Exemplars FromFile(string fileName)
        {
            return new Exemplars(new KaitaiStream(fileName));
        }

        public Exemplars(KaitaiStream p__io, MetricsContract.TlvField p__parent = null, MetricsContract p__root = null) : base(p__io)
        {
            m_parent = p__parent;
            m_root = p__root;
            _read();
        }
        private void _read()
        {
            _version = m_io.ReadU1();
            _numberOfExemplars = new VlqBase128Le(m_io);
            _exemplarList = new List<SingleExemplar>((int)(NumberOfExemplars.Value));
            for (var i = 0; i < NumberOfExemplars.Value; i++)
            {
                _exemplarList.Add(new SingleExemplar(m_io, this, m_root));
            }
        }
        private byte _version;
        private VlqBase128Le _numberOfExemplars;
        private List<SingleExemplar> _exemplarList;
        private MetricsContract m_root;
        private MetricsContract.TlvField m_parent;

        /// <summary>
        /// The version of exemplar package.
        /// </summary>
        public byte Version { get { return _version; } }
        public VlqBase128Le NumberOfExemplars { get { return _numberOfExemplars; } }
        public List<SingleExemplar> ExemplarList { get { return _exemplarList; } }
        public MetricsContract M_Root { get { return m_root; } }
        public MetricsContract.TlvField M_Parent { get { return m_parent; } }
    }
    public partial class SingleDoubleValueV2 : KaitaiStruct
    {
        public static SingleDoubleValueV2 FromFile(string fileName)
        {
            return new SingleDoubleValueV2(new KaitaiStream(fileName));
        }

        public SingleDoubleValueV2(KaitaiStream p__io, MetricsContract.TlvField p__parent = null, MetricsContract p__root = null) : base(p__io)
        {
            m_parent = p__parent;
            m_root = p__root;
            _read();
        }
        private void _read()
        {
            _timestamp = m_io.ReadU8le();
            _value = m_io.ReadF8le();
        }
        private ulong _timestamp;
        private double _value;
        private MetricsContract m_root;
        private MetricsContract.TlvField m_parent;

        /// <summary>
        /// Timestamp in Windows FILETIME format, i.e. number of 100 ns ticks passed since 1601-01-01 00:00:00 UTC.
        /// </summary>
        public ulong Timestamp { get { return _timestamp; } }

        /// <summary>
        /// Metric value as double.
        /// </summary>
        public double Value { get { return _value; } }
        public MetricsContract M_Root { get { return m_root; } }
        public MetricsContract.TlvField M_Parent { get { return m_parent; } }
    }
    public partial class ExtAggregatedUint64ValueV2 : KaitaiStruct
    {
        public static ExtAggregatedUint64ValueV2 FromFile(string fileName)
        {
            return new ExtAggregatedUint64ValueV2(new KaitaiStream(fileName));
        }

        public ExtAggregatedUint64ValueV2(KaitaiStream p__io, MetricsContract.TlvField p__parent = null, MetricsContract p__root = null) : base(p__io)
        {
            m_parent = p__parent;
            m_root = p__root;
            _read();
        }
        private void _read()
        {
            _count = m_io.ReadU4le();
            _padding = m_io.ReadBytes(4);
            _timestamp = m_io.ReadU8le();
            _sum = m_io.ReadU8le();
            _min = m_io.ReadU8le();
            _max = m_io.ReadU8le();
        }
        private uint _count;
        private byte[] _padding;
        private ulong _timestamp;
        private ulong _sum;
        private ulong _min;
        private ulong _max;
        private MetricsContract m_root;
        private MetricsContract.TlvField m_parent;
        public uint Count { get { return _count; } }

        /// <summary>
        /// Count of events aggregated in this event.
        /// </summary>
        public byte[] Padding { get { return _padding; } }

        /// <summary>
        /// Timestamp in Windows FILETIME format, i.e. number of 100 ns ticks passed since 1601-01-01 00:00:00 UTC.
        /// </summary>
        public ulong Timestamp { get { return _timestamp; } }

        /// <summary>
        /// Sum of all metric values aggregated in this event.
        /// </summary>
        public ulong Sum { get { return _sum; } }

        /// <summary>
        /// Minimum of all metric values aggregated in this event.
        /// </summary>
        public ulong Min { get { return _min; } }

        /// <summary>
        /// Maximum of all metric values aggregated in this event.
        /// </summary>
        public ulong Max { get { return _max; } }
        public MetricsContract M_Root { get { return m_root; } }
        public MetricsContract.TlvField M_Parent { get { return m_parent; } }
    }
    public partial class Histogram : KaitaiStruct
    {
        public static Histogram FromFile(string fileName)
        {
            return new Histogram(new KaitaiStream(fileName));
        }

        public Histogram(KaitaiStream p__io, MetricsContract.Userdata p__parent = null, MetricsContract p__root = null) : base(p__io)
        {
            m_parent = p__parent;
            m_root = p__root;
            _read();
        }
        private void _read()
        {
            _version = m_io.ReadU1();
            _type = ((MetricsContract.DistributionType)m_io.ReadU1());
            switch (Type)
            {
                case MetricsContract.DistributionType.Bucketed:
                    {
                        _body = new HistogramUint16Bucketed(m_io, this, m_root);
                        break;
                    }
                case MetricsContract.DistributionType.MonBucketed:
                    {
                        _body = new HistogramUint16Bucketed(m_io, this, m_root);
                        break;
                    }
                case MetricsContract.DistributionType.ValueCountPairs:
                    {
                        _body = new HistogramValueCountPairs(m_io, this, m_root);
                        break;
                    }
            }
        }
        private byte _version;
        private DistributionType _type;
        private KaitaiStruct _body;
        private MetricsContract m_root;
        private MetricsContract.Userdata m_parent;
        public byte Version { get { return _version; } }
        public DistributionType Type { get { return _type; } }
        public KaitaiStruct Body { get { return _body; } }
        public MetricsContract M_Root { get { return m_root; } }
        public MetricsContract.Userdata M_Parent { get { return m_parent; } }
    }

    /// <summary>
    /// double or long stored as base 128encoded.
    /// </summary>
    public partial class DoubleOrVlq : KaitaiStruct
    {
        public DoubleOrVlq(bool p_isDoubleStoredAsLong, KaitaiStream p__io, MetricsContract.SingleExemplarBody p__parent = null, MetricsContract p__root = null) : base(p__io)
        {
            m_parent = p__parent;
            m_root = p__root;
            _isDoubleStoredAsLong = p_isDoubleStoredAsLong;
            _read();
        }
        private void _read()
        {
            if (IsDoubleStoredAsLong)
            {
                _valueAsVlq = new VlqBase128Le(m_io);
            }
            if (IsDoubleStoredAsLong == false)
            {
                _valueAsDouble = m_io.ReadF8le();
            }
        }
        private VlqBase128Le _valueAsVlq;
        private double? _valueAsDouble;
        private bool _isDoubleStoredAsLong;
        private MetricsContract m_root;
        private MetricsContract.SingleExemplarBody m_parent;
        public VlqBase128Le ValueAsVlq { get { return _valueAsVlq; } }
        public double? ValueAsDouble { get { return _valueAsDouble; } }
        public bool IsDoubleStoredAsLong { get { return _isDoubleStoredAsLong; } }
        public MetricsContract M_Root { get { return m_root; } }
        public MetricsContract.SingleExemplarBody M_Parent { get { return m_parent; } }
    }

    /// <summary>
    /// A single exemplar data.
    /// </summary>
    public partial class SingleExemplar : KaitaiStruct
    {
        public static SingleExemplar FromFile(string fileName)
        {
            return new SingleExemplar(new KaitaiStream(fileName));
        }

        public SingleExemplar(KaitaiStream p__io, MetricsContract.Exemplars p__parent = null, MetricsContract p__root = null) : base(p__io)
        {
            m_parent = p__parent;
            m_root = p__root;
            _read();
        }
        private void _read()
        {
            _version = m_io.ReadU1();
            _length = m_io.ReadU1();
            __raw_body = m_io.ReadBytes((Length - 2));
            var io___raw_body = new KaitaiStream(__raw_body);
            _body = new SingleExemplarBody(io___raw_body, this, m_root);
        }
        private byte _version;
        private byte _length;
        private SingleExemplarBody _body;
        private MetricsContract m_root;
        private MetricsContract.Exemplars m_parent;
        private byte[] __raw_body;

        /// <summary>
        /// The version of single exemplar scheme.
        /// </summary>
        public byte Version { get { return _version; } }

        /// <summary>
        /// Total length of single_exemplar, allows to skip parsing the exemplar data during aggregation.
        /// </summary>
        public byte Length { get { return _length; } }

        /// <summary>
        /// Minus 2 comes from the size of version + size of length fields)
        /// </summary>
        public SingleExemplarBody Body { get { return _body; } }
        public MetricsContract M_Root { get { return m_root; } }
        public MetricsContract.Exemplars M_Parent { get { return m_parent; } }
        public byte[] M_RawBody { get { return __raw_body; } }
    }
    public partial class TlvField : KaitaiStruct
    {
        public static TlvField FromFile(string fileName)
        {
            return new TlvField(new KaitaiStream(fileName));
        }

        public TlvField(KaitaiStream p__io, MetricsContract.UserdataV2 p__parent = null, MetricsContract p__root = null) : base(p__io)
        {
            m_parent = p__parent;
            m_root = p__root;
            _read();
        }
        private void _read()
        {
            _type = ((MetricsContract.PayloadTypes)m_io.ReadU1());
            _lenValue = m_io.ReadU2le();
            switch (Type)
            {
                case MetricsContract.PayloadTypes.HistogramDoubleScaledToUint64ValueCountPairs:
                    {
                        __raw_value = m_io.ReadBytes(LenValue);
                        var io___raw_value = new KaitaiStream(__raw_value);
                        _value = new HistogramValueCountPairs(io___raw_value, this, m_root);
                        break;
                    }
                case MetricsContract.PayloadTypes.SingleDoubleScaledToUint64Value:
                    {
                        __raw_value = m_io.ReadBytes(LenValue);
                        var io___raw_value = new KaitaiStream(__raw_value);
                        _value = new SingleDoubleValueV2(io___raw_value, this, m_root);
                        break;
                    }
                case MetricsContract.PayloadTypes.MetricName:
                    {
                        __raw_value = m_io.ReadBytes(LenValue);
                        var io___raw_value = new KaitaiStream(__raw_value);
                        _value = new WrappedString(io___raw_value, this, m_root);
                        break;
                    }
                case MetricsContract.PayloadTypes.ExtAggregatedUint64Value:
                    {
                        __raw_value = m_io.ReadBytes(LenValue);
                        var io___raw_value = new KaitaiStream(__raw_value);
                        _value = new ExtAggregatedUint64ValueV2(io___raw_value, this, m_root);
                        break;
                    }
                case MetricsContract.PayloadTypes.ExtAggregatedDoubleValue:
                    {
                        __raw_value = m_io.ReadBytes(LenValue);
                        var io___raw_value = new KaitaiStream(__raw_value);
                        _value = new ExtAggregatedDoubleValueV2(io___raw_value, this, m_root);
                        break;
                    }
                case MetricsContract.PayloadTypes.SingleDoubleValue:
                    {
                        __raw_value = m_io.ReadBytes(LenValue);
                        var io___raw_value = new KaitaiStream(__raw_value);
                        _value = new SingleDoubleValueV2(io___raw_value, this, m_root);
                        break;
                    }
                case MetricsContract.PayloadTypes.SingleUint64Value:
                    {
                        __raw_value = m_io.ReadBytes(LenValue);
                        var io___raw_value = new KaitaiStream(__raw_value);
                        _value = new SingleUint64ValueV2(io___raw_value, this, m_root);
                        break;
                    }
                case MetricsContract.PayloadTypes.Dimensions:
                    {
                        __raw_value = m_io.ReadBytes(LenValue);
                        var io___raw_value = new KaitaiStream(__raw_value);
                        _value = new Dimensions(io___raw_value, this, m_root);
                        break;
                    }
                case MetricsContract.PayloadTypes.NamespaceName:
                    {
                        __raw_value = m_io.ReadBytes(LenValue);
                        var io___raw_value = new KaitaiStream(__raw_value);
                        _value = new WrappedString(io___raw_value, this, m_root);
                        break;
                    }
                case MetricsContract.PayloadTypes.ExtAggregatedDoubleScaledToUint64Value:
                    {
                        __raw_value = m_io.ReadBytes(LenValue);
                        var io___raw_value = new KaitaiStream(__raw_value);
                        _value = new ExtAggregatedDoubleValueV2(io___raw_value, this, m_root);
                        break;
                    }
                case MetricsContract.PayloadTypes.Exemplars:
                    {
                        __raw_value = m_io.ReadBytes(LenValue);
                        var io___raw_value = new KaitaiStream(__raw_value);
                        _value = new Exemplars(io___raw_value, this, m_root);
                        break;
                    }
                case MetricsContract.PayloadTypes.HistogramUint64ValueCountPairs:
                    {
                        __raw_value = m_io.ReadBytes(LenValue);
                        var io___raw_value = new KaitaiStream(__raw_value);
                        _value = new HistogramValueCountPairs(io___raw_value, this, m_root);
                        break;
                    }
                case MetricsContract.PayloadTypes.AccountName:
                    {
                        __raw_value = m_io.ReadBytes(LenValue);
                        var io___raw_value = new KaitaiStream(__raw_value);
                        _value = new WrappedString(io___raw_value, this, m_root);
                        break;
                    }
                case MetricsContract.PayloadTypes.HistogramUint16Bucketed:
                    {
                        __raw_value = m_io.ReadBytes(LenValue);
                        var io___raw_value = new KaitaiStream(__raw_value);
                        _value = new HistogramUint16Bucketed(io___raw_value, this, m_root);
                        break;
                    }
                default:
                    {
                        _value = m_io.ReadBytes(LenValue);
                        break;
                    }
            }
        }
        private PayloadTypes _type;
        private ushort _lenValue;
        private object _value;
        private MetricsContract m_root;
        private MetricsContract.UserdataV2 m_parent;
        private byte[] __raw_value;
        public PayloadTypes Type { get { return _type; } }
        public ushort LenValue { get { return _lenValue; } }
        public object Value { get { return _value; } }
        public MetricsContract M_Root { get { return m_root; } }
        public MetricsContract.UserdataV2 M_Parent { get { return m_parent; } }
        public byte[] M_RawValue { get { return __raw_value; } }
    }

    /// <summary>
    /// Bucket #index, claiming to hold exactly `count` hits. See notes in
    /// `histogram_uint16_bucketed` for interpreting index.
    /// </summary>
    public partial class PairUint16 : KaitaiStruct
    {
        public static PairUint16 FromFile(string fileName)
        {
            return new PairUint16(new KaitaiStream(fileName));
        }

        public PairUint16(KaitaiStream p__io, MetricsContract.HistogramUint16Bucketed p__parent = null, MetricsContract p__root = null) : base(p__io)
        {
            m_parent = p__parent;
            m_root = p__root;
            _read();
        }
        private void _read()
        {
            _index = m_io.ReadU2le();
            _count = m_io.ReadU2le();
        }
        private ushort _index;
        private ushort _count;
        private MetricsContract m_root;
        private MetricsContract.HistogramUint16Bucketed m_parent;
        public ushort Index { get { return _index; } }
        public ushort Count { get { return _count; } }
        public MetricsContract M_Root { get { return m_root; } }
        public MetricsContract.HistogramUint16Bucketed M_Parent { get { return m_parent; } }
    }
    public partial class SingleUint64Value : KaitaiStruct
    {
        public static SingleUint64Value FromFile(string fileName)
        {
            return new SingleUint64Value(new KaitaiStream(fileName));
        }

        public SingleUint64Value(KaitaiStream p__io, MetricsContract.Userdata p__parent = null, MetricsContract p__root = null) : base(p__io)
        {
            m_parent = p__parent;
            m_root = p__root;
            _read();
        }
        private void _read()
        {
            _padding = m_io.ReadBytes(4);
            _timestamp = m_io.ReadU8le();
            _value = m_io.ReadU8le();
        }
        private byte[] _padding;
        private ulong _timestamp;
        private ulong _value;
        private MetricsContract m_root;
        private MetricsContract.Userdata m_parent;
        public byte[] Padding { get { return _padding; } }

        /// <summary>
        /// Timestamp in Windows FILETIME format, i.e. number of 100 ns ticks passed since 1601-01-01 00:00:00 UTC.
        /// </summary>
        public ulong Timestamp { get { return _timestamp; } }

        /// <summary>
        /// Metric value as 64-bit unsigned integer.
        /// </summary>
        public ulong Value { get { return _value; } }
        public MetricsContract M_Root { get { return m_root; } }
        public MetricsContract.Userdata M_Parent { get { return m_parent; } }
    }
    public partial class Dimensions : KaitaiStruct
    {
        public static Dimensions FromFile(string fileName)
        {
            return new Dimensions(new KaitaiStream(fileName));
        }

        public Dimensions(KaitaiStream p__io, MetricsContract.TlvField p__parent = null, MetricsContract p__root = null) : base(p__io)
        {
            m_parent = p__parent;
            m_root = p__root;
            _read();
        }
        private void _read()
        {
            _numDimensions = m_io.ReadU2le();
            _dimensionsNames = new List<LenString>((int)(NumDimensions));
            for (var i = 0; i < NumDimensions; i++)
            {
                _dimensionsNames.Add(new LenString(m_io, this, m_root));
            }
            _dimensionsValues = new List<LenString>((int)(NumDimensions));
            for (var i = 0; i < NumDimensions; i++)
            {
                _dimensionsValues.Add(new LenString(m_io, this, m_root));
            }
        }
        private ushort _numDimensions;
        private List<LenString> _dimensionsNames;
        private List<LenString> _dimensionsValues;
        private MetricsContract m_root;
        private MetricsContract.TlvField m_parent;

        /// <summary>
        /// Number of dimensions specified in this event.
        /// </summary>
        public ushort NumDimensions { get { return _numDimensions; } }

        /// <summary>
        /// Dimension names strings (&quot;key&quot; parts of key-value pairs). Must be sorted,
        /// unless MetricsExtenion's option `enableDimensionSortingOnIngestion` is
        /// enabled.
        /// </summary>
        public List<LenString> DimensionsNames { get { return _dimensionsNames; } }

        /// <summary>
        /// Dimension values strings (&quot;value&quot; parts of key-value pairs).
        /// </summary>
        public List<LenString> DimensionsValues { get { return _dimensionsValues; } }
        public MetricsContract M_Root { get { return m_root; } }
        public MetricsContract.TlvField M_Parent { get { return m_parent; } }
    }
    public partial class ExtAggregatedDoubleValue : KaitaiStruct
    {
        public static ExtAggregatedDoubleValue FromFile(string fileName)
        {
            return new ExtAggregatedDoubleValue(new KaitaiStream(fileName));
        }

        public ExtAggregatedDoubleValue(KaitaiStream p__io, MetricsContract.Userdata p__parent = null, MetricsContract p__root = null) : base(p__io)
        {
            m_parent = p__parent;
            m_root = p__root;
            _read();
        }
        private void _read()
        {
            _count = m_io.ReadU4le();
            _timestamp = m_io.ReadU8le();
            _sum = m_io.ReadF8le();
            _min = m_io.ReadF8le();
            _max = m_io.ReadF8le();
        }
        private uint _count;
        private ulong _timestamp;
        private double _sum;
        private double _min;
        private double _max;
        private MetricsContract m_root;
        private MetricsContract.Userdata m_parent;

        /// <summary>
        /// Count of events aggregated in this event.
        /// </summary>
        public uint Count { get { return _count; } }

        /// <summary>
        /// Timestamp in Windows FILETIME format, i.e. number of 100 ns ticks passed since 1601-01-01 00:00:00 UTC.
        /// </summary>
        public ulong Timestamp { get { return _timestamp; } }

        /// <summary>
        /// Sum of all metric values aggregated in this event.
        /// </summary>
        public double Sum { get { return _sum; } }

        /// <summary>
        /// Minimum of all metric values aggregated in this event.
        /// </summary>
        public double Min { get { return _min; } }

        /// <summary>
        /// Maximum of all metric values aggregated in this event.
        /// </summary>
        public double Max { get { return _max; } }
        public MetricsContract M_Root { get { return m_root; } }
        public MetricsContract.Userdata M_Parent { get { return m_parent; } }
    }
    public partial class ExtAggregatedUint64Value : KaitaiStruct
    {
        public static ExtAggregatedUint64Value FromFile(string fileName)
        {
            return new ExtAggregatedUint64Value(new KaitaiStream(fileName));
        }

        public ExtAggregatedUint64Value(KaitaiStream p__io, MetricsContract.Userdata p__parent = null, MetricsContract p__root = null) : base(p__io)
        {
            m_parent = p__parent;
            m_root = p__root;
            _read();
        }
        private void _read()
        {
            _count = m_io.ReadU4le();
            _timestamp = m_io.ReadU8le();
            _sum = m_io.ReadU8le();
            _min = m_io.ReadU8le();
            _max = m_io.ReadU8le();
        }
        private uint _count;
        private ulong _timestamp;
        private ulong _sum;
        private ulong _min;
        private ulong _max;
        private MetricsContract m_root;
        private MetricsContract.Userdata m_parent;

        /// <summary>
        /// Count of events aggregated in this event.
        /// </summary>
        public uint Count { get { return _count; } }

        /// <summary>
        /// Timestamp in Windows FILETIME format, i.e. number of 100 ns ticks passed since 1601-01-01 00:00:00 UTC.
        /// </summary>
        public ulong Timestamp { get { return _timestamp; } }

        /// <summary>
        /// Sum of all metric values aggregated in this event.
        /// </summary>
        public ulong Sum { get { return _sum; } }

        /// <summary>
        /// Minimum of all metric values aggregated in this event.
        /// </summary>
        public ulong Min { get { return _min; } }

        /// <summary>
        /// Maximum of all metric values aggregated in this event.
        /// </summary>
        public ulong Max { get { return _max; } }
        public MetricsContract M_Root { get { return m_root; } }
        public MetricsContract.Userdata M_Parent { get { return m_parent; } }
    }
    public partial class SingleDoubleValue : KaitaiStruct
    {
        public static SingleDoubleValue FromFile(string fileName)
        {
            return new SingleDoubleValue(new KaitaiStream(fileName));
        }

        public SingleDoubleValue(KaitaiStream p__io, MetricsContract.Userdata p__parent = null, MetricsContract p__root = null) : base(p__io)
        {
            m_parent = p__parent;
            m_root = p__root;
            _read();
        }
        private void _read()
        {
            _padding = m_io.ReadBytes(4);
            _timestamp = m_io.ReadU8le();
            _value = m_io.ReadF8le();
        }
        private byte[] _padding;
        private ulong _timestamp;
        private double _value;
        private MetricsContract m_root;
        private MetricsContract.Userdata m_parent;
        public byte[] Padding { get { return _padding; } }

        /// <summary>
        /// Timestamp in Windows FILETIME format, i.e. number of 100 ns ticks passed since 1601-01-01 00:00:00 UTC.
        /// </summary>
        public ulong Timestamp { get { return _timestamp; } }

        /// <summary>
        /// Metric value as double.
        /// </summary>
        public double Value { get { return _value; } }
        public MetricsContract M_Root { get { return m_root; } }
        public MetricsContract.Userdata M_Parent { get { return m_parent; } }
    }

    /// <summary>
    /// Label name-value pair which are in vlq_string type.
    /// </summary>
    public partial class LabelPair : KaitaiStruct
    {
        public static LabelPair FromFile(string fileName)
        {
            return new LabelPair(new KaitaiStream(fileName));
        }

        public LabelPair(KaitaiStream p__io, MetricsContract.SingleExemplarBody p__parent = null, MetricsContract p__root = null) : base(p__io)
        {
            m_parent = p__parent;
            m_root = p__root;
            _read();
        }
        private void _read()
        {
            _name = new VlqString(m_io, this, m_root);
            _value = new VlqString(m_io, this, m_root);
        }
        private VlqString _name;
        private VlqString _value;
        private MetricsContract m_root;
        private MetricsContract.SingleExemplarBody m_parent;
        public VlqString Name { get { return _name; } }
        public VlqString Value { get { return _value; } }
        public MetricsContract M_Root { get { return m_root; } }
        public MetricsContract.SingleExemplarBody M_Parent { get { return m_parent; } }
    }
    public partial class ExtAggregatedDoubleValueV2 : KaitaiStruct
    {
        public static ExtAggregatedDoubleValueV2 FromFile(string fileName)
        {
            return new ExtAggregatedDoubleValueV2(new KaitaiStream(fileName));
        }

        public ExtAggregatedDoubleValueV2(KaitaiStream p__io, MetricsContract.TlvField p__parent = null, MetricsContract p__root = null) : base(p__io)
        {
            m_parent = p__parent;
            m_root = p__root;
            _read();
        }
        private void _read()
        {
            _count = m_io.ReadU4le();
            _padding = m_io.ReadBytes(4);
            _timestamp = m_io.ReadU8le();
            _sum = m_io.ReadF8le();
            _min = m_io.ReadF8le();
            _max = m_io.ReadF8le();
        }
        private uint _count;
        private byte[] _padding;
        private ulong _timestamp;
        private double _sum;
        private double _min;
        private double _max;
        private MetricsContract m_root;
        private MetricsContract.TlvField m_parent;

        /// <summary>
        /// Count of events aggregated in this event.
        /// </summary>
        public uint Count { get { return _count; } }
        public byte[] Padding { get { return _padding; } }

        /// <summary>
        /// Timestamp in Windows FILETIME format, i.e. number of 100 ns ticks passed since 1601-01-01 00:00:00 UTC.
        /// </summary>
        public ulong Timestamp { get { return _timestamp; } }

        /// <summary>
        /// Sum of all metric values aggregated in this event.
        /// </summary>
        public double Sum { get { return _sum; } }

        /// <summary>
        /// Minimum of all metric values aggregated in this event.
        /// </summary>
        public double Min { get { return _min; } }

        /// <summary>
        /// Maximum of all metric values aggregated in this event.
        /// </summary>
        public double Max { get { return _max; } }
        public MetricsContract M_Root { get { return m_root; } }
        public MetricsContract.TlvField M_Parent { get { return m_parent; } }
    }

    /// <summary>
    /// A single exemplar data.
    /// </summary>
    public partial class SingleExemplarBody : KaitaiStruct
    {
        public static SingleExemplarBody FromFile(string fileName)
        {
            return new SingleExemplarBody(new KaitaiStream(fileName));
        }

        public SingleExemplarBody(KaitaiStream p__io, MetricsContract.SingleExemplar p__parent = null, MetricsContract p__root = null) : base(p__io)
        {
            m_parent = p__parent;
            m_root = p__root;
            _read();
        }
        private void _read()
        {
            _flags = new ExemplarFlags(m_io, this, m_root);
            _value = new DoubleOrVlq(Flags.IsMetricValueDoubleStoredAsLong, m_io, this, m_root);
            _numberOfLabels = m_io.ReadU1();
            if (Flags.IsTimestampAvailable)
            {
                _timeUnixNano = m_io.ReadU8le();
            }
            if (Flags.TraceIdExists)
            {
                _traceId = m_io.ReadBytes(16);
            }
            if (Flags.SpanIdExists)
            {
                _spanId = m_io.ReadBytes(8);
            }
            if (Flags.SampleCountExists)
            {
                _sampleCount = m_io.ReadF8le();
            }
            _labels = new List<LabelPair>((int)(NumberOfLabels));
            for (var i = 0; i < NumberOfLabels; i++)
            {
                _labels.Add(new LabelPair(m_io, this, m_root));
            }
        }
        private ExemplarFlags _flags;
        private DoubleOrVlq _value;
        private byte _numberOfLabels;
        private ulong? _timeUnixNano;
        private byte[] _traceId;
        private byte[] _spanId;
        private double? _sampleCount;
        private List<LabelPair> _labels;
        private MetricsContract m_root;
        private MetricsContract.SingleExemplar m_parent;

        /// <summary>
        /// Defines characteristics of an exemplar.
        /// </summary>
        public ExemplarFlags Flags { get { return _flags; } }

        /// <summary>
        /// Metric value as double.
        /// </summary>
        public DoubleOrVlq Value { get { return _value; } }

        /// <summary>
        /// Total number of filtered labels.
        /// </summary>
        public byte NumberOfLabels { get { return _numberOfLabels; } }

        /// <summary>
        /// Exact time that the measurement was recorded.
        /// </summary>
        public ulong? TimeUnixNano { get { return _timeUnixNano; } }

        /// <summary>
        /// Trace ID of the current trace.
        /// </summary>
        public byte[] TraceId { get { return _traceId; } }

        /// <summary>
        /// Span ID of the current trace.
        /// </summary>
        public byte[] SpanId { get { return _spanId; } }

        /// <summary>
        /// When sample_count is non-zero, this exemplar has been chosen in a statistically
        /// unbiased way such that the exemplar is representative of `sample_count` individual events.
        /// </summary>
        public double? SampleCount { get { return _sampleCount; } }

        /// <summary>
        /// Dimension key-value pairs.
        /// </summary>
        public List<LabelPair> Labels { get { return _labels; } }
        public MetricsContract M_Root { get { return m_root; } }
        public MetricsContract.SingleExemplar M_Parent { get { return m_parent; } }
    }
    public partial class UserdataV2 : KaitaiStruct
    {
        public static UserdataV2 FromFile(string fileName)
        {
            return new UserdataV2(new KaitaiStream(fileName));
        }

        public UserdataV2(KaitaiStream p__io, MetricsContract p__parent = null, MetricsContract p__root = null) : base(p__io)
        {
            m_parent = p__parent;
            m_root = p__root;
            _read();
        }
        private void _read()
        {
            _fields = new List<TlvField>();
            {
                var i = 0;
                while (!m_io.IsEof)
                {
                    _fields.Add(new TlvField(m_io, this, m_root));
                    i++;
                }
            }
        }
        private List<TlvField> _fields;
        private MetricsContract m_root;
        private MetricsContract m_parent;
        public List<TlvField> Fields { get { return _fields; } }
        public MetricsContract M_Root { get { return m_root; } }
        public MetricsContract M_Parent { get { return m_parent; } }
    }

    /// <summary>
    /// A simple string, length-prefixed with a 2-byte integer.
    /// </summary>
    public partial class LenString : KaitaiStruct
    {
        public static LenString FromFile(string fileName)
        {
            return new LenString(new KaitaiStream(fileName));
        }

        public LenString(KaitaiStream p__io, KaitaiStruct p__parent = null, MetricsContract p__root = null) : base(p__io)
        {
            m_parent = p__parent;
            m_root = p__root;
            _read();
        }
        private void _read()
        {
            _lenValue = m_io.ReadU2le();
            _value = System.Text.Encoding.GetEncoding("UTF-8").GetString(m_io.ReadBytes(LenValue));
        }
        private ushort _lenValue;
        private string _value;
        private MetricsContract m_root;
        private KaitaiStruct m_parent;
        public ushort LenValue { get { return _lenValue; } }
        public string Value { get { return _value; } }
        public MetricsContract M_Root { get { return m_root; } }
        public KaitaiStruct M_Parent { get { return m_parent; } }
    }

    /// <summary>
    /// UTF-8 string with its length prefixed using a VLQ integer.
    /// </summary>
    public partial class VlqString : KaitaiStruct
    {
        public static VlqString FromFile(string fileName)
        {
            return new VlqString(new KaitaiStream(fileName));
        }

        public VlqString(KaitaiStream p__io, MetricsContract.LabelPair p__parent = null, MetricsContract p__root = null) : base(p__io)
        {
            m_parent = p__parent;
            m_root = p__root;
            _read();
        }
        private void _read()
        {
            _lenValue = new VlqBase128Le(m_io);
            _value = System.Text.Encoding.GetEncoding("UTF-8").GetString(m_io.ReadBytes(LenValue.Value));
        }
        private VlqBase128Le _lenValue;
        private string _value;
        private MetricsContract m_root;
        private MetricsContract.LabelPair m_parent;
        public VlqBase128Le LenValue { get { return _lenValue; } }
        public string Value { get { return _value; } }
        public MetricsContract M_Root { get { return m_root; } }
        public MetricsContract.LabelPair M_Parent { get { return m_parent; } }
    }
    private bool f_eventType;
    private MetricEventType _eventType;
    public MetricEventType EventType
    {
        get
        {
            if (f_eventType)
                return _eventType;
            _eventType = (MetricEventType)(((MetricEventType)EventId));
            f_eventType = true;
            return _eventType;
        }
    }
    private ushort _eventId;
    private ushort _lenBody;
    private KaitaiStruct _body;
    private MetricsContract m_root;
    private KaitaiStruct m_parent;
    private byte[] __raw_body;

    /// <summary>
    /// Type of message, affects format of the body.
    /// </summary>
    public ushort EventId { get { return _eventId; } }

    /// <summary>
    /// Size of body in bytes.
    /// </summary>
    public ushort LenBody { get { return _lenBody; } }

    /// <summary>
    /// Body of Metrics binary protocol message.
    /// </summary>
    public KaitaiStruct Body { get { return _body; } }
    public MetricsContract M_Root { get { return m_root; } }
    public KaitaiStruct M_Parent { get { return m_parent; } }
    public byte[] M_RawBody { get { return __raw_body; } }
}
