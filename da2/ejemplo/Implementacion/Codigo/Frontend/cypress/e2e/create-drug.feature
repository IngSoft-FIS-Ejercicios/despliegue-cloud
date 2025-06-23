Feature: Create drug

    Scenario: Create drug successfully
        Given I am logged in as an "Employee"
        And I am in the create drug page "http://localhost:4200/employee/create-drug"
        When I fill the code field with "12345"
        And I fill the name field with "Paracetamol"
        And I fill the symptom field with "Headache"
        And I fill the price field with "100"
        And I fill the quantity field with "10"
        And the user clicks on the create button
        Then the system should display a success message "Success creating"
    
    Scenario: Get drug measures
        Given I am logged in as an "Employee"
        And I am in the create drug page "http://localhost:4200/employee/create-drug"
        When the user select the measures option
        Then the system should display the list of measures with its names
        And the dropdown should allow the user to select "Milligrams" as measure

    Scenario: Get drug presentations
        Given I am logged in as an "Employee"
        And I am in the create drug page "http://localhost:4200/employee/create-drug"
        When the user select the presentation option
        Then the system should display the list of presentations with its names
        And the dropdown should allow the user to select "Capsule" as presentation