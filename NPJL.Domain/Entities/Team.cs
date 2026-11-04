using System;
using System.Collections.Generic;
using System.Text;


namespace NPJL.Domain.Entities
{
    public class Team
    {
        // Primärnyckel
        public int Id { get; set; }

        // Navigationsegenskaper
        public ICollection<TeamEmployee> TeamEmployees { get; set; }
            = new List<TeamEmployee>();

        public ICollection<BookingTeam> BookingTeams { get; set; }
            = new List<BookingTeam>();
    }
}
