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

    public async Task DeleteAsync(long contactId)
    {
        var contact = await _contactRepository.SelectByIdAsync(contactId);
        if (contact is null)
        {
            throw new Exception($"Contact not found to delete with id: {contactId}");
        }

        await _contactRepository.DeleteAsync(contact);
    }

    public ICollection<ContactDto> GetAll()
    {
        var query = _contactRepository.SelectAll();
        var contacts = query.ToList();

        var contactDtos = contacts.Select(c => MapService.ConvertToContactDto(c)).ToList();
        return contactDtos;
    }

    public async Task<ContactDto> GetByIdAsync(long contactId)
    {
        var contact = await _contactRepository.SelectByIdAsync(contactId);
        if (contact is null)
        {
            throw new Exception($"Contact not found with id: {contactId}");
        }

        return MapService.ConvertToContactDto(contact);
    }

    public async Task<long> PostAsync(ContactCreateDto contact)
    {
        var contactToDB = MapService.ConvertToContactEntity(contact);
        var contactId = await _contactRepository.InsertAsync(contactToDB);
        return contactId;
    }

    public async Task UpdateAsync(ContactDto contactDto)
    {
        var contact = await _contactRepository.SelectByIdAsync(contactDto.ContactId);
        if (contact is null)
        {
            throw new Exception($"Contact not found to update with id: {contactDto.ContactId}");
        }

        contact.Name = contactDto.Name;
        contact.Address = contactDto.Address;
        contact.Phone = contactDto.Phone;
        contact.Email = contactDto.Email;

        await _contactRepository.UpdateAsync(contact);
    }
}
