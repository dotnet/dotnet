using System;
using System.Collections.Generic;
using System.Text;
using AwesomeAssertions;
using Microsoft.Arcade.Test.Common;
using Microsoft.DotNet.Build.Tasks.Workloads.Wix;
using Xunit;

namespace Microsoft.DotNet.Build.Tasks.Workloads.Tests
{
    public class ToolsetTests : TestBase
    {
        [WindowsOnlyFact]
        public void ValidateWixCli()
        {
            //var tooltask = new WixToolTask(new MockBuildEngine(), @"C:\Wix5");
            //tooltask.AddPreprocessorDefinition("X", "Y");
            //tooltask.AddExtension(@"C:\a\folder with a space\FooExtension.dll");
            //var commandline = tooltask.GetCommandLine();

            //// Unlike Candle in v3, preprocessor definitions now require a space, so instead of "-dX=Y" we now
            //// pass "-d X=Y".
            //commandline.Should().Contain("-d X=Y");
            //commandline.Should().Contain(@"-ext ""C:\a\folder with a space\FooExtension.dll""");

            //// We should always pass the bcgg option to generate v3 backwards compatible GUIDs.
            //commandline.Should().Contain("-bcgg");

            //// Primary command to execute
            //commandline.Should().Contain("build");

            //// Default architecture should be x86
            //commandline.Should().Contain("-arch x86");
        }
    }
}
