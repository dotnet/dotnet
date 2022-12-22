## Seamlessly running JavaScript tests with VSTest
VSTest provides tremendous extensibility for running tests from many different frameworks. A single test runner to rule them all. To name just a few - NUnit, xUnit, MSTest. Be it running in Visual Studio, CLI or using VSTest task in Azure Pipelines, this extensibility allows you to run any test and get a consistent experience. 

With services becoming more common and UX evolving consistently for fluid and rich experiences, there is a need for JavaScript testing. JavaScript has a plethora of test frameworks, each with it's own runner. Not all of them plug-in seamlessly across the IDE, CLI and a CI pipeline, the way VSTest does. 
VSTest does have a JavaScript test adapter called Chutzpah, which leverages the extensibility for a consistent experience. However,  Chutzpah limits the available runtime to PhantomJS. PhantomJS is no longer being actively maintained and also limits you to using ECMAScript 5.

#### JSTest to the rescue  
JSTestAdapter is an open source test adapter extension for VSTest for running, well you guessed it, any kind of JavaScript tests. The adapter has been built with extensibility in mind to allow plugging in a variety of JavaScript test frameworks and environments.

The adapter currently supports nodejs as its runtime environment and three most popular test runners - mocha, jasmine and jest. 
The JSTestAdapter allows you to leverage the VSTest capabilities for a seamless test execution experience across the VS IDE, CLI and the VSTest task in Azure Pipelines.

JSTest's deep integration with VSTest and Azure Pipelines enables us to leverage the distribution capability for all three of the runners. It also provides the *MSTest style* capability to upload attachments from tests with [jstestcontext](https://github.com/karanjitsingh/jstestcontext) extension. This is especially helpful for UI test failures, by simply uploading the screenshot of the UI we can figure out what went wrong in the Pipelines itself.

####  Running JavaScript tests using the vstest.console.exe CLI

Let's take an example of a basic calculator test.

```javascript
// Calculator.js
function add(a, b) {
    return a + b;
}

module.exports = {
    add
}
```

```javascript
// CalculatorTest.js
const assert = require("assert");
const calculator = require("./calculator.js")

describe("Calculator tests", () => {
    
    it("This test will fail", () => {
        assert.equal(calculator.add(1, 2), 2);
    });

    it("This test will pass", () => {
        assert.equal(calculator.add(1, 2), 3);
    });

});
```

And now to setup `jasmine` and `jstestadapter` npm packages.

```powershell
> npm install jasmine
> npm install jstestadapter
```

Running this test is as simple as passing the path to the node module and the path to the test file as arguments.

```powershell
> vstest.console.exe --TestAdapterPath:.\node_modules\jstestadapter .\calculatortest.js
```

![Imgur](https://i.imgur.com/bwrEbDJ.png)

Since the default test framework for JSTest is Jasmine, let's try to run the tests with Mocha. 

```powershell
> vstest.console.exe --TestAdapterPath:.\node_modules\jstestadapter .\calculatortest.js -- JSTest.TestFramework=mocha
```

`... -- JSTest.TestFramework=mocha` is how run-settings are provided to VSTest. Run setting configurations can also be defined in an xml and passed as a CLI argument to vstest.console.exe, `--Settings:runsettings.xml`. Run settings can also contain configurations specific to the test framework/runner in question. Now let's try this with Jest, since Jest uses file patterns to check for test files, package.json is used as the test file container for running tests through Jest.

```powershell
vstest.console.exe --Settings:RunSettings.xml --TestAdapterPath:.\node_modules\jstestadapter .\package.json
```

With RunSettings.xml:
```xml
<RunSettings>
    <JSTest>
        <TestFramework>jest</TestFramework>
        <TestFrameworkConfigJson>{
            "collectCoverage": true,
            "verbose": true
        }</TestFrameworkConfigJson>
    </JSTest>
</RunSettings>
```

You can find more options for Jest at https://jestjs.io/docs/en/configuration.html

#### Running JavaScript tests in Azure Pipelines with VSTest task

We can use the VSTest task in Azure Pipelines to run JavaScript tests with VSTest and JSTest. 

1. First, let's go ahead and create a pipeline and add VSTest task to it.

    ![Imgur](https://i.imgur.com/wVinSKh.png)

2. Now that we've added the task let's set it up to run JavaScript tests with `jstestadapter`, we fill in the test pattern for the test files and make sure to exclude js files from node_modules.

   ![Imgur](https://i.imgur.com/ONBBy1r.png)

3. Finally, we configure the task with `RunSettings.xml` and a path to the `jstestadapter` node module as the path to the tests adapter path

   ![Imgur](https://i.imgur.com/cKoZIJl.png)

Here's the yaml for the same:

```yml
- task: VSTest@2
  displayName: 'VsTest - testAssemblies'
  inputs:
    testAssemblyVer2: |
     **\*Tests.js
     !**\node_modules\**\*.js
    runSettingsFile: RunSettings.xml
    pathtoCustomTestAdapters: '$(System.DefaultWorkingDirectory)\node_modules\jstestadapter'
```

#### Distributing JavaScript tests in Azure Pipelines with VSTest task

To run these tests with distribution across multiple agents:

1. First we need to configure the job for multi-agent parallelism.

   ![Imgur](https://i.imgur.com/B63yBoF.png)

2. Next, we go ahead and configure distribution in the task. In this particular scenario, default values for distribution remains same. So as long as we've enabled multi-agent parallelism in the pipeline we do not need to change any property for Advanced Execution in VSTest. Hence the yaml for the task remains the same.

   ![Imgur](https://i.imgur.com/Uv1IBRC.png)

*Since multi-configuration and multi-agent job options are not exported to YAML. They can be configured by following this guide, https://docs.microsoft.com/en-us/azure/devops/pipelines/process/phases?view=azure-devops&tabs=yaml.*

#### Limitations

While the adapter was designed to be cross-platform there are still a few issues with JSTest when trying to run it with .NET core build for VSTest. Along with not smoothly being able to run on Linux (there are a few workarounds to get it to work), even though the architecture is designed to be runtime and test framework abstracted but currently there is no public API to leverage that extensible design.

#### Conclusion

JSTest provides a fantastic way of running any kinds of tests through node in a pipeline setting like Azure Devops Pipeline. With support for the latest trend in JavaScript, Jest, testing and node runtime, it makes it possible to run about any kind of tests.

The JavaScript tests for the adapter itself are run through the adapter, checkout the [_testception_](https://dev.azure.com/karanjitsingh/JSTestAdapter/_build?definitionId=4) in action for yourself.

###### Source: [JSTestAdapter Repo](https://github.com/karanjitsingh/JSTestAdapter/)
