// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Microsoft.Cci.Mappings;

namespace Microsoft.Cci.Differs
{
    public static class ListMerger
    {
        public static IEnumerable<ElementMapping<T>> MergeLists<T>(IEnumerable<T> list0, IEnumerable<T> list1) where T : class
        {
            T[] array0 = list0 == null ? new T[0] : list0.ToArray();
            T[] array1 = list1 == null ? new T[0] : list1.ToArray();

            return ListMerger.Merge<T>(array0, array1).Select(t => new SimpleElementMapping<T>(t.Item1, t.Item2));
        }

        public static List<Tuple<DifferenceType, T>> Merge<T>(T[] list0, T[] list1) where T : class
        {
            return Merge<T>(list0, 0, list0.Length, list1, 0, list1.Length);
        }

        public static List<Tuple<DifferenceType, T>> Merge<T>(T[] list0, int list0Start, int list0End, T[] list1, int list1Start, int list1End) where T : class
        {
            List<Tuple<DifferenceType, T>> list = new List<Tuple<DifferenceType, T>>();

            int list1Index = list1Start;
            for (int list0Index = list0Start; list0Index < list0End;)
            {
                // No more in list1 so consume list0 items
                if (list1Index >= list1End)
                {
                    list.Add(Tuple.Create(DifferenceType.Removed, list0[list0Index]));
                    list0Index++;
                    continue;
                }

                // Items are equal consume.
                if (list0[list0Index].Equals(list1[list1Index]))
                {
                    list.Add(Tuple.Create(DifferenceType.Unchanged, list0[list0Index]));
                    list0Index++;
                    list1Index++;
                    continue;
                }

                // Find the next matching item in the list0 and consume to that point
                int findIndex0 = Array.FindIndex(list0, list0Index, list0End - list0Index, t => list1[list1Index].Equals(t));
                if (findIndex0 >= list0Index)
                {
                    while (findIndex0 > list0Index)
                    {
                        list.Add(Tuple.Create(DifferenceType.Removed, list0[list0Index]));
                        list0Index++;
                    }
                    continue;
                }

                // Find the next matching item in list1 and consume to that point
                int findIndex1 = Array.FindIndex(list1, list1Index, list1End - list1Index, t => list0[list0Index].Equals(t));
                if (findIndex1 >= list1Index)
                {
                    while (findIndex1 > list1Index)
                    {
                        list.Add(Tuple.Create(DifferenceType.Added, list1[list1Index]));
                        list1Index++;
                    }
                    continue;
                }

                // Either item is found in the other list so just consume both single items
                Contract.Assert(findIndex0 == -1 && findIndex1 == -1);
                Contract.Assert(!list0[list0Index].Equals(list1[list1Index]));
                list.Add(Tuple.Create(DifferenceType.Removed, list0[list0Index]));
                list0Index++;

                list.Add(Tuple.Create(DifferenceType.Added, list1[list1Index]));
                list1Index++;
            }

            // Consume any remaining in list1
            while (list1Index < list1End)
            {
                list.Add(Tuple.Create(DifferenceType.Added, list1[list1Index]));
                list1Index++;
            }

            return list;
        }
    }
}
