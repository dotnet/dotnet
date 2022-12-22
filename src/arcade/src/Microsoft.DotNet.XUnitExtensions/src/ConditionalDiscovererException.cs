using System;

namespace Microsoft.DotNet.XUnitExtensions
{
    internal class ConditionalDiscovererException : Exception
    {
        public ConditionalDiscovererException(string message) : base(message) { }
    }
}
