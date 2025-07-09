using Application.Interfaces.Services;
using Contracts.DTOs.UsersDto;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public static class DbInitializer
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<CinemaDbContext>();

        // приклад сідингу адміна
        var userService = scope.ServiceProvider.GetRequiredService<IUserService>();

        if (!await context.Users.AnyAsync(u => u.Email == "admin@cinema.com"))
        {
            await userService.RegisterAdminAsync(new RegisterUserDto
            {
                Name = "Admin",
                Email = "admin@cinema.com",
                Password = "Admin123!",
            });
        }
    }
}
