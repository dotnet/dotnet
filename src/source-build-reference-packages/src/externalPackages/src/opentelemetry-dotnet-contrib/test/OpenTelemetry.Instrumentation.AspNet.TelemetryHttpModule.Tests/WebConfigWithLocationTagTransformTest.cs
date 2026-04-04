// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Xml.Linq;
using Microsoft.Web.XmlTransform;
using Xunit;

namespace OpenTelemetry.Instrumentation.AspNet.Tests;

public class WebConfigWithLocationTagTransformTest
{
    private const string InstallConfigTransformationResourceName = "OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule.Tests.Resources.web.config.install.xdt";

    [Fact]
    public void VerifyInstallationWhenNonGlobalLocationTagExists()
    {
        const string originalWebConfigContent = @"
                    <configuration>
                      <location path=""a.aspx"">
                        <system.webServer>
                          <modules>
                            <add name=""abc"" type=""type"" />
                          </modules>
                        </system.webServer>
                      </location>
                    </configuration>";

        const string expectedWebConfigContent = @"
                    <configuration>
                      <location path=""a.aspx"">
                        <system.webServer>
                          <modules>
                            <add name=""abc"" type=""type"" />
                          </modules>
                        </system.webServer>
                      </location>
                      <system.web>
                        <httpModules>
                          <add name=""TelemetryHttpModule"" type=""OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule, OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule"" />
                        </httpModules >
                      </system.web>
                      <system.webServer>
                        <validation validateIntegratedModeConfiguration=""false"" />
                        <modules>
                          <remove name=""TelemetryHttpModule"" />
                          <add name=""TelemetryHttpModule"" type=""OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule, OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule"" preCondition=""managedHandler"" />
                        </modules>
                      </system.webServer>
                    </configuration>";

        var transformedWebConfig = this.ApplyInstallTransformation(originalWebConfigContent, InstallConfigTransformationResourceName);
        this.VerifyTransformation(expectedWebConfigContent, transformedWebConfig);
    }

    [Fact]
    public void VerifyInstallationWhenGlobalAndNonGlobalLocationTagExists()
    {
        const string originalWebConfigContent = @"
                    <configuration>
                        <location path=""a.aspx"">
                            <system.webServer>
                                <modules>
                                    <add name=""abc"" type=""type"" />
                                </modules>
                            </system.webServer>
                        </location>
                        <location path=""."">
                            <system.web>
                              <httpModules>
                                <add name=""abc"" type=""type"" />
                              </httpModules >
                            </system.web>
                            <system.webServer>
                                <modules>
                                    <add name=""abc"" type=""type""/>
                                </modules>
                            </system.webServer>
                        </location>
                    </configuration>";

        const string expectedWebConfigContent = @"
                    <configuration>
                        <location path=""a.aspx"">
                            <system.webServer>
                                <modules>
                                  <add name=""abc"" type=""type"" />
                                </modules>
                            </system.webServer>
                        </location>
                        <location path=""."">
                            <system.web>
                              <httpModules>
                                <add name=""abc"" type=""type"" />
                                <add name=""TelemetryHttpModule"" type=""OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule, OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule"" />
                              </httpModules >
                            </system.web>
                            <system.webServer>
                                <modules>
                                    <add name=""abc"" type=""type"" />
                                    <remove name=""TelemetryHttpModule"" />
                                    <add name=""TelemetryHttpModule"" type=""OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule, OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule"" preCondition=""managedHandler"" />
                                </modules>
                                <validation validateIntegratedModeConfiguration=""false"" />
                            </system.webServer>
                        </location>
                        <system.web></system.web>
                        <system.webServer></system.webServer>
                    </configuration>";

        var transformedWebConfig = this.ApplyInstallTransformation(originalWebConfigContent, InstallConfigTransformationResourceName);
        this.VerifyTransformation(expectedWebConfigContent, transformedWebConfig);
    }

