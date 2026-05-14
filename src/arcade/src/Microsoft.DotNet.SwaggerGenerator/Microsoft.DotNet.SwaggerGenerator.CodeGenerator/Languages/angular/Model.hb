export {{#if IsEnum}}enum{{else}}class{{/if}} {{pascalCase Name}} {
    {{#if IsEnum}}
    {{#each Values}}
    {{pascalCase this}} = "{{this}}",
    {{/each}}
    {{else}}
    public constructor(
        {
        {{#each Properties}}
            {{camelCase Name}},
        {{/each}}
        }: {
        {{#each Properties}}
            {{camelCase Name}}{{#if (not Required)}}?{{/if}}: {{typeRef Type}},
        {{/each}}
        }
    ) {
        {{#each Properties}}
        this._{{camelCase Name}} = {{camelCase Name}};
        {{/each}}
    }
    {{#each Properties}}

    private _{{camelCase Name}}{{#if (not Required)}}?{{/if}}: {{typeRef Type}};

    public get {{camelCase Name}}(): {{typeRef Type}}{{#if (not Required)}} | undefined{{/if}} {
        return this._{{camelCase Name}};
    }
    {{#if (not ReadOnly)}}

    public set {{camelCase Name}}(__value: {{typeRef Type}}{{#if (not Required)}} | undefined{{/if}}) {
        this._{{camelCase Name}} = __value;
    }
    {{/if}}
    {{/each}}
    {{#if AdditionalProperties}}

    private __data: Record<string, {{typeRef AdditionalProperties}}> = {};

    public data(): Record<string, {{typeRef AdditionalProperties}}>;
    public data(key: string): {{typeRef AdditionalProperties}};
    public data(key: string, value: {{typeRef AdditionalProperties}}): void;
    public data(key?: string, value?: {{typeRef AdditionalProperties}}): Record<string, {{typeRef AdditionalProperties}}> | {{typeRef AdditionalProperties}} | void {
        if (key !== undefined && value !== undefined)
        {
            this.__data[key] = value;
        }
        if (key !== undefined)
        {
            return this.__data[key];
        }
        return this.__data;
    }
    {{/if}}
    {{#if (isVerifyable this)}}
    
    public isValid(): boolean {
        return (
            {{#each RequiredProperties}}
            this._{{camelCase Name}} !== undefined{{#unless @last}} &&{{/unless}}
            {{/each}}
        );
    }
    {{/if}}

    public static fromRawObject(value: any): {{pascalCase Name}} {
        let result = new {{pascalCase Name}}({
            {{#each Properties}}
            {{camelCase Name}}: value["{{Name}}"] == null ? undefined : {{#deserializeFromRawObject Type}}value["{{Name}}"]{{/deserializeFromRawObject}} as any,
            {{/each}}
        });
        {{#if AdditionalProperties}}
        for (let key of Object.keys(value))
        {
            {{#each Properties}}
            if (key === "{{Name}}")
            {
                continue;
            }
            {{/each}}
            result.data(key, {{#deserializeFromRawObject AdditionalProperties}}value[key]{{/deserializeFromRawObject}});
        }
        {{/if}}
        return result;
    }

    public static toRawObject(value: {{pascalCase Name}}): any {
        let result: any = {};
        {{#if AdditionalProperties}}
        for (const key of Object.keys(value.__data))
        {
            result[key] = {{#serializeToRawObject AdditionalProperties}}value.__data[key]{{/serializeToRawObject}};
        }
        {{/if}}
        {{#each Properties}}
        {{#if (not Required)}}
        if (value._{{camelCase Name}}) {
            result["{{Name}}"] = {{#serializeToRawObject Type}}value._{{camelCase Name}}{{/serializeToRawObject}};
        }
        {{else}}
        result["{{Name}}"] = {{#serializeToRawObject Type}}value._{{camelCase Name}}{{/serializeToRawObject}};
        {{/if}}
        {{/each}}
        return result;
    }
    {{/if}}
}
