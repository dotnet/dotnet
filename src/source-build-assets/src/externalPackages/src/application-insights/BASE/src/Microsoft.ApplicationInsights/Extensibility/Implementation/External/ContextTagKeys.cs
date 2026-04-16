// <copyright file="ContextTagKeys.cs" company="Microsoft">
// Copyright © Microsoft. All Rights Reserved.
// </copyright>
namespace Microsoft.ApplicationInsights.Extensibility.Implementation.External
{
    using System.Threading;

    /// <summary>
    /// Holds the static singleton instance of ContextTagKeys.
    /// </summary>
    internal partial class ContextTagKeys
    {
        private static ContextTagKeys keys;

        internal static ContextTagKeys Keys
        {
            get { return LazyInitializer.EnsureInitialized(ref keys); }
        }
    }
}