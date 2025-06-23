import { Given, When, Then, And } from 'cypress-cucumber-preprocessor/steps';
import { environment } from '../../support/env';

const mockUser = {
  pharmacyId: '123',
};

beforeEach(() => {
  cy.stub(localStorage, 'getItem').withArgs('login').returns(JSON.stringify(mockUser));
});


Given('I am logged in as an {string}', (role) => {
    cy.window().then((window) => {
        const loginResponse = {
          token: '123s-456d-789f-012g',
          role: role,         
          userName: 'TestUser'
        };
        window.localStorage.setItem('login', JSON.stringify(loginResponse));
      });
  });
  

And('I am in the {string} menu page {string}' , (role, url) => {
  cy.visit(url);
  cy.contains(role).should('be.visible');
});

When('I click the {string} {string} card', (action, name) => {
    cy.get('.card')
    .contains(name)
    .should('be.visible')
    .closest('.card')
    .within(() => {
        cy.contains(action) 
        .should('be.visible');
        
        cy.get('.customCardIcon .font-weight-bold')
        .click();
    });
});

Then('I should be redirected to the create cosmetic page {string}', (url) => {
    cy.visit(url);
});


And('I visit the create cosmetic page {string} with {string} role', (url, role) => {
  cy.visit(url);
  cy.contains(role).should('be.visible');
});

When('I fill in the field {string} with {string}', (field, name) => {
    cy.get(`#${field}`).type(name);
});

And('I fill in the field {string} with {string}', (field, code) => {
    cy.get(`#${field}`).type(code);
});

And('I fill in the field {string} with {string}', (field, description) => {
    cy.get(`#${field}`).type(description);
});

And('I fill in the field {string} with {string}', (field, price) => {
    cy.get(`#${field}`).type(price);
});

And('I click the {string} button', (buttonText) => {
  cy.intercept('POST', environment.apiUrl + '/api/Cosmetic', {
    statusCode: 200,
    body: { message: 'Cosmetic created successfully'},
  }).as('createCosmetic');
  cy.contains('button', buttonText).click();
});

Then('I should see a success message {string}', (message) => {
  cy.get('c-toast')
    .should('be.visible')
    .contains(message);
});

Then('I should see an error message {string}', (message) => {
  cy.get('.error-message') 
    .should('be.visible')
    .contains(message);
});

Then('The {string} button is disabled', (buttonText) => {
  cy.contains('button', buttonText).should('be.disabled');
});

Then('I should see the character limit {string} for {string} in red', (limit, field) => {
  cy.get(`#${field}`).should('exist');
  cy.get(`#${field}`).closest('div').find('.character-count').should('exist');
  cy.get(`#${field}`).closest('div').find('.character-count').should('contain.text', `/${limit}`);
  cy.get(`#${field}`).closest('div').find('.character-count').should('have.class', 'text-danger');
});