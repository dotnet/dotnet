using System.Collections.Generic;

namespace Microsoft.IdentityModel.Json.Linq.JsonPath
{
#nullable enable
    internal class ArrayMultipleIndexFilter : PathFilter
    {
        internal List<int> Indexes;

        public ArrayMultipleIndexFilter(List<int> indexes)
        {
            Indexes = indexes;
        }

        public override IEnumerable<JToken> ExecuteFilter(JToken root, IEnumerable<JToken> current, JsonSelectSettings? settings)
        {
            foreach (JToken t in current)
            {
                foreach (int i in Indexes)
                {
                    JToken? v = GetTokenIndex(t, settings, i);

                    if (v != null)
                    {
                        yield return v;
                    }
                }
            }
        }
    }
#nullable disable
}
