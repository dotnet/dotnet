namespace Microsoft.DotNet.Helix.Client
{
    /// <summary>
    /// Job definition that lacks required information about
    /// queue that the job will run at.
    /// </summary>
    public interface IJobDefinitionWithTargetQueue
    {
        /// <summary>
        /// The Helix queue this job should run on.
        /// </summary>
        /// <param name="queueId">Queue name like "Windows.10.Arm64.Open" or "Debian.9.Amd64".</param>
        IJobDefinition WithTargetQueue(string queueId);
    }
}
