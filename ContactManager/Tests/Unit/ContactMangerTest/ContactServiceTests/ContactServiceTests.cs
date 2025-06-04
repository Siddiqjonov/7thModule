using ContactManager.Application.Interfaces;
using ContactManager.Application.Services;
using ContactManager.Domain.Entities;
using ContactManager.Domain.Errors;
using ContactManager.Infrastructure.Persistence.Repositories;
using Moq;
using Xunit;

namespace ContactMangerTest.ContactServiceTests;

public class ContactServiceTests
{
    private readonly Mock<IContactRepository> _mockRepo;
    private readonly ContactService _service;

    public ContactServiceTests()
    {
        _mockRepo = new Mock<IContactRepository>();
        _service = new ContactService(_mockRepo.Object);
    }

    //[Fact]
    //public async Task DeleteAsync_ContactExists_DeletesContact()
    //{
    //    // Arrange
    //    var contactId = 1L;
    //    var contact = new Contact { ContactId = contactId };
    //    _mockRepo.Setup(r => r.SelectContactByContactIdAsync(contactId)).ReturnsAsync(contact);

    //    // Act
    //    await _service.DeleteContactAsync(contactId, 6);

    //    // Assert
    //    _mockRepo.Verify(r => r.DeleteAsync(contact), Times.Once);
    //}

    //[Fact]
    //public async Task DeleteContactAsync_ValidUserAndContact_DeletesContact()
    //{
    //    // Arrange
    //    var contactId = 1L;
    //    var userId = 100L;
    //    var contact = new Contact { ContactId = contactId, UserId = userId };

    //    _mockRepo.Setup(r => r.SelectAllContacts())
    //            .Returns(new List<Contact> { contact });

    //    // Act
    //    await _service.DeleteContactAsync(contactId, userId);

    //    // Assert
    //    _mockRepo.Verify(r => r.DeleteContactAsync(contact), Times.Once);
    //}

    //[Fact]
    //public async Task DeleteContactAsync_ValidUserAndContact_DeletesContact()
    //{
    //    // Arrange
    //    var contactId = 1L;
    //    var userId = 100L;
    //    var contact = new Contact { ContactId = contactId, UserId = userId };

    //    _mockRepo.Setup(r => r.SelectAllContacts())
    //             .Returns(new List<Contact> { contact });

    //    // Act
    //    await _service.DeleteContactAsync(contactId, userId);

    //    // Assert
    //    _mockRepo.Verify(r => r.DeleteContactAsync(contact), Times.Once);
    //}

    [Fact]
    public async Task DeleteContactAsync_ValidContact_CallsRepositoryDelete()
    {
        // Arrange
        var contactId = 1L;
        var userId = 10L;

        var contact = new Contact { ContactId = contactId, UserId = userId };

        _mockRepo.Setup(r => r.SelectAllContacts())
                 .Returns(new List<Contact> { contact }.AsQueryable());

        _mockRepo.Setup(r => r.DeleteContactAsync(contact))
                 .Returns(Task.CompletedTask);

        // Act
        await _service.DeleteContactAsync(contactId, userId); // ✅ Call the service method

        // Assert
        _mockRepo.Verify(r => r.DeleteContactAsync(contact), Times.Once); // ✅ Verify repo was used
        
    }

    [Fact]
    public async Task DeleteContactAsync_ContactNotFound_ThrowsInvalidArgumentException()
    {
        // Arrange
        var contactId = 1L;
        var userId = 10L;
        var contact = new Contact { ContactId = contactId, UserId = userId };


        _mockRepo.Setup(r => r.SelectAllContacts())
                 .Returns(new List<Contact>().AsQueryable()); // Empty list, no contact found

        // Act & Assert
        await Assert.ThrowsAsync<InvalidArgumentException>(() =>
            _service.DeleteContactAsync(contactId, userId)); // ✅ Test exception is thrown
    }

}
