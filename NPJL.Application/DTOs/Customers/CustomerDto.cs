namespace NPJL.Application.DTOs.Customers
{
    public sealed record CustomerDto
    {
        public int Id { get; init; }
        public string Name { get; init; } = String.Empty;
        public ContactInfoDto? ContactInfo { get; init; }

    }
}
