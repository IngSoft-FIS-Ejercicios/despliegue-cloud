Feature: Create cosmetic

    Scenario: Navigate to create cosmetic page
        Given I am logged in as an "Employee"
        And I am in the "Employee" menu page "http://localhost:4200/employee"
        When I click the "Create" "Cosmetic" card
        Then I should be redirected to the create cosmetic page "http://localhost:4200/employee/create-cosmetic"

    Scenario: User can not create a cosmetic with empty fields
        Given I am logged in as an "Employee"
        And I visit the create cosmetic page "http://localhost:4200/employee/create-cosmetic" with "Employee" role
        Then The "Create Cosmetic" button is disabled

    Scenario: Successfully create a cosmetic
        Given I am logged in as an "Employee"
        And I visit the create cosmetic page "http://localhost:4200/employee/create-cosmetic" with "Employee" role
        When I fill in the field "Code" with "12345"
        And I fill in the field "Name" with "Cosmetic"
        And I fill in the field "Price" with "99.99"
        And I fill in the field "Description" with "Cosmetic description"
        And I click the "Create Cosmetic" button
        Then I should see a success message "Cosmetic created successfully"

    Scenario: Fail to create a cosmetic with invalid code
        Given I am logged in as an "Employee"
        And I visit the create cosmetic page "http://localhost:4200/employee/create-cosmetic" with "Employee" role
        When I fill in the field "Code" with "1236"
        And I fill in the field "Name" with "Cosmetic"
        And I fill in the field "Price" with "100.00"
        And I fill in the field "Description" with "Cosmetic description"
        Then I should see an error message "Code must be 5 characters long"

    Scenario: Fail to create a cosmetic with invalid name
        Given I am logged in as an "Employee"
        And I visit the create cosmetic page "http://localhost:4200/employee/create-cosmetic" with "Employee" role
        When I fill in the field "Code" with "12345"
        And I fill in the field "Name" with "Cosmetic123 Cosmetic123 Cosmetic123 Cosmetic123"
        And I fill in the field "Price" with "100.00"
        And I fill in the field "Description" with "Cosmetic description"
        Then I should see the character limit "30" for "Name" in red
        Then The "Create Cosmetic" button is disabled


    Scenario: Fail to create a cosmetic with invalid price
        Given I am logged in as an "Employee"
        And I visit the create cosmetic page "http://localhost:4200/employee/create-cosmetic" with "Employee" role
        When I fill in the field "Code" with "12345"
        And I fill in the field "Name" with "Cosmetic"
        And I fill in the field "Price" with "100.123"
        And I fill in the field "Description" with "Cosmetic description"
        Then I should see an error message "Price must be a number with two decimal places"
        
    Scenario: Fail to create a cosmetic with invalid description
        Given I am logged in as an "Employee"
        And I visit the create cosmetic page "http://localhost:4200/employee/create-cosmetic" with "Employee" role
        When I fill in the field "Code" with "12345"
        And I fill in the field "Name" with "Cosmetic"
        And I fill in the field "Price" with "100.00"
        And I fill in the field "Description" with "Very long description for a cosmetic that should not be allowed to be created to test the validation of the description field"
        Then I should see the character limit "70" for "Description" in red
        Then The "Create Cosmetic" button is disabled

    Scenario: Fail to create a cosmetic without name
        Given I am logged in as an "Employee"
        And I visit the create cosmetic page "http://localhost:4200/employee/create-cosmetic" with "Employee" role
        When I fill in the field "Code" with "12345"
        And I fill in the field "Price" with "100.00"
        And I fill in the field "Description" with "Cosmetic description"
        Then The "Create Cosmetic" button is disabled

    Scenario: Fail to create a cosmetic without price
        Given I am logged in as an "Employee"
        And I visit the create cosmetic page "http://localhost:4200/employee/create-cosmetic" with "Employee" role
        When I fill in the field "Code" with "12345"
        And I fill in the field "Name" with "Cosmetic"
        And I fill in the field "Description" with "Cosmetic description"
        Then The "Create Cosmetic" button is disabled

    Scenario: Fail to create a cosmetic with empty code
        Given I am logged in as an "Employee"
        And I visit the create cosmetic page "http://localhost:4200/employee/create-cosmetic" with "Employee" role
        When I fill in the field "Name" with "Cosmetic"
        And I fill in the field "Price" with "100.00"
        And I fill in the field "Description" with "Cosmetic description"
        Then The "Create Cosmetic" button is disabled

    Scenario: Fail to create a cosmetic with empty description
        Given I am logged in as an "Employee"
        And I visit the create cosmetic page "http://localhost:4200/employee/create-cosmetic" with "Employee" role
        When I fill in the field "Code" with "12345"
        And I fill in the field "Name" with "Cosmetic"
        And I fill in the field "Price" with "100.00"
        Then The "Create Cosmetic" button is disabled

    Scenario: Fail to create a cosmetic with negative price
        Given I am logged in as an "Employee"
        And I visit the create cosmetic page "http://localhost:4200/employee/create-cosmetic" with "Employee" role
        When I fill in the field "Code" with "12345"
        And I fill in the field "Name" with "Cosmetic"
        And I fill in the field "Price" with "-100.00"
        And I fill in the field "Description" with "Cosmetic description"
        Then I should see an error message "Price must be a positive number"
