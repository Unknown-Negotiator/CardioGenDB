using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebPubApp;

namespace WebPubApp.Controllers
{
    public class OrgBranchesController : Controller
    {
        private Entities db = new Entities();

        #region Search
        // GET: OrgBranches
        public ActionResult Index(string title)
        {
            IQueryable<OrgBranch> orgBranches = db.OrgBranches.Include(o => o.Country).Include(o => o.Organization);
            string searchInfo = "";

            if (!string.IsNullOrEmpty(title))
            {
                orgBranches = orgBranches.Where(x => x.Title.Contains(title));
                searchInfo += $"Title = {title}";
            }

            ViewBag.SearchInfo = "Search parameters: " + searchInfo;
            return View(orgBranches.ToList());
        }
        #endregion

        // GET: OrgBranches/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrgBranch orgBranch = db.OrgBranches.Find(id);
            if (orgBranch == null)
            {
                return HttpNotFound();
            }
            return View(orgBranch);
        }

        // GET: OrgBranches/Create
        public ActionResult Create()
        {
            ViewBag.CountryID = new SelectList(db.Countries, "ID", "Name");
            ViewBag.OrgID = new SelectList(db.Organizations, "ID", "Title");
            return View();
        }

        // POST: OrgBranches/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Title,OrgID,CountryID")] OrgBranch orgBranch)
        {
            if (ModelState.IsValid)
            {
                db.OrgBranches.Add(orgBranch);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CountryID = new SelectList(db.Countries, "ID", "Name", orgBranch.CountryID);
            ViewBag.OrgID = new SelectList(db.Organizations, "ID", "Title", orgBranch.OrgID);
            return View(orgBranch);
        }

        // GET: OrgBranches/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrgBranch orgBranch = db.OrgBranches.Find(id);
            if (orgBranch == null)
            {
                return HttpNotFound();
            }
            ViewBag.CountryID = new SelectList(db.Countries, "ID", "Name", orgBranch.CountryID);
            ViewBag.OrgID = new SelectList(db.Organizations, "ID", "Title", orgBranch.OrgID);
            return View(orgBranch);
        }

        // POST: OrgBranches/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Title,OrgID,CountryID")] OrgBranch orgBranch)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orgBranch).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CountryID = new SelectList(db.Countries, "ID", "Name", orgBranch.CountryID);
            ViewBag.OrgID = new SelectList(db.Organizations, "ID", "Title", orgBranch.OrgID);
            return View(orgBranch);
        }

        // GET: OrgBranches/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrgBranch orgBranch = db.OrgBranches.Find(id);
            if (orgBranch == null)
            {
                return HttpNotFound();
            }
            return View(orgBranch);
        }

        // POST: OrgBranches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OrgBranch orgBranch = db.OrgBranches.Find(id);
            db.OrgBranches.Remove(orgBranch);
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
