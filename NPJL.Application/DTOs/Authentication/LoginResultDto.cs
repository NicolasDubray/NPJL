using System;
using System.Collections.Generic;
using System.Text;

namespace NPJL.Application.DTOs.Authentication
{
    public class LoginResultDto
    {
        public bool Succeeded { get; set; }

        public string Message { get; set; } = string.Empty;

        public string? Token { get; set; }

        public string? Email { get; set; }

        public IList<string> Roles { get; set; } = new List<string>();
    }
}