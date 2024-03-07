// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Threading.Tasks;
using NuGet.Credentials;
using NuGet.VisualStudio;

namespace NuGet.PackageManagement.VisualStudio
{
    /// <summary>
    /// Find all MEF imports for IVsCredentialProvider, and handle inserting fallback provider
    /// for Dev14
    /// </summary>
    public class VsCredentialProviderImporter
    {
        private readonly Action<Exception, string> _errorDelegate;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dte">DTE instance, used to determine the Visual Studio version.</param>
        /// <param name="errorDelegate">Used to write error messages to the user.</param>
        public VsCredentialProviderImporter(
            Action<Exception, string> errorDelegate)
        {
            _errorDelegate = errorDelegate ?? throw new ArgumentNullException(nameof(errorDelegate));
        }

        // The export from TeamExplorer uses a named contract, so we need to import this one separately.
        // To avoid conflicts with any third-party providers exported using the same contract name,
        // an ImportMany is used instead of a single Import.
        [ImportMany("VisualStudioAccountProvider", typeof(IVsCredentialProvider))]
        public IEnumerable<Lazy<IVsCredentialProvider>> VisualStudioAccountProviders { get; set; }

        // This will import any third-party exports, excluding those with a named contract.
        [ImportMany(typeof(IVsCredentialProvider))]
        public IEnumerable<Lazy<IVsCredentialProvider>> ImportedProviders { get; set; }

        [Import]
        public IVsSolutionManager SolutionManager { get; set; }

        /// <summary>
        /// Plugin providers are entered loaded the same way as other nuget extensions,
        /// matching any extension named CredentialProvider.*.exe.
        /// </summary>
        /// <returns>An enumeration of plugin providers</returns>
        public async Task<IReadOnlyCollection<ICredentialProvider>> GetProvidersAsync()
        {
            var results = new List<ICredentialProvider>();

            await InitializeAsync();

            TryImportCredentialProviders(results, VisualStudioAccountProviders);
            TryImportCredentialProviders(results, ImportedProviders);

            // Ensure imported providers ordering is deterministic
            results.Sort((a, b) => string.Compare(a.GetType().FullName, b.GetType().FullName, StringComparison.Ordinal));

            return results;
        }

        private void TryImportCredentialProviders(
            List<ICredentialProvider> importedProviders,
            IEnumerable<Lazy<IVsCredentialProvider>> credentialProviders)
        {
            if (credentialProviders != null)
            {
                foreach (var credentialProviderFactory in credentialProviders)
                {
                    try
                    {
                        var credentialProvider = credentialProviderFactory.Value;
                        importedProviders.Add(new VsCredentialProviderAdapter(credentialProvider, SolutionManager));
                    }
                    catch (Exception exception)
                    {
                        var targetAssemblyPath = exception.TargetSite.DeclaringType.Assembly.Location;

                        _errorDelegate(
                            exception,
                            string.Format(CultureInfo.CurrentCulture, Strings.CredentialProviderFailed_ImportedProvider, targetAssemblyPath)
                            );
                    }
                }
            }
        }

        private async Task InitializeAsync()
        {
            var componentModel = await ServiceLocator.GetComponentModelAsync();
            // ensure we satisfy our imports
            componentModel?.DefaultCompositionService.SatisfyImportsOnce(this);
        }
    }
}
