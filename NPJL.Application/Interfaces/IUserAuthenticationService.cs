using System;
using System.Collections.Generic;
using System.Text;
using NPJL.Application.DTOs.Authentication;

namespace NPJL.Application.Interfaces
{
    public interface IUserAuthenticationService
    {
        Task<LoginResultDto> LoginAsync(LoginDto request);
    }
}