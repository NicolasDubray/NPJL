
using NPJL.Domain.Enums;

namespace NPJL.Application.DTOs.Employees
{
    // DTO for creating banking details for an employee.
    public sealed record CreateBankInfoDto
    {
        public string AccountHolderName { get; init; } = string.Empty;

        public AccountHolderType AccountHolderType { get; init; }

        public string BankName { get; init; } = string.Empty;

        public string BankAccountNumber { get; init; } = string.Empty;

        public string ClearingNumber { get; init; } = string.Empty;
    }
}
