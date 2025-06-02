using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMS.BLL.Shared;
using UMS.Models;

namespace UMS.ConsoleApplication
{
    public class MenuService
    {
        private readonly ICloudServiceService _cloudServiceService;
        private readonly IUserService _userService;
        private User _currentUser;

        public MenuService(ICloudServiceService cloudServiceService, IUserService userService)
        {
            _cloudServiceService = cloudServiceService;
            _userService = userService;
        }

        public async Task DisplayLoginMenuAsync()
        {
            bool exit = false;

            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("=================================");
                Console.WriteLine("   CLOUD SERVICES APPLICATION    ");
                Console.WriteLine("=================================");
                Console.WriteLine("1. Login");
                Console.WriteLine("2. Exit");
                Console.WriteLine("=================================");
                Console.Write("Enter your choice: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await LoginAsync();
                        break;
                    case "2":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Press any key to continue...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private async Task LoginAsync()
        {
            Console.Clear();
            Console.WriteLine("=================================");
            Console.WriteLine("            LOGIN               ");
            Console.WriteLine("=================================");
            Console.Write("Username: ");
            string username = Console.ReadLine();
            Console.Write("Password: ");
            string password = ReadPassword();

            _currentUser = await _userService.AuthenticateAsync(username, password);

            if (_currentUser != null)
            {
                Console.WriteLine("\nLogin successful!");
                Console.WriteLine($"Welcome, {_currentUser.Username}!");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                await DisplayMainMenuAsync();
            }
            else
            {
                Console.WriteLine("\nInvalid username or password. Press any key to continue...");
                Console.ReadKey();
            }
        }

        private async Task DisplayMainMenuAsync()
        {
            bool logout = false;

            while (!logout)
            {
                Console.Clear();
                Console.WriteLine("=================================");
                Console.WriteLine("          MAIN MENU             ");
                Console.WriteLine("=================================");
                Console.WriteLine($"Logged in as: {_currentUser.Username} ({_currentUser.Role})");
                Console.WriteLine("=================================");
                Console.WriteLine("1. View All Cloud Services");
                Console.WriteLine("2. View Services by Type");
                Console.WriteLine("3. Search Services");

                if (_currentUser.Role == UserRole.Admin || _currentUser.Role == UserRole.Editor)
                {
                    Console.WriteLine("4. Add New Cloud Service");
                    Console.WriteLine("5. Edit Cloud Service");
                    Console.WriteLine("6. Delete Cloud Service");
                }

                if (_currentUser.Role == UserRole.Admin)
                {
                    Console.WriteLine("7. User Management");
                }

                Console.WriteLine("0. Logout");
                Console.WriteLine("=================================");
                Console.Write("Enter your choice: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await ViewAllCloudServicesAsync();
                        break;
                    case "2":
                        await ViewServicesByTypeAsync();
                        break;
                    case "3":
                        await SearchServicesAsync();
                        break;
                    case "4":
                        if (_currentUser.Role == UserRole.Admin || _currentUser.Role == UserRole.Editor)
                            await AddCloudServiceAsync();
                        break;
                    case "5":
                        if (_currentUser.Role == UserRole.Admin || _currentUser.Role == UserRole.Editor)
                            await EditCloudServiceAsync();
                        break;
                    case "6":
                        if (_currentUser.Role == UserRole.Admin || _currentUser.Role == UserRole.Editor)
                            await DeleteCloudServiceAsync();
                        break;
                    case "7":
                        if (_currentUser.Role == UserRole.Admin)
                            await UserManagementMenuAsync();
                        break;
                    case "0":
                        logout = true;
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Press any key to continue...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private async Task ViewAllCloudServicesAsync()
        {
            Console.Clear();
            Console.WriteLine("=================================");
            Console.WriteLine("      ALL CLOUD SERVICES        ");
            Console.WriteLine("=================================");

            var services = await _cloudServiceService.GetAllCloudServicesAsync();
            DisplayCloudServices(services);

            Console.WriteLine("\nPress any key to return to the main menu...");
            Console.ReadKey();
        }

        private async Task ViewServicesByTypeAsync()
        {
            Console.Clear();
            Console.WriteLine("=================================");
            Console.WriteLine("     SERVICES BY TYPE           ");
            Console.WriteLine("=================================");
            Console.WriteLine("1. Software as a Service (SaaS)");
            Console.WriteLine("2. Platform as a Service (PaaS)");
            Console.WriteLine("3. Infrastructure as a Service (IaaS)");
            Console.WriteLine("=================================");
            Console.Write("Enter your choice: ");

            string choice = Console.ReadLine();
            ServiceType type;

            switch (choice)
            {
                case "1":
                    type = ServiceType.SaaS;
                    break;
                case "2":
                    type = ServiceType.PaaS;
                    break;
                case "3":
                    type = ServiceType.IaaS;
                    break;
                default:
                    Console.WriteLine("Invalid choice. Press any key to continue...");
                    Console.ReadKey();
                    return;
            }

            var services = await _cloudServiceService.GetCloudServicesByTypeAsync(type);

            Console.Clear();
            Console.WriteLine($"=================================");
            Console.WriteLine($"      {type} SERVICES           ");
            Console.WriteLine($"=================================");

            DisplayCloudServices(services);

            Console.WriteLine("\nPress any key to return to the main menu...");
            Console.ReadKey();
        }

        private async Task SearchServicesAsync()
        {
            Console.Clear();
            Console.WriteLine("=================================");
            Console.WriteLine("        SEARCH SERVICES         ");
            Console.WriteLine("=================================");
            Console.Write("Enter search term: ");
            string searchTerm = Console.ReadLine();

            var services = await _cloudServiceService.SearchCloudServicesAsync(searchTerm);

            Console.Clear();
            Console.WriteLine($"=================================");
            Console.WriteLine($"  SEARCH RESULTS FOR: {searchTerm}");
            Console.WriteLine($"=================================");

            DisplayCloudServices(services);

            Console.WriteLine("\nPress any key to return to the main menu...");
            Console.ReadKey();
        }

        private async Task AddCloudServiceAsync()
        {
            Console.Clear();
            Console.WriteLine("=================================");
            Console.WriteLine("      ADD CLOUD SERVICE         ");
            Console.WriteLine("=================================");

            var cloudService = new CloudService();

            Console.Write("Name: ");
            cloudService.Name = Console.ReadLine();

            Console.WriteLine("Type:");
            Console.WriteLine("1. Software as a Service (SaaS)");
            Console.WriteLine("2. Platform as a Service (PaaS)");
            Console.WriteLine("3. Infrastructure as a Service (IaaS)");
            Console.Write("Enter your choice: ");
            string typeChoice = Console.ReadLine();

            switch (typeChoice)
            {
                case "1":
                    cloudService.Type = ServiceType.SaaS;
                    break;
                case "2":
                    cloudService.Type = ServiceType.PaaS;
                    break;
                case "3":
                    cloudService.Type = ServiceType.IaaS;
                    break;
                default:
                    Console.WriteLine("Invalid type. Defaulting to SaaS.");
                    cloudService.Type = ServiceType.SaaS;
                    break;
            }

            Console.Write("Description: ");
            cloudService.Description = Console.ReadLine();

            Console.Write("Provider: ");
            cloudService.Provider = Console.ReadLine();

            Console.Write("Website URL: ");
            cloudService.WebsiteUrl = Console.ReadLine();

            Console.Write("Monthly Cost (leave blank if unknown): ");
            string costInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(costInput) && decimal.TryParse(costInput, out decimal cost))
            {
                cloudService.MonthlyCost = cost;
            }

            Console.Write("Has Free Version (y/n): ");
            string freeVersionInput = Console.ReadLine();
            cloudService.HasFreeVersion = freeVersionInput?.ToLower() == "y";

            Console.Write("Features (comma-separated): ");
            cloudService.Features = Console.ReadLine();

            bool success = await _cloudServiceService.AddCloudServiceAsync(cloudService);

            if (success)
            {
                Console.WriteLine("\nCloud service added successfully!");
            }
            else
            {
                Console.WriteLine("\nFailed to add cloud service.");
            }

            Console.WriteLine("Press any key to return to the main menu...");
            Console.ReadKey();
        }

        private async Task EditCloudServiceAsync()
        {
            Console.Clear();
            Console.WriteLine("=================================");
            Console.WriteLine("      EDIT CLOUD SERVICE        ");
            Console.WriteLine("=================================");
            Console.Write("Enter the ID of the service to edit: ");

            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID. Press any key to continue...");
                Console.ReadKey();
                return;
            }

            var cloudService = await _cloudServiceService.GetCloudServiceByIdAsync(id);

            if (cloudService == null)
            {
                Console.WriteLine("Service not found. Press any key to continue...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"\nEditing service: {cloudService.Name}");

            Console.Write($"Name [{cloudService.Name}]: ");
            string name = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(name))
                cloudService.Name = name;

            Console.WriteLine($"Current Type: {cloudService.Type}");
            Console.WriteLine("1. Software as a Service (SaaS)");
            Console.WriteLine("2. Platform as a Service (PaaS)");
            Console.WriteLine("3. Infrastructure as a Service (IaaS)");
            Console.Write("Enter your choice (leave blank to keep current): ");
            string typeChoice = Console.ReadLine();

            switch (typeChoice)
            {
                case "1":
                    cloudService.Type = ServiceType.SaaS;
                    break;
                case "2":
                    cloudService.Type = ServiceType.PaaS;
                    break;
                case "3":
                    cloudService.Type = ServiceType.IaaS;
                    break;
            }

            Console.Write($"Description [{cloudService.Description}]: ");
            string description = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(description))
                cloudService.Description = description;

            Console.Write($"Provider [{cloudService.Provider}]: ");
            string provider = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(provider))
                cloudService.Provider = provider;

            Console.Write($"Website URL [{cloudService.WebsiteUrl}]: ");
            string websiteUrl = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(websiteUrl))
                cloudService.WebsiteUrl = websiteUrl;

            Console.Write($"Monthly Cost [{cloudService.MonthlyCost}] (leave blank to keep current): ");
            string costInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(costInput) && decimal.TryParse(costInput, out decimal cost))
            {
                cloudService.MonthlyCost = cost;
            }

            Console.Write($"Has Free Version [{(cloudService.HasFreeVersion ? "y" : "n")}] (y/n): ");
            string freeVersionInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(freeVersionInput))
                cloudService.HasFreeVersion = freeVersionInput.ToLower() == "y";

