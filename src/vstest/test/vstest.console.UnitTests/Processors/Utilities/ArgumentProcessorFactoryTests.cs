// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestPlatform.CommandLine.Processors;
using Microsoft.VisualStudio.TestPlatform.Common.Interfaces;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using vstest.console.UnitTests.Processors;

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
        var ex = Assert.ThrowsExactly<ArgumentException>(() => factory.CreateArgumentProcessor(command, null!));
        Assert.Contains("Cannot be null or empty", ex.Message);
    }

    [TestMethod]
    public void CreateArgumentProcessorShouldReturnNullIfInvalidCommandIsPassed()
    {
        var command = "/-";

        ArgumentProcessorFactory factory = ArgumentProcessorFactory.Create();

        IArgumentProcessor result = factory.CreateArgumentProcessor(command, [""])!;

        Assert.IsNull(result);
    }

    [TestMethod]
    public void CreateArgumentProcessorShouldReturnCliRunSettingsArgumentProcessorIfCommandIsGiven()
    {
        var command = "--";

        ArgumentProcessorFactory factory = ArgumentProcessorFactory.Create();

        IArgumentProcessor result = factory.CreateArgumentProcessor(command, [""])!;

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
            // Processors declare different constructor shapes: most now take a CommandLineOptions first,
            // optionally followed by an IRunSettingsProvider and/or IRunSettingsHelper; a few legacy ones take
            // only a run settings dependency, and the rest are parameterless. Pick the matching one.
            var commandLineOptions = CommandLineOptions.Instance;
            var runSettingsProvider = new TestableRunSettingsProvider();
            var runSettingsHelper = new RunSettingsHelper();

            var instance = (processor.GetConstructor([typeof(CommandLineOptions), typeof(IRunSettingsProvider), typeof(IRunSettingsHelper)]) is { } optionsProviderHelperCtor
                ? optionsProviderHelperCtor.Invoke([commandLineOptions, runSettingsProvider, runSettingsHelper])
                : processor.GetConstructor([typeof(CommandLineOptions), typeof(IRunSettingsProvider)]) is { } optionsProviderCtor
                    ? optionsProviderCtor.Invoke([commandLineOptions, runSettingsProvider])
                    : processor.GetConstructor([typeof(CommandLineOptions), typeof(IRunSettingsHelper)]) is { } optionsHelperCtor
                        ? optionsHelperCtor.Invoke([commandLineOptions, runSettingsHelper])
                        : processor.GetConstructor([typeof(CommandLineOptions)]) is { } optionsCtor
                            ? optionsCtor.Invoke([commandLineOptions])
                            : processor.GetConstructor([typeof(IRunSettingsProvider), typeof(IRunSettingsHelper)]) is { } providerAndHelperCtor
                                ? providerAndHelperCtor.Invoke([runSettingsProvider, runSettingsHelper])
                                : processor.GetConstructor([typeof(IRunSettingsProvider)]) is { } providerCtor
                                    ? providerCtor.Invoke([runSettingsProvider])
                                    : processor.GetConstructor([typeof(IRunSettingsHelper)]) is { } helperCtor
                                        ? helperCtor.Invoke([runSettingsHelper])
                                        : Activator.CreateInstance(processor)) as IArgumentProcessor;
            Assert.IsNotNull(instance, $"Unable to instantiate processor: {processor}");

            var specialProcessor = instance.Metadata.Value.IsSpecialCommand;
            if ((specialCommandFilter && specialProcessor) || (!specialCommandFilter && !specialProcessor))
            {
                yield return instance;
            }
        }
    }
}
