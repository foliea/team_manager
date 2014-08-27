using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeamManager.Models
{
    public class PlayerRepository : IPlayerRepository
    {
        private TeamManagerDataContext _dataContext;
        public PlayerRepository()
        {
            _dataContext = new TeamManagerDataContext();
        }
        public IEnumerable<PlayerModel> GetPlayers()
        {
            IList<PlayerModel> playerList = new List<PlayerModel>();
            var query = from player in _dataContext.Player
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
                    Tie = playerData.Tie
                });
            }
            return playerList;
        }
        public PlayerModel GetPlayerById(int playerId)
        {
            var query = from u in _dataContext.Player
                        where u.Id == playerId
                        select u;
            var player = query.FirstOrDefault();
            var model = new PlayerModel()
            {
                Id = playerId,
                TeamId = player.TeamId,
                Name = player.Name,
                Avatar = player.Avatar,
                Win = player.Win,
                Loss = player.Loss,
                Tie = player.Tie
            };
            return model;
        }
        
        public void InsertPlayer(PlayerModel player)
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
            _dataContext.Player.InsertOnSubmit(playerData);
            _dataContext.SubmitChanges();
        }

        public void DeletePlayer(int playerId)
        {
            Player player = _dataContext.Player.Where(u => u.Id == playerId).SingleOrDefault();
            _dataContext.Player.DeleteOnSubmit(player);
            _dataContext.SubmitChanges();
        }

        public void UpdatePlayer(PlayerModel player)
        {
            Player playerData = _dataContext.Player.Where(u => u.Id == player.Id).SingleOrDefault();
            playerData.TeamId = player.TeamId;
            playerData.Name = player.Name;
            playerData.Avatar = player.Avatar;
            playerData.Win = player.Win;
            playerData.Loss = player.Loss;
            playerData.Tie = player.Tie;
            _dataContext.SubmitChanges();
        }
    }
}