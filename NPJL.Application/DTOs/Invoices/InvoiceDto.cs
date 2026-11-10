namespace NPJL.Application.DTOs.Invoices
{
    public sealed record InvoiceDto
    {
        public string OCR { get; init; } = "";
        public decimal Amount { get; init; }
        public DateTime? SendDate { get; init; }
        public DateTime? ExpiryDate { get; init; }
        public string Status { get; init; } = "";
    }
}
