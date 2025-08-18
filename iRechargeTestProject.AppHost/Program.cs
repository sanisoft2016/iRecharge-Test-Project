using Microsoft.Extensions.Hosting;
using System.Buffers.Text;

var builder = DistributedApplication.CreateBuilder(args);//"1234@Abc$56"

var sqlPassword = builder.Configuration["Parameters:sqlpassword"]
                  ?? Environment.GetEnvironmentVariable("Parameters__sqlpassword");
var password = builder.AddParameter("sqlPassword", sqlPassword);

var sqlUsername = builder.Configuration["Parameters:sqlusername"]
                  ?? Environment.GetEnvironmentVariable("Parameters__sqlusername");
var username = builder.AddParameter("sqlUsername", sqlUsername);

var sqlPortNo = builder.Configuration["Parameters:sqlportno"]
                  ?? Environment.GetEnvironmentVariable("Parameters__sqlportno");
var sqlPortNoInInteger = int.Parse(sqlPortNo);//sqlPortNoInInteger = 60518


var postgres = builder.AddPostgres("irechargepostgres12th", username, password, sqlPortNoInInteger)
    .WithImageTag("17.0")
    .WithDataVolume(isReadOnly: false)
    .WithPgAdmin();

var sqlServerDb = postgres.AddDatabase("irechargeDb2nd");



builder.AddProject<Projects.iRechargeTestProject_API>("iRechargeTestProject-v3-api")
     .WithReference(sqlServerDb)
     .WaitFor(sqlServerDb);


builder.Build().Run();