Feature: Manage Discount

    Scenario: Navigate to discount page
        Given I am logged in as an "Employee"
        And I am in the "Employee" menu page "http://localhost:4200/employee"
        When I click the "Manage" "Manage Discounts" card
        Then I should be redirected to the manage discount page "http://localhost:4200/employee/manage-discount"

    Scenario: Create a discount successfully
        Given I am logged in as an "Employee"
        And I visit the manage discount page "http://localhost:4200/employee/manage-discount" with "Employee" role
        When I select "Product 1" to apply the discount
        And The "Disable Discount" button is disabled
        And I fill in the field "Discount" with "10"
        And I select the discount type "Percentage"
        And Initial price of the product is "100"
        And I click the "Apply Discount" button
        Then I should see a success message "Discount applied successfully"
        And The final price of the product should be "90"

    Scenario: Disable discount
        Given I am logged in as an "Employee"
        And I visit the manage discount page "http://localhost:4200/employee/manage-discount" with "Employee" role
        When I select "Product 2" to apply the discount
        And The "Apply Discount" button is disabled
        And Product has a discount of "10"
        And I click the "Disable Discount" button to disable the discount
        Then I should see a success message "Discount disabled successfully"

    Scenario: User can not create a cosmetic with empty fields
        Given I am logged in as an "Employee"
        And I visit the manage discount page "http://localhost:4200/employee/manage-discount" with "Employee" role
        Then The "Apply Discount" button is disabled

    Scenario: User can not create a discount with empty product
        Given I am logged in as an "Employee"
        And I visit the manage discount page "http://localhost:4200/employee/manage-discount" with "Employee" role
        When I fill in the field "Discount" with "10"
        And I select the discount type "Percentage"
        Then The "Apply Discount" button is disabled

    Scenario: User can not create a discount with empty discount
        Given I am logged in as an "Employee"
        And I visit the manage discount page "http://localhost:4200/employee/manage-discount" with "Employee" role
        When I select "Product 1" to apply the discount
        And I select the discount type "Percentage"
        Then The "Apply Discount" button is disabled

    Scenario: User can not create a discount with percentage greater than 100
        Given I am logged in as an "Employee"
        And I visit the manage discount page "http://localhost:4200/employee/manage-discount" with "Employee" role
        When I select "Product 1" to apply the discount
        And I fill in the field "Discount" with "101"
        And I select the discount type "Percentage"
        Then The "Apply Discount" button is disabled
        And I should see an error message "Discount must be less than or equal to 100"

    Scenario: User can not create a discount with negative percentage
        Given I am logged in as an "Employee"
        And I visit the manage discount page "http://localhost:4200/employee/manage-discount" with "Employee" role
        When I select "Product 1" to apply the discount
        And I fill in the field "Discount" with "-10"
        And I select the discount type "Percentage"
        Then The "Apply Discount" button is disabled
        And I should see an error message "Discount must be a positive number"


        





        
