var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReact", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


builder.Services.AddSingleton<IGameService, GameService>();
builder.Services.AddSingleton<IWordRepository, WordRepository>();
builder.Services.AddSingleton<IGameRepository, InMemoryGameRepository>();

var app = builder.Build();

// Middleware
app.UseCors("AllowReact");

// Midlertidigt kommenteret for HTTP-test
// app.UseHttpsRedirection();

// Controller endpoints
app.MapControllers();

// OpenAPI kun i dev
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.Run();