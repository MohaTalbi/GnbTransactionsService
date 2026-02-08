using GnbTransactionsService.Application.Services;
using GnbTransactionsService.Infrastructure.Middleware;
using GnbTransactionsService.Infrastructure.Repositories;
using GnbTransactionsService.Infrastructure.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Paths a los JSON
var dataPath = Path.Combine(builder.Environment.ContentRootPath, "Infrastructure", "Data");

var transactionsPath = Path.Combine(dataPath, "transactions.json");
var ratesPath = Path.Combine(dataPath, "rates.json");

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

//Alternative,- desirable, + coupling
//RateRepository rateRepository = new RateRepository(ratesPath);
//TransactionRepository transactionRepository = new TransactionRepository(transactionsPath);
//RateService rateService = new RateService(rateRepository);
//CurrencyConverterService currencyConverter = rateService.BuildCurrencyConverter();
//TransactionService transactionService = new TransactionService(transactionRepository, currencyConverter, null);

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
