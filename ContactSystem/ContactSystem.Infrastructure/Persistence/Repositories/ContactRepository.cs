using ContactSystem.Aplication.Interfaces;
using ContactSystem.Domain.Entities;

namespace ContactSystem.Infrastructure.Persistence.Repositories;

public class ContactRepository : IContactRepository
{
    private readonly AppDbContext _appDbContext;

    public ContactRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public Task DeleteAsync(long contactId)
    {
        throw new NotImplementedException();
    }

    public async Task<long> InsertAsync(Contact contact)
    {
        await _appDbContext.Contacts.AddAsync(contact);
        await _appDbContext.SaveChangesAsync();
        return contact.ContactId;
    }

    public IQueryable<Contact> SelectAll()
    {
        return _appDbContext.Contacts.AsQueryable();
    }

    public Task<Contact> SelectByIdAsync(long contactId)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Contact contact)
    {
        throw new NotImplementedException();
    }
}
