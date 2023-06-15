// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Xml.Xsl;
using NuGet.Common;

namespace NuGet.PackageManagement.UI
{
    internal class UpgradeLogger : IDisposable
    {
        // Example upgrade report XML:
        //
        // <?xml version="1.0" encoding="UTF-8"?>
        // <NuGetUpgradeReport Name="ConsoleApplication13" BackupPath="c:\MySolution\Backup\ConsoleApplication13">
        //   <Properties />
        //   <Projects>
        //     <Project Name="ConsoleApplication13">
        //       <Issues>
        //         <Package Name="Newtonsoft.Json" Version="1.0.0">
        //           <Issue Description="contains an install.ps1 script that will not be applied after upgrading." />
        //           <Issue Description="could not be found." />
        //         </Package>
        //       </Issues>
        //       <IncludedPackages>
        //         <Package Name="Microsoft.Owin" Version="3.0.1" />
        //         <Package Name="WindowsAzure.Storage" Version="7.0.0" />
        //       </IncludedPackages>
        //       <ExcludedPackages>
        //         <Package Name="Microsoft.Azure.KeyVault.Core" Version="1.0.0" />
        //         <Package Name="Microsoft.Data.Edm" Version="5.6.4" />
        //         <Package Name="Microsoft.Data.OData" Version="5.6.4" />
        //         <Package Name="Microsoft.Data.Services.Client" Version="5.6.4" />
        //         <Package Name="Newtonsoft.Json" Version="6.0.8" />
        //         <Package Name="Owin" Version="1.0.0" />
        //         <Package Name="System.Spatial" Version="5.6.4" />
        //       </ExcludedPackages>
        //     </Project>
        //   </Projects>
        // </NuGetUpgradeReport>
        //
        // Note that we currently only supported upgrading a single project, but the log format can handle multiple projects
        // (in which case the log name would likely be the name of the solution).

        private const string DescriptionString = "Description";
        private const string ExcludedPackagesString = "ExcludedPackages";
        private const string IncludedPackagesString = "IncludedPackages";
        private const string IssuesString = "Issues";
        private const string IssueString = "Issue";
        private const string NameString = "Name";
        private const string VersionString = "Version";
        private const string NuGetUpgradeReportString = "NuGetUpgradeReport";
        private const string PackageString = "Package";
        private const string ProjectsString = "Projects";
        private const string ProjectString = "Project";
        private const string PropertiesString = "Properties";
        private const string PropertyString = "Property";
        private const string ValueString = "Value";
        private const string BackupPathString = "BackupPath";

        private const string XsltManifestResourceName = "NuGet.PackageManagement.UI.Resources.UpgradeReport.xslt";

        private readonly ConcurrentDictionary<string, XmlElement> _projectElements = new ConcurrentDictionary<string, XmlElement>();

        private readonly XmlDocument _xmlDocument;

        private readonly XmlElement _projectsElement;
        private readonly XmlElement _propertiesElement;

        private readonly string _backupPath;
        private readonly string _htmlFilePath;

        internal UpgradeLogger(string reportName, string backupPath)
        {
            if (string.IsNullOrEmpty(reportName))
            {
                throw new ArgumentException(Resources.ArgumentNullOrEmpty, nameof(reportName));
            }

            if (string.IsNullOrEmpty(backupPath) || !Directory.Exists(backupPath))
            {
                throw new ArgumentException(Resources.UpgradeLogger_BackupPathMustBeValid, nameof(backupPath));
            }

            _backupPath = backupPath;
            _htmlFilePath = $@"{_backupPath}\NuGetUpgradeLog.html";
            _xmlDocument = new XmlDocument { PreserveWhitespace = true };
            _xmlDocument.LoadXml($"<?xml version='1.0' encoding='UTF-16'?>\r\n<{NuGetUpgradeReportString}>\r\n</{NuGetUpgradeReportString}>");

            var upgradeReportElement = _xmlDocument.DocumentElement;
            Debug.Assert(upgradeReportElement != null, "_upgradeReportElement != null");

            upgradeReportElement.SetAttribute(NameString, reportName);
            upgradeReportElement.SetAttribute(BackupPathString, backupPath);

            _propertiesElement = _xmlDocument.CreateElement(PropertiesString);
            upgradeReportElement.AppendChild(_propertiesElement);

            _projectsElement = _xmlDocument.CreateElement(ProjectsString);
            upgradeReportElement.AppendChild(_projectsElement);
        }

        internal void SetProperty(string propertyName, string propertyValue)
        {
            var propertyElement = _xmlDocument.CreateElement(PropertyString);
            propertyElement.SetAttribute(NameString, propertyName);
            propertyElement.SetAttribute(ValueString, propertyValue);
            _propertiesElement.AppendChild(propertyElement);
        }

        internal void RegisterPackage(string projectName, string name, string version, IList<PackagingLogMessage> issues, bool included)
        {
            var packageElement = _xmlDocument.CreateElement(PackageString);
            packageElement.SetAttribute(NameString, name);
            packageElement.SetAttribute(VersionString, version);

            var packagesElement = GetProjectElement(projectName).SelectSingleNode(included ? IncludedPackagesString : ExcludedPackagesString);
            Debug.Assert(packagesElement != null, "packagesElement != null");
            packagesElement.AppendChild(packageElement);

            if (issues.Count > 0)
            {
                var issuesElement = GetProjectElement(projectName).SelectSingleNode(IssuesString);
                Debug.Assert(issuesElement != null, "issuesElement != null");
                var issuePackageElement = packageElement.Clone();
                issuesElement.AppendChild(issuePackageElement);

                foreach (var issue in issues)
                {
                    var issueElement = _xmlDocument.CreateElement(IssueString);
                    issueElement.SetAttribute(DescriptionString, issue.Message);

                    issuePackageElement.AppendChild(issueElement);
                }
            }
        }

        internal string GetHtmlFilePath()
        {
            return _htmlFilePath;
        }

        internal void Flush()
        {

            using (var xsltStream = typeof(UpgradeLogger).Assembly.GetManifestResourceStream(XsltManifestResourceName))
            {
                Debug.Assert(xsltStream != null, $"Resource {XsltManifestResourceName} could not be loaded.");

                using (var xmlReader = XmlReader.Create(xsltStream))
                using (var writer = new XmlTextWriter(_htmlFilePath, null))
                {
                    var transform = new XslCompiledTransform();
                    transform.Load(xmlReader);
                    transform.Transform(_xmlDocument, writer);
                }
            }
        }

        private XmlElement GetProjectElement(string projectName)
        {
            return _projectElements.GetOrAdd(projectName, name =>
            {
                var projectElement = _xmlDocument.CreateElement(ProjectString);
                projectElement.SetAttribute("Name", projectName);
                projectElement.AppendChild(_xmlDocument.CreateElement(IssuesString));
                projectElement.AppendChild(_xmlDocument.CreateElement(IncludedPackagesString));
                projectElement.AppendChild(_xmlDocument.CreateElement(ExcludedPackagesString));
                _projectsElement.AppendChild(projectElement);
                return projectElement;
            });
        }

        public void Dispose()
        {
            Flush();
        }

        internal enum ErrorLevel
        {
            Information,
            Warning,
            Error
        }
    }
}
