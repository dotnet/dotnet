// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Threading.Tasks;
using Microsoft.VisualStudio.ProjectSystem;
using Microsoft.VisualStudio.ProjectSystem.Properties;

namespace Microsoft.VisualStudio.Razor.ProjectSystem;

internal partial class WindowsRazorProjectHostBase
{
    internal TestAccessor GetTestAccessor() => new(this);

    internal sealed class TestAccessor(WindowsRazorProjectHostBase @this)
    {
        private readonly WindowsRazorProjectHostBase _this = @this;

        internal bool GetIntermediateOutputPathFromProjectChange(IImmutableDictionary<string, IProjectRuleSnapshot> state, out string? result)
        {
            _this._skipDirectoryExistCheck_TestOnly = true;
            return _this.TryGetIntermediateOutputPath(state, out result);
        }

        internal Task InitializeAsync()
            => _this.InitializeAsync();

        internal Task OnProjectChangedAsync(string sliceDimensions, IProjectVersionedValue<IProjectSubscriptionUpdate> update)
            => _this.OnProjectChangedAsync(sliceDimensions, update);

        internal Task OnProjectRenamingAsync(string oldProjectFilePath, string newProjectFilePath)
            => _this.OnProjectRenamingAsync(oldProjectFilePath, newProjectFilePath);
    }
}
