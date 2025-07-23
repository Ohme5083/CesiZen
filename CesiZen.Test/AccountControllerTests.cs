using CesiZen.Data.Models;
using CesiZen.Ui.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace CesiZen.Test;
public class AccountControllerTests
{
    private readonly Mock<UserManager<UserModel>> _userManagerMock;
    private readonly Mock<SignInManager<UserModel>> _signInManagerMock;
    private readonly AccountController _controller;

    public AccountControllerTests()
    {
        var userStoreMock = new Mock<IUserStore<UserModel>>();
        _userManagerMock = new Mock<UserManager<UserModel>>(
            userStoreMock.Object,
            new Mock<IOptions<IdentityOptions>>().Object,
            new Mock<IPasswordHasher<UserModel>>().Object,
            new IUserValidator<UserModel>[0],
            new IPasswordValidator<UserModel>[0],
            new Mock<ILookupNormalizer>().Object,
            new IdentityErrorDescriber(),
            new Mock<IServiceProvider>().Object,
            new Mock<ILogger<UserManager<UserModel>>>().Object
        );

        _signInManagerMock = new Mock<SignInManager<UserModel>>(
            _userManagerMock.Object,
            new Mock<IHttpContextAccessor>().Object,
            new Mock<IUserClaimsPrincipalFactory<UserModel>>().Object,
            new Mock<IOptions<IdentityOptions>>().Object,
            new Mock<ILogger<SignInManager<UserModel>>>().Object,
            new Mock<IAuthenticationSchemeProvider>().Object,
            new Mock<IUserConfirmation<UserModel>>().Object
        );

        _controller = new AccountController(_userManagerMock.Object, _signInManagerMock.Object);
    }

    [Fact]
    public async Task Register_ValidModel_RedirectsToHomeIndex()
    {
        // Arrange
        var model = new RegisterViewModel
        {
            UserName = "testuser",
            Email = "test@example.com",
            Password = "Test123!",
            FirstName = "Test",
            LastName = "User"
        };

        _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<UserModel>(), model.Password))
                        .ReturnsAsync(IdentityResult.Success);

        _signInManagerMock.Setup(x => x.SignInAsync(It.IsAny<UserModel>(), false, null))
                          .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Register(model);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
        Assert.Equal("Home", redirectResult.ControllerName);
    }

    [Fact]
    public async Task Register_InvalidModel_ReturnsViewWithModel()
    {
        // Arrange
        var model = new RegisterViewModel(); // Vide = invalid
        _controller.ModelState.AddModelError("UserName", "Required");

        // Act
        var result = await _controller.Register(model);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(model, viewResult.Model);
    }

    [Fact]
    public async Task Register_UserCreationFails_AddsErrorsToModelState()
    {
        // Arrange
        var model = new RegisterViewModel
        {
            UserName = "failuser",
            Email = "fail@example.com",
            Password = "Fail123!",
            FirstName = "Fail",
            LastName = "User"
        };

        var identityErrors = new List<IdentityError>
        {
            new IdentityError { Description = "Password is too weak." }
        };

        _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<UserModel>(), model.Password))
                        .ReturnsAsync(IdentityResult.Failed(identityErrors.ToArray()));

        // Act
        var result = await _controller.Register(model);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(model, viewResult.Model);
        Assert.False(_controller.ModelState.IsValid);
        Assert.True(_controller.ModelState.ContainsKey(""));
    }
}
