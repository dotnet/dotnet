// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
//using Microsoft.VisualStudio.Text.Editor.Commanding.Commands;
using Microsoft.VisualStudio.Utilities.UnifiedSettings;
using NuGet.Configuration;
using NuGet.VisualStudio;

namespace NuGet.PackageManagement.VisualStudio.Services
{
    [Guid("6C09BBE2-4537-48B4-87D8-01BF5EB75901")]
    public sealed class ExternalSettingsProviderService : IExternalSettingsProvider
    {
        private const string MonikerAllowRestoreDownload = "packageRestore.allowRestoreDownload";
        private const string MonikerPackageRestoreAutomatic = "packageRestore.packageRestoreAutomatic";
        private const string MonikerSkipBindingRedirects = "bindingRedirects.skipBindingRedirects";
        private const string MonikerDefaultPackageManagementFormat = "packageManagement.defaultPackageManagementFormat";
        private const string MonikerPackageReference = "package-reference";
        private const string MonikerPackagesConfig = "packages-config";
        private const string MonikerShowPackageManagementChooser = "packageManagement.showPackageManagementChooser";

        private readonly ISettings? _settings;
        private readonly VSSettings? _vsSettings;

        private PackageRestoreConsent? _packageRestoreConsent;
        private BindingRedirectBehavior? _bindingRedirectBehavior;
        private PackageManagementFormat? _packageManagementFormat;

        public ExternalSettingsProviderService()
        {
            var componentModel = NuGetUIThreadHelper.JoinableTaskFactory.Run(ServiceLocator.GetComponentModelAsync);
            _settings = componentModel.GetService<ISettings>();
            _vsSettings = _settings as VSSettings;
            if (_vsSettings != null)
            {
                _vsSettings.SettingsChanged += VsSettings_SettingsChanged;
            }
            Debug.Assert(_settings != null);
        }

        private void VsSettings_SettingsChanged(object sender, EventArgs e)
        {
            _packageRestoreConsent = null;
            _bindingRedirectBehavior = null;
            _packageManagementFormat = null;
            SettingValuesChanged?.Invoke(this, ExternalSettingsChangedEventArgs.SomeOrAll);
        }

        private BindingRedirectBehavior BindingRedirectBehavior
        {
            get
            {
                if (_bindingRedirectBehavior is null)
                {
                    _bindingRedirectBehavior = new BindingRedirectBehavior(_settings);
                }

                return _bindingRedirectBehavior;
            }
        }

        private PackageRestoreConsent PackageRestoreConsent
        {
            get
            {
                if (_packageRestoreConsent is null)
                {
                    _packageRestoreConsent = new PackageManagement.PackageRestoreConsent(_settings);
                }

                return _packageRestoreConsent;
            }
        }

        private PackageManagementFormat PackageManagementFormat
        {
            get
            {
                if (_packageManagementFormat is null)
                {
                    _packageManagementFormat = new PackageManagementFormat(_settings);
                }

                return _packageManagementFormat;
            }
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
            return Task.FromResult(ExternalSettingOperationResult.SuccessResult((IReadOnlyList<EnumChoice>)new List<EnumChoice>().AsReadOnly()));
        }

