// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Text;

namespace StructInClass
{
    internal class StructInClass
    {
    }
    internal class TestClass
    {
        public struct TheStruct
        {
            public string fieldinStruct;
        }

        private static void StructTaker_Inline(TheStruct Struct)
        {
            Struct.fieldinStruct = "xyz";
        }

        private static int Main()
        {
            try
            {
                TestClass newobj = new TestClass();

                TheStruct s = new TheStruct();

                StructTaker_Inline(s);

                return 100;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return 666;
            }
        }
    }
}
