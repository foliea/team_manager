using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeamManager.Models
{
    public interface ITeamRepository
    {
        IEnumerable<TeamModel> GetTeams();
        TeamModel GetTeamById(int teamId);
        void InsertTeam(TeamModel team);
        void DeleteTeam(int teamId);
        void UpdateTeam(TeamModel team);
    }
}