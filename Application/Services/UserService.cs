using Application.Authentication;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using CinemaManagementSystem.Core.Entities;
using CinemaManagementSystem.Core.Enums;
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

        public UserService(
            IUserRepository userRepository,
            ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        public async Task<ServiceResult<List<UserDto>>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();

            var dtos = users.Select(u => new UserDto
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email
            }).ToList();

            return ServiceResult<List<UserDto>>.Success(dtos);
        }

        public async Task<ServiceResult<UserDto?>> GetByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return ServiceResult<UserDto?>.Failure("User not found");

            var dto = new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            };

            return ServiceResult<UserDto?>.Success(dto);
        }

        public async Task<ServiceResult<int>> RegisterAsync(RegisterUserDto dto)
        {
            var existing = await _userRepository.GetByEmailAsync(dto.Email);
            if (existing != null)
                return ServiceResult<int>.Failure("Email already registered");

            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                PasswordHash = dto.Password, // без хешування
                Role = UserType.User,
                Bonus = 0
            };

            await _userRepository.AddAsync(user);
            return ServiceResult<int>.Success(user.Id);
        }

        public async Task<ServiceResult<string>> LoginAsync(LoginDto dto)
        {
            var user = await _userRepository.GetByEmailAsync(dto.Email);
            if (user == null || user.PasswordHash != dto.Password)
                return ServiceResult<string>.Failure("Invalid email or password");

            var token = _tokenService.GenerateToken(user);
            return ServiceResult<string>.Success(token);
        }

        public async Task<ServiceResult<bool>> UpdateProfileAsync(int id, UpdateUserDto dto)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return ServiceResult<bool>.Failure("User not found");

            user.Name = dto.Name;
            user.Email = dto.Email;

            await _userRepository.UpdateAsync(user);
            return ServiceResult<bool>.Success(true);
        }

        public async Task<ServiceResult<bool>> DeleteAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return ServiceResult<bool>.Failure("User not found");

            await _userRepository.DeleteAsync(user);
            return ServiceResult<bool>.Success(true);
        }
    }

}

