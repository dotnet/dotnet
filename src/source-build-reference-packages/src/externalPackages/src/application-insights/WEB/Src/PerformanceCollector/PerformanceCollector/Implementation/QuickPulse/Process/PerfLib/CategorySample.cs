namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse.PerfLib
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Runtime.InteropServices;
    using Microsoft.ApplicationInsights.Common;

    /// <summary>
    /// Represents performance data for a performance object (category).
    /// </summary>
    internal class CategorySample
    {
        public Dictionary<int, CounterDefinitionSample> CounterTable;

        public Dictionary<string, int> InstanceNameTable;

        private readonly PerfLib library;

        /// <summary>Initializes a new instance of the <see cref="CategorySample"/> class. Instantiates a <see cref="CategorySample"/> class.</summary>
        /// <param name="data">Performance data.</param>
        /// <param name="categoryNameIndex">Category name index.</param>
        /// <param name="counterNameIndex">Counter name index.</param>
        /// <param name="library">Performance library.</param>
        public CategorySample(byte[] data, int categoryNameIndex, int counterNameIndex, PerfLib library)
        {
            if (library == null)
            {
                return;
            }

            this.library = library;

            NativeMethods.PERF_DATA_BLOCK dataBlock = new NativeMethods.PERF_DATA_BLOCK();

            GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);

            try
            {
                IntPtr dataRef = handle.AddrOfPinnedObject();

                Marshal.PtrToStructure(dataRef, dataBlock);

                dataRef = (IntPtr)((long)dataRef + dataBlock.HeaderLength);

                int numPerfObjects = dataBlock.NumObjectTypes;
                if (numPerfObjects == 0)
                {
                    this.CounterTable = new Dictionary<int, CounterDefinitionSample>();
                    this.InstanceNameTable = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
                    return;
                }

                // Need to find the right category, GetPerformanceData might return several of them.
                NativeMethods.PERF_OBJECT_TYPE perfObject = new NativeMethods.PERF_OBJECT_TYPE();
                bool foundCategory = false;
                for (int index = 0; index < numPerfObjects; index++)
                {
                    Marshal.PtrToStructure(dataRef, perfObject);

                    if (perfObject.ObjectNameTitleIndex == categoryNameIndex)
                    {
                        foundCategory = true;
                        break;
                    }

                    dataRef = (IntPtr)((long)dataRef + perfObject.TotalByteLength);
                }

                if (!foundCategory)
                {
                    throw new InvalidOperationException("Category not found for categoryNameIndex " + categoryNameIndex.ToString(CultureInfo.InvariantCulture));
                }

                int counterNumber = perfObject.NumCounters;
                int instanceNumber = perfObject.NumInstances;

                bool isMultiInstance = instanceNumber != -1;

                // Move pointer forward to end of PERF_OBJECT_TYPE
                dataRef = (IntPtr)((long)dataRef + perfObject.HeaderLength);

                CounterDefinitionSample[] samples = new CounterDefinitionSample[counterNumber];
                CounterDefinitionSample sample = null;
                this.CounterTable = new Dictionary<int, CounterDefinitionSample>(counterNumber);
                for (int index = 0; index < samples.Length; ++index)
                {
                    NativeMethods.PERF_COUNTER_DEFINITION perfCounter = new NativeMethods.PERF_COUNTER_DEFINITION();
                    Marshal.PtrToStructure(dataRef, perfCounter);

                    samples[index] = new CounterDefinitionSample(perfCounter, instanceNumber);
                    if (perfCounter.CounterNameTitleIndex == counterNameIndex)
                    {
                        sample = samples[index];
                    }

                    dataRef = (IntPtr)((long)dataRef + perfCounter.ByteLength);

                    int currentSampleType = samples[index].CounterType;
                    if (!CategorySample.IsBaseCounter(currentSampleType))
                    {
                        // We'll put only non-base counters in the table. 
                        if (currentSampleType != NativeMethods.PERF_COUNTER_NODATA)
                        {
                            this.CounterTable[samples[index].NameIndex] = samples[index];
                        }
                    }
                    else
                    {
                        // ignore base counters
                    }
                }

                if (sample == null)
                {
                    throw new InvalidOperationException("Could not find the counter " + counterNameIndex.ToString(CultureInfo.InvariantCulture));
                }

                // now set up the InstanceNameTable.  
                if (!isMultiInstance)
                {
                    throw new InvalidOperationException("Single instance categories are not supported");
                }

                string[] parentInstanceNames = null;
                this.InstanceNameTable = new Dictionary<string, int>(instanceNumber, StringComparer.OrdinalIgnoreCase);
                for (int i = 0; i < instanceNumber; i++)
                {
                    NativeMethods.PERF_INSTANCE_DEFINITION perfInstance = new NativeMethods.PERF_INSTANCE_DEFINITION();
                    Marshal.PtrToStructure(dataRef, perfInstance);

                    if (perfInstance.ParentObjectTitleIndex > 0 && parentInstanceNames == null)
                    {
                        parentInstanceNames = this.GetInstanceNamesFromIndex(perfInstance.ParentObjectTitleIndex);
                    }

                    string instanceName;
                    if (parentInstanceNames != null && perfInstance.ParentObjectInstance >= 0
                        && perfInstance.ParentObjectInstance < parentInstanceNames.Length - 1)
                    {
                        instanceName = parentInstanceNames[perfInstance.ParentObjectInstance] + "/"
                                       + Marshal.PtrToStringUni((IntPtr)((long)dataRef + perfInstance.NameOffset));
                    }
                    else
                    {
                        instanceName = Marshal.PtrToStringUni((IntPtr)((long)dataRef + perfInstance.NameOffset));
                    }

                    // in some cases instance names are not unique (Process), same as perfmon, so generate a unique name
                    string newInstanceName = instanceName;
                    int newInstanceNumber = 1;
                    while (true)
                    {
                        if (!this.InstanceNameTable.ContainsKey(newInstanceName))
                        {
                            this.InstanceNameTable[newInstanceName] = i;
                            break;
                        }
                        else
                        {
                            newInstanceName = instanceName + "#" + newInstanceNumber.ToString(CultureInfo.InvariantCulture);
                            ++newInstanceNumber;
                        }
                    }

                    dataRef = (IntPtr)((long)dataRef + perfInstance.ByteLength);

                    // we only need one counter right now, to get more - use the following pattern:
                    ////foreach (CounterDefinitionSample s in samples)
                    ////{
                    ////    s.SetInstanceValue(i, dataRef);
                    ////}

                    sample.SetInstanceValue(i, dataRef);

                    dataRef = (IntPtr)((long)dataRef + Marshal.ReadInt32(dataRef));
                }
            }
            finally
            {
                handle.Free();
            }
        }

        private static bool IsBaseCounter(int type)
        {
            return type == NativeMethods.PERF_AVERAGE_BASE || type == NativeMethods.PERF_COUNTER_MULTI_BASE || type == NativeMethods.PERF_RAW_BASE
                   || type == NativeMethods.PERF_LARGE_RAW_BASE || type == NativeMethods.PERF_SAMPLE_BASE;
        }

        private string[] GetInstanceNamesFromIndex(int categoryIndex)
        {
            byte[] data = this.library.GetPerformanceData(categoryIndex.ToString(CultureInfo.InvariantCulture));

            GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);

            try
            {
                IntPtr dataRef = handle.AddrOfPinnedObject();

                NativeMethods.PERF_DATA_BLOCK dataBlock = new NativeMethods.PERF_DATA_BLOCK();
                Marshal.PtrToStructure(dataRef, dataBlock);

                dataRef = (IntPtr)((long)dataRef + dataBlock.HeaderLength);

                int numPerfObjects = dataBlock.NumObjectTypes;

                NativeMethods.PERF_OBJECT_TYPE perfObject = null;
                bool foundCategory = false;
                for (int index = 0; index < numPerfObjects; index++)
                {
                    perfObject = new NativeMethods.PERF_OBJECT_TYPE();
                    Marshal.PtrToStructure(dataRef, perfObject);

                    if (perfObject.ObjectNameTitleIndex == categoryIndex)
                    {
                        foundCategory = true;
                        break;
                    }

                    dataRef = (IntPtr)((long)dataRef + perfObject.TotalByteLength);
                }

                if (!foundCategory)
                {
                    return ArrayExtensions.Empty<string>();
                }

                int counterNumber = perfObject.NumCounters;
                int instanceNumber = perfObject.NumInstances;

                dataRef = (IntPtr)((long)dataRef + perfObject.HeaderLength);

                if (instanceNumber == -1)
                {
                    return ArrayExtensions.Empty<string>();
                }

                CounterDefinitionSample[] samples = new CounterDefinitionSample[counterNumber];
                for (int index = 0; index < samples.Length; ++index)
                {
                    NativeMethods.PERF_COUNTER_DEFINITION perfCounter = new NativeMethods.PERF_COUNTER_DEFINITION();
                    Marshal.PtrToStructure(dataRef, perfCounter);
                    dataRef = (IntPtr)((long)dataRef + perfCounter.ByteLength);
                }

                string[] instanceNames = new string[instanceNumber];
                for (int i = 0; i < instanceNumber; i++)
                {
                    NativeMethods.PERF_INSTANCE_DEFINITION perfInstance = new NativeMethods.PERF_INSTANCE_DEFINITION();
                    Marshal.PtrToStructure(dataRef, perfInstance);

                    instanceNames[i] = Marshal.PtrToStringUni((IntPtr)((long)dataRef + perfInstance.NameOffset));

                    dataRef = (IntPtr)((long)dataRef + perfInstance.ByteLength);
                    dataRef = (IntPtr)((long)dataRef + Marshal.ReadInt32(dataRef));
                }

                return instanceNames;
            }
            finally
            {
                handle.Free();
            }
        }
    }
}