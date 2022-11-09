// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if NETFRAMEWORK

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

using Microsoft.VisualStudio.TestPlatform.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace testhost.UnitTests;

[TestClass]
public class AppDomainEngineInvokerTests
{
    private const string XmlNamespace = "urn:schemas-microsoft-com:asm.v1";
    private const string TestHostConfigXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
  <startup useLegacyV2RuntimeActivationPolicy=""true"">
    <supportedRuntime version=""v4.0"" sku="".NETFramework,Version=v4.0""/>
  </startup>
  <runtime>
    <legacyUnhandledExceptionPolicy enabled=""1""/>
     <assemblyBinding xmlns=""urn:schemas-microsoft-com:asm.v1"">
      <probing privatePath=""Extensions""/>
      <dependentAssembly>
        <assemblyIdentity name=""Microsoft.VisualStudio.TestPlatform.ObjectModel"" publicKeyToken=""b03f5f7f11d50a3a"" culture=""neutral""/>
        <bindingRedirect oldVersion=""11.0.0.0-14.0.0.0""  newVersion=""15.0.0.0""/>
      </dependentAssembly>
    </assemblyBinding>
  </runtime>
  <system.diagnostics>
    <switches>
      <add name=""TpTraceLevel"" value=""0""/>
    </switches>
  </system.diagnostics>
  <appSettings>
    <add key=""TestProjectRetargetTo35Allowed"" value=""true""/>
  </appSettings>
</configuration>
    ";
    [TestMethod]
    public void AppDomainEngineInvokerShouldCreateNewAppDomain()
    {
        var tempFile = Path.GetTempFileName();
        var appDomainInvoker = new TestableEngineInvoker(tempFile);

        Assert.IsNotNull(appDomainInvoker.NewAppDomain, "New AppDomain must be created.");
        Assert.IsNotNull(appDomainInvoker.ActualInvoker, "Invoker must be created.");
        Assert.AreNotEqual(AppDomain.CurrentDomain.FriendlyName, appDomainInvoker.NewAppDomain.FriendlyName,
            "New AppDomain must be different from default one.");
    }

    [TestMethod]
    public void AppDomainEngineInvokerShouldInvokeEngineInNewDomainAndUseTestHostConfigFile()
    {
        var tempFile = Path.GetTempFileName();
        var appDomainInvoker = new TestableEngineInvoker(tempFile);

        var newAppDomain = appDomainInvoker.NewAppDomain;

        Assert.IsNotNull(newAppDomain, "New AppDomain must be created.");
        Assert.IsNotNull(appDomainInvoker.ActualInvoker, "Invoker must be created.");
        Assert.AreNotEqual(AppDomain.CurrentDomain.FriendlyName,
            (appDomainInvoker.ActualInvoker as MockEngineInvoker)?.DomainFriendlyName,
            "Engine must be invoked in new domain.");

        Assert.AreEqual(newAppDomain.SetupInformation.ConfigurationFile, AppDomain.CurrentDomain.SetupInformation.ConfigurationFile,
            "TestHost config file must be used in the absence of user config file.");
    }

