// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace Internal.Microsoft.Extensions.DependencyModel
{
    internal class EnvironmentWrapper : IEnvironment
    {
        public static IEnvironment Default = new EnvironmentWrapper();

        public string GetEnvironmentVariable(string name)
        {
            return Environment.GetEnvironmentVariable(name);
        }
    }
}
