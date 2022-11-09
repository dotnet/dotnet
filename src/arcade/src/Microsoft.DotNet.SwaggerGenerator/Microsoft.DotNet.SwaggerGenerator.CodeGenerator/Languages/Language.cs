using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using HandlebarsDotNet;
using Microsoft.DotNet.SwaggerGenerator.Modeler;

namespace Microsoft.DotNet.SwaggerGenerator.Languages
{
    public delegate void Template(TextWriter writer, object context);

    public class Templates : Dictionary<string, Template>
    {
        private Templates() { }

        public static string BasePath { get; set; } = Path.GetDirectoryName(typeof(Templates).Assembly.Location);

        public static Templates Load(string languageName, IHandlebars hb)
        {
            string templateDirectory = Path.GetFullPath(Path.Combine(
                BasePath,
                "Languages",
                languageName));
            var templates = new Templates();
            foreach (var file in Directory.EnumerateFiles(templateDirectory, "*.hb", SearchOption.AllDirectories))
            {
                var relative = Path.GetFullPath(file);
                relative = relative.Substring(templateDirectory.Length + 1);
                relative = relative.Substring(0, relative.Length - 3); // Remove extension
                templates.Add(relative, new Template(LoadTemplateFile(hb, file)));
            }

            return templates;
        }

        private static Action<TextWriter, object> LoadTemplateFile(IHandlebars hb, string path)
        {
            var file = new FileInfo(path);
            using (StreamReader reader = file.OpenText())
            {
                    return hb.Compile(reader);
            }
        }
    }

    public abstract partial class Language
    {
        public const string FormatStringPlaceholder = "%%";
        private static readonly Dictionary<string, Language> _languages;

        static Language()
        {
            _languages = new Dictionary<string, Language>(StringComparer.OrdinalIgnoreCase)
            {
                ["csharp"] = new CSharp(),
                ["angular"] = new Angular(),
            };
        }

        public abstract string Extension { get; }

        public static Language Get(string name)
        {
            _languages.TryGetValue(name, out Language value);
            return value;
        }

        [HelperMethod]
        public string TypeRef(TypeReference reference, params object[] args)
        {
            return ResolveReference(reference, args);
        }

        /// <summary>
        ///   Gets the source code representation of the given <see cref="TypeReference"/>
        /// </summary>
        protected abstract string ResolveReference(TypeReference reference, object[] args);

        public abstract Templates GetTemplates(IHandlebars hb);

        public abstract void GenerateCode(GenerateCodeContext context);
    }
}
