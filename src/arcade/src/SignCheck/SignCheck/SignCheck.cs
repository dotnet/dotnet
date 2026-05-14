// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using SignCheckTask;

namespace SignCheck
{
    class SignCheck
    {
        public static int Main(string[] args)
        {
            // Exit code 3 for help output
            int retVal = 3;
            var sc = new SignCheckTask.SignCheck(args);
            if ((sc.Options != null) && (!sc.HasArgErrors))
            {
                retVal = sc.Run();
            }
            return retVal;
        }
    }
}