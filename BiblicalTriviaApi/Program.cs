var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure TriviaService
builder.Services.AddSingleton(provider => 
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var geminiApiKey = Environment.GetEnvironmentVariable("GEMINI_API_KEY") 
        ?? throw new InvalidOperationException("GeminiApiKey is not configured");
    var logger = provider.GetRequiredService<ILogger<BiblicalTriviaApi.Services.TriviaService>>();
    return new BiblicalTriviaApi.Services.TriviaService(geminiApiKey, logger);
});

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
