// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.TestPlatform.AdapterUtilities;

/// <summary>
/// Constants to help declare Hierarchy test property.
/// </summary>
public static class HierarchyConstants
{
    /// <summary>
    /// Label to use on Hierarchy test property.
    /// </summary>
    public const string HierarchyLabel = "Hierarchy";

    /// <summary>
    /// Property id to use on Hierarchy test property.
    /// </summary>
    public const string HierarchyPropertyId = "TestCase." + HierarchyLabel;

    /// <summary>
    /// Meanings of the indices in the Hierarchy array.
    /// </summary>
    public static class Levels
    {
        /// <summary>
        /// Total length of Hierarchy array.
        /// </summary>
        /// <remarks>
        /// Currently the order for this is usually:
        /// <c>Assembly Display Name</c>, <c>Namespace</c>, <c>ClassName</c>, <c>Managed Method Name</c>.
        /// </remarks>
        public const int TotalLevelCount = 4;

        /// <summary>
        /// Index of the test container element of the array.
        /// </summary>
        /// <remarks>
        /// This is usually test asssembly display name.
        /// </remarks>
        public const int ContainerIndex = 0;

        /// <summary>
        /// Index of the namespace element of the array.
        /// </summary>
        /// <remarks>
        /// This is usually test namespace without class name.
        /// </remarks>
        public const int NamespaceIndex = 1;

        /// <summary>
        /// Index of the class element of the array.
        /// </summary>
        /// <remarks>
        /// This is usually test class name without namespace.
        /// </remarks>
        public const int ClassIndex = 2;

        /// <summary>
        /// Index of the test group element of the array.
        /// </summary>
        /// <remarks>
        /// This is usually test method name.
        /// </remarks>
        public const int TestGroupIndex = 3;
    }
}
