//using ContactSystem.Aplication.Dtos;
//using ContactSystem.Aplication.Services;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;

//namespace ContactSystem.Server.Controllers;

//[Route("api/contact")]
//[ApiController]
//public class ContactController : ControllerBase
//{
//    private readonly IContactService _contactService;

//    public ContactController(IContactService contactService)
//    {
//        _contactService = contactService;
//    }

//    [HttpPost("add")]
//    public async Task<long> Post(ContactCreateDto contactCreateDto)
//    {
//        return await _contactService.PostAsync(contactCreateDto);
//    }

//    [HttpGet("getAll")]
//    public ICollection<ContactDto> GetAll()
//    {
//        return _contactService.GetAll();
//    }
//}
