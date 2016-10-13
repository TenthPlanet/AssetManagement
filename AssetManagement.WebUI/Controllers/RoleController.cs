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
    public class RoleController : Controller
    {
        public RoleController()
            : this(new RoleRepository())
        {

        }
        private readonly IRoleRepository _repository;
        public RoleController(IRoleRepository _repository)
        {
            this._repository = _repository;
        }
        // GET: /Role/
        public ActionResult Index()
        {
            return View(_repository.GetRoles());
        }

        // GET: /Role/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Role role = _repository.FindRole(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        // GET: /Role/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RoleID,RoleName,Description")] Role role)
        {
            if (ModelState.IsValid)
            {
                _repository.Insert(role);
                _repository.Save();
                return RedirectToAction("Index");
            }

            return View(role);
        }

        // GET: /Role/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Role role = _repository.FindRole(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RoleID,RoleName,Description")] Role role)
        {
            if (ModelState.IsValid)
            {
                _repository.Update(role);
                _repository.Save();
                return RedirectToAction("Index");
            }
            return View(role);
        }

        // GET: /Role/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Role role = _repository.FindRole(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        // POST: /Role/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Role role = _repository.FindRole(id);
            _repository.Delete(role);
            _repository.Save();
            return RedirectToAction("Index");
        }

    }
}
