using ContactManager.Application.Dtos;
using ContactManager.Application.FluentValidation;
using ContactManager.Application.Interfaces;
using ContactManager.Domain.Entities;
using ContactManager.Domain.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManager.Application.Services;

public class ContactService : IContactService
{
    private readonly IContactRepository ContactRepository;

    public ContactService(IContactRepository contactRepository)
    {
        ContactRepository = contactRepository;
    }

    public async Task<long> AddContactAsync(ContactCreateDto contactCreateDto, long userId)
    {
        var contactValidator = new ContactCreateDtoValidator();
        var result = contactValidator.Validate(contactCreateDto);

        if (!result.IsValid)
        {
            var errors = "";
            foreach (var error in result.Errors)
            {
                errors = errors + "\n" + error.ErrorMessage;
            }
            throw new ValidationFailedException(errors);
        }

        var contact = new Contact()
        {
            FirstName = contactCreateDto.FirstName,
            LastName = contactCreateDto.LastName,
            FullName = contactCreateDto.FirstName + " " + contactCreateDto.LastName,
            Email = contactCreateDto.Email,
            PhoneNumber = contactCreateDto.PhoneNumber,
            Address = contactCreateDto.Address,
            CreatedAt = DateTime.UtcNow,
            UserId = userId,
        };

        var contactId = await ContactRepository.InsertContactAsync(contact);
        return contactId;
    }

    public async Task DeleteContactAsync(long contactId, long userId)
    {
        var contactOfUser = ContactRepository.SelectAllContacts()
            .FirstOrDefault(c => c.ContactId == contactId && c.UserId == userId);
        if (contactOfUser is null)
            throw new InvalidArgumentException($"Contact with contactId: {contactId} does not belong to user with userId: {userId}");
        await ContactRepository.DeleteContactAsync(contactOfUser);
    }

    public async Task<ICollection<ContactDto>> GetAllContactstAsync(long userId)
    {
        var contacts = await ContactRepository.SelectAllUserContactsAsync(userId);

        var contactsDto = contacts.Select(contact => ConvertToContactGetDto(contact));
        return contactsDto.ToList();
    }

    private ContactDto ConvertToContactDto(Contact contact)
    {
        return new ContactDto()
        {
            ContactId = contact.ContactId,
            FirstName = contact.FirstName,
            LastName = contact.LastName,
            Email = contact.Email,
            PhoneNumber = contact.PhoneNumber,
            Address = contact.Address,
        };
    }
    private ContactDto ConvertToContactGetDto(Contact contact)
    {
        return new ContactDto()
        {
            ContactId = contact.ContactId,
            FirstName = contact.FirstName,
            LastName = contact.LastName,
            FullName = contact.FullName,
            Email = contact.Email,
            PhoneNumber = contact.PhoneNumber,
            Address = contact.Address,
        };
    }

    public async Task<ContactDto> GetContactByContacIdAsync(long contactId, long userId)
    {
        var contact = await ContactRepository.SelectContactByContactIdAsync(contactId);
        var contactDto = ConvertToContactDto(contact);
        if (contact.User.UserId == userId)
            return contactDto;
        else
            throw new NotAllowedException($"Contact does not belong to user with userId: {userId}");
    }

    public async Task UpdateContactAsync(ContactDto contactDto, long userId)
    {
        var contactDtoValidator = new ContactDtoValidator();
        var result = contactDtoValidator.Validate(contactDto);

        if (!result.IsValid)
        {
            var errors = "";
            foreach (var error in result.Errors)
            {
                errors = errors + "\n" + error.ErrorMessage;
            }
            throw new ValidationFailedException(errors);
        }

        var contactOfUser = ContactRepository.SelectAllContacts()
            .FirstOrDefault(c => c.ContactId == contactDto.ContactId && c.UserId == userId);
        if (contactOfUser is null)
            throw new NotAllowedException($"Contact with contactId: {contactDto.ContactId} does not belong to user with userId: {userId}");
        //else if(await SelectAllContacts().Include(c => c.User).SingleOrDefaultAsync(c => c.UserId == userId && c.PhoneNumber != contactDto.PhoneNumber) is not null)
        //{
        //    throw new DuplicateEntryException($"Contact with phone number: {contactDto.PhoneNumber} already exsixts");
        //}
        else
        {
            contactOfUser.FirstName = contactDto.FirstName;
            contactOfUser.LastName = contactDto.LastName;
            contactOfUser.FullName = contactDto.FirstName + " " + contactDto.LastName;
            contactOfUser.Email = contactDto.Email;
            contactOfUser.PhoneNumber = contactDto.PhoneNumber;
            contactOfUser.Address = contactDto.Address;
        }

        await ContactRepository.UpdateContactAsync(contactOfUser);
    }
}
