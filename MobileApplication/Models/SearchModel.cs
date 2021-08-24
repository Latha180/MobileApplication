using Sitecore.ContentSearch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobileApplication.Models
{
    public class SearchModel
    {
        [IndexField("_name")]
        public virtual string Title { get; set; }

        [IndexField("title_t")]
        public virtual string MobileBrand { get; set; }

        [IndexField("description_t")]
        public virtual string Description { get; set; } // Custom field on my template

        [IndexField("mobilesmallimage_t")]
        public virtual string MobileSmallImage { get; set; } // Custom field on my template

        [IndexField("mobilerelisedate_tdt")]
        public virtual DateTime MobileReleaseDate { get; set; } // Custom field on my template

        [IndexField("category_s")]
        public virtual string Category { get; set; } // Custom field on my template
    }

    public class SearchResult
    {
        public string Title { get; set; }
        public string MobileBrand { get; set; }
        public string MobileSmallImage { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public DateTime MobileReleaseDate { get; set; }
    }

    /// <summary>
    /// Custom search result model for binding to front end
    /// </summary>
    public class SearchResults
    {
        public List<SearchResult> Results { get; set; }
    }
}