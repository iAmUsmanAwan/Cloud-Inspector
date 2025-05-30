using System;
using UMS.BLL.Shared;

namespace UMS.ConsoleApplication
{
    internal class Program
    {
        static void Main()
        {
            var service = new UserService();

            Console.Write("Enter name: ");
            string name = Console.ReadLine();

            Console.Write("Enter email: ");
            string email = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(email))
            {
                bool success = service.RegisterUser(name, email); // 🔧 
                Console.WriteLine(success ? "User registered!" : "Registration failed.");
            }
            else
            {
                Console.WriteLine("Name and Email cannot be empty!");
            }

            Console.WriteLine("\nAll Users:");
            foreach (var user in service.GetUsers())
            {
                Console.WriteLine($"{user.Id}: {user.Name} - {user.Email}");
            }
        }
    }
}
