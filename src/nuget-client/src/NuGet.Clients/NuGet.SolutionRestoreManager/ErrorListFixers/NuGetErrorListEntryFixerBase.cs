// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.Internal.VisualStudio.Shell;
using Microsoft.VisualStudio.Imaging;
using Microsoft.VisualStudio.Imaging.Interop;
using Microsoft.VisualStudio.Shell.TableControl;

namespace NuGet.SolutionRestoreManager.ErrorListFixers
{
#pragma warning disable CS0618 // Obsolete in VS because it "may change without warning". It remains the only Error List fixer extensibility point today.
    /// <summary>
    /// Base class for NuGet Error List entry fixers that surface the sparkle action.
    /// Provides the shared icon, entry-code matching, and fix dispatch, leaving
    /// each fixer to declare the codes it handles, its tooltip, and the fix action itself.
    /// </summary>
    internal abstract class NuGetErrorListEntryFixerBase : IErrorListEntryFixer
#pragma warning restore CS0618
    {
        /// <summary>
        /// The inspector that determines whether an Error List entry is supported by this fixer.
        /// </summary>
        protected abstract IErrorListEntryInspector EntryInspector { get; }

        public ImageMoniker? Icon => KnownMonikers.SparkleNoColor;

        public abstract string Tooltip { get; }

        public bool CanFix(ITableEntryHandle entry)
        {
            return EntryInspector.IsSupportedCode(entry);
        }

        public bool TryFix(ITableEntryHandle entry)
        {
            if (!CanFix(entry))
            {
                return false;
            }

            return TryFixCore(entry);
        }

        /// <summary>
        /// Performs the fix action for an <paramref name="entry"/> that has already passed
        /// <see cref="CanFix"/>.
        /// </summary>
        /// <param name="entry">The Error List entry to fix.</param>
        /// <returns><see langword="true"/> if the fix was initiated; otherwise <see langword="false"/>.</returns>
        protected abstract bool TryFixCore(ITableEntryHandle entry);
    }
}
