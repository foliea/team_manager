using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TeamManager.Common;
using TeamManager.Repositories.Schema;
using TeamManager.Models;

namespace TeamManager.Repositories
{
    public class TeamRepository : ITeamRepository
    {
        public TeamRepository()
        {
        }

        public IEnumerable<TeamModel> GetTeams()
        {
            IList<TeamModel> teamList = new List<TeamModel>();
            using (var dataContext = new TeamManagerDataContext()) {
                var query = from team in dataContext.Team
                            select team;
                var teams = query.ToList();
                foreach (var teamData in teams)
                    teamList.Add(Mapper.ToTeamModel(teamData));
            }
            return teamList;
        }
        public TeamModel GetTeamById(int teamId)
        {
            using (var dataContext = new TeamManagerDataContext())
            {
                var query = from t in dataContext.Team
                            where t.Id == teamId
                            select t;
                var team = query.FirstOrDefault();
                return Mapper.ToTeamModel(team);
            }
        }

        public void InsertTeam(TeamModel team)
        {
            insertOrUpdate(team, update: false);
        }
        
        public void DeleteTeam(int teamId)
        {
            using (var dataContext = new TeamManagerDataContext())
            {
                Team team = dataContext.Team.Where(t => t.Id == teamId).SingleOrDefault();

                var players = dataContext.Player.Where(p => p.TeamId == teamId);
                foreach (var player in players)
                    player.TeamId = null;

                dataContext.Team.DeleteOnSubmit(team);
                dataContext.SubmitChanges();
            }
        }

        public void UpdateTeam(TeamModel team)
        {
            insertOrUpdate(team, update: true);
        }

        private void insertOrUpdate(TeamModel team, bool update)
        {
            using (var dataContext = new TeamManagerDataContext())
            {
                Team teamData;
                if (update)
                    teamData = dataContext.Team.Where(t => t.Id == team.Id).SingleOrDefault();
                else
                    teamData = new Team();
                teamData.Name = team.Name;
                if (!String.IsNullOrEmpty(team.Avatar))
                    teamData.Avatar = team.Avatar;
                if (!update)
                    dataContext.Team.InsertOnSubmit(teamData);
                dataContext.SubmitChanges();
            }
        }
    }
}