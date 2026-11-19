namespace NPJL.Application.DTOs.Customers;

public sealed record ContactInfoDto
{
    public string Address { get; init; } = "";
    public string Telephone { get; init; } = "";
    public string Email { get; init; } = "";
}
