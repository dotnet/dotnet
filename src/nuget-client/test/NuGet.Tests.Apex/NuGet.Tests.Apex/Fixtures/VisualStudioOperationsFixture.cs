// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Test.Apex;
using Microsoft.Test.Apex.Services;
using Microsoft.Test.Apex.VisualStudio;
using NuGet.Console.TestContract;
using NuGet.PackageManagement.UI.TestContract;

namespace NuGet.Tests.Apex
{
    public class VisualStudioOperationsFixture
    {
        private VisualStudioHostConfiguration _visualStudioHostConfiguration;
        private readonly IOperations _operations;
        private readonly IAssertionVerifier _verifier;
        private readonly ITestLogger _testLogger;

        public VisualStudioOperationsFixture()
        {
            if (!Microsoft.Test.Apex.Operations.IsConfigured)
            {
                Microsoft.Test.Apex.Operations.Configure(new NuGetTestOperationConfiguration());
            }

            _operations = Microsoft.Test.Apex.Operations.Current;
            _verifier = _operations.Get<IAssertionVerifier>();
            _verifier.AssertionDelegate = FailAction;
            _verifier.AssertOnFirstFailure = true;
            _testLogger = _operations.Get<ITestLogger>();
        }

        internal VisualStudioHostConfiguration VisualStudioHostConfiguration
        {
            get
            {
                if (_visualStudioHostConfiguration == null)
                {
                    _visualStudioHostConfiguration = new VisualStudioHostConfiguration()
                    {
                        InheritProcessEnvironment = true,
                        InProcessHostConstraints = new List<ITypeConstraint>() { new NuGetTypeConstraint() }
                    };

                    string[] compositionAssemblies = new[]
                    {
                        typeof(NuGetApexTestService).Assembly.Location, // NuGet.Test.Apex.dll
                        typeof(NuGetApexUITestService).Assembly.Location, // NuGet.PackageManagement.UI.TestContract.dll
                        typeof(NuGetApexConsoleTestService).Assembly.Location, // NuGet.Console.TestContract.dll
                    };

                    foreach(var testAssembly in compositionAssemblies)
                    {
                        _visualStudioHostConfiguration.AddCompositionAssembly(testAssembly);
                    }

                    // If test is being run in VS, "Developer PowerShell" , or "Developer Command Prompt", use the same install of VS.
                    // But don't override Apex's env vars if they have already been set.
                    const string vsUnderTestVariableName = "VisualStudio.InstallationUnderTest.Path";
                    if (Environment.GetEnvironmentVariable(vsUnderTestVariableName) == null)
                    {
                        string vsInstallDir = Environment.GetEnvironmentVariable("VSAPPIDDIR") ?? Environment.GetEnvironmentVariable("DevEnvDir");
                        if (!string.IsNullOrEmpty(vsInstallDir))
                        {
                            var devenvPath = Path.Combine(vsInstallDir, "devenv.exe");
                            Environment.SetEnvironmentVariable(vsUnderTestVariableName, devenvPath);

                            const string rootSuffixVariableName = "VisualStudio.InstallationUnderTest.RootSuffix";
                            if (Environment.GetEnvironmentVariable(rootSuffixVariableName) == null)
                            {
                                // the environment variable doesn't work for the first class tested in the test execution,
                                // but .RootSuffix only works for the first class tested. Hence both are needed.
                                Environment.SetEnvironmentVariable(rootSuffixVariableName, "Exp");
                                _visualStudioHostConfiguration.RootSuffix = "Exp";
                            }
                        }
                    }
                }
                return _visualStudioHostConfiguration;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the test should fail on the 
        /// first assert failure or continue executing as far as possible.
        /// The default value is false.
        /// </summary>
        public bool FailTestOnFirstFailure
        {
            get
            {
                return _verifier.AssertOnFirstFailure;
            }
            set
            {
                _verifier.AssertOnFirstFailure = value;
            }
        }

        public IOperations Operations
        {
            get { return _operations; }
        }

        private void FailAction(string message)
        {
            // Log error
            _testLogger.WriteError(message);

            // throw error for xUnit
            throw new InvalidOperationException(message);
        }
    }
}
