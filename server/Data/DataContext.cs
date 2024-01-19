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
	}
}
