using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Cci.Differs
{
    /// <summary>
    /// Rule Settings
    /// </summary>
    public interface IRuleSettings
    {
        /// <summary>
        /// Gets a value indicating whether DIM rules should be applied.
        /// </summary>
        bool AllowDefaultInterfaceMethods { get; }
    }

    public class RuleSettings : IRuleSettings
    {
        public bool AllowDefaultInterfaceMethods { get; set; }
    }
}
