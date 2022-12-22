using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using HandlebarsDotNet;
using Microsoft.DotNet.SwaggerGenerator.Languages;
using Microsoft.DotNet.SwaggerGenerator.Modeler;

namespace Microsoft.DotNet.SwaggerGenerator
{
    public class GenerateCodeContext
    {
        private readonly Language _language;
        private Dictionary<string, CodeFile> _files = new Dictionary<string, CodeFile>();

        public GenerateCodeContext(
            ServiceClientModel clientModel,
            GeneratorOptions options,
            Templates templates,
            Language language)
        {
            _language = language;
            ClientModel = clientModel;
            Options = options;
            Templates = templates;
        }

        public ServiceClientModel ClientModel { get; }
        public GeneratorOptions Options { get; }
        public Templates Templates { get; }

        public IReadOnlyDictionary<string, CodeFile> Files => _files;

        public void WriteTemplate(string filePath, Template template, object context, bool append = false)
        {
            if (!filePath.EndsWith(_language.Extension))
            {
                filePath += _language.Extension;
            }

            if (_files.TryGetValue(filePath, out CodeFile file) && !append)
            {
                throw new InvalidOperationException($"File '{filePath}' was already generated.");
            }

            if (file == null)
            {
                file = new CodeFile(filePath, "");
            }
            using (var writer = new StringWriter())
            {
                template(writer, context);
                _files[filePath] = new CodeFile(filePath, file.Contents + writer);
            }
        }
    }

    public class ServiceClientCodeFactory
    {
        public List<CodeFile> GenerateCode(ServiceClientModel model, GeneratorOptions options)
        {
            Language language = Language.Get(options.LanguageName);

            IHandlebars hb = Handlebars.Create();

            RegisterHelpers(hb, language, options);

            Templates templates = language.GetTemplates(hb);

            RegisterTemplates(hb, templates);

            var context = new GenerateCodeContext(model, options, templates, language);
            language.GenerateCode(context);

            return context.Files.Values.ToList();
        }

        private void RegisterTemplates(IHandlebars hb, Templates templates)
        {
            foreach (var (name, template) in templates)
            {
                hb.RegisterTemplate(name, new Action<TextWriter, object>(template));
            }
        }

        private void RegisterHelpers(IHandlebars hb, Language language, GeneratorOptions options)
        {
            HelperFactory.RegisterAllForType(hb, typeof(DefaultHelpers), null);
            HelperFactory.RegisterAllForType(hb, language.GetType(), language);

            hb.RegisterHelper("clientName", (writer, context, parameters) => { writer.Write(options.ClientName); });
        }
    }

    internal static class Encodings
    {
        public static readonly Encoding Utf8NoBom = new UTF8Encoding(false);
    }
}
