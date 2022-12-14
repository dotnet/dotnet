// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Utilities;
using Microsoft.DotNet.Build.Tasks;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Microsoft.DotNet.GenFacades
{
    internal class SourceGenerator
    {
        private readonly IReadOnlyDictionary<string, string> _seedTypePreferences;
        private readonly IEnumerable<string> _referenceTypes;
        private readonly IReadOnlyDictionary<string, IList<string>> _seedTypes;
        private readonly string _outputSourcePath;
        private readonly HashSet<string> _ignoreMissingTypesList = new HashSet<string>();
        private readonly ILog _logger;

        public SourceGenerator(
            IEnumerable<string> referenceTypes,
            IReadOnlyDictionary<string, IList<string>> seedTypes,
            IReadOnlyDictionary<string, string> seedTypePreferences,
            string outputSourcePath,
            string[] ignoreMissingTypesList,
            ILog logger
            )
        {
            _referenceTypes = referenceTypes;
            _seedTypes = seedTypes;
            _seedTypePreferences = seedTypePreferences;
            _outputSourcePath = outputSourcePath;
            _logger = logger;
            _ignoreMissingTypesList = ignoreMissingTypesList != null 
                                        ? new HashSet<string>(ignoreMissingTypesList)
                                        : new HashSet<string>();
        }

        public bool GenerateSource(
            IEnumerable<string> compileFiles,
            IEnumerable<string> constants,
            string langVersion,
            bool ignoreMissingTypes)
        {
            List<string> externAliases = new List<string>();

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("// <auto-generated/>"); // Adding this because the following code is autogenerated.
            sb.AppendLine("#pragma warning disable CS0618,CA2252"); // Adding this to avoid warnings while adding typeforwards for obselete types. CA2252 suppresses warnings on preview APIs

            bool result = true;

            HashSet<string> existingTypes = compileFiles != null ? TypeParser.GetAllPublicTypes(compileFiles, constants, langVersion) : null;
            IEnumerable<string> typesToForward = compileFiles == null ? _referenceTypes : _referenceTypes.Where(id => !existingTypes.Contains(id));

            foreach (string type in typesToForward.OrderBy(s => s))
            {
                IList<string> seedTypes;
                if (!_seedTypes.TryGetValue(type, out seedTypes))
                {
                    if (!ignoreMissingTypes && !_ignoreMissingTypesList.Contains(type))
                    {
                        result = false;
                        _logger.LogError("Did not find type '{0}' in any of the seed assemblies.", type);
                    }
                    continue;
                }

                string alias = "";

                if (_seedTypePreferences.Keys.Contains(type))
                {
                    alias = _seedTypePreferences[type];
                    if (!externAliases.Contains(alias))
                        externAliases.Add(alias);
                }
                else if (seedTypes.Count > 1)
                {
                    _logger.LogError("The type '{0}' is defined in multiple seed assemblies. The multiple assemblies are {1}. If this is intentional, specify the alias for this type and project reference", type, string.Join(", ", seedTypes));
                    result = false;
                    continue;
                }

                sb.AppendLine(GetTypeForwardsToString(type, alias));
            }

            sb.AppendLine("#pragma warning restore CS0618");
            if (result)
                File.WriteAllText(_outputSourcePath, BuildAliasDeclarations(externAliases) + sb.ToString());

            return result;
        }

        private string BuildAliasDeclarations(IEnumerable<string> externAliases)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string alias in externAliases.OrderBy(s => s))
            {
                sb.AppendLine(string.Format("extern alias {0};", alias));
            }

            return sb.ToString();
        }

        private static string GetTypeForwardsToString(string typeName, string alias = "")
        {
            if (!string.IsNullOrEmpty(alias))
                alias += "::";

            return string.Format($"[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof({alias}{TransformGenericTypes(typeName)}))]");
        }

        // typename`3 gets transformed into typename<,,>
        private static string TransformGenericTypes(string typeName)
        {
            int splitIndex = typeName.LastIndexOf('`');

            if (splitIndex == -1)
                return typeName;

            StringBuilder sb = new StringBuilder();
            sb.Append(typeName.Substring(0, splitIndex));
            sb.Append('<');
            sb.Append(',', int.Parse(typeName.Substring(splitIndex + 1)) - 1);
            sb.Append('>');
            return sb.ToString();
        }
    }
}
