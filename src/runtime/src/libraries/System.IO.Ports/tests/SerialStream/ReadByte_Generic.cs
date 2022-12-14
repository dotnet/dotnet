// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;
using System.IO.PortsTests;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Legacy.Support;
using Xunit;
using Microsoft.DotNet.XUnitExtensions;

namespace System.IO.Ports.Tests
{
    public class SerialStream_ReadByte_Generic : PortsTest
    {
        // Set bounds fore random timeout values.
        // If the min is to low read will not timeout accurately and the testcase will fail
        private const int minRandomTimeout = 250;

        // If the max is to large then the testcase will take forever to run
        private const int maxRandomTimeout = 2000;

        // If the percentage difference between the expected timeout and the actual timeout
        // found through Stopwatch is greater then 10% then the timeout value was not correctly
        // to the read method and the testcase fails.
        private const double maxPercentageDifference = .15;

        // The number of random bytes to receive
        private const int numRndByte = 8;
        private const int NUM_TRYS = 5;

        #region Test Cases

        [ConditionalFact(nameof(HasOneSerialPort))]
        public void ReadAfterClose()
        {
            using (SerialPort com = new SerialPort(TCSupport.LocalMachineSerialInfo.FirstAvailablePortName))
            {
                Debug.WriteLine("Verifying read method throws exception after a call to Close()");

                com.Open();
                Stream serialStream = com.BaseStream;
                com.Close();

                VerifyReadException(serialStream, typeof(ObjectDisposedException));
            }
        }

        [ConditionalFact(nameof(HasOneSerialPort))]
        public void ReadAfterBaseStreamClose()
        {
            using (SerialPort com = new SerialPort(TCSupport.LocalMachineSerialInfo.FirstAvailablePortName))
            {
                Debug.WriteLine("Verifying read method throws exception after a call to .BaseStream.Close()");

                com.Open();
                Stream serialStream = com.BaseStream;
                com.BaseStream.Close();

                VerifyReadException(serialStream, typeof(ObjectDisposedException));
            }
        }

        [Trait(XunitConstants.Category, XunitConstants.IgnoreForCI)]  // Timing-sensitive
        [ConditionalFact(nameof(HasOneSerialPort))]
        public void Timeout()
        {
            using (SerialPort com = new SerialPort(TCSupport.LocalMachineSerialInfo.FirstAvailablePortName))
            {
                var rndGen = new Random(-55);

                com.ReadTimeout = rndGen.Next(minRandomTimeout, maxRandomTimeout);

                Debug.WriteLine("Verifying ReadTimeout={0}", com.ReadTimeout);
                com.Open();

                VerifyTimeout(com);
            }
        }

        [Trait(XunitConstants.Category, XunitConstants.IgnoreForCI)]  // Timing-sensitive
        [ConditionalFact(nameof(HasOneSerialPort))]
        public void SuccessiveReadTimeoutNoData()
        {
            using (SerialPort com = new SerialPort(TCSupport.LocalMachineSerialInfo.FirstAvailablePortName))
            {
                var rndGen = new Random(-55);

                com.ReadTimeout = rndGen.Next(minRandomTimeout, maxRandomTimeout);
                com.Encoding = Encoding.Unicode;

                Debug.WriteLine("Verifying ReadTimeout={0} with successive call to read method and no data", com.ReadTimeout);
                com.Open();

                Assert.Throws<TimeoutException>(() => com.BaseStream.ReadByte());

                VerifyTimeout(com);
            }
        }

        [ConditionalFact(nameof(HasNullModem))]
        public void SuccessiveReadTimeoutSomeData()
        {
            using (var com1 = new SerialPort(TCSupport.LocalMachineSerialInfo.FirstAvailablePortName))
            {
                var rndGen = new Random(-55);
                var t = new Task(WriteToCom1);


                com1.ReadTimeout = rndGen.Next(minRandomTimeout, maxRandomTimeout);
                com1.Encoding = new UTF8Encoding();

                Debug.WriteLine(
                    "Verifying ReadTimeout={0} with successive call to read method and some data being received in the first call",
                    com1.ReadTimeout);
                com1.Open();

                // Call WriteToCom1 asynchronously this will write to com1 some time before the following call
                // to a read method times out
                t.Start();

                try
                {
                    com1.BaseStream.ReadByte();
                }
                catch (TimeoutException)
                {
                }

                TCSupport.WaitForTaskCompletion(t);

                // Make sure there is no bytes in the buffer so the next call to read will timeout
                com1.DiscardInBuffer();
                VerifyTimeout(com1);
            }
        }

        private void WriteToCom1()
        {
            using (var com2 = new SerialPort(TCSupport.LocalMachineSerialInfo.SecondAvailablePortName))
            {
                var rndGen = new Random(-55);
                var xmitBuffer = new byte[1];
                int sleepPeriod = rndGen.Next(minRandomTimeout, maxRandomTimeout / 2);

                // Sleep some random period with of a maximum duration of half the largest possible timeout value for a read method on COM1
                Thread.Sleep(sleepPeriod);
                com2.Open();

                com2.Write(xmitBuffer, 0, xmitBuffer.Length);
            }
        }

