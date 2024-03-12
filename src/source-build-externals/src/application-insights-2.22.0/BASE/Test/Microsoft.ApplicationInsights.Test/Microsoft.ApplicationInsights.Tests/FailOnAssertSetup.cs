// <copyright file="FailOnAssertSetup.cs" company="Microsoft">
// Copyright © Microsoft. All Rights Reserved.
// </copyright>

namespace Microsoft.ApplicationInsights
{
#if NETFRAMEWORK
    using System;
    using System.Diagnostics;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class FailOnAssertSetup
    {
        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            // Fail on Debug.Assert rather than popping up a window
            var defaultListener = Debug.Listeners
                .OfType<DefaultTraceListener>()
                .FirstOrDefault();
            if (defaultListener != null)
            {
                Debug.Listeners.Remove(defaultListener);
                Debug.Listeners.Add(new FailOnDebugAssertTraceListener());
            }
        }

        /// <summary>
        /// Converts Debug.Assert into a test failure.
        /// </summary>
        private class FailOnDebugAssertTraceListener : TraceListener
        {
            public override void Fail(string message)
            {
                this.WriteLine(message);
                Assert.Fail(message);
            }

            public override void Fail(string message, string detailMessage)
            {
                this.WriteLine(message);
                this.WriteLine(detailMessage);
                Assert.Fail(message + " : " + detailMessage);
            }

            public override void Write(string message)
            {
                Console.Write(message);
            }

            public override void WriteLine(string message)
            {
                Console.WriteLine(message);
            }
        }
    }
#endif
}
