// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace MSTestv2UnitTestExtension
{
    using System;
    using EnvDTE;
    using Microsoft.VisualStudio.TestPlatform.TestGeneration.Model;

    /// <summary>
    /// A unit test project for MSTest unit tests.
    /// </summary>
    public class MSTestv2UnitTestProjectManager : UnitTestProjectManagerBase
    {
        /// <summary>
        /// The service provider to use to get the interfaces required.
        /// </summary>
        private IServiceProvider serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="MSTestv2UnitTestProjectManager"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider to use to get the interfaces required.</param>
        /// <param name="naming">The naming object used to decide how projects, classes and methods are named and created.</param>
        public MSTestv2UnitTestProjectManager(IServiceProvider serviceProvider, INaming naming)
            : base(serviceProvider, naming)
        {
            this.serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Returns the full namespace that contains the test framework code elements for a given source project.
        /// </summary>
        /// <param name="sourceProject">The source project.</param>
        /// <returns>The full namespace that contains the test framework code elements.</returns>
        public override string FrameworkNamespace(Project sourceProject)
        {
            return "Microsoft.VisualStudio.TestTools.UnitTesting";
        }
    }
}
