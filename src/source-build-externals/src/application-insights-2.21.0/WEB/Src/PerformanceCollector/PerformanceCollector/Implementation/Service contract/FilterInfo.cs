namespace Microsoft.ApplicationInsights.Extensibility.Filtering
{
    using System;
    using System.Globalization;
    using System.Runtime.Serialization;

    [DataContract]
    internal class FilterInfo
    {
        [DataMember]
        public string FieldName { get; set; }

        [DataMember(Name = "Predicate")]
        public string PredicateForSerialization
        {
            get
            {
                return this.Predicate.ToString();
            }

            set
            {
                Predicate predicate;
                if (!Enum.TryParse(value, out predicate))
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(value),
                        string.Format(CultureInfo.InvariantCulture, "Unsupported Predicate value: {0}", value));
                }

                this.Predicate = predicate;
            }
        }

        public Predicate Predicate { get; set; }

        [DataMember]
        public string Comparand { get; set; }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0} {1} {2}", this.FieldName, this.Predicate, this.Comparand);
        }

        public override int GetHashCode()
        {
            throw new InvalidOperationException("Hash calculation is not supported.");
        }
    }
}