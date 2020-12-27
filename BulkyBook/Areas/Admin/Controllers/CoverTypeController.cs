using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Utility;
using Dapper;
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
                var param = new DynamicParameters();
                param.Add("@Id", id);
                coverType = _unitofwork.SP_Call.OneRecord<CoverType>(SD.Proc_CoverType_Get, param);
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
                var param = new DynamicParameters();
                param.Add("@Name", coverType.Name);
                if (coverType.Id == 0)
                {
                    _unitofwork.SP_Call.Execute(SD.Proc_CoverType_Create, param);
                }
                else
                {
                    param.Add("@Id", coverType.Id);
                    _unitofwork.SP_Call.Execute(SD.Proc_CoverType_Update, param);
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
            var allCoverType = _unitofwork.SP_Call.List<CoverType>(SD.Proc_CoverType_GetAll, null);
            return Json(new { data = allCoverType });
        }

        [HttpDelete]
        public IActionResult Delete(int Id)
        {
            var param = new DynamicParameters();
            param.Add("@Id", Id);
            var obj = _unitofwork.SP_Call.OneRecord<CoverType>(SD.Proc_CoverType_Get, param);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitofwork.SP_Call.Execute(SD.Proc_CoverType_Delete, param);
            _unitofwork.Save();

            return Json(new { success = true, message = "Deleted Successfully" });

        }

        #endregion
    }
}
