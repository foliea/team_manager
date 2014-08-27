using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeamManager.Models
{
    public class TeamRepository : ITeamRepository
    {
        private TeamManagerDataContext _dataContext;
        public TeamRepository()
        {
            _dataContext = new TeamManagerDataContext();
        }
        public IEnumerable<TeamModel> GetTeams()
        {
            IList<TeamModel> teamList = new List<TeamModel>();
            var query = from team in _dataContext.Team
                        select team;
            var teams = query.ToList();
            foreach (var teamData in teams)
            {
                teamList.Add(new TeamModel()
                {
                    Id = teamData.Id,
                    Name = teamData.Name,
                    Avatar = teamData.Avatar,
                });
            }
            return teamList;
        }
        public TeamModel GetTeamById(int teamId)
        {
            var query = from u in _dataContext.Team
                        where u.Id == teamId
                        select u;
            var team = query.FirstOrDefault();
            var model = new TeamModel()
            {
                Id = teamId,
                Name = team.Name,
                Avatar = team.Avatar,
                Players = team.Player.Select(player => new PlayerModel()
                {
                    Id = player.Id,
                    TeamId = player.TeamId,
                    Name = player.Name,
                    Avatar = player.Avatar,
                    Win = player.Win,
                    Loss = player.Loss,
                    Tie = player.Tie
                })
            };
            return model;
        }

        public void InsertTeam(TeamModel team)
        {
            var teamData = new Team()
            {
                Name = team.Name,
                Avatar = team.Avatar
            };
            _dataContext.Team.InsertOnSubmit(teamData);
            _dataContext.SubmitChanges();
        }
        
        public void DeleteTeam(int teamId)
        {
            Team team = _dataContext.Team.Where(u => u.Id == teamId).SingleOrDefault();
            _dataContext.Team.DeleteOnSubmit(team);
            _dataContext.SubmitChanges();
        }
        public void UpdateTeam(TeamModel team)
        {
            Team teamData = _dataContext.Team.Where(u => u.Id == team.Id).SingleOrDefault();
            teamData.Name = team.Name;
            teamData.Avatar = team.Avatar;
            _dataContext.SubmitChanges();
        }
    }
}