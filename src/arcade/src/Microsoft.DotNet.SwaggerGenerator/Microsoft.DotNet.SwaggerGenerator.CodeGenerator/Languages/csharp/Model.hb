using System;
using System.Collections.Immutable;
{{#if IsEnum}}
using System.Runtime.Serialization;
{{/if}}
using Newtonsoft.Json;

namespace {{pascalCaseNs Namespace}}.Models
{
    public {{#if IsEnum}}enum{{else}}partial class{{/if}} {{pascalCase Name}}
    {
        {{#if IsEnum}}
        {{#each Values}}
        [EnumMember(Value = "{{this}}")]
        {{pascalCase this}},
        {{/each}}
        {{else}}
        public {{pascalCase Name}}({{#each RequiredAndReadOnlyProperties}}{{typeRef Type}}{{#if (and (not Required) (not (IsNullable Type)))}}?{{/if}} {{camelCase Name}}{{#unless @last}}, {{/unless}}{{/each}})
        {
            {{#each RequiredAndReadOnlyProperties}}
            {{pascalCase Name}} = {{camelCase Name}};
            {{/each}}
        }
        {{#each Properties}}

        [JsonProperty("{{Name}}")]
        public {{typeRef Type}}{{#if (and (not Required) (not (IsNullable Type)))}}?{{/if}} {{pascalCase Name}} { get;{{#if ReadOnly}}{{else}} set;{{/if}} }
        {{/each}}
        {{/if}}
        {{#if (isVerifyable this)}}

        [JsonIgnore]
        public bool IsValid
        {
            get
            {
                {{#each Properties}}
                {{#if (and Required (isNullable Type))}}
                if ({{#nullCheck Type Required}}{{pascalCase Name}}{{/nullCheck}})
                {
                    return false;
                }
                {{/if}}
                {{/each}}
                return true;
            }
        }
        {{/if}}
    }
}
