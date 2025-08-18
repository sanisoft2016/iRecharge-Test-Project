REQUIREMENT
A running docker engine, PostgreSQL Database Image, .net 9.0 and .net Aspire for orchestration, localstack/localstack for local development and can be used in place of AWS SDK for SQS for message queue service,


You must Set environmental variable:
1.	 setx IRECHARGE_DB_CONNECTION "Host=127.0.0.1;Port=60518;Username=postgreadmin;Password=1234@Abc-56;Database=irechargeDb2nd"
2.	setx Parameters__sqlportno "60518"
3.	setx JWT__Secret "JWT093ticationHIGHsecuredPasswordVVVp1OH7Xzyr"
4.	setx Parameters__sqlusername "postgreadmin"
5.	setx Parameters__sqlpassword "1234@Abc-56"
6. setx AWS__SqsQueueUrl http://sqs.af-south-1.localhost.localstack.cloud:4566/000000000000/my-local-queue

   To do Automatic Database migration. always make iRechargeTestProject.AppHost as the start-up project.
   You need to login using appropriate Credential to access either OrderController and ProductController
   To access ProductController method, use: Username: admin, password: 123456@Abc
    To access OrderController method, Register your self as a customer using: localhost:5254/api/Account/customer-self-registeration
