// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace NuGet.Commands
{
    /// <summary>
    /// DG v2 related validation error.
    /// </summary>
    public class RestoreSpecException : Exception
    {
        public IEnumerable<string> Files { get; }

        private RestoreSpecException(string message, IEnumerable<string> files, Exception innerException)
                : base(message, innerException)
        {
            Files = files;
        }

        public static RestoreSpecException Create(string message, IEnumerable<string> files)
        {
            return Create(message, files, innerException: null);
        }

        public static RestoreSpecException Create(string message, IEnumerable<string> files, Exception innerException)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            if (files == null)
            {
                throw new ArgumentNullException(nameof(files));
            }

            files = files.Where(path => !string.IsNullOrEmpty(path)).Distinct(StringComparer.Ordinal);

            string completeMessage = null;

            if (files.Any())
            {
                completeMessage = string.Format(
                    CultureInfo.CurrentCulture,
                    Strings.InvalidRestoreInputWithFiles,
                    message,
                    string.Join(", ", files));
            }
            else
            {
                completeMessage = string.Format(
                    CultureInfo.CurrentCulture,
                    Strings.InvalidRestoreInput,
                    message);
            }

            return new RestoreSpecException(completeMessage, files, innerException);
        }
    }
}
