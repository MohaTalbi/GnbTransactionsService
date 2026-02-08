using GnbTransactionsService.Application.Services;
using GnbTransactionsService.Infrastructure.Middleware;
using GnbTransactionsService.Infrastructure.Repositories;
using GnbTransactionsService.Infrastructure.Repositories.Interfaces;
using GnbTransactionsService.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// JSON Paths
string transactionsPath = PathValidator.GetRequiredFilePath(
    builder.Environment.ContentRootPath,
    builder.Configuration["DataPaths:Transactions"],
    "transactions.json"
);

string ratesPath = PathValidator.GetRequiredFilePath(
    builder.Environment.ContentRootPath,
    builder.Configuration["DataPaths:Rates"],
    "rates.json"
);

// Repositories
builder.Services.AddSingleton<ITransactionRepository>(new TransactionRepository(transactionsPath));
builder.Services.AddSingleton<IRateRepository>(new RateRepository(ratesPath));

// Services
builder.Services.AddSingleton<RateService>();
builder.Services.AddSingleton(sp =>
{
    //build CurrencyConverter used in TransactionService
    var rateService = sp.GetRequiredService<RateService>();
    return rateService.BuildCurrencyConverter();
});
builder.Services.AddSingleton<TransactionService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseMiddleware<ErrorHandlingMiddleware>();
app.MapControllers();

app.Run();
