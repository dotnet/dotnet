// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace NuGet.Resolver
{
    /// <summary>
    /// This class is responsible for finding the best combination of compatible items. The caller
    /// supplies a collection of groups, a sorting function (to determine priority within a group), and
    /// a function to determine whether two items are incompatible. The solution (if found) will contain
    /// exactly 1 item from each group.
    /// </summary>
    /// <remarks>Created by Aaron Marten</remarks>
    /// <typeparam name="T">The type of item to evaluate.</typeparam>
    public class CombinationSolver<T>
    {
        private readonly T[] _solution;

        /// <summary>
        /// The initial domains are the full/initial candidate sets we start with when
        /// attempting to discover a solution. They need to be stored and referred to
        /// as the algorithm executes to re-initialize the current/working domains.
        /// </summary>
        private readonly IList<IEnumerable<T>> _initialDomains;

        /// <summary>
        /// The current domains are initialized with the initial domains. As we progress
        /// through the algorithm, we may remove elements from the current domain as we
        /// discover that an item cannot be part of the solution. If we need to backtrack,
        /// we may reset the current domain to the corresponding initial domain.
        /// </summary>
        private readonly List<HashSet<T>> _currentDomains;

        private readonly List<List<T>> _currentDomainsSorted;

        /// <summary>
        /// The subset of past indexes where a conflict was found. Used to calculate the biggest and safest
        /// (i.e. not missing a better solution) jump we can make in MoveBackward.
        /// </summary>
        private readonly List<HashSet<int>> _conflictSet;

        /// <summary>
        /// For each position, maintain a stack of past indexes that forward checked (and found/removed conflicts)
        /// from the position.
        /// </summary>
        private readonly List<Stack<int>> _pastForwardChecking;

        /// <summary>
        /// For each position, maintain a stack of forward/future indexes where conflicts were found.
        /// </summary>
        private readonly List<Stack<int>> _futureForwardChecking;

        /// <summary>
        /// For each position, maintain a Stack of sets of items that were 'reduced' from the domain. This allows us
        /// to restore the items back into the domain on future iterations in case we need to back up, etc...
        /// </summary>
        private readonly List<Stack<Stack<T>>> _reductions;

        private readonly IComparer<T> _prioritySorter;
        private readonly Func<T, T, bool> _shouldRejectPair;

        private CombinationSolver(IEnumerable<IEnumerable<T>> groupedItems,
            IComparer<T> itemSorter,
            Func<T, T, bool> shouldRejectPairFunc)
        {
            _prioritySorter = itemSorter;
            _shouldRejectPair = shouldRejectPairFunc;

            var initialDomains = groupedItems.ToList();

            _initialDomains = groupedItems.ToList();

            // Initialize various arrays required for the algorithm to run.
            _currentDomains = initialDomains.Select(d => new HashSet<T>(d)).ToList();

            _currentDomainsSorted = initialDomains.Select(d => new List<T>(d)).ToList();

            foreach (var list in _currentDomainsSorted)
            {
                list.Sort(_prioritySorter);
            }

            _conflictSet = initialDomains.Select(d => new HashSet<int>()).ToList();
            _pastForwardChecking = initialDomains.Select(d => new Stack<int>()).ToList();
            _futureForwardChecking = initialDomains.Select(d => new Stack<int>()).ToList();
            _reductions = initialDomains.Select(d => new Stack<Stack<T>>()).ToList();

            _solution = new T[initialDomains.Count];
        }

        /// <summary>
        /// Entry point for the combination evalutation phase of the algorithm. The algorithm
        /// combines forward checking [FC] (i.e. trying to eliminate future possible combinations to evaluate)
        /// with Conflict-directed Back Jumping.
        /// Based off the FC-CBJ algorithm described in Prosser's Hybrid
        /// Algorithms for the Constraint Satisfaction Problem:
        /// http://archive.nyu.edu/bitstream/2451/14410/1/IS-90-10.pdf
        /// </summary>
        /// <param name="groupedItems">The candidate enlistment items grouped by product.</param>
        /// <param name="itemSorter">
        /// Function supplied by the caller to sort items in preferred/priority order. 'Higher
        /// priority' items should come *first* in the sort.
        /// </param>
        /// <param name="shouldRejectPairFunc">
        /// Function supplied by the caller to determine whether two items are
        /// compatible or not.
        /// </param>
        /// <param name="diagnosticOutput">
        /// Used to provide partial solutions to be used for diagnostic messages.
        /// </param>
        /// <returns>The 'best' solution (if one exists). Null otherwise.</returns>
        public static IEnumerable<T> FindSolution(IEnumerable<IEnumerable<T>> groupedItems,
            IComparer<T> itemSorter,
            Func<T, T, bool> shouldRejectPairFunc,
            Action<IEnumerable<T>> diagnosticOutput)
        {
            var solver = new CombinationSolver<T>(groupedItems,
                    itemSorter,
                    shouldRejectPairFunc);

            return solver.FindSolution(diagnosticOutput);
        }

        private IEnumerable<T> FindSolution(Action<IEnumerable<T>> diagnosticOutput)
        {
            var consistent = true;
            var i = 0;
            var highest = -1;

            var limit = 0;

            while (true)
            {
                if (!consistent)
                {
                    limit++;

                    if (limit > 10000)
                    {
                        return null;
                    }
                }

                i = consistent ? MoveForward(i, ref consistent) : MoveBackward(i, ref consistent);

                if (diagnosticOutput != null && i > highest)
                {
                    highest = i;

                    // if a diagnostic hook was passed in give it each new best solution as it occurs
                    // create a new list since this method reuses the solution array
                    diagnosticOutput(new List<T>(_solution));
                }

                if (i > _solution.Length)
                {
                    Debug.Fail("Evaluated past the end of the array.");

                    throw new NuGetResolverException(Strings.FatalError);
                }
                else if (i == _solution.Length)
                {
                    return _solution;
                }
                else if (i < 0)
                {
                    // Impossible (no solution)
                    return null;
                }
            }
        }

        /// <summary>
        /// Attempts to populate the element at position i with a consistent possibility
        /// and move forward to the next element in the sequence.
        /// </summary>
        /// <param name="i">The position in the solution to attempt to populate.</param>
        /// <param name="consistent">
        /// Upon completion, set to true if the function was able to find a candidate to
        /// populate position i with. False otherwise.
        /// </param>
        /// <returns>
        /// The next position to evaluate if consistent is true. If false, return value is the value to move
        /// back to.
        /// </returns>
        private int MoveForward(int i, ref bool consistent)
        {
            consistent = false;

            //Call ToList so we can potentially remove the currentItem from currentDomains[i] as we're iterating
            foreach (var currentItem in GetSortedList(i))
            {
                if (consistent)
                {
                    break;
                }

                consistent = true;
                _solution[i] = currentItem;

                for (var j = i + 1; j < _currentDomains.Count && consistent; j++)
                {
                    consistent = CheckForward(i, j);
                    if (!consistent)
                    {
                        _currentDomains[i].Remove(currentItem);
                        UndoReductions(i);
                        _conflictSet[i].UnionWith(_pastForwardChecking[j]);
                    }
                }
            }

            return consistent ? i + 1 : i;
        }

        /// <summary>
        /// Attempts to move back in the algorithm from position i.
        /// </summary>
        /// <param name="i">The position to unset / move back from.</param>
        /// <param name="consistent">
        /// True if backwards move was successful and algorithm can move forward again. False
        /// if the algorithm should continue to move backwards.
        /// </param>
        /// <returns>The position that the call was able to safely move back to.</returns>
        private int MoveBackward(int i, ref bool consistent)
        {
            if (i < 0
                || i >= _solution.Length)
            {
                Debug.Fail("MoveBackward called with invalid value for i.");
                throw new NuGetResolverException(Strings.FatalError);
            }

            if (i == 0
                && !consistent)
            {
                //We're being asked to back up from the starting position. No solution is possible.
                return -1;
            }

            var max = new Func<IEnumerable<int>, int>(enumerable => (enumerable == null || !enumerable.Any()) ? 0 : enumerable.Max());

            //h is the index we can *safely* move back to
            var h = Math.Max(max(_conflictSet[i]), max(_pastForwardChecking[i]));
            _conflictSet[h] = new HashSet<int>(_conflictSet[i].Union(_pastForwardChecking[i]).Union(_conflictSet[h]).Except(new[] { h }));

            for (var j = i; j > h; j--)
            {
                _conflictSet[j].Clear();
                UndoReductions(j);
                UpdateCurrentDomain(j);
            }

            UndoReductions(h);
            _currentDomains[h].Remove(_solution[h]);
            consistent = _currentDomains[h] != null && _currentDomains[h].Any();

            return h;
        }

        /// <summary>
        /// Performs forward checking between the already selected element at position i
        /// and potential candidates at position j.
        /// </summary>
        /// <param name="i">The position of the current element.</param>
        /// <param name="j">The position of the future domain to check against.</param>
        /// <returns>
        /// True if there are still remaining possibilities in the future domain. False if all possibilities
        /// have been eliminated.
        /// </returns>
        private bool CheckForward(int i, int j)
        {
            var reductionAgainstFutureDomain = new Stack<T>();
            foreach (var itemInFutureDomain in GetSortedList(j))
            {
                _solution[j] = itemInFutureDomain;

                if (_shouldRejectPair(_solution[i], _solution[j]))
                {
                    reductionAgainstFutureDomain.Push(itemInFutureDomain);
                }
            }

            if (reductionAgainstFutureDomain.Count > 0)
            {
                //Remove the items from the future domain
                _currentDomains[j].ExceptWith(reductionAgainstFutureDomain);

                //Store the items we just removed as a 'reduction' against the future domain.
                _reductions[j].Push(reductionAgainstFutureDomain);

                //Record that we've done future forward checking/reduction from i=>j
                _futureForwardChecking[i].Push(j);

                //Likewise in the past array, store that we've done forward checking/reduction from j=>i
                _pastForwardChecking[j].Push(i);
            }

            return _currentDomains[j].Count > 0;
        }

        /// <summary>
        /// Undo reductions that were previously performed from position i.
        /// </summary>
        /// <param name="i">The position to undo reductions from.</param>
        private void UndoReductions(int i)
        {
            foreach (var j in _futureForwardChecking[i])
            {
                var reduction = _reductions[j].Pop();
                _currentDomains[j].UnionWith(reduction);

                var pfc = _pastForwardChecking[j].Pop();
                Debug.Assert(i == pfc);
            }

            _futureForwardChecking[i].Clear();
        }

        /// <summary>
        /// Reinitialize the current domain to its initial value and apply any reductions against it.
        /// </summary>
        /// <param name="i">The position of the domain to update.</param>
        private void UpdateCurrentDomain(int i)
        {
            // Initialize it to the original domain values. Since currentDomain[i] will be
            // manipulated throughout the algorithm, it is critical to create a *new* set at this
            // point to avoid having initialDomains[i] be tampered with.
            _currentDomains[i] = new HashSet<T>(_initialDomains[i]);

            //Remove any current reduction items
            foreach (var reduction in _reductions[i])
            {
                _currentDomains[i].ExceptWith(reduction);
            }
        }

        private IEnumerable<T> GetSortedList(int pos)
        {
            var subSet = _currentDomains[pos];
            var sorted = _currentDomainsSorted[pos];

            if (subSet.Count == sorted.Count)
            {
                foreach (var item in sorted)
                {
                    yield return item;
                }
            }
            else
            {
                foreach (var item in sorted)
                {
                    if (subSet.Contains(item))
                    {
                        yield return item;
                    }
                }
            }

            yield break;
        }
    }
}
