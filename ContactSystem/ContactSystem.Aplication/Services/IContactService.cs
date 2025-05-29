using ContactSystem.Aplication.Dtos;
using ContactSystem.Domain.Entities;

namespace ContactSystem.Aplication.Services;

public interface IContactService
{
    Task<long> PostAsync(ContactCreateDto contact);
    Task DeleteAsync(long contactId);
    Task<ContactDto> GetByIdAsync(long contactId);
    ICollection<ContactDto> GetAll();
    Task UpdateAsync(ContactDto contact);
}