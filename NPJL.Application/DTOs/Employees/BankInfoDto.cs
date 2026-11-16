
namespace NPJL.Application.DTOs.Employees
{
    // DTO for displaying an employee's banking details.
    public sealed record BankInfoDto
    {
        public int Id { get; init; }

        public string AccountHolderName { get; init; } = string.Empty;

        public string AccountHolderType { get; init; } = string.Empty;

        public string BankName { get; init; } = string.Empty;

        public string BankAccountNumber { get; init; } = string.Empty;

        public string ClearingNumber { get; init; } = string.Empty;
    }
}
