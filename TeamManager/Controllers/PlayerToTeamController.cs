using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TeamManager.Models;

namespace TeamManager.Controllers
{
    public class PlayerToTeamController : Controller
    {
        private IPlayerRepository _playersRepository;
        private ITeamRepository _teamsRepository;

        public PlayerToTeamController(IPlayerRepository playersRepository, ITeamRepository teamsRepository)
        {
            _playersRepository = playersRepository;
            _teamsRepository = teamsRepository;
        }

        public PlayerToTeamController() : this(new PlayerRepository(), new TeamRepository())
        {
        }

        public ActionResult SelectTeam(int playerId)
        {
            var player = _playersRepository.GetPlayerById(playerId);
            var teams = _teamsRepository.GetTeams();
            return View(new SelectTeamViewModel() { Player = player, Teams = teams });
        }

        public ActionResult SelectPlayer(int teamId)
        {
            var players = _playersRepository.GetPlayers();
            var team = _teamsRepository.GetTeamById(teamId);
            return View(new SelectPlayerViewModel() { Team = team, Players = players });
        }

        public ActionResult AddToTeam(int playerId, int teamId, bool redirectToPlayer)
        {
            var player = _playersRepository.GetPlayerById(playerId);
            
            player.TeamId = teamId;
            try
            {
                _playersRepository.UpdatePlayer(player);
                if (redirectToPlayer)
                    return RedirectToAction("Details", "Players", new { id = playerId });
                return RedirectToAction("Details", "Teams", new { id = teamId });
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return RedirectToAction("SelectTeam", "PlayerToTeam", new { playerId = playerId });
        }

        public ActionResult Remove(int playerId, int? teamId)
        {
            var player = _playersRepository.GetPlayerById(playerId);

            player.TeamId = null;
            try
            {
                _playersRepository.UpdatePlayer(player);
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            if (teamId.HasValue)
                return RedirectToAction("Details", "Teams", new { id = teamId.Value });
            return RedirectToAction("Details", "Players", new { id = playerId });
        }
    }
}
