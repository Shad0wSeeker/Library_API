using Library.Application.Interfaces;
using Library.Application.Mapper;
using Library.Application.Services;
using Library.Domain.Interfaces;
using Library.Infrastructure;
using Library.Application;
using Library.Infrastructure.Data;
using Library.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Library.Domain.Models;
using Microsoft.OpenApi.Models;
using FluentValidation.AspNetCore;
using Library.Application.Validators;
using LibraryAPI.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddFluentValidation(fv => {
        fv.RegisterValidatorsFromAssembly(typeof(AuthorDtoValidator).Assembly);
        fv.RegisterValidatorsFromAssembly(typeof(BookDtoValidator).Assembly);
        fv.RegisterValidatorsFromAssembly(typeof(UserDtoValidator).Assembly);
    });

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddApplicationServices();

var key = Encoding.ASCII.GetBytes(builder.Configuration["JwtSettings:Secret"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var token = context.Request.Headers["Authorization"].ToString();
            if (token.StartsWith("Bearer "))
            {
                token = token.Substring("Bearer ".Length).Trim();
            }
            context.Token = token; // Устанавливаем токен
            return Task.CompletedTask;
        }
        
    };
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        ClockSkew = TimeSpan.Zero
    };

});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Library API", Version = "v1" });

    options.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        In = ParameterLocation.Header,
        Description = "Enter your token in the format `{token}`"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole(UserRole.Admin.ToString()));
    options.AddPolicy("ClientPolicy", policy => policy.RequireRole(UserRole.Client.ToString()));
    options.AddPolicy("AdminAndClientPolicy", policy => policy.RequireAssertion(context =>
            context.User.IsInRole(UserRole.Admin.ToString()) ||
            context.User.IsInRole(UserRole.Client.ToString())));
});

var connStr = builder.Configuration.GetConnectionString("SqliteConnection");
string dataDirectory = String.Empty;
connStr = String.Format(connStr, dataDirectory);

var options = new DbContextOptionsBuilder<AppDbContext>().UseSqlite(connStr).Options;
builder.Services.AddScoped<AppDbContext>(s => new AppDbContext(options));

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

// Инициализация базы данных
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    DBInitializer.SeedData(dbContext).Wait();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
