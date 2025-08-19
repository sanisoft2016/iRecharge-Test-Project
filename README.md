I go for Option 2: C# ASP.NET Core Web API with PostgreSQL/MySQL and AWS SDK for SQS

Architecture & Design
Clean Architecture
The solution follows full Clean Architecture principles, ensuring separation of concerns and maintainability.

Repository Pattern
A Generic Repository Pattern is used to abstract data access and enforce consistency across the data layer.

Configuration & Secrets
Environment variables are used for sensitive configuration values (e.g., DB connections, JWT secrets, AWS SQS URL).

Authentication & Authorization
Implemented using ASP.NET Identity and JWT-based authentication, ensuring secure access to protected endpoints.

Error Logging
Serilog is used for application-wide logging.
All errors are automatically logged into:
The Database (for structured error tracking).
A text file (for local troubleshooting and audit trails).
This provides robust observability and makes debugging easier in both local and production environments.

AWS SDK for SQS
Ihe project uses localstack/localstack (in place of AWS SDK for SQS) for local development. It serves as a drop-in replacement for the AWS SDK when working with SQS, providing a fully local message queue service without needing access to AWS.


Startup Workflow

Clone the repository
git clone https://github.com/sanisoft2016/iRecharge-Test-Project.git and
Rebuild the solution in Visual Studio 2022 (with .NET 9.0 SDK installed).

Set environment variables (Windows example):

setx IRECHARGE_DB_CONNECTION "Host=127.0.0.1;Port=60518;Username=postgreadmin;Password=1234@Abc-56;Database=irechargeDb2nd"
setx Parameters__sqlportno "60518"
setx JWT__Secret "JWT093ticationHIGHsecuredPasswordVVVp1OH7Xzyr"
setx Parameters__sqlusername "postgreadmin"
setx Parameters__sqlpassword "1234@Abc-56"
setx AWS__SqsQueueUrl "http://sqs.af-south-1.localhost.localstack.cloud:4566/000000000000/my-local-queue"


⚠️ Important: After setting these variables, close and re-open Visual Studio 2022 for the changes to take effect.

Set iRechargeTestProject.AppHost as the Start-up Project,
This ensures that the DbContext is created and database migrations run automatically during application startup.

Run the application 🎉

🔐 Authentication & Authorization

ProductController
Login with:
Username: admin and Password: 123456@Abc

OrderController
First register as a customer:
POST http://localhost:5254/api/Account/customer-self-registeration
Then log in with your registered credentials to access OrderController endpoints.

📖 API Documentation
Refer to Swagger UI (available at:
http://localhost:5254/swagger/index.html) for full API documentation and to interact with all available endpoints.