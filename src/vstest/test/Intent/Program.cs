// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Intent.Console;

public class Program
{
    public static void Main(string[] paths)
    {
        Runner.Run(paths, new ConsoleLogger());
    }
}
