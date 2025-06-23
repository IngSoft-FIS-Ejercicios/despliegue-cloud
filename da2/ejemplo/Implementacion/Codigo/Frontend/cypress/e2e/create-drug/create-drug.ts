import { Given, When, Then, And } from 'cypress-cucumber-preprocessor/steps';
import { environment } from '../../support/env';

const mockUser = {
  pharmacyId: '123',
};

const mockMeasureOptionsResponse = [
    { "id": 1, "name": "Grams" },
    { "id": 2, "name": "Milligrams" },
    { "id": 3, "name": "Units" }
  ];

const mockPresentationsOptionsResponse = [
    { "id": 1, "name": "Capsule" },
    { "id": 2, "name": "Tablet" },
    { "id": 3, "name": "Syrup" }
  ];

beforeEach(() => {
    cy.stub(localStorage, 'getItem').withArgs('login').returns(JSON.stringify(mockUser));
    cy.clearLocalStorage();
    
    cy.intercept('GET', environment.apiUrl + '/api/unitmeasures', {
        statusCode: 200,
        body: mockMeasureOptionsResponse,
    }).as('getUnitMeasures');

    cy.intercept('GET', environment.apiUrl + '/api/presentations', {
        statusCode: 200,
        body: mockPresentationsOptionsResponse,
    }).as('getPresentations');
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

And('I am in the create drug page {string}', (url) => {
  cy.visit(url);
  cy.contains('Create Drug').should('be.visible');
});

When('I fill the code field with {string}', (value) => {
    cy.get('input[formControlName="code"]')
    .should('be.visible')
    .type(value);
});

And('I fill the name field with {string}', (value) => {
    cy.get('input[formControlName="name"]')
    .should('be.visible')
    .type(value);
});

And('I fill the symptom field with {string}', (value) => {
    cy.get('input[formControlName="symptom"]')
    .should('be.visible')
    .type(value);
});

And('I fill the quantity field with {string}', (value) => {
    cy.get('input[formControlName="quantity"]')
    .should('be.visible')
    .type(value);
});

And('I fill the price field with {string}', (value) => {
    cy.get('input[formControlName="price"]')
    .should('be.visible')
    .type(value);
});

And('I fill the measure field with {string}', (value) => {
    cy.get('input[formControlName="measure"]')
    .should('be.visible')
    .type(value);
}); 

When('the user select the measures option', () => {
    cy.get('select[formControlName="unitMeasureControl"]')
    .should('be.visible')
    .select('Grams')
});

When('the user select the presentation option', () => {
    cy.get('select[formControlName="presentationControl"]')
    .should('be.visible')
    .select('Capsule')
});

Then('the system should display the list of measures with its names', () => {
    cy.get('select[formControlName="unitMeasureControl"]')
      .should('be.visible')
      .children('option') 
      .should('have.length.greaterThan', 0)
        .each((option) => {
        cy.wrap(option)
          .should('not.have.value', '') 
          .and('not.be.empty')
      });
});

And('the user clicks on the create button', () => {
    cy.intercept('GET', environment.apiUrl + '/api/pharmacy/by-employee', {
        statusCode: 200,
        body: { name: 'Farmacia Test'},
      }).as('getPharmacyByEmployee')
    cy.intercept('POST', environment.apiUrl + '/api/drug', {
        statusCode: 200,
        body: { message: 'Cosmetic created successfully'},
      }).as('createCosmetic');
    cy.contains('button', 'Create Drug').click();

});

Then('the system should display a success message {string}', (message) => {
    cy.get('c-toast')
    .should('be.visible')
    .contains(message);
});

Then('the system should display the list of presentations with its names', () => {
    cy.get('select[formControlName="presentationControl"]')
      .should('be.visible')
      .children('option') 
      .should('have.length.greaterThan', 0)
        .each((option) => {
        cy.wrap(option)
          .should('not.have.value', '') 
          .and('not.be.empty')
      });
});

And('the dropdown should allow the user to select {string} as measure', (value) => {
    cy.get('select[formControlName="unitMeasureControl"]') 
    .select(value);
});

And('the dropdown should allow the user to select {string} as presentation', (value) => {
    cy.get('select[formControlName="presentationControl"]') 
    .select(value);
});