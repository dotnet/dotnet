namespace NuGet.Protocol
{
    public enum FeedType
    {
        /// <summary>
        /// Undetermined type
        /// </summary>
        Undefined = 0,

        /// <summary>
        /// V2 OData protocol, ex: https://www.nuget.org/api/v2/
        /// </summary>
        HttpV2 = 1 << 0,

        /// <summary>
        /// V3 Json protocol, ex: https://api.nuget.org/v3/index.json
        /// </summary>
        HttpV3 = 1 << 1,

        /// <summary>
        /// Flat folder of nupkgs
        /// </summary>
        FileSystemV2 = 1 << 2,

        /// <summary>
        /// Version folder structure used for project.json
        /// </summary>
        FileSystemV3 = 1 << 3,

        /// <summary>
        /// Unzipped folder of nupkgs used by project templates
        /// </summary>
        FileSystemUnzipped = 1 << 4,

        /// <summary>
        /// Packages.config packages folder format
        /// </summary>
        FileSystemPackagesConfig = 1 << 5,

        /// <summary>
        /// Undetermined folder type. Occurs when the folder is empty
        /// or does not exist yet.
        /// </summary>
        FileSystemUnknown = 1 << 10
    }
}
