using ContactSystem.Aplication.Interfaces;
using ContactSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContactSystem.Infrastructure.Persistence.Repositories;

public class ContactRepository : IContactRepository
{
    private readonly AppDbContext _appDbContext;

    public ContactRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task DeleteAsync(Contact contact)
    {
        _appDbContext.Contacts.Remove(contact);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task<long> InsertAsync(Contact contact)
    {
        await _appDbContext.Contacts.AddAsync(contact);
        await _appDbContext.SaveChangesAsync();
        return contact.ContactId;
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _appDbContext.SaveChangesAsync();
    }

    public IQueryable<Contact> SelectAll()
    {
        return _appDbContext.Contacts.AsQueryable();
    }

    public async Task<Contact?> SelectByIdAsync(long contactId)
    {
        var contact = await _appDbContext.Contacts.FirstOrDefaultAsync(c => c.ContactId == contactId);
        return contact;
    }

    public async Task UpdateAsync(Contact contact)
    {
        _appDbContext.Contacts.Update(contact);
        await _appDbContext.SaveChangesAsync();
    }
}
