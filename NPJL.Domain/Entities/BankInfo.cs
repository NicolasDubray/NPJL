using NPJL.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;


namespace NPJL.Domain.Entities
{
    public class BankInfo
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }

        public string AccountHolderName { get; set; } = string.Empty;

        // Individual eller Company
        public AccountHolderType AccountHolderType { get; set; }

        public string BankName { get; set; } = string.Empty;
        public string BankAccountNumber { get; set; } = string.Empty;
        public string ClearingNumber { get; set; } = string.Empty;

        public Employee Employee { get; set; } = null!;
    }
}