    [Fact]
    public void VerifyInstallationToLocationTagWithDotPathAndExistingModules()
    {
        const string originalWebConfigContent = @"
                    <configuration>
                        <location path=""."">
                            <system.web>
                              <httpModules>
                                <add name=""abc"" type=""type"" />
                              </httpModules >
                            </system.web>
                            <system.webServer>
                                <modules>
                                    <add name=""abc"" type=""type""/>
                                </modules>
                            </system.webServer>
                        </location>
                        <system.webServer>
                        </system.webServer>
                    </configuration>";

        const string expectedWebConfigContent = @"
                    <configuration>
                      <location path=""."">
                        <system.web>
                          <httpModules>
                            <add name=""abc"" type=""type"" />
                            <add name=""TelemetryHttpModule"" type=""OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule, OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule"" />
                          </httpModules >
                        </system.web>
                        <system.webServer>
                          <modules>
                            <add name=""abc"" type=""type"" />
                            <remove name=""TelemetryHttpModule"" />
                            <add name=""TelemetryHttpModule"" type=""OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule, OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule"" preCondition=""managedHandler"" />
                          </modules>
                          <validation validateIntegratedModeConfiguration=""false"" />
                        </system.webServer>
                      </location>
                      <system.webServer></system.webServer>
                      <system.web></system.web>
                    </configuration>";

        var transformedWebConfig = this.ApplyInstallTransformation(originalWebConfigContent, InstallConfigTransformationResourceName);
        this.VerifyTransformation(expectedWebConfigContent, transformedWebConfig);
    }

    [Fact]
    public void VerifyInstallationToLocationTagWithEmptyPathAndExistingModules()
    {
        const string originalWebConfigContent = @"
                    <configuration>
                        <location>
                            <system.web>
                              <httpModules>
                                <add name=""abc"" type=""type"" />
                              </httpModules >
                            </system.web>
                            <system.webServer>
                                <modules>
                                    <add name=""abc"" type=""type""/>
                                </modules>
                            </system.webServer>
                        </location>
                    </configuration>";

        const string expectedWebConfigContent = @"
                    <configuration>
                      <location>
                        <system.web>
                          <httpModules>
                            <add name=""abc"" type=""type"" />
                            <add name=""TelemetryHttpModule"" type=""OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule, OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule"" />
                          </httpModules>
                        </system.web>
                        <system.webServer>
                          <modules>
                            <add name=""abc"" type=""type"" />
                            <remove name=""TelemetryHttpModule"" />
                            <add name=""TelemetryHttpModule"" type=""OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule, OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule"" preCondition=""managedHandler"" />
                          </modules>
                          <validation validateIntegratedModeConfiguration=""false"" />
                        </system.webServer>
                      </location>
                      <system.web></system.web>
                      <system.webServer></system.webServer>
                    </configuration>";

        var transformedWebConfig = this.ApplyInstallTransformation(originalWebConfigContent, InstallConfigTransformationResourceName);
        this.VerifyTransformation(expectedWebConfigContent, transformedWebConfig);
    }

    [Fact]
    public void VerifyInstallationToLocationTagWithDotPathWithNoModules()
    {
        const string originalWebConfigContent = @"
                    <configuration>
                        <location path=""."">
                            <system.web>
                            </system.web>
                            <system.webServer>
                            </system.webServer>
                        </location>
                        <system.web>
                        </system.web>
                        <system.webServer>
                        </system.webServer>
                    </configuration>";

        const string expectedWebConfigContent = @"
                    <configuration>
                      <location path=""."">
                        <system.web>
                          <httpModules>
                            <add name=""TelemetryHttpModule"" type=""OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule, OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule"" />
                          </httpModules>
                        </system.web>
                        <system.webServer>
                          <validation validateIntegratedModeConfiguration=""false"" />
                          <modules>
                            <remove name=""TelemetryHttpModule"" />
                            <add name=""TelemetryHttpModule"" type=""OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule, OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule"" preCondition=""managedHandler"" />
                          </modules>
                        </system.webServer>
                      </location>
                      <system.web>
                      </system.web>
                      <system.webServer>
                      </system.webServer>
                    </configuration>";

        var transformedWebConfig = this.ApplyInstallTransformation(originalWebConfigContent, InstallConfigTransformationResourceName);
        this.VerifyTransformation(expectedWebConfigContent, transformedWebConfig);
    }

