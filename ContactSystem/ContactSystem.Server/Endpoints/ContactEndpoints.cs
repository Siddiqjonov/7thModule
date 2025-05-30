using ContactSystem.Aplication.Dtos;
using ContactSystem.Aplication.Services;
using ContactSystem.Domain.Entities;

namespace ContactSystem.Server.Endpoints;

public static class ContactEndpoints
{
    public static void MapContactEndpoints(this WebApplication app)
    {
        var contactGroup = app.MapGroup("api/contact")
            //.RequireAuthorization()
            .WithTags("Contacts");

        //contactGroup.MapDelete("/delete", DeleteAsync)
        //    .WithName("DeleteContact").
        //Produces(200).
        //Produces(404);

        // Delete contact
        contactGroup.MapDelete("/delete",
            async (long contactId, IContactService _contactService) =>
            {
                await _contactService.DeleteAsync(contactId);
                return Results.Ok();
            }).
        WithName("DeleteContact").
        Produces(200).
        Produces(404);

        // Update contact
        contactGroup.MapPut("/update",
            async (ContactDto contactDto, IContactService _contactService) =>
            {
                await _contactService.UpdateAsync(contactDto);
            }).
        WithName("UpdateContact").
        Produces(200).
        Produces(404).
        Produces(422);

        // Post contact
        contactGroup.MapPost("/add",
            async (ContactCreateDto contactCreateDeo, IContactService _contactService) =>
            {
                return await _contactService.PostAsync(contactCreateDeo);
            }).
        WithName("PostContact").
        Produces(200).
        Produces(422);

        // Get contact by id
        contactGroup.MapGet("/get{contactId}",
            async (long contactId, IContactService _contactService) =>
            {
                return await _contactService.GetByIdAsync(contactId);
            }).
        WithName("GetContactById").
        Produces(200).
        Produces(404);

        // Get contacts
        contactGroup.MapGet("/get-all",
            (IContactService _contactService) =>
            {
                return _contactService.GetAll();
            }).
        WithName("GetAllContacts").
        Produces(200).
        Produces(404);
    }

    public static async Task<IResult> DeleteAsync(long contactId, IContactService _contactService)
    {
        await _contactService.DeleteAsync(contactId);
        return Results.Ok();
    }
}