    [TestMethod]
    public void AppDomainEngineInvokerShouldUseTestHostStartupConfigAndRuntimeAfterMerging()
    {
        string appConfig = @"<?xml version=""1.0"" encoding=""utf-8""?>
                                <configuration>
                                    <startup>
                                        <supportedRuntime version=""v4.0"" sku="".NETFramework,Version=v4.5.2"" />
                                    </startup>
                                </configuration>";

        var doc = TestableEngineInvoker.MergeConfigXmls(appConfig, TestHostConfigXml);

        var startupElements = doc.Descendants("startup");

        Assert.AreEqual(1, startupElements.Count(), "Merged config must have only one 'startup' element");

        var supportedRuntimeXml = startupElements.First().Descendants("supportedRuntime").FirstOrDefault()?.ToString();
        Assert.AreEqual(@"<supportedRuntime version=""v4.0"" sku="".NETFramework,Version=v4.0"" />", supportedRuntimeXml,
            "TestHost Supported Runtime must be used on merging");

        var runtimeEle = doc.Descendants("runtime").FirstOrDefault();
        Assert.IsNotNull(runtimeEle, "Runtime element must be present");

        var legacyUnhandledEleExpectedXml = @"<legacyUnhandledExceptionPolicy enabled=""1"" />";

        Assert.AreEqual(legacyUnhandledEleExpectedXml, runtimeEle.Descendants("legacyUnhandledExceptionPolicy").First()?.ToString(),
            "legacyUnhandledExceptionPolicy element must be of the TestHost one.");

        Assert.IsFalse(runtimeEle.ToString().Contains("probing"), "Probing element of TestHost must not be present.");

        var assemblyBindingXName = XName.Get("assemblyBinding", XmlNamespace);
        var mergedDocAssemblyBindingNodes = runtimeEle.Descendants(assemblyBindingXName);
        Assert.AreEqual(1, mergedDocAssemblyBindingNodes.Count(), "AssemblyRedirect of TestHost must be present.");

        var dependentAssemblyXName = XName.Get("dependentAssembly", XmlNamespace);
        var dependentAssemblyNodes = mergedDocAssemblyBindingNodes.First().Descendants(dependentAssemblyXName);
        Assert.AreEqual(1, dependentAssemblyNodes.Count(), "AssemblyRedirect of TestHost must be present.");
        Assert.IsTrue(dependentAssemblyNodes.First().ToString().Contains("Microsoft.VisualStudio.TestPlatform.ObjectModel"), "Correct AssemblyRedirect must be present.");

        var diagEle = doc.Descendants("system.diagnostics").FirstOrDefault();
        var appSettingsEle = doc.Descendants("appSettings").FirstOrDefault();

        Assert.IsNull(diagEle, "No Diagnostics element must be present as user config does not have it.");
        Assert.IsNull(appSettingsEle, "No AppSettings element must be present as user config does not have it.");
    }

    [TestMethod]
    public void AppDomainEngineInvokerShouldOnlyMergeAssemblyRedirectionsFromTestHost()
    {
        string appConfig = @"<?xml version=""1.0"" encoding=""utf-8""?>
                                <configuration>
                                  <runtime>
                                     <assemblyBinding xmlns=""urn:schemas-microsoft-com:asm.v1"">
                                      <probing privatePath=""TestProbing"" />
                                      <dependentAssembly>
                                        <assemblyIdentity name=""Microsoft.VisualStudio.UnitTests"" publicKeyToken=""b03f5f7f11d50a3a"" culture=""neutral"" />
                                        <bindingRedirect oldVersion=""1.0.0.0-3.0.0.0""  newVersion=""4.0.0.0"" />
                                      </dependentAssembly>
                                    </assemblyBinding>
                                  </runtime>
                                </configuration>";

        var doc = TestableEngineInvoker.MergeConfigXmls(appConfig, TestHostConfigXml);

        var runtimeEle = doc.Descendants("runtime").FirstOrDefault();

        Assert.AreEqual(0, runtimeEle.Descendants("legacyUnhandledExceptionPolicy").Count(), "legacyUnhandledExceptionPolicy element must NOT be present.");

        var probingXName = XName.Get("probing", XmlNamespace);
        var probingEleNodes = runtimeEle.Descendants(probingXName);
        Assert.AreEqual(1, probingEleNodes.Count(), "Only one Probing element of UserConfig must be present.");
        Assert.AreEqual(@"<probing privatePath=""TestProbing"" xmlns=""urn:schemas-microsoft-com:asm.v1"" />", probingEleNodes.First().ToString(), "Probing element must be correct.");

        var assemblyBindingXName = XName.Get("assemblyBinding", XmlNamespace);
        var mergedDocAssemblyBindingNodes = runtimeEle.Descendants(assemblyBindingXName);
        Assert.AreEqual(1, mergedDocAssemblyBindingNodes.Count(), "AssemblyBinding Ele must be present.");

        var dependentAssemblyXName = XName.Get("dependentAssembly", XmlNamespace);
        var dependentAssemblyNodes = mergedDocAssemblyBindingNodes.First().Descendants(dependentAssemblyXName);
        Assert.AreEqual(2, dependentAssemblyNodes.Count(), "AssemblyBinding of TestHost must be present.");

        Assert.IsTrue(dependentAssemblyNodes.ElementAt(0).ToString().Contains("Microsoft.VisualStudio.UnitTests"), "First AssemblyRedirect must be of UserConfig.");
        Assert.IsTrue(dependentAssemblyNodes.ElementAt(1).ToString().Contains("Microsoft.VisualStudio.TestPlatform.ObjectModel"), "Second AssemblyRedirect must be from TestHost Node.");

        var diagEle = doc.Descendants("system.diagnostics").FirstOrDefault();
        var appSettingsEle = doc.Descendants("appSettings").FirstOrDefault();

        Assert.IsNull(diagEle, "No Diagnostics element must be present as user config does not have it.");
        Assert.IsNull(appSettingsEle, "No AppSettings element must be present as user config does not have it.");
    }

