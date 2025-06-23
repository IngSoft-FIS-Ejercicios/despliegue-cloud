Feature: Login

    Scenario: Navigate to login page
        Given I am in the page "http://localhost:4200/home"
        And I am not logged in as "Luis"
        When I click the "Login" button
        Then I should be redirected to the login page "http://localhost:4200/login"

    Scenario: Display logged user in home
        Given I am logged in as "Luis" as "Employee"
        And I am in the page "http://localhost:4200/home"
        Then I should see my username "Luis"
        And I should see the Logout button

    Scenario: Display username in employee page
        Given I am logged in as "Luis" as "Employee"
        And I am in the page "http://localhost:4200/employee"
        Then I should see my username "Luis"
        And I should see the Logout button

    Scenario: Display username in admin page
        Given I am logged in as "Luis" as "Administrator"
        And I am in the page "http://localhost:4200/admin"
        Then I should see my username "Luis"
        And I should see the Logout button

    Scenario: Display username in owner page
        Given I am logged in as "Luis" as "Owner"
        And I am in the page "http://localhost:4200/owner"
        Then I should see my username "Luis"
        And I should see the Logout button

