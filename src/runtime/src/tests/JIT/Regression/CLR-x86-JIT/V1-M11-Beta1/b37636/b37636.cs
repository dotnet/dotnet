// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
//

namespace Test
{
    using System;

    public class AA
    {
        static float[] m_af = new float[2];

        public static int Main()
        {
            while (m_af[0] < m_af[1])
            {
                try
                {
                    while (0.0f > m_af[0]) { }
                }
                catch (DivideByZeroException) { return -1; }
            }
            return 100;
        }
    }
}
