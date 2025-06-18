namespace Microsoft.ApplicationInsights.Metrics.ConcurrentDatastructures
{
    using System;

    [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Design",
            "CA1008: Enums should have zero value",
            Justification = "Crafted these flags to fit into a byte to make the struct container cheaper.")]
    [Flags]
    internal enum MultidimensionalPointResultCodes : byte
    {
        /// <summary>
        /// A new point was created and returned in this result.
        /// </summary>
        Success_NewPointCreated = 1,

        /// <summary>
        /// A point already existed, it was retreived and returned in this result.
        /// </summary>
        Success_ExistingPointRetrieved = 2,

        /// <summary>
        /// A new point was created and returned in this result.
        /// The newly created point exceeded the specified dimension values count limit for one or more dimensions,
        /// but it was capped with a fallback value.
        /// </summary>
        Success_NewPointCreatedAboveDimCapLimit = 4,

        /// <summary>
        /// A point could not be created becasue the sub-dimsnions count was reached.
        /// </summary>
        Failure_SubdimensionsCountLimitReached = 8,

        /// <summary>
        /// A point could not be created becasue the total points limit was reached.
        /// </summary>
        Failure_TotalPointsCountLimitReached = 16,

        /// <summary>
        /// A point could not be retreived becasue it does not exist and creation was not requested.
        /// </summary>
        Failure_PointDoesNotExistCreationNotRequested = 32,

        /// <summary>
        /// Timeout reached.
        /// </summary>
        Failure_AsyncTimeoutReached = 128,
    }
}
