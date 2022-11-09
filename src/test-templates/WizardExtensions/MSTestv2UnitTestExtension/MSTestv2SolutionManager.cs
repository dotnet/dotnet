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

            TraceLogger.LogInfo("MSTestv2SolutionManager.OnUnitTestProjectCreated: Adding reference to MSTestv2 assemblies through nuget.");

            base.OnUnitTestProjectCreated(unitTestProject, sourceMethod);

            this.EnsureNuGetReference(unitTestProject, "MSTest.TestAdapter", "2.2.10");
            this.EnsureNuGetReference(unitTestProject, "MSTest.TestFramework", "2.2.10");

            VSProject2 vsp = unitTestProject.Object as VSProject2;
            if (vsp != null)
            {
                Reference reference = vsp.References.Find(GlobalConstants.MSTestAssemblyName);
                if (reference != null)
                {
                    TraceLogger.LogInfo("MSTestv2SolutionManager.OnUnitTestProjectCreated: Removing reference to {0}", reference.Name);
                    reference.Remove();
                }
            }
        }
    }
}
