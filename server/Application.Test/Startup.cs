using Application.Customers;
using Application.Projects;
using Application.TimeEntries;
using Application.Users;
using Data;
using Data.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Application.Test
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DataContext>(opt => opt.UseInMemoryDatabase("e-conomic interview test"));
            services.AddScoped<ProjectService>();
            services.AddScoped<UserService>();
            services.AddScoped<TimeEntryService>();
            services.AddScoped<CustomerService>();
            services.AddScoped<ProjectService>();
            services.AddScoped<CustomerService>();
        }
    }
}