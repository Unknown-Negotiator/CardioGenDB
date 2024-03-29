using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebPubApp.Common
{
    public class SearchInfo
    {
        public string Title { get; set; } = "";
        public string Journal { get; set; } = "";
        public string Author { get; set; } = "";
        public string OrgBranch { get; set; } = "";
        public string Keywords { get; set; } = "";
        public int? TypeId { get; set; }
        public int? CountryId { get; set; }
        public int? Year { get; set; }
        public bool? OnlyFree { get; set; }

        public SearchInfo(string title, string orgBranch, string author, string journal, string keywords, int? typeId, int? countryId, int? year, bool? onlyFree)
        {
            Title = title;
            OrgBranch = orgBranch;            
            Author = author;
            Journal = journal;
            Keywords = keywords;
            TypeId = typeId;
            CountryId = countryId;
            Year = year;
            OnlyFree = onlyFree;
        }
    }
}