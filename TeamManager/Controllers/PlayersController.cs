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
    public class PlayersController : Controller
    {
        private IPlayerRepository _repository;

        public PlayersController(IPlayerRepository repository)
        {
            _repository = repository;
        }

        public PlayersController() : this(new PlayerRepository())
        {
        }

        public ActionResult Index()
        {
            var players = _repository.GetPlayers();
            return View(players);
        }

        public ActionResult Create()
        {
            return View(new PlayerModel());
        }

        [HttpPost]
        public ActionResult Create(PlayerModel player)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    player.Avatar = ImagesHandler.Upload(player.AvatarImage, getAvatarsPath(), player.Id.ToString());
                    _repository.InsertPlayer(player);
                    return RedirectToAction("Index");
                }
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(player);
        }

        public ActionResult Details(int id)
        {
            PlayerModel model = _repository.GetPlayerById(id);
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            PlayerModel model = _repository.GetPlayerById(id);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(PlayerModel player)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newAvatar = ImagesHandler.Upload(player.AvatarImage, getAvatarsPath(), player.Id.ToString());
                    var avatarToDelete = player.Avatar;
                    if (!String.IsNullOrEmpty(newAvatar))                        
                        player.Avatar = newAvatar;
                    _repository.UpdatePlayer(player);
                    if (!String.IsNullOrEmpty(newAvatar))
                        ImagesHandler.Delete(getAvatarsPath(), avatarToDelete);
                    return RedirectToAction("Details", new { id = player.Id });
                }
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(player);
        }

        public ActionResult Delete(int id, bool? saveChangesError)
        {
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Unable to save changes. Try again, and if the problem persists see your system administrator.";
            }
            PlayerModel player = _repository.GetPlayerById(id);
            return View(player);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                PlayerModel player = _repository.GetPlayerById(id);
                _repository.DeletePlayer(id);
                ImagesHandler.Delete(getAvatarsPath(), player.Avatar);
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
            return Server.MapPath("~/Content/Avatars/Players");
        }
    }
}
