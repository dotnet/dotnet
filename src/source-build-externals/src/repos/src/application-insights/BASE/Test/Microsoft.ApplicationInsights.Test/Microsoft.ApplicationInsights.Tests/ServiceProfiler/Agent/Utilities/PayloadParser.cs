using static Microsoft.ServiceProfiler.Agent.Utilities.PayloadParserUtilities;

namespace Microsoft.ServiceProfiler.Agent.Utilities
{
    /// <summary>
    /// This code is derived from the Application Insights Profiler agent. It is included in this repo
    /// in order to validate ETW payload serialization in RichPayloadEventSource.
    /// </summary>
    internal static class PayloadParser
    {
        public unsafe static ParsedPayload ParsePayload(byte[] payload)
        {
            var result = new ParsedPayload();

            fixed (byte* begin = &payload[0])
            {
                byte* p = begin;
                byte* end = p + payload.Length;

                // Parse the InstrumentationKey.
                if (!TryParseNextLengthPrefixedUnicodeString(ref p, end, out result.InstrumentationKey))
                {
                    return null;
                }

                // Parse the operation name and root operation id from tags.
                if (!TryParseRequestDataTags(ref p, end, out result.OperationName, out result.OperationId))
                {
                    return null;
                }

                // Parse the version.
                if (!TryParseNextInt32(ref p, end, out result.Version))
                {
                    return null;
                }

                // Parse the request id.
                if (!TryParseNextLengthPrefixedUnicodeString(ref p, end, out result.RequestId))
                {
                    return null;
                }

                // Parse the source.
                if (!TryParseNextLengthPrefixedUnicodeString(ref p, end, out result.Source))
                {
                    return null;
                }

                // Parse the name.
                if (!TryParseNextLengthPrefixedUnicodeString(ref p, end, out result.Name))
                {
                    return null;
                }

                // Parse the duration.
                if (!TryParseTimespan(ref p, end, out result.Duration))
                {
                    return null;
                }

                return result;
            }
        }

        private unsafe static bool TryParseRequestDataTags(ref byte* p, byte* end, out string operationName, out string rootOperationId)
        {
            operationName = null;
            rootOperationId = null;

            if (!TryParseNextInt16(ref p, end, out short count))
            {
                return false;
            }

            for (short i = 0; i < count; i++)
            {
                if (!TryParseNextLengthPrefixedUnicodeString(ref p, end, out string key))
                {
                    return false;
                }

                switch (key)
                {
                    case "ai.operation.name":
                        {
                            if (!TryParseNextLengthPrefixedUnicodeString(ref p, end, out operationName))
                            {
                                return false;
                            }
                            break;
                        }
                    case "ai.operation.id":
                        {
                            if (!TryParseNextLengthPrefixedUnicodeString(ref p, end, out rootOperationId))
                            {
                                return false;
                            }
                            break;
                        }
                    default:
                        {
                            if (!TryParseNextLengthPrefixedUnicodeString(ref p, end, out string value))
                            {
                                return false;
                            }
                            break;
                        }
                }
            }

            return operationName != null && rootOperationId != null;
        }
    }
}
