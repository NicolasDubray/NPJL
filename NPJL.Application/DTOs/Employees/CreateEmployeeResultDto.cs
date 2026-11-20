namespace NPJL.Application.DTOs.Employees
{
    public sealed record CreateEmployeeResultDto
    {
        public bool Succeeded { get; init; }
        public string Message { get; init; } = string.Empty;
        public int? EmployeeId { get; init; }
    }
}