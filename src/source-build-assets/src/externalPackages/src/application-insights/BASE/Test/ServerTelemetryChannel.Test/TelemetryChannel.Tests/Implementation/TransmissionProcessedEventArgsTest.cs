namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation
{
    using System;

    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.TestFramework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    
    
    [TestClass]
    public class TransmissionProcessedEventArgsTest
    {
        [TestMethod]
        public void TransmissionReturnsValueSuppliedByConstructor()
        {
            Transmission transmission = new StubTransmission();
            var args = new TransmissionProcessedEventArgs(transmission);
            Assert.AreSame(transmission, args.Transmission);
        }

        [TestMethod]
        public void ExceptionReturnsValueSuppliedByConstructor()
        {
            Exception exception = new Exception();
            var args = new TransmissionProcessedEventArgs(new StubTransmission(), exception);
            Assert.AreSame(exception, args.Exception);
        }
    }
}
