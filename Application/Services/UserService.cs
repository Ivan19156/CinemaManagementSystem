using Application.Authentication;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using CinemaManagementSystem.Core.Entities;
using CinemaManagementSystem.Core.Enums;
using CinemaManagementSystem.Infrastructure.Logging;
using CinemaManagementSystem.WebAPI.Helpers;
using Contracts.DTOs.UsersDto;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaManagementSystem.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IAppLogger<UserService> _logger;

        public UserService(
            IUserRepository userRepository,
            ITokenService tokenService,
            IAppLogger<UserService> logger)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _logger = logger;
        }

        public async Task<ServiceResult<List<UserDto>>> GetAllAsync()
        {
            _logger.LogInfo("Getting all users");

            try
            {
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all users");
                return ServiceResult<List<UserDto>>.Failure("Failed to get users");
            }
        }

        public async Task<ServiceResult<UserDto?>> GetByIdAsync(int id)
        {
            _logger.LogInfo($"Getting user by Id = {id}");

            try
            {
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
                    Email = user.Email
                };

                _logger.LogInfo($"User with Id = {id} retrieved successfully");
                return ServiceResult<UserDto?>.Success(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting user by Id = {id}");
                return ServiceResult<UserDto?>.Failure("Failed to get user");
            }
        }

        public async Task<ServiceResult<int>> RegisterAsync(RegisterUserDto dto)
        {
            _logger.LogInfo($"Registering user with email: {dto.Email}");

            try
            {
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
                    PasswordHash = dto.Password, // TODO: Hash password properly
                    Role = UserType.User,
                    Bonus = 0
                };

                await _userRepository.AddAsync(user);

                _logger.LogInfo($"User registered successfully with Id = {user.Id}");
                return ServiceResult<int>.Success(user.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Register failed");
                return ServiceResult<int>.Failure(ex.Message); // для дебагу (НЕ залишай так у проді)
                //_logger.LogError(ex, $"Error occurred while registering user with email: {dto.Email}");
                //return ServiceResult<int>.Failure("Failed to register user");
            }
        }

        public async Task<ServiceResult<string>> LoginAsync(LoginDto dto)
        {
            _logger.LogInfo($"Login attempt for email: {dto.Email}");

            try
            {
                var user = await _userRepository.GetByEmailAsync(dto.Email);
                if (user == null || user.PasswordHash != dto.Password)
                {
                    _logger.LogWarning($"Invalid login attempt for email: {dto.Email}");
                    return ServiceResult<string>.Failure("Invalid email or password");
                }

                var token = _tokenService.GenerateToken(user);

                _logger.LogInfo($"User logged in successfully: {dto.Email}");
                return ServiceResult<string>.Success(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred during login for email: {dto.Email}");
                return ServiceResult<string>.Failure("Failed to login");
            }
        }

        public async Task<ServiceResult<bool>> UpdateProfileAsync(int id, UpdateUserDto dto)
        {
            _logger.LogInfo($"Updating profile for user Id = {id}");

            try
            {
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
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating user profile Id = {id}");
                return ServiceResult<bool>.Failure("Failed to update profile");
            }
        }

        public async Task<ServiceResult<bool>> DeleteAsync(int id)
        {
            _logger.LogInfo($"Deleting user with Id = {id}");

            try
            {
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
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting user Id = {id}");
                return ServiceResult<bool>.Failure("Failed to delete user");
            }
        }
    }

}

