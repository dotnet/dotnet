// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.ProjectManagement;

namespace NuGet.PackageManagement.UI.Utility
{
    internal class RestoreBarLogger : LoggerBase
    {
        private readonly INuGetProjectContext _nuGetProjectContext;

        /// <summary>
        /// Creates a <see cref="LoggerBase"/> with verbosity level <see cref="Common.LogLevel.Warning"/>
        /// so that only critical log messages are shown in the <see cref="PackageRestoreBar"/>.
        /// </summary>
        /// <param name="nuGetProjectContext">Underlying <see cref="ILogger"/> implementation to invoke.</param>
        public RestoreBarLogger(INuGetProjectContext nuGetProjectContext)
            : base(verbosityLevel: LogLevel.Warning)
        {
            _nuGetProjectContext = nuGetProjectContext ?? throw new ArgumentNullException(nameof(nuGetProjectContext));
        }

        public override void Log(ILogMessage message)
        {
            _nuGetProjectContext.Log(message);
        }

        public override Task LogAsync(ILogMessage message)
        {
            Log(message);
            return Task.CompletedTask;
        }
    }
}
