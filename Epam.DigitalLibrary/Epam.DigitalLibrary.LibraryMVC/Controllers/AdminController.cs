using Epam.DigitalLibrary.CustomExeptions;
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

        public ActionResult Index()
        {
            try
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

            catch (DataAccessException e)
            {
                _logger.LogError(4, $"Error on data acces layer |Method: {e.TargetSite.Name} | User: {User.Identity.Name} | Exception Path: {e.StackTrace}");
                return Redirect("/");
            }

            catch (BusinessLogicException e)
            {
                _logger.LogError(4, $"Error on business layer |Method: {e.TargetSite.Name} | User: {User.Identity.Name} | Exception Path: {e.StackTrace}");
                return Redirect("/");
            }

            catch (Exception e)
            {
                _logger.LogError(4, $"Unhandled exception | Method: {e.TargetSite.Name} | User: {User.Identity.Name} | Exception Path: {e.StackTrace}");
                return Redirect("/");
            }
        }
       
        [HttpPost]
        [Route("Admin/GrantRole/{roleId:Guid}/{userId:Guid}")]
        [ValidateAntiForgeryToken]
        public ActionResult GrantRole(Guid roleId, Guid userId)
        {
            try
            {
                bool setResult = _userLogic.SetUserToRole(userId, roleId);

                if (!setResult)
                {
                    TempData["Error"] = "Unable set user role";
                }

                _logger.LogInformation(2, $"Presentation layer | User: {User.Identity.Name} | Role with id {roleId} was granted to user {_userLogic.GetUser(userId).Login}");
                return RedirectToAction(nameof(Index));
            }

            catch (DataAccessException e)
            {
                _logger.LogError(4, $"Error on data acces layer |Method: {e.TargetSite.Name} | User: {User.Identity.Name} | Exception Path: {e.StackTrace}");
                return Redirect("/");
            }

            catch (BusinessLogicException e)
            {
                _logger.LogError(4, $"Error on business layer |Method: {e.TargetSite.Name} | User: {User.Identity.Name} | Exception Path: {e.StackTrace}");
                return Redirect("/");
            }

            catch (Exception e)
            {
                _logger.LogError(4, $"Unhandled exception | Method: {e.TargetSite.Name} | User: {User.Identity.Name} | Exception Path: {e.StackTrace}");
                return Redirect("/");
            }
        }

        [HttpPost]
        [Route("Admin/RemoveRole/{roleId:Guid}/{userId:Guid}")]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveRole(Guid roleId, Guid userId)
        {
            try
            {
                bool removeRoleresult = _userLogic.RemoveRoleFromUser(userId, roleId);

                if (!removeRoleresult)
                {
                    TempData["Error"] = "Unable remove role from user";
                }

                _logger.LogInformation(2, $"Presentation layer | User: {User.Identity.Name} | Role with id {roleId} was removed from user {_userLogic.GetUser(userId).Login}");
                return RedirectToAction(nameof(Index));
            }

            catch (DataAccessException e)
            {
                _logger.LogError(4, $"Error on data acces layer |Method: {e.TargetSite.Name} | User: {User.Identity.Name} | Exception Path: {e.StackTrace}");
                return Redirect("/");
            }

            catch (BusinessLogicException e)
            {
                _logger.LogError(4, $"Error on business layer |Method: {e.TargetSite.Name} | User: {User.Identity.Name} | Exception Path: {e.StackTrace}");
                return Redirect("/");
            }

            catch (Exception e)
            {
                _logger.LogError(4, $"Unhandled exception | Method: {e.TargetSite.Name} | User: {User.Identity.Name} | Exception Path: {e.StackTrace}");
                return Redirect("/");
            }
        }
    }
}
