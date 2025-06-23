import { Given, When, Then, And } from 'cypress-cucumber-preprocessor/steps';

Given('I am in the page {string}', (url) => {
    cy.visit(url);
});

Given('I am logged in as {string} as {string}', (username, role) => {
    const mockedUser = {
        userName: username,
        role: role,         
        pharmacyId: '123',
      };
      cy.window().then((win) => {
        win.localStorage.setItem('login', JSON.stringify(mockedUser));
      }); 
});

And('I am in the page {string}', (url) => {
    cy.visit(url);
});
  
And('I am not logged in as {string}' , (username) => {
    cy.contains(username).should('not.exist');
});

When('I click the {string} button', (button) => {
    cy.get('.btn')
      .contains(button)
      .should('be.visible')
      .click();
});
  
Then('I should be redirected to the login page {string}', (url) => {
    cy.visit(url);
});

Then('I should see my username {string}', (username) => {
    cy.contains(username).should('be.visible');
});

And('I should see the Logout button', () => {
    cy.get('a[title="Logout"]')
    .should('be.visible');  

  cy.get('a[title="Logout"]')
    .find('svg[cIcon]')
    .should('exist'); 
  });