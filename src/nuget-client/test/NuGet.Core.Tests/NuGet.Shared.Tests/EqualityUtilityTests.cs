// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace NuGet.Shared.Tests
{
    public class EqualityUtilityTests
    {
        // OrderedEquals IList

        [Fact]
        public void OrderedEquals_CompareWithNullList_ReturnsFalse()
        {
            // Arrange
            var list = new List<string>();

            // Act & Assert
            list.OrderedEquals(null, s => s, StringComparer.Ordinal, StringComparer.Ordinal).Should().BeFalse();
        }

        [Fact]
        public void OrderedEquals_CompareFromANullList_ReturnsFalse()
        {
            // Arrange
            List<string> list = null;

            // Act & Assert
            list.OrderedEquals(new List<string>(), s => s, StringComparer.Ordinal, StringComparer.Ordinal).Should().BeFalse();
        }

        [Fact]
        public void OrderedEquals_CompareTwoNullLists_ReturnsTrue()
        {
            // Arrange
            List<string> list = null;

            // Act & Assert
            list.OrderedEquals(null, s => s, StringComparer.Ordinal, StringComparer.Ordinal).Should().BeTrue();
        }

        [Fact]
        public void OrderedEquals_CompareSameLists_ReturnsTrue()
        {
            // Arrange
            var list = new List<string>();

            // Act & Assert
            list.OrderedEquals(list, s => s, StringComparer.Ordinal, StringComparer.Ordinal).Should().BeTrue();
        }

        [Fact]
        public void OrderedEquals_CompareEmptyLists_ReturnsTrue()
        {
            // Arrange
            var list1 = new List<string>();
            var list2 = new List<string>();

            // Act & Assert
            list1.OrderedEquals(list2, s => s, StringComparer.OrdinalIgnoreCase, StringComparer.OrdinalIgnoreCase).Should().BeTrue();
        }

        [Fact]
        public void OrderedEquals_CompareListsWithDifferentLengths_ReturnsFalse()
        {
            // Arrange
            var list1 = new List<string> { "unit.test" };
            var list2 = new List<string> { "unit.test", "unit.test" };

            // Act & Assert
            list1.OrderedEquals(list2, s => s, StringComparer.OrdinalIgnoreCase, StringComparer.OrdinalIgnoreCase).Should().BeFalse();
        }

        [Fact]
        public void OrderedEquals_CompareListsWithDifferentCaseWithOrdinalIgnoreCase_ReturnsTrue()
        {
            // Arrange
            var list1 = new List<string> { "unit.test" };
            var list2 = new List<string> { "Unit.test" };

            // Act & Assert
            list1.OrderedEquals(list2, s => s, StringComparer.OrdinalIgnoreCase, StringComparer.OrdinalIgnoreCase).Should().BeTrue();
        }

        [Fact]
        public void OrderedEquals_CompareListsWithDifferentCaseWithOrdinal_ReturnsFalse()
        {
            // Arrange
            var list1 = new List<string> { "unit.test" };
            var list2 = new List<string> { "Unit.test" };

            // Act & Assert
            list1.OrderedEquals(list2, s => s, StringComparer.Ordinal, StringComparer.Ordinal).Should().BeFalse();
        }

        [Fact]
        public void OrderedEquals_CompareListsWithSameElementsInDifferentOrder_ReturnsTrue()
        {
            // Arrange
            var list1 = new List<string> { "unit", "test" };
            var list2 = new List<string> { "test", "unit" };

            // Act & Assert
            list1.OrderedEquals(list2, x => x, StringComparer.Ordinal, StringComparer.Ordinal).Should().BeTrue();
        }

        //  OrderedEquals ICollection

        [Fact]
        public void OrderedEquals_CompareWithNullCollection_ReturnsFalse()
        {
            // Arrange
            ICollection<string> list = new List<string>();

            // Act & Assert
            list.OrderedEquals(null, s => s, StringComparer.Ordinal, StringComparer.Ordinal).Should().BeFalse();
        }

        [Fact]
        public void OrderedEquals_CompareFromANullCollection_ReturnsFalse()
        {
            // Arrange
            ICollection<string> list = null;

            // Act & Assert
            list.OrderedEquals(new List<string>(), s => s, StringComparer.Ordinal, StringComparer.Ordinal).Should().BeFalse();
        }

        [Fact]
        public void OrderedEquals_CompareTwoNullCollections_ReturnsTrue()
        {
            // Arrange
            ICollection<string> list = null;

            // Act & Assert
            list.OrderedEquals(null, s => s, StringComparer.Ordinal, StringComparer.Ordinal).Should().BeTrue();
        }

        [Fact]
        public void OrderedEquals_CompareSameCollections_ReturnsTrue()
        {
            // Arrange
            ICollection<string> list = new List<string>();

            // Act & Assert
            list.OrderedEquals(list, s => s, StringComparer.Ordinal, StringComparer.Ordinal).Should().BeTrue();
        }

        [Fact]
        public void OrderedEquals_CompareEmptyCollections_ReturnsTrue()
        {
            // Arrange
            ICollection<string> list1 = new List<string>();
            ICollection<string> list2 = new List<string>();

            // Act & Assert
            list1.OrderedEquals(list2, s => s, StringComparer.OrdinalIgnoreCase, StringComparer.OrdinalIgnoreCase).Should().BeTrue();
        }

        [Fact]
        public void OrderedEquals_CompareCollectionsWithDifferentLengths_ReturnsFalse()
        {
            // Arrange
            ICollection<string> list1 = new List<string> { "unit.test" };
            ICollection<string> list2 = new List<string> { "unit.test", "unit.test" };

            // Act & Assert
            list1.OrderedEquals(list2, s => s, StringComparer.OrdinalIgnoreCase, StringComparer.OrdinalIgnoreCase).Should().BeFalse();
        }

        [Fact]
        public void OrderedEquals_CompareCollectionsWithDifferentCaseWithOrdinalIgnoreCase_ReturnsTrue()
        {
            // Arrange
            ICollection<string> list1 = new List<string> { "unit.test" };
            ICollection<string> list2 = new List<string> { "Unit.test" };

            // Act & Assert
            list1.OrderedEquals(list2, s => s, StringComparer.OrdinalIgnoreCase, StringComparer.OrdinalIgnoreCase).Should().BeTrue();
        }

        [Fact]
        public void OrderedEquals_CompareCollectionsWithDifferentCaseWithOrdinal_ReturnsFalse()
        {
            // Arrange
            ICollection<string> list1 = new List<string> { "unit.test" };
            ICollection<string> list2 = new List<string> { "Unit.test" };

            // Act & Assert
            list1.OrderedEquals(list2, s => s, StringComparer.Ordinal, StringComparer.Ordinal).Should().BeFalse();
        }

        [Fact]
        public void OrderedEquals_CompareCollectionsWithSameElementsInDifferentOrder_ReturnsTrue()
        {
            // Arrange
            ICollection<string> list1 = new List<string> { "unit", "test" };
            ICollection<string> list2 = new List<string> { "test", "unit" };

            // Act & Assert
            list1.OrderedEquals(list2, x => x, StringComparer.Ordinal, StringComparer.Ordinal).Should().BeTrue();
        }

        // OrderedEquals IEnumerable

        [Fact]
        public void OrderedEquals_CompareWithNullEnumerable_ReturnsFalse()
        {
            // Arrange
            IEnumerable<string> list = new List<string>();

            // Act & Assert
            list.OrderedEquals(null, s => s, StringComparer.Ordinal, StringComparer.Ordinal).Should().BeFalse();
        }

        [Fact]
        public void OrderedEquals_CompareFromANullEnumerable_ReturnsFalse()
        {
            // Arrange
            IEnumerable<string> list = null;

            // Act & Assert
            list.OrderedEquals(new List<string>(), s => s, StringComparer.Ordinal, StringComparer.Ordinal).Should().BeFalse();
        }

        [Fact]
        public void OrderedEquals_CompareTwoNullEnumerables_ReturnsTrue()
        {
            // Arrange
            IEnumerable<string> list = null;

            // Act & Assert
            list.OrderedEquals(null, s => s, StringComparer.Ordinal, StringComparer.Ordinal).Should().BeTrue();
        }

        [Fact]
        public void OrderedEquals_CompareSameEnumerables_ReturnsTrue()
        {
            // Arrange
            IEnumerable<string> list = new List<string>();

            // Act & Assert
            list.OrderedEquals(list, s => s, StringComparer.Ordinal, StringComparer.Ordinal).Should().BeTrue();
        }

        [Fact]
        public void OrderedEquals_CompareEmptyEnumerables_ReturnsTrue()
        {
            // Arrange
            IEnumerable<string> list1 = new List<string>();
            IEnumerable<string> list2 = new List<string>();

            // Act & Assert
            list1.OrderedEquals(list2, s => s, StringComparer.OrdinalIgnoreCase, StringComparer.OrdinalIgnoreCase).Should().BeTrue();
        }

        [Fact]
        public void OrderedEquals_CompareEnumerablesWithDifferentLengths_ReturnsFalse()
        {
            // Arrange
            IEnumerable<string> list1 = new List<string> { "unit.test" };
            IEnumerable<string> list2 = new List<string> { "unit.test", "unit.test" };

            // Act & Assert
            list1.OrderedEquals(list2, s => s, StringComparer.OrdinalIgnoreCase, StringComparer.OrdinalIgnoreCase).Should().BeFalse();
        }

        [Fact]
        public void OrderedEquals_CompareEnumerablesWithDifferentCaseWithOrdinalIgnoreCase_ReturnsTrue()
        {
            // Arrange
            IEnumerable<string> list1 = new List<string> { "unit.test" };
            IEnumerable<string> list2 = new List<string> { "Unit.test" };

            // Act & Assert
            list1.OrderedEquals(list2, s => s, StringComparer.OrdinalIgnoreCase, StringComparer.OrdinalIgnoreCase).Should().BeTrue();
        }

        [Fact]
        public void OrderedEquals_CompareEnumerablesWithDifferentCaseWithOrdinal_ReturnsFalse()
        {
            // Arrange
            IEnumerable<string> list1 = new List<string> { "unit.test" };
            IEnumerable<string> list2 = new List<string> { "Unit.test" };

            // Act & Assert
            list1.OrderedEquals(list2, s => s, StringComparer.Ordinal, StringComparer.Ordinal).Should().BeFalse();
        }

        [Fact]
        public void OrderedEquals_CompareEnumerablesWithSameElementsInDifferentOrder_ReturnsTrue()
        {
            // Arrange
            IEnumerable<string> list1 = new List<string> { "unit", "test" };
            IEnumerable<string> list2 = new List<string> { "test", "unit" };

            // Act & Assert
            list1.OrderedEquals(list2, x => x, StringComparer.Ordinal, StringComparer.Ordinal).Should().BeTrue();
        }

        // SequenceEqualWithNullCheck IList

        [Fact]
        public void SequenceEqualWithNullCheck_CompareWithNullList_ReturnsFalse()
        {
            // Arrange
            var list = new List<string>();

            // Act & Assert
            list.SequenceEqualWithNullCheck(null).Should().BeFalse();
        }

        [Fact]
        public void SequenceEqualWithNullCheck_CompareFromANullList_ReturnsFalse()
        {
            // Arrange
            List<string> list = null;

            // Act & Assert
            list.SequenceEqualWithNullCheck(new List<string>()).Should().BeFalse();
        }

        [Fact]
        public void SequenceEqualWithNullCheck_CompareTwoNullLists_ReturnsTrue()
        {
            // Arrange
            List<string> list = null;

            // Act & Assert
            list.SequenceEqualWithNullCheck(null).Should().BeTrue();
        }

        [Fact]
        public void SequenceEqualWithNullCheck_CompareSameLists_ReturnsTrue()
        {
            // Arrange
            var list = new List<string>();

            // Act & Assert
            list.SequenceEqualWithNullCheck(list, StringComparer.Ordinal).Should().BeTrue();
        }

        [Fact]
        public void SequenceEqualWithNullCheck_CompareEmptyLists_ReturnsTrue()
        {
            // Arrange
            var list1 = new List<string>();
            var list2 = new List<string>();

            // Act & Assert
            list1.SequenceEqualWithNullCheck(list2, StringComparer.Ordinal).Should().BeTrue();
        }

        [Fact]
        public void SequenceEqualWithNullCheck_CompareListsWithDifferentLengths_ReturnsFalse()
        {
            // Arrange
            var list1 = new List<string> { "unit.test" };
            var list2 = new List<string> { "unit.test", "unit.test" };

            // Act & Assert
            list1.SequenceEqualWithNullCheck(list2, StringComparer.OrdinalIgnoreCase).Should().BeFalse();
        }

        [Fact]
        public void SequenceEqualWithNullCheck_CompareListsWithDifferentCaseWithOrdinalIgnoreCase_ReturnsTrue()
        {
            // Arrange
            var list1 = new List<string> { "unit.test" };
            var list2 = new List<string> { "Unit.test" };

            // Act & Assert
            list1.SequenceEqualWithNullCheck(list2, StringComparer.OrdinalIgnoreCase).Should().BeTrue();
        }

        [Fact]
        public void SequenceEqualWithNullCheck_CompareListsWithDifferentCaseWithOrdinal_ReturnsFalse()
        {
            // Arrange
            var list1 = new List<string> { "unit.test" };
            var list2 = new List<string> { "Unit.test" };

            // Act & Assert
            list1.SequenceEqualWithNullCheck(list2, StringComparer.Ordinal).Should().BeFalse();
        }

        [Fact]
        public void SequenceEqualWithNullCheck_CompareListsWithSameElementsInDifferentOrder_ReturnsFalse()
        {
            // Arrange
            var list1 = new List<string> { "unit", "test" };
            var list2 = new List<string> { "test", "unit" };

            // Act & Assert
            list1.SequenceEqualWithNullCheck(list2, StringComparer.Ordinal).Should().BeFalse();
        }

        // SequenceEqualWithNullCheck ICollection

        [Fact]
        public void SequenceEqualWithNullCheck_CompareWithNullCollection_ReturnsFalse()
        {
            // Arrange
            ICollection<string> list = new List<string>();

            // Act & Assert
            list.SequenceEqualWithNullCheck(null).Should().BeFalse();
        }

        [Fact]
        public void SequenceEqualWithNullCheck_CompareFromANullCollection_ReturnsFalse()
        {
            // Arrange
            ICollection<string> list = null;

            // Act & Assert
            list.SequenceEqualWithNullCheck(new List<string>()).Should().BeFalse();
        }

        [Fact]
        public void SequenceEqualWithNullCheck_CompareTwoNullCollections_ReturnsTrue()
        {
            // Arrange
            ICollection<string> list = null;

            // Act & Assert
            list.SequenceEqualWithNullCheck(null).Should().BeTrue();
        }

        [Fact]
        public void SequenceEqualWithNullCheck_CompareSameCollections_ReturnsTrue()
        {
            // Arrange
            ICollection<string> list = new List<string>();

            // Act & Assert
            list.SequenceEqualWithNullCheck(list, StringComparer.Ordinal).Should().BeTrue();
        }

        [Fact]
        public void SequenceEqualWithNullCheck_CompareEmptyCollections_ReturnsTrue()
        {
            // Arrange
            ICollection<string> list1 = new List<string>();
            ICollection<string> list2 = new List<string>();

            // Act & Assert
            list1.SequenceEqualWithNullCheck(list2, StringComparer.Ordinal).Should().BeTrue();
        }

        [Fact]
        public void SequenceEqualWithNullCheck_CompareCollectionsWithDifferentLengths_ReturnsFalse()
        {
            // Arrange
            ICollection<string> list1 = new List<string> { "unit.test" };
            ICollection<string> list2 = new List<string> { "unit.test", "unit.test" };

            // Act & Assert
            list1.SequenceEqualWithNullCheck(list2, StringComparer.OrdinalIgnoreCase).Should().BeFalse();
        }

        [Fact]
        public void SequenceEqualWithNullCheck_CompareCollectionsWithDifferentCaseWithOrdinalIgnoreCase_ReturnsTrue()
        {
            // Arrange
            ICollection<string> list1 = new List<string> { "unit.test" };
            ICollection<string> list2 = new List<string> { "Unit.test" };

            // Act & Assert
            list1.SequenceEqualWithNullCheck(list2, StringComparer.OrdinalIgnoreCase).Should().BeTrue();
        }

        [Fact]
        public void SequenceEqualWithNullCheck_CompareCollectionsWithDifferentCaseWithOrdinal_ReturnsFalse()
        {
            // Arrange
            ICollection<string> list1 = new List<string> { "unit.test" };
            ICollection<string> list2 = new List<string> { "Unit.test" };

            // Act & Assert
            list1.SequenceEqualWithNullCheck(list2, StringComparer.Ordinal).Should().BeFalse();
        }

        [Fact]
        public void SequenceEqualWithNullCheck_CompareCollectionsWithSameElementsInDifferentOrder_ReturnsFalse()
        {
            // Arrange
            ICollection<string> list1 = new List<string> { "unit", "test" };
            ICollection<string> list2 = new List<string> { "test", "unit" };

            // Act & Assert
            list1.SequenceEqualWithNullCheck(list2, StringComparer.Ordinal).Should().BeFalse();
        }

        // SequenceEqualWithNullCheck IEnumerable

        [Fact]
        public void SequenceEqualWithNullCheck_CompareWithNullEnumerable_ReturnsFalse()
        {
            // Arrange
            IEnumerable<string> list = new List<string>();

            // Act & Assert
            list.SequenceEqualWithNullCheck(null).Should().BeFalse();
        }

        [Fact]
        public void SequenceEqualWithNullCheck_CompareFromANullEnumerable_ReturnsFalse()
        {
            // Arrange
            IEnumerable<string> list = null;

            // Act & Assert
            list.SequenceEqualWithNullCheck(new List<string>()).Should().BeFalse();
        }

        [Fact]
        public void SequenceEqualWithNullCheck_CompareTwoNullEnumerables_ReturnsTrue()
        {
            // Arrange
            IEnumerable<string> list = null;

            // Act & Assert
            list.SequenceEqualWithNullCheck(null).Should().BeTrue();
        }

        [Fact]
        public void SequenceEqualWithNullCheck_CompareSameEnumerables_ReturnsTrue()
        {
            // Arrange
            IEnumerable<string> list = new List<string>();

            // Act & Assert
            list.SequenceEqualWithNullCheck(list, StringComparer.Ordinal).Should().BeTrue();
        }

        [Fact]
        public void SequenceEqualWithNullCheck_CompareEmptyEnumerables_ReturnsTrue()
        {
            // Arrange
            IEnumerable<string> list1 = new List<string>();
            IEnumerable<string> list2 = new List<string>();

            // Act & Assert
            list1.SequenceEqualWithNullCheck(list2, StringComparer.Ordinal).Should().BeTrue();
        }

        [Fact]
        public void SequenceEqualWithNullCheck_CompareEnumerablesWithDifferentLengths_ReturnsFalse()
        {
            // Arrange
            IEnumerable<string> list1 = new List<string> { "unit.test" };
            IEnumerable<string> list2 = new List<string> { "unit.test", "unit.test" };

            // Act & Assert
            list1.SequenceEqualWithNullCheck(list2, StringComparer.OrdinalIgnoreCase).Should().BeFalse();
        }

        [Fact]
        public void SequenceEqualWithNullCheck_CompareEnumerablesWithDifferentCaseWithOrdinalIgnoreCase_ReturnsTrue()
        {
            // Arrange
            IEnumerable<string> list1 = new List<string> { "unit.test" };
            IEnumerable<string> list2 = new List<string> { "Unit.test" };

            // Act & Assert
            list1.SequenceEqualWithNullCheck(list2, StringComparer.OrdinalIgnoreCase).Should().BeTrue();
        }

        [Fact]
        public void SequenceEqualWithNullCheck_CompareEnumerablesWithDifferentCaseWithOrdinal_ReturnsFalse()
        {
            // Arrange
            IEnumerable<string> list1 = new List<string> { "unit.test" };
            IEnumerable<string> list2 = new List<string> { "Unit.test" };

            // Act & Assert
            list1.SequenceEqualWithNullCheck(list2, StringComparer.Ordinal).Should().BeFalse();
        }

        [Fact]
        public void SequenceEqualWithNullCheck_CompareEnumerablesWithSameElementsInDifferentOrder_ReturnsFalse()
        {
            // Arrange
            IEnumerable<string> list1 = new List<string> { "unit", "test" };
            IEnumerable<string> list2 = new List<string> { "test", "unit" };

            // Act & Assert
            list1.SequenceEqualWithNullCheck(list2, StringComparer.Ordinal).Should().BeFalse();
        }

        // SetEqualsWithNullCheck

        [Fact]
        public void SetEqualsWithNullCheck_CompareWithNullSet_ReturnsFalse()
        {
            // Arrange
            var set = new HashSet<string>();

            // Act & Assert
            set.SetEqualsWithNullCheck(null).Should().BeFalse();
        }

        [Fact]
        public void SetEqualsWithNullCheck_CompareFromANullSet_ReturnsFalse()
        {
            // Arrange
            HashSet<string> set = null;

            // Act & Assert
            set.SetEqualsWithNullCheck(new HashSet<string>()).Should().BeFalse();
        }

        [Fact]
        public void SetEqualsWithNullCheck_CompareTwoNullSets_ReturnsTrue()
        {
            // Arrange
            HashSet<string> set = null;

            // Act & Assert
            set.SetEqualsWithNullCheck(null).Should().BeTrue();
        }

        [Fact]
        public void SetEqualsWithNullCheck_CompareTwoSetsWithDifferentLengths_ReturnsFalse()
        {
            // Arrange
            var set1 = new HashSet<string>() { "unit.test", "unit" };
            var set2 = new HashSet<string>() { "unit.test" };

            // Act & Assert
            set1.SetEqualsWithNullCheck(set2).Should().BeFalse();
        }

        [Fact]
        public void SetEqualsWithNullCheck_CompareSameSets_ReturnsTrue()
        {
            // Arrange
            var set = new HashSet<string>();

            // Act & Assert
            set.SetEqualsWithNullCheck(set).Should().BeTrue();
        }

        [Fact]
        public void SetEqualsWithNullCheck_CompareEmptySets_ReturnsTrue()
        {
            // Arrange
            var set1 = new HashSet<string>();
            var set2 = new HashSet<string>();

            // Act & Assert
            set1.SetEqualsWithNullCheck(set2).Should().BeTrue();
        }

        [Fact]
        public void SetEqualsWithNullCheck_CompareEquivalentSets_ReturnsTrue()
        {
            // Arrange
            var set1 = new HashSet<string>() { "unit.test" };
            var set2 = new HashSet<string>() { "unit.test" };

            // Act & Assert
            set1.SetEqualsWithNullCheck(set2).Should().BeTrue();
        }

        // DictionaryEquals

        [Fact]
        public void DictionaryEquals_CompareWithNullDict_ReturnsFalse()
        {
            // Arrange
            var dict = new Dictionary<int, string>();

            // Act & Assert
            EqualityUtility.DictionaryEquals(dict, null).Should().BeFalse();
        }
        [Fact]
        public void DictionaryEquals_CompareTwoNullDicts_ReturnsTrue()
        {
            // Arrange
            Dictionary<int, string> dict = null;

            // Act & Assert
            dict.SequenceEqualWithNullCheck(null).Should().BeTrue();
        }

        [Fact]
        public void DictionaryEquals_CompareTwoDictsWithDifferentLengths_ReturnsFalse()
        {
            // Arrange
            var dict1 = new Dictionary<int, string>() { { 1, "unit.test" }, { 2, "unit" } };
            var dict2 = new Dictionary<int, string>() { { 1, "unit.test" } };

            // Act & Assert
            EqualityUtility.DictionaryEquals(dict1, dict2).Should().BeFalse();
        }

        [Fact]
        public void DictionaryEquals_CompareSameDicts_ReturnsTrue()
        {
            // Arrange
            var dict = new Dictionary<int, string>();

            // Act & Assert
            EqualityUtility.DictionaryEquals(dict, dict).Should().BeTrue();
        }

        [Fact]
        public void DictionaryEquals_CompareEmptyDicts_ReturnsTrue()
        {
            // Arrange
            var dict1 = new Dictionary<int, string>();
            var dict2 = new Dictionary<int, string>();

            // Act & Assert
            EqualityUtility.DictionaryEquals(dict1, dict2).Should().BeTrue();
        }

        [Fact]
        public void DictionaryEquals_CompareEquivalentDicts_ReturnsTrue()
        {
            // Arrange
            var dict1 = new Dictionary<int, string>() { { 1, "unit.test" } };
            var dict2 = new Dictionary<int, string>() { { 1, "unit.test" } };

            // Act & Assert
            EqualityUtility.DictionaryEquals(dict1, dict2).Should().BeTrue();
        }
    }
}
