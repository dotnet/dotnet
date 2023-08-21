using System.Collections.Generic;
using System.ComponentModel.Composition;
using Microsoft.Test.Apex.VisualStudio;
using NuGet.VisualStudio;

namespace NuGet
{
    [Export(typeof(TestIVsPackageSourceProvider))]
    public sealed class TestIVsPackageSourceProvider : VisualStudioTestService
    {
        public IEnumerable<KeyValuePair<string, string>> GetSources(bool includeUnOfficial, bool includeDisabled)
        {
            IVsPackageSourceProvider packageSourceProvider = VisualStudioObjectProviders.GetComponentModelService<IVsPackageSourceProvider>();

            return packageSourceProvider.GetSources(includeUnOfficial, includeDisabled);
        }
    }
}
