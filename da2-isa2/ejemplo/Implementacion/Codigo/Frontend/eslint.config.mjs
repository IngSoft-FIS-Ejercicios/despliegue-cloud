import globals from "globals";
import pluginJs from "@eslint/js";
import tseslint from "typescript-eslint";

export default tseslint.config({
  files: ["**/*.ts"],
  extends: [...tseslint.configs.strict],
  rules: {
    "@typescript-eslint/no-non-null-assertion": "off",
    "@typescript-eslint/no-explicit-any": "warn",
    "@typescript-eslint/no-extraneous-class": "warn",
    "@typescript-eslint/no-useless-constructor": "warn",
    "@typescript-eslint/no-unused-vars": "warn",
    "@typescript-eslint/no-this-alias": "warn",
     "@typescript-eslint/no-require-imports": "warn",
  },
});
