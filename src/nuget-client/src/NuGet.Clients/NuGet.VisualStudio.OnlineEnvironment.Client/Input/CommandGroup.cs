// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace NuGet.VisualStudio.OnlineEnvironment.Client
{
    /// <summary>
    /// Specifies the command groups handled in this project.
    /// </summary>
    internal static class CommandGroup
    {
        public const string NuGetOnlineEnvironmentsClientProjectCommandSet = "{282008cc-d0db-45e1-80d1-00fabac5de92}";

        public static readonly Guid NuGetOnlineEnvironmentsClientProjectCommandSetGuid = Guid.Parse(NuGetOnlineEnvironmentsClientProjectCommandSet);
    }
}
