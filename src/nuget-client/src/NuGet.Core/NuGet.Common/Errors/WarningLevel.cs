// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.Common
{
    /// <summary>
    /// These are Warning Levels used by NuGet while throwing warnings.
    /// These logically correspond to .NET spec at https://msdn.microsoft.com/en-us/library/13b90fz7(v=vs.140).aspx
    /// 
    /// We do not have a level 0 as that has no logical meaning of having a warning with level 0.
    /// 
    /// Severe - This should be used to throw warnings that are just short of being an error.
    /// 
    /// Important - Lower level than severe. 
    /// 
    /// Minimal - Lower level than important. 
    /// 
    /// Default - Lowest level of warnings. 
    ///           Default NuGet logging will ignore these warnings.
    /// </summary>
    public enum WarningLevel
    {
        Severe = 1,
        Important = 2,
        Minimal = 3,
        Default = 4
    }
}
