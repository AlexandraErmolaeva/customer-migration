using DotNetEnv;
using System.Reflection;
using WebApi.Configurations;
using Application;
using CustomerMigrationApi.Services.Middlewares;
using Infrastructure;
using CustomerMigrationApi.Confugirations;
using Infrastructure.Persistence;

Env.TraversePath().Load();

var assemblyName = Assembly.GetExecutingAssembly().GetName();
var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

builder.AddLogger();

services.AddSwagger(assemblyName.Name!);
services.AddControllers();
services.AddApplicationServices();
services.AddInfrastrucureServices(builder.Configuration, builder.Environment.IsDevelopment());

services.AddCors(options =>
{
    var origins = builder.Configuration.GetSection("AllowedOrigins").Get<string>();
    options.AddDefaultPolicy(builder =>
    {
        builder
        .WithOrigins(origins!)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
    });
});

var app = builder.Build();

await app.ApplyMigrationsAsync(builder.Configuration);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseMiddleware<ExceptionMiddleware>();

app.UseRouting();
app.UseCors();
app.MapControllers();
app.Run();
