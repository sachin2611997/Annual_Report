using Annual_Report.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Annual_Report.Controllers
{
    public class GuwahtriTestController : Controller
    {
        DbEntity db = new DbEntity();
        // GET: GuwahtriTest
        public ActionResult Indextest()
        {
            return View();
        }

        public ActionResult All_sitesTest()
        {
            var System4 = db.guwahti.Select(x => x.System4).Distinct().ToList();
            var System4list = System4.ToList();
            ViewBag.System4 = new SelectList(System4list, System4, "");

            var males = db.guwahti.Where(x => x.SEX == "Male").Average(x => x.AGE);
            ViewBag.male = Math.Round(males, 0);
            var females = db.guwahti.Where(x => x.SEX == "Female").Average(x => x.AGE);
            ViewBag.female = Math.Round(females, 0);
            var total = db.guwahti.Count();
            ViewBag.total = total;
            ViewBag.malecount = db.guwahti.Where(x => x.SEX == "Male").Count();
            ViewBag.femalecount = db.guwahti.Where(x => x.SEX == "Female").Count();
            ViewBag.Totalcancers = db.guwahti.Select(x => x.TGPR).Count();
            var list = db.guwahti.GroupBy(x => x.TGPR).Select(x => new final_analysisViewModel() { NewCase = x.Key, NewCaseCount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.guwahti.Select(y => y.TGPR).Count(), 1) }).ToList();

            ViewBag.totalper = db.guwahti.Select(x => x.TGPR).Count() * 100 / db.guwahti.Select(x => x.TGPR).Count();
            var data = db.guwahti.GroupBy(x => x.N_CLINEXT).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.guwahti.Select(x => x.N_CLINEXT).Count(), 1) }).OrderBy(i => i.Key == "Treated outside").ThenBy(i => i.Key == "Unk. Primary").ThenBy(i => i.Key == "Not Known").ThenBy(i => i.Key == "Not applicable").ThenBy(i => i.Key == "Too Advanced").ThenBy(i => i.Key == "Dist. Mets").ThenBy(i => i.Key == "Regional").ThenBy(i => i.Key == "Localised").ToList();
         
            var colorMap = new Dictionary<string, string>
              {

                 { "Regional", "#FF8000" },
                { "Localised", "#33FF57" },
                { "Not Known", "#8D0B41" },
                { "Treated outside", "#37AFE1" },
                { "Too Advanced", "#FF33A1" },
                { "Dist. Mets", "#FF2929" }

              };
          
            var random = new Random();
            var defaultColors = new[] { "#FF00FF", "#00FFFF", "#FFFF00", "#FF6347", "#B0E0E6" }; // Can add more if necessary
     
            var dataWithColors = data.Select(item => new
            {
                item.Key,
                item.Count,
                color = colorMap.ContainsKey(item.Key) ? colorMap[item.Key] : defaultColors[random.Next(defaultColors.Length)]
            }).ToList();
     
            ViewBag.data = dataWithColors;

            ViewBag.agedata = db.guwahti.GroupBy(x => x.BROAD_AGE).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.guwahti.Select(x => x.BROAD_AGE).Count(), 1) }).OrderBy(i => i.Key == ">=60").ThenBy(i => i.Key == "46-59").ThenBy(i => i.Key == "20-45").ThenBy(i => i.Key == "0-19").ToList();

            var objj = db.guwahti.Where(x => x.final_tti != null).GroupBy(x => x.final_tti).Select(y => new { y.Key, value = y.Count(), Count = Math.Round(y.Count() * 100.0 / total, 1) }).OrderBy(i => i.Key == ">180 days").ThenBy(i => i.Key == "91-180 days").ThenBy(i => i.Key == "31-90 days").ThenBy(i => i.Key == "<30 days").ToList();
            ViewBag.timedata = objj.Where(x => x.Key != null);

            var obj = db.guwahti.Where(x => x.CANCER_DIRECTED_TREATMENT != "No" && x.CANCER_DIRECTED_TREATMENT != "Treatment advised but not accepted").GroupBy(x => x.n_treatment).Select(y => new { y.Key, Count = y.Count() }).ToList();

            ViewBag.objectname = obj;
            ViewBag.Totalobjectname = obj.Sum(x => x.Count);

            ViewBag.newtmc = db.guwahti.Where(x => x.TGPR == "No" && x.CANCER_DIRECTED_TREATMENT == "Yes" && x.CANCER_DIRECTED_TREATMENT == "Incomplete Treatment").Select(y => y.CANCER_DIRECTED_TREATMENT).Count();
            ViewBag.oldtmc = db.guwahti.Where(x => x.TGPR == "Yes" && x.CANCER_DIRECTED_TREATMENT == "Yes" && x.CANCER_DIRECTED_TREATMENT == "Incomplete Treatment").Select(y => y.CANCER_DIRECTED_TREATMENT).Count();
            ViewBag.untmc = db.guwahti.Where(x => x.TGPR == "U" && x.CANCER_DIRECTED_TREATMENT == "Yes" && x.CANCER_DIRECTED_TREATMENT == "Incomplete Treatment").Select(y => y.CANCER_DIRECTED_TREATMENT).Count();

            ViewBag.totaltre = db.guwahti.Where(x => x.CANCER_DIRECTED_TREATMENT == "Yes" && x.CANCER_DIRECTED_TREATMENT == "Incomplete Treatment").Select(y => y.CANCER_DIRECTED_TREATMENT).Count();

            ViewBag.newcase = db.guwahti.Where(x => x.TGPR == "No").Count();
            ViewBag.newcaseper = Math.Round(ViewBag.newcase * 100.0 / total, 1);
            ViewBag.oldcase = db.guwahti.Where(x => x.TGPR == "Yes").Count();
            ViewBag.oldcaseper = Math.Round(ViewBag.oldcase * 100.0 / total, 1);
            ViewBag.unkcase = db.guwahti.Where(x => x.TGPR == "U").Count();
            ViewBag.unkcaseper = Math.Round(ViewBag.unkcase * 100.0 / total, 2);

            ViewBag.NewTretedTmh = db.guwahti.Where(x => x.TGPR == "No" && (x.CANCER_DIRECTED_TREATMENT == "Yes" || x.CANCER_DIRECTED_TREATMENT == "Incomplete Treatment")).Count();
            ViewBag.NewTretedTmhper = Math.Round((ViewBag.NewTretedTmh) * 100.0 / (ViewBag.total), 1);
            ViewBag.oldtreatedtmh = db.guwahti.Where(x => x.TGPR == "Yes" && (x.CANCER_DIRECTED_TREATMENT == "Yes" || x.CANCER_DIRECTED_TREATMENT == "Incomplete Treatment")).Count();
            ViewBag.oldtreatedtmhper = Math.Round((ViewBag.oldtreatedtmh) * 100.0 / (ViewBag.total), 1);
            ViewBag.untreatedtmh = db.guwahti.Where(x => x.TGPR == "U" && (x.CANCER_DIRECTED_TREATMENT == "Yes" || x.CANCER_DIRECTED_TREATMENT == "Incomplete Treatment")).Count();
            ViewBag.untreatedtmhper = Math.Round((ViewBag.untreatedtmh) * 100.0 / (ViewBag.total), 1);
            if (double.IsNaN(ViewBag.untreatedtmhper))
            {
                ViewBag.untreatedtmhper = 0;
            }
            ViewBag.totalcase = (ViewBag.newcase + ViewBag.oldcase + ViewBag.unkcase);
            ViewBag.totalcaseper = (ViewBag.newcaseper + ViewBag.oldcaseper + ViewBag.unkcaseper);
            ViewBag.tottreatmh = (ViewBag.NewTretedTmh + ViewBag.oldtreatedtmh + ViewBag.untreatedtmh);
            ViewBag.tottreatmhper = Math.Round((ViewBag.tottreatmh) * 100.0 / (ViewBag.totalcase), 1);

            ViewBag.nontretnewcases = ((ViewBag.newcase) - (ViewBag.NewTretedTmh));
            ViewBag.nontretnewcasesper = Math.Round((ViewBag.nontretnewcases) * 100.0 / (ViewBag.total), 1);
            ViewBag.nontretoldcases = ((ViewBag.oldcase) - (ViewBag.oldtreatedtmh));
            ViewBag.nontretoldcasesper = Math.Round((ViewBag.nontretoldcases) * 100.0 / (ViewBag.total), 1);

            ViewBag.advicenotakennew = db.guwahti.Where(x => x.CANCER_DIRECTED_TREATMENT == "Treatment advised but not accepted" && x.TGPR == "No").Count();
            ViewBag.advicenotakennewper = Math.Round((ViewBag.advicenotakennew) * 100.0 / (ViewBag.total), 1);
            ViewBag.advicenotakenold = db.guwahti.Where(x => x.CANCER_DIRECTED_TREATMENT == "Treatment advised but not accepted" && x.TGPR == "Yes").Count();
            ViewBag.advicenotakenoldper = Math.Round((ViewBag.advicenotakenold) * 100.0 / (ViewBag.total), 1);

            ViewBag.norxnnew = db.guwahti.Where(x => x.CANCER_DIRECTED_TREATMENT == "No" && x.TGPR == "No").Count();
            ViewBag.norxnnewper = Math.Round((ViewBag.norxnnew) * 100.0 / (ViewBag.total), 1);
            ViewBag.norxnold = db.guwahti.Where(x => x.CANCER_DIRECTED_TREATMENT == "No" && x.TGPR == "Yes").Count();
            ViewBag.norxnoldper = Math.Round((ViewBag.norxnold) * 100.0 / (ViewBag.total), 1);

            ViewBag.discharge = db.guwahti.Where(x => x.CANCER_DIRECTED_TREATMENT != "No" && x.CANCER_DIRECTED_TREATMENT != "Treatment advised but not accepted").GroupBy(x => x.n_status).Select(g => new { Key = g.Key, Count = g.Count() }).ToList()
                            .Select(x => new {    x.Key,  x.Count,  Color =  x.Key == "Regression" ? "#9CD66B" : x.Key == "Progression" ? "#FF4C4C" :
                            x.Key == "NED" ? "#E78AC3" : x.Key == "Lost to Follow-up" ? "#FFF200" : x.Key == "Death in Hospital" ? "#4FC3F7" : "#CCCCCC"
                            }).ToList();         
            return View();
        }


        [HttpGet]
        public ActionResult Head_NeckTest()
        {
            // var data = db.guwahti.Where(x => x.System4 == "Head & Neck").GroupBy(x => x.N_CLINEXT).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.guwahti.Where(x => x.System4 == "Head & Neck").Select(x => x.N_CLINEXT).Count(), 1) }).OrderBy(i => i.Key == "Not Known").ThenBy(i => i.Key == "Treated outside").ThenBy(i => i.Key == "Too Advanced").ThenBy(i => i.Key == "Dist. Mets").ThenBy(i => i.Key == "Regional").ThenBy(i => i.Key == "Localised").ToList();

            var filtered = db.guwahti.Where(x => x.System4 == "Head & Neck");

            var totalCount = filtered.Count();
            ViewBag.total = totalCount;
            var data = filtered
                .GroupBy(x => x.N_CLINEXT)
                .Select(y => new
                {
                    Key = y.Key,
                    Count = Math.Round((double)y.Count() * 100 / totalCount, 1)
                })
                .AsEnumerable()   // switch to memory for custom ordering
                .OrderBy(i =>
                            i.Key == "Localised" ? 0 :
                            i.Key == "Regional" ? 1 :
                            i.Key == "Dist. Mets" ? 2 :
                            i.Key == "Too Advanced" ? 3 :
                            i.Key == "Treated outside" ? 4 :
                            i.Key == "Not Known" ? 5 : 6)
                .ToList();
            // Define specific colors for certain values
            var colorMap = new Dictionary<string, string>
              {
                { "Regional", "#FF8000" },
                { "Localised", "#33FF57" },
                { "Not Known", "#8D0B41" },
                { "Treated outside", "#37AFE1" },
                { "Too Advanced", "#FF33A1" },
                { "Dist. Mets", "#FF2929" }
              };
            // Define a random color for any other key
            var random = new Random();
            var defaultColors = new[] { "#FF00FF", "#00FFFF", "#FFFF00", "#FF6347", "#B0E0E6" }; // Can add more if necessary

            // Add color property to each data point
            var dataWithColors = data.Select(item => new
            {
                item.Key,
                item.Count,
                color = colorMap.ContainsKey(item.Key) ? colorMap[item.Key] : defaultColors[random.Next(defaultColors.Length)]
            }).ToList();

            // Store the data in ViewBag
            ViewBag.data = dataWithColors;

            ViewBag.malecount = db.guwahti.Where(x => x.SEX == "M" && x.System4 == "Head & Neck").Count();
            ViewBag.femalecount = db.guwahti.Where(x => x.SEX == "F" && x.System4 == "Head & Neck").Count();

            var obj = db.guwahti.Where(x => x.CANCER_DIRECTED_TREATMENT != "No" && x.CANCER_DIRECTED_TREATMENT != "Treatment advised but not accepted" && x.System4 == "Head & Neck").GroupBy(x => x.n_treatment).Select(y => new { y.Key, Count = y.Count() }).ToList();
            ViewBag.objectname = obj.Where(x => x.Key != null);
            ViewBag.Totalobjectname = obj.Sum(x => x.Count);

         
            ViewBag.TotalCount = db.guwahti.Where(x => x.System4 == "Head & Neck").Count();


            ViewBag.newcase = db.guwahti.Where(x => x.System4 == "Head & Neck").Where(x => x.TGPR == "No").Count();
            ViewBag.newcaseper = Math.Round((ViewBag.newcase) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.oldcase = db.guwahti.Where(x => x.System4 == "Head & Neck").Where(x => x.TGPR == "Yes").Count();
            ViewBag.oldcaseper = Math.Round((ViewBag.oldcase) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.unkcase = db.guwahti.Where(x => x.System4 == "Head & Neck").Where(x => x.TGPR == "U").Count();

            ViewBag.unkcaseper = Math.Round((ViewBag.unkcase) * 100.0 / (ViewBag.TotalCount), 1);

            ViewBag.NewTretedTmh = db.guwahti.Where(x => x.System4 == "Head & Neck").Where(x => x.TGPR == "No" && (x.CANCER_DIRECTED_TREATMENT == "Yes" || x.CANCER_DIRECTED_TREATMENT == "Incomplete Treatment")).Count();
            ViewBag.NewTretedTmhper = Math.Round((ViewBag.NewTretedTmh) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.oldtreatedtmh = db.guwahti.Where(x => x.System4 == "Head & Neck").Where(x => x.TGPR == "Yes" && (x.CANCER_DIRECTED_TREATMENT == "Yes" || x.CANCER_DIRECTED_TREATMENT == "Incomplete Treatment")).Count();
            ViewBag.oldtreatedtmhper = Math.Round((ViewBag.oldtreatedtmh) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.untreatedtmh = db.guwahti.Where(x => x.System4 == "Head & Neck").Where(x => x.TGPR == "U" && (x.CANCER_DIRECTED_TREATMENT == "Yes" || x.CANCER_DIRECTED_TREATMENT == "Incomplete Treatment")).Count();



            ViewBag.untreatedtmhper = Math.Round((ViewBag.untreatedtmh) * 100.0 / (ViewBag.unkcase), 1);
            if (double.IsNaN(ViewBag.untreatedtmhper))
            {
                ViewBag.untreatedtmhper = 0;
            }
            ViewBag.totalcase = (ViewBag.newcase + ViewBag.oldcase + ViewBag.unkcase);
            ViewBag.totalcaseper = (ViewBag.newcaseper + ViewBag.oldcaseper + ViewBag.unkcaseper);
            ViewBag.tottreatmh = (ViewBag.NewTretedTmh + ViewBag.oldtreatedtmh + ViewBag.untreatedtmh);
            ViewBag.tottreatmhper = Math.Round((ViewBag.tottreatmh) * 100.0 / (ViewBag.totalcase), 1);


            ViewBag.totaltre = db.guwahti.Where(x => x.System4 == "Head & Neck").Where(x => x.CANCER_DIRECTED_TREATMENT == "Yes" && x.CANCER_DIRECTED_TREATMENT == "Incomplete Treatment").Count();
            ViewBag.notunderwent = (ViewBag.TotalCount - ViewBag.totaltre);
            ViewBag.Outside = (ViewBag.TotalCount - ViewBag.newtmc);
            ViewBag.Per = Math.Round((ViewBag.totaltre) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.Per1 = Math.Round((ViewBag.notunderwent) * 100.0 / (ViewBag.TotalCount), 1);

            ViewBag.nontretnewcases = ((ViewBag.newcase) - (ViewBag.NewTretedTmh));
            ViewBag.nontretnewcasesper = Math.Round((ViewBag.nontretnewcases) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.nontretoldcases = ((ViewBag.oldcase) - (ViewBag.oldtreatedtmh));
            ViewBag.nontretoldcasesper = Math.Round((ViewBag.nontretoldcases) * 100.0 / (ViewBag.TotalCount), 1);

            ViewBag.advicenotakennew = db.guwahti.Where(x => x.CANCER_DIRECTED_TREATMENT == "Treatment advised but not accepted" && x.TGPR == "No" && x.System4 == "Head & Neck").Count();
            ViewBag.advicenotakennewper = Math.Round((ViewBag.advicenotakennew) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.advicenotakenold = db.guwahti.Where(x => x.CANCER_DIRECTED_TREATMENT == "Treatment advised but not accepted" && x.TGPR == "Yes" && x.System4 == "Head & Neck").Count();
            ViewBag.advicenotakenoldper = Math.Round((ViewBag.advicenotakenold) * 100.0 / (ViewBag.TotalCount), 1);

            ViewBag.norxnnew = db.guwahti.Where(x => x.CANCER_DIRECTED_TREATMENT == "No" && x.TGPR == "No" && x.System4 == "Head & Neck").Count();
            ViewBag.norxnnewper = Math.Round((ViewBag.norxnnew) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.norxnold = db.guwahti.Where(x => x.CANCER_DIRECTED_TREATMENT == "No" && x.TGPR == "Yes" && x.System4 == "Head & Neck").Count();
            ViewBag.norxnoldper = Math.Round((ViewBag.norxnold) * 100.0 / (ViewBag.TotalCount), 1);

            ViewBag.agedata = db.guwahti.Where(x => x.System4 == "Head & Neck").GroupBy(x => x.BROAD_AGE).Select
                (y => new {
                    y.Key,
                    Count = Math.Round(y.Count() * 100.0 / db.guwahti.Where(x => x.System4 == "Head & Neck").Select
                (x => x.BROAD_AGE).Count(), 1)
                }).OrderBy(i => i.Key == ">=60").ThenBy(i => i.Key == "46-59").ThenBy(i => i.Key == "20-45").ThenBy(i => i.Key == "0-19").ToList();

            var objj = db.guwahti.Where(x => x.System4 == "Head & Neck" && x.final_tti != null).GroupBy(x => x.final_tti).Select
                (y => new {
                    y.Key,
                    total = y.Count(),
                    Count = Math.Round(y.Count() * 100.0 / db.guwahti.
                Where(x => x.System4 == "Head & Neck" && x.final_tti != null).Select(x => x.final_tti).Count(), 1)
                }).OrderBy(i => i.Key == ">180 days").ThenBy(i => i.Key == "91-180 days").ThenBy(i => i.Key == "31-90 days").ThenBy(i => i.Key == "<30 days").ToList();

            ViewBag.timedata = objj.Where(x => x.Key != null);

            ViewBag.discharge = db.guwahti.Where(x => x.System4 == "Head & Neck" && x.CANCER_DIRECTED_TREATMENT != "No" && x.CANCER_DIRECTED_TREATMENT != "Treatment advised but not accepted").GroupBy(x => x.n_status).Select
                (g => new { Key = g.Key, Count = g.Count() }).ToList()
                              .Select(x => new {
                                  x.Key,
                                  x.Count,
                                  Color =
                              //  x.Key == "Palliative Care" ? "#FFF200" :
                              x.Key == "Regression" ? "#9CD66B" :
                              x.Key == "Progression" ? "#FF4C4C" :
                              x.Key == "NED" ? "#E78AC3" :
                              x.Key == "Lost to Follow-up" ? "#2F3E75" :
                              x.Key == "Death in Hospital" ? "#4FC3F7" :
                              "#CCCCCC"
                              }).ToList();


        

            ViewBag.System4 = "Head & Neck";
            return View();
        }


        [HttpGet]
        public ActionResult Buccal_MucosaTest()
        {
            var data = db.guwahti.Where(x => x.System2 == "Buccal Mucosa").GroupBy(x => x.N_CLINEXT).Select
                (y => new {
                    y.Key,
                    Count = Math.Round(y.Count() * 100.0 / db.guwahti.Where(x => x.System2 == "Buccal Mucosa").Select
                (x => x.N_CLINEXT).Count(), 1)
                }).OrderBy(i => i.Key == "Not Known").ThenBy(i => i.Key == "Treated outside").
                ThenBy(i => i.Key == "Too Advanced").ThenBy(i => i.Key == "Dist. Mets").ThenBy(i => i.Key == "Regional").ThenBy(i => i.Key == "Localised").ToList();
            // Define specific colors for certain values
            var colorMap = new Dictionary<string, string>
           {
                { "Regional", "#FF8000" },
                { "Localised", "#33FF57" },
                { "Not Known", "#8D0B41" },
                { "Treated outside", "#37AFE1" },
                { "Too Advanced", "#FF33A1" },
                { "Dist. Mets", "#FF2929" }
           };
            // Define a random color for any other key
            var random = new Random();
            var defaultColors = new[] { "#FF00FF", "#00FFFF", "#FFFF00", "#FF6347", "#B0E0E6" }; // Can add more if necessary

            // Add color property to each data point
            var dataWithColors = data.Select(item => new
            {
                item.Key,
                item.Count,
                color = colorMap.ContainsKey(item.Key) ? colorMap[item.Key] : defaultColors[random.Next(defaultColors.Length)]
            }).ToList();

            // Store the data in ViewBag
            ViewBag.data = dataWithColors;

            ViewBag.malecount = db.guwahti.Where(x => x.SEX == "M" && x.System2 == "Buccal Mucosa").Count();
            ViewBag.femalecount = db.guwahti.Where(x => x.SEX == "F" && x.System2 == "Buccal Mucosa").Count();

            var obj = db.guwahti.Where(x => x.CANCER_DIRECTED_TREATMENT != "No" && x.CANCER_DIRECTED_TREATMENT != "Treatment advised but not accepted" && x.System2 == "Buccal Mucosa").GroupBy(x => x.n_treatment).Select(y => new { y.Key, Count = y.Count() }).ToList();
            ViewBag.objectname = obj.Where(x => x.Key != null);
            ViewBag.Totalobjectname = obj.Sum(x => x.Count);


            ViewBag.TotalCount = db.guwahti.Where(x => x.System2 == "Buccal Mucosa").Count();


            ViewBag.newcase = db.guwahti.Where(x => x.System2 == "Buccal Mucosa").Where(x => x.TGPR == "No").Count();
            ViewBag.newcaseper = Math.Round((ViewBag.newcase) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.oldcase = db.guwahti.Where(x => x.System2 == "Buccal Mucosa").Where(x => x.TGPR == "Yes").Count();
            ViewBag.oldcaseper = Math.Round((ViewBag.oldcase) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.unkcase = db.guwahti.Where(x => x.System2 == "Buccal Mucosa").Where(x => x.TGPR == "U").Count();

            ViewBag.unkcaseper = Math.Round((ViewBag.unkcase) * 100.0 / (ViewBag.TotalCount), 1);

            ViewBag.NewTretedTmh = db.guwahti.Where(x => x.System2 == "Buccal Mucosa").Where(x => x.TGPR == "No" && (x.CANCER_DIRECTED_TREATMENT == "Yes" || x.CANCER_DIRECTED_TREATMENT == "Incomplete Treatment")).Count();
            ViewBag.NewTretedTmhper = Math.Round((ViewBag.NewTretedTmh) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.oldtreatedtmh = db.guwahti.Where(x => x.System2 == "Buccal Mucosa").Where(x => x.TGPR == "Yes" && (x.CANCER_DIRECTED_TREATMENT == "Yes" || x.CANCER_DIRECTED_TREATMENT == "Incomplete Treatment")).Count();
            ViewBag.oldtreatedtmhper = Math.Round((ViewBag.oldtreatedtmh) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.untreatedtmh = db.guwahti.Where(x => x.System2 == "Buccal Mucosa").Where(x => x.TGPR == "U" && (x.CANCER_DIRECTED_TREATMENT == "Yes" || x.CANCER_DIRECTED_TREATMENT == "Incomplete Treatment")).Count();



            ViewBag.untreatedtmhper = Math.Round((ViewBag.untreatedtmh) * 100.0 / (ViewBag.unkcase), 1);
            if (double.IsNaN(ViewBag.untreatedtmhper))
            {
                ViewBag.untreatedtmhper = 0;
            }
            ViewBag.totalcase = (ViewBag.newcase + ViewBag.oldcase + ViewBag.unkcase);
            ViewBag.totalcaseper = (ViewBag.newcaseper + ViewBag.oldcaseper + ViewBag.unkcaseper);
            ViewBag.tottreatmh = (ViewBag.NewTretedTmh + ViewBag.oldtreatedtmh + ViewBag.untreatedtmh);
            ViewBag.tottreatmhper = Math.Round((ViewBag.tottreatmh) * 100.0 / (ViewBag.totalcase), 1);


            ViewBag.totaltre = db.guwahti.Where(x => x.System2 == "Buccal Mucosa").Where(x => x.CANCER_DIRECTED_TREATMENT == "Yes" && x.CANCER_DIRECTED_TREATMENT == "Incomplete Treatment").Count();
            ViewBag.notunderwent = (ViewBag.TotalCount - ViewBag.totaltre);
            ViewBag.Outside = (ViewBag.TotalCount - ViewBag.newtmc);
            ViewBag.Per = Math.Round((ViewBag.totaltre) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.Per1 = Math.Round((ViewBag.notunderwent) * 100.0 / (ViewBag.TotalCount), 1);

            ViewBag.nontretnewcases = ((ViewBag.newcase) - (ViewBag.NewTretedTmh));
            ViewBag.nontretnewcasesper = Math.Round((ViewBag.nontretnewcases) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.nontretoldcases = ((ViewBag.oldcase) - (ViewBag.oldtreatedtmh));
            ViewBag.nontretoldcasesper = Math.Round((ViewBag.nontretoldcases) * 100.0 / (ViewBag.TotalCount), 1);

            ViewBag.advicenotakennew = db.guwahti.Where(x => x.CANCER_DIRECTED_TREATMENT == "Treatment advised but not accepted" && x.TGPR == "No" && x.System2 == "Buccal Mucosa").Count();
            ViewBag.advicenotakennewper = Math.Round((ViewBag.advicenotakennew) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.advicenotakenold = db.guwahti.Where(x => x.CANCER_DIRECTED_TREATMENT == "Treatment advised but not accepted" && x.TGPR == "Yes" && x.System2 == "Buccal Mucosa").Count();
            ViewBag.advicenotakenoldper = Math.Round((ViewBag.advicenotakenold) * 100.0 / (ViewBag.TotalCount), 1);

            ViewBag.norxnnew = db.guwahti.Where(x => x.CANCER_DIRECTED_TREATMENT == "No" && x.TGPR == "No" && x.System2 == "Buccal Mucosa").Count();
            ViewBag.norxnnewper = Math.Round((ViewBag.norxnnew) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.norxnold = db.guwahti.Where(x => x.CANCER_DIRECTED_TREATMENT == "No" && x.TGPR == "Yes" && x.System2 == "Buccal Mucosa").Count();
            ViewBag.norxnoldper = Math.Round((ViewBag.norxnold) * 100.0 / (ViewBag.TotalCount), 1);

            ViewBag.agedata = db.guwahti.Where(x => x.System2 == "Buccal Mucosa").GroupBy(x => x.BROAD_AGE).Select
                (y => new {
                    y.Key,
                    Count = Math.Round(y.Count() * 100.0 / db.guwahti.Where(x => x.System2 == "Buccal Mucosa").Select
                (x => x.BROAD_AGE).Count(), 1)
                }).OrderBy(i => i.Key == ">=60").ThenBy(i => i.Key == "46-59").ThenBy(i => i.Key == "20-45").ThenBy(i => i.Key == "0-19").ToList();

            var objj = db.guwahti.Where(x => x.System2 == "Buccal Mucosa" && x.final_tti != null).GroupBy(x => x.final_tti).Select
                (y => new {
                    y.Key,
                    total = y.Count(),
                    Count = Math.Round(y.Count() * 100.0 / db.guwahti.
                Where(x => x.System2 == "Buccal Mucosa" && x.final_tti != null).Select(x => x.final_tti).Count(), 1)
                }).OrderBy(i => i.Key == ">180 days").ThenBy(i => i.Key == "91-180 days").ThenBy(i => i.Key == "31-90 days").ThenBy(i => i.Key == "<30 days").ToList();

            ViewBag.timedata = objj.Where(x => x.Key != null);

            ViewBag.discharge = db.guwahti.Where(x => x.System2 == "Buccal Mucosa" && x.CANCER_DIRECTED_TREATMENT != "No" && x.CANCER_DIRECTED_TREATMENT != "Treatment advised but not accepted").GroupBy(x => x.n_status).Select
                (g => new { Key = g.Key, Count = g.Count() }).ToList()
                              .Select(x => new {
                                  x.Key,
                                  x.Count,
                                  Color =
                              //  x.Key == "Palliative Care" ? "#FFF200" :
                              x.Key == "Regression" ? "#9CD66B" :
                              x.Key == "Progression" ? "#FF4C4C" :
                              x.Key == "NED" ? "#E78AC3" :
                              x.Key == "Lost to Follow-up" ? "#2F3E75" :
                              x.Key == "Death in Hospital" ? "#4FC3F7" :
                              "#CCCCCC"
                              }).ToList();

            ViewBag.System2 = "Buccal Mucosa";
            return View();

        }
    }
}