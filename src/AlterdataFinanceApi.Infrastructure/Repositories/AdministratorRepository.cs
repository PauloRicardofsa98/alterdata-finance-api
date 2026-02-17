using AlterdataFinanceApi.Domain.Entities;
using AlterdataFinanceApi.Domain.Interfaces;
using AlterdataFinanceApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AlterdataFinanceApi.Infrastructure.Repositories;

public class AdministratorRepository : IAdministratorRepository
{
    private readonly AppDbContext _context;

    public AdministratorRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Administrator?> GetByIdAsync(Guid id)
    {
        return await _context.Administrators.FindAsync(id);
    }

    public async Task<Administrator?> GetByEmailAsync(string email)
    {
        return await _context.Administrators
            .FirstOrDefaultAsync(a => a.Email.ToLower() == email.ToLower());
    }

    public async Task<IEnumerable<Administrator>> GetAllAsync()
    {
        return await _context.Administrators
            .OrderBy(a => a.Name)
            .ToListAsync();
    }

    public async Task<Administrator> AddAsync(Administrator administrator)
    {
        _context.Administrators.Add(administrator);
        await _context.SaveChangesAsync();
        return administrator;
    }

    public async Task<Administrator> UpdateAsync(Administrator administrator)
    {
        _context.Administrators.Update(administrator);
        await _context.SaveChangesAsync();
        return administrator;
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _context.Administrators.FindAsync(id);
        if (entity is not null)
        {
            _context.Administrators.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
