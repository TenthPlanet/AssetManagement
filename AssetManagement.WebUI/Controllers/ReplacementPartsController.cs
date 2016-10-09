using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AssetManagement.Domain.Context;
using AssetManagement.Domain.Entities;

namespace AssetManagement.WebUI.Controllers
{
    public class ReplacementPartsController : Controller
    {
        private AssetManagementEntities db = new AssetManagementEntities();

        // GET: ReplacementParts
        public ActionResult Index()
        {
            return View(db.ReplacementParts.ToList());
        }

        // GET: ReplacementParts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReplacementPart replacementPart = db.ReplacementParts.Find(id);
            if (replacementPart == null)
            {
                return HttpNotFound();
            }
            return View(replacementPart);
        }

        // GET: ReplacementParts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ReplacementParts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "replacementPartID,name,associatedAsset,associatedTicket,invoiceNumber,supplier,price,partNumber,pruchaseDate")] ReplacementPart replacementPart)
        {
            if (ModelState.IsValid)
            {
                db.ReplacementParts.Add(replacementPart);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(replacementPart);
        }

        // GET: ReplacementParts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReplacementPart replacementPart = db.ReplacementParts.Find(id);
            if (replacementPart == null)
            {
                return HttpNotFound();
            }
            return View(replacementPart);
        }

        // POST: ReplacementParts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "replacementPartID,name,associatedAsset,associatedTicket,invoiceNumber,supplier,price,partNumber,pruchaseDate")] ReplacementPart replacementPart)
        {
            if (ModelState.IsValid)
            {
                db.Entry(replacementPart).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(replacementPart);
        }

        // GET: ReplacementParts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReplacementPart replacementPart = db.ReplacementParts.Find(id);
            if (replacementPart == null)
            {
                return HttpNotFound();
            }
            return View(replacementPart);
        }

        // POST: ReplacementParts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ReplacementPart replacementPart = db.ReplacementParts.Find(id);
            db.ReplacementParts.Remove(replacementPart);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
