
using NPJL.Application.DTOs.Employees;
using NPJL.Application.Interfaces;
using NPJL.Domain.Entities;

namespace NPJL.Application.Services
{
    public class BankInfoService
    {
        private readonly IBankInfoRepository _bankInfoRepository;

        public BankInfoService(IBankInfoRepository bankInfoRepository)
        {
            _bankInfoRepository = bankInfoRepository;
        }

        // Hämtar samtliga bankuppgifter för en anställd.
        public async Task<IReadOnlyList<BankInfoDto>> GetByEmployeeIdAsync(
            int employeeId)
        {
            if (employeeId <= 0)
            {
                throw new ArgumentException("Invalid employee ID.");
            }

            var bankInfos =
                await _bankInfoRepository.GetByEmployeeIdAsync(employeeId);

            return bankInfos.Select(MapToDto).ToList();
        }

        // Skapar nya bankuppgifter för en anställd.
        public async Task<BankInfoDto> CreateAsync(
            int employeeId,
            CreateBankInfoDto request)
        {
            if (employeeId <= 0)
            {
                throw new ArgumentException("Invalid employee ID.");
            }

            ArgumentNullException.ThrowIfNull(request);

            ValidateBankInfo(
                request.AccountHolderName,
                request.BankName,
                request.BankAccountNumber,
                request.ClearingNumber,
                request.AccountHolderType);

            var bankInfo = new BankInfo
            {
                EmployeeId = employeeId,
                AccountHolderName = request.AccountHolderName.Trim(),
                AccountHolderType = request.AccountHolderType,
                BankName = request.BankName.Trim(),
                BankAccountNumber = request.BankAccountNumber.Trim(),
                ClearingNumber = request.ClearingNumber.Trim()
            };

            await _bankInfoRepository.CreateAsync(bankInfo);
            await _bankInfoRepository.SaveChangesAsync();

            return MapToDto(bankInfo);
        }

        // Uppdaterar befintliga bankuppgifter.
        public async Task<bool> UpdateAsync(
            int employeeId,
            int bankInfoId,
            UpdateBankInfoDto request)
        {
            if (employeeId <= 0 || bankInfoId <= 0)
            {
                throw new ArgumentException("Invalid ID.");
            }

            ArgumentNullException.ThrowIfNull(request);

            ValidateBankInfo(
                request.AccountHolderName,
                request.BankName,
                request.BankAccountNumber,
                request.ClearingNumber,
                request.AccountHolderType);

            var bankInfo =
                await _bankInfoRepository.GetByIdAsync(bankInfoId);

            if (bankInfo == null || bankInfo.EmployeeId != employeeId)
            {
                return false;
            }

            bankInfo.AccountHolderName = request.AccountHolderName.Trim();
            bankInfo.AccountHolderType = request.AccountHolderType;
            bankInfo.BankName = request.BankName.Trim();
            bankInfo.BankAccountNumber = request.BankAccountNumber.Trim();
            bankInfo.ClearingNumber = request.ClearingNumber.Trim();

            await _bankInfoRepository.UpdateAsync(bankInfo);
            await _bankInfoRepository.SaveChangesAsync();

            return true;
        }

        // Validerar bankuppgifterna.
        private static void ValidateBankInfo(
            string accountHolderName,
            string bankName,
            string bankAccountNumber,
            string clearingNumber,
            NPJL.Domain.Enums.AccountHolderType accountHolderType)
        {
            if (string.IsNullOrWhiteSpace(accountHolderName))
                throw new ArgumentException("Account holder name is required.");

            if (string.IsNullOrWhiteSpace(bankName))
                throw new ArgumentException("Bank name is required.");

            if (string.IsNullOrWhiteSpace(bankAccountNumber))
                throw new ArgumentException("Bank account number is required.");

            if (string.IsNullOrWhiteSpace(clearingNumber))
                throw new ArgumentException("Clearing number is required.");

            if (!Enum.IsDefined(accountHolderType))
                throw new ArgumentException("Invalid account holder type.");
        }

        // Mappar entiteten till en DTO.
        private static BankInfoDto MapToDto(BankInfo bankInfo)
        {
            return new BankInfoDto
            {
                Id = bankInfo.Id,
                AccountHolderName = bankInfo.AccountHolderName,
                AccountHolderType = bankInfo.AccountHolderType.ToString(),
                BankName = bankInfo.BankName,
                BankAccountNumber = bankInfo.BankAccountNumber,
                ClearingNumber = bankInfo.ClearingNumber
            };
        }
    }
}
