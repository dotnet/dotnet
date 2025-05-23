// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Utilities.UnifiedSettings;
using NuGet.Configuration;
using NuGet.PackageManagement.VisualStudio.IDE;

namespace NuGet.PackageManagement.VisualStudio.Options
{
    [Guid("4F0DC114-28A6-4888-84E7-766D6E7DE456")]
    public class ConfigurationFilesPage : NuGetExternalSettingsProvider, IExternalArrayItemCommandsProvider
    {
        private const string MonikerConfigurationFiles = "configurationFiles";
        private readonly OpenFileArrayItemCommand _openFileArrayItemCommand;

        public ConfigurationFilesPage(VSSettings vsSettings)
            : base(vsSettings)
        {
            var documentOpener = new VSDocumentOpener();
            _openFileArrayItemCommand = new OpenFileArrayItemCommand(documentOpener);
        }

        public override Task<ExternalSettingOperationResult<T>> GetValueAsync<T>(string moniker, CancellationToken cancellationToken)
        {
            switch (moniker)
            {
                case MonikerConfigurationFiles: return LoadConfigurationFilePathsOrThrow<T>(_vsSettings);
                default: break;
            }

            // Shouldn't happen as these are monikers we declared in registration.json.
            throw new InvalidOperationException();
        }

        /// <summary>
        /// Not supported, as Configuration Files has no user-settable values.
        /// </summary>
        /// <exception cref="InvalidOperationException">Always thrown.</exception>
        public override Task<ExternalSettingOperationResult> SetValueAsync<T>(string moniker, T value, CancellationToken cancellationToken)
        {
            throw new InvalidOperationException();
        }

        public Task<IReadOnlyList<IArrayItemCommand>> GetArrayItemCommandsAsync(string arraySettingMoniker, CancellationToken cancellationToken)
        {
            if (arraySettingMoniker == MonikerConfigurationFiles)
            {
                return Task.FromResult<IReadOnlyList<IArrayItemCommand>>([_openFileArrayItemCommand]);
            }

            return Task.FromResult<IReadOnlyList<IArrayItemCommand>>(Array.Empty<IArrayItemCommand>());
        }

        private static Task<ExternalSettingOperationResult<T>> LoadConfigurationFilePathsOrThrow<T>(ISettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            ExternalSettingOperationResult<T> result;
#pragma warning disable CA1031 // Do not catch general exception types
            try
            {
                var configPathsList = settings.GetConfigFilePaths();

                var configPathsDictionary = new List<Dictionary<string, object>>(capacity: configPathsList.Count);

                // Each list item is represented by a dictionary, which in this case will have a single key-value pair for ConfigPath.
                foreach (var configPath in configPathsList)
                {
                    var dict = new Dictionary<string, object>(capacity: 1)
                    {
                        { "filePath", configPath }
                    };

                    configPathsDictionary.Add(dict);
                }

                var castedConfigPaths = (T)(object)configPathsDictionary;
                result = ExternalSettingOperationResult.SuccessResult(castedConfigPaths);
            }
            catch (Exception ex)
            {
                result = CreateSettingErrorResult<T>(ex.Message + " ('" + MonikerConfigurationFiles + "')");
            }
#pragma warning restore CA1031 // Do not catch general exception types

            return Task.FromResult(result);
        }
    }
}
