// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace CodeCoverageTest
{
    public class Logic
    {
        public Logic()
        {
        }

        public int Abs(int x)
        {
            return (x < 0) ? (-x) : (x);
        }

        public int Sign(int x)
        {
            if (x < 0) return -1;
            if (x > 0) return 1;

            return 0;
        }
    }
}
