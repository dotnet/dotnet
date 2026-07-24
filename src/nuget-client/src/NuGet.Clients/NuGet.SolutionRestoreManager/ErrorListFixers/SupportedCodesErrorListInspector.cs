// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.Shell.TableControl;
using Microsoft.VisualStudio.Shell.TableManager;

namespace NuGet.SolutionRestoreManager.ErrorListFixers
{
    internal sealed class SupportedCodesErrorListInspector : IErrorListEntryInspector
    {
        private readonly ISet<string> _supportedCodes;

        public SupportedCodesErrorListInspector(ISet<string> supportedCodes)
        {
            _supportedCodes = supportedCodes ?? throw new ArgumentNullException(nameof(supportedCodes));
        }

        public bool IsSupportedCode(ITableEntryHandle entry)
        {
            return entry != null
                && TryGetErrorCode(entry, out string errorCode)
                && _supportedCodes.Contains(errorCode);
        }

        /// <summary>
        /// Attempts to read the error code from the <paramref name="entry"/>.
        /// </summary>
        /// <param name="entry">The Error List entry to inspect.</param>
        /// <param name="errorCode">The error code when the method returns <see langword="true"/>; otherwise <see cref="string.Empty"/>.</param>
        /// <returns><see langword="true"/> if an error code was read; otherwise <see langword="false"/>.</returns>
        internal static bool TryGetErrorCode(ITableEntryHandle entry, out string errorCode)
        {
            errorCode = string.Empty;

            if (entry == null)
            {
                return false;
            }

            object value;
            if (!entry.TryGetValue(StandardTableKeyNames.ErrorCode, out value))
            {
                return false;
            }

            if (value is not string code || string.IsNullOrEmpty(code))
            {
                return false;
            }

            errorCode = code;
            return true;
        }
    }
}
