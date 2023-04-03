# Loan Validation Service

This is a loan validation service that validates loan leads and returns a response with validation results.

## How to Run the Application

1. Clone the repository to your local machine.
2. Open the solution file in Visual Studio.
3. Build the solution.
4. Set the LoanValidation.Api project as the startup project.
5. Run the application using IIS Express or another local web server.
6. Use a tool like Postman to send a POST request to the API with a JSON payload representing a lead. The payload should have the following properties:
   - firstName: string
   - lastName: string
   - email: string
   - phone: string
   - address: string
   - loanAmount: number
   - creditScore: number
   
   Example payload:
   
   {
      "FirstName": "John",
      "LastName": "Doe",
      "EmailAddress": "johndoe@example.com",
      "PhoneNumber": "+61412345678",
      "BusinessNumber": "ABN12345678",
      "LoanAmount": 5000,
      "CitizenshipStatus": "Citizen",
      "TimeTrading": 5,
      "CountryCode": "AU",
      "Industry": "Industry 1"
    }
   
7. The API will return a JSON response with the validation results.

## Dependencies

- .NET Core 3.1
