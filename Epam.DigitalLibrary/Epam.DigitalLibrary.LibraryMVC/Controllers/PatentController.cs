using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Epam.DigitalLibrary.LibraryMVC.Controllers
{
    public class PatentController : Controller
    {
        // GET: PatentController
        public ActionResult Index()
        {
            return View();
        }

        // GET: PatentController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PatentController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PatentController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PatentController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PatentController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PatentController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PatentController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
