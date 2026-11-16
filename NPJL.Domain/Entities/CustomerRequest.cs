using NPJL.Domain.Enums;

namespace NPJL.Domain.Entities;

public class CustomerRequest
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public int BookingId { get; set; }
    public Customer? Customer { get; set; }
    public Booking? Booking { get; set; }
    public RequestType Type { get; set; }
    public string Description { get; set; } = "";
    public DateTime CreatedAt { get; set; }
    public bool IsReviewed { get; set; }

}
