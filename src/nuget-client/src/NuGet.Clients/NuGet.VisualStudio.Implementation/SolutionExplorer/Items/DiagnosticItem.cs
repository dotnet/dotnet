// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using Microsoft.VisualStudio.Imaging;
using Microsoft.VisualStudio.Imaging.Interop;
using Microsoft.VisualStudio.ProjectSystem.VS.Tree.Dependencies.AttachedCollections;
using Microsoft.VisualStudio.Shell;
using NuGet.VisualStudio.Implementation.Resources;
using NuGet.VisualStudio.SolutionExplorer.Models;
using LogLevel = NuGet.Common.LogLevel;

namespace NuGet.VisualStudio.SolutionExplorer
{
    /// <summary>
    /// Backing object for diagnostic message items within a package within the dependencies tree.
    /// </summary>
    internal sealed class DiagnosticItem : RelatableItemBase
    {
        public AssetsFileTarget Target { get; private set; }
        public AssetsFileTargetLibrary Library { get; private set; }
        public AssetsFileLogMessage Log { get; private set; }

        public DiagnosticItem(AssetsFileTarget target, AssetsFileTargetLibrary library, AssetsFileLogMessage log)
            : base(log.Message)
        {
            Target = target;
            Library = library;
            Log = log;
        }

        public override object Identity => Tuple.Create(Library.Name, Log.Message);

        public override int Priority => AttachedItemPriority.Diagnostic;

        public override ImageMoniker IconMoniker => Log.Level switch
        {
            LogLevel.Error => KnownMonikers.StatusError,
            LogLevel.Warning => KnownMonikers.StatusWarning,
            _ => KnownMonikers.StatusInformation
        };

        public bool TryUpdateState(AssetsFileTarget target, AssetsFileTargetLibrary library, in AssetsFileLogMessage log)
        {
            if (ReferenceEquals(Target, target) && ReferenceEquals(Library, library))
            {
                return false;
            }

            Target = target;
            Library = library;
            Log = log;
            Text = log.Message;
            return true;
        }

        public override object? GetBrowseObject() => new BrowseObject(this);

        private sealed class BrowseObject : LocalizableProperties
        {
            private readonly DiagnosticItem _item;

            public BrowseObject(DiagnosticItem log) => _item = log;

            public override string GetComponentName() => Code;

            public override string GetClassName() => VsResources.DiagnosticBrowseObjectClassName;

            [BrowseObjectDisplayName(nameof(VsResources.DiagnosticMessageDisplayName))]
            [BrowseObjectDescription(nameof(VsResources.DiagnosticMessageDescription))]
            public string Message => _item.Log.Message;

            [BrowseObjectDisplayName(nameof(VsResources.DiagnosticCodeDisplayName))]
            [BrowseObjectDescription(nameof(VsResources.DiagnosticCodeDescription))]
            public string Code => _item.Log.Code.ToString();

            [BrowseObjectDisplayName(nameof(VsResources.DiagnosticLibraryNameDisplayName))]
            [BrowseObjectDescription(nameof(VsResources.DiagnosticLibraryNameDescription))]
            public string LibraryName => _item.Log.LibraryName;

            [BrowseObjectDisplayName(nameof(VsResources.DiagnosticLevelDisplayName))]
            [BrowseObjectDescription(nameof(VsResources.DiagnosticLevelDescription))]
            public string Level => _item.Log.Level.ToString();

            [BrowseObjectDisplayName(nameof(VsResources.DiagnosticWarningLevelDisplayName))]
            [BrowseObjectDescription(nameof(VsResources.DiagnosticWarningLevelDescription))]
            public string WarningLevel => _item.Log.Level == LogLevel.Warning ? _item.Log.WarningLevel.ToString() : "";
        }
    }
}
