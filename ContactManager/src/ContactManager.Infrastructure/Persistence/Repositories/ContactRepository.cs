using ContactManager.Application.Interfaces;
using ContactManager.Domain.Entities;
using ContactManager.Domain.Errors;
using Microsoft.EntityFrameworkCore;

namespace ContactManager.Infrastructure.Persistence.Repositories;

public class ContactRepository : IContactRepository
{
    private readonly MainContext MainContext;

    public ContactRepository(MainContext mainContext)
    {
        MainContext = mainContext;
    }

    public async Task<int> ContactTotalCount()
    {
        var totalCount = await MainContext.Contacts.CountAsync();
        return totalCount;
    }

    public async Task DeleteContactAsync(Contact contact)
    {
        MainContext.Contacts.Remove(contact);
        await MainContext.SaveChangesAsync();
    }

    public async Task<long> InsertContactAsync(Contact contact)
    {
        await MainContext.Contacts.AddAsync(contact);
        await MainContext.SaveChangesAsync();
        return contact.ContactId;
    }

    public IQueryable<Contact> SelectAllContacts()
    {
        return MainContext.Contacts.Include(c => c.User);
    }

    public async Task<ICollection<Contact>> SelectAllUserContactsAsync(long userId)
    {
        var contacts = await MainContext.Contacts.Where(c => c.UserId == userId).ToListAsync();
        return contacts;
    }

    public async Task<Contact> SelectContactByContactIdAsync(long contactId)
    {
        var contact = await MainContext.Contacts.Include(c => c.User).FirstOrDefaultAsync(c => c.ContactId == contactId);
        return contact == null ? throw new EntityNotFoundException($"Contact wiht contactId {contactId} not found") : contact;
    }

    public async Task UpdateContactAsync(Contact contact)
    {
        var contactFronDb = await SelectContactByContactIdAsync(contact.ContactId);
        MainContext.Contacts.Update(contactFronDb);
        await MainContext.SaveChangesAsync();
    }
}
