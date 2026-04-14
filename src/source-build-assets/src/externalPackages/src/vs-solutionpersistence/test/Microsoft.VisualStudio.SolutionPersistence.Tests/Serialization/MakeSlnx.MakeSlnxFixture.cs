// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Serialization;

/// <summary>
/// Fixture for <see cref="MakeSlnx"/> tests.
/// </summary>
public sealed partial class MakeSlnx
{
    /// <summary>
    /// The temp subdirectory name for the output directory.
    /// </summary>
    public const string OutputDirectory = "OutputSln";

    /// <summary>
    /// Used by <see cref="MakeSlnx"/> tests to ensure temp directory structure is created and
    /// empty before the tests are run.
    /// </summary>
    public class MakeSlnxFixture
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MakeSlnxFixture"/> class.
        /// </summary>
        public MakeSlnxFixture()
        {
            string outputDirectory = Path.Combine(Path.GetTempPath(), OutputDirectory);
            if (Directory.Exists(outputDirectory))
            {
                Directory.Delete(outputDirectory, true);
            }

            _ = Directory.CreateDirectory(outputDirectory);

            this.SlnToSlnxDirectory = Path.Join(outputDirectory, "slnToSlnx");
            this.SlnViaSlnxDirectory = Path.Join(outputDirectory, "slnViaSlnx");
            this.SlnxToSlnDirectory = Path.Join(outputDirectory, "slnxToSln");
            _ = Directory.CreateDirectory(this.SlnToSlnxDirectory);
            _ = Directory.CreateDirectory(this.SlnViaSlnxDirectory);
            _ = Directory.CreateDirectory(this.SlnxToSlnDirectory);
        }

        /// <summary>
        /// Gets the full path to the directory where the SLN files are converted to SLNX.
        /// </summary>
        public string SlnToSlnxDirectory { get; private set; }

        /// <summary>
        /// Gets the full path to the directory where the SLN files are converted to SLNX and back to SLN.
        /// </summary>
        public string SlnViaSlnxDirectory { get; private set; }

        /// <summary>
        /// Gets the full path to the directory where the SLNX files are converted to SLN.
        /// </summary>
        public string SlnxToSlnDirectory { get; private set; }
    }
}
