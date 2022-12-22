// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.VisualStudio;

namespace NuGet.SolutionRestoreManager
{
    [Export(typeof(IRestoreEventsPublisher))]
    [Export(typeof(IRestoreEvents))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public sealed class RestoreEventPublisher : IRestoreEventsPublisher, IRestoreEvents
    {
        private readonly Lazy<ILogger> _logger;

        public event SolutionRestoreCompletedEventHandler SolutionRestoreCompleted;

        [ImportingConstructor]
        public RestoreEventPublisher(
            [Import("VisualStudioActivityLogger")]
            Lazy<ILogger> logger)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            _logger = logger;
        }

        public void OnSolutionRestoreCompleted(SolutionRestoredEventArgs args)
        {
            Task.Run(() =>
            {
                try
                {
                    SolutionRestoreCompleted?.Invoke(args);
                }
                catch (Exception ex)
                {
                    _logger.Value.LogError(ex.ToString());
                }
            });
        }
    }
}
