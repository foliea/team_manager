using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeamManager.Models
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
                {
                    teamList.Add(new TeamModel()
                    {
                        Id = teamData.Id,
                        Name = teamData.Name,
                        Avatar = teamData.Avatar,
                        Players = teamData.Player.Select(player => getPlayerModel(player)).ToList()
                    });
                }
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
                var model = new TeamModel()
                {
                    Id = teamId,
                    Name = team.Name,
                    Avatar = team.Avatar,
                    Players = team.Player.Select(player => getPlayerModel(player)).ToList()
                };
                return model;
            }
        }

        private PlayerModel getPlayerModel(Player player)
        {
            return new PlayerModel()
            {
                Id = player.Id,
                TeamId = player.TeamId,
                Name = player.Name,
                Avatar = player.Avatar,
                Win = player.Win,
                Loss = player.Loss,
                Tie = player.Tie
            };
        }

        public void InsertTeam(TeamModel team)
        {
            using (var dataContext = new TeamManagerDataContext())
            {
                var teamData = new Team()
                {
                    Name = team.Name,
                    Avatar = team.Avatar
                };
                dataContext.Team.InsertOnSubmit(teamData);
                dataContext.SubmitChanges();
            }
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
            using (var dataContext = new TeamManagerDataContext())
            {
                Team teamData = dataContext.Team.Where(t => t.Id == team.Id).SingleOrDefault();
                teamData.Name = team.Name;
                teamData.Avatar = team.Avatar;
                dataContext.SubmitChanges();
            }
        }
    }
}