// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace MSTestv2UnitTestExtension
{
    using System;
    using EnvDTE;
    using EnvDTE80;
    using Microsoft.VisualStudio.TestPlatform.TestGeneration;
    using Microsoft.VisualStudio.TestPlatform.TestGeneration.Data;
    using Microsoft.VisualStudio.TestPlatform.TestGeneration.Logging;
    using Microsoft.VisualStudio.TestPlatform.TestGeneration.Model;
    using VSLangProj;
    using VSLangProj80;

    /// <summary>
    /// A solution manager for MSTestv2 unit tests.
    /// </summary>
    public class MSTestv2SolutionManager : SolutionManagerBase
    {
        /// <summary>
        /// The service provider to use to get the interfaces required.
        /// </summary>
        private IServiceProvider serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="MSTestv2SolutionManager"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider to use to get the interfaces required.</param>
        /// <param name="naming">The naming object used to decide how projects, classes and methods are named and created.</param>
        /// <param name="directory">The directory object to use for directory operations.</param>
        public MSTestv2SolutionManager(IServiceProvider serviceProvider, INaming naming, IDirectory directory)
            : base(serviceProvider, naming, directory)
        {
            this.serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Performs any preparatory tasks that have to be done after a new unit test project has been created.
        /// </summary>
        /// <param name="unitTestProject">The <see cref="Project"/> of the unit test project that has just been created.</param>
        /// <param name="sourceMethod">The <see cref="CodeFunction2"/> of the source method that is to be unit tested.</param>
        protected override void OnUnitTestProjectCreated(Project unitTestProject, CodeFunction2 sourceMethod)
        {
            if (unitTestProject == null)
            {
                throw new ArgumentNullException("unitTestProject");
            }
            
            base.OnUnitTestProjectCreated(unitTestProject, sourceMethod);

            // Note: we don't add the test framework reference since it is already included in the test project template
        }
    }
}
