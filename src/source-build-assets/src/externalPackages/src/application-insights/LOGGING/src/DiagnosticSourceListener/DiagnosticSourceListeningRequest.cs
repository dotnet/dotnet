//-----------------------------------------------------------------------
// <copyright file="DiagnosticSourceListeningRequest.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.ApplicationInsights.DiagnosticSourceListener
{
    /// <summary>
    /// Represents a request to listen to a specific DiagnosticSource.
    /// </summary>
    public class DiagnosticSourceListeningRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DiagnosticSourceListeningRequest"/> class.
        /// </summary>
        public DiagnosticSourceListeningRequest()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DiagnosticSourceListeningRequest"/> class.
        /// </summary>
        /// <param name="name">The name of the diagnostic source to listen to.</param>
        public DiagnosticSourceListeningRequest(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Gets or sets the name of the diagnostic source to listen to.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Tests for equality.
        /// </summary>
        /// <param name="obj">Object to compare with.</param>
        /// <returns>True if the supplied object is equal to "this", otherwise false.</returns>
        public override bool Equals(object obj)
        {
            var other = obj as DiagnosticSourceListeningRequest;
            if (other == null)
            {
                return false;
            }

            return string.Equals(this.Name, other.Name, System.StringComparison.Ordinal);
        }

        /// <summary>
        /// Gets the hash code for the current listening request.
        /// </summary>
        /// <returns>Hash code.</returns>
        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }
    }
}
