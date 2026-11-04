using System;
using System.Collections.Generic;
using System.Text;


namespace NPJL.Domain.Enums
{
    public enum InvoiceStatus
    {
        Draft,
        PendingApproval,
        Rejected,
        Approved,
        Sent,
        Paid,
        Expired
    }
}
