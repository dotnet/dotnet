// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace Microsoft.Deployment.Launcher
{
    [Serializable]
    public class LauncherException : Exception
    {
        public LauncherException()
        {
        }

        public LauncherException(string message)
            : base(message)
        {
        }

        public LauncherException(string format, params object[] args)
            : base(string.Format(format, args))
        {
        }

        public LauncherException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected LauncherException(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext)
        {
            throw new NotImplementedException();
        }
    }
}