        [KnownFailure]
        [ConditionalFact(nameof(HasNullModem))]
        public void DefaultParityReplaceByte()
        {
            VerifyParityReplaceByte(-1, numRndByte - 2);
        }

        [KnownFailure]
        [ConditionalFact(nameof(HasNullModem))]
        public void NoParityReplaceByte()
        {
            var rndGen = new Random(-55);
            VerifyParityReplaceByte('\0', rndGen.Next(0, numRndByte - 1), Encoding.UTF32);
        }

        [KnownFailure]
        [ConditionalFact(nameof(HasNullModem))]
        public void RNDParityReplaceByte()
        {
            var rndGen = new Random(-55);

            VerifyParityReplaceByte(rndGen.Next(0, 128), 0, new UTF8Encoding());
        }

        [KnownFailure]
        [ConditionalFact(nameof(HasNullModem))]
        public void ParityErrorOnLastByte()
        {
            using (var com1 = new SerialPort(TCSupport.LocalMachineSerialInfo.FirstAvailablePortName))
            using (var com2 = new SerialPort(TCSupport.LocalMachineSerialInfo.SecondAvailablePortName))
            {
                var rndGen = new Random(15);
                var bytesToWrite = new byte[numRndByte];
                var expectedBytes = new byte[numRndByte];
                var actualBytes = new byte[numRndByte + 1];
                var actualByteIndex = 0;

                /* 1 Additional character gets added to the input buffer when the parity error occurs on the last byte of a stream
                 We are verifying that besides this everything gets read in correctly. See NDP Whidbey: 24216 for more info on this */
                Debug.WriteLine("Verifying default ParityReplace byte with a parity error on the last byte");

                // Generate random characters without an parity error
                for (var i = 0; i < bytesToWrite.Length; i++)
                {
                    var randByte = (byte)rndGen.Next(0, 128);

                    bytesToWrite[i] = randByte;
                    expectedBytes[i] = randByte;
                }

                bytesToWrite[bytesToWrite.Length - 1] = (byte)(bytesToWrite[bytesToWrite.Length - 1] | 0x80);
                // Create a parity error on the last byte
                expectedBytes[expectedBytes.Length - 1] = com1.ParityReplace;
                // Set the last expected byte to be the ParityReplace Byte

                com1.Parity = Parity.Space;
                com1.DataBits = 7;
                com1.ReadTimeout = 250;

                com1.Open();
                com2.Open();

                com2.Write(bytesToWrite, 0, bytesToWrite.Length);

                TCSupport.WaitForReadBufferToLoad(com1, bytesToWrite.Length + 1);

                while (true)
                {
                    int byteRead;
                    try
                    {
                        byteRead = com1.ReadByte();
                    }
                    catch (TimeoutException)
                    {
                        break;
                    }

                    actualBytes[actualByteIndex] = (byte)byteRead;
                    actualByteIndex++;
                }

                // Compare the chars that were written with the ones we expected to read
                for (var i = 0; i < expectedBytes.Length; i++)
                {
                    if (expectedBytes[i] != actualBytes[i])
                    {
                        Fail("ERROR!!!: Expected to read {0}  actual read  {1}", (int)expectedBytes[i],
                            (int)actualBytes[i]);
                    }
                }

                if (1 < com1.BytesToRead)
                {
                    Fail("ERROR!!!: Expected BytesToRead=0 actual={0}", com1.BytesToRead);
                    Fail("ByteRead={0}, {1}", com1.ReadByte(), bytesToWrite[bytesToWrite.Length - 1]);
                }

                bytesToWrite[bytesToWrite.Length - 1] = (byte)(bytesToWrite[bytesToWrite.Length - 1] & 0x7F);
                // Clear the parity error on the last byte
                expectedBytes[expectedBytes.Length - 1] = bytesToWrite[bytesToWrite.Length - 1];
                VerifyRead(com1, com2, bytesToWrite, expectedBytes, Encoding.ASCII);
            }
        }

        #endregion

