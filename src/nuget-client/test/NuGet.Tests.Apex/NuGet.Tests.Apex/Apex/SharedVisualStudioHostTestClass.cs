// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using FluentAssertions;
using Microsoft.Test.Apex.VisualStudio;
using Microsoft.Test.Apex.VisualStudio.Solution;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NuGet.Tests.Apex
{
    [TestClass]
    public abstract class SharedVisualStudioHostTestClass : ApexBaseTestClass
    {
        private static readonly IVisualStudioHostFixtureFactory _contextFixtureFactory = new VisualStudioHostFixtureFactory();
        private readonly Lazy<VisualStudioHostFixture> _hostFixture;
        private NuGetConsoleTestExtension _console;
        private string _packageManagerOutputWindowText;

        public const int DefaultTimeout = 5 * 60 * 1000; // 5 minutes

        protected SharedVisualStudioHostTestClass()
        {
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
            Logger.WriteMessage("GetConsole");
            VisualStudio.ClearWindows();
            NuGetApexTestService nugetTestService = GetNuGetTestService();

            Logger.WriteMessage("EnsurePackageManagerConsoleIsOpen");
            nugetTestService.EnsurePackageManagerConsoleIsOpen().Should().BeTrue("Console was opened");

            Logger.WriteMessage("GetPackageManagerConsole");
            _console = nugetTestService.GetPackageManagerConsole(project.Name);

            // This is not a magic number.
            // It is intended to eliminate unexpected hard line breaks in PMC output which might foil validation,
            // but not so large as to create memory problems.
            _console.SetConsoleWidth(consoleWidth: 1024);

            nugetTestService.WaitForAutoRestore();

            Logger.WriteMessage("GetConsole complete");


            return _console;
        }

        public override void Dispose()
        {
            if (_console != null)
            {
                string text = _console.GetText();

                Logger.WriteMessage($"Package Manager Console contents:  {text}");
            }

            _packageManagerOutputWindowText = _packageManagerOutputWindowText ?? GetPackageManagerOutputWindowPaneText();

            Logger.WriteMessage($"Package Manager Output Window Pane contents:  {_packageManagerOutputWindowText}");

            base.Dispose();
        }

        internal string GetPackageManagerOutputWindowPaneText()
        {
            return string.Join(Environment.NewLine, VisualStudio.GetOutputWindowsLines());
        }
    }
}
