namespace Microsoft.IdentityModel.Json.Linq
{
    /// <summary>
    /// Specifies how JSON arrays are merged together.
    /// </summary>
    internal enum MergeArrayHandling
    {
        /// <summary>Concatenate arrays.</summary>
        Concat = 0,

        /// <summary>Union arrays, skipping items that already exist.</summary>
        Union = 1,

        /// <summary>Replace all array items.</summary>
        Replace = 2,

        /// <summary>Merge array items together, matched by index.</summary>
        Merge = 3
    }
}
