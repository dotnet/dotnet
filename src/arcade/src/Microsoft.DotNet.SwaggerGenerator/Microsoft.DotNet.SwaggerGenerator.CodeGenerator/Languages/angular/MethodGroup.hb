export interface I{{pascalCase Name}}Api {
    {{#each Methods}}

    {{camelCase Name}}Async(
        {{#if FormalParameters}}
        parameters: {
            {{#each FormalParameters}}
            {{camelCase Name}}{{#unless Required}}?{{/unless}}: {{typeRef Type true}},
            {{/each}}
        }
        {{/if}}
    ): Observable<{{typeRef ResponseType true}}>;
    {{/each}}
}

export class {{pascalCase Name}}ApiService implements I{{pascalCase Name}}Api {
    private options: {{clientName null}}Options;
    constructor(public client: {{clientName null}}Service) {
        this.options = client.options;
    }
    {{#each Methods}}

    public {{camelCase Name}}Async(
        {{#if FormalParameters}}
        {
            {{#each FormalParameters}}
            {{camelCase Name}},
            {{/each}}
        }: {
            {{#each FormalParameters}}
            {{camelCase Name}}{{#unless Required}}?{{/unless}}: {{typeRef Type true}},
            {{/each}}
        }
        {{/if}}
    ): Observable<{{typeRef ResponseType true}}> {
        {{#each NonConstantParameters}}
        {{#if Required}}
        if ({{camelCase Name}} === undefined) {
            throw new Error("Required parameter {{camelCase Name}} is undefined.");
        }

        {{/if}}
        {{/each}}
        {{#each VerifyableParameters}}
        if ({{#unless Required}}{{camelCase Name}} !== undefined && {{/unless}}!{{camelCase Name}}.isValid()) {
            throw new Error("The parameter {{camelCase Name}} is not valid.");
        }

        {{/each}}
        {{#each ConstantParameters}}
        const {{camelCase Name}} = "{{Type.Value}}";
        {{/each}}
        let _path = this.options.baseUrl;
        if (_path.endsWith("/"))
        {
            _path = _path.slice(0, -1);
        }
        _path = _path + "{{Path}}";
        {{#each PathParameters}}
        _path = _path.replace("{ {{~Name~}} }", {{#serialize Type true}}{{camelCase Name}}{{/serialize}});
        {{/each}}

        let queryParameters = new HttpParams();
        let headerParameters = new HttpHeaders(this.options.defaultHeaders);

        {{#each QueryParameters}}
        {{#if IsConstant}}
        queryParameters = queryParameters.set("{{Name}}", {{#serialize Type true}}{{camelCase Name}}{{/serialize}});
        {{else}}
        if ({{camelCase Name}})
        {
            {{#if IsArray}}
            for (const _item of {{#serialize Type true}}{{camelCase Name}}{{/serialize}})
            {
                queryParameters = queryParameters.append("{{Name}}", _item);
            }
            {{else}}
            queryParameters = queryParameters.set("{{Name}}", {{#serialize Type true}}{{camelCase Name}}{{/serialize}});
            {{/if}}
        }
        {{/if}}

        {{/each}}
        {{#each HeaderParameters}}
        if ({{camelCase Name}})
        {
            {{#if IsArray}}
            for (const _item of {{#serialize Type true}}{{camelCase Name}}{{/serialize}})
            {
                headerParameters = headerParameters.append("{{Name}}", _item);
            }
            {{else}}
            headerParameters = headerParameters.set("{{Name}}", {{#serialize Type true}}{{camelCase Name}}{{/serialize}});
            {{/if}}
        }

        {{/each}}

        return this.client.request(
            "{{method HttpMethod}}",
            _path,
            {
                headers: headerParameters,
                params: queryParameters,
                {{#if (isBlob ResponseType)}}
                responseType: "blob",
                {{else}}
                {{#if (isVoid ResponseType)}}
                responseType: "text",
                {{else}}
                responseType: "json",
                {{/if}}
                {{/if}}
                {{#with BodyParameter}}
                body: {{#if (not Required)}}{{camelCase Name}} == undefined ? undefined : {{/if}}{{#serialize Type true}}{{camelCase Name}}{{/serialize}},
                {{/with}}
            }
        )
        {{~#if (not (isVoid ResponseType))~}}
        .pipe(
            map(raw => {{#deserializeFromRawObject ResponseType true}}raw{{/deserializeFromRawObject}})
        )
        {{~/if~}}
        ;

    }
    {{/each}}
}
