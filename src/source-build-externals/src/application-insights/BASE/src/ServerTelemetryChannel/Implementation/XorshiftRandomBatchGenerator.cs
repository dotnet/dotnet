namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Generates batches of random number using Xorshift algorithm
    /// Note: the base code is from http://www.codeproject.com/Articles/9187/A-fast-equivalent-for-System-Random.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly",
        Justification = "Xorshift is a well-known algorithm name")]
    internal class XorshiftRandomBatchGenerator : IRandomNumberBatchGenerator
    {
        private const ulong Y = 4477743899113974427L, Z = 2994213561913849757L, W = 9123831478480964153L;

        private ulong lastX, lastY, lastZ, lastW;

        /// <summary>
        /// Initializes a new instance of the <see cref="XorshiftRandomBatchGenerator"/> class.
        /// </summary>
        /// <param name="seed">Random generator seed value.</param>
        public XorshiftRandomBatchGenerator(ulong seed)
        {
            // The only stipulation stated for the xorshift RNG is that at least one of
            // the seeds X,Y,Z,W is non-zero. We fulfill that requirement by only allowing
            // resetting of the X seed.

            // The first random sample will be very closely related to the value of X we set here. 
            // Thus setting X = seed will result in a close correlation between the bit patterns of the seed and
            // the first random sample, therefore if the seed has a pattern (e.g. 1,2,3) then there will also be 
            // a recognizable pattern across the first random samples.
            //
            // Such a strong correlation between the seed and the first random sample is an undesirable
            // characteristic of a RNG, therefore we significantly weaken any correlation by hashing the seed's bits. 
            // This is achieved by multiplying the seed with four large primes each with bits distributed over the
            // full length of a 64bit value, finally adding the results to give X.
            this.lastX = (seed * 5073061188973594169L) + (seed * 8760132611124384359L) + (seed * 8900702462021224483L)
                         + (seed * 6807056130438027397L);

            this.lastY = Y;
            this.lastZ = Z;
            this.lastW = W;
        }

        /// <summary>
        /// Generates a batch of random numbers.
        /// </summary>
        /// <param name="buffer">Buffer to put numbers in.</param>
        /// <param name="index">Start index in the buffer.</param>
        /// <param name="count">Count of random numbers to generate.</param>
        public void NextBatch(ulong[] buffer, int index, int count)
        {
            ulong x = this.lastX;
            ulong y = this.lastY;
            ulong z = this.lastZ;
            ulong w = this.lastW;

            for (int i = 0; i < count; i++)
            {
                ulong t = x ^ (x << 11);
                x = y;
                y = z;
                z = w;
                w = (w ^ (w >> 19)) ^ (t ^ (t >> 8));

                buffer[index + i] = w;
            }

            this.lastX = x;
            this.lastY = y;
            this.lastZ = z;
            this.lastW = w;
        }
    }
}
