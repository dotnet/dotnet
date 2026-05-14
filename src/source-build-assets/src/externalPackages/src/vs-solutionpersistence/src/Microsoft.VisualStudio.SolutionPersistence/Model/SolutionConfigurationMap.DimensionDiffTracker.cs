// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.SolutionPersistence.Model;

internal sealed partial class SolutionConfigurationMap
{
    // Keeps track of changes to a specific dimension value.
    // This is used to tell if the values are the same and a configuration rule can be created.
    private struct DimensionDiffTracker<T>
    {
        private int itemsChecked;
        private int differences;
        private T firstDifferent;
        private bool anyDifferent;

        // There was at least one item that was different than the expected value.
        internal readonly bool HasDifferences => this.differences > 0;

        // All items are different than expected, but they are the same as each other.
        internal readonly bool SameDifference => !this.anyDifferent && this.itemsChecked == this.differences && this.itemsChecked > 0;

        internal void ObserveDifferentValue(T current)
        {
            this.itemsChecked++;
            this.differences++;
            if (this.differences == 1)
            {
                this.firstDifferent = current;
            }
            else
            {
                this.anyDifferent = this.anyDifferent || !EqualityComparer<T>.Default.Equals(this.firstDifferent, current);
            }
        }

        internal void ObserveValue(T expected, T current)
        {
            if (!EqualityComparer<T>.Default.Equals(expected, current))
            {
                this.ObserveDifferentValue(current);
            }
            else
            {
                this.itemsChecked++;
            }
        }

        internal void ClearDifferences()
        {
            this.differences = 0;
            this.itemsChecked = 0;
            this.anyDifferent = false;
            this.firstDifferent = default!;
        }

        internal readonly bool TryGetSame(out T sameChanged)
        {
            sameChanged = this.firstDifferent;
            return this.SameDifference;
        }

        internal readonly bool TryGetSame(DimensionDiffTracker<T> alternate, out T sameChanged)
        {
            if (this.TryGetSame(out sameChanged))
            {
                return true;
            }

            if (this.HasDifferences)
            {
                return alternate.TryGetSame(out sameChanged);
            }

            return false;
        }
    }
}
