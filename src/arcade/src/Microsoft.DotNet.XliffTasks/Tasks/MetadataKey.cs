// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace XliffTasks
{
    internal static class MetadataKey
    {
        public const string DependentUpon = nameof(DependentUpon);
        public const string Link = nameof(Link);
        public const string LogicalName = nameof(LogicalName);
        public const string ManifestResourceName = nameof(ManifestResourceName);
        public const string SourceDocumentPath = nameof(SourceDocumentPath);
        public const string XlfLanguage = nameof(XlfLanguage);
        public const string XlfSource = nameof(XlfSource);
        public const string XlfSourceFormat = nameof(XlfSourceFormat);

        /// <summary>
        /// Specifies the naming convention for the translated file.
        /// By default (false or unspecified) the translation process takes an item (e.g. .resx) and generates a translated file 
        /// {project name}.xlf\{name}.{lang}.resx in the obj directory of the project.
        ///
        /// When true it specifies that instead of inserting the language name into the file name we preserve the file name and
        /// store the file in a {lang} subdirectory, e.g. {lang}\string-resources.json.
        /// </summary>
        public const string XlfPreserveFileName = nameof(XlfPreserveFileName);

        public const string XlfTranslatedFilename = nameof(XlfTranslatedFilename);
        public const string XlfTranslatedFullPath = nameof(XlfTranslatedFullPath);
    }
}
