using Microsoft.EntityFrameworkCore;
using MikEBank.Api.Data;
using MikEBank.Api.Models;

namespace MikEBank.Api.Repositories;

public sealed class UserRepository(AppDbContext db)
{
    public Task<List<User>> GetAllAsync(CancellationToken cancellationToken) =>
        db.Users.ToListAsync(cancellationToken);

    public async Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken) =>
        await db.Users.FindAsync([id], cancellationToken);

    public void Add(User user) => db.Users.Add(user);

    public void Remove(User user) => db.Users.Remove(user);

    public async Task SaveChangesAsync(CancellationToken cancellationToken) =>
        await db.SaveChangesAsync(cancellationToken);
}
