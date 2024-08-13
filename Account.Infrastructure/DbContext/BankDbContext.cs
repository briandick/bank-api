using Bank.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Bank.Infrastructure.DbContext
{
    public class BankDbContext : Microsoft.EntityFrameworkCore.DbContext
    {

        public BankDbContext(DbContextOptions<BankDbContext> options) : base(options) { }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Deposit> Deposits { get; set; }

        public DbSet<Withdrawal> Withdrawals { get; set; }
    }

    public class TestSetup
    {
        public ServiceProvider SetupInMemoryDatabase()
        {
            // Create a new service collection
            var serviceCollection = new ServiceCollection();

            // Add the DbContext configured to use the In-Memory database
            serviceCollection.AddDbContext<BankDbContext>(options =>
                options.UseInMemoryDatabase("TestDatabase"));

            // Build the service provider
            return serviceCollection.BuildServiceProvider();
        }
    }

}
