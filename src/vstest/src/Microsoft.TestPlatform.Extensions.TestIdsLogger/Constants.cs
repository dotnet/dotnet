// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.TestPlatform.Extensions.TestIdsLogger;

/// <summary>
/// Constants identifying the test id report logger and its parameters.
/// </summary>
/// <remarks>
/// <para>
/// TEMPORARY - THIS LOGGER WILL BE REMOVED. It exists only to support migrating stored test case
/// ids across the change of id hashing algorithm, and is deleted together with the SHA1
/// implementation it reports on. See the class level remarks on <see cref="TestIdsLogger"/>.
/// </para>
/// <para>
/// Internal, unlike the equivalent on the permanent loggers. A component whose stated plan is
/// deletion should not leave public API behind for its removal to break; the uri and the friendly
/// name are the contract, and those are documented rather than exported.
/// </para>
/// </remarks>
internal static class Constants
{
    /// <summary>
    /// Uri used to uniquely identify the test id report logger.
    /// </summary>
    internal const string ExtensionUri = "logger://Microsoft/TestPlatform/TestIdsLogger/v1";

    /// <summary>
    /// Alternate user friendly string to uniquely identify the test id report logger.
    /// </summary>
    internal const string FriendlyName = "testids";

    /// <summary>
    /// Log file parameter key, holding the path the report is written to. An absolute path is used
    /// as given, a relative one is resolved against the test results directory.
    /// </summary>
    internal const string LogFileNameKey = "LogFileName";

    /// <summary>
    /// The report file name used when <see cref="LogFileNameKey"/> is not given and the platform
    /// reported no target framework.
    /// </summary>
    /// <remarks>
    /// A fixed stem rather than a timestamp, so that the name is recognisable and the first run into
    /// a clean results directory produces exactly it. It is qualified by the target framework, and
    /// an already claimed name iterates to <c>TestIds_net8.0(1).csv</c> rather than overwriting,
    /// because the projects of one solution share a results directory and a mapping replaced by
    /// another project's is a mapping lost. The path actually written is printed at the end of the
    /// run, and <see cref="LogFileNameKey"/> is there for a script that needs to know the path up
    /// front. Internal, because a real run almost always qualifies the name by framework and a public
    /// constant naming a file that is usually absent is worse than none.
    /// </remarks>
    internal const string DefaultReportFileName = DefaultReportFileNameWithoutExtension + ReportFileExtension;

    /// <summary>
    /// The stem of <see cref="DefaultReportFileName"/>, to which the target framework is appended.
    /// </summary>
    internal const string DefaultReportFileNameWithoutExtension = "TestIds";

    /// <summary>
    /// The extension of the report file.
    /// </summary>
    internal const string ReportFileExtension = ".csv";

    /// <summary>
    /// Property id of the managed type a test case carries when its adapter reports one.
    /// </summary>
    /// <remarks>
    /// Matched by id against the properties present on the test case rather than looked up in the
    /// global property store, because whether the property has been registered in this process
    /// depends on load order that a logger has no control over.
    /// </remarks>
    internal const string ManagedTypePropertyId = "TestCase.ManagedType";

    /// <summary>
    /// Property id of the managed method a test case carries when its adapter reports one.
    /// </summary>
    internal const string ManagedMethodPropertyId = "TestCase.ManagedMethod";
}
