namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse.PerfLib
{
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Runtime.InteropServices;
    using System.Security.Permissions;

    // not public!
    [HostProtection(MayLeakOnAbort = true)]

    internal static class NativeMethods
    {
        public const int WAIT_TIMEOUT = 0x00000102;
        
        public const int ERROR_NOT_READY = 21;
        public const int ERROR_LOCK_FAILED = 167;
        public const int ERROR_BUSY = 170;

        // file src\services\monitoring\system\diagnosticts\nativemethods.cs
        public const int PERF_SIZE_DWORD = 0x00000000;
        public const int PERF_SIZE_LARGE = 0x00000100;
        public const int PERF_SIZE_ZERO = 0x00000200;  // for Zero Length fields
                
        // select one of the following values to indicate the counter field usage
        public const int PERF_TYPE_COUNTER = 0x00000400;  // an increasing numeric value
        
        // If the PERF_TYPE_COUNTER value was selected then select one of the
        // following to indicate the type of counter
        public const int PERF_COUNTER_BASE = 0x00030000;  // base value used In fractions
        
        // Any types that have calculations performed can use one or more of
        // the following calculation modification flags listed here
        public const int PERF_MULTI_COUNTER = 0x02000000;  // sum of multiple instances

        // Select one of the following values to indicate the display suffix (if any)
        public const int PERF_DISPLAY_NOSHOW = 0x40000000;  // value is not displayed

        // Predefined counter types
        // A label: no data is associated with this counter (it has 0 length)
        // Do not display.
        public const int PERF_COUNTER_NODATA =
                PERF_SIZE_ZERO | PERF_DISPLAY_NOSHOW;
        
        // The divisor for a sample, used with the previous counter to form a
        // sampled %.  You must check for >0 before dividing by this!  This
        // counter will directly follow the  numerator counter.  It should not
        // be displayed to the user.
        public const int PERF_SAMPLE_BASE =
                PERF_SIZE_DWORD | PERF_TYPE_COUNTER | PERF_COUNTER_BASE |
                PERF_DISPLAY_NOSHOW |
                0x00000001;  // for compatibility with pre-beta versions

        // Used as the denominator In the computation of time or count
        // averages.  Must directly follow the numerator counter.  Not dis-
        // played to the user.
        public const int PERF_AVERAGE_BASE =
                PERF_SIZE_DWORD | PERF_TYPE_COUNTER | PERF_COUNTER_BASE |
                PERF_DISPLAY_NOSHOW |
                0x00000002;  // for compatibility with pre-beta versions
        
        // Number of instances to which the preceding _MULTI_..._INV counter
        // applies.  Used as a factor to get the percentage.
        public const int PERF_COUNTER_MULTI_BASE =
                PERF_SIZE_LARGE | PERF_TYPE_COUNTER | PERF_COUNTER_BASE |
                PERF_MULTI_COUNTER | PERF_DISPLAY_NOSHOW;
        
        // Indicates the data is a base for the preceding counter which should
        // not be time averaged on display (such as free space over total space.)
        public const int PERF_RAW_BASE =
                PERF_SIZE_DWORD | PERF_TYPE_COUNTER | PERF_COUNTER_BASE |
                PERF_DISPLAY_NOSHOW |
                0x00000003;  // for compatibility with pre-beta versions

        public const int PERF_LARGE_RAW_BASE =
                    PERF_SIZE_LARGE | PERF_TYPE_COUNTER | PERF_COUNTER_BASE |
                    PERF_DISPLAY_NOSHOW;

        public const int ERROR_ACCESS_DENIED = 5;
        public const int ERROR_INVALID_HANDLE = 6;

        public const int RPC_S_SERVER_UNAVAILABLE = 1722;
        public const int RPC_S_CALL_FAILED = 1726;

        [StructLayout(LayoutKind.Sequential)]
        internal class PERF_COUNTER_DEFINITION
        {
            public int ByteLength = 0;
            public int CounterNameTitleIndex = 0;

            // this one is kind of weird. It is defined as in SDK:
            // #ifdef _WIN64
            // DWORD           CounterNameTitle;
            // #else
            // LPWSTR          CounterNameTitle;
            // #endif
            // so we can't use IntPtr here.
            public int CounterNameTitlePtr = 0;
            public int CounterHelpTitleIndex = 0;
            public int CounterHelpTitlePtr = 0;
            public int DefaultScale = 0;
            public int DetailLevel = 0;
            public int CounterType = 0;
            public int CounterSize = 0;
            public int CounterOffset = 0;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal class PERF_DATA_BLOCK
        {
            public int Signature1 = 0;
            public int Signature2 = 0;
            public int LittleEndian = 0;
            public int Version = 0;
            public int Revision = 0;
            public int TotalByteLength = 0;
            public int HeaderLength = 0;
            public int NumObjectTypes = 0;
            public int DefaultObject = 0;
            public SYSTEMTIME SystemTime = null;
            public int pad1 = 0;  // Need to pad the struct to get quadword alignment for the 'long' after SystemTime
            public long PerfTime = 0;
            public long PerfFreq = 0;
            public long PerfTime100nSec = 0;
            public int SystemNameLength = 0;
            public int SystemNameOffset = 0;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal class PERF_INSTANCE_DEFINITION
        {
            public int ByteLength = 0;
            public int ParentObjectTitleIndex = 0;
            public int ParentObjectInstance = 0;
            public int UniqueID = 0;
            public int NameOffset = 0;
            public int NameLength = 0;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal class PERF_OBJECT_TYPE
        {
            public int TotalByteLength = 0;
            public int DefinitionLength = 0;
            public int HeaderLength = 0;
            public int ObjectNameTitleIndex = 0;
            public int ObjectNameTitlePtr = 0;
            public int ObjectHelpTitleIndex = 0;
            public int ObjectHelpTitlePtr = 0;
            public int DetailLevel = 0;
            public int NumCounters = 0;
            public int DefaultCounter = 0;
            public int NumInstances = 0;
            public int CodePage = 0;
            public long PerfTime = 0;
            public long PerfFreq = 0;
        }

        [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Class needed")]
        [StructLayout(LayoutKind.Sequential)]
        internal class SYSTEMTIME
        {
            public short wYear;
            public short wMonth;
            public short wDayOfWeek;
            public short wDay;
            public short wHour;
            public short wMinute;
            public short wSecond;
            public short wMilliseconds;

            public override string ToString()
            {
                return "[SYSTEMTIME: "
                + this.wDay.ToString(CultureInfo.CurrentCulture) + "/" + this.wMonth.ToString(CultureInfo.CurrentCulture) + "/" + this.wYear.ToString(CultureInfo.CurrentCulture)
                + " " + this.wHour.ToString(CultureInfo.CurrentCulture) + ":" + this.wMinute.ToString(CultureInfo.CurrentCulture) + ":" + this.wSecond.ToString(CultureInfo.CurrentCulture)
                + "]";
            }
        }
    }
}