using NPJL.Application.DTOs.Requests;
using NPJL.Application.Interfaces;
using NPJL.Domain.Entities;

namespace NPJL.Application.Services;

public class CustomerRequestService(ICustomerRequestRepository customerRequestRepository)
{
    public async Task CreateAsync(CreateRequestDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Description))
        {
            throw new ArgumentException("Description is required");
        }

        var description = dto.Description.Trim();

        if (description.Length > 1000)
        {
            throw new ArgumentException("Description is too long");
        }

        var request = new CustomerRequest
        {
            CustomerId = dto.CustomerId,
            BookingId = dto.BookingId,
            Type = dto.Type,
            Description = description,
            CreatedAt = DateTime.UtcNow,
            IsReviewed = false
        };

        await customerRequestRepository.CreateAsync(request);
    }
}