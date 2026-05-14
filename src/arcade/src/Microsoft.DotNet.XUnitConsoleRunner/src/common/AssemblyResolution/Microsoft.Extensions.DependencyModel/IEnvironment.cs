// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Internal.Microsoft.Extensions.DependencyModel
{
    internal interface IEnvironment
    {
        string GetEnvironmentVariable(string name);
    }
}
