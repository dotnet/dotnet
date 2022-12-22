using System;
using System.Collections.Immutable;
using Newtonsoft.Json;

namespace Microsoft.DotNet.Helix.Client.Models
{
    public partial class AggregateWorkItemSummary
    {
        public AggregateWorkItemSummary(IImmutableList<Models.WorkItemAggregateSummary> workItems, IImmutableList<Models.AggregateAnalysisSummaryKeyedData> analyses)
        {
            WorkItems = workItems;
            Analyses = analyses;
        }

        [JsonProperty("WorkItems")]
        public IImmutableList<Models.WorkItemAggregateSummary> WorkItems { get; set; }

        [JsonProperty("Analyses")]
        public IImmutableList<Models.AggregateAnalysisSummaryKeyedData> Analyses { get; set; }

        [JsonIgnore]
        public bool IsValid
        {
            get
            {
                if (WorkItems == default(IImmutableList<Models.WorkItemAggregateSummary>))
                {
                    return false;
                }
                if (Analyses == default(IImmutableList<Models.AggregateAnalysisSummaryKeyedData>))
                {
                    return false;
                }
                return true;
            }
        }
    }
}