    [Fact]
    public void VerifyInstallationToLocationTagWithEmptyPathWithNoModules()
    {
        const string originalWebConfigContent = @"
                    <configuration>
                        <location>
                            <system.web>
                            </system.web>
                            <system.webServer>
                            </system.webServer>
                        </location>
                    </configuration>";

        const string expectedWebConfigContent = @"
                    <configuration>
                      <location>
                        <system.web>
                          <httpModules>
                            <add name=""TelemetryHttpModule"" type=""OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule, OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule"" />
                          </httpModules>
                        </system.web>
                        <system.webServer>
                          <validation validateIntegratedModeConfiguration=""false"" />
                          <modules>
                            <remove name=""TelemetryHttpModule"" />
                            <add name=""TelemetryHttpModule"" type=""OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule, OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule"" preCondition=""managedHandler"" />
                          </modules>
                        </system.webServer>
                      </location>
                      <system.web>
                      </system.web>
                      <system.webServer>
                      </system.webServer>
                    </configuration>";

        var transformedWebConfig = this.ApplyInstallTransformation(originalWebConfigContent, InstallConfigTransformationResourceName);
        this.VerifyTransformation(expectedWebConfigContent, transformedWebConfig);
    }

    [Fact]
    public void VerifyInstallationToLocationTagWithDotPathWithGlobalModules()
    {
        const string originalWebConfigContent = @"
                    <configuration>
                        <location path=""."">
                            <system.web>
                            </system.web>
                            <system.webServer>
                            </system.webServer>
                        </location>
                        <system.web>
                            <httpModules>
                                <add name=""abc"" type=""type"" />
                            </httpModules>
                        </system.web>
                        <system.webServer>
                            <modules>
                                <add name=""abc"" type=""type""/>
                            </modules>
                        </system.webServer>
                    </configuration>";

        const string expectedWebConfigContent = @"
                    <configuration>
                      <location path=""."">
                        <system.web>
                        </system.web>
                        <system.webServer>
                            <validation validateIntegratedModeConfiguration=""false"" />
                        </system.webServer>
                      </location>
                      <system.web>
                          <httpModules>
                              <add name=""abc"" type=""type"" />
                              <add name=""TelemetryHttpModule"" type=""OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule, OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule"" />
                          </httpModules>
                      </system.web>
                      <system.webServer>
                        <modules>
                          <add name=""abc"" type=""type"" />
                          <remove name=""TelemetryHttpModule"" />
                          <add name=""TelemetryHttpModule"" type=""OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule, OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule"" preCondition=""managedHandler"" />
                        </modules>
                      </system.webServer>
                    </configuration>";

        var transformedWebConfig = this.ApplyInstallTransformation(originalWebConfigContent, InstallConfigTransformationResourceName);
        this.VerifyTransformation(expectedWebConfigContent, transformedWebConfig);
    }

    [Fact]
    public void VerifyInstallationToLocationTagWithEmptyPathWithGlobalModules()
    {
        const string originalWebConfigContent = @"
                    <configuration>
                        <location>
                        </location>
                        <system.web>
                          <httpModules>
                            <add name=""abc"" type=""type"" />
                          </httpModules>
                        </system.web>
                        <system.webServer>
                            <modules>
                                <add name=""abc"" type=""type""/>
                            </modules>
                        </system.webServer>
                    </configuration>";

        const string expectedWebConfigContent = @"
                    <configuration>
                      <location>
                      </location>
                      <system.web>
                          <httpModules>
                              <add name=""abc"" type=""type"" />
                              <add name=""TelemetryHttpModule"" type=""OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule, OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule"" />
                          </httpModules>
                      </system.web>
                      <system.webServer>
                        <modules>
                          <add name=""abc"" type=""type"" />
                          <remove name=""TelemetryHttpModule"" />
                          <add name=""TelemetryHttpModule"" type=""OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule, OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule"" preCondition=""managedHandler"" />
                        </modules>
                        <validation validateIntegratedModeConfiguration=""false"" />
                      </system.webServer>
                    </configuration>";

        var transformedWebConfig = this.ApplyInstallTransformation(originalWebConfigContent, InstallConfigTransformationResourceName);
        this.VerifyTransformation(expectedWebConfigContent, transformedWebConfig);
    }

    private XDocument ApplyInstallTransformation(string originalConfiguration, string resourceName)
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
