using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TeamManager.Models;

namespace TeamManager.Controllers
{
    public class TeamController : Controller
    {
        private ITeamRepository _repository;

        public TeamController(ITeamRepository repository)
        {
            _repository = repository;
        }
        public TeamController()
            : this(new TeamRepository())
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
