using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PlanWiseApi.Data;
using PlanWiseApi.Models;
using PlanWiseApi.Models.Config;
using PlanWiseApi.Services;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "JwtBearer";
    options.DefaultChallengeScheme = "JwtBearer";
})
.AddJwtBearer("JwtBearer", options =>
{
    var key = builder.Configuration["Jwt:Key"];

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        NameClaimType = ClaimTypes.Email,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(key!)
        )
    };
});


// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());
});


// Services
builder.Services.AddScoped<IAuthService, AuthService>();


// Azure Configurations
builder.Services.Configure<AzureOpenAIConfig>(
    builder.Configuration.GetSection("AzureOpenAI"));

builder.Services.Configure<AzureAISearchConfig>(
    builder.Configuration.GetSection("AzureAISearch"));


// HttpClient for AI services
builder.Services.AddHttpClient();


// AI Services
builder.Services.AddScoped<EmbeddingService>();
builder.Services.AddScoped<SearchService>();
builder.Services.AddScoped<ChatService>();
builder.Services.AddScoped<TemplateIndexService>();
builder.Services.AddScoped<UserActivityService>();


var app = builder.Build();


// Swagger
app.UseSwagger();
app.UseSwaggerUI();



app.UseHttpsRedirection();
app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


// Seed Admin User
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    var adminExists = await db.Users.AnyAsync(u => u.Email == "admin@planwise.com");

    if (!adminExists)
    {
        db.Users.Add(new User
        {
            Name = "Admin",
            Email = "admin@planwise.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
            Role = "Admin",
            CreatedAt = DateTime.UtcNow
        });

        await db.SaveChangesAsync();
    }
}

using (var scope = app.Services.CreateScope())
{
    var searchService = scope.ServiceProvider.GetRequiredService<SearchService>();
    await searchService.EnsureIndexAsync();
}

app.Run();