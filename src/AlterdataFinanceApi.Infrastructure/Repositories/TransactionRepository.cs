using AlterdataFinanceApi.Domain.Entities;
using AlterdataFinanceApi.Domain.Enums;
using AlterdataFinanceApi.Domain.Interfaces;
using AlterdataFinanceApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AlterdataFinanceApi.Infrastructure.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly AppDbContext _context;

    public TransactionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Transaction?> GetByIdAsync(Guid id)
    {
        return await _context.Transactions.FindAsync(id);
    }

    public async Task<IEnumerable<Transaction>> GetAllAsync()
    {
        return await _context.Transactions
            .OrderByDescending(t => t.Date)
            .ToListAsync();
    }

    public async Task<IEnumerable<Transaction>> GetByPeriodAsync(DateTime startDate, DateTime endDate, TransactionType? type)
    {
        var query = _context.Transactions
            .Where(t => t.Date >= startDate && t.Date <= endDate);

        if (type.HasValue)
            query = query.Where(t => t.Type == type.Value);

        return await query
            .OrderByDescending(t => t.Date)
            .ToListAsync();
    }

    public async Task<Transaction> AddAsync(Transaction transaction)
    {
        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync();
        return transaction;
    }

    public async Task<Transaction> UpdateAsync(Transaction transaction)
    {
        _context.Transactions.Update(transaction);
        await _context.SaveChangesAsync();
        return transaction;
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _context.Transactions.FindAsync(id);
        if (entity is not null)
        {
            _context.Transactions.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
