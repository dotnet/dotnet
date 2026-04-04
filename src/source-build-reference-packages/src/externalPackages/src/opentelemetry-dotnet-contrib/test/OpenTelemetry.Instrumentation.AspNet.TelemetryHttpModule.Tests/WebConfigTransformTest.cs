// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Xml.Linq;
using Microsoft.Web.XmlTransform;
using Xunit;

namespace OpenTelemetry.Instrumentation.AspNet.Tests;

public class WebConfigTransformTest
{
    private const string InstallConfigTransformationResourceName = "OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule.Tests.Resources.web.config.install.xdt";
    private const string UninstallConfigTransformationResourceName = "OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule.Tests.Resources.web.config.uninstall.xdt";

    [Fact]
    public void VerifyInstallationToBasicWebConfig()
    {
        const string originalWebConfigContent = @"
                <configuration>
                    <system.web>
                        <httpModules />
                    </system.web>
                    <system.webServer>
                        <modules />
                    </system.webServer>
                </configuration>";

        const string expectedWebConfigContent = @"
                <configuration>
                    <system.web>
                        <httpModules>
                            <add name=""TelemetryHttpModule"" type=""OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule, OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule"" />
                        </httpModules >
                    </system.web>
                    <system.webServer>
                        <modules>
                           <remove name=""TelemetryHttpModule"" />
                           <add name=""TelemetryHttpModule"" type=""OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule, OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule"" preCondition=""managedHandler""/>
                        </modules>
                        <validation validateIntegratedModeConfiguration=""false"" />
                    </system.webServer>
                </configuration>";

        var transformedWebConfig = this.ApplyInstallTransformation(originalWebConfigContent, InstallConfigTransformationResourceName);
        this.VerifyTransformation(expectedWebConfigContent, transformedWebConfig);
    }

    [Fact]
    public void VerifyUpdateWithTypeRenamingWebConfig()
    {
        const string originalWebConfigContent = @"
                <configuration>
                    <system.web>
                        <httpModules>
                            <add name=""TelemetryHttpModuleSomeOldName"" type=""OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule, OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule"" />
                        </httpModules>
                    </system.web>
                    <system.webServer>
                        <modules>
                           <add name=""TelemetryHttpModuleSomeOldName"" type=""OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule, OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule"" preCondition=""managedHandler""/>
                        </modules>
                    </system.webServer>
                </configuration>";

        const string expectedWebConfigContent = @"
                <configuration>
                    <system.web>
                        <httpModules>
                            <add name=""TelemetryHttpModule"" type=""OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule, OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule"" />
                        </httpModules>
                    </system.web>
                    <system.webServer>
                        <modules>
                           <remove name=""TelemetryHttpModule"" />
                           <add name=""TelemetryHttpModule"" type=""OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule, OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule"" preCondition=""managedHandler""/>
                        </modules>
                        <validation validateIntegratedModeConfiguration=""false"" />
                    </system.webServer>
                </configuration>";

        var transformedWebConfig = this.ApplyInstallTransformation(originalWebConfigContent, InstallConfigTransformationResourceName);
        this.VerifyTransformation(expectedWebConfigContent, transformedWebConfig);
    }

    [Fact]
    public void VerifyUpdateNewerVersionWebConfig()
    {
        const string originalWebConfigContent = @"
                <configuration>
                    <system.web>
                        <httpModules>
                            <add name=""TelemetryHttpModule"" type=""OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule, OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule"" />
                        </httpModules >
                    </system.web>
                    <system.webServer>
                        <modules>
                           <add name=""TelemetryHttpModule"" type=""Microsoft.AspNet.TelemetryCorrelation, Version=1.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"" preCondition=""managedHandler""/>
                        </modules>
                    </system.webServer>
                </configuration>";

        const string expectedWebConfigContent = @"
                <configuration>
                    <system.web>
                        <httpModules>
                            <add name=""TelemetryHttpModule"" type=""OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule, OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule"" />
                        </httpModules >
                    </system.web>
                    <system.webServer>
                        <modules>
                           <add name=""TelemetryHttpModule"" type=""Microsoft.AspNet.TelemetryCorrelation, Version=1.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"" preCondition=""managedHandler""/>
                           <remove name=""TelemetryHttpModule"" />
                           <add name=""TelemetryHttpModule"" type=""OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule, OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule"" preCondition=""managedHandler""/>
                        </modules>
                        <validation validateIntegratedModeConfiguration=""false"" />
                    </system.webServer>
                </configuration>";

        var transformedWebConfig = this.ApplyInstallTransformation(originalWebConfigContent, InstallConfigTransformationResourceName);
        this.VerifyTransformation(expectedWebConfigContent, transformedWebConfig);
    }

