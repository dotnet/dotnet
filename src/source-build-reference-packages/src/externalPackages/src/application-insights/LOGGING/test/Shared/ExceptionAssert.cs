//-----------------------------------------------------------------------
// <copyright file="EventSourceTelemetryModuleTests.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.ApplicationInsights.Tests
{
    using System;
    using VisualStudio.TestTools.UnitTesting;

    public static class ExceptionAssert
    {
        public static void Throws<TException>(Action action, Action<TException> notifyException = null) where TException : Exception
        {
            try
            {
                action();
                throw new AssertFailedException($"An exception of type {typeof(TException)} was expected, but was not thrown");
            }
            catch (TException ex)
            {
                if (notifyException != null)
                {
                    notifyException(ex);
                }
            }
        }
    }
}
