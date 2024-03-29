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
    public class AuthorshipsController : Controller
    {
        private Entities db = new Entities();

        #region Search
        // GET: Authorships
        public ActionResult Index(string title)
        {
            IQueryable<Authorship> authorships = db.Authorships;
            string searchInfo = "";

            if (!string.IsNullOrEmpty(title))
            {
                authorships = authorships.Where(x => x.Publication.Title.Contains(title));
                searchInfo += $"Publication title = {title}";
            }

            ViewBag.SearchInfo = "Search parameters: " + searchInfo;
            return View(authorships.ToList());
        }
        #endregion

        // GET: Authorships/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Authorship authorship = db.Authorships.Find(id);
            if (authorship == null)
            {
                return HttpNotFound();
            }
            return View(authorship);
        }

        // GET: Authorships/Create
        public ActionResult Create(int? PMID)
        {
            ViewBag.BranchID = new SelectList(db.OrgBranches.OrderBy(x => x.Title), "ID", "Title");
            ViewBag.PersonID = new SelectList(db.People.OrderBy(x => x.SecondName), "ID", "FullName");
            ViewBag.PMID = new SelectList(db.Publications.OrderBy(x => x.Title), "PMID", "Title", PMID);

            //((SelectList)ViewBag.PMID). = PMID;
            //if (PMID != null) ViewBag.SelectedPubId = PMID;
            
            //Publication publication = db.Publications.Find(PMID);
            //return View(publication);

            return View();
        }

        // POST: Authorships/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PersonID,PMID,BranchID,Position")] Authorship authorship)
        {
            if (ModelState.IsValid)
            {
                db.Authorships.Add(authorship);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BranchID = new SelectList(db.OrgBranches.OrderBy(x => x.Title), "ID", "Title", authorship.BranchID);
            ViewBag.PersonID = new SelectList(db.People.OrderBy(x => x.SecondName), "ID", "FirstName", authorship.PersonID);
            ViewBag.PMID = new SelectList(db.Publications.OrderBy(x => x.Title), "PMID", "ISSN", authorship.PMID);

            return View(authorship);
        }

        // GET: Authorships/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Authorship authorship = db.Authorships.Find(id);
            if (authorship == null)
            {
                return HttpNotFound();
            }
            ViewBag.BranchID = new SelectList(db.OrgBranches.OrderBy(x => x.Title), "ID", "Title", authorship.BranchID);
            ViewBag.PersonID = new SelectList(db.People.OrderBy(x => x.SecondName), "ID", "FirstName", authorship.PersonID);
            ViewBag.PMID = new SelectList(db.Publications.OrderBy(x => x.Title), "PMID", "ISSN", authorship.PMID);
            return View(authorship);
        }

        // POST: Authorships/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PersonID,PMID,BranchID,Position")] Authorship authorship)
        {
            if (ModelState.IsValid)
            {
                db.Entry(authorship).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BranchID = new SelectList(db.OrgBranches.OrderBy(x => x.Title), "ID", "Title", authorship.BranchID);
            ViewBag.PersonID = new SelectList(db.People.OrderBy(x => x.SecondName), "ID", "FirstName", authorship.PersonID);
            ViewBag.PMID = new SelectList(db.Publications.OrderBy(x => x.Title), "PMID", "ISSN", authorship.PMID);
            return View(authorship);
        }

        // GET: Authorships/Delete/5
        public ActionResult Delete(int? personId, int? pmid, int? branchId)
        {
            if (personId == null || pmid == null || branchId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Authorship authorship = db.Authorships.Find(personId, pmid, branchId);
            if (authorship == null)
            {
                return HttpNotFound();
            }
            return View(authorship);
        }

        // POST: Authorships/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int personId, int pmid, int branchId)
        {
            Authorship authorship = db.Authorships.Find(personId, pmid, branchId);
            db.Authorships.Remove(authorship);
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
