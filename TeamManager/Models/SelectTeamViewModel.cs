using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeamManager.Models
{
    public class SelectTeamViewModel
    {
        public PlayerModel Player { get; set; }
        public IEnumerable<TeamModel> Teams { get; set; }
    }
}