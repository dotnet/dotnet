// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


using System;
using System.Globalization;


namespace DefaultNamespace
{
    public class cinfo
    {
        public static int Main()
        {
            Console.Out.WriteLine("Char Class tests");

            VTestIsPrintable();
            VTestIsTitleCase();
            return 100;
        }
        internal static void VTestIsPrintable()
        {
            for (char ch = '\x0'; ch <= '\x255'; ch++)
            {
                bool bResult = (ch) == '\x23';
            }
        }

        internal static void VTestIsTitleCase()
        {
            for (char ch = '\x0'; ch <= '\x255'; ch++)
            {
                Console.Out.Write("Char.IsTitleCase('" + ch + "')=");
                bool bResult = (ch) == '\x25';
            }
        }
    }
}
