// Copyright (c) Microsoft Corporation. All rights reserved.

namespace Microsoft.VisualStudio.Test.Project
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Text;
    using Microsoft.VisualStudio.ComponentModelHost;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.TemplateWizard;
    using IOleServiceProvider = Microsoft.VisualStudio.OLE.Interop.IServiceProvider;

    [SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable",
        Justification = "This is called by wizard framework which does not call IDisposable at all.")]
    public class FrameworkTemplateWizard : IWizard
    {
        private IOleServiceProvider _oleServiceProvider;
        private ServiceProvider _serviceProvider;
        private IComponentModel _componentModel;
        private bool _importsAreLoaded = false;
        private Dictionary<string, string> _replacementsDictionary;
        private string _thisVstemplateFile;
        private string _customParams;

        [Import(typeof(FrameworkTemplateWizardData), RequiredCreationPolicy = CreationPolicy.NonShared)]
        private FrameworkTemplateWizardData WizardData { get; set; }

        public EnvDTE.DTE DTE
        {
            get
            {
                ThreadHelper.ThrowIfNotOnUIThread();
                return _serviceProvider.GetService(typeof(EnvDTE.DTE)) as EnvDTE.DTE;
            }
        }

        public EnvDTE90.Solution3 Solution
        {
            get
            {
                ThreadHelper.ThrowIfNotOnUIThread();
                return (EnvDTE90.Solution3)DTE.Solution;
            }
        }

        /// <summary>
        /// IWizard
        /// First function the wizard framework calls. Just remember some values like the replacementsDictionary
        /// </summary>
        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            Initialize(automationObject);

            _replacementsDictionary = replacementsDictionary;

            // Custom parameters should have one entry, the full path to this vstemplate file
            Debug.Assert(customParams.Length >= 1);
            if (customParams.Length >= 1 && customParams[0] is string)
            {
                _thisVstemplateFile = (string)customParams[0];

                // Pass additional replacement parameters, they get passed in 'customParams' parameter.
                // Save customParams so that we can pass them on further
                StringBuilder customParamsBuilder = new StringBuilder();
                for (int i = 1; i < customParams.Length; i++)
                {
                    if (customParams[i] is string)
                    {
                        customParamsBuilder.Append('|');
                        customParamsBuilder.Append((string)customParams[i]);
                    }
                }

                _customParams = customParamsBuilder.ToString();
                WizardData.Initialize(_thisVstemplateFile, GetString("$wizarddata$"));

                return;
            }

            throw new InvalidOperationException(FrameworkTemplateWizardResources.InvalidWizardData);

        }

        /// <summary>
        /// IWizard
        /// This is where we do all the work of creating the intermediate folders and creating
        /// the actual project
        /// </summary>
        public void RunFinished()
        {
            try
            {
                ThreadHelper.ThrowIfNotOnUIThread();

                // Get the path the user said to create the project
                string projectDirectory = GetString("$destinationdirectory$");
                string projectName = GetString("$projectname$");
                string targetFramework = GetString("$targetframeworkversion$");
                string projectTemplatePath = WizardData.GetTemplatePathForFramework(targetFramework);

                if (string.IsNullOrEmpty(projectTemplatePath))
                {
                    throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, FrameworkTemplateWizardResources.NoMatchingTemplate, targetFramework));
                }

                // Append saved custom parameters
                string projectTemplateWithCustomParams = projectTemplatePath + _customParams;

                // Now create the real project using the solution
                Solution.AddFromTemplate(projectTemplateWithCustomParams, projectDirectory, projectName, Exclusive: false);
            }
            finally
            {
                _oleServiceProvider = null;
            }
        }


        private void Initialize(object automationObject)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            _oleServiceProvider = automationObject as IOleServiceProvider;
            Contract.Assert(_oleServiceProvider != null);
            _serviceProvider = new ServiceProvider(_oleServiceProvider);

            EnsureSatisfyImports();
        }

        /// <summary>
        /// Ensure MEF imports were satisfied once
        /// </summary>
        private void EnsureSatisfyImports()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            if (_importsAreLoaded)
            {
                return;
            }

            try
            {
                if (ServiceProvider.GlobalProvider.GetService(typeof(SComponentModel)) is IComponentModel componentModel)
                {
                    _componentModel = componentModel;
                    ICompositionService compositionService = _componentModel.DefaultCompositionService;
                    compositionService.SatisfyImportsOnce(this); 
                }
            }
            finally
            {
                _importsAreLoaded = true;
            }
        }

        /// <summary>
        /// Helper to get a string value from the replacements dictionary
        /// </summary>
        private string GetString(string key)
        {
            string value;
            _replacementsDictionary.TryGetValue(key, out value);
            return value;
        }

        /// <summary>
        /// IWizard
        /// A number of interfaces we don't care about
        /// </summary>
        public void BeforeOpeningFile(EnvDTE.ProjectItem projectItem)
        {
        }

        public void ProjectFinishedGenerating(EnvDTE.Project project)
        {
        }

        public void ProjectItemFinishedGenerating(EnvDTE.ProjectItem projectItem)
        {
        }

        public bool ShouldAddProjectItem(string filePath)
        {
            return true;
        }
    }
}
