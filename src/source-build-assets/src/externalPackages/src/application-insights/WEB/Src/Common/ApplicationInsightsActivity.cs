namespace Microsoft.ApplicationInsights.Common
{
    using System;
    using System.Globalization;
    using System.Threading;
    using Microsoft.ApplicationInsights.DataContracts;

    // See
    // https://github.com/lmolkova/correlation/blob/master/http_protocol_proposal_v1.md
    // https://github.com/dotnet/corefx/blob/master/src/System.Diagnostics.DiagnosticSource/src/System/Diagnostics/Activity.cs

    /// <summary>
    /// Mimics System.Diagnostics.Activity and Correlation HTTP protocol 
    /// and intended to be used on .NET 4.0.
    /// </summary>
    internal class ApplicationInsightsActivity
    {
        private const int RequestIdMaxLength = 1024;

        /// <summary>
        /// A unique number inside the AppDomain, randomized between AppDomains. 
        /// Integer gives enough randomization and keeps hex-encoded s_currentRootId 8 chars long for most applications.
        /// </summary>
        private static long currentRootId = (uint)GetRandomNumber();

        /// <summary>
        /// Generates Id for the RequestTelemetry from the parentId.
        /// </summary>
        /// <param name="parentId">Parent Activity/Request Id.</param>
        public static string GenerateRequestId(string parentId = null)
        {
            string ret;
            if (!string.IsNullOrEmpty(parentId))
            {
                // Start from outside the process (e.g. incoming HTTP)
                // sanitize external RequestId as it may not be hierarchical. 
                // we cannot update ParentId, we must let it be logged exactly as it was passed.
                parentId = parentId[0] == '|' ? parentId : '|' + parentId;
                if (parentId[parentId.Length - 1] != '.')
                {
                    parentId += '.';
                }

                ret = AppendSuffix(parentId, Interlocked.Increment(ref currentRootId).ToString("x", CultureInfo.InvariantCulture), '_');
            }
            else
            {
                // A Root Activity (no parent).  
                ret = GenerateRootId();
            }

            // Useful place to place a conditional breakpoint.  
            return ret;
        }

        /// <summary>
        /// Generates Id for the DependencyTelemetry.
        /// </summary>
        /// <param name="parentId">Parent Activity/Request Id.</param>
        public static string GenerateDependencyId(string parentId)
        {
            string ret;
            if (!string.IsNullOrEmpty(parentId))
            {
                // Start from outside the process (e.g. incoming HTTP)
                // sanitize external RequestId as it may not be hierarchical. 
                // we cannot update ParentId, we must let it be logged exactly as it was passed.
                parentId = parentId[0] == '|' ? parentId : '|' + parentId;
                if (parentId[parentId.Length - 1] != '.')
                {
                    parentId += '.';
                }

                ret = AppendSuffix(parentId, Interlocked.Increment(ref currentRootId).ToString("x", CultureInfo.InvariantCulture), '_');
            }
            else
            {
                // A Root Activity (no parent).  
                ret = GenerateRootId();
            }

            // Useful place to place a conditional breakpoint.  
            return ret;
        }

        /// <summary>
        /// Gets the root Id from the request Id: substring between '|' and first '.'.
        /// </summary>
        /// <param name="id">Id to get the root from.</param>
        public static string GetRootId(string id)
        {
            // id MAY start with '|' and contain '.'. We return substring between them
            // ParentId MAY NOT have hierarchical structure and we don't know if initially rootId was started with '|',
            // so we must NOT include first '|' to allow mixed hierarchical and non-hierarchical request id scenarios
            int rootEnd = id.IndexOf('.');
            if (rootEnd < 0)
            {
                rootEnd = id.Length;
            }

            int rootStart = id[0] == '|' ? 1 : 0;
            return id.Substring(rootStart, rootEnd - rootStart);
        }

        private static string AppendSuffix(string parentId, string suffix, char delimiter)
        {
            if (parentId.Length + suffix.Length < RequestIdMaxLength)
            {
                return parentId + suffix + delimiter;
            }

            // Id overflow:
            // find position in RequestId to trim
            int trimPosition = RequestIdMaxLength - 9; // overflow suffix + delimiter length is 9
            while (trimPosition > 1)
            {
                if (parentId[trimPosition - 1] == '.' || parentId[trimPosition - 1] == '_')
                {
                    break;
                }

                trimPosition--;
            }

            // ParentId is not valid Request-Id, let's generate proper one.
            if (trimPosition == 1)
            {
                return GenerateRootId();
            }

            // generate overflow suffix
            string overflowSuffix = ((int)GetRandomNumber()).ToString("x8", CultureInfo.InvariantCulture);
            return parentId.Substring(0, trimPosition) + overflowSuffix + '#';
        }

        private static string GenerateRootId()
        {
            return string.Format(CultureInfo.InvariantCulture, "|{0}.", new RequestTelemetry().Id);
        }

        private static ulong GetRandomNumber()
        {
            return BitConverter.ToUInt64(Guid.NewGuid().ToByteArray(), 8);
        }
    }
}