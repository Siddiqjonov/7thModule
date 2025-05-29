using ContactSystem.Aplication.Dtos;
using ContactSystem.Aplication.Interfaces;
using ContactSystem.Domain.Entities;

namespace ContactSystem.Aplication.Services;

public class ContactService : IContactService
{
    private readonly IContactRepository _contactRepository;

    public ContactService(IContactRepository contactRepository)
    {
        _contactRepository = contactRepository;
    }

    public Task DeleteAsync(long contactId)
    {
        throw new NotImplementedException();
    }

    public ICollection<ContactDto> GetAll()
    {
        var query = _contactRepository.SelectAll();
        var contacts = query.ToList();

        var contactDtos = contacts.Select(c => MapService.ConvertToContactDto(c)).ToList();
        return contactDtos;
    }

    public Task<ContactDto> GetByIdAsync(long contactId)
    {
        throw new NotImplementedException();
    }

    public async Task<long> PostAsync(ContactCreateDto contact)
    {
        var contactToDB = MapService.ConvertToContactEntity(contact);
        var contactId = await _contactRepository.InsertAsync(contactToDB);
        return contactId;
    }

    public Task UpdateAsync(ContactDto contact)
    {
        throw new NotImplementedException();
    }
}
