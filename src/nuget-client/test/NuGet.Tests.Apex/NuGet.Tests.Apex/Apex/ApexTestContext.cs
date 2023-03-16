// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using Microsoft.Build.Utilities;
using Microsoft.Test.Apex.VisualStudio;
using Microsoft.Test.Apex.VisualStudio.Solution;
using NuGet.Common;
using NuGet.Test.Utility;

namespace NuGet.Tests.Apex
{
    internal class ApexTestContext : IDisposable
    {

        private VisualStudioHost _visualStudio;
        private readonly ILogger _logger;
        private SimpleTestPathContext _pathContext;

        public SolutionService SolutionService { get; }
        public ProjectTestExtension Project { get; }
        public string PackageSource => _pathContext.PackageSource;

        public NuGetApexTestService NuGetApexTestService { get; }

        public ApexTestContext(VisualStudioHost visualStudio, ProjectTemplate projectTemplate, ILogger logger, bool noAutoRestore = false, bool addNetStandardFeeds = false, SimpleTestPathContext simpleTestPathContext = null)
        {
            logger.LogInformation("Creating test context");
            _pathContext = simpleTestPathContext ?? new SimpleTestPathContext();

            if (noAutoRestore)
            {
                _pathContext.Settings.DisableAutoRestore();
            }

            if (addNetStandardFeeds)
            {
                _pathContext.Settings.AddNetStandardFeeds();
            }

            _visualStudio = visualStudio;
            _logger = logger;
            SolutionService = _visualStudio.Get<SolutionService>();
            NuGetApexTestService = _visualStudio.Get<NuGetApexTestService>();

            VisualStudioHostExtension.ClearWindows(_visualStudio);

            Project = CommonUtility.CreateAndInitProject(projectTemplate, _pathContext, SolutionService, logger);

            NuGetApexTestService.WaitForAutoRestore();
        }

        public void Dispose()
        {
            _logger.LogInformation("Test complete, closing solution.");
            for (int attempt = 1; attempt <= 3; attempt++)
            {
                try
                {
                    SolutionService.SaveAndClose();
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogInformation($"Failed to close VS on dispose. Attempt #{attempt}");
                    Thread.Sleep(TimeSpan.FromSeconds(3));
                    ExceptionUtilities.LogException(ex, _logger);
                }
            }
            _pathContext.Dispose();
        }
    }
}
