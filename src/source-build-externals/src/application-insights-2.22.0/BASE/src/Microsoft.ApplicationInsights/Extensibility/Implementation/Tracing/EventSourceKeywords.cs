namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing
{
    internal static class EventSourceKeywords
    {
        public const long UserActionable = 0x1;

        public const long Diagnostics = 0x2;

        public const long VerboseFailure = 0x4;

        public const long ErrorFailure = 0x8;

        public const long ReservedUserKeywordBegin = 0x10;
    }
}
