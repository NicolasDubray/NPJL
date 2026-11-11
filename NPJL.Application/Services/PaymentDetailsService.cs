using NPJL.Application.DTOs.Customers;
using NPJL.Application.Interfaces;

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
}
