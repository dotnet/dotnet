using System;
using Newtonsoft.Json.Linq;

namespace PackageReferenceSdk
{
    public class Class1
    {
        internal JObject JObject { get; }

        internal Class1()
        {
            JObject = JObject.Parse("{}");
        }
    }
}
