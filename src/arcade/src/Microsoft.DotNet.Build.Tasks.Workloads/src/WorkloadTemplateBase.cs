// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.IO;

namespace Microsoft.DotNet.Build.Tasks.Workloads
{
    /// <summary>
    /// Base class to generate project templates and source files to produce workload artifacts.
    /// </summary>
    public abstract class WorkloadTemplateBase
    {
        /// <summary>
        /// List of source files that have been added to the template.
        /// </summary>
        private List<string> _files = new();

        /// <summary>
        /// The root intermediate output directory. 
        /// </summary>
        public string OutputPath
        {
            get;
            init;
        }

        /// <summary>
        /// Dictionary to map replacement tokens and values.
        /// </summary>
        protected Dictionary<string, string> ReplacementTokens
        {
            get;
        } = new();

        /// <summary>
        /// List of source files that have been added to the template.
        /// </summary>
        protected IEnumerable<string> Files => _files;

        /// <summary>
        /// The path of the root directory where the template will be generated.
        /// </summary>
        protected string TemplateOutputPath
        {
            get;
            init;
        }

        /// <summary>
        /// The file system path where source files are generated.
        /// </summary>
        /// <remarks>
        /// Derived classes may extend the path to include additional directories as needed.
        /// </remarks>
        protected string SourcePath
        {
            get;
            init;
        }

        /// <summary>
        /// The file system path where intermediate outputs are generated.
        /// </summary>
        protected string IntermediateOutputPath
        {
            get;
            init;
        }

        /// <summary>
        /// Creates a new <see cref="WorkloadTemplateBase"/> instance.
        /// </summary>
        /// <param name="outputPath">The file system path where the template will be generated. </param>
        public WorkloadTemplateBase(string outputPath)
        {
            OutputPath = outputPath;
            SourcePath = Path.Combine(OutputPath, "src");
            IntermediateOutputPath = Path.Combine(OutputPath, "obj");
        }

        /// <summary>
        /// Builds the template.
        /// </summary>
        public virtual void Build()
        {

        }

        /// <summary>
        /// Generates the template content like project and source files.
        /// </summary>
        /// <returns>The path to the project file if applicable.</returns>
        public virtual string Create() => "";        

        /// <summary>
        /// Extracts the specified template file into the source directory and optionally replace tokens.
        /// </summary>
        /// <param name="templateFilename">The name of the template file to add to the source directory.</param>
        /// <param name="replaceTokens">When true, replace tokens in the generated file.</param>
        /// <returns>The full path of the file that was added.</returns>
        public string AddFile(string templateFilename, bool replaceTokens = true) =>
            AddFile(templateFilename, templateFilename, replaceTokens);

        /// <summary>
        /// Extracts the specified template file into the source directory using the specified filename and 
        /// optionally replace tokens.
        /// </summary>
        /// <param name="templateFilename">The name of the template file to add.</param>
        /// <param name="destinationFilename">The name of the file to be created in the source directory.</param>
        /// <param name="replaceTokens">When true, replace tokens in the generated file.</param>
        /// <returns>The full path of the file that was added.</returns>
        public string AddFile(string templateFilename, string destinationFilename, bool replaceTokens = true)
        {
            string templatePath = EmbeddedTemplates.Extract(templateFilename, SourcePath, destinationFilename);

            if (replaceTokens)
            {
                Utils.StringReplace(templatePath, ReplacementTokens);
            }

            AddSourceFile(templatePath);

            return templatePath;
        }

        /// <summary>
        /// Add an existing file to the list of source files associated with the template.
        /// </summary>
        /// <param name="path">The file system path of the file to add.</param>
        public void AddSourceFile(string path) => _files.Add(path);
    }
}
