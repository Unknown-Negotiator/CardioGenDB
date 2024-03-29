using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebPubApp;
using WebPubApp.Common;

namespace WebPubApp.Controllers
{
    [BasicAuthenticationAttribute("admin", "pass", BasicRealm = "your-realm")]
    public class PublicationsController : Controller
    {
        private Entities db = new Entities();

        #region Search
        // GET: Publications
        public ActionResult Index(string title, string orgBranch, string author, string journal, string keywords, int? typeId, int? countryId, int? year, bool? onlyFree)
        {
            var publications = db.Publications.Include(p => p.ArticleType).Include(p => p.Journal);
            Dictionary<int, string> types = db.ArticleTypes.ToDictionary(t => t.ID, t => t.Name);
            Dictionary<int, string> countries = db.Countries.ToDictionary(t => t.ID, t => t.Name);
            string searchInfo = "Search parameters: ";

            if (!string.IsNullOrEmpty(title))
            {
                publications = publications.Where(p => p.Title.Contains(title));
                searchInfo += $"Title = {title}; ";
            }
            if (!string.IsNullOrEmpty(journal))
            {
                publications = publications.Where(p => p.Journal.Title.Contains(journal));
                searchInfo += $"Journal = {journal}; ";
            }
            if (!string.IsNullOrEmpty(orgBranch))
            {
                publications = Publication.FindByOrgBranch(publications, orgBranch);
                searchInfo += $"Organization branch title = {orgBranch}; ";
            }
            if (!string.IsNullOrEmpty(author))
            {
                publications = Publication.FindByAuthor(publications, author);
                searchInfo += $"Author = {author}; ";
            }
            if (!string.IsNullOrEmpty(keywords))
            {
                publications = Publication.FindByKeywords(publications, keywords);
                searchInfo += $"Keywords = {keywords}; ";
            }
            if (typeId != null)
            {
                publications = publications.Where(p => p.ArticleType.ID == typeId);
                searchInfo += $"Article type = {types[(int)typeId]}; ";
            }
            if (countryId != null)
            {
                publications = Publication.FindByCountry(publications, (int)countryId);
                searchInfo += $"Country = {countries[(int)countryId]}; ";
            }
            if (year != null)
            {
                publications = publications.Where(p => p.Year == year);
                searchInfo += $"Year = {year}; ";
            }
            if (onlyFree != null)
            {
                publications = publications.Where(p => p.Free == onlyFree);
                searchInfo += $"Only free = {onlyFree};";
            }

            ViewBag.Types = new SelectList(db.ArticleTypes, "ID", "Name");
            ViewBag.Countries = new SelectList(db.Countries.OrderBy(x => x.Name), "ID", "Name");
            ViewBag.SearchInfo = searchInfo;

            return View(publications.ToList());
        }
        #endregion

        #region Keywords manipulation
        // GET: Publications/DisplayKeywords/5
        public ActionResult DisplayKeywords(int? pmid)
        {
            if (pmid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Publication publication = db.Publications.Find(pmid);
            if (publication == null)
            {
                return HttpNotFound();
            }
            return View(publication);
        }

        // GET: Publications/DeleteKeyword/5
        public ActionResult DeleteKeyword(int? PMID, int? wordId)
        {
            if (PMID == null || wordId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Publication publication = db.Publications.Find(PMID);
            Keyword word = publication.Keywords.FirstOrDefault(w => w.ID == wordId);

            if (publication == null || word == null)
            {
                return HttpNotFound();
            }
            ViewBag.Word = word.Word;
            return View(publication);
        }

        // POST: Publications/DeleteKeyword/5
        [HttpPost, ActionName("DeleteKeyword")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteKeywordConfirmed(int PMID, int wordId)
        {
            Publication publication = db.Publications.Find(PMID);
            Keyword word = publication.Keywords.FirstOrDefault(w => w.ID == wordId);
            publication.Keywords.Remove(word);
            db.SaveChanges();
            return RedirectToAction($"Edit/{PMID}");
        }

        // GET: Publications/AddKeyword
        public ActionResult AddKeyword(int? PMID)
        {
            if (PMID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Publication publication = db.Publications.Find(PMID);            
            if (publication == null)
            {
                return HttpNotFound();
            }            
            ViewBag.Keywords = new SelectList(db.Keywords.OrderBy(x => x.Word), "ID", "Word");
            return View(publication);
        }

        // POST: Publications/AddKeyword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddKeyword(int PMID, int? wordId)
        {
            if (wordId == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Publication publication = db.Publications.Find(PMID);
            if (ModelState.IsValid)
            {                
                Keyword keyword = db.Keywords.FirstOrDefault(w => w.ID == wordId);
                publication.Keywords.Add(keyword);
                db.Entry(publication).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction($"Edit/{PMID}");
            }
            ViewBag.Keywords = new SelectList(db.Keywords, "ID", "Word"); // why?
            return View(publication);
        }
        #endregion

        #region Details
        // GET: Publications/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Publication publication = db.Publications.Find(id);
            if (publication == null)
            {
                return HttpNotFound();
            }
            return View(publication);
        }
        #endregion

        #region Create
        // GET: Publications/Create
        public ActionResult Create()
        {
            ViewBag.TypeID = new SelectList(db.ArticleTypes.OrderBy(x => x.Name), "ID", "Name");
            ViewBag.ISSN = new SelectList(db.Journals.OrderBy(x => x.Title), "ISSN", "Title");
            return View();
        }

        // POST: Publications/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PMID,ISSN,TypeID,Title,Volume,Issue,Pages,Date,Year,Link,Free,Abstract")] Publication publication)
        {
            if (ModelState.IsValid)
            {
                db.Publications.Add(publication);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TypeID = new SelectList(db.ArticleTypes, "ID", "Name", publication.TypeID);
            ViewBag.ISSN = new SelectList(db.Journals.OrderBy(x => x.Title), "ISSN", "Title", publication.ISSN);
            return View(publication);
        }
        #endregion

        #region Edit
        // GET: Publications/Edit/5        
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Publication publication = db.Publications.Find(id);
            if (publication == null)
            {
                return HttpNotFound();
            }
            ViewBag.TypeID = new SelectList(db.ArticleTypes, "ID", "Name", publication.TypeID);
            ViewBag.ISSN = new SelectList(db.Journals.OrderBy(x => x.Title), "ISSN", "Title", publication.ISSN);
            return View(publication);
        }

        // POST: Publications/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PMID,ISSN,TypeID,Title,Volume,Issue,Pages,Date,Year,Link,Free,Abstract")] Publication publication)
        {
            if (ModelState.IsValid)
            {
                db.Entry(publication).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TypeID = new SelectList(db.ArticleTypes, "ID", "Name", publication.TypeID);
            ViewBag.ISSN = new SelectList(db.Journals.OrderBy(x => x.Title), "ISSN", "Title", publication.ISSN);
            return View(publication);
        }
        #endregion

        #region Delete
        // GET: Publications/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Publication publication = db.Publications.Find(id);
            if (publication == null)
            {
                return HttpNotFound();
            }
            return View(publication);
        }

        // POST: Publications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Publication publication = db.Publications.Find(id);
            db.Publications.Remove(publication);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        #endregion

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
