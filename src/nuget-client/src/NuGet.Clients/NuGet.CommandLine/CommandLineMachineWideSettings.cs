// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using NuGet.Common;

namespace NuGet.CommandLine
{
    [Export(typeof(Configuration.IMachineWideSettings))]
    public class CommandLineMachineWideSettings : Configuration.IMachineWideSettings
    {
        Lazy<Configuration.ISettings> _settings;

        public CommandLineMachineWideSettings()
        {
            var baseDirectory = NuGetEnvironment.GetFolderPath(NuGetFolderPath.MachineWideConfigDirectory);
            _settings = new Lazy<Configuration.ISettings>(
                () => Configuration.Settings.LoadMachineWideSettings(baseDirectory));
        }

        public Configuration.ISettings Settings => _settings.Value;
    }
}
