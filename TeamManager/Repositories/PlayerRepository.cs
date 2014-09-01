using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TeamManager.Common;
using TeamManager.Repositories.Schema;
using TeamManager.Models;

namespace TeamManager.Repositories
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
                    playerList.Add(Mapper.ToPlayerModel(playerData));
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
                return Mapper.ToPlayerModel(player);
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
            insertOrUpdate(player, update: false);
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
            insertOrUpdate(player, update: true);
        }

        private void insertOrUpdate(PlayerModel player, bool update)
        {
            using (var dataContext = new TeamManagerDataContext())
            {
                Player playerData;
                if (update)
                    playerData = dataContext.Player.Where(t => t.Id == player.Id).SingleOrDefault();
                else
                    playerData = new Player();
                playerData.Name = player.Name;
                playerData.Avatar = player.Avatar;
                playerData.Win = player.Win;
                playerData.Loss = player.Loss;
                playerData.Tie = player.Tie;
                if (player.Team != null)
                    playerData.TeamId = player.Team.Id;
                if (!update)
                    dataContext.Player.InsertOnSubmit(playerData);
                dataContext.SubmitChanges();
            }
        }
    }
}