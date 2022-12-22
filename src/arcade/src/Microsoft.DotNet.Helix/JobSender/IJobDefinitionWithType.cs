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
