using Amazon.Extensions.NETCore.Setup;
using Amazon.SQS;
using iRechargeTestProject.API;
using iRechargeTestProject.API.BackgroundServices;
using iRechargeTestProject.Application.IService;
using iRechargeTestProject.Application.Service;
using iRechargeTestProject.Domain.Entities;
using iRechargeTestProject.Domain.IRepository;
using iRechargeTestProject.Infrastructure.Data;
using iRechargeTestProject.Infrastructure.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Sinks.PostgreSQL;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Text.Json.Serialization;

//For automatic global error logging into the database and text file:
var postgresConnectionString = Environment.GetEnvironmentVariable("IRECHARGE_DB_CONNECTION");
if (string.IsNullOrWhiteSpace(postgresConnectionString))
    throw new InvalidOperationException("Database connection string not set.");

// Define the column writers for the PostgreSQL sink
var columnWriters = new Dictionary<string, ColumnWriterBase>
{
    { "Message", new RenderedMessageColumnWriter() },
    { "MessageTemplate", new MessageTemplateColumnWriter() },
    { "Level", new LevelColumnWriter() },
    { "TimeStamp", new TimestampColumnWriter() },
    { "Exception", new ExceptionColumnWriter() },
    { "Properties", new LogEventSerializedColumnWriter() }
};

Serilog.Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/app_log.txt", rollingInterval: RollingInterval.Day)
    .WriteTo.PostgreSQL(
        connectionString: postgresConnectionString,
        tableName: "Logs",
        columnOptions: columnWriters,
        needAutoCreateTable: true // Set to true to auto-create the table if it doesn't exist
    )
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

// Replace default logging with Serilog
builder.Host.UseSerilog();

builder.AddNpgsqlDbContext<AppDbContext>("irechargeDb2nd");

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();



builder.AddServiceDefaults();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var secretKey = builder.Configuration["JWT:Secret"]?? Environment.GetEnvironmentVariable("JWT__Secret"); ;

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})

    // Adding Jwt Bearer
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = builder.Configuration["JWT:ValidAudience"],
            ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        };
    });

builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles
);

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddAWSService<IAmazonSQS>(new AWSOptions
{
    DefaultClientConfig =
    {
        ServiceURL = "http://localhost:4566", // LocalStack endpoint
        UseHttp = true
    }
});
builder.Services.AddScoped<ISqsService, SqsService>(); 
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOrderService, OrderService>();
// Register SqsProductUpdateService
builder.Services.AddScoped<SqsProductUpdateService>();


// Register the background service
builder.Services.AddHostedService<SqsMessageProcessingService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "iRechargeUi",
                      policy =>
                      {
                          policy.WithOrigins(
                            //"http://localhost:4206"
                        ).AllowAnyMethod().AllowAnyHeader();
                      });
});


var app = builder.Build();
app.MapDefaultEndpoints();
if (app.Environment.IsDevelopment())
{
    //await app.ConfigureDatabaseAsync();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";
        var error = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();
        if (error != null)
        {
            var ex = error.Error;
            var result = System.Text.Json.JsonSerializer.Serialize(new { error = ex.Message });
            await context.Response.WriteAsync(result);
        }
    });
});

app.UseHttpsRedirection();

app.UseCors("iRechargeUi"); // Apply CORS
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    iRechargeTestProject.Infrastructure.DbSeeder.Seed(dbContext);
}

app.Run();