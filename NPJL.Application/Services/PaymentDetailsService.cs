using NPJL.Application.DTOs.Customers;
using NPJL.Application.Interfaces;
using NPJL.Domain.Entities;

namespace NPJL.Application.Services;

public class PaymentDetailsService(IPaymentDetailsRepository _paymentDetailsRepository)
{
    public async Task<List<PaymentDetailsDto>> GetByCustomerIdAsync(int customerId)
    {
        var paymentDetails = await _paymentDetailsRepository.GetByCustomerIdAsync(customerId);

        return paymentDetails
            .Select(p => new PaymentDetailsDto
            {
                PaymentMethod = p.PaymentMethod,
                PaymentReference = p.PaymentReference,
                IsActive = p.IsActive
            })
            .ToList();
    }

    public async Task UpdateAsync(int customerId, UpdatePaymentDetailsDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.PaymentMethod)
            || string.IsNullOrWhiteSpace(dto.PaymentReference))
        {
            throw new ArgumentException("Payment method and reference are required");
        }

        var existing = (await _paymentDetailsRepository.GetByCustomerIdAsync(customerId))
            .FirstOrDefault();

        if (existing == null)
        {
            await _paymentDetailsRepository.CreateAsync(new PaymentDetails
            {
                CustomerId = customerId,
                PaymentMethod = dto.PaymentMethod.Trim(),
                PaymentReference = dto.PaymentReference.Trim(),
                IsActive = dto.IsActive
            });

            return;
        }

        existing.PaymentMethod = dto.PaymentMethod.Trim();
        existing.PaymentReference = dto.PaymentReference.Trim();
        existing.IsActive = dto.IsActive;

        await _paymentDetailsRepository.UpdateAsync(existing);
    }
}
