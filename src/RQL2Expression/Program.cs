using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using RQL2Expression.Core.Abstract;
using RQL2Expression.Core.Options;
using RQL2Expression.Core.Service;
using RQL2Expression.Data.Context;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSingleton<IRqlAttributeMapper, RqlAttributeMapper>();
builder.Services.AddSingleton<IRqlToExpressionService, RqlToExpressionService>();
builder.Services.Configure<RqlAttributeOptions>(builder.Configuration.GetSection(RqlAttributeOptions.OptionName));

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Account Search API", Version = "v1" });
});

// Add HealthCheck
builder.Services.AddHealthChecks();
    //.AddDbContextCheck<AppDbContext>();

var app = builder.Build();

// turn on Swagger в development-mode
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Account Search API v1");
    });
}

// Add HealthCheck endpoint
app.MapHealthChecks("/hc");

app.MapGet("/search", async (
    [FromQuery] string rql,
    AppDbContext dbContext,
    IRqlToExpressionService rqlConverter ) =>
{
    if (string.IsNullOrEmpty(rql))
    {
        return Results.BadRequest("RQL query is required");
    }

    if (string.IsNullOrEmpty(rql))
    {
        return Results.BadRequest("RQL query is required");
    }

    var expression = rqlConverter.ParseRqlToExpression(rql);
    var accounts = await dbContext.Accounts.Where(expression).ToListAsync();

    return Results.Ok(accounts);
});

app.Run();