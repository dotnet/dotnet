// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace NuGet.SolutionRestoreManager
{
    internal static class GuidList
    {
        private const string guidNuGetDialogCmdSetString = "25fd982b-8cae-4cbd-a440-e03ffccde106";

        public static readonly Guid guidNuGetDialogCmdSet = new Guid(guidNuGetDialogCmdSetString);

        public static readonly Guid guidNuGetSBMEvents = Guid.Parse("cb59213f-41c3-4d56-85b6-cc1546c5ed07");
    }
}
