// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Tokens.Experimental
{
    /// <summary>
    /// Contains information about a message that is used to generate a message for logging or exceptions.
    /// </summary>
    public class MessageDetail
    {
        private string _message;

        /// <summary>
        /// Creates an instance of <see cref="MessageDetail"/>.
        /// </summary>
        /// <param name="message">The message to be formated.</param>
        public MessageDetail(string message)
        {
            // TODO - paramter validation.
            _message = message;
        }

        /// <summary>
        /// Creates an instance of <see cref="MessageDetail"/>.
        /// </summary>
        /// <param name="formatString">The message to be formated.</param>
        /// <param name="parameters">The parameters for formatting.</param>
        public MessageDetail(string formatString, params object[] parameters)
        {
            // TODO - paramter validation.
            FormatString = formatString;
            Parameters = parameters;
        }

        /// <summary>
        /// Creates a message detail for a null parameter.
        /// </summary>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <returns>A new <see cref="MessageDetail"/>.</returns>
        public static MessageDetail NullParameter(string parameterName)
            => new MessageDetail(LogMessages.IDX10000, LogHelper.MarkAsNonPII(parameterName));

        /// <summary>
        /// Gets the formatted message.
        /// </summary>
        public string Message
        {
            get
            {
                _message ??= LogHelper.FormatInvariant(FormatString, Parameters);
                return _message;
            }
        }

        private string FormatString { get; }

        private object[] Parameters { get; }
    }
}
