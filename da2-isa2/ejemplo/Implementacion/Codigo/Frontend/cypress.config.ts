import { defineConfig } from 'cypress';
import cucumber from 'cypress-cucumber-preprocessor';
const browserify = require('@cypress/browserify-preprocessor');

const options = {
    ...browserify.defaultOptions,
    typescript: require.resolve('typescript'),
};
export default defineConfig({
    e2e: {        
        specPattern: "**/*.feature",
        setupNodeEvents(on) {
            on('file:preprocessor', cucumber(options))
        },
    },
});