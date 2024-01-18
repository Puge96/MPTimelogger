using Application.Customers.Models;
using Application.Projects.Models;
using Application.Users;
using Data;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Customers
{
    public class CustomerService
    {
        private readonly DataContext dataContext;
        private readonly UserService userService;

        public CustomerService(DataContext dataContext, UserService userService)
        {
            this.dataContext = dataContext;
            this.userService = userService;
        }

        public async Task<CustomerModelResult> Single(int userId, int customerId, CancellationToken cancellationToken)
        {
            var result = new CustomerModelResult();

            var userResult = await userService.Single(userId, cancellationToken);
            if (!userResult.IsValid)
            {
                result.Errors.AddRange(userResult.Errors);
                return result;
            }

            var customer = await dataContext.Customer.FirstOrDefaultAsync(x => x.CustomerId == customerId, cancellationToken);
            if (customer == null)
            {
                result.Errors.Add("Customer does not exist or you don't have the required access to view it.");
                return result;
            }

            result.Customer = new CustomerDTO(customer);
            return result;
        }

        public async Task<CustomersModelResult> List(int userId, CancellationToken cancellationToken)
        {
            var result = new CustomersModelResult();

            var userResult = await userService.Single(userId, cancellationToken);
            if (!userResult.IsValid)
            {
                result.Errors.AddRange(userResult.Errors);
                return result;
            }

            var customers = await dataContext.Customer.Where(x => x.CompanyId == userResult.User!.CompanyId).ToListAsync(cancellationToken);
            if (customers != null)
            {
                result.Customers = customers.Select(x => new CustomerDTO(x)).ToList();
            }

            return result;
        }

        public async Task<CustomerModelResult> Create(CustomerCreateModel model, CancellationToken cancellationToken)
        {
            var result = new CustomerModelResult();

            var userResult = await userService.Single(model.UserId, cancellationToken);
            if (!userResult.IsValid)
            {
                result.Errors.AddRange(userResult.Errors);
                return result;
            }

            if (await dataContext.Customer.AnyAsync(x => x.CompanyId == model.CompanyId && x.Name == model.Name, cancellationToken))
            {
                result.Errors.Add("A customer with this name already exists.");
                return result;
            }

            if (userResult.User!.CompanyId != model.CompanyId)
            {
                result.Errors.Add("You do not currently have access to this company.");
                return result;
            }

            if (result.IsValid)
            {
                var customer = new Customer
                {
                    CompanyId = model.CompanyId,
                    Name = model.Name
                };

                await dataContext.Customer.AddAsync(customer, cancellationToken);
                await dataContext.SaveChangesAsync(cancellationToken);

                result.Customer = new CustomerDTO(customer);
            }

            return result;
        } 
    }
}
