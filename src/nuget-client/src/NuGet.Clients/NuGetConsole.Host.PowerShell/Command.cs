// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace NuGetConsole.Host.PowerShell
{
    /// <summary>
    /// Represents a parsed powershell command e.g. "Install-Package el -Version "
    /// </summary>
    public class Command : IEqualityComparer<object>
    {
        // Command arguments by name and index (That's why it's <object, string>)
        // "-Version " would be { "Version", "" } and
        // "-Version" would be { "Version", null }
        // Whitespace is significant wrt completion. We don't want to show intellisense for "-Version" but we do for "-Version "
        public Dictionary<object, string> Arguments { get; private set; }

        // Index of the argument we're trying to get completion for
        public int? CompletionIndex { get; set; }

        // Argument we're trying to get completion for
        public string CompletionArgument { get; set; }

        // Command name
        public string CommandName { get; set; }

        public Command()
        {
            Arguments = new Dictionary<object, string>(this);
        }

        bool IEqualityComparer<object>.Equals(object x, object y)
        {
            if (x == null
                && y == null)
            {
                return true;
            }

            if (x == null
                || y == null)
            {
                return false;
            }

            string xString = x as string;
            string yString = y as string;
            if (xString != null
                && yString != null)
            {
                return xString.Equals(yString, StringComparison.OrdinalIgnoreCase);
            }

            return x.Equals(y);
        }

        int IEqualityComparer<object>.GetHashCode(object obj)
        {
            return obj == null ? 0 : obj.GetHashCode();
        }
    }
}
