using System;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data
{
	public class DataContext : DbContext
	{
		public DataContext(DbContextOptions<DataContext> options)
			: base(options)
		{
		}

		public DbSet<Company> Company { get; set; }
		public DbSet<User> User { get; set; }
		public DbSet<Project> Project { get; set; }
		public DbSet<Customer> Customer { get; set; }
		public DbSet<TimeEntry> TimeEntry { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<TimeEntry>()
				.HasIndex(x => new { x.ProjectId, x.UserId, x.Date })
				.IsUnique();

			modelBuilder.Entity<Company>()
				.HasIndex(x => x.Name)
				.IsUnique();

			modelBuilder.Entity<User>()
				.HasIndex(x => new { x.CompanyId, x.Name })
				.IsUnique();

			modelBuilder.Entity<Customer>()
				.HasIndex(x => new { x.CompanyId, x.Name })
				.IsUnique();

			modelBuilder.Entity<Project>()
				.HasIndex(x => new { x.CompanyId, x.CustomerId, x.Name })
				.IsUnique();

			base.OnModelCreating(modelBuilder);
		}

		public void SeedDatabase()
		{
			var dummyCompany = new Company
			{
				CompanyId = 1,
				Name = "The Testing Company"
			};

			var dummyUser1 = new User
			{
				UserId = 1,
				Name = "Mr. Freelancer",
				Company = dummyCompany
			};

			var dummyUser2 = new User
			{
				UserId = 2,
				Name = "The other guy",
				Company = dummyCompany
			};

			var dummyCustomer1 = new Customer
			{
				CustomerId = 1,
				Name = "Visma A/S",
				Company = dummyCompany
			};

			var dummyCustomer2 = new Customer
			{
				CustomerId = 2,
				Name = "TV2 A/S",
				Company = dummyCompany
			};

			var dummyProject = new Project
			{
				ProjectId = 1,
				Customer = dummyCustomer1,
				Company = dummyCompany,
				Name = "Digital marketing 2024",
				Status = ProjectStatus.Open,
				StartDate = DateOnly.FromDateTime(DateTime.UtcNow),
				EndDate = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(2))
			};

			this.Company.Add(dummyCompany);
			this.User.Add(dummyUser1);
			this.User.Add(dummyUser2);
			this.Customer.Add(dummyCustomer1);
			this.Customer.Add(dummyCustomer2);
			this.Project.Add(dummyProject);

			this.SaveChanges();
		}
	}
}
