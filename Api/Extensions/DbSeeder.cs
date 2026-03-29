using Data.Context;
using Data.Entities;
using Data.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Api.Extensions;

public static class DbSeeder
{
    public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        // 1. Aseguramos que los roles existan
        foreach (var role in UserRoles.All)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        // 2. Creamos al usuario Administrador por defecto si no existe
        const string adminEmail = "IngenieriaAdmin@upds.edu.bo";
        
        if (await userManager.FindByEmailAsync(adminEmail) == null)
        {
            // Creamos primero la entidad persona (requerida por nuestra relación forzosa)
            var adminPerson = new Person
            {
                FirstName = "Administrador",
                LastName = "Sistema",
                MotherLastName = "UPDS",
                DateOfBirth = new DateOnly(2000, 1, 1),
                Gender = Gender.Other,
                NationalId = "0000000",
                NationalIdExpedition = "SCZ"
            };

            context.People.Add(adminPerson);
            await context.SaveChangesAsync();

            // Creamos el Identity User
            var adminUser = new User
            {
                UserName = adminEmail,
                Email = adminEmail,
                PersonId = adminPerson.Id,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(adminUser, "IngenieriaPassword@2026");

            if (result.Succeeded)
            {
                 await userManager.AddToRoleAsync(adminUser, UserRoles.Admin);
            }
        }
    }
}