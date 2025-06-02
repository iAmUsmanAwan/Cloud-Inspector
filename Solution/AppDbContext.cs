using System;

public class Class1
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<CloudService> CloudServices { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Cloud Service configuration
            modelBuilder.Entity<CloudService>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<CloudService>()
                .Property(c => c.Name)
                .IsRequired();

            modelBuilder.Entity<CloudService>()
                .Property(c => c.Type)
                .IsRequired();

            // User configuration
            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            // Seed data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed sample cloud services
            modelBuilder.Entity<CloudService>().HasData(
                new CloudService
                {
                    Id = 1,
                    Name = "Microsoft Azure",
                    Type = ServiceType.IaaS,
                    Description = "Microsoft's cloud computing platform",
                    Provider = "Microsoft",
                    WebsiteUrl = "https://azure.microsoft.com",
                    MonthlyCost = 50.00m,
                    HasFreeVersion = true,
                    Features = "Virtual Machines, Storage, Networking, Databases",
                    DateAdded = new System.DateTime(2023, 1, 1)
                },
                new CloudService
                {
                    Id = 2,
                    Name = "Salesforce",
                    Type = ServiceType.SaaS,
                    Description = "CRM platform",
                    Provider = "Salesforce.com",
                    WebsiteUrl = "https://salesforce.com",
                    MonthlyCost = 25.00m,
                    HasFreeVersion = false,
                    Features = "Sales Cloud, Service Cloud, Marketing Cloud",
                    DateAdded = new System.DateTime(2023, 1, 1)
                },
                new CloudService
                {
                    Id = 3,
                    Name = "Heroku",
                    Type = ServiceType.PaaS,
                    Description = "Cloud platform for deploying apps",
                    Provider = "Salesforce.com",
                    WebsiteUrl = "https://heroku.com",
                    MonthlyCost = 7.00m,
                    HasFreeVersion = true,
                    Features = "App deployment, Database, Monitoring",
                    DateAdded = new System.DateTime(2023, 1, 1)
                }
            );

            // Seed admin user (Password: Admin123!)
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Username = "admin",
                    Email = "admin@cloudservices.com",
                    PasswordHash = "BB1C5F7264A35BC47A9DE9387F5D37BC26A0F44AD894B72D3664D56C45CC48DF", // "Admin123!" with salt
                    Salt = "ABCDEF123456",
                    Role = UserRole.Admin,
                    IsActive = true,
                    DateCreated = System.DateTime.Now
                }
            );
        }
    }
}
