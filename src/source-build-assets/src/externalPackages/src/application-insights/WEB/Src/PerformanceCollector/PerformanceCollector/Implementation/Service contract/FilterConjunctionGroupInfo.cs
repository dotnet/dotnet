namespace Microsoft.ApplicationInsights.Extensibility.Filtering
{
    using System.Linq;
    using System.Runtime.Serialization;

    /// <summary>
    /// An AND-connected group of FilterInfo objects.
    /// </summary>
    [DataContract]
    internal class FilterConjunctionGroupInfo
    {
        [DataMember]
        public FilterInfo[] Filters { get; set; }

        public override string ToString()
        {
            if (this.Filters == null)
            {
                return string.Empty;
            }
            else
            {
                return string.Join(", ", this.Filters.Select(filter => filter.ToString()));
            }
        }
    }
}