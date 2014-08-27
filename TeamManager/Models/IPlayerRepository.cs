using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeamManager.Models
{
    public interface IPlayerRepository
    {
        IEnumerable<PlayerModel> GetPlayers();
        PlayerModel GetPlayerById(int playerId);
        void InsertPlayer(PlayerModel player);
        void DeletePlayer(int playerId);
        void UpdatePlayer(PlayerModel player);
    }
}