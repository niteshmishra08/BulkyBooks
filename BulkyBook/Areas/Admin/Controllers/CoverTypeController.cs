using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BulkyBook.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CoverTypeController : Controller
    {
        private readonly IunitOfWork _unitofwork;

        public CoverTypeController(IunitOfWork unitofwork)
        {
            _unitofwork = unitofwork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            CoverType coverType = new CoverType();
            if (id == null)
            {
                //create
                return View(coverType);
            }
            else
            {
                //update
                coverType = _unitofwork.CoverType.Get(id.GetValueOrDefault());
                if (coverType == null)
                {
                    return NotFound();
                }
                return View(coverType);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(CoverType coverType)
        {

            if (ModelState.IsValid)
            {
                if (coverType.Id == 0)
                {
                    _unitofwork.CoverType.Add(coverType);
                }
                else
                {
                    _unitofwork.CoverType.Update(coverType);
                }

                _unitofwork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(coverType);
        }


        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var allCoverType = _unitofwork.CoverType.GetAll();
            return Json(new { data = allCoverType });
        }

        [HttpDelete]
        public IActionResult Delete(int Id)
        {
            var obj = _unitofwork.CoverType.Get(Id);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
                
            }
            else
                _unitofwork.CoverType.Remove(obj);
            _unitofwork.Save();
            return Json(new { success = true, message = "Deleted Successfully" });

        }

        #endregion
    }
}
