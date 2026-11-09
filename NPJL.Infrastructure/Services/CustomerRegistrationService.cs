using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NPJL.Application.DTOs.Authentication;
using NPJL.Application.Interfaces;
using NPJL.Domain.Entities;
using NPJL.Infrastructure.Identity;
using NPJL.Infrastructure.Persistence;

namespace NPJL.Infrastructure.Services
{
    public class CustomerRegistrationService : ICustomerRegistrationService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CustomerRegistrationService(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<CustomerRegistrationResultDto> RegisterAsync(
            CustomerRegistrationDto request)
        {
            // Företagsnamn måste anges för företagskunder
            if (request.IsCompany &&
                string.IsNullOrWhiteSpace(request.CompanyName))
            {
                return new CustomerRegistrationResultDto
                {
                    Succeeded = false,
                    Message = "Company name is required."
                };
            }

            // Kontrollera om e-postadressen redan används
            var existingUser = await _userManager.FindByEmailAsync(
                request.Email);

            if (existingUser != null)
            {
                return new CustomerRegistrationResultDto
                {
                    Succeeded = false,
                    Message = "Email address is already registered."
                };
            }

            // Starta databastransaktion
            await using var transaction =
                await _context.Database.BeginTransactionAsync();

            try
            {
                // Skapa kund
                var customer = new Customer
                {
                    FirstName = request.FirstName.Trim(),
                    LastName = request.LastName.Trim(),
                    IsCompany = request.IsCompany,
                    CompanyName = request.IsCompany
                        ? request.CompanyName!.Trim()
                        : null,
                    IsActive = true
                };

                // Skapa kontaktuppgifter
                customer.ContactInfos.Add(new ContactInfo
                {
                    Address = request.Address.Trim(),
                    Telephone = request.Telephone.Trim(),
                    Email = request.Email.Trim()
                });

                // Spara kunden så att den får ett Id
                await _context.Customers.AddAsync(customer);
                await _context.SaveChangesAsync();

                // Skapa Identity-konto
                var user = new ApplicationUser
                {
                    UserName = request.Email.Trim(),
                    Email = request.Email.Trim(),
                    CustomerId = customer.Id
                };

                var createResult = await _userManager.CreateAsync(
                    user,
                    request.Password);

                if (!createResult.Succeeded)
                {
                    await transaction.RollbackAsync();

                    return new CustomerRegistrationResultDto
                    {
                        Succeeded = false,
                        Message = string.Join(
                            ", ",
                            createResult.Errors.Select(e => e.Description))
                    };
                }

                // Tilldela rollen Customer
                var roleResult = await _userManager.AddToRoleAsync(
                    user,
                    "Customer");

                if (!roleResult.Succeeded)
                {
                    await transaction.RollbackAsync();

                    return new CustomerRegistrationResultDto
                    {
                        Succeeded = false,
                        Message = string.Join(
                            ", ",
                            roleResult.Errors.Select(e => e.Description))
                    };
                }

                // Bekräfta transaktionen
                await transaction.CommitAsync();

                return new CustomerRegistrationResultDto
                {
                    Succeeded = true,
                    Message = "Customer registered successfully.",
                    CustomerId = customer.Id
                };
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}