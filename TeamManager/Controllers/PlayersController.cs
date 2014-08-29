using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TeamManager.Models;

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
                    if (player.AvatarImage != null && player.AvatarImage.ContentLength > 0)
                    {
                        player.Avatar = String.Format("{0}_{1}", player.Id, Path.GetFileName(player.AvatarImage.FileName));
                        var filepath = Path.Combine(Server.MapPath("~/Content/Avatars/Players"), player.Avatar);
                        player.AvatarImage.SaveAs(filepath);
                    }
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
                    if (player.AvatarImage != null && player.AvatarImage.ContentLength > 0)
                    {
                        var imageToDelete = player.Avatar;
                        player.Avatar = String.Format("{0}_{1}", player.Id, Path.GetFileName(player.AvatarImage.FileName));
                        var filepath = Path.Combine(Server.MapPath("~/Content/Avatars/Players"), player.Avatar);
                        player.AvatarImage.SaveAs(filepath);

                        if (!String.IsNullOrEmpty(imageToDelete))
                        {
                            filepath = Path.Combine(Server.MapPath("~/Content/Avatars/Players"), imageToDelete);
                            if (System.IO.File.Exists(filepath))
                            {
                                System.IO.File.Delete(filepath);
                            }
                        }
                    }
                    _repository.UpdatePlayer(player);
                    return RedirectToAction("Index");
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
                if (!String.IsNullOrEmpty(player.Avatar))
                {
                    var filepath = Path.Combine(Server.MapPath("~/Content/Avatars/Players"), player.Avatar);
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
