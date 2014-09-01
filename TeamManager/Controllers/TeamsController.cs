using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TeamManager.Repositories;
using TeamManager.Models;
using TeamManager.Common;

namespace TeamManager.Controllers
{
    public class TeamsController : Controller
    {
        private ITeamRepository _repository;

        public TeamsController(ITeamRepository repository)
        {
            _repository = repository;
        }
        public TeamsController() : this(new TeamRepository())
        {
        }

        public ActionResult Index()
        {
            var teams = _repository.GetTeams();
            return View(teams);
        }

        public ActionResult Create()
        {
            return View(new TeamModel());
        }

        [HttpPost]
        public ActionResult Create(TeamModel team)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    team.Avatar = ImagesHandler.Upload(team.AvatarImage, getAvatarsPath(), team.Id.ToString());
                    _repository.InsertTeam(team);
                    return RedirectToAction("Index");
                }
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(team);
        }

        public ActionResult Details(int id)
        {
            TeamModel model = _repository.GetTeamById(id);
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            TeamModel model = _repository.GetTeamById(id);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(TeamModel team)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newAvatar = ImagesHandler.Upload(team.AvatarImage, getAvatarsPath(), team.Id.ToString());
                    var avatarToDelete = team.Avatar;
                    if (!String.IsNullOrEmpty(newAvatar))
                        team.Avatar = newAvatar;
                    _repository.UpdateTeam(team);
                    if (!String.IsNullOrEmpty(newAvatar))
                        ImagesHandler.Delete(getAvatarsPath(), avatarToDelete);
                    return RedirectToAction("Index");
                }
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(team);
        }

        public ActionResult Delete(int id, bool? saveChangesError)
        {
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Unable to save changes. Try again, and if the problem persists see your system administrator.";
            }
            TeamModel team = _repository.GetTeamById(id);
            return View(team);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                TeamModel team = _repository.GetTeamById(id);
                _repository.DeleteTeam(id);
                ImagesHandler.Delete(getAvatarsPath(), team.Avatar);
            }
            catch (DataException)
            {
                return RedirectToAction("Delete",
                new System.Web.Routing.RouteValueDictionary { 
                    { "id", id }, 
                    { "saveChangesError", true } });
            }
            return RedirectToAction("Index");
        }

        private string getAvatarsPath()
        {
            return Server.MapPath("~/Content/Avatars/Teams");
        }
    }
}
