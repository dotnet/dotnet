#if WCWIDTH
#pragma warning restore
#else
#pragma warning disable
#endif

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Wcwidth;

/// <summary>
/// Represents a Unicode version.
/// </summary>
[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
#if WCWIDTH_VISIBILITY_INTERNAL
    internal
#else
public
#endif
    enum Unicode
{
    /// <summary>
    /// Unicode version 4.1.0.
    /// </summary>
    [Description("4.1.0")] Version_4_1_0 = 0,

    /// <summary>
    /// Unicode version 5.0.0.
    /// </summary>
    [Description("5.0.0")] Version_5_0_0 = 1,

    /// <summary>
    /// Unicode version 5.1.0.
    /// </summary>
    [Description("5.1.0")] Version_5_1_0 = 2,

    /// <summary>
    /// Unicode version 5.2.0.
    /// </summary>
    [Description("5.2.0")] Version_5_2_0 = 3,

    /// <summary>
    /// Unicode version 6.0.0.
    /// </summary>
    [Description("6.0.0")] Version_6_0_0 = 4,

    /// <summary>
    /// Unicode version 6.1.0.
    /// </summary>
    [Description("6.1.0")] Version_6_1_0 = 5,

    /// <summary>
    /// Unicode version 6.2.0.
    /// </summary>
    [Description("6.2.0")] Version_6_2_0 = 6,

    /// <summary>
    /// Unicode version 6.3.0.
    /// </summary>
    [Description("6.3.0")] Version_6_3_0 = 7,

    /// <summary>
    /// Unicode version 7.0.0.
    /// </summary>
    [Description("7.0.0")] Version_7_0_0 = 8,

    /// <summary>
    /// Unicode version 8.0.0.
    /// </summary>
    [Description("8.0.0")] Version_8_0_0 = 9,

    /// <summary>
    /// Unicode version 9.0.0.
    /// </summary>
    [Description("9.0.0")] Version_9_0_0 = 10,

    /// <summary>
    /// Unicode version 10.0.0.
    /// </summary>
    [Description("10.0.0")] Version_10_0_0 = 11,

    /// <summary>
    /// Unicode version 11.0.0.
    /// </summary>
    [Description("11.0.0")] Version_11_0_0 = 12,

    /// <summary>
    /// Unicode version 12.0.0.
    /// </summary>
    [Description("12.0.0")] Version_12_0_0 = 13,

    /// <summary>
    /// Unicode version 12.1.0.
    /// </summary>
    [Description("12.1.0")] Version_12_1_0 = 14,

    /// <summary>
    /// Unicode version 13.0.0.
    /// </summary>
    [Description("13.0.0")] Version_13_0_0 = 15,

    /// <summary>
    /// Unicode version 14.0.0.
    /// </summary>
    [Description("14.0.0")] Version_14_0_0 = 16,

    /// <summary>
    /// Unicode version 15.0.0.
    /// </summary>
    [Description("15.0.0")] Version_15_0_0 = 17,

    /// <summary>
    /// Unicode version 15.1.0.
    /// </summary>
    [Description("15.1.0")] Version_15_1_0 = 18,

    /// <summary>
    /// Unicode version 16.0.0.
    /// </summary>
    [Description("16.0.0")] Version_16_0_0 = 19,

    /// <summary>
    /// Unicode version 17.0.0.
    /// </summary>
    [Description("17.0.0")] Version_17_0_0 = 20,
}