using Microsoft.EntityFrameworkCore;
using NPJL.Domain.Entities;
using NPJL.Domain.Enums;

namespace NPJL.Infrastructure.Persistence
{
    public static class DevelopmentDataSeeder
    {
        public static async Task SeedAsync(
            ApplicationDbContext context,
            string customerEmail)
        {
            if (await context.Bookings.AnyAsync())
            {
                return;
            }

            var user = await context.Users
                .FirstOrDefaultAsync(u => u.Email == customerEmail);

            if (user?.CustomerId == null)
            {
                return;
            }

            var customerId = user.CustomerId.Value;
            var now = DateTime.UtcNow;

            var basic = new Service { Type = "Basic package", Cost = 1000m };
            var standard = new Service { Type = "Standard package", Cost = 2500m };
            var premium = new Service { Type = "Premium package", Cost = 5000m };
            context.Services.AddRange(basic, standard, premium);

            if (!await context.PaymentDetails.AnyAsync(p => p.CustomerId == customerId))
            {
                context.PaymentDetails.Add(new PaymentDetails
                {
                    CustomerId = customerId,
                    PaymentMethod = "Bank transfer",
                    PaymentReference = "1234-5678901",
                    IsActive = true
                });
            }

            var received = new Booking
            {
                CustomerId = customerId,
                StartTime = now.AddDays(10),
                EndTime = now.AddDays(10).AddHours(4),
                Status = BookingStatus.Received
            };
            received.BookingServices.Add(new BookingService { Service = basic, PriceAtBooking = 1000m });

            var inProgress = new Booking
            {
                CustomerId = customerId,
                StartTime = now.AddDays(-1),
                EndTime = now.AddDays(3),
                Status = BookingStatus.InProgress
            };
            inProgress.BookingServices.Add(new BookingService { Service = standard, PriceAtBooking = 2500m });

            var finishedOne = new Booking
            {
                CustomerId = customerId,
                StartTime = now.AddDays(-30),
                EndTime = now.AddDays(-28),
                Status = BookingStatus.Finished
            };
            finishedOne.BookingServices.Add(new BookingService { Service = premium, PriceAtBooking = 5000m });
            finishedOne.BookingServices.Add(new BookingService { Service = basic, PriceAtBooking = 1000m });

            var finishedTwo = new Booking
            {
                CustomerId = customerId,
                StartTime = now.AddDays(-60),
                EndTime = now.AddDays(-59),
                Status = BookingStatus.Finished
            };
            finishedTwo.BookingServices.Add(new BookingService { Service = standard, PriceAtBooking = 2400m });

            var finishedThree = new Booking
            {
                CustomerId = customerId,
                StartTime = now.AddDays(-5),
                EndTime = now.AddDays(-4),
                Status = BookingStatus.Finished
            };
            finishedThree.BookingServices.Add(new BookingService { Service = basic, PriceAtBooking = 900m });

            context.Bookings.AddRange(received, inProgress, finishedOne, finishedTwo, finishedThree);

            var sentInvoice = new Invoice
            {
                CustomerId = customerId,
                OCR = "1000000001",
                SendDate = now.AddDays(-27).Date,
                ExpiryDate = now.AddDays(3).Date,
                Status = InvoiceStatus.Sent,
                ApprovedByUserId = user.Id,
                ApprovedAt = now.AddDays(-27)
            };
            sentInvoice.InvoiceBookings.Add(new InvoiceBooking { Booking = finishedOne });

            var paidInvoice = new Invoice
            {
                CustomerId = customerId,
                OCR = "1000000002",
                SendDate = now.AddDays(-58).Date,
                ExpiryDate = now.AddDays(-28).Date,
                Status = InvoiceStatus.Paid,
                ApprovedByUserId = user.Id,
                ApprovedAt = now.AddDays(-58)
            };
            paidInvoice.InvoiceBookings.Add(new InvoiceBooking { Booking = finishedTwo });

            var draftInvoice = new Invoice
            {
                CustomerId = customerId,
                OCR = "1000000003",
                Status = InvoiceStatus.Draft
            };
            draftInvoice.InvoiceBookings.Add(new InvoiceBooking { Booking = finishedThree });

            context.Invoices.AddRange(sentInvoice, paidInvoice, draftInvoice);

            var others = new[]
            {
                new Customer { FirstName = "Erik", LastName = "Lindqvist", IsActive = true },
                new Customer { FirstName = "Maria", LastName = "Berg", IsActive = true },
                new Customer { FirstName = "Olof", LastName = "Nyström", CompanyName = "Nyström Bygg AB", IsCompany = true, IsActive = true }
            };

            others[0].ContactInfos.Add(new ContactInfo { Address = "Storgatan 1", Telephone = "0701111111", Email = "erik@exempel.se" });
            others[1].ContactInfos.Add(new ContactInfo { Address = "Lillgatan 2", Telephone = "0702222222", Email = "maria@exempel.se" });
            others[2].ContactInfos.Add(new ContactInfo { Address = "Industrivägen 3", Telephone = "0703333333", Email = "olof@exempel.se" });

            context.Customers.AddRange(others);

            await context.SaveChangesAsync();
        }
    }
}