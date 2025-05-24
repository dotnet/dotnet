// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Utilities.UnifiedSettings;
using NuGet.VisualStudio;

namespace NuGet.PackageManagement.VisualStudio.Options
{
    [Guid("6C09BBE2-4537-48B4-87D8-01BF5EB75901")]
    public sealed class GeneralPage : NuGetExternalSettingsProvider
    {
        private const string MonikerAllowRestoreDownload = "packageRestore.allowRestoreDownload";
        private const string MonikerPackageRestoreAutomatic = "packageRestore.packageRestoreAutomatic";
        private const string MonikerSkipBindingRedirects = "bindingRedirects.skipBindingRedirects";
        private const string MonikerDefaultPackageManagementFormat = "packageManagement.defaultPackageManagementFormat";
        private const string MonikerPackageReference = "package-reference";
        private const string MonikerPackagesConfig = "packages-config";
        private const string MonikerShowPackageManagementChooser = "packageManagement.showPackageManagementChooser";

        internal PackageRestoreConsent? _packageRestoreConsent;
        internal BindingRedirectBehavior? _bindingRedirectBehavior;
        internal PackageManagementFormat? _packageManagementFormat;

        public GeneralPage(VSSettings vsSettings)
            : base(vsSettings)
        { }

        internal BindingRedirectBehavior BindingRedirectBehavior
        {
            get
            {
                if (_bindingRedirectBehavior is null)
                {
                    _bindingRedirectBehavior = new BindingRedirectBehavior(_vsSettings);
                }

                return _bindingRedirectBehavior;
            }
        }

        internal PackageRestoreConsent PackageRestoreConsent
        {
            get
            {
                if (_packageRestoreConsent is null)
                {
                    _packageRestoreConsent = new PackageRestoreConsent(_vsSettings);
                }

                return _packageRestoreConsent;
            }
        }

        internal PackageManagementFormat PackageManagementFormat
        {
            get
            {
                if (_packageManagementFormat is null)
                {
                    _packageManagementFormat = new PackageManagementFormat(_vsSettings);
                }

                return _packageManagementFormat;
            }
        }

        /// <summary>
        /// Reset any cached values for this specific page instance when the settings change.
        /// </summary>
        internal override void VsSettings_SettingsChanged(object sender, EventArgs e)
        {
            _packageRestoreConsent = null;
            _bindingRedirectBehavior = null;
            _packageManagementFormat = null;
            base.VsSettings_SettingsChanged(sender, e);
        }

        public override Task<ExternalSettingOperationResult<T>> GetValueAsync<T>(string moniker, CancellationToken cancellationToken)
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

        public override Task<ExternalSettingOperationResult> SetValueAsync<T>(string moniker, T value, CancellationToken cancellationToken)
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

        private static Task<ExternalSettingOperationResult<T>> ConvertDefaultPackageManagementFormatKeyOrThrow<T>(Func<int> input)
        {
            ExternalSettingOperationResult<T> result;
#pragma warning disable CA1031 // Do not catch general exception types
            try
            {
                var inputValue = input();
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
    }
}