            Console.Write($"Features [{cloudService.Features}]: ");
            string features = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(features))
                cloudService.Features = features;

            bool success = await _cloudServiceService.UpdateCloudServiceAsync(cloudService);

            if (success)
            {
                Console.WriteLine("\nCloud service updated successfully!");
            }
            else
            {
                Console.WriteLine("\nFailed to update cloud service.");
            }

            Console.WriteLine("Press any key to return to the main menu...");
            Console.ReadKey();
        }

        private async Task DeleteCloudServiceAsync()
        {
            Console.Clear();
            Console.WriteLine("=================================");
            Console.WriteLine("     DELETE CLOUD SERVICE       ");
            Console.WriteLine("=================================");
            Console.Write("Enter the ID of the service to delete: ");

            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID. Press any key to continue...");
                Console.ReadKey();
                return;
            }

            var cloudService = await _cloudServiceService.GetCloudServiceByIdAsync(id);

            if (cloudService == null)
            {
                Console.WriteLine("Service not found. Press any key to continue...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"\nAre you sure you want to delete the service: {cloudService.Name}? (y/n)");
            string confirm = Console.ReadLine();

            if (confirm?.ToLower() != "y")
            {
                Console.WriteLine("Deletion cancelled. Press any key to continue...");
                Console.ReadKey();
                return;
            }

            bool success = await _cloudServiceService.DeleteCloudServiceAsync(id);

            if (success)
            {
                Console.WriteLine("\nCloud service deleted successfully!");
            }
            else
            {
                Console.WriteLine("\nFailed to delete cloud service.");
            }

            Console.WriteLine("Press any key to return to the main menu...");
            Console.ReadKey();
        }

        private async Task UserManagementMenuAsync()
        {
            bool back = false;

            while (!back)
            {
                Console.Clear();
                Console.WriteLine("=================================");
                Console.WriteLine("      USER MANAGEMENT           ");
                Console.WriteLine("=================================");
                Console.WriteLine("1. View All Users");
                Console.WriteLine("2. Add New User");
                Console.WriteLine("3. Edit User");
                Console.WriteLine("4. Delete User");
                Console.WriteLine("0. Back to Main Menu");
                Console.WriteLine("=================================");
                Console.Write("Enter your choice: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await ViewAllUsersAsync();
                        break;
                    case "2":
                        await AddUserAsync();
                        break;
                    case "3":
                        await EditUserAsync();
                        break;
                    case "4":
                        await DeleteUserAsync();
                        break;
                    case "0":
                        back = true;
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Press any key to continue...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private async Task ViewAllUsersAsync()
        {
            Console.Clear();
            Console.WriteLine("=================================");
            Console.WriteLine("          ALL USERS             ");
            Console.WriteLine("=================================");

            var users = await _userService.GetAllUsersAsync();

            Console.WriteLine($"{"ID",-5}{"Username",-15}{"Email",-30}{"Role",-10}{"Active",-10}{"Created",-20}");
            Console.WriteLine(new string('-', 90));

            foreach (var user in users)
            {
                Console.WriteLine($"{user.Id,-5}{user.Username,-15}{user.Email,-30}{user.Role,-10}{(user.IsActive ? "Yes" : "No"),-10}{user.DateCreated.ToShortDateString(),-20}");
            }

            Console.WriteLine("\nPress any key to return to the user management menu...");
            Console.ReadKey();
        }

        private async Task AddUserAsync()
        {
            Console.Clear();
            Console.WriteLine("=================================");
            Console.WriteLine("          ADD USER              ");
            Console.WriteLine("=================================");

            var user = new User();

            Console.Write("Username: ");
            user.Username = Console.ReadLine();

            Console.Write("Email: ");
            user.Email = Console.ReadLine();

            Console.Write("Password: ");
            string password = ReadPassword();
            Console.WriteLine();

            Console.WriteLine("Role:");
            Console.WriteLine("1. Admin");
            Console.WriteLine("2. Editor");
            Console.WriteLine("3. Viewer");
            Console.Write("Enter your choice: ");
            string roleChoice = Console.ReadLine();

            switch (roleChoice)
            {
                case "1":
                    user.Role = UserRole.Admin;
                    break;
                case "2":
                    user.Role = UserRole.Editor;
                    break;
                case "3":
                    user.Role = UserRole.Viewer;
                    break;
                default:
                    Console.WriteLine("Invalid role. Defaulting to Viewer.");
                    user.Role = UserRole.Viewer;
                    break;
            }

            user.IsActive = true;

            try
            {
                bool success = await _userService.RegisterUserAsync(user, password);

                if (success)
                {
                    Console.WriteLine("\nUser added successfully!");
                }
                else
                {
                    Console.WriteLine("\nFailed to add user.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
            }

            Console.WriteLine("Press any key to return to the user management menu...");
            Console.ReadKey();
        }

        private async Task EditUserAsync()
        {
            Console.Clear();
            Console.WriteLine("=================================");
            Console.WriteLine("          EDIT USER             ");
            Console.WriteLine("=================================");
            Console.Write("Enter the ID of the user to edit: ");

            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID. Press any key to continue...");
                Console.ReadKey();
                return;
            }

            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
            {
                Console.WriteLine("User not found. Press any key to continue...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"\nEditing user: {user.Username}");

            Console.Write($"Email [{user.Email}]: ");
            string email = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(email))
                user.Email = email;

            Console.WriteLine($"Current Role: {user.Role}");
            Console.WriteLine("1. Admin");
            Console.WriteLine("2. Editor");
            Console.WriteLine("3. Viewer");
            Console.Write("Enter your choice (leave blank to keep current): ");
            string roleChoice = Console.ReadLine();

            switch (roleChoice)
            {
                case "1":
                    user.Role = UserRole.Admin;
                    break;
                case "2":
                    user.Role = UserRole.Editor;
                    break;
                case "3":
                    user.Role = UserRole.Viewer;
                    break;
            }

            Console.Write($"Active [{(user.IsActive ? "y" : "n")}] (y/n): ");
            string activeInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(activeInput))
                user.IsActive = activeInput.ToLower() == "y";

            bool success = await _userService.UpdateUserAsync(user);

            if (success)
            {
                Console.WriteLine("\nUser updated successfully!");
            }
            else
            {
                Console.WriteLine("\nFailed to update user.");
            }

            Console.WriteLine("Press any key to return to the user management menu...");
            Console.ReadKey();
        }

        private async Task DeleteUserAsync()
        {
            Console.Clear();
            Console.WriteLine("=================================");
            Console.WriteLine("         DELETE USER            ");
            Console.WriteLine("=================================");
            Console.Write("Enter the ID of the user to delete: ");

            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID. Press any key to continue...");
                Console.ReadKey();
                return;
            }

            if (id == _currentUser.Id)
            {
                Console.WriteLine("You cannot delete your own account. Press any key to continue...");
                Console.ReadKey();
                return;
            }

            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
            {
                Console.WriteLine("User not found. Press any key to continue...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"\nAre you sure you want to delete the user: {user.Username}? (y/n)");
            string confirm = Console.ReadLine();

            if (confirm?.ToLower() != "y")
            {
                Console.WriteLine("Deletion cancelled. Press any key to continue...");
                Console.ReadKey();
                return;
            }

            bool success = await _userService.DeleteUserAsync(id);

            if (success)
            {
                Console.WriteLine("\nUser deleted successfully!");
            }
            else
            {
                Console.WriteLine("\nFailed to delete user.");
            }

            Console.WriteLine("Press any key to return to the user management menu...");
            Console.ReadKey();
        }

        private void DisplayCloudServices(IEnumerable<CloudService> services)
        {
            if (!services.Any())
            {
                Console.WriteLine("No services found.");
                return;
            }

            Console.WriteLine($"{"ID",-5}{"Name",-20}{"Type",-10}{"Provider",-20}{"Cost",-10}{"Free Version",-15}");
            Console.WriteLine(new string('-', 80));

            foreach (var service in services)
            {
                string cost = service.MonthlyCost.HasValue ? $"${service.MonthlyCost:F2}" : "N/A";
                Console.WriteLine($"{service.Id,-5}{service.Name,-20}{service.Type,-10}{service.Provider,-20}{cost,-10}{(service.HasFreeVersion ? "Yes" : "No"),-15}");
            }

            Console.WriteLine("\nEnter a service ID to view details (or press Enter to go back): ");
            string input = Console.ReadLine();

            if (int.TryParse(input, out int serviceId))
            {
                var selectedService = services.FirstOrDefault(s => s.Id == serviceId);
                if (selectedService != null)
                {
                    DisplayCloudServiceDetails(selectedService);
                }
            }
        }

        private void DisplayCloudServiceDetails(CloudService service)
        {
            Console.Clear();
            Console.WriteLine("=================================");
            Console.WriteLine("      SERVICE DETAILS           ");
            Console.WriteLine("=================================");
            Console.WriteLine($"ID: {service.Id}");
            Console.WriteLine($"Name: {service.Name}");
            Console.WriteLine($"Type: {service.Type}");
            Console.WriteLine($"Provider: {service.Provider}");
            Console.WriteLine($"Description: {service.Description}");
            Console.WriteLine($"Website: {service.WebsiteUrl}");
            Console.WriteLine($"Monthly Cost: {(service.MonthlyCost.HasValue ? $"${service.MonthlyCost:F2}" : "N/A")}");
            Console.WriteLine($"Free Version: {(service.HasFreeVersion ? "Yes" : "No")}");
            Console.WriteLine($"Features: {service.Features}");
            Console.WriteLine($"Date Added: {service.DateAdded.ToShortDateString()}");

            if (service.LastUpdated.HasValue)
                Console.WriteLine($"Last Updated: {service.LastUpdated.Value.ToShortDateString()}");

            Console.WriteLine("\nPress any key to go back...");
            Console.ReadKey();
        }

        private string ReadPassword()
        {
            string password = "";
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true);

                if (key.Key != ConsoleKey.Enter && key.Key != ConsoleKey.Backspace)
                {
                    password += key.KeyChar;
                    Console.Write("*");
                }
                else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password = password.Substring(0, password.Length - 1);
                    Console.Write("\b \b");
                }
            } while (key.Key != ConsoleKey.Enter);

            return password;
        }
    }
}
