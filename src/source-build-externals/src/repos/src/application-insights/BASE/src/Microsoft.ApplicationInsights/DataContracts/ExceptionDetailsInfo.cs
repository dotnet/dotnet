namespace Microsoft.ApplicationInsights.DataContracts
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.External;

    /// <summary>
    /// Wrapper class for <see cref="ExceptionDetails"/> that lets user gets/sets TypeName and Message.
    /// </summary>
    public sealed class ExceptionDetailsInfo
    {
        internal readonly ExceptionDetails InternalExceptionDetails = null;

        /// <summary>
        /// Constructs the instance of <see cref="ExceptionDetailsInfo"/>.
        /// </summary>
        /// <param name="id">Exception id.</param>
        /// <param name="outerId">Parent exception's id.</param>
        /// <param name="typeName">Type name for the exception.</param>
        /// <param name="message">Exception message.</param>
        /// <param name="hasFullStack">Indicates that this exception has full stack information.</param>
        /// <param name="stack">Exception's stack trace.</param>
        /// <param name="parsedStack">Exception's stack.</param>
        public ExceptionDetailsInfo(int id, int outerId, string typeName, string message, bool hasFullStack,
            string stack, IEnumerable<StackFrame> parsedStack)
        {
            this.InternalExceptionDetails = new ExceptionDetails()
            {
                id = id,
                outerId = outerId,
                typeName = typeName,
                message = message,
                hasFullStack = hasFullStack,
                stack = stack,
                parsedStack = parsedStack.Select(ps => ps.Data).ToList(),
            };
        }

        internal ExceptionDetailsInfo(ExceptionDetails exceptionDetails)
        {
            this.InternalExceptionDetails = exceptionDetails;
        }

        /// <summary>
        /// Gets or sets type name of the underlying <see cref="System.Exception"/> that this object represents.
        /// </summary>
        public string TypeName
        {
            get => this.InternalExceptionDetails.typeName;
            set => this.InternalExceptionDetails.typeName = value;
        }

        /// <summary>
        /// Gets or sets message name of the underlying <see cref="System.Exception"/> that this object represents.
        /// </summary>
        public string Message
        {
            get => this.InternalExceptionDetails.message;
            set => this.InternalExceptionDetails.message = value;
        }

        internal ExceptionDetails ExceptionDetails => this.InternalExceptionDetails;
    }
}