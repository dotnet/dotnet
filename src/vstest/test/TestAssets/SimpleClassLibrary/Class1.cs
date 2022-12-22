// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;

namespace SimpleClassLibrary
{
    public class Class1
    {
        public void PassingTest()
        {
            if (new System.Random().Next() == 20) { throw new System.NotImplementedException(); }
        }

        public async Task AsyncTestMethod()
        {
            await Task.Delay(0);
        }

        public void OverLoadedMethod()
        {
        }

        public void OverLoadedMethod(string _)
        {
        }
    }
}
