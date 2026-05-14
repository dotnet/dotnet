// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Microsoft.DotNet.Build.Tasks.Workloads.Wix
{
    /// <summary>
    /// Serves as a base class for implementing a <see cref="ToolTask"/> to invoke a WiX command.
    /// </summary>
    public abstract class WixToolTaskBase : ToolTask
    {
        private HashSet<string> _extensions = new();
        private List<string> _preprocessorDefinitions = new();

        /// <summary>
        /// Provides utility methods for constructing a commandline.
        /// </summary>
        protected CommandLineBuilder CommandLineBuilder
        {
            get;
        } = new CommandLineBuilder();

        /// <summary>
        /// Gets the collection of extensions to pass to the underlying tool task.
        /// </summary>
        public IEnumerable<string> Extensions => _extensions;        

        /// <summary>
        /// Gets the collection of preprocessor definitions. Each element represents a single definition. 
        /// For example, "SomeVar=Hello world" defines a preprocessor variable named SomeVar set to "Hello world". The
        /// value of the variable will automatically be quoted when passed to the underlying tool.
        /// </summary>
        public IEnumerable<string> PreprocessorDefinitions => _preprocessorDefinitions;
        
        /// <inheritdoc/>        
        protected override MessageImportance StandardOutputLoggingImportance => MessageImportance.High;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="engine"></param>
        /// <param name="wixToolsetPath">The path where the WiX toolset is located.</param>
        /// <exception cref="ArgumentNullException"></exception>
        protected WixToolTaskBase(IBuildEngine engine, string wixToolsetPath)
        {
            BuildEngine = engine ?? throw new ArgumentNullException(nameof(engine));
            ToolPath = wixToolsetPath;
        }

        /// <summary>
        /// Adds the specified extension to the tool task.
        /// </summary>
        /// <param name="name">The name of the WiX extension. See <see cref="WixExtensions"/> for a list of well known extensions.</param>
        public void AddExtension(string name) =>
            _extensions.Add(name);

        /// <summary>
        /// Removes the specified extension from the tool task.
        /// </summary>
        /// <param name="name">The name of the WiX extension. See <see cref="WixExtensions"/> for a list of well known extensions.</param>
        public void RemoveExtension(string name) =>
            _extensions.Remove(name);
        

        /// <summary>
        /// Adds a new preprocessor definition.
        /// </summary>
        /// <param name="name">The name of the preprocessor variable.</param>
        /// <param name="value">The value of the preprocessor variable.</param>
        public void AddPreprocessorDefinition(string name, string value) =>
            _preprocessorDefinitions.Add($@"{name}={value}");

        /// <inheritdoc />
        protected override string GenerateFullPathToTool() => ToolPath;
    }
}
