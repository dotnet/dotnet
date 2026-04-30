// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using SimpleTestAdapter;

namespace SimpleTestProject4;

public class UnitTest1
{
    [SimpleTest]
    public void PassingTest()
    {
    }

    [SimpleTest]
    public void FailingTest()
    {
        SimpleAssert.Fail();
    }

    [SimpleTest]
    public void AnotherPassingTest()
    {
    }
}
