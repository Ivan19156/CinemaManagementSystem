using System.Text;
using System.Threading;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using FluentValidation.AspNetCore;

using CinemaManagementSystem.Core.Entities;
using CinemaManagementSystem.Infrastructure;
using CinemaManagementSystem.Infrastructure.Logging;
using CinemaManagementSystem.Infrastructure.Repositories;
using CinemaManagementSystem.Infrastructure.Services;
using CinemaManagementSystem.WebAPI;

using Application.Authentication;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Authorization.YourProject.Authorization;

using Contracts.Validators.UserValidator;
using CinemaManagementSystem.WebAPI.extensions;
using Contracts.Validators.TicketValidators;
using Contracts.Validators.SessionValidators;
using Contracts.Validators.SaleValidators;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var services = builder.Services;
        var config = builder.Configuration;

        // ---------- Configuration ----------
        var connectionString = config.GetConnectionString("DefaultConnection");
        var jwtOptionsSection = config.GetSection("JwtOptions");
        services.Configure<JwtOptions>(jwtOptionsSection);
        var jwtOptions = jwtOptionsSection.Get<JwtOptions>();

        // ---------- Database ----------
        services.AddDbContext<CinemaDbContext>(options =>
            options.UseSqlServer(connectionString, sql =>
                sql.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null)));

        // ---------- Authentication ----------
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IJwtProvider, JwtProvider>();
        services.AddApiAuth(Options.Create(jwtOptions)); // JWT + Auth cookie setup

        // ---------- Repositories & Services ----------
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ITicketRepository, TicketRepository>();
        services.AddScoped<ITicketService, TicketService>();
        services.AddScoped<IHallRepository, HallRepository>();
        services.AddScoped<IHallService, HallService>();
        services.AddScoped<IFilmRepository, FilmRepository>();
        services.AddScoped<IFilmService, FilmService>();
        services.AddScoped<ISessionRepository, SessionRepository>();
        services.AddScoped<ISessionService, SessionService>();
        services.AddScoped<ISaleRepository, SaleRepository>();
        services.AddScoped<ISaleService, SaleService>();
        services.AddScoped<IDiscountRepository, DiscountRepository>();
        services.AddScoped<IDiscountService, DiscountService>();
        services.AddScoped(typeof(IAppLogger<>), typeof(AppLogger<>));

        // ---------- Authorization ----------
        services.AddAuthorization(options =>
        {
            options.AddPolicy("ResourceOwnerOrAdmin", policy =>
                policy.Requirements.Add(new ResourceOwnerOrAdminRequirement()));
        });
        services.AddSingleton<IAuthorizationHandler, ResourceOwnerOrAdminHandler>();
        services.AddHttpContextAccessor();

        // ---------- Validation ----------
        services.AddControllers()
                .AddFluentValidation(fv =>
                    fv.RegisterValidatorsFromAssemblyContaining<LoginDtoValidator>());
        services.AddControllers()
    .AddFluentValidation(fv =>
        fv.RegisterValidatorsFromAssemblyContaining<RegisterDTOValidator>());
        services.AddControllers()
    .AddFluentValidation(fv =>
        fv.RegisterValidatorsFromAssemblyContaining<UpdateDTOValidator>());
        services.AddControllers()
    .AddFluentValidation(fv =>
        fv.RegisterValidatorsFromAssemblyContaining<UserDTOValidator>());
        services.AddControllers()
    .AddFluentValidation(fv =>
        fv.RegisterValidatorsFromAssemblyContaining<CreateTicketDTOValidator>());
        services.AddControllers()
    .AddFluentValidation(fv =>
        fv.RegisterValidatorsFromAssemblyContaining<TicketDtoValidator>());
        services.AddControllers()
    .AddFluentValidation(fv =>
        fv.RegisterValidatorsFromAssemblyContaining<UpdateTicketDTOValidator>());
        services.AddControllers()
    .AddFluentValidation(fv =>
        fv.RegisterValidatorsFromAssemblyContaining<CreateSessionDtoValidator>());
        services.AddControllers()
    .AddFluentValidation(fv =>
        fv.RegisterValidatorsFromAssemblyContaining<SessionDtoValidator>());
        services.AddControllers()
    .AddFluentValidation(fv =>
        fv.RegisterValidatorsFromAssemblyContaining<UpdateSessionDtoValidator>());
        services.AddControllers()
    .AddFluentValidation(fv =>
        fv.RegisterValidatorsFromAssemblyContaining<CreateSaleDtoValidator>());
        services.AddControllers()
    .AddFluentValidation(fv =>
        fv.RegisterValidatorsFromAssemblyContaining<SaleDtoValidator>());
        services.AddControllers()
    .AddFluentValidation(fv =>
        fv.RegisterValidatorsFromAssemblyContaining<UpdateSaleDtoValidator>());
        services.AddControllers()
    .AddFluentValidation(fv =>
        fv.RegisterValidatorsFromAssemblyContaining<CreateFilmDtoValidator>());
        services.AddControllers()
    .AddFluentValidation(fv =>
        fv.RegisterValidatorsFromAssemblyContaining<FilmDtoValidator>());
        services.AddControllers()
    .AddFluentValidation(fv =>
        fv.RegisterValidatorsFromAssemblyContaining<UpdateFilmDtoValidator>());
        services.AddControllers()
    .AddFluentValidation(fv =>
        fv.RegisterValidatorsFromAssemblyContaining<CreateDiscountDtoValidator>());
        services.AddControllers()
    .AddFluentValidation(fv =>
        fv.RegisterValidatorsFromAssemblyContaining<DiscountDtoValidator>());
        services.AddControllers()
    .AddFluentValidation(fv =>
        fv.RegisterValidatorsFromAssemblyContaining<UserDiscountDtoValidator>());


        // ---------- Swagger ----------
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter JWT token in the format: Bearer {your token}"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        // ---------- Build App ----------
        var app = builder.Build();

        app.UseSwagger();
        app.UseSwaggerUI();

        app.Use(async (context, next) =>
        {
            if (!context.Request.Headers.ContainsKey("Authorization") &&
                context.Request.Cookies.TryGetValue("tasty-cookies", out var token))
            {
                context.Request.Headers.Authorization = $"Bearer {token}";
            }

            await next();
        });

        app.UseMiddleware<ExceptionHandlingMiddleware>();

        // app.UseHttpsRedirection(); // Optional for dev/test
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        // ---------- Apply Migrations with Retry ----------
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<CinemaDbContext>();
            const int maxRetries = 5;
            int retryCount = 0;

            while (true)
            {
                try
                {
                    dbContext.Database.Migrate();
                    break;
                }
                catch (Microsoft.Data.SqlClient.SqlException)
                {
                    if (++retryCount >= maxRetries) throw;
                    Thread.Sleep(2000);
                }
            }
        }

        app.Run();
    }
}
