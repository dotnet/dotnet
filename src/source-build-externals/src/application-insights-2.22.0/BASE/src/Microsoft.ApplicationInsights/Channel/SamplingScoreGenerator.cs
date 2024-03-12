namespace Microsoft.ApplicationInsights.Channel
{
    using System;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;

    /// <summary>
    /// Utility class for sampling score generation.
    /// </summary>
    public static class SamplingScoreGenerator
    {
        /// <summary>
        ///  Generates telemetry sampling score between 0 and 100.
        /// </summary>
        /// <param name="value">String value to generate score.</param>
        /// <returns>Item sampling score.</returns>
        public static double GetSamplingScore(string value)
        {
            return (double)value.GetSamplingHashCode() / int.MaxValue * 100;
        }

        /// <summary>
        /// Generates telemetry sampling score between 0 and 100.
        /// </summary>
        /// <param name="telemetry">Telemetry item to score.</param>
        /// <returns>Item sampling score.</returns>
        public static double GetSamplingScore(ITelemetry telemetry)
        {
            if (telemetry == null)
            {
                throw new ArgumentNullException(nameof(telemetry));
            }

            double samplingScore = 0;

            if (telemetry.Context.Operation.Id != null)
            {
                samplingScore = (double)telemetry.Context.Operation.Id.GetSamplingHashCode() / int.MaxValue;
            }
            else
            {
                samplingScore = (double)WeakConcurrentRandom.Instance.Next() / ulong.MaxValue;
            }

            return samplingScore * 100;
        }

        internal static int GetSamplingHashCode(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return 0;
            }

            while (input.Length < 8)
            {
                input = input + input;
            }

            int hash = 5381;

            for (int i = 0; i < input.Length; i++)
            {
                hash = ((hash << 5) + hash) + (int)input[i];
            }

            return hash == int.MinValue ? int.MaxValue : Math.Abs(hash);
        }
    }
}
