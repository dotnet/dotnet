using System;
using System.Text;

namespace Microsoft.DotNet.Helix.Client
{
    /// <summary>
    /// Work item definition that lacks required information
    /// about payload to execute against.
    /// </summary>
    public interface IWorkItemDefinitionWithPayload
    {
        /// <summary>
        /// Specify payload by pointing to URI with archive present.
        /// </summary>
        /// <returns>Fluent workitem builder.</returns>
        IWorkItemDefinition WithPayloadUri(Uri payloadUri);

        /// <summary>
        /// Adds ad-hoc specified list of files as a payload.
        /// Directory structure won't be preserved.
        /// </summary>
        /// <param name="files">The files to archive and add as payload.</param>
        /// <returns>Fluent workitem builder.</returns>
        IWorkItemDefinition WithFiles(params string[] files);

        /// <summary>
        /// Adds whole directory as a payload, archived.
        /// </summary>
        /// <param name="directory">Path to the directory.</param>
        /// <returns>Fluent workitem builder.</returns>
        IWorkItemDefinition WithDirectoryPayload(string directory);

        /// <summary>
        /// Adds whole directory as a payload, archived.
        /// </summary>
        /// <param name="directory">Path to the directory.</param>
        /// <param name="includeDirectoryName">
        /// <see langword="true"/> if the resulting archive should contain a top
        /// level folder with the content inside; otherwise, <see langword="false"/>
        /// to place the content directly within the archive.
        /// </param>
        /// <returns>Fluent workitem builder.</returns>
        IWorkItemDefinition WithDirectoryPayload(string directory, bool includeDirectoryName);

        /// <summary>
        /// Adds whole directory as a payload, archived.
        /// </summary>
        /// <param name="directory">Path to the directory.</param>
        /// <param name="archiveEntryPrefix">Prefix paths inside of the archive by arbitrary subpath.</param>
        /// <returns>Fluent workitem builder.</returns>
        IWorkItemDefinition WithDirectoryPayload(string directory, string archiveEntryPrefix);

        /// <summary>
        /// Adds archive as a payload.
        /// </summary>
        /// <param name="archive">Path to the archive.</param>
        /// <returns>Fluent workitem builder.</returns>
        IWorkItemDefinition WithArchivePayload(string archive);

        /// <summary>
        /// Provide single file payload content directly.
        /// </summary>
        /// <param name="name">Name of the file.</param>
        /// <param name="content">Content of the file.</param>
        /// <returns>Fluent workitem builder.</returns>
        IWorkItemDefinition WithSingleFilePayload(string name, string content);

        /// <summary>
        /// Provide single file payload content directly.
        /// </summary>
        /// <param name="name">Name of the file.</param>
        /// <param name="content">Content of the file.</param>
        /// <param name="encoding">Encoding to use for the file content.</param>
        /// <returns>Fluent workitem builder.</returns>
        IWorkItemDefinition WithSingleFilePayload(string name, string content, Encoding encoding);

        /// <summary>
        /// Provide single file payload content directly.
        /// </summary>
        /// <param name="name">Name of the file.</param>
        /// <param name="content">Content of the file.</param>
        /// <returns>Fluent workitem builder.</returns>
        IWorkItemDefinition WithSingleFilePayload(string name, byte[] content);

        /// <summary>
        /// This work item does not need any data to work with.
        /// </summary>
        /// <returns>Fluent workitem builder.</returns>
        IWorkItemDefinition WithEmptyPayload();
    }
}
