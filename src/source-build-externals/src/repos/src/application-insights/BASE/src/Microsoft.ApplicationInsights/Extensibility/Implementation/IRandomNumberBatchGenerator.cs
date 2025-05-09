namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
    /// <summary>
    /// Interface for random number generator capable of producing 
    /// a batch of unsigned 64 bit random numbers.
    /// </summary>
    internal interface IRandomNumberBatchGenerator
    {
        void NextBatch(ulong[] buffer, int index, int count);
    }
}
