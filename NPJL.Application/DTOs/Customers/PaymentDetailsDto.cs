namespace NPJL.Application.DTOs.Customers;

public sealed record PaymentDetailsDto
{
    public string PaymentMethod { get; init; } = "";
    public string PaymentReference { get; init; } = "";
    public bool IsActive { get; init; } = false;
}
