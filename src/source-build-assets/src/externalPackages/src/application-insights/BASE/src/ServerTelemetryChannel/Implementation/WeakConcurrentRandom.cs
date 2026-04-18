namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Threading;

    internal class WeakConcurrentRandom
    {
        /// <summary>
        /// Generator singleton.
        /// </summary>
        private static WeakConcurrentRandom random;

        /// <summary>
        /// Index of the last used random number within pre-generated array.
        /// </summary>
        private int index;

        /// <summary>
        /// Count of segments of random numbers.
        /// </summary>
        private int segmentCount;

        /// <summary>
        /// Number of random numbers per segment.
        /// </summary>
        private int segmentSize;

        /// <summary>
        /// Number of bits used to store index of the random number within segment.
        /// </summary>
        private int bitsToStoreRandomIndexWithinSegment;

        /// <summary>
        /// Bit mask to get segment index bits.
        /// </summary>
        private int segmentIndexMask;

        /// <summary>
        /// Bit mask to get index of the random number within segment.
        /// </summary>
        private int randomIndexWithinSegmentMask;

        /// <summary>
        /// Bit mask to get index of the random number in the pre-generated array.
        /// </summary>
        private int randomArrayIndexMask;

        /// <summary>
        /// Array of random number batch generators (one per each segment).
        /// </summary>
        private IRandomNumberBatchGenerator[] randomGemerators;

        /// <summary>
        /// Array of pre-generated random numbers.
        /// </summary>
        private ulong[] randomNumbers;

        /// <summary>
        /// Initializes a new instance of the <see cref="WeakConcurrentRandom"/> class.
        /// </summary>
        public WeakConcurrentRandom()
        {
            this.Initialize();
        }

        public static WeakConcurrentRandom Instance
        {
#if NETFRAMEWORK
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
            get
            {
                if (random != null)
                {
                    return random;
                }

                Interlocked.CompareExchange(ref random, new WeakConcurrentRandom(), null);
                return random;
            }
        }

        /// <summary>
        /// Initializes generator with a set of random numbers.
        /// </summary>
        public void Initialize()
        {
            // by default we use xorshift algorithm with 8 segments 1024 random numbers each
            this.Initialize((seed) => new XorshiftRandomBatchGenerator(seed), 3, 10);
        }

        /// <summary>
        /// Initializes generator with a set of random numbers.
        /// </summary>
        /// <param name="randomGeneratorFactory">Factory used to create random number batch generators.</param>
        /// <param name="segmentIndexBits">Number of significant bits in segment index, i.e. value of 3 means 8 segments of random numbers - 0..7.</param>
        /// <param name="segmentBits">Number of significant bits in random number index within segment, i.e. value of 10 means 1024 random numbers per segment.</param>
        public void Initialize(
            Func<ulong, IRandomNumberBatchGenerator> randomGeneratorFactory,
            int segmentIndexBits,
            int segmentBits)
        {
            // **************************************************
            // validate value of the segementIndexBits parameter
            int effectiveSegementIndexBits = segmentIndexBits;

            // number of segments must be between 1 and 15
            if ((segmentIndexBits < 1) || (segmentIndexBits > 4))
            {
                // 8 segments is default in case the value supplied by caller is incorrect
                effectiveSegementIndexBits = 3;
            }

            // **************************************************
            // validate value of the segementBits parameter
            int effectiveSegementBits = segmentBits;

            // number of randoms within segment must be between 256 and 64K
            if ((segmentBits < 7) || (segmentBits > 15))
            {
                // 1024 randoms is default in case the value supplied by caller is incorrect
                effectiveSegementBits = 9;
            }

            this.bitsToStoreRandomIndexWithinSegment = effectiveSegementBits;

            // store count of segments and count of randoms within the segment
            this.segmentCount = 1 << effectiveSegementIndexBits;
            this.segmentSize = 1 << effectiveSegementBits;

            // store masks
            this.segmentIndexMask = (this.segmentCount - 1) << this.bitsToStoreRandomIndexWithinSegment;
            this.randomIndexWithinSegmentMask = this.segmentSize - 1;
            this.randomArrayIndexMask = this.segmentIndexMask | this.randomIndexWithinSegmentMask;

            int randomNumberCount = this.segmentCount * this.segmentSize;

            // create segmentCount random generators
            this.randomGemerators = new IRandomNumberBatchGenerator[this.segmentCount];

            // create random generator to seed the other ones
            XorshiftRandomBatchGenerator seedingRnd = new XorshiftRandomBatchGenerator((ulong)Environment.TickCount);
            ulong[] seeds = new ulong[this.segmentCount];

            seedingRnd.NextBatch(seeds, 0, this.segmentCount);

            for (int i = 0; i < this.segmentCount; i++)
            {
                // default random batch generator in case factory is null or returns null generator
                Func<ulong, IRandomNumberBatchGenerator> defaultGenerator = (ulong seed) => new XorshiftRandomBatchGenerator(seed);

                IRandomNumberBatchGenerator segmentGenerator = (randomGeneratorFactory == null)
                    ? defaultGenerator(seeds[i])
                    : randomGeneratorFactory(seeds[i]) ?? defaultGenerator(seeds[i]);

                this.randomGemerators[i] = segmentGenerator;
            }

            // allocate storage for random numbers
            this.randomNumbers = new ulong[randomNumberCount];

            // generate entire set of random numbers using seeding random
            seedingRnd.NextBatch(this.randomNumbers, 0, randomNumberCount);
        }

        /// <summary>
        /// Weakly thread safe next (random) operation id generator
        /// where 'weakly' indicates that it is unlikely we'll get into 
        /// collision state.
        /// </summary>
        /// <returns>Next operation id.</returns>
        public ulong Next()
        {
            // get index of the next random number in the pre-generated array
            int newIndex = Interlocked.Increment(ref this.index);

            // check to see if we just switched to a different segment
            if ((newIndex & this.randomIndexWithinSegmentMask) == 0)
            {
                this.RegenerateSegment(newIndex);
            }

            return this.randomNumbers[newIndex & this.randomArrayIndexMask];
        }

        /// <summary>
        /// Generates random number batch for segment which just exhausted
        /// according to value of the new index.
        /// </summary>
        /// <param name="newIndex">Index in random number array of the random number we're about to return.</param>
        private void RegenerateSegment(int newIndex)
        {
            // regenerate segment we just finished
            int segementToRegenerate;

            if ((newIndex & this.segmentIndexMask) == 0)
            {
                segementToRegenerate = this.segmentCount - 1;
            }
            else
            {
                segementToRegenerate = ((newIndex & this.segmentIndexMask) >> this.bitsToStoreRandomIndexWithinSegment) - 1;
            }

            this.randomGemerators[segementToRegenerate].NextBatch(
                this.randomNumbers,
                segementToRegenerate * this.segmentSize,
                this.segmentSize);
        }
    }
}