    [TestMethod]
    public void AppDomainEngineInvokerShouldUseDiagAndAppSettingsElementsUnMergedFromUserConfig()
    {
        string appConfig = @"<?xml version=""1.0"" encoding=""utf-8""?>
                                <configuration>
                                  <system.diagnostics>
                                    <switches>
                                      <add name=""SampleSwitch"" value=""1""/>
                                    </switches>
                                  </system.diagnostics>
                                  <appSettings>
                                    <add key=""SampleKey"" value=""SampleValue""/>
                                  </appSettings>
                                </configuration>";

        var doc = TestableEngineInvoker.MergeConfigXmls(appConfig, TestHostConfigXml);

        var diagEle = doc.Descendants("system.diagnostics").FirstOrDefault();
        var appSettingsEle = doc.Descendants("appSettings").FirstOrDefault();

        Assert.IsNotNull(diagEle, "Diagnostics element must be retained from user config.");
        Assert.IsNotNull(appSettingsEle, "AppSettings element must be retained from user config.");

        var diagAddNodes = diagEle.Descendants("add");
        Assert.AreEqual(1, diagAddNodes.Count(), "Only switches from user config should be present.");
        Assert.AreEqual(@"<add name=""SampleSwitch"" value=""1"" />", diagAddNodes.First().ToString(),
            "Correct Switch must be merged.");

        var appSettingsAddNodes = appSettingsEle.Descendants("add");
        Assert.AreEqual(1, appSettingsAddNodes.Count(), "Only switches from user config should be present.");
        Assert.AreEqual(@"<add key=""SampleKey"" value=""SampleValue"" />", appSettingsAddNodes.First().ToString(),
            "Correct Switch must be merged.");
    }

    private class TestableEngineInvoker : AppDomainEngineInvoker<MockEngineInvoker>
    {
        public TestableEngineInvoker(string testSourcePath) : base(testSourcePath)
        {
        }

        public static XDocument MergeConfigXmls(string userConfigText, string testHostConfigText)
        {
            return MergeApplicationConfigFiles(
                XDocument.Load(new MemoryStream(Encoding.UTF8.GetBytes(userConfigText))),
                XDocument.Load(new MemoryStream(Encoding.UTF8.GetBytes(testHostConfigText))));
        }

        public AppDomain NewAppDomain => _appDomain;

        public IEngineInvoker ActualInvoker => _actualInvoker;
    }

    private class MockEngineInvoker : MarshalByRefObject, IEngineInvoker
    {
        public string DomainFriendlyName { get; private set; } = "";

        public void Invoke(IDictionary<string, string?> argsDictionary)
        {
            DomainFriendlyName = AppDomain.CurrentDomain.FriendlyName;
        }
    }
}
#endif
