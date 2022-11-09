namespace Microsoft.ApplicationInsights.TestFramework
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Extension of <see cref="ExpectedExceptionAttribute"/> class to validate a test based on both the exception type and message.
    /// </summary>
    /// <remarks>
    /// Inspired by StackOverflow answer: https://stackoverflow.com/a/16945443/1466768
    /// </remarks>
    public class ExpectedExceptionWithMessageAttribute : ExpectedExceptionBaseAttribute
    {
        private readonly Type exceptionType;
        private readonly string expectedMessage;

        public ExpectedExceptionWithMessageAttribute(Type expectedType) : this(expectedType, null)
        {
        }

        public ExpectedExceptionWithMessageAttribute(Type expectedType, string expectedMessage)
        {
            this.exceptionType = expectedType;
            this.expectedMessage = expectedMessage;
        }

        protected override void Verify(Exception ex)
        {
            if (ex.GetType() != this.exceptionType)
            {
                Assert.Fail($"Test method threw exception '{ex.GetType().FullName}', but exception '{this.exceptionType.FullName}' was expected. Actual exception message: '{ex.Message}'");
            }

            if (this.expectedMessage != null && this.expectedMessage != ex.Message)
            {
                Assert.Fail($"Test method threw the expected exception type, but with an unexpected message: '{ex.Message}'");
            }

            Console.Write("ExpectedExceptionWithMessageAttribute:" + ex.Message);
        }
    }
}
