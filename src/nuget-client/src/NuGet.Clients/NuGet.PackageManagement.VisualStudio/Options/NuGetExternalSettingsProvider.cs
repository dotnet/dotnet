// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Utilities.UnifiedSettings;
using NuGet.VisualStudio;

namespace NuGet.PackageManagement.VisualStudio.Options
{
    public abstract class NuGetExternalSettingsProvider : IExternalSettingsProvider
    {
        protected readonly VSSettings _vsSettings;
        protected bool _suppressSettingValuesChanged;

        protected NuGetExternalSettingsProvider(VSSettings vsSettings)
        {
            _vsSettings = vsSettings ?? throw new ArgumentNullException(paramName: nameof(vsSettings));
            _vsSettings.SettingsChanged += VsSettings_SettingsChanged;
        }

        public event EventHandler<ExternalSettingsChangedEventArgs>? SettingValuesChanged;

        // Events here are typically unused, so an empty add and remove accessor block is used to avoid a CS0067 analyzer warning.
        public virtual event EventHandler<EnumSettingChoicesChangedEventArgs> EnumSettingChoicesChanged { add { } remove { } }
        public virtual event EventHandler<DynamicMessageTextChangedEventArgs> DynamicMessageTextChanged { add { } remove { } }
        public virtual event EventHandler ErrorConditionResolved { add { } remove { } }

        public abstract Task<ExternalSettingOperationResult<T>> GetValueAsync<T>(string moniker, CancellationToken cancellationToken) where T : notnull;
        public abstract Task<ExternalSettingOperationResult> SetValueAsync<T>(string moniker, T value, CancellationToken cancellationToken) where T : notnull;

        public virtual Task<ExternalSettingOperationResult<IReadOnlyList<EnumChoice>>> GetEnumChoicesAsync(string enumSettingMoniker, CancellationToken cancellationToken)
        {
            return Task.FromResult(ExternalSettingOperationResult.SuccessResult((IReadOnlyList<EnumChoice>)new List<EnumChoice>().AsReadOnly()));
        }

        internal virtual void VsSettings_SettingsChanged(object sender, EventArgs e)
        {
            if (_suppressSettingValuesChanged && !IsSuppressionDisabledByEnvironment())
            {
                return; // Suppress the event invocation
            }

            SettingValuesChanged?.Invoke(this, ExternalSettingsChangedEventArgs.SomeOrAll);
        }

        /// <summary>
        /// For DEBUG purposes only. Expected to be removed by the next release.
        /// </summary>
        private static bool IsSuppressionDisabledByEnvironment()
        {
            try
            {
                const string envVarName = "NUGET_DISABLE_SUPPRESS_SETTING_VALUES_CHANGED";
                return string.Equals(
                    Common.EnvironmentVariableWrapper.Instance.GetEnvironmentVariable(envVarName),
                    "true",
                    StringComparison.OrdinalIgnoreCase);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                // Ignore errors since this is not a production feature.
                return false;
            }
        }

        public virtual Task<string> GetMessageTextAsync(string messageId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task OpenBackingStoreAsync(CancellationToken cancellationToken)
        {
            var optionsPageActivator = ServiceLocator.GetComponentModelService<IOptionsPageActivator>();
            optionsPageActivator.ActivatePage(OptionsPage.ConfigurationFiles, closeCallback: null);
            return Task.CompletedTask;
        }

        public static ExternalSettingOperationResult<T> CreateSettingErrorResult<T>(string errorMessage)
        {
            var failure = new ExternalSettingOperationResult<T>.Failure(
                errorMessage,
                scope: ExternalSettingsErrorScope.SingleSettingOnly,
                isTransient: true);

            return failure;
        }

        public static ExternalSettingOperationResult CreateSettingErrorResult(string errorMessage, bool isTransient)
        {
            var failure = new ExternalSettingOperationResult.Failure(
                errorMessage,
                scope: ExternalSettingsErrorScope.SingleSettingOnly,
                isTransient);

            return failure;
        }

        public static Task<ExternalSettingOperationResult<T>> ConvertValueOrThrow<T>(object input) where T : notnull
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
    }
}
