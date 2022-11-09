// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Runtime.Serialization;

namespace NuGet.PackageManagement.PowerShellCmdlets
{
    [Serializable]
    public class PackageSourceException : Exception
    {
        public PackageSourceException(string message)
            : base(message)
        {
        }

        protected PackageSourceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
