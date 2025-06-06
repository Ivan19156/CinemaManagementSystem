using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using CinemaManagementSystem.Infrastructure;
using CinemaManagementSystem.WebAPI.Helpers;
using Application.Authentication;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using CinemaManagementSystem.Infrastructure.Repositories;
using CinemaManagementSystem.Infrastructure.Services;
using Application.Authorization.YourProject.Authorization;
using Microsoft.AspNetCore.Authorization;
using CinemaManagementSystem.Infrastructure.Logging;
using FluentValidation.AspNetCore;
using Contracts.Validators.UserValidator;



public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // ?? ������������ ����� ���������� �� ��
        var connection = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<CinemaDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(10),
                errorNumbersToAdd: null);
        }));


        // ?? ������������ JWT
        var jwtSettings = builder.Configuration.GetSection("JwtSettings");
        builder.Services.Configure<JwtSettings>(jwtSettings);

        // ?? �������� �����������
        builder.Services.AddScoped<ITokenService, TokenService>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<ITicketRepository, TicketRepository>();
        builder.Services.AddScoped<ITicketService, TicketService>();
        builder.Services.AddScoped<IHallRepository, HallRepository>();
        builder.Services.AddScoped<IHallService, HallService>();
        builder.Services.AddScoped<IFilmRepository, FilmRepository>();
        builder.Services.AddScoped<IFilmService, FilmService>();
        builder.Services.AddScoped<ISessionRepository, SessionRepository>();
        builder.Services.AddScoped<ISessionService, SessionService>();
        builder.Services.AddScoped<ISaleRepository, SaleRepository>();
        builder.Services.AddScoped<ISaleService, SaleService>();
        builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();
        builder.Services.AddScoped<IDiscountService, DiscountService>();
        builder.Services.AddScoped(typeof(IAppLogger<>), typeof(AppLogger<>));

        builder.Services.AddControllers()
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<LoginDtoValidator>());


        // ?? JWT ��������������
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            var config = jwtSettings.Get<JwtSettings>();
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = config.Issuer,
                ValidAudience = config.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.SecretKey))
            };
        });
        builder.Services.AddHttpContextAccessor();

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("ResourceOwnerOrAdmin", policy =>
                policy.Requirements.Add(new ResourceOwnerOrAdminRequirement()));
        });

        builder.Services.AddSingleton<IAuthorizationHandler, ResourceOwnerOrAdminHandler>();
        // ?? ����������
        builder.Services.AddControllers();

        // ?? Swagger � ��������� ����������� ����� JWT
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "������ JWT ����� � ������: Bearer {your token}"
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
            new string[] {}
        }
    });
        });

        var app = builder.Build();

        // ?? Middleware
        if (app.Environment.IsProduction())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        //app.UseHttpsRedirection();

        app.UseAuthentication(); // ���������� ����� Authorization
        app.UseAuthorization();

        app.MapControllers();

        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<CinemaDbContext>();

            var retryCount = 0;
            var maxRetries = 5;

            while (true)
            {
                try
                {
                    dbContext.Database.Migrate();
                    break;
                }
                catch (Microsoft.Data.SqlClient.SqlException)
                {
                    if (++retryCount >= maxRetries)
                        throw;
                    Thread.Sleep(2000); // �������� 2 ��� � ���������
                }
            }
        }

        app.Run();


    }
}
