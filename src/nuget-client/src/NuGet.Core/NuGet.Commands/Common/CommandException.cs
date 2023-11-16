// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace NuGet.Commands
{
    [Serializable]
    public class CommandException : Exception
    {
        public CommandException()
        {
        }

        public CommandException(string message)
            : base(message)
        {
        }

        public CommandException(string format, params object[] args)
            : base(string.Format(CultureInfo.CurrentCulture, format, args))
        {
        }

        public CommandException(string message, Exception exception)
            : base(message, exception)
        {
        }

#if NET8_0_OR_GREATER
        [Obsolete(DiagnosticId = "SYSLIB0051")] // https://github.com/dotnet/docs/issues/34893
#endif
        protected CommandException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
