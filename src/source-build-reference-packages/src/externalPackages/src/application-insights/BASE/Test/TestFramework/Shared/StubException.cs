namespace Microsoft.ApplicationInsights.TestFramework
{
    using System;

#if NET452
    [Serializable]
#endif
    internal class StubException : Exception
    {
        public Func<string> OnGetStackTrace = () => string.Empty;
        public Func<string> OnToString = () => string.Empty;

        public override string StackTrace
        {
            get { return this.OnGetStackTrace(); }
        }

        public override string ToString()
        {
            return this.OnToString();
        }
    }
}
