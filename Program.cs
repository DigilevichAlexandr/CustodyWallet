using CustodyWallet.Data;
using CustodyWallet.DTOs;
using CustodyWallet.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<UserService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapPost("/api/users", async (CreateUserDto dto, UserService service) =>
{
    var result = await service.CreateUserAsync(dto);
    return Results.Ok(result);
});

app.MapGet("/api/users/{userId:guid}/balance", async (Guid userId, UserService service) =>
{
    try
    {
        var result = await service.GetBalanceAsync(userId);
        return Results.Ok(result);
    }
    catch (Exception e)
    {
        return Results.NotFound(new { error = e.Message });
    }
});

app.MapPost("/api/users/{userId:guid}/deposit", async (Guid userId, TransactionDto dto, UserService service) =>
{
    try
    {
        var result = await service.DepositAsync(userId, dto);
        return Results.Ok(result);
    }
    catch (Exception e)
    {
        return Results.BadRequest(new { error = e.Message });
    }
});

app.MapPost("/api/users/{userId:guid}/withdraw", async (Guid userId, TransactionDto dto, UserService service) =>
{
    var result = await service.WithdrawAsync(userId, dto);
    return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(new { error = result.Error });
});

app.Run();