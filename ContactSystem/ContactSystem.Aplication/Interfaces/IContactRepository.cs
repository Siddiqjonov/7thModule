using ContactSystem.Domain.Entities;

namespace ContactSystem.Aplication.Interfaces;

public interface IContactRepository
{
    Task<long> InsertAsync(Contact contact);
    Task DeleteAsync(long contactId);
    Task<Contact> SelectByIdAsync(long contactId);
    IQueryable<Contact> SelectAll();
    Task UpdateAsync(Contact contact);
}
