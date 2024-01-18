using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Data.Entities;
using System;
using Data;
using Application.Projects;
using Application.Users;
using Application.TimeEntries;
using Microsoft.OpenApi.Models;
using Application.Customers;

namespace API
{
	public class Startup
	{
		private readonly IWebHostEnvironment _environment;
		public IConfigurationRoot Configuration { get; }

		public Startup(IWebHostEnvironment env)
		{
			_environment = env;

			var builder = new ConfigurationBuilder()
				.SetBasePath(env.ContentRootPath)
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
				.AddEnvironmentVariables();
			Configuration = builder.Build();
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			// Add framework services.
			services.AddDbContext<DataContext>(opt => opt.UseInMemoryDatabase("e-conomic interview"));
			services.AddLogging(builder =>
			{
				builder.AddConsole();
				builder.AddDebug();
			});

			// Add services required for swagger to work
            services.AddControllers();
			services.AddDateOnlyTimeOnlyStringConverters();
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo
				{
					Title = "Timelogger API",
					Version = "v1"
				});
            });

            // Add services
            services.AddScoped<ProjectService>();
			services.AddScoped<UserService>();
			services.AddScoped<TimeEntryService>();
			services.AddScoped<CustomerService>();

			if (_environment.IsDevelopment())
			{
				services.AddCors();
			}
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseCors(builder => builder
					.AllowAnyMethod()
					.AllowAnyHeader()
					.SetIsOriginAllowed(origin => true)
					.AllowCredentials());
			}

			// .NET 6 routing
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

			// Swagger
            app.UseSwagger();
			app.UseSwaggerUI();


            var serviceScopeFactory = app.ApplicationServices.GetService<IServiceScopeFactory>();
			using (var scope = serviceScopeFactory.CreateScope())
			{
				SeedDatabase(scope);
			}
		}

		private static void SeedDatabase(IServiceScope scope)
		{
			var context = scope.ServiceProvider.GetService<DataContext>();

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

			var dummyTimeEntry1 = new TimeEntry
			{
				ProjectId = 1,
				UserId = 1,
				Date = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(-1),
				Comment = "Scoping assignment with client",
				Hours = 1,
			};

            var dummyTimeEntry2 = new TimeEntry
            {
                ProjectId = 1,
                UserId = 1,
                Date = DateOnly.FromDateTime(DateTime.UtcNow),
                Comment = "Research for client",
                Hours = 4,
            };

            context.Company.Add(dummyCompany);
            context.User.Add(dummyUser1);
            context.User.Add(dummyUser2);
            context.Customer.Add(dummyCustomer1);
            context.Customer.Add(dummyCustomer2);
            context.Project.Add(dummyProject);
			context.TimeEntry.Add(dummyTimeEntry1);
			context.TimeEntry.Add(dummyTimeEntry2);

			context.SaveChanges();
		}
	}
}