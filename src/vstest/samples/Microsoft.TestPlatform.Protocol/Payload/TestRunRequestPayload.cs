// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.TestPlatform.Protocol
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Class used to define the TestRunRequestPayload sent to Vstest.console in design mode
    /// </summary>
    public class TestRunRequestPayload
    {
        /// <summary>
        /// Gets or sets the sources for the test run request.
        /// </summary>
        [DataMember]
        public List<string> Sources { get; set; }

        /// <summary>
        /// Gets or sets the test cases for the test run request.
        /// </summary>
        [DataMember]
        public dynamic TestCases { get; set; }

        /// <summary>
        /// Gets or sets the settings used for the test run request.
        /// </summary>
        [DataMember]
        public string RunSettings { get; set; }

        /// <summary>
        /// Settings used for the Run request.
        /// </summary>
        [DataMember]
        public bool KeepAlive { get; set; }

        /// <summary>
        /// Is Debugging enabled
        /// </summary>
        [DataMember]
        public bool DebuggingEnabled { get; set; }
    }
}
