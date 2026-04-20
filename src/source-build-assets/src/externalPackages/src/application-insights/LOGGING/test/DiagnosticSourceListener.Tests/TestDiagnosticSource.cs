//-----------------------------------------------------------------------
// <copyright file="TestDiagnosticSource.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.ApplicationInsights.DiagnosticSourceListener.Tests
{
    using System.Diagnostics;

    internal class TestDiagnosticSource : DiagnosticListener
    {
        public const string ListenerName = nameof(TestDiagnosticSource);

        public TestDiagnosticSource(string listenerName = ListenerName) : base(listenerName)
        {
        }
    }
}
