//-----------------------------------------------------------------------
// <copyright file="EventSourceNamingMatchRuleBase.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.ApplicationInsights.EventSourceListener
{
    /// <summary>
    /// Abstraction of a match rule for event source. To be inherited by either an enabling rule or a disabling rule, etc.
    /// </summary>
    public abstract class EventSourceListeningRequestBase
    {
        /// <summary>
        /// Gets or sets the name of the EventSource to listen to.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the value of the <see cref="Name"/> property should match the name of an EventSource exactly, or should the value be treated as EventSource name prefix.
        /// </summary>
        public bool PrefixMatch { get; set; }

        /// <summary>
        /// Tests for equality.
        /// </summary>
        /// <param name="obj">Object to compare with.</param>
        /// <returns>True if the supplied object is equal to "this", otherwise false.</returns>
        public override bool Equals(object obj)
        {
            var other = obj as EventSourceListeningRequestBase;
            if (other == null)
            {
                return false;
            }

            return string.Equals(this.Name, other.Name, System.StringComparison.Ordinal) && this.PrefixMatch == other.PrefixMatch;
        }

        /// <summary>
        /// Gets the hash code for the event source name matching rule.
        /// </summary>
        /// <returns>Hash code.</returns>
        public override int GetHashCode()
        {
            return this.Name.GetHashCode() ^ this.PrefixMatch.GetHashCode();
        }
    }
}
