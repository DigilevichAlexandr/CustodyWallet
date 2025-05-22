using CustodyWallet.DTOs;
using CustodyWallet.Models;
using CustodyWallet.Data;
using Microsoft.EntityFrameworkCore;

namespace CustodyWallet.Services;

public class UserService
{
    private readonly AppDbContext _context;
    public UserService(AppDbContext context) => _context = context;

    public async Task<BalanceDto> CreateUserAsync(CreateUserDto dto)
    {
        var user = new User { Id = Guid.NewGuid(), Email = dto.Email };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return new BalanceDto { UserId = user.Id, Balance = user.Balance };
    }

    public async Task<BalanceDto> GetBalanceAsync(Guid userId)
    {
        var user = await _context.Users.FindAsync(userId) ?? throw new Exception("User not found");
        return new BalanceDto { UserId = user.Id, Balance = user.Balance };
    }

    public async Task<BalanceDto> DepositAsync(Guid userId, TransactionDto dto)
    {
        if (dto.Amount <= 0) throw new Exception("Amount must be greater than 0");
        var user = await _context.Users.FindAsync(userId) ?? throw new Exception("User not found");
        user.Balance += dto.Amount;
        await _context.SaveChangesAsync();
        return new BalanceDto { UserId = user.Id, Balance = user.Balance };
    }

    public async Task<(bool IsSuccess, BalanceDto? Value, string? Error)> WithdrawAsync(Guid userId, TransactionDto dto)
    {
        if (dto.Amount <= 0) return (false, null, "Amount must be greater than 0");
        var user = await _context.Users.FindAsync(userId);
        if (user == null) return (false, null, "User not found");
        if (user.Balance < dto.Amount) return (false, null, "Insufficient funds");
        user.Balance -= dto.Amount;
        await _context.SaveChangesAsync();
        return (true, new BalanceDto { UserId = user.Id, Balance = user.Balance }, null);
    }
}