using CQRS.Pattern.Common.AutoMapping;
using CQRS.Pattern.Infastructure.Data;
using CQRS.Pattern.Infastructure.Models;
using CQRS.Pattern.Infastructure.Repositories.Implementation;
using CQRS.Pattern.Infastructure.Repositories.Interfaces;
using CQRS.Pattern.Services.Implementation;
using CQRS.Pattern.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

//inject DI services into middleware

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.Load("CQRS.Pattern.Applications")));
builder.Services.AddAutoMapper(config =>
{
    config.AddProfile<MappingProfile>();
}, typeof(Program).Assembly);

//inject application code middleware
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<CQRSDbContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
/**
 * Got lazy and created a generic repo ??
 */
builder.Services.AddScoped<IGenericRepository<Payment, CQRSDbContext>, GenericRepository<Payment, CQRSDbContext>>();
//inject services layer
builder.Services.AddScoped<IStripePaymentServices, StripePaymentServices>();

//inject core services into middleware
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//cors
var MyAllowSpecificOrigins = "localhost:test";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("https://localhost:7250")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors(MyAllowSpecificOrigins);
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
