using Domain.DTOs;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Services;
using Domain.Validators;
using FluentValidation;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// DATABASE
builder.Services.AddDbContext<PlataformaDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// HEALTH CHECKS
builder.Services.AddHealthChecks();

// REPOSITORIES
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

// SERVICES
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();

// VALIDATORS (FluentValidation)
builder.Services.AddScoped<IValidator<LoginDto>, LoginDtoValidator>();
builder.Services.AddScoped<IValidator<CadastrarAlunoDto>, CadastrarAlunoDtoValidator>();
builder.Services.AddScoped<IValidator<AlterarSenhaDto>, AlterarSenhaDtoValidator>();

// JWT AUTHENTICATION
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
            ClockSkew = TimeSpan.Zero
        };
    });

// AUTHORIZATION
builder.Services.AddAuthorization();

// SWAGGER (SEMPRE ATIVO - Development e Production)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Plataforma EAD API",
        Version = "v1",
        Description = "API para plataforma de ensino de Química e Física"
    });

    // Configurar JWT no Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header usando Bearer scheme. Exemplo: 'Bearer seu_token_aqui'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// SWAGGER SEMPRE ATIVO (Development e Production)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Plataforma EAD API V1");
    c.RoutePrefix = string.Empty; // Swagger na raiz (http://localhost:5000)
});

// HEALTH CHECK ENDPOINT
app.MapHealthChecks("/health");

// HTTPS Redirection (automático - funciona local e Docker)
if (app.Environment.IsDevelopment())
{
    // Em Development (local), use HTTPS
    app.UseHttpsRedirection();
}
// Em Production (Docker), não usa HTTPS (evita problemas)

app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

Console.WriteLine("🚀 Plataforma EAD API iniciada!");
Console.WriteLine($"📱 Environment: {app.Environment.EnvironmentName}");
Console.WriteLine("📋 Endpoints disponíveis:");
Console.WriteLine("   🏠 Swagger UI: http://localhost:5000 (raiz)");
Console.WriteLine("   📊 Swagger JSON: http://localhost:5000/swagger/v1/swagger.json");
Console.WriteLine("   ❤️  Health Check: http://localhost:5000/health");

app.Run();