using System;
using System.Collections.Immutable;
using Newtonsoft.Json;

namespace Microsoft.DotNet.Helix.Client.Models
{
    public partial class JobCreationResult
    {
        public JobCreationResult(string name, string summaryUrl, string resultsUri, string resultsUriRSAS)
        {
            Name = name;
            SummaryUrl = summaryUrl;
            ResultsUri = resultsUri;
            ResultsUriRSAS = resultsUriRSAS;
        }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("SummaryUrl")]
        public string SummaryUrl { get; set; }

        [JsonProperty("ResultsUri")]
        public string ResultsUri { get; set; }

        [JsonProperty("ResultsUriRSAS")]
        public string ResultsUriRSAS { get; set; }

        [JsonProperty("CancellationToken")]
        public string CancellationToken { get; set; }

        [JsonIgnore]
        public bool IsValid
        {
            get
            {
                if (string.IsNullOrEmpty(Name))
                {
                    return false;
                }
                if (string.IsNullOrEmpty(SummaryUrl))
                {
                    return false;
                }
                if (string.IsNullOrEmpty(ResultsUri))
                {
                    return false;
                }
                if (string.IsNullOrEmpty(ResultsUriRSAS))
                {
                    return false;
                }
                return true;
            }
        }
    }
}
