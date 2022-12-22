import { NgModule, Injectable, Inject, InjectionToken } from "@angular/core";
import { HttpClientModule, HttpClient, HttpHeaders, HttpParams } from "@angular/common/http";
import { Observable } from "rxjs";
import { map } from "rxjs/operators";
import { parseISO } from "date-fns";

import * as models from "./models";
import { Helper } from "./helper";

type RequestOptions = {
  body?: any;
  headers?: HttpHeaders;
  params?: HttpParams;
  responseType?: "json" | "arraybuffer" | "blob" | "text";
};

export interface ICredentials {
    processRequest(method: string, uri: string, opts: RequestOptions): [string, string, RequestOptions];
}

export class TokenCredentials {
    constructor(public value: string, public type: string = "Bearer") {
    }

    processRequest(method: string, uri: string, opts: RequestOptions): [string, string, RequestOptions] {
        if (!opts || !opts.headers) {
            throw new Error("request options not valid");
        }
        opts.headers = opts.headers.set("Authorization", this.type + " " + this.value);
        return [method, uri, opts];
    }
}

export let {{pascalCase Name}}OptionsToken = new InjectionToken<{{pascalCase Name}}Options>('{{camelCase Name}}Options');
export interface {{pascalCase Name}}Options {
    baseUrl: string;
    defaultHeaders: {
        [key: string]: string;
    };
    credentials?: ICredentials;
}

@Injectable()
export class {{pascalCase Name}}Service {
    public options: {{pascalCase Name}}Options;
    constructor(private http: HttpClient, @Inject({{pascalCase Name}}OptionsToken) options: {{pascalCase Name}}Options) {
        this.options = Object.assign({}, {{pascalCase Name}}Module.defaultOptions, options);
        {{#each MethodGroups}}
        this.{{camelCase name}} = new {{pascalCase Name}}ApiService(this);
        {{/each}}
    }
{{#each MethodGroups}}
    public {{camelCase name}}: I{{pascalCase Name}}Api;
{{/each}}

    public request(method: string, uri: string, opts: RequestOptions): Observable<any> {
        if (this.options.credentials) {
            [method, uri, opts] = this.options.credentials.processRequest(method, uri, opts);
        }
        return this.http.request(method, uri, opts);
    }
}

@NgModule({
    imports: [
        HttpClientModule,
    ],
    providers: [
        {{pascalCase Name}}Service,
        { provide: {{pascalCase Name}}OptionsToken, useValue: {{pascalCase Name}}Module.defaultOptions },
    ],
})
export class {{pascalCase Name}}Module {
    public static defaultOptions: {{pascalCase Name}}Options = {
        baseUrl: "{{BaseUrl}}/",
        defaultHeaders: {},
    };

    public static forRoot(options?: Partial<{{pascalCase Name}}Options>) {
        return {
            ngModule: {{pascalCase Name}}Module,
            providers: [
                { provide: {{pascalCase Name}}OptionsToken, useValue: options || {} },
            ],
        };
    }
}
{{#each MethodGroups}}

{{> MethodGroup}}
{{/each}}
