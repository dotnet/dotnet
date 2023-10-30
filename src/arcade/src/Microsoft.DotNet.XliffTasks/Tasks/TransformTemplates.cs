// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using XliffTasks.Model;

namespace XliffTasks.Tasks
{
    public sealed class TransformTemplates : XlfTask
    {
        [Required]
        public ITaskItem[] Templates { get; set; }

        [Required]
        public ITaskItem[] UnstructuredResources { get; set; }

        [Required]
        public string[] Languages { get; set; }

        [Required]
        public string TranslatedOutputDirectory { get; set; }

        [Output]
        public ITaskItem[] TransformedTemplates { get; set; }

        protected override void ExecuteCore()
        {
            List<ITaskItem> transformedTemplates = new();
            Dictionary<string, ITaskItem> resourceMap = UnstructuredResources.ToDictionary(item => item.GetMetadata("FullPath"));
            foreach (ITaskItem template in Templates)
            {
                // special-case the default template items as the 1033 culture
                ITaskItem defaultTemplate = TransformTemplate(template, language: null, resourceMap: null);
                transformedTemplates.Add(defaultTemplate);

                // and process other languages like normal
                foreach (string language in Languages)
                {
                    ITaskItem item = TransformTemplate(template, language, resourceMap);
                    transformedTemplates.Add(item);
                }
            }

            TransformedTemplates = transformedTemplates.ToArray();
        }

        private ITaskItem TransformTemplate(ITaskItem template, string language, IDictionary<string, ITaskItem> resourceMap)
        {
            if ((language == null) ^ (resourceMap == null))
            {
                throw new ArgumentException($"Either both '{nameof(language)}' and '{nameof(resourceMap)}' must be specified, or they both must be 'null'.");
            }

            bool transformingDefaultTemplate = language == null;
            string templateCulture = transformingDefaultTemplate ? "1033" : language;

            string templateName = Path.GetFileNameWithoutExtension(template.ItemSpec);
            string templatePath = template.GetMetadata("FullPath");
            string templateDirectory = Path.GetDirectoryName(templatePath);
            XDocument templateXml = XDocument.Load(templatePath);

            // create a copy of the .vstemplate and all files
            string localizedTemplateDirectory = transformingDefaultTemplate
                ? Path.Combine(TranslatedOutputDirectory, $"{templateName}.default.1033")
                : Path.Combine(TranslatedOutputDirectory, $"{templateName}.{language}");
            Directory.CreateDirectory(localizedTemplateDirectory);
            string cultureSpecificTemplateFile = Path.Combine(localizedTemplateDirectory, Path.GetFileName(template.ItemSpec));
            File.Copy(templatePath, cultureSpecificTemplateFile, overwrite: true);

            // copy the template project files
            foreach (XElement projectNode in templateXml.Descendants().Where(d => d.Name.LocalName == "Project"))
            {
                string projectFileFullPath = Path.Combine(templateDirectory, projectNode.Attribute("File").Value);
                File.Copy(projectFileFullPath, Path.Combine(localizedTemplateDirectory, Path.GetFileName(projectNode.Attribute("File").Value)), overwrite: true);
            }

            // copy the template project items
            foreach (XElement templateItem in templateXml.Descendants().Where(d => d.Name.LocalName == "ProjectItem"))
            {
                string templateItemFullPath = Path.Combine(templateDirectory, templateItem.Value);
                string templateItemDestinationPath = Path.Combine(localizedTemplateDirectory, templateItem.Value);
                if (transformingDefaultTemplate)
                {
                    // if not localizing anything, simply strip out the translation markers
                    UnstructuredDocument document = new();
                    document.Load(templateItemFullPath);
                    Dictionary<string, string> defaultTranslation = document.Nodes.ToDictionary(node => node.Id, node => node.Source);
                    document.Translate(defaultTranslation);
                    document.Save(templateItemDestinationPath);
                }
                else
                {
                    // try to localize the template items
                    if (resourceMap.TryGetValue(templateItemFullPath, out ITaskItem unstructuredResource))
                    {
                        // copy a localized file
                        string localizedFileName = string.Concat(
                            Path.GetFileNameWithoutExtension(unstructuredResource.ItemSpec),
                            ".",
                            language,
                            Path.GetExtension(unstructuredResource.ItemSpec));
                        File.Copy(Path.Combine(TranslatedOutputDirectory, localizedFileName), templateItemDestinationPath, overwrite: true);
                    }
                    else
                    {
                        // copy the original unaltered file
                        File.Copy(templateItemFullPath, templateItemDestinationPath, overwrite: true);
                    }
                }
            }

            TaskItem item = new(cultureSpecificTemplateFile);
            item.SetMetadata("Culture", templateCulture);
            return item;
        }
    }
}
