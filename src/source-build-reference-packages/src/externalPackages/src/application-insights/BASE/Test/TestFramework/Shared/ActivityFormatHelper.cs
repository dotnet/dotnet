using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Microsoft.ApplicationInsights.TestFramework
{
    internal class ActivityFormatHelper
    {
        public static void DisableW3CFormatInActivity()
        {
            Activity.DefaultIdFormat = ActivityIdFormat.Hierarchical;
            Activity.ForceDefaultIdFormat = true;
        }

        public static void EnableW3CFormatInActivity()
        {
            Activity.DefaultIdFormat = ActivityIdFormat.W3C;
            Activity.ForceDefaultIdFormat = true;
        }
    }
}
