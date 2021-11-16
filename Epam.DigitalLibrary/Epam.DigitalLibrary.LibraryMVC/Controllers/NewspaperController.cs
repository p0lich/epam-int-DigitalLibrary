using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Epam.DigitalLibrary.LibraryMVC.Controllers
{
    public class NewspaperController : Controller
    {
        // GET: NewspaperController
        public ActionResult Index()
        {
            return View();
        }

        // GET: NewspaperController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: NewspaperController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: NewspaperController/Create
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

        // GET: NewspaperController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: NewspaperController/Edit/5
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

        // GET: NewspaperController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: NewspaperController/Delete/5
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
