using Annual_Report.Models;
using Annual_Report.Models.testmodel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Annual_Report.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        DbEntity db = new DbEntity();
        public ActionResult Index()
        {

            var combination = db.test1.Where(x => x.System4 == "Head & Neck").Select(x => new { x.System3, x.System4 })
                .Concat(db.test2.Where(x => x.System4 == "Head & Neck").Select(x => new { x.System3, x.System4 }))
                .Concat(db.test3.Where(x => x.System4 == "Head & Neck").Select(x => new { x.System3, x.System4 }))
                .GroupBy(x => new { x.System4, x.System3 })
                .Select(g=>new testViewModel { System4=g.Key.System4, System3 = g.Key.System3,count=g.Count() }).ToList();
                
            


            return View(combination);
        }
        public class CancerData
        {
            public string City { get; set; }
            public int BuccalMucosaCases { get; set; }
            public int LungCases { get; set; }
        }
        public ActionResult index1()
        {
            var model = new List<CancerData>() {
            new CancerData { City = "Mumbai", BuccalMucosaCases = 56,LungCases=68 },
            new CancerData { City = "Vizag", BuccalMucosaCases = 86,LungCases=98 },
            new CancerData { City = "Sanrur", BuccalMucosaCases = 96,LungCases=64 },
            new CancerData { City = "Varanasi", BuccalMucosaCases = 45,LungCases=24 }
        };

            return View(model);
        }

       
    }
}