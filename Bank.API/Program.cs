using Bank.Domain.Models;
using Bank.Domain.Services;
using Bank.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<Bank.Infrastructure.DbContext.BankDbContext>();
builder.Services.AddTransient<Bank.Domain.Services.IRepository<Account>, DbBasedRepository<Account>>();
builder.Services.AddTransient<Bank.Domain.Services.IRepository<Deposit>, DbBasedRepository<Deposit>>();
builder.Services.AddTransient<Bank.Domain.Services.IRepository<Customer>, DbBasedRepository<Customer>>();
builder.Services.AddTransient<Bank.Domain.Services.IRepository<Withdrawal>, DbBasedRepository<Withdrawal>>();

builder.Services.AddScoped<IValidator, Bank.Domain.Services.Validator>();

builder.Services.AddTransient<Bank.Domain.Services.AccountService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();
builder.Services.AddAuthorization();
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
