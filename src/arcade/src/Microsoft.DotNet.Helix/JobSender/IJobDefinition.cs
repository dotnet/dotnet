using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DotNet.Helix.Client
{
    /// <summary>
    /// <para>Job definition with almost all required information already specified
    /// that can be either completed by calling <c>SendAsync</c> or further extended.</para>
    /// 
    /// <para>Note that when not running inside Azure DevOps with its set of environment
    /// variables that JobSender captures, Source need to be provided before sending the job.
    /// Also, for non-authenticated scenarios it's required to provide Creator.</para>
    /// </summary>
    public interface IJobDefinition
    {
        /// <summary>
        /// Adds a single work item into the job.
        /// </summary>
        /// <param name="workItemName">Name of the work item.</param>
        /// <returns>Work item fluent builder.</returns>
        IWorkItemDefinitionWithCommand DefineWorkItem(string workItemName);

        /// <summary>
        /// Adds several correlation payloads from URIs.
        /// </summary>
        /// <returns>Fluent job builder.</returns>
        IJobDefinition WithCorrelationPayloadUris(params Uri[] payloadUris);

        /// <summary>
        /// <para>Adds several correlation payloads and corresponding destinations
        /// from URIs.</para>
        /// 
        /// <para>Destination is the place where the payload is extracted
        /// on the target Helix system. It cannot be an absolute path
        /// and is relative to the $HELIX_CORRELATION_PAYLOAD directory
        /// on the machine.</para>
        /// </summary>
        /// <returns>Fluent job builder.</returns>
        IJobDefinition WithCorrelationPayloadUris(IDictionary<Uri, string> payloadUrisWithDestinations);

        /// <summary>
        /// Adds whole directory as a correlation payload, archived.
        /// </summary>
        /// <param name="directory">
        /// The directory to be archived and sent as a correlation payload.
        /// </param>
        /// <param name="destination">
        /// The path on the target system for the correlation payload to be extracted.
        /// It cannot be an absolute path and is relative to the $HELIX_CORRELATION_PAYLOAD
        /// directory on the machine.
        /// </param>
        /// <returns>Fluent job builder.</returns>
        IJobDefinition WithCorrelationPayloadDirectory(string directory, string destination = "");

        /// <summary>
        /// Adds whole directory as a correlation payload, archived.
        /// </summary>
        /// <param name="directory">Path to the directory.</param>
        /// <param name="includeDirectoryName">
        /// <see langword="true"/> if the resulting archive should contain a top
        /// level folder with the content inside; otherwise, <see langword="false"/>
        /// to place the content directly within the archive.
        /// </param>
        /// <param name="destination">
        /// The path on the target system for the correlation payload to be extracted.
        /// It cannot be an absolute path and is relative to the $HELIX_CORRELATION_PAYLOAD
        /// directory on the machine.
        /// </param>
        /// <returns>Fluent job builder.</returns>
        IJobDefinition WithCorrelationPayloadDirectory(string directory, bool includeDirectoryName, string destination = "");

        /// <summary>
        /// Adds whole directory as a correlation payload, archived.
        /// </summary>
        /// <param name="directory">Path to the directory.</param>
        /// <param name="archiveEntryPrefix">Prefix paths inside of the archive by arbitrary subpath.</param>
        /// <param name="destination">
        /// The path on the target system for the correlation payload to be extracted.
        /// It cannot be an absolute path and is relative to the $HELIX_CORRELATION_PAYLOAD
        /// directory on the machine.
        /// </param>
        /// <returns>Fluent job builder.</returns>
        IJobDefinition WithCorrelationPayloadDirectory(string directory, string archiveEntryPrefix, string destination);

        /// <summary>
        /// Adds archive as a correlation payload.
        /// </summary>
        /// <param name="archive">Path to the archive.</param>
        /// <param name="destination">
        /// The path on the target system for the correlation payload to be extracted.
        /// It cannot be an absolute path and is relative to the $HELIX_CORRELATION_PAYLOAD
        /// directory on the machine.
        /// </param>
        /// <returns>Fluent job builder.</returns>
        IJobDefinition WithCorrelationPayloadArchive(string archive, string destination = "");

        /// <summary>
        /// Adds ad-hoc specified list of files as a correlation payload.
        /// Directory structure won't be preserved.
        /// </summary>
        /// <param name="files">The files to archive and add as correlation payload.</param>
        /// <returns>Fluent job builder.</returns>
        IJobDefinition WithCorrelationPayloadFiles(params string[] files);

        /// <summary>
        /// Adds ad-hoc specified list of files as a correlation payload.
        /// Directory structure won't be preserved.
        /// </summary>
        /// <param name="files">Paths to archives.</param>
        /// <param name="destination">
        /// The path on the target system for the correlation payload to be extracted.
        /// It cannot be an absolute path and is relative to the $HELIX_CORRELATION_PAYLOAD
        /// directory on the machine.
        /// </param>
        /// <returns>Fluent job builder.</returns>
        IJobDefinition WithCorrelationPayloadFiles(IList<string> files, string destination);

        /// <summary>
        /// Sets the Helix Source property.
        /// </summary>
        /// <returns>Fluent job builder.</returns>
        IJobDefinition WithSource(string source);

        /// <summary>
        /// Upserts a value into the dictionary of properties
        /// attached to the job. Helix infrastructure does not care
        /// about these properties and you can use them for your
        /// own ends.
        /// </summary>
        /// <returns>Fluent job builder.</returns>
        IJobDefinition WithProperty(string key, string value);

        /// <summary>
        /// <para>For non-authenticated scenarios you have to provide information
        /// about creator of the job.</para>
        /// 
        /// <para>Beware using this for authenticated calls. The value is going
        /// to be provided by the user corresponding to your auth and
        /// an attempt to set anything else will make the job fail.</para>
        /// </summary>
        /// <returns>Fluent job builder.</returns>
        IJobDefinition WithCreator(string creator);

        /// <summary>
        /// Sets custom container name for Azure blob storage to store the payload.
        /// Unless <see cref="WithStorageAccountConnectionString"/> is used, container
        /// name will be modified to ensure uniqueness.
        /// </summary>
        /// <returns>Fluent job builder.</returns>
        IJobDefinition WithContainerName(string targetContainerName);

        /// <summary>
        /// Specifies custom connection string to Azure storage account of your choice.
        /// It will be used for storing blobs of payloads and job lists.
        /// </summary>
        /// <returns>Fluent job builder.</returns>
        IJobDefinition WithStorageAccountConnectionString(string accountConnectionString);

        /// <summary>
        /// Specifies name of the container for storage of results. Unless
        /// <see cref="WithStorageAccountConnectionString"/> is used, container
        /// name will be modified to ensure uniqueness.
        /// </summary>
        /// <returns>Fluent job builder.</returns>
        IJobDefinition WithResultsContainerName(string resultsContainerName);

        /// <summary>
        /// Sets max automatic retry of work items which do not return 0.
        /// </summary>
        /// <returns>Fluent job builder.</returns>
        IJobDefinition WithMaxRetryCount(int? maxRetryCount);

        /// <summary>
        /// <para>Sends the fully specified job to execution.</para>
        /// 
        /// <para>This includes upload of all the provided correlation data, but does not
        /// include waiting for the task to fully execute in Helix.</para>
        /// 
        /// <para>You can use WaitAsync method of the ISentJob return value to wait for completion.</para>
        /// </summary>
        /// <returns>Job accepted by Helix.</returns>
        Task<ISentJob> SendAsync(Action<string> log = null, CancellationToken cancellationToken = default);
    }
}
