using System;
using Microsoft.AspNetCore.Identity;
using reservacion.Areas.Identity.Data;

namespace reservacion.Models{
    public static class DataSeeder
    {
        private static string normalRoleName = "Normal";
        private static string adminRoleName = "Administrador";
        private static string adminUsername = "admin";
        private static string baseNombreUser = "Usuario";
        private static string baseApellidoUser = "Autogenerado";
        private static string baseUsername = "user";
        private static string basePassword = "passReservacion2020";
        private static int cantidadUsuarios = 3;
        public static void SeedData(UserManager<User> userManager,RoleManager<IdentityRole> roleManager){
            Console.WriteLine("Iniciado Seeder");
            SeedRoles(roleManager);
            SeedUsers(userManager);
            Console.WriteLine("Seeder Completado");
        }

        public static void SeedRoles(RoleManager<IdentityRole> roleManager){
            if(!roleManager.RoleExistsAsync(normalRoleName).Result){
                IdentityRole role = new IdentityRole();
                role.Name = normalRoleName;
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }

            if(!roleManager.RoleExistsAsync(adminRoleName).Result){
                IdentityRole role = new IdentityRole();
                role.Name = adminRoleName;
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
        }

        public static void SeedUsers(UserManager<User> userManager){
            for(int i=1; i<=cantidadUsuarios; i++){
                if(userManager.FindByNameAsync(baseUsername + i.ToString()).Result == null){
                    User user = new User();
                    user.UserName = baseUsername + i.ToString();
                    user.Email = baseUsername + "@localhost";
                    user.Nombres = baseNombreUser;
                    user.Apellidos = baseApellidoUser + i;
                    IdentityResult result = userManager.CreateAsync(user, basePassword).Result;
                    if(result.Succeeded){
                        userManager.AddToRoleAsync(user, normalRoleName).Wait();
                    }
                }
            }
            if(userManager.FindByNameAsync(adminUsername).Result == null){
                User user = new User();
                user.UserName = adminUsername;
                user.Email = adminUsername + "@localhost";
                user.Nombres = adminUsername;
                user.Apellidos = "site";
                IdentityResult result = userManager.CreateAsync(user, basePassword).Result;
                if(result.Succeeded){
                    userManager.AddToRoleAsync(user, adminRoleName).Wait();
                }
            }
        }
    }
}