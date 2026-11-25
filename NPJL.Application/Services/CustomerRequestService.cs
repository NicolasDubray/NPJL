using NPJL.Application.DTOs.Requests;
using NPJL.Application.Interfaces;
using NPJL.Domain.Entities;
using NPJL.Domain.Enums;

namespace NPJL.Application.Services;

public class CustomerRequestService(ICustomerRequestRepository customerRequestRepository)
{
    public async Task CreateAsync(CreateRequestDto dto)
    {
        if (dto.BookingId.HasValue == dto.InvoiceId.HasValue)
        {
            throw new ArgumentException("Request must be for either a booking or an invoice");
        }

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
            InvoiceId = dto.InvoiceId,
            Type = dto.Type,
            Description = description,
            CreatedAt = DateTime.UtcNow,
            
        };

        await customerRequestRepository.CreateAsync(request);
    }
    public async Task<IReadOnlyList<CustomerRequest>> GetAllAsync()
    {
        return await customerRequestRepository.GetAllAsync();
    }
    public async Task UpdateStatusAsync(int requestId, CustomerRequestStatus status)
    {
        if (!Enum.IsDefined(status))
        {
            throw new ArgumentException("Invalid request status");
        }

        var request = await customerRequestRepository.GetByIdAsync(requestId);

        if (request == null)
        {
            throw new KeyNotFoundException("Customer request not found");
        }

        request.Status = status;

        await customerRequestRepository.UpdateAsync(request);
    }
}