// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using System.Globalization;
using Microsoft.VisualStudio.Experimentation;
using NuGet.Common;
using NuGet.VisualStudio.Telemetry;

namespace NuGet.VisualStudio
{
    [Export(typeof(INuGetExperimentationService))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NuGetExperimentationService : INuGetExperimentationService
    {
        private readonly IEnvironmentVariableReader _environmentVariableReader;
        private readonly IExperimentationService _experimentationService;
        private readonly Lazy<IOutputConsoleProvider> _outputConsoleProvider;

        [ImportingConstructor]
        public NuGetExperimentationService(Lazy<IOutputConsoleProvider> _outputConsoleProvider)
            : this(EnvironmentVariableWrapper.Instance, ExperimentationService.Default, _outputConsoleProvider)
        {
            // ensure uniqueness.
        }

        internal NuGetExperimentationService(IEnvironmentVariableReader environmentVariableReader, IExperimentationService experimentationService, Lazy<IOutputConsoleProvider> outputConsoleProvider)
        {
            _environmentVariableReader = environmentVariableReader ?? throw new ArgumentNullException(nameof(environmentVariableReader));
            _experimentationService = experimentationService ?? throw new ArgumentNullException(nameof(experimentationService));
            _outputConsoleProvider = outputConsoleProvider ?? throw new ArgumentNullException(nameof(outputConsoleProvider));
        }

        public bool IsExperimentEnabled(ExperimentationConstants experiment)
        {
            var isExpForcedEnabled = false;
            var isExpForcedDisabled = false;
            string flightVariable = experiment.FlightEnvironmentVariable;

            if (!string.IsNullOrEmpty(flightVariable))
            {
                string envVarOverride = _environmentVariableReader.GetEnvironmentVariable(flightVariable);

                isExpForcedDisabled = envVarOverride == "0";
                isExpForcedEnabled = envVarOverride == "1";

                if (isExpForcedDisabled || isExpForcedEnabled)
                {
                    LogEnvironmentVariableOverride(experiment.FlightFlag, flightVariable, envVarOverride);
                }
            }

            return !isExpForcedDisabled && (isExpForcedEnabled || _experimentationService.IsCachedFlightEnabled(experiment.FlightFlag));
        }

        private void LogEnvironmentVariableOverride(string flightName, string variableName, string variableValue)
        {
            NuGetUIThreadHelper.JoinableTaskFactory.RunAsync(async () =>
            {
                IOutputConsole console = await _outputConsoleProvider.Value.CreatePackageManagerConsoleAsync();
                await console.WriteLineAsync(string.Format(CultureInfo.CurrentCulture, Resources.ExperimentVariableOverrideLogText, flightName, variableName, variableValue));
            }).PostOnFailure(nameof(NuGetExperimentationService), nameof(LogEnvironmentVariableOverride));
        }
    }
}
