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
    public class HomeController : Controller
    {
        private Entities db = new Entities();
        private static List<Publication> FoundPublications = new List<Publication>();

        #region Search
        // GET: Publications
        public ActionResult Index(string title, string orgBranch, string author, string journal, string keywords, int? typeId, int? countryId, int? year, bool? onlyFree)
        {
            var publications = db.Publications.Include(p => p.ArticleType).Include(p => p.Journal);
            Dictionary<int, string> types = db.ArticleTypes.ToDictionary(t => t.ID, t => t.Name);
            Dictionary<int, string> countries = db.Countries.ToDictionary(t => t.ID, t => t.Name);
            string searchInfo = "";

            if (!string.IsNullOrEmpty(title))
            {
                publications = publications.Where(p => p.Title.Contains(title));
                searchInfo += $"Title = {title} & ";
            }
            if (!string.IsNullOrEmpty(journal))
            {
                publications = publications.Where(p => p.Journal.Title.Contains(journal));
                searchInfo += $"Journal = {journal} & ";
            }
            if (!string.IsNullOrEmpty(orgBranch))
            {
                publications = Publication.FindByOrgBranch(publications, orgBranch);
                searchInfo += $"Organization branch title = {orgBranch} & ";
            }
            if (!string.IsNullOrEmpty(author))
            {
                publications = Publication.FindByAuthor(publications, author);
                searchInfo += $"Author = {author} & ";
            }
            if (!string.IsNullOrEmpty(keywords))
            {
                publications = Publication.FindByKeywords(publications, keywords);
                searchInfo += $"Keywords = {keywords} & ";
            }
            if (typeId != null)
            {
                publications = publications.Where(p => p.ArticleType.ID == typeId);
                searchInfo += $"Article type = {types[(int)typeId]} & ";
            }
            if (countryId != null)
            {
                publications = Publication.FindByCountry(publications, (int)countryId);
                searchInfo += $"Country = {countries[(int)countryId]} & ";
            }
            if (year != null)
            {
                publications = publications.Where(p => p.Year == year);
                searchInfo += $"Year = {year} & ";
            }
            if (onlyFree != null)
            {
                publications = publications.Where(p => p.Free == onlyFree);
                searchInfo += $"Only free = {onlyFree}";
            }

            ViewBag.Types = new SelectList(db.ArticleTypes, "ID", "Name");
            ViewBag.Countries = new SelectList(db.Countries.OrderBy(x => x.Name), "ID", "Name");
            ViewBag.SearchInfo = "Search parameters: " + searchInfo.TrimEnd('&', ' ');

            FoundPublications = new List<Publication>(publications.ToList());

            return View(publications.ToList());
        }
        #endregion
        public ActionResult DownloadCSV(string searchInfo)
        {
            string csv = $"#{searchInfo}\n#Number of search results: {FoundPublications.Count}\n\nTitle;Year;Free;Article Type;Journal\n";

            foreach (Publication publication in FoundPublications)
            {
                csv += publication + "\n";
            }

            return File(new System.Text.UTF8Encoding().GetBytes(csv), "text/csv", "Report.csv");
        }

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

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}