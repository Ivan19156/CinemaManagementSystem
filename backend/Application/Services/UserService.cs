using Application.Authentication;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Core.Entities;
using Core.Enums;
using CinemaManagementSystem.Infrastructure.Logging;
using CinemaManagementSystem.WebAPI.Helpers;
using Contracts.DTOs.UsersDto;
using Microsoft.AspNetCore.Identity;
using Shared.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtProvider _jwtProvider;
    private readonly IAppLogger<UserService> _logger;
    private readonly IPasswordHasher _passwordHasher;

    public UserService(
        IUserRepository userRepository,
        IJwtProvider jwtProvider,
        IAppLogger<UserService> logger,
        IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _jwtProvider = jwtProvider;
        _logger = logger;
        _passwordHasher = passwordHasher;
    }

    public async Task<ServiceResult<List<UserDto>>> GetAllAsync()
    {
        _logger.LogInfo("Getting all users");

        var users = await _userRepository.GetAllAsync();

        var dtos = users.Select(u => new UserDto
        {
            Id = u.Id,
            Name = u.Name,
            Email = u.Email
        }).ToList();

        _logger.LogInfo($"Retrieved {dtos.Count} users");
        return ServiceResult<List<UserDto>>.Success(dtos);
    }

    public async Task<ServiceResult<UserDto?>> GetByIdAsync(int id)
    {
        _logger.LogInfo($"Getting user by Id = {id}");

        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            _logger.LogWarning($"User with Id = {id} not found");
            return ServiceResult<UserDto?>.Failure("User not found");
        }

        var dto = new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role.ToString()
        };

        _logger.LogInfo($"User with Id = {id} retrieved successfully");
        return ServiceResult<UserDto?>.Success(dto);
    }

    public async Task<ServiceResult<int>> RegisterAsync(RegisterUserDto dto)
    {
        _logger.LogInfo($"Registering user with email: {dto.Email}");

        var existing = await _userRepository.GetByEmailAsync(dto.Email);
        if (existing != null)
        {
            _logger.LogWarning($"Registration failed: Email {dto.Email} is already registered");
            return ServiceResult<int>.Failure("Email already registered");
        }

        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email,
            PasswordHash = _passwordHasher.Generate(dto.Password),
            Role = UserType.User,
            Bonus = 0
        };

        await _userRepository.AddAsync(user);

        _logger.LogInfo($"User registered successfully with Id = {user.Id}");
        return ServiceResult<int>.Success(user.Id);
    }

    public async Task<ServiceResult<string>> LoginAsync(LoginDto dto)
    {
        _logger.LogInfo($"Login attempt for email: {dto.Email}");

        var user = await _userRepository.GetByEmailAsync(dto.Email);
        if (user == null || !_passwordHasher.Verify(dto.Password, user.PasswordHash))
        {
            _logger.LogWarning($"Invalid login attempt for email: {dto.Email}");
            return ServiceResult<string>.Failure("Invalid email or password");
        }

        var token = _jwtProvider.CreateToken(user);

        _logger.LogInfo($"User logged in successfully: {dto.Email}");
        return ServiceResult<string>.Success(token);
    }

    public async Task<ServiceResult<bool>> UpdateProfileAsync(int id, UpdateUserDto dto)
    {
        _logger.LogInfo($"Updating profile for user Id = {id}");

        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            _logger.LogWarning($"Update failed: User with Id = {id} not found");
            return ServiceResult<bool>.Failure("User not found");
        }

        user.Name = dto.Name;
        user.Email = dto.Email;

        await _userRepository.UpdateAsync(user);

        _logger.LogInfo($"User profile updated successfully: Id = {id}");
        return ServiceResult<bool>.Success(true);
    }

    public async Task<ServiceResult<bool>> DeleteAsync(int id)
    {
        _logger.LogInfo($"Deleting user with Id = {id}");

        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            _logger.LogWarning($"Delete failed: User with Id = {id} not found");
            return ServiceResult<bool>.Failure("User not found");
        }

        await _userRepository.DeleteAsync(user);

        _logger.LogInfo($"User deleted successfully: Id = {id}");
        return ServiceResult<bool>.Success(true);
    }

    public async Task<ServiceResult<int>> RegisterAdminAsync(RegisterUserDto dto)
    {
        _logger.LogInfo($"Registering admin with email: {dto.Email}");
        var existing = await _userRepository.GetByEmailAsync(dto.Email);
        if (existing != null)
        {
            _logger.LogWarning($"Registration failed: Email {dto.Email} is already registered");
            return ServiceResult<int>.Failure("Email already registered");
        }
        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email,
            PasswordHash = _passwordHasher.Generate(dto.Password),
            Role = UserType.Admin,
            Bonus = 0
        };
        await _userRepository.AddAsync(user);

        _logger.LogInfo($"Admin registered successfully with Id = {user.Id}");
        return ServiceResult<int>.Success(user.Id);
    }

}
