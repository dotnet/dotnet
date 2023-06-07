// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestPlatform.CommandLine.Processors;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Microsoft.VisualStudio.TestPlatform.CommandLine.UnitTests.Processors.Utilities;

[TestClass]
public class ArgumentProcessorFactoryTests
{
    [TestMethod]
    public void CreateArgumentProcessorIsTreatingNonArgumentAsSource()
    {
        string argument = "--NonArgumet:Dummy";

        ArgumentProcessorFactory factory = ArgumentProcessorFactory.Create();

        IArgumentProcessor result = factory.CreateArgumentProcessor(argument)!;

        Assert.AreEqual(typeof(TestSourceArgumentProcessor), result.GetType());
    }

    [TestMethod]
    public void CreateArgumentProcessorIsTreatingNonArgumentAsSourceEvenItIsStratingFromForwardSlash()
    {
        string argument = "/foo/foo.dll";

        ArgumentProcessorFactory factory = ArgumentProcessorFactory.Create();

        IArgumentProcessor result = factory.CreateArgumentProcessor(argument)!;

        Assert.AreEqual(typeof(TestSourceArgumentProcessor), result.GetType());
    }

    [TestMethod]
    public void CreateArgumentProcessorShouldReturnPlatformArgumentProcessorWhenArgumentIsPlatform()
    {
        string argument = "/Platform:x64";

        ArgumentProcessorFactory factory = ArgumentProcessorFactory.Create();

        IArgumentProcessor result = factory.CreateArgumentProcessor(argument)!;

        Assert.AreEqual(typeof(PlatformArgumentProcessor), result.GetType());
    }

    [TestMethod]
    public void CreateArgumentProcessorShouldReturnPlatformArgumentProcessorWhenArgumentIsPlatformInXplat()
    {
        string argument = "--Platform:x64";

        ArgumentProcessorFactory factory = ArgumentProcessorFactory.Create();

        IArgumentProcessor result = factory.CreateArgumentProcessor(argument)!;

        Assert.AreEqual(typeof(PlatformArgumentProcessor), result.GetType());
    }

    [TestMethod]
    public void CreateArgumentProcessorShouldReturnThrowExceptionIfArgumentsIsNull()
    {
        var command = "--";

        ArgumentProcessorFactory factory = ArgumentProcessorFactory.Create();
        Action action = () => factory.CreateArgumentProcessor(command, null!);

        ExceptionUtilities.ThrowsException<ArgumentException>(
            action,
            "Cannot be null or empty", "argument");
    }

    [TestMethod]
    public void CreateArgumentProcessorShouldReturnNullIfInvalidCommandIsPassed()
    {
        var command = "/-";

        ArgumentProcessorFactory factory = ArgumentProcessorFactory.Create();

        IArgumentProcessor result = factory.CreateArgumentProcessor(command, new string[] { "" })!;

        Assert.IsNull(result);
    }

    [TestMethod]
    public void CreateArgumentProcessorShouldReturnCliRunSettingsArgumentProcessorIfCommandIsGiven()
    {
        var command = "--";

        ArgumentProcessorFactory factory = ArgumentProcessorFactory.Create();

        IArgumentProcessor result = factory.CreateArgumentProcessor(command, new string[] { "" })!;

        Assert.AreEqual(typeof(CliRunSettingsArgumentProcessor), result.GetType());
    }

    [TestMethod]
    public void BuildCommandMapsForProcessorWithIsSpecialCommandSetAddsProcessorToSpecialMap()
    {
        var specialCommands = GetArgumentProcessors(specialCommandFilter: true);

        List<string> xplatspecialCommandNames = new();
        List<string> specialCommandNames = new();

        // for each command add there xplat version
        foreach (var specialCommand in specialCommands)
        {
            specialCommandNames.Add(specialCommand.Metadata.Value.CommandName);
            if (!specialCommand.Metadata.Value.AlwaysExecute)
            {
                xplatspecialCommandNames.Add(string.Concat("--", specialCommand.Metadata.Value.CommandName.Remove(0, 1)));
            }
        }
        var factory = ArgumentProcessorFactory.Create();

        CollectionAssert.AreEquivalent(
            specialCommandNames.Concat(xplatspecialCommandNames).ToList(),
            factory.SpecialCommandToProcessorMap.Keys.ToList());
    }

    [TestMethod]
    public void BuildCommandMapsForMultipleProcessorAddsProcessorToAppropriateMaps()
    {
        var commandProcessors = GetArgumentProcessors(specialCommandFilter: false);
        var commands = commandProcessors.Select(a => a.Metadata.Value.CommandName);
        List<string> xplatCommandName = new();

        // for each command add there xplat version
        foreach (string name in commands)
        {
            xplatCommandName.Add(string.Concat("--", name.Remove(0, 1)));
        }

        var shortCommands = commandProcessors.Where(a => !string.IsNullOrEmpty(a.Metadata.Value.ShortCommandName))
            .Select(a => a.Metadata.Value.ShortCommandName);

        List<string> xplatShortCommandName = new();

        // for each short command add there xplat version
        foreach (var name in shortCommands)
        {
            xplatShortCommandName.Add(name!.Replace('/', '-'));
        }

        Mock<IFeatureFlag> featureFlag = new();
        featureFlag.Setup(x => x.IsSet(It.IsAny<string>())).Returns(false);
        ArgumentProcessorFactory factory = ArgumentProcessorFactory.Create(featureFlag.Object);

        // Expect command processors to contain both long and short commands.
        CollectionAssert.AreEquivalent(
            commands.Concat(xplatCommandName).Concat(shortCommands).Concat(xplatShortCommandName).ToList(),
            factory.CommandToProcessorMap.Keys.ToList());
    }

    private static IEnumerable<IArgumentProcessor> GetArgumentProcessors(bool specialCommandFilter)
    {
        var allProcessors = typeof(ArgumentProcessorFactory)
            .Assembly.GetTypes()
            .Where(t => !t.IsAbstract && !t.Name.Equals("IArgumentProcessor") && typeof(IArgumentProcessor).IsAssignableFrom(t));

        foreach (var processor in allProcessors)
        {
            var instance = Activator.CreateInstance(processor) as IArgumentProcessor;
            Assert.IsNotNull(instance, "Unable to instantiate processor: {0}", processor);

            var specialProcessor = instance.Metadata.Value.IsSpecialCommand;
            if ((specialCommandFilter && specialProcessor) || (!specialCommandFilter && !specialProcessor))
            {
                yield return instance;
            }
        }
    }
}
