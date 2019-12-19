// using SharesAPI.Models;
// using Microsoft.EntityFrameworkCore;

// namespace EntityCoreAPIExample.DatabaseAccess
// {
//     public class AppDbContext : DbContext
//     {
//         public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
//         //passes options to overloaded base implementation
//         {

//         }

//         public DbSet<Employee> Employees { get; set; }
//         public DbSet<Department> Departments { get; set; }

//         //To create/update database we need to use migration commands within the package manager console (This REQUIRES Microsoft.EntityFrameworkCore.Tools Nuget Package)
//         //Get-Help about_entityframeworkcore - Will provide some useful info 
//         //Add-Migration {MigrationName}
//         //Update-Database {MigrationName}
//     }
// }
