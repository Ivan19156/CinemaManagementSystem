using Application.Authentication;
using Application.Interfaces.Repositories;
using CinemaManagementSystem.Core.Entities;
using CinemaManagementSystem.Infrastructure.Logging;
using CinemaManagementSystem.Infrastructure.Services;
using Contracts.DTOs.UsersDto;
using Moq;
using System.Security.Cryptography.X509Certificates;
using Xunit;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IAppLogger<UserService>> _loggerMock;
    private readonly Mock<IJwtProvider> _jwtProviderMock;
    private readonly Mock<IPasswordHasher> _passwordHasherMock;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _loggerMock = new Mock<IAppLogger<UserService>>();
        _jwtProviderMock = new Mock<IJwtProvider>();
        _passwordHasherMock = new Mock<IPasswordHasher>();
        _userService = new UserService(_userRepositoryMock.Object,
            _jwtProviderMock.Object,
            _loggerMock.Object,
            _passwordHasherMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsSuccessResult_WithUserDtos()
    {
        // Arrange
        var users = new List<User>
        {
            new User { Id = 1, Name = "Alice", Email = "alice@example.com" },
            new User { Id = 2, Name = "Bob", Email = "bob@example.com" }
        };

        _userRepositoryMock.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(users);

        // Act
        var result = await _userService.GetAllAsync();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Equal(2, result.Data.Count);
        Assert.Equal("Alice", result.Data[0].Name);
        Assert.Equal("bob@example.com", result.Data[1].Email);

        _loggerMock.Verify(l => l.LogInfo("Getting all users"), Times.Once);
        _loggerMock.Verify(l => l.LogInfo("Retrieved 2 users"), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_WhenNoUsers_ReturnsEmptyList()
    {
        _userRepositoryMock.Setup(r => r.GetAllAsync())
            .ReturnsAsync(new List<User>());

        var result = await _userService.GetAllAsync();

        Assert.True(result.IsSuccess);
        Assert.Empty(result.Data);
        _loggerMock.Verify(l => l.LogInfo("Retrieved 0 users"), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_UserExists_ReturnsSuccessResultWithUserDto()
    {
        // Arrange
        var userId = 1;
        var user = new User
        {
            Id = userId,
            Name = "John",
            Email = "john@example.com"
        };

        _userRepositoryMock
            .Setup(repo => repo.GetByIdAsync(userId))
            .ReturnsAsync(user);

        // Act
        var result = await _userService.GetByIdAsync(userId);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Equal(user.Id, result.Data.Id);
        Assert.Equal(user.Name, result.Data.Name);
        Assert.Equal(user.Email, result.Data.Email);

        _loggerMock.Verify(l => l.LogInfo($"Getting user by Id = {userId}"), Times.Once);
        _loggerMock.Verify(l => l.LogInfo($"User with Id = {userId} retrieved successfully"), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_UserDoesNotExist_ReturnsFailureResult()
    {
        // Arrange
        var userId = 42;

        _userRepositoryMock
            .Setup(repo => repo.GetByIdAsync(userId))
            .ReturnsAsync((User?)null);

        // Act
        var result = await _userService.GetByIdAsync(userId);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("User not found", result.Message);
        Assert.Null(result.Data);

        _loggerMock.Verify(l => l.LogInfo($"Getting user by Id = {userId}"), Times.Once);
        _loggerMock.Verify(l => l.LogWarning($"User with Id = {userId} not found"), Times.Once);
    }

    [Fact]
    public async Task RegisterAsync_NewEmail_AddsUserAndReturnsSuccess()
    {
        var dto = new RegisterUserDto
        {
            Name = "Test User",
            Email = "test@example.com",
            Password = "password123"
        };

        _userRepositoryMock
            .Setup(r => r.GetByEmailAsync(dto.Email))
            .ReturnsAsync((User?)null);
        _passwordHasherMock
            .Setup(p => p.Generate(dto.Password))
            .Returns("hashedPassword");
        var createdUser = new User
        { Id = 1, Name = dto.Name, Email = dto.Email, PasswordHash = "hashedPassword" };
        _userRepositoryMock
            .Setup(r => r.AddAsync(It.IsAny<User>()))
            .Callback<User>(u => u.Id = 1)
            .Returns(Task.CompletedTask);

        var result = await _userService.RegisterAsync(dto);


        Assert.True(result.IsSuccess);
        Assert.Equal(1, result.Data);
        _userRepositoryMock.Verify(r => r.AddAsync(It.Is<User>(
        u => u.Name == dto.Name && u.Email == dto.Email && u.PasswordHash == "hashedPassword"
    )), Times.Once);

        _loggerMock.Verify(l => l.LogInfo($"Registering user with email: {dto.Email}"), Times.Once);
        _loggerMock.Verify(l => l.LogInfo($"User registered successfully with Id = 1"), Times.Once);

    }
    [Fact]
    public async Task RegisterAsync_EmailAlreadyExists_ReturnsFailure()
    {
        var dto = new RegisterUserDto
        {
            Name = "Test User",
            Email = "test@example.com",
            Password = "password123"
        };
        var existingUser = new User { Id = 1, Email = dto.Email };
        _userRepositoryMock
            .Setup(r => r.GetByEmailAsync(dto.Email))
            .ReturnsAsync(existingUser);

        var result = await _userService.RegisterAsync(dto);

        Assert.False(result.IsSuccess);
        Assert.Equal(0, result.Data);

        _userRepositoryMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Never);
        _loggerMock.Verify(l => l.LogInfo($"Registering user with email: {dto.Email}"), Times.Once);

    }

    [Fact]
    public async Task LoginAsync_ReturnsSuccess_WhenCredentialsAreValid()
    {
        var dto = new LoginDto
        {
            Email = "test@example.com",
            Password = "password123"
        };
        var user = new User
        {
            Id = 1,
            Email = dto.Email,
            PasswordHash = "hashedPassword"
        };
        _userRepositoryMock
            .Setup(r => r.GetByEmailAsync(dto.Email))
            .ReturnsAsync(user);
        _passwordHasherMock
            .Setup(p => p.Verify(dto.Password, user.PasswordHash))
            .Returns(true);
        _jwtProviderMock
            .Setup(j => j.CreateToken(user))
            .Returns("jwtToken");

        var res = await _userService.LoginAsync(dto);

        Assert.True(res.IsSuccess);
        Assert.Equal("jwtToken", res.Data);
        _loggerMock.Verify(l => l.LogInfo($"Login attempt for email: {dto.Email}"), Times.Once);
        _loggerMock.Verify(l => l.LogInfo($"User logged in successfully: {dto.Email}"), Times.Once);
        _loggerMock.Verify(l => l.LogWarning(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task LoginAsync_ReturnsFailure_WhenEmailDoesNotExist()
    {
        var dto = new LoginDto
        {
            Email = "test@example.com",
            Password = "password123"
        };

        _userRepositoryMock
            .Setup(r => r.GetByEmailAsync(dto.Email))
             .ReturnsAsync((User?)null);


        var res = await _userService.LoginAsync(dto);

        Assert.False(res.IsSuccess);
        Assert.Equal("Invalid email or password", res.Message);
        Assert.Null(res.Data);
        _loggerMock.Verify(l => l.LogInfo($"Login attempt for email: {dto.Email}"), Times.Once);
        _loggerMock.Verify(l => l.LogWarning($"Invalid login attempt for email: {dto.Email}"), Times.Once);

    }

    [Fact]
    public async Task LoginAsync_WithInvalidPassword_ReturnsFailure()
    {
        var dto = new LoginDto
        {
            Email = "test@example.com",
            Password = "password123"
        };
        var user = new User
        {
            Id = 1,
            Email = dto.Email,
            PasswordHash = "hashedPassword"
        };
        _userRepositoryMock
            .Setup(r => r.GetByEmailAsync(dto.Email))
            .ReturnsAsync(user);
        _passwordHasherMock.Setup(p => p.Verify(dto.Password, user.PasswordHash))
            .Returns(false);

        var res = await _userService.LoginAsync(dto);

        Assert.False(res.IsSuccess);
        Assert.Equal("Invalid email or password", res.Message);
        Assert.Null(res.Data);
        _loggerMock.Verify(l => l.LogInfo($"Login attempt for email: {dto.Email}"), Times.Once);
        _loggerMock.Verify(l => l.LogWarning($"Invalid login attempt for email: {dto.Email}"), Times.Once);
    }

    [Fact]
    public async Task UpdateProfileAsync_WithValidData_ReturnsSuccess()
    {
        var dto = new UpdateUserDto
        {
            Name = "Ivan",
            Email = "test@example.com"
        };
        var user = new User
        {
            Id = 1,
            Name = "OldName",
            Email = "old@example.com"

        };
        _userRepositoryMock.Setup(l => l.GetByIdAsync(1)).ReturnsAsync(user);
        _userRepositoryMock
        .Setup(r => r.UpdateAsync(It.IsAny<User>()))
        .Returns(Task.CompletedTask);

        var res = await _userService.UpdateProfileAsync(1, dto);
        Assert.True(res.IsSuccess);
        Assert.True(res.Data);
        Assert.Equal(dto.Name, user.Name);
        Assert.Equal(dto.Email, user.Email);
        _loggerMock.Verify(l => l.LogInfo($"Updating profile for user Id = {user.Id}"), Times.Once);
        _loggerMock.Verify(l => l.LogInfo($"User profile updated successfully: Id = {user.Id}"), Times.Once);
    }

    [Fact]
    public async Task UpdateProfileAsync_WhenUserNotFound_ReturnsFailure()
    {
        // Arrange
        var dto = new UpdateUserDto
        {
            Name = "Ivan",
            Email = "test@example.com"
        };

        int userId = 1;

        _userRepositoryMock
            .Setup(r => r.GetByIdAsync(userId))
            .ReturnsAsync((User?)null);

        // Act
        var res = await _userService.UpdateProfileAsync(userId, dto);

        // Assert
        Assert.False(res.IsSuccess);
        Assert.False(res.Data);
        Assert.Equal("User not found", res.Message);

        _loggerMock.Verify(
    l => l.LogInfo(It.Is<string>(msg => msg.Contains($"Updating profile for user Id = {userId}"))),
    Times.Once);

        _loggerMock.Verify(
            l => l.LogWarning(It.Is<string>(msg => msg.Contains($"User with Id = {userId} not found"))),
            Times.Once);

    }

    [Fact]
    public async Task DeleteAsync_WithValidData_ReturnsTrue()
    {
        int userId = 1;
        var user = new User
        {
            Id = userId,
            Name = "OldName",
            Email = "old@example.com"
        };

        _userRepositoryMock
            .Setup(r => r.GetByIdAsync(userId))
            .ReturnsAsync(user);

        _userRepositoryMock
            .Setup(r => r.DeleteAsync(user))
            .Returns(Task.CompletedTask);

        var res = await _userService.DeleteAsync(userId);

        Assert.True(res.IsSuccess);
        Assert.True(res.Data);

        _loggerMock.Verify(l => l.LogInfo($"Deleting user with Id = {userId}"), Times.Once);
        _loggerMock.Verify(l => l.LogInfo($"User deleted successfully: Id = {userId}"), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WhenUserNotFound_ReturnsFailure()
    {
        int userId = 1;

        _userRepositoryMock
            .Setup(r => r.GetByIdAsync(userId))
            .ReturnsAsync((User?)null);

        var res = await _userService.DeleteAsync(userId);

        Assert.False(res.IsSuccess);
        Assert.False(res.Data);
        Assert.Equal("User not found", res.Message);

        _loggerMock.Verify(l => l.LogInfo($"Deleting user with Id = {userId}"), Times.Once);
        _loggerMock.Verify(l => l.LogWarning($"Delete failed: User with Id = {userId} not found"), Times.Once);
        _loggerMock.Verify(l => l.LogInfo($"User deleted successfully: Id = {userId}"), Times.Never);
    }


}
