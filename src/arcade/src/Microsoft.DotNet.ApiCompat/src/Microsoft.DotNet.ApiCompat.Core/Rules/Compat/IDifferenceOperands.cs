using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Cci.Differs
{
    /// <summary>
    /// Names for the left and right operands of a difference
    /// </summary>
    public interface IDifferenceOperands
    {
        /// <summary>
        /// Name of left operand of a difference operation.  Typically called a contract or reference.
        /// </summary>
        string Contract { get; }
        /// <summary>
        /// Name of right operand of a difference operation.  Typically called an implementation.
        /// </summary>
        string Implementation { get; }
    }

    public class DifferenceOperands : IDifferenceOperands
    {
        public string Contract { get; set; }

        public string Implementation { get; set; }
    }
}