        #region Verification for Test Cases
        private void VerifyTimeout(SerialPort com)
        {
            var timer = new Stopwatch();
            int expectedTime = com.ReadTimeout;
            var actualTime = 0;
            double percentageDifference;


            try
            {
                com.BaseStream.ReadByte(); // Warm up read method
                Fail("Err_6941814ahbpa!!!: Read did not throw Timeout Exception when it timed out for the first time");
            }
            catch (TimeoutException) { }

            Thread.CurrentThread.Priority = ThreadPriority.Highest;

            for (var i = 0; i < NUM_TRYS; i++)
            {
                timer.Start();
                try
                {
                    com.BaseStream.ReadByte();
                    Fail("Err_17087ahps!!!: Read did not reuturn 0 when it timed out");
                }
                catch (TimeoutException) { }

                timer.Stop();
                actualTime += (int)timer.ElapsedMilliseconds;
                timer.Reset();
            }

            Thread.CurrentThread.Priority = ThreadPriority.Normal;
            actualTime /= NUM_TRYS;
            percentageDifference = Math.Abs((expectedTime - actualTime) / (double)expectedTime);

            // Verify that the percentage difference between the expected and actual timeout is less then maxPercentageDifference
            if (maxPercentageDifference < percentageDifference)
            {
                Fail("ERROR!!!: The read method timedout in {0} expected {1} percentage difference: {2}", actualTime, expectedTime, percentageDifference);
            }

            if (com.IsOpen)
                com.Close();
        }


        private void VerifyReadException(Stream serialStream, Type expectedException)
        {
            Assert.Throws(expectedException, () => serialStream.ReadByte());
        }

        private void VerifyParityReplaceByte(int parityReplace, int parityErrorIndex)
        {
            VerifyParityReplaceByte(parityReplace, parityErrorIndex, new ASCIIEncoding());
        }

        private void VerifyParityReplaceByte(int parityReplace, int parityErrorIndex, Encoding encoding)
        {
            using (var com1 = new SerialPort(TCSupport.LocalMachineSerialInfo.FirstAvailablePortName))
            using (var com2 = new SerialPort(TCSupport.LocalMachineSerialInfo.SecondAvailablePortName))
            {
                var rndGen = new Random(-55);
                var byteBuffer = new byte[numRndByte];
                var expectedBytes = new byte[numRndByte];
                int expectedChar;

                // Generate random bytes without an parity error
                for (var i = 0; i < byteBuffer.Length; i++)
                {
                    int randChar = rndGen.Next(0, 128);

                    byteBuffer[i] = (byte)randChar;
                    expectedBytes[i] = (byte)randChar;
                }

                if (-1 == parityReplace)
                {
                    // If parityReplace is -1 and we should just use the default value
                    expectedChar = com1.ParityReplace;
                }
                else if ('\0' == parityReplace)
                {
                    // If parityReplace is the null charachater and parity replacement should not occur
                    com1.ParityReplace = (byte)parityReplace;
                    expectedChar = expectedBytes[parityErrorIndex];
                }
                else
                {
                    // Else parityReplace was set to a value and we should expect this value to be returned on a parity error
                    com1.ParityReplace = (byte)parityReplace;
                    expectedChar = parityReplace;
                }

                // Create an parity error by setting the highest order bit to true
                byteBuffer[parityErrorIndex] = (byte)(byteBuffer[parityErrorIndex] | 0x80);
                expectedBytes[parityErrorIndex] = (byte)expectedChar;

                Debug.WriteLine("Verifying ParityReplace={0} with an ParityError at: {1} ", com1.ParityReplace,
                    parityErrorIndex);

                com1.Parity = Parity.Space;
                com1.DataBits = 7;

                com1.Open();
                com2.Open();

                VerifyRead(com1, com2, byteBuffer, expectedBytes, encoding);
            }
        }

        private void VerifyRead(SerialPort com1, SerialPort com2, byte[] bytesToWrite, byte[] expectedBytes, Encoding encoding)
        {
            var byteRcvBuffer = new byte[expectedBytes.Length];
            var rcvBufferSize = 0;
            int i;

            com2.Write(bytesToWrite, 0, bytesToWrite.Length);
            com1.ReadTimeout = 250;
            com1.Encoding = encoding;

            TCSupport.WaitForReadBufferToLoad(com1, bytesToWrite.Length);

            i = 0;
            while (true)
            {
                int readInt;
                try
                {
                    readInt = com1.BaseStream.ReadByte();
                }
                catch (TimeoutException)
                {
                    break;
                }

                // While their are more bytes to be read
                if (expectedBytes.Length <= i)
                {
                    // If we have read in more bytes then we expecte
                    Fail("ERROR!!!: We have received more bytes then were sent");
                    break;
                }

                byteRcvBuffer[i] = (byte)readInt;
                rcvBufferSize++;

                if (bytesToWrite.Length - rcvBufferSize != com1.BytesToRead)
                {
                    Fail("ERROR!!!: Expected BytesToRead={0} actual={1}", bytesToWrite.Length - rcvBufferSize, com1.BytesToRead);
                }

                if (readInt != expectedBytes[i])
                {
                    // If the bytes read is not the expected byte
                    Fail("ERROR!!!: Expected to read {0}  actual read byte {1}", expectedBytes[i], (byte)readInt);
                }

                i++;
            }

            if (rcvBufferSize != expectedBytes.Length)
            {
                Fail("ERROR!!! Expected to read {0} char actually read {1} chars", bytesToWrite.Length, rcvBufferSize);
            }
        }
        #endregion
    }
}
