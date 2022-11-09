namespace Microsoft.ApplicationInsights.Metrics.ConcurrentDatastructures
{
    using System;

    internal struct MultidimensionalPointResult<TPoint>
    {
        private TPoint point;
        private int failureCoordinateIndex;
        private MultidimensionalPointResultCodes resultCode;

        internal MultidimensionalPointResult(MultidimensionalPointResultCodes failureCode, int failureCoordinateIndex)
        {
            this.resultCode = failureCode;
            this.failureCoordinateIndex = failureCoordinateIndex;
            this.point = default(TPoint);
        }

        internal MultidimensionalPointResult(MultidimensionalPointResultCodes successCode, TPoint point)
        {
            this.resultCode = successCode;
            this.failureCoordinateIndex = -1;
            this.point = point;
        }

        public TPoint Point
        {
            get { return this.point; }
        }

        public int FailureCoordinateIndex
        {
            get { return this.failureCoordinateIndex; }
        }

        public MultidimensionalPointResultCodes ResultCode
        {
            get { return this.resultCode; }
        }

        public bool IsPointCreatedNew
        {
            get
            {
                return ((this.ResultCode & MultidimensionalPointResultCodes.Success_NewPointCreated) != 0)
                          || ((this.ResultCode & MultidimensionalPointResultCodes.Success_NewPointCreatedAboveDimCapLimit) != 0);
            }
        }

        public bool IsSuccess
        {
            get
            {
                return ((this.ResultCode & MultidimensionalPointResultCodes.Success_NewPointCreated) != 0)
                          || ((this.ResultCode & MultidimensionalPointResultCodes.Success_ExistingPointRetrieved) != 0)
                          || ((this.ResultCode & MultidimensionalPointResultCodes.Success_NewPointCreatedAboveDimCapLimit) != 0);
            }
        }

        internal void SetAsyncTimeoutReachedFailure()
        {
            this.resultCode |= MultidimensionalPointResultCodes.Failure_AsyncTimeoutReached;
        }
    }
}
