using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeamManager.Models
{
    public class PlayerRepository : IPlayerRepository
    {
        public PlayerRepository()
        {
            
        }
        public IEnumerable<PlayerModel> GetPlayers()
        {
            IList<PlayerModel> playerList = new List<PlayerModel>();

            using (var dataContext = new TeamManagerDataContext()) {
                var query = from player in dataContext.Player
                            select player;
                var players = query.ToList();
                foreach (var playerData in players)
                {
                    playerList.Add(new PlayerModel()
                    {
                        Id = playerData.Id,
                        TeamId = playerData.TeamId,
                        Name = playerData.Name,
                        Avatar = playerData.Avatar,
                        Win = playerData.Win,
                        Loss = playerData.Loss,
                        Tie = playerData.Tie,
                        Team = getTeamModel(playerData.Team)
                    });
                }
            }
            return playerList;
        }
        public PlayerModel GetPlayerById(int playerId)
        {
            using (var dataContext = new TeamManagerDataContext())
            {
                var query = from p in dataContext.Player
                            where p.Id == playerId
                            select p;
                var player = query.FirstOrDefault();
                var model = new PlayerModel()
                {
                    Id = playerId,
                    TeamId = player.TeamId,
                    Name = player.Name,
                    Avatar = player.Avatar,
                    Win = player.Win,
                    Loss = player.Loss,
                    Tie = player.Tie,
                    Team = getTeamModel(player.Team)
                };
                return model;
            }
        }

        private TeamModel getTeamModel(Team team)
        {
            if (team == null) return null;
            return new TeamModel()
            {
                Id = team.Id,
                Name = team.Name,
                Avatar = team.Avatar
            };
        }
        
        public void InsertPlayer(PlayerModel player)
        {
            using (var dataContext = new TeamManagerDataContext())
            {
                var playerData = new Player()
                {
                    TeamId = player.TeamId,
                    Name = player.Name,
                    Avatar = player.Avatar,
                    Win = player.Win,
                    Loss = player.Loss,
                    Tie = player.Tie
                };
                dataContext.Player.InsertOnSubmit(playerData);
                dataContext.SubmitChanges();
            }
        }

        public void DeletePlayer(int playerId)
        {
            using (var dataContext = new TeamManagerDataContext())
            {
                Player player = dataContext.Player.Where(p => p.Id == playerId).SingleOrDefault();
                dataContext.Player.DeleteOnSubmit(player);
                dataContext.SubmitChanges();
            }
        }

        public void UpdatePlayer(PlayerModel player)
        {
            using (var dataContext = new TeamManagerDataContext())
            {
                Player playerData = dataContext.Player.Where(p => p.Id == player.Id).SingleOrDefault();
                playerData.TeamId = player.TeamId;
                playerData.Name = player.Name;
                playerData.Avatar = player.Avatar;
                playerData.Win = player.Win;
                playerData.Loss = player.Loss;
                playerData.Tie = player.Tie;
                dataContext.SubmitChanges();
            }
        }
    }
}