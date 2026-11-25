using NPJL.Domain.Enums;

namespace NPJL.Application.DTOs.Requests;

public class UpdateRequestStatusDto
{
    public CustomerRequestStatus Status { get; set; }
}