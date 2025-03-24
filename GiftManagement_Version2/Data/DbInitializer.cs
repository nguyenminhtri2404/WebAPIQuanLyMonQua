using Microsoft.EntityFrameworkCore;

namespace GiftManagement_Version2.Data
{
    public class DbInitializer
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            // Seed Roles
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Admin" },
                new Role { Id = 2, Name = "User" }
            );

            // Seed Permissions
            modelBuilder.Entity<Permission>().HasData(
                new Permission { Code = "CreateUser", Name = "CreateUser" },
                new Permission { Code = "ViewUser", Name = "ViewUser" },
                new Permission { Code = "UpdateteUser", Name = "UpdateteUser" },
                new Permission { Code = "DeleteUser", Name = "DeleteUser" }
            );

            // Seed RolePermissions
            modelBuilder.Entity<RolePermission>().HasData(
                new RolePermission { RoleId = 1, PermissionCode = "CreateUser" },
                new RolePermission { RoleId = 1, PermissionCode = "ViewUser" },
                new RolePermission { RoleId = 1, PermissionCode = "UpdateteUser" },
                new RolePermission { RoleId = 1, PermissionCode = "DeleteUser" }
            );

            // Hash the password before seeding
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword("Admin12345@");

            // Seed Users
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Username = "admin", Email = "admin@example.com", Password = hashedPassword, Phone = "0345567810", RoleId = 1 }
            );
        }
    }
}
