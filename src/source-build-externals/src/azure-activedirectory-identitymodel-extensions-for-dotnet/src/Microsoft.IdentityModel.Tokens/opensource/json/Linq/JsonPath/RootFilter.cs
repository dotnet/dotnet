using System.Collections.Generic;

namespace Microsoft.IdentityModel.Json.Linq.JsonPath
{
#nullable enable
    internal class RootFilter : PathFilter
    {
        public static readonly RootFilter Instance = new RootFilter();

        private RootFilter()
        {
        }

        public override IEnumerable<JToken> ExecuteFilter(JToken root, IEnumerable<JToken> current, JsonSelectSettings? settings)
        {
            return new[] { root };
        }
    }
#nullable disable
}
