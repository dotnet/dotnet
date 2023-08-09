// Copyright (c) Microsoft Corporation. All rights reserved.

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Microsoft.VisualStudio.Test.Project
{
    [Export(typeof(FrameworkTemplateWizardData))]
    internal class FrameworkTemplateWizardData
    {
        private List<TemplateData> _templates;

        [ImportingConstructor]
        public FrameworkTemplateWizardData()
        {
        }

        public void Initialize(string templatePath, string wizardData)
        {
            XDocument wizardDataDoc = XDocument.Load(new StringReader(wizardData));
            XNamespace ns = wizardDataDoc.Root.GetDefaultNamespace();

            XElement templates = wizardDataDoc.Element(ns + "Templates");
            if (templates == null)
            {
                throw new InvalidOperationException(FrameworkTemplateWizardResources.MissingTemplatesInWizardData);
            }

            var templateData = from template in templates.Elements(ns + "Template")
                               select new
                               {
                                   Path = template.Attribute("Path")?.Value,
                                   MinimumFrameworkVersion = template.Attribute("MinimumFrameworkVersion")?.Value,
                               };

            _templates = new List<TemplateData>();
            foreach (var td in templateData)
            {
                TemplateData t = new TemplateData();
                if (string.IsNullOrEmpty(td.Path))
                {
                    throw new InvalidOperationException(FrameworkTemplateWizardResources.TemplatePathMustBeSpecified);
                }

                t.Path = Path.Combine(Path.GetDirectoryName(templatePath), td.Path);

                if (!Version.TryParse(td.MinimumFrameworkVersion, out Version version))
                {
                    throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, FrameworkTemplateWizardResources.InvalidValueSpecified, td.MinimumFrameworkVersion));
                }

                t.MinimumVersion = version;
                _templates.Add(t);
            }
        }

        public string GetTemplatePathForFramework(string frameworkVersion)
        {
            if (Version.TryParse(frameworkVersion, out Version targetFrameworkVersion))
            {

                // We need to get the path to the project to create
                TemplateData projectTemplate = _templates.Where(t => targetFrameworkVersion >= t.MinimumVersion)
                                                     .OrderByDescending(t => t.MinimumVersion)
                                                     .FirstOrDefault();

                return projectTemplate == null? null : Path.GetFullPath(projectTemplate.Path);
            }

            return null;
        }
    }
}
