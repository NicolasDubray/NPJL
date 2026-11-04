using System;
using System.Collections.Generic;
using System.Text;


namespace NPJL.Domain.Entities
{
    public class Service
    {
        // Primärnyckel
        public int Id { get; set; }

        // Tjänstens namn eller typ
        public string Type { get; set; } = string.Empty;

        // Tjänstens grundkostnad
        public decimal Cost { get; set; }
    }
}
