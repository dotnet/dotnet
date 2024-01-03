// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.DotNet.Helix.Client
{
    /// <summary>
    /// Job definition that lacks required Type information.
    /// </summary>
    public interface IJobDefinitionWithType
    {
        /// <summary>
        /// Assigns type to the job. This value is used to filter jobs in Kusto and in the Helix job API.
        /// </summary>
        IJobDefinitionWithTargetQueue WithType(string type);
    }
}