    [Fact]
    public void VerifyUpdateWithIntegratedModeWebConfig()
    {
        const string originalWebConfigContent = @"
                <configuration>
                    <system.web>
                        <httpModules>
                            <add name=""TelemetryHttpModule"" type=""OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule, OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule"" />
                        </httpModules >
                    </system.web>
                    <system.webServer>
                        <validation validateIntegratedModeConfiguration=""false"" />
                        <modules>
                           <add name=""TelemetryHttpModule"" type=""OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule, OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule"" preCondition=""integratedMode,managedHandler""/>
                        </modules>
                    </system.webServer>
                </configuration>";

        const string expectedWebConfigContent = @"
                <configuration>
                    <system.web>
                        <httpModules>
                            <add name=""TelemetryHttpModule"" type=""OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule, OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule"" />
                        </httpModules >
                    </system.web>
                    <system.webServer>
                        <validation validateIntegratedModeConfiguration=""false"" />
                        <modules>
                           <remove name=""TelemetryHttpModule"" />
                           <add name=""TelemetryHttpModule"" type=""OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule, OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule"" preCondition=""managedHandler""/>
                        </modules>
                    </system.webServer>
                </configuration>";

        var transformedWebConfig = this.ApplyInstallTransformation(originalWebConfigContent, InstallConfigTransformationResourceName);
        this.VerifyTransformation(expectedWebConfigContent, transformedWebConfig);
    }

    [Fact]
    public void VerifyUninstallationWithBasicWebConfig()
    {
        const string originalWebConfigContent = @"
                <configuration>
                    <system.web>
                        <httpModules>
                            <add name=""TelemetryHttpModule"" type=""OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule, OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule"" />
                        </httpModules >
                    </system.web>
                    <system.webServer>
                        <modules>
                           <remove name=""TelemetryHttpModule"" />
                           <add name=""TelemetryHttpModule"" type=""OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule, OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule"" preCondition=""integratedMode,managedHandler""/>
                        </modules>
                    </system.webServer>
                </configuration>";

        const string expectedWebConfigContent = @"
                <configuration>
                    <system.web>
                        <httpModules></httpModules>
                    </system.web>
                    <system.webServer>
                        <modules></modules>
                    </system.webServer>
                </configuration>";

        var transformedWebConfig = this.ApplyUninstallTransformation(originalWebConfigContent, UninstallConfigTransformationResourceName);
        this.VerifyTransformation(expectedWebConfigContent, transformedWebConfig);
    }

    [Fact]
    public void VerifyUninstallWithIntegratedPrecondition()
    {
        const string originalWebConfigContent = @"
                <configuration>
                    <system.web>
                        <httpModules>
                            <add name=""TelemetryHttpModule"" type=""OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule, OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule"" />
                        </httpModules>
                    </system.web>
                    <system.webServer>
                        <modules>
                           <remove name=""TelemetryHttpModule"" />
                           <add name=""TelemetryHttpModule"" type=""OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule, OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule"" preCondition=""integratedMode,managedHandler""/>
                        </modules>
                    </system.webServer>
                </configuration>";

        const string expectedWebConfigContent = @"
                <configuration>
                    <system.web>
                        <httpModules></httpModules>
                    </system.web>
                    <system.webServer>
                        <modules></modules>
                    </system.webServer>
                </configuration>";

        var transformedWebConfig = this.ApplyUninstallTransformation(originalWebConfigContent, UninstallConfigTransformationResourceName);
        this.VerifyTransformation(expectedWebConfigContent, transformedWebConfig);
    }

    [Fact]
    public void VerifyUninstallationWithUserModules()
    {
        const string originalWebConfigContent = @"
                <configuration>
                    <system.web>
                        <httpModules>
                            <add name=""UserModule"" type=""UserNamespace.WebModuleFoo""/>
                            <add name=""TelemetryHttpModule"" type=""OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule, OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule"" />
                        </httpModules>
                    </system.web>
                    <system.webServer>
                        <modules runAllManagedModulesForAllRequests=""true"">
                           <remove name=""TelemetryHttpModule"" />
                           <add name=""UserModule"" type=""UserNamespace.WebModuleFoo""/>
                           <add name=""TelemetryHttpModule"" type=""OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule, OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule"" preCondition=""managedHandler""/>
                        </modules>
                    </system.webServer>
                </configuration>";

        const string expectedWebConfigContent = @"
                <configuration>
                    <system.web>
                        <httpModules>
                            <add name=""UserModule"" type=""UserNamespace.WebModuleFoo""/>
                        </httpModules >
                    </system.web>
                    <system.webServer>
                        <modules runAllManagedModulesForAllRequests=""true"">
                           <add name=""UserModule"" type=""UserNamespace.WebModuleFoo""/>
                        </modules>
                    </system.webServer>
                </configuration>";

        var transformedWebConfig = this.ApplyUninstallTransformation(originalWebConfigContent, UninstallConfigTransformationResourceName);
        this.VerifyTransformation(expectedWebConfigContent, transformedWebConfig);
    }

