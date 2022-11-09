// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

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
