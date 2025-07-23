using CesiZen.Data.Models;
using CesiZen.Ui.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace TestProject;
public class AccountControllerTests
{
    private readonly Mock<UserManager<UserModel>> _userManagerMock;
    private readonly Mock<SignInManager<UserModel>> _signInManagerMock;
    private readonly AccountController _controller;

    public AccountControllerTests()
    {
        var userStore = new Mock<IUserStore<UserModel>>();
        _userManagerMock = new Mock<UserManager<UserModel>>(
            userStore.Object, null, null, null, null, null, null, null, null
        );

        var contextAccessor = new Mock<Microsoft.AspNetCore.Http.IHttpContextAccessor>();
        var userPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<UserModel>>();

        _signInManagerMock = new Mock<SignInManager<UserModel>>(
            _userManagerMock.Object,
            contextAccessor.Object,
            userPrincipalFactory.Object,
            null, null, null, null
        );

        _controller = new AccountController(_userManagerMock.Object, _signInManagerMock.Object);
    }

    // --- Test Register (succès) ---
    [Fact]
    public async Task Register_ValidModel_ShouldRedirectToHome()
    {
        // Arrange
        var model = new RegisterViewModel
        {
            UserName = "testuser",
            Email = "test@test.com",
            Password = "Password123!",
            FirstName = "John",
            LastName = "Doe"
        };

        _userManagerMock
            .Setup(x => x.CreateAsync(It.IsAny<UserModel>(), model.Password))
            .ReturnsAsync(IdentityResult.Success);

        _signInManagerMock
            .Setup(x => x.SignInAsync(It.IsAny<UserModel>(), false, null))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Register(model);

        // Assert
        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirect.ActionName);
        Assert.Equal("Home", redirect.ControllerName);
    }

    // --- Test Register (échec) ---
    [Fact]
    public async Task Register_InvalidCreate_ShouldReturnViewWithErrors()
    {
        // Arrange
        var model = new RegisterViewModel
        {
            UserName = "testuser",
            Email = "test@test.com",
            Password = "Password123!"
        };

        _userManagerMock
            .Setup(x => x.CreateAsync(It.IsAny<UserModel>(), model.Password))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Erreur" }));

        // Act
        var result = await _controller.Register(model);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(model, viewResult.Model);
        Assert.False(_controller.ModelState.IsValid);
    }

    // --- Test Login (succès) ---
    [Fact]
    public async Task Login_ValidCredentials_ShouldRedirectToHome()
    {
        // Arrange
        var model = new LoginViewModel
        {
            UserNameOrEmail = "testuser",
            Password = "Password123!",
            RememberMe = false
        };

        var user = new UserModel { UserName = "testuser", Active = true };

        _userManagerMock
            .Setup(x => x.FindByNameAsync(model.UserNameOrEmail))
            .ReturnsAsync(user);

        _signInManagerMock
            .Setup(x => x.PasswordSignInAsync(user, model.Password, model.RememberMe, true))
            .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

        // Act
        var result = await _controller.Login(model);

        // Assert
        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirect.ActionName);
        Assert.Equal("Home", redirect.ControllerName);
    }

    // --- Test Login (utilisateur non trouvé) ---
    [Fact]
    public async Task Login_UserNotFound_ShouldReturnViewWithError()
    {
        // Arrange
        var model = new LoginViewModel
        {
            UserNameOrEmail = "unknown",
            Password = "Password123!"
        };

        _userManagerMock
            .Setup(x => x.FindByNameAsync(model.UserNameOrEmail))
            .ReturnsAsync((UserModel?)null);

        // Act
        var result = await _controller.Login(model);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(model, viewResult.Model);
        Assert.False(_controller.ModelState.IsValid);
    }
}

