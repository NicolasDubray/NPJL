
using NPJL.Domain.Enums;

namespace NPJL.Application.DTOs.Employees
{
    // DTO for updating existing employee banking details.
    public sealed record UpdateBankInfoDto
    {
        public string AccountHolderName { get; init; } = string.Empty;

        public AccountHolderType AccountHolderType { get; init; }

        public string BankName { get; init; } = string.Empty;

        public string BankAccountNumber { get; init; } = string.Empty;

        public string ClearingNumber { get; init; } = string.Empty;
    }
}
