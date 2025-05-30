using ContactManager.Application.Dtos;
using ContactManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManager.Application.Services;

public static class MapService
{
    public static Contact ConvertToContactEntity(ContactCreateDto dto)
    {
        return new Contact()
        {
            FirstName = dto.FirstName,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            LastName = dto.LastName,
            Address = dto.Address,
        };
    }

    public static ContactDto ConvertToContactDto(Contact contact)
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
    public static ContactDto ConvertToContactGetDto(Contact contact)
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
}
