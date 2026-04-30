namespace Microsoft.ApplicationInsights.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse.PerfLib;

    internal class CategorySampleMock : CategorySample
    {
        public CategorySampleMock(IList<Tuple<string, long>> procValues) : base(null, 0, 0, null)
        {
            var counterDefinitionSample = new CounterDefinitionSample(new NativeMethods.PERF_COUNTER_DEFINITION(), -1)
                                              {
                                                  InstanceValues = procValues.Select(v => v.Item2).ToArray()
                                              };

            this.CounterTable = new Dictionary<int, CounterDefinitionSample>() { [6] = counterDefinitionSample };

            this.InstanceNameTable = new Dictionary<string, int>();

            for (int i = 0; i < procValues.Count; i++)
            {
                this.InstanceNameTable.Add(procValues[i].Item1, i);
            }
        }
    }
}