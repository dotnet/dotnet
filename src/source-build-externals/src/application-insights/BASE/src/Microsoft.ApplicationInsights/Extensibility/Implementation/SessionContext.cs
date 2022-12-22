namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
    using System.Collections.Generic;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.External;

    /// <summary>
    /// Encapsulates information about a user session.
    /// </summary>
    public sealed class SessionContext
    {
        private string id;
        private bool? isFirst;

        internal SessionContext()
        {
        }

        /// <summary>
        /// Gets or sets the application-defined session ID.
        /// </summary>
        public string Id
        {
            get { return string.IsNullOrEmpty(this.id) ? null : this.id; }
            set { this.id = value; }
        }

        /// <summary>
        /// Gets or sets the IsFirst Session for the user.
        /// </summary>
        public bool? IsFirst
        {
            get { return this.isFirst; }
            set { this.isFirst = value; }
        }

        internal void UpdateTags(IDictionary<string, string> tags)
        {
            tags.UpdateTagValue(ContextTagKeys.Keys.SessionId, this.Id);
            tags.UpdateTagValue(ContextTagKeys.Keys.SessionIsFirst, this.IsFirst);
        }
        
        internal void CopyTo(SessionContext target)
        {
            Tags.CopyTagValue(this.Id, ref target.id);
            Tags.CopyTagValue(this.IsFirst, ref target.isFirst);
        }
    }
}
