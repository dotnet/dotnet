// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
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
            _this.SkipIntermediateOutputPathExistCheck_TestOnly = true;
            return _this.TryGetIntermediateOutputPath(state, out result);
        }
    }
}
