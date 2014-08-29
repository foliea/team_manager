using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeamManager.Models
{
    public class SelectPlayerViewModel
    {
        public TeamModel Team { get; set; }
        public IEnumerable<PlayerModel> Players { get; set; }
    }
}