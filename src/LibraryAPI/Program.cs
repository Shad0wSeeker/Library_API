using Library.Application.Interfaces;
using Library.Application.Mapper;
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
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Library.Infrastructure.Services;
using LibraryAPI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddLibraryServices(builder.Configuration);
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddCustomAuthorization();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

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
