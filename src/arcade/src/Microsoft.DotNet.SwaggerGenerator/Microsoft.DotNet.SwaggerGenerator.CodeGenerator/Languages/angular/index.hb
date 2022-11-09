export {
    ICredentials,
    TokenCredentials,
    {{pascalCase Name}}Options,
    {{pascalCase Name}}Service,
    {{pascalCase Name}}Module,
    {{#each MethodGroups}}
    I{{pascalCase Name}}Api,
    {{/each}}
} from "./{{camelCase Name}}";

