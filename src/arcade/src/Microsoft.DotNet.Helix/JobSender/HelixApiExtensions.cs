// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.DotNet.Helix.Client
{
    public static class HelixApiExtensions
    {
        public static IJobDefinitionWithType Define(this IJob jobApi)
        {
            return new JobDefinition(jobApi);
        }
    }
}
