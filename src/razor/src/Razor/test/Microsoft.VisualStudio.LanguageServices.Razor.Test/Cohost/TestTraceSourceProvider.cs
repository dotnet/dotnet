// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.LanguageServices.Razor.Test.Cohost;

/// <summary>
/// An implementation of IServiceProvider that only provides a TraceSource, that writes to test output
/// </summary>
internal class TestTraceSourceProvider(ITestOutputHelper testOutputHelper) : IServiceProvider
{
    public object GetService(Type serviceType)
    {
        if (serviceType == typeof(TraceSource))
        {
            return new TestOutputTraceSource(testOutputHelper);
        }

        throw new NotImplementedException();
    }

    private class TestOutputTraceSource : TraceSource
    {
        public TestOutputTraceSource(ITestOutputHelper testOutputHelper)
            : base("OOP", SourceLevels.All)
        {
            Listeners.Add(new TestOutputTraceListener(testOutputHelper));
        }

        private class TestOutputTraceListener(ITestOutputHelper testOutputHelper) : TraceListener
        {
            public override void Write(string message)
            {
                // ITestOutputHelper doesn't have a Write method, but all we lose is some extra ServiceHub details like log level
            }

            public override void WriteLine(string message)
            {
                // Ignore some specific ServiceHub noise, since we're not using ServiceHub anyway
                if (message.StartsWith("Added local RPC method") || message == "Listening started.")
                {
                    return;
                }

                testOutputHelper.WriteLine(message);
            }
        }
    }
}
