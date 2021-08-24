using MobileApplication.Models;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Linq;
using Sitecore.ContentSearch.Linq.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace MobileApplication.Controllers
{
    public class MobilesController : Controller
    {
        /// <summary>
        /// For Tags
        /// </summary>
        /// <returns>Returns list of tags</returns>
        public ActionResult Tags()
        {
            var item = Sitecore.Context.Database.GetItem("{C0CEB0EB-6E73-477A-A77F-DF7C161B284C}");
            // Logic
            Sitecore.Data.Fields.MultilistField tags = item.Fields["Tag"];
            if (tags != null)
            {
                foreach (Sitecore.Data.Items.Item tag in tags.GetItems())
                {
                    string name = tag.Name;
                }
            }
            // Debugging - Being The Detective In a Crime Movie Where You Are Also The Murderer.
            return View("~/Views/Mobiles/Tags.cshtml", item);
        }
        /// <summary>
        /// For Preffered PostCards
        /// </summary>
        /// <returns>It returns list of PostCards</returns>
        public ActionResult DemoComponent()
        {
            var item = Sitecore.Context.Database.GetItem("{7F31B620-066C-4498-A7F9-99315C4F1DC6}");
            string str = item.Fields["CopyRight"].Value;
            // Logic
            Sitecore.Data.Fields.MultilistField prefferedmobiles = item.Fields["PrefferedMobiles"];
            if (prefferedmobiles != null)
            {
                foreach (Sitecore.Data.Items.Item mobiles in prefferedmobiles.GetItems())
                {
                    string name = mobiles.Name;
                    string title = mobiles["Title"];
                    string date = mobiles["MobileReliseDate"];
                    string longDescription = mobiles["LangDescription"];
                }
            }
            // Debugging - Being The Detective In a Crime Movie Where You Are Also The Murderer.
            return View("~/Views/Mobiles/ControllerDemo1.cshtml", item);
        }

        /// <summary>
        /// For Search
        /// </summary>
        /// <param name="searchText"></param>
        /// <returns>returns Search list</returns>
        public ActionResult DoSearch(string searchText)
        {
            var myResults = new SearchResults
            {
                Results = new List<SearchResult>()
            };
            var searchIndex = ContentSearchManager.GetIndex("sitecore_web_index"); // Get the search index
            var searchPredicate = GetSearchPredicate(searchText); // Build the search predicate
            using (var searchContext = searchIndex.CreateSearchContext()) // Get a context of the search index
            {
                //Select * from Sitecore_web_index Where Author="searchText" OR Description="searchText" OR Title="searchText"
             
                //var searchResults = searchContext.GetQueryable<SearchModel>().Where(searchPredicate); // Search the index for items which match the predicate
                var searchResults = searchContext.GetQueryable<SearchModel>()
                    .Where(x => x.Title.Contains(searchText) || x.MobileBrand.Contains(searchText) || x.Description.Contains(searchText)||x.MobileSmallImage.Contains(searchText));   //LINQ query

                var fullResults = searchResults.GetResults();

               
                // This is better and will get paged results - page 1 with 10 results per page
                //var pagedResults = searchResults.Page(1, 10).GetResults();
                foreach (var hit in fullResults.Hits)
                {
                    var item = Sitecore.Context.Database.GetItem("{C0CEB0EB-6E73-477A-A77F-DF7C161B284C}");
                    string MobileSmallImage = item.Fields["MobileSmallImage"].Value;
                    myResults.Results.Add(new SearchResult
                    {
                        Category = hit.Document.Category,
                        Title = hit.Document.Title,
                        Description = hit.Document.Description,
                        MobileBrand = hit.Document.MobileBrand,
                        MobileSmallImage = MobileSmallImage,
                        MobileReleaseDate = hit.Document.MobileReleaseDate
                    }) ; 
                }
                return new JsonResult { JsonRequestBehavior = JsonRequestBehavior.AllowGet, Data = myResults };
            }
        }

        /// <summary>
        /// Search logic
        /// </summary>
        /// <param name="searchText">Search term</param>
        /// <returns>Search predicate object</returns>
        public static Expression<Func<SearchModel, bool>> GetSearchPredicate(string searchText)
        {
            var predicate = PredicateBuilder.True<SearchModel>(); // Items which meet the predicate
                                                                  // Search the whole phrase - LIKE
                                                                  // predicate = predicate.Or(x => x.DispalyName.Like(searchText)).Boost(1.2f);
                                                                  // predicate = predicate.Or(x => x.Description.Like(searchText)).Boost(1.2f);
                                                                  // predicate = predicate.Or(x => x.Title.Like(searchText)).Boost(1.2f);
                                                                  // Search the whole phrase - CONTAINS
            predicate = predicate.Or(x => x.MobileBrand.Contains(searchText)); // .Boost(2.0f);
            predicate = predicate.Or(x => x.Description.Contains(searchText)); // .Boost(2.0f);
            predicate = predicate.Or(x => x.Title.Contains(searchText)); // .Boost(2.0f);
            predicate = predicate.Or(x => x.Category.Contains(searchText)); // .Boost(2.0f);
            //Where Author="searchText" OR Description="searchText" OR Title="searchText"
            return predicate;
        }
    }
}