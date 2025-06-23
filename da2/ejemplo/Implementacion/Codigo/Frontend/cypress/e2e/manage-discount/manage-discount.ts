import { Given, When, Then, And } from 'cypress-cucumber-preprocessor/steps';
import { environment } from '../../support/env';


const mockProducts = [
    { id: 1, code: 'P001', name: 'Product 1', quantity: 50, price: 100, hasDiscount: false },
    { id: 2, code: 'P002', name: 'Product 2', quantity: 30, price: 150, hasDiscount: true, discountPercentage: 10 },
    { id: 3, code: 'P003', name: 'Product 3', quantity: 20, price: 200, hasDiscount: false },
  ];
  
beforeEach(() => {
cy.intercept('GET', `${environment.apiUrl}/api/products`, {
    statusCode: 200,
    body: mockProducts,
}).as('getProducts');
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
  
Then('I should be redirected to the manage discount page {string}', (url) => {
    cy.visit(url);
});

And('I visit the manage discount page {string} with {string} role', (url, role) => {
    cy.visit(url);
    cy.contains(role).should('be.visible');
  });

When('I select {string} to apply the discount', (name) => {
    cy.get('#Product').select(name);
});

And('Initial price of the product is {string}', (price) => {
    cy.contains(`Initial price: ${price}`).should('be.visible');
});

And('Product has a discount of {string}', (discount) => {
    cy.get('#Discount').should('have.value', discount);
});

And('I fill in the field {string} with {string}', (field, value) => {
    cy.get(`#${field}`).type(value);
});

And('I select the discount type {string}', (type) => {
    cy.get('#Discount-Type').select(type);

});

And('I click the {string} button', (button) => {
    cy.intercept('POST', `${environment.apiUrl}/api/discounts`, {
        statusCode: 200,
        body: { message: 'Discount applied successfully' },
    }).as('createDiscount');
    
    cy.get('.btn')
    .contains(button)
    .should('be.visible')
    .click();
});

And('I click the {string} button to disable the discount', (button) => {
    cy.intercept('DELETE', `${environment.apiUrl}/api/discounts/*`, {
        statusCode: 200,
        body: { message: 'Discount disabled successfully' },
    }).as('removeDiscount');
    cy.get('.btn').contains(button).should('be.visible').click();

    cy.get('.btn')
    .contains(button)
    .should('be.visible')
    .click();
});

Then('The {string} button is disabled', (buttonText) => {
    cy.contains('button', buttonText).should('be.disabled');
  });

Then('I should see a success message {string}', (message) => {
    cy.get('c-toast')
      .should('be.visible')
      .contains(message);
  });

And('I should see an error message {string}', (message) => {
cy.get('.error-message') 
    .should('be.visible')
    .contains(message);
});

And('The final price of the product should be {string}', (price) => {
    cy.contains(`Final price: ${price}`).should('be.visible');
});

And('The {string} button is disabled', (buttonText) => {
    cy.contains('button', buttonText).should('be.disabled');
  });