    [Fact]
    public void VerifyInstallationToWebConfigWithUserModules()
    {
        const string originalWebConfigContent = @"
                <configuration>
                    <system.web>
                        <httpModules>
                            <add name=""UserModule"" type=""UserNamespace.WebModuleFoo""/>
                        </httpModules>
                    </system.web>
                    <system.webServer>
                        <modules runAllManagedModulesForAllRequests=""true"">
                           <add name=""UserModule"" type=""UserNamespace.WebModuleFoo""/>
                        </modules>
                    </system.webServer>
                </configuration>";

        const string expectedWebConfigContent = @"
                <configuration>
                    <system.web>
                        <httpModules>
                            <add name=""UserModule"" type=""UserNamespace.WebModuleFoo""/>
                            <add name=""TelemetryHttpModule"" type=""OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule, OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule"" />
                        </httpModules>
                    </system.web>
                    <system.webServer>
                        <modules runAllManagedModulesForAllRequests=""true"">
                           <add name=""UserModule"" type=""UserNamespace.WebModuleFoo""/>
                           <remove name=""TelemetryHttpModule"" />
                           <add name=""TelemetryHttpModule"" type=""OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule, OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule"" preCondition=""managedHandler""/>
                        </modules>
                        <validation validateIntegratedModeConfiguration=""false"" />
                    </system.webServer>
                </configuration>";

        var transformedWebConfig = this.ApplyInstallTransformation(originalWebConfigContent, InstallConfigTransformationResourceName);
        this.VerifyTransformation(expectedWebConfigContent, transformedWebConfig);
    }

    [Fact]
    public void VerifyInstallationToEmptyWebConfig()
    {
        const string originalWebConfigContent = @"<configuration/>";

        const string expectedWebConfigContent = @"
                <configuration>
                    <system.web>
                        <httpModules>
                            <add name=""TelemetryHttpModule"" type=""OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule, OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule"" />
                        </httpModules>
                    </system.web>
                    <system.webServer>
                        <validation validateIntegratedModeConfiguration=""false"" />
                        <modules>
                           <remove name=""TelemetryHttpModule"" />
                           <add name=""TelemetryHttpModule"" type=""OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule, OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule"" preCondition=""managedHandler""/>
                        </modules>
                    </system.webServer>
                </configuration>";

        var transformedWebConfig = this.ApplyInstallTransformation(originalWebConfigContent, InstallConfigTransformationResourceName);
        this.VerifyTransformation(expectedWebConfigContent, transformedWebConfig);
    }

    [Fact]
    public void VerifyInstallationToWebConfigWithoutModules()
    {
        const string originalWebConfigContent = @"<configuration><system.webServer/></configuration>";

        const string expectedWebConfigContent = @"
                <configuration>
                    <system.webServer>
                        <validation validateIntegratedModeConfiguration=""false"" />
                        <modules>
                           <remove name=""TelemetryHttpModule"" />
                           <add name=""TelemetryHttpModule"" type=""OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule, OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule"" preCondition=""managedHandler""/>
                        </modules>
                    </system.webServer>
                    <system.web>
                        <httpModules>
                            <add name=""TelemetryHttpModule"" type=""OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule, OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule"" />
                        </httpModules>
                    </system.web>
                </configuration>";

        var transformedWebConfig = this.ApplyInstallTransformation(originalWebConfigContent, InstallConfigTransformationResourceName);
        this.VerifyTransformation(expectedWebConfigContent, transformedWebConfig);
    }

    private XDocument ApplyInstallTransformation(string originalConfiguration, string resourceName)
    {
        return this.ApplyTransformation(originalConfiguration, resourceName);
    }

    private XDocument ApplyUninstallTransformation(string originalConfiguration, string resourceName)
    {
        return this.ApplyTransformation(originalConfiguration, resourceName);
    }

    private void VerifyTransformation(string expectedConfigContent, XDocument transformedWebConfig)
    {
        Assert.True(
            XNode.DeepEquals(
                transformedWebConfig.FirstNode,
                XDocument.Parse(expectedConfigContent).FirstNode));
    }

    private XDocument ApplyTransformation(string originalConfiguration, string transformationResourceName)
    {
        XDocument result;
        Stream? stream = null;
        try
        {
            stream = typeof(WebConfigTransformTest).Assembly.GetManifestResourceStream(transformationResourceName);
            var document = new XmlTransformableDocument();
            using var transformation = new XmlTransformation(stream, null);
            stream = null;
#pragma warning disable CA3075 // Insecure DTD processing in XML
            document.LoadXml(originalConfiguration);
#pragma warning restore CA3075 // Insecure DTD processing in XML
            transformation.Apply(document);
            result = XDocument.Parse(document.OuterXml);
        }
        finally
        {
#pragma warning disable CA1508 // Avoid dead conditional code
            stream?.Dispose();
#pragma warning restore CA1508 // Avoid dead conditional code
        }

        return result;
    }
}