        public Task<string> GetMessageTextAsync(string messageId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<ExternalSettingOperationResult<T>> GetValueAsync<T>(string moniker, CancellationToken cancellationToken) where T : notnull
        {
            switch (moniker)
            {
                case MonikerAllowRestoreDownload: return ConvertValueOrThrow<T>(PackageRestoreConsent.IsGrantedInSettings);
                case MonikerPackageRestoreAutomatic: return ConvertValueOrThrow<T>(PackageRestoreConsent.IsAutomatic);
                case MonikerSkipBindingRedirects: return ConvertValueOrThrow<T>(BindingRedirectBehavior.IsSkipped);
                case MonikerDefaultPackageManagementFormat: return ConvertDefaultPackageManagementFormatKeyOrThrow<T>(() => PackageManagementFormat.SelectedPackageManagementFormat);
                case MonikerShowPackageManagementChooser: return ConvertValueOrThrow<T>(PackageManagementFormat.Enabled);
                default: break;
            }

            // Shouldn't happen as these are monikers we declared in registration.json.
            throw new InvalidOperationException();
        }

        public Task OpenBackingStoreAsync(CancellationToken cancellationToken)
        {
            var optionsPageActivator = ServiceLocator.GetComponentModelService<IOptionsPageActivator>();
            optionsPageActivator.ActivatePage(OptionsPage.ConfigurationFiles, closeCallback: null);
            return Task.CompletedTask;
        }

        public Task<ExternalSettingOperationResult> SetValueAsync<T>(string moniker, T value, CancellationToken cancellationToken) where T : notnull
        {
            switch (moniker)
            {
                case MonikerAllowRestoreDownload:
                    {
                        if (value is bool boolValue)
                        {
                            PackageRestoreConsent.IsGrantedInSettings = boolValue;
                            return Task.FromResult((ExternalSettingOperationResult)ExternalSettingOperationResult.Success.Instance);
                        }
                        break;
                    }
                case MonikerPackageRestoreAutomatic:
                    {
                        if (value is bool boolValue)
                        {
                            PackageRestoreConsent.IsAutomatic = boolValue;
                            return Task.FromResult((ExternalSettingOperationResult)ExternalSettingOperationResult.Success.Instance);
                        }
                        break;
                    }
                case MonikerSkipBindingRedirects:
                    {
                        if (value is bool boolValue)
                        {
                            // Note that BindingRedirectBehavior defaults to `false` for any parsing errors.
                            BindingRedirectBehavior.IsSkipped = boolValue;
                            return Task.FromResult((ExternalSettingOperationResult)ExternalSettingOperationResult.Success.Instance);
                        }
                        break;
                    }
                case MonikerDefaultPackageManagementFormat:
                    {
                        if (value is string strValue)
                        {
                            PackageManagementFormat.SelectedPackageManagementFormat = strValue switch
                            {
                                MonikerPackagesConfig => 0,
                                MonikerPackageReference => 1,
                                // Shouldn't happen as these are monikers we declared in registration.json.
                                _ => throw new ArgumentOutOfRangeException(),
                            };

                            PackageManagementFormat.ApplyChanges();

                            return Task.FromResult((ExternalSettingOperationResult)ExternalSettingOperationResult.Success.Instance);
                        }
                        break;
                    }
                case MonikerShowPackageManagementChooser:
                    {
                        if (value is bool boolValue)
                        {
                            PackageManagementFormat.Enabled = boolValue;
                            PackageManagementFormat.ApplyChanges();
                            return Task.FromResult((ExternalSettingOperationResult)ExternalSettingOperationResult.Success.Instance);
                        }
                        break;
                    }
                default: break;
            }

            // Shouldn't happen as these are monikers we declared in registration.json.
            throw new InvalidOperationException();
        }

        private static Task<ExternalSettingOperationResult<T>> ConvertValueOrThrow<T>(object input) where T : notnull
        {
            if (input is T value)
            {
                return Task.FromResult(ExternalSettingOperationResult.SuccessResult(value));
            }
            else
            {
                // Shouldn't happen as these are types we declared in registration.json.
                throw new IncompatibleSettingTypeException(input.GetType().Name, typeof(T).Name);
            }
        }

        private static Task<ExternalSettingOperationResult<T>> ConvertDefaultPackageManagementFormatKeyOrThrow<T>(Func<int> input)
        {
            ExternalSettingOperationResult<T> result;
#pragma warning disable CA1031 // Do not catch general exception types
            try
            {
                int inputValue = input();
                T strValue = inputValue switch
                {
                    0 => (T)(object)MonikerPackagesConfig,
                    1 => (T)(object)MonikerPackageReference,
                    _ => throw new ArgumentOutOfRangeException(
                        paramName: nameof(input),
                        actualValue: inputValue,
                        message: nameof(MonikerDefaultPackageManagementFormat))
                };

                result = ExternalSettingOperationResult.SuccessResult(strValue);
            }
            catch (Exception ex)
            {
                result = CreateSettingErrorResult<T>(ex.Message + " ('" + MonikerDefaultPackageManagementFormat + "')");
            }
#pragma warning restore CA1031 // Do not catch general exception types

            return Task.FromResult(result);
        }

        private static ExternalSettingOperationResult<T> CreateSettingErrorResult<T>(string errorMessage)
        {
            var failure = new ExternalSettingOperationResult<T>.Failure(
                errorMessage,
                scope: ExternalSettingsErrorScope.SingleSettingOnly,
                isTransient: true);

            return failure;
        }
    }
}
