// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using FluentAssertions;
using Microsoft.Test.Apex;
using Microsoft.Test.Apex.VisualStudio;
using Microsoft.Test.Apex.VisualStudio.Solution;
using Test.Utility;
using Xunit;
using Xunit.Abstractions;

namespace NuGet.Tests.Apex
{
    [CollectionDefinition("SharedVSHost")]
    public sealed class SharedVisualStudioHostTestCollectionDefinition : ICollectionFixture<VisualStudioHostFixtureFactory>
    {
        private SharedVisualStudioHostTestCollectionDefinition()
        {
            throw new InvalidOperationException("SharedVisualStudioHostTestCollectionDefinition only exists for metadata, it should never be constructed.");
        }
    }

    [Collection("SharedVSHost")]
    public abstract class SharedVisualStudioHostTestClass : ApexBaseTestClass
    {
        private readonly IVisualStudioHostFixtureFactory _contextFixtureFactory;
        private readonly Lazy<VisualStudioHostFixture> _hostFixture;
        private NuGetConsoleTestExtension _console;
        private string _packageManagerOutputWindowText;

        /// <summary>
        /// ITestOutputHelper wrapper
        /// </summary>
        public XunitLogger XunitLogger { get; }

        protected SharedVisualStudioHostTestClass(IVisualStudioHostFixtureFactory contextFixtureFactory, ITestOutputHelper output)
        {
            XunitLogger = new XunitLogger(output);
            _contextFixtureFactory = contextFixtureFactory;

            _hostFixture = new Lazy<VisualStudioHostFixture>(() =>
            {
                return _contextFixtureFactory.GetVisualStudioHostFixture();
            });
        }

        public override VisualStudioHost VisualStudio => _hostFixture.Value.VisualStudio;

        public override TService GetApexService<TService>()
        {
            return _hostFixture.Value.Operations.Get<TService>();
        }

        public override void EnsureVisualStudioHost()
        {
            _hostFixture.Value.EnsureHost();
        }

        public override void CloseVisualStudioHost()
        {
            _packageManagerOutputWindowText = GetPackageManagerOutputWindowPaneText();

            VisualStudio.Stop();
        }

        protected NuGetConsoleTestExtension GetConsole(ProjectTestExtension project)
        {
            XunitLogger.LogInformation("GetConsole");
            VisualStudio.ClearWindows();
            NuGetApexTestService nugetTestService = GetNuGetTestService();

            XunitLogger.LogInformation("EnsurePackageManagerConsoleIsOpen");
            nugetTestService.EnsurePackageManagerConsoleIsOpen().Should().BeTrue("Console was opened");

            XunitLogger.LogInformation("GetPackageManagerConsole");
            _console = nugetTestService.GetPackageManagerConsole(project.Name);

            // This is not a magic number.
            // It is intended to eliminate unexpected hard line breaks in PMC output which might foil validation,
            // but not so large as to create memory problems.
            _console.SetConsoleWidth(consoleWidth: 1024);

            nugetTestService.WaitForAutoRestore();

            XunitLogger.LogInformation("GetConsole complete");

            return _console;
        }

        public IOperations Operations => _hostFixture.Value.Operations;

        public override void Dispose()
        {
            if (_console != null)
            {
                string text = _console.GetText();

                XunitLogger.LogInformation($"Package Manager Console contents:  {text}");
            }

            _packageManagerOutputWindowText = _packageManagerOutputWindowText ?? GetPackageManagerOutputWindowPaneText();

            XunitLogger.LogInformation($"Package Manager Output Window Pane contents:  {_packageManagerOutputWindowText}");

            base.Dispose();
        }

        internal string GetPackageManagerOutputWindowPaneText()
        {
            return string.Join(Environment.NewLine, VisualStudio.GetOutputWindowsLines());
        }
    }
}
