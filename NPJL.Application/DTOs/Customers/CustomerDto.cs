namespace NPJL.Application.DTOs.Customers
{
    public sealed record CustomerDto
    {
        public int Id { get; init; }
        public string FirstName { get; init; } = String.Empty;
        public string LastName { get; init; } = String.Empty;
        public string Name => FirstName + " " + LastName;
        public ContactInfoDto? ContactInfo { get; init; }

    }
}
