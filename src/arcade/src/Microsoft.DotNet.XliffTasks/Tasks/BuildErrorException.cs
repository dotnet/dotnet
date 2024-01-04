// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace XliffTasks.Tasks
{
    internal sealed class BuildErrorException : Exception
    {
        /// <summary>
        /// The file associated with this <see cref="BuildErrorException"/>.
        /// </summary>
        public string RelatedFile { get; set;}

        public BuildErrorException(string message) : base(message)
        {
        }

    }
}
