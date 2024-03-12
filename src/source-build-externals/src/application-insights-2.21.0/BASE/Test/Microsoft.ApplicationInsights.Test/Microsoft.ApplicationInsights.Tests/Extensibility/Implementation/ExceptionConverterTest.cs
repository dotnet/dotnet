namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
    using System;
    using System.Diagnostics;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    using Microsoft.ApplicationInsights.Extensibility.Implementation.External;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.ApplicationInsights.TestFramework;
    using Moq;

    /// <summary>
    /// Tests of exception stack serialization.
    /// </summary>
    [TestClass]
    public class ExceptionConverterTest
    {
        [TestMethod]
        public void CallingConvertToExceptionDetailsWithNullExceptionThrowsArgumentNullException()
        {
            AssertEx.Throws<ArgumentNullException>(() => ExceptionConverter.ConvertToExceptionDetails(null, null));
        }

        [TestMethod]
        public void EmptyStringIsReturnedForExceptionWithoutStack()
        {
            var exp = new ArgumentException();

            ExceptionDetails expDetails = ExceptionConverter.ConvertToExceptionDetails(exp, null);

            Assert.AreEqual(string.Empty, expDetails.stack);
            Assert.AreEqual(0, expDetails.parsedStack.Count);

            // hasFullStack defaults to true.
            Assert.IsTrue(expDetails.hasFullStack);
        }

        [TestMethod]
        public void AllStackFramesAreConvertedIfSizeOfParsedStackIsLessOrEqualToMaximum()
        {
            var exp = this.CreateException(42);

            ExceptionDetails expDetails = ExceptionConverter.ConvertToExceptionDetails(exp, null);
            Assert.AreEqual(43, expDetails.parsedStack.Count);
            Assert.IsTrue(expDetails.hasFullStack);
        }

        [TestMethod]
        public void TestFirstAndLastStackPointsAreCollectedForLongStack()
        {
            var exp = this.CreateException(300);

            ExceptionDetails expDetails = ExceptionConverter.ConvertToExceptionDetails(exp, null);
            
            Assert.IsFalse(expDetails.hasFullStack);
            Assert.IsTrue(expDetails.parsedStack.Count < 300);

            // We should keep top of stack, and end of stack hence CreateException function should be present
            Assert.AreEqual("Microsoft.ApplicationInsights.Extensibility.Implementation.ExceptionConverterTest.FailedFunction", expDetails.parsedStack[0].method);
            Assert.AreEqual("Microsoft.ApplicationInsights.Extensibility.Implementation.ExceptionConverterTest.CreateException", expDetails.parsedStack[expDetails.parsedStack.Count - 1].method);
        }

        [TestMethod]
        public void TestNullMethodInfoInStack()
        {
            var frameMock = new Mock<System.Diagnostics.StackFrame>(null, 0, 0);
            frameMock.Setup(x => x.GetMethod()).Returns((MethodBase)null);

            External.StackFrame stackFrame = null;

            try
            {
                stackFrame = ExceptionConverter.GetStackFrame(frameMock.Object, 0);
            }
            catch (Exception e)
            {
                Assert.Fail("GetStackFrame threw " + e);
            }

            Assert.AreEqual("unknown", stackFrame.assembly);
            Assert.AreEqual(null, stackFrame.fileName);
            Assert.AreEqual("unknown", stackFrame.method);
            Assert.AreEqual(0, stackFrame.line);
        }

        [TestMethod]
        public void CheckThatFileNameAndLineAreCorrectIfAvailable()
        {
            var exp = this.CreateException(1);

            StackTrace st = new StackTrace(exp, true);
            var frame = st.GetFrame(0);
            var line = frame.GetFileLineNumber();
            var fileName = frame.GetFileName();

            ExceptionDetails expDetails = ExceptionConverter.ConvertToExceptionDetails(exp, null);
            var stack = expDetails.parsedStack;

            if (line != 0)
            {
                Assert.AreEqual(line, stack[0].line);
                Assert.AreEqual(fileName, stack[0].fileName);
            }
            else
            {
                Assert.AreEqual(0, stack[0].line);
                Assert.IsNull(stack[0].fileName);
            }
        }

        [TestMethod]
        public void CheckLevelCorrespondsToFrameForLongStack()
        {
            const int NumberOfStackFrames = 100;

            var exp = this.CreateException(NumberOfStackFrames - 1);

            ExceptionDetails expDetails = ExceptionConverter.ConvertToExceptionDetails(exp, null);
            var stack = expDetails.parsedStack;

            // Checking levels for first few and last few.
            for (int i = 0; i < 10; ++i)
            {
                Assert.AreEqual(i, stack[i].level);
            }

            for (int j = NumberOfStackFrames - 1, i = 0; j > NumberOfStackFrames - 10; --j, i++)
            {
                Assert.AreEqual(j, stack[stack.Count - 1 - i].level);
            }
        }

        [TestMethod]
        public void SizeOfParsedStackFrameIsLessThanMaxValue()
        {
            var exp = this.CreateException(300);

            ExceptionDetails expDetails = ExceptionConverter.ConvertToExceptionDetails(exp, null);
            int parsedStackLength = 0;

            var stack = expDetails.parsedStack;
            for (int i = 0; i < stack.Count; i++)
            {
                parsedStackLength += (stack[i].method == null ? 0 : stack[i].method.Length)
                                     + (stack[i].assembly == null ? 0 : stack[i].assembly.Length)
                                     + (stack[i].fileName == null ? 0 : stack[i].fileName.Length);
            }
            Assert.IsTrue(parsedStackLength <= ExceptionConverter.MaxParsedStackLength);
        }

        [TestMethod]
        public void SanitizesLineNumberOnParsedStackFrame()
        {
            var stackFrame = ExceptionConverter.GetStackFrame(new System.Diagnostics.StackFrame("test", 1000001), 0);
            
            Assert.AreEqual(0, stackFrame.line);

            stackFrame = ExceptionConverter.GetStackFrame(new System.Diagnostics.StackFrame("test", -1000001), 0);

            Assert.AreEqual(0, stackFrame.line);

            stackFrame = ExceptionConverter.GetStackFrame(new System.Diagnostics.StackFrame("test", 10), 0);

            Assert.AreEqual(10, stackFrame.line);
        }

        [TestMethod]
        public void TrimsExceptionMessagesGreaterThanMaxLength()
        {
            var exp = new Exception(new string('x', ExceptionConverter.MaxExceptionMessageLength + 5));

            ExceptionDetails expDetails = ExceptionConverter.ConvertToExceptionDetails(exp, null);

            Assert.AreEqual(ExceptionConverter.MaxExceptionMessageLength, expDetails.message.Length);
        }

        [TestMethod]
        public void DoesNotTrimShortExceptionMessages()
        {
            var exp = new Exception(new string('x', 5));

            ExceptionDetails expDetails = ExceptionConverter.ConvertToExceptionDetails(exp, null);

            Assert.AreEqual(5, expDetails.message.Length);
        }

        [MethodImplAttribute(MethodImplOptions.NoInlining)]
        private Exception CreateException(int numberOfStackpoints)
        {
            Exception exception = null;

            try
            {
                this.FailedFunction(numberOfStackpoints);
            }
            catch (Exception exp)
            {
                exception = exp;
            }

            return exception;
        }

        [MethodImplAttribute(MethodImplOptions.NoInlining)]
        private void FailedFunction(int numberOfStackpoints)
        {
            if (numberOfStackpoints > 1)
            {
                this.FailedFunction(--numberOfStackpoints);
            }

            throw new AggregateException("exception message");
        }
    }
}