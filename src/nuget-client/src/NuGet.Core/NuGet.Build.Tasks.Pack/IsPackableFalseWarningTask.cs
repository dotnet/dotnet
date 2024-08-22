// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Globalization;
using NuGet.Common;

namespace NuGet.Build.Tasks.Pack
{
    public class IsPackableFalseWarningTask : Microsoft.Build.Utilities.Task
    {
        public ILogger Logger => new MSBuildLogger(Log);
        public override bool Execute()
        {
            Logger.LogWarning(string.Format(CultureInfo.CurrentCulture,
                    Strings.IsPackableFalseError));
            return true;
        }
    }
}
