using System;
using Microsoft.Extensions.DependencyInjection;
using UMS.BLL.Shared;
using UMS.DAL.Shared;
using UMS.DAL;
using UMS.Models;

//using CloudServicesApp.DataLayer;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace UMS.ConsoleApplication
{
    class Program
    {
        private static IServiceProvider _serviceProvider;
        private static User _currentUser;

        static async Task Main(string[] args)
        {
            ConfigureServices();

            try
            {
                await InitializeDatabase();
                await RunLoginMenu();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            finally
            {
                DisposeServices();
            }
        }

        private static void ConfigureServices()
        {
            var services = new ServiceCollection();

            // Load configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            // Configure DbContext
            string connectionString = configuration.GetConnectionString("DefaultConnection") ??
                "server=localhost;port=3306;database=cloudservices;user=root;password=password";

            services.AddDbContext<AppDbContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            // Register services
            services.AddScoped<ICloudServiceRepository, CloudServiceRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICloudServiceService, CloudServiceService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<MenuService>();

            _serviceProvider = services.BuildServiceProvider();
        }

        private static async Task InitializeDatabase()
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            try
            {
                await dbContext.Database.EnsureCreatedAsync();
                Console.WriteLine("Database initialized successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing database: {ex.Message}");
                Console.WriteLine("Make sure your MySQL server is running and the connection string is correct.");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
                Environment.Exit(1);
            }
        }

        private static async Task RunLoginMenu()
        {
            var menuService = _serviceProvider.GetRequiredService<MenuService>();
            await menuService.DisplayLoginMenuAsync();
        }

        private static void DisposeServices()
        {
            if (_serviceProvider is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }
}
