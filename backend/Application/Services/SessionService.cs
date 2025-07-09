using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Core.Entities;
using CinemaManagementSystem.WebAPI.Helpers;
using Contracts.DTOs.SessionDTOs;
using Shared.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class SessionService : ISessionService
    {
        private readonly ISessionRepository _sessionRepository;
        public SessionService(ISessionRepository sessionRepository)
        {
            _sessionRepository = sessionRepository;
        }
        public async Task<ServiceResult<List<SessionDto>>> GetAllAsync()
        {
            var sessions = await _sessionRepository.GetAllAsync();
            var dtos = sessions.Select(s => new SessionDto
            {
                Id = s.Id,
                FilmId = s.FilmId,
                StartTime = s.Time,
                HallId = s.HallId
            });
            return ServiceResult<List<SessionDto>>.Success(dtos.ToList());

        }

        public async Task<ServiceResult<SessionDto>> GetByIdAsync(int id)
        {
            var session = await _sessionRepository.GetByIdAsync(id);
            if (session == null)
                return ServiceResult<SessionDto>.Failure("Сеанс не знайдено");
            var dto = new SessionDto
            {
                Id = session.Id,
                FilmId = session.FilmId,
                StartTime = session.Time,
                HallId = session.HallId
            };
            return ServiceResult<SessionDto>.Success(dto);
        }
        public async Task<ServiceResult<int>> CreateAsync(CreateSessionDto dto)
        {
            var session = new Session
            {
                FilmId = dto.FilmId,
                Time = dto.StartTime,
                HallId = dto.HallId
            };
            var id = await _sessionRepository.CreateAsync(session);
            return ServiceResult<int>.Success(id);

        }

        public async Task<ServiceResult<bool>> UpdateAsync(int id, UpdateSessionDto dto)
        {
            var session = await _sessionRepository.GetByIdAsync(id);
            if (session == null)
                return ServiceResult<bool>.Failure("Сеанс не знайдено");
            session.FilmId = dto.FilmId;
            session.Time = dto.StartTime;
            session.HallId = dto.HallId;
            var result = await _sessionRepository.UpdateAsync(session);
            return ServiceResult<bool>.Success(result);
        }

        public async Task<ServiceResult<bool>> DeleteAsync(int id)
        {
            var session = await _sessionRepository.GetByIdAsync(id);
            if (session == null)
                return ServiceResult<bool>.Failure("Сеанс не знайдено");
            var result = await _sessionRepository.DeleteAsync(id);
            return ServiceResult<bool>.Success(result);
        }

        public async Task<ServiceResult<List<SessionDto>>> GetByFilmIdAsync(int filmId)
        {
            var sessions = await _sessionRepository.GetByFilmIdAsync(filmId);
            if (sessions == null || sessions.Count == 0)
                return ServiceResult<List<SessionDto>>.Failure("Сеанси не знайдено");

            var dtos = sessions.Select(s => new SessionDto
            {
                Id = s.Id,
                FilmId = s.FilmId,
                StartTime = s.Time,
                HallId = s.HallId
            }).ToList();

            return ServiceResult<List<SessionDto>>.Success(dtos);
        }

    }
}