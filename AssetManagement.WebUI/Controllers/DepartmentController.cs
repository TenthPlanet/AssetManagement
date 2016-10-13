using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AssetManagement.Domain.Entities;
using AssetManagement.Domain.Context;
using AssetManagement.Domain.Concrete;
using AssetManagement.Domain.Abstract;

namespace AssetManagement.WebUI.Controllers
{
    public class DepartmentController : Controller
    {
        public DepartmentController() : this(new DepartmentRepository()) { }

        private readonly IDepartmentRepository _repository;

        public DepartmentController(IDepartmentRepository _repository)
        {
            this._repository = _repository;
        }

        // GET: /Department/
        public ActionResult Index()
        {
            return View(_repository.GetAll());
        }


        // GET: /Department/Create
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "departmentID,departmentName")] Department department)
        {
            if (ModelState.IsValid)
            {
                _repository.Insert(department);
                return RedirectToAction("Index");
            }

            return View(department);
        }

        // GET: /Department/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = _repository.FindDepartment(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "departmentID,departmentName")] Department department)
        {
            if (ModelState.IsValid)
            {
                _repository.Update(department);
                //db.Entry(department).State = EntityState.Modified;
                //db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(department);
        }

        // GET: /Department/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = _repository.FindDepartment(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }

        // POST: /Department/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Department department = _repository.FindDepartment(id);
            _repository.Delete(department);
            return RedirectToAction("Index");
        }
    }
}
