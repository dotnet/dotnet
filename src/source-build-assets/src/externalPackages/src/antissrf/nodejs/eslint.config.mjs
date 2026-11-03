import eslint from "@eslint/js";
import tseslint from "typescript-eslint";
import security from "eslint-plugin-security";

export default tseslint.config({
    files: ["**/*.ts"],
    extends: [eslint.configs.recommended, tseslint.configs.recommendedTypeChecked, security.configs.recommended],
    languageOptions: {
        parserOptions: {
            projectService: true,
            tsconfigRootDir: import.meta.dirname
        }
    },
    plugins: {
        eslint: eslint
    },
    rules: {
        "func-style": ["error", "declaration", { "allowArrowFunctions": true }]
    }
});
