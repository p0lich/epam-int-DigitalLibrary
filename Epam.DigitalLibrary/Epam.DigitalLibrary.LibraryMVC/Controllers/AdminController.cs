using Epam.DigitalLibrary.Entities;
using Epam.DigitalLibrary.LibraryMVC.Models;
using Epam.DigitalLibrary.LogicContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Epam.DigitalLibrary.LibraryMVC.Controllers
{
    [Authorize(Roles = UserRights.Admin)]
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly INoteLogic _logic;
        private readonly IUserRightsProvider _userLogic;

        public AdminController(ILogger<AdminController> logger, INoteLogic logic, IUserRightsProvider userLogic)
        {
            _logger = logger;
            _logic = logic;
            _userLogic = userLogic;
        }

        // GET: AdminController
        public ActionResult Index()
        {
            IEnumerable<UserLinkView> userLinks = _userLogic.GetUsers()
                .Select(u => new UserLinkView()
                {
                    Id = u.ID,
                    Login = u.Login,
                    Roles = _userLogic.GetRoles(u.ID)
                });

            return View(userLinks);
        }
       
        [HttpPost]
        [Route("Admin/GrantRole/{roleId:Guid}/{userId:Guid}")]
        [ValidateAntiForgeryToken]
        public ActionResult GrantRole(Guid roleId, Guid userId)
        {
            bool setResult = _userLogic.SetUserToRole(userId, roleId);

            if (!setResult)
            {
                TempData["Error"] = "Unable set user role";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Route("Admin/RemoveRole/{roleId:Guid}/{userId:Guid}")]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveRole(Guid roleId, Guid userId)
        {
            bool removeRoleresult = _userLogic.RemoveRoleFromUser(userId, roleId);

            if (!removeRoleresult)
            {
                TempData["Error"] = "Unable remove role from user";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
