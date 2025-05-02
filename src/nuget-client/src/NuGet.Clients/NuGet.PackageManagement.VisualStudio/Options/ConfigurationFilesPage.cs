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
    public class ConfigurationFilesPage : IExternalSettingsProvider, IExternalArrayItemCommandsProvider
    {
        private const string MonikerConfigurationFiles = "configurationFiles";

        private readonly VSSettings _vsSettings;
        private readonly OpenFileArrayItemCommand _openFileArrayItemCommand;

        public ConfigurationFilesPage(VSSettings vsSettings)
        {
            if (vsSettings is null)
            {
                throw new ArgumentNullException(paramName: nameof(vsSettings));
            }

            _vsSettings = vsSettings;
            _vsSettings.SettingsChanged += VsSettings_SettingsChanged;

            var documentOpener = new VSDocumentOpener();
            _openFileArrayItemCommand = new OpenFileArrayItemCommand(documentOpener);
        }

        private void VsSettings_SettingsChanged(object sender, EventArgs e)
        {
            SettingValuesChanged?.Invoke(this, ExternalSettingsChangedEventArgs.SomeOrAll);
        }

        public event EventHandler<ExternalSettingsChangedEventArgs>? SettingValuesChanged;

        // Event is unused at this time, so an empty add and remove accessor block is used to avoid a CS0067 analyzer warning.
        public event EventHandler<EnumSettingChoicesChangedEventArgs> EnumSettingChoicesChanged { add { } remove { } }
        // Event is unused at this time, so an empty add and remove accessor block is used to avoid a CS0067 analyzer warning.
        public event EventHandler<DynamicMessageTextChangedEventArgs> DynamicMessageTextChanged { add { } remove { } }
        // Event is unused at this time, so an empty add and remove accessor block is used to avoid a CS0067 analyzer warning.
        public event EventHandler ErrorConditionResolved { add { } remove { } }

        public void Dispose()
        {
            if (_vsSettings != null)
            {
                _vsSettings.SettingsChanged -= VsSettings_SettingsChanged;
            }
        }

        public Task<ExternalSettingOperationResult<IReadOnlyList<EnumChoice>>> GetEnumChoicesAsync(string enumSettingMoniker, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetMessageTextAsync(string messageId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<ExternalSettingOperationResult<T>> GetValueAsync<T>(string moniker, CancellationToken cancellationToken) where T : notnull
        {
            switch (moniker)
            {
                case MonikerConfigurationFiles: return LoadConfigurationFilePathsOrThrow<T>(_vsSettings);
                default: break;
            }

            // Shouldn't happen as these are monikers we declared in registration.json.
            throw new InvalidOperationException();
        }

        public Task OpenBackingStoreAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<ExternalSettingOperationResult> SetValueAsync<T>(string moniker, T value, CancellationToken cancellationToken) where T : notnull
        {
            throw new NotImplementedException();
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
                result = ExternalSettingsUtility.CreateSettingErrorResult<T>(ex.Message + " ('" + MonikerConfigurationFiles + "')");
            }
#pragma warning restore CA1031 // Do not catch general exception types

            return Task.FromResult(result);
        }

        public Task<IReadOnlyList<IArrayItemCommand>> GetArrayItemCommandsAsync(string arraySettingMoniker, CancellationToken cancellationToken)
        {
            if (arraySettingMoniker == MonikerConfigurationFiles)
            {
                return Task.FromResult<IReadOnlyList<IArrayItemCommand>>([_openFileArrayItemCommand]);
            }

            return Task.FromResult<IReadOnlyList<IArrayItemCommand>>(Array.Empty<IArrayItemCommand>());
        }

    }
}
