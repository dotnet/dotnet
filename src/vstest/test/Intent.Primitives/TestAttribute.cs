// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Intent;

[AttributeUsage(AttributeTargets.Method)]
public class TestAttribute : Attribute
{
    public TestAttribute(string name)
    {
        Name = name;
    }

    public string Name { get; }
}
