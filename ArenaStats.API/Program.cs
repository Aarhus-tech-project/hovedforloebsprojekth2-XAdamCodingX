using ArenaStats.API.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
    c.SwaggerDoc("v1", new() { Title = "Arena Stats API", Version = "v1" }));

// Database
builder.Services.AddSingleton<IDbConnectionFactory, SqlConnectionFactory>();

// Repositories
builder.Services.AddScoped<IHeroRepository,  HeroRepository>();
builder.Services.AddScoped<IMapRepository,   MapRepository>();
builder.Services.AddScoped<IMatchRepository, MatchRepository>();

// CORS — allow the Blazor project (adjust port if needed)
var origins = builder.Configuration
    .GetSection("Cors:AllowedOrigins").Get<string[]>()
    ?? ["https://localhost:7001"];

builder.Services.AddCors(o => o.AddPolicy("BlazorApp",
    p => p.WithOrigins(origins).AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Arena Stats v1"));

app.UseCors("BlazorApp");
app.MapControllers();
app.Run();
