// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace NuGet.Tests.Apex
{
    public class ApexTestRequirementsFixture : IDisposable
    {
        private static bool _isInitialized = false;

        public ApexTestRequirementsFixture()
        {
            if (_isInitialized)
            {
                return;
            }

            TestInitialize();
            _isInitialized = true;
        }

        public void Dispose()
        {
            TestCleanup();
        }

        private void TestCleanup()
        {
            // test clean up code
        }

        private void TestInitialize()
        {
            // test initialization code
        }

    }
}
