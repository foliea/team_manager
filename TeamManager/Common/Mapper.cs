using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TeamManager.Repositories.Schema;
using TeamManager.Models;

namespace TeamManager.Common
{
    public class Mapper
    {
        public static PlayerModel ToPlayerModel(Player player, bool eagerLoading = true)
        {
            if (player == null) return null;
            var model = new PlayerModel()
            {
                Id = player.Id,
                TeamId = player.TeamId,
                Name = player.Name,
                Avatar = player.Avatar,
                Win = player.Win,
                Loss = player.Loss,
                Tie = player.Tie, 
            };
            if (eagerLoading)
                model.Team = ToTeamModel(player.Team, false);
            return model;
        }

        public static TeamModel ToTeamModel(Team team, bool eagerLoading = true)
        {
            if (team == null) return null;
            var model = new TeamModel()
            {
                Id = team.Id,
                Name = team.Name,
                Avatar = team.Avatar,
            };
            if (eagerLoading)
                model.Players = team.Player.Select(player => ToPlayerModel(player, false)).ToList();
            return model;
        }
    }
}