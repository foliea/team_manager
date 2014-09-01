using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TeamManager.Repositories;
using TeamManager.Models;

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
                    if (team.AvatarImage != null && team.AvatarImage.ContentLength > 0) {
                        team.Avatar = String.Format("{0}_{1}", team.Id, Path.GetFileName(team.AvatarImage.FileName));
                        var filepath = Path.Combine(Server.MapPath("~/Content/Avatars/Teams"), team.Avatar);
                        team.AvatarImage.SaveAs(filepath);
                    }
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
                    if (team.AvatarImage != null && team.AvatarImage.ContentLength > 0)
                    {
                        var imageToDelete = team.Avatar;
                        team.Avatar = String.Format("{0}_{1}", team.Id, Path.GetFileName(team.AvatarImage.FileName));
                        var filepath = Path.Combine(Server.MapPath("~/Content/Avatars/Teams"), team.Avatar);
                        team.AvatarImage.SaveAs(filepath);

                        if (!String.IsNullOrEmpty(imageToDelete))
                        {
                            filepath = Path.Combine(Server.MapPath("~/Content/Avatars/Teams"), imageToDelete);
                            if (System.IO.File.Exists(filepath))
                            {
                                System.IO.File.Delete(filepath);
                            }
                        }
                    }
                    _repository.UpdateTeam(team);
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
                if (!String.IsNullOrEmpty(team.Avatar))
                {
                    var filepath = Path.Combine(Server.MapPath("~/Content/Avatars/Teams"), team.Avatar);
                    if (System.IO.File.Exists(filepath))
                    {
                        System.IO.File.Delete(filepath);
                    }
                }
                
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
    }
}
