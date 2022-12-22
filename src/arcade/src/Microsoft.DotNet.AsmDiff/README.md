# Microsoft.DotNet.AsmDiff

AsmDiff is a command line tool which may be used to check the API changes between a two sets of .NET assemblies. 

## Required Options

- `-os|--OldSet` - Provide path to an assembly or directory for an assembly set to gather the old set of types. These types will be the baseline for the compare.
- `-ns|--NewSet` - Provide path to an assembly or directory for an assembly set to gather the new set of types. If this parameter is not provided the API's for the oldset will be printed instead of the diff.

## Additional Options

- `-nsn|--NewSetName` - Provide a name for the new set in output. If this parameter is not provided the file or directory name will be used.
- `-osn|--OldSetName` - Provide a name for the old set in output. If this parameter is not provided the file or directory name will be used.
- `-u|--Unchanged` - Include members, types, and namespaces that are unchanged.
- `-r|--Removed` - Include members, types, and namespaces that were removed. (default is removed and added)
- `-a|--Added` - Include members, types, and namespaces that were added. (default is removed and added)
- `-c|--Changed` - Include members, types, and namespaces that were changed. (default is removed and added)
- `-to|--TypesOnly` - Only show down to the type level not the member level.
- `-sr|--StrikeRemoved` - For removed API's also strike them out. This option currently only works with the HTML writer which is the default.
- `-da|--DiffAttributes` - Enables diffing of the attributes as well, by default all attributes are ignored. For CSV writer causes the assembly name to be included in the column for types.
- `-dai|--DiffAssemblyInfo` - Enables diffing of the assembly level information like version, key, etc.
- `-adm|--AlwaysDiffMembers` - By default if an entire class is added or removed we don't show the members, setting this option forces all the members to be shown instead.
- `-hbm|--HighlightBaseMembers` - Highlight members that are interface implementations or overrides of a base member.
- `-ft|--FlattenTypes` - Will flatten types so that all members available on the type show on the type not just the implemented ones.
- `-gba|--GroupByAssembly` - Group the differences by assembly instead of flattening the namespaces.
- `-eat|--ExcludeAddedTypes` - Do not show types that have been added to the new set of types.
- `-ert|--ExcludeRemovedTypes` - Do not show types that have been removed from the new set of types.
- `-iia|--IncludeInternalApis` - Include internal types and members as part of the diff.
- `-ipa|--IncludePrivateApis` - Include private types and members as part of the diff.
- `-w|--DiffWriter` - Type of difference writer to use, either CSharp code diffs or flat list of compat violations (default).
- `-s|--SyntaxWriter` - Specific the syntax writer type. Only used if the writer is CSDecl
- `-o|--OutFile` - Output file path. Default is the console.
- `-l|--Language` - Provide a languagetag for localized content. Currently language specific content is only available in Markdown for en and de. If this parameter is not provided the environments default language will be used. If a specific language is not supported english is the default language.
