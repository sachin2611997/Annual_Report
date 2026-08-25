using Annual_Report.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Annual_Report.Controllers
{
    public class HBCR2025Controller : Controller
    {
        // GET: HBCR2025
        DbEntity db = new DbEntity();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Head_Neck_Table()
        {
            var priorityOrder = new List<string> { "Oral Cavity", "Oropharynx", "Larynx", "Hypopharynx", "Para Nasal Sinus", "Salivary Gland (Major)", "Nasopharynx" };

            // Calculate total counts
            var totalMaleCount = db.fnl_alysis.Count(x => x.System4 == "Head & Neck" && x.SEX == "M");
            var totalFemaleCount = db.fnl_alysis.Count(x => x.System4 == "Head & Neck" && x.SEX == "F");
            var totalCount = totalMaleCount + totalFemaleCount;

            ViewBag.tMcount = totalMaleCount;
            ViewBag.tFcount = totalFemaleCount;
            ViewBag.TotalGUSCount = totalCount;

            // Prepare System3 and System2 wise data
            var system3Values = db.fnl_alysis
                .Where(x => x.System4 == "Head & Neck")
                .GroupBy(x => new { x.System3, x.System2 })
                .Select(g => new System3StatsViewModel
                {
                    System3 = g.Key.System3,
                    System2 = g.Key.System2,
                    MaleCount = g.Count(x => x.SEX == "M"),
                    FemaleCount = g.Count(x => x.SEX == "F"),
                    TotalCount = totalMaleCount,   // Total Males for Head & Neck
                    TotalFCount = totalFemaleCount, // Total Females for Head & Neck
                    TotalNosCount = g.Count(), // Total for this group
                }).ToList()
                 .OrderBy(x => priorityOrder.IndexOf(x.System3)) // Custom priority order // Optional: Order by System3 for clean display
                .ToList();

            ViewBag.System3Values = system3Values;

            // If you want extra GroupedCounts for male in specific System3 sites
            var groupedCounts = db.fnl_alysis
                .Where(x => x.SEX == "M" &&
                           (x.System3 == "Oral Cavity" ||
                            x.System3 == "Oropharynx" ||
                            x.System3 == "Hypopharynx" ||
                            x.System3 == "Larynx" ||
                            x.System3 == "Nasopharynx"))
                .GroupBy(x => new { x.System3, x.System2 })
                .Select(g => new
                {
                    System3 = g.Key.System3,
                    System2 = g.Key.System2,
                    Count = g.Count()
                })
                .GroupBy(x => x.System3)
                .Select(g => new
                {
                    System3 = g.Key,
                    Counts = g.Select(x => new { x.System2, x.Count }),
                    Total = g.Sum(x => x.Count)
                })
                .ToList();

            ViewBag.GroupedCounts = groupedCounts;

            return View();


        }  // table

        public ActionResult DistributionCanceCasesSiteICD10()
        {
            var report = db.fnl_alysis.GroupBy(x => new { x.Sys, x.System1 })
            .Select(g => new System3StatsViewModel
            {
                ICD10 = g.Key.Sys,
                System = g.Key.System1,

                MaleCount = g.Count(x => x.SEX == "M"),

                FemaleCount = g.Count(x => x.SEX == "F"),

                TotalCount = g.Count()
            }).OrderBy(x => x.ICD10).ToList();

            ViewBag.TotalMale = report.Sum(x => x.MaleCount);
            ViewBag.TotalFemale = report.Sum(x => x.FemaleCount);
            ViewBag.GrandTotal = report.Sum(x => x.TotalCount);
            return View(report);
        }






        public ActionResult amchart4()
        {
            return View();
        }



        [HttpGet]
        public ActionResult Malignant_Lymphoma()
        {


            var data = db.fnl_alysis.Where(x => new[] { "Myeloid  Leukemia", "Lymphoid  Leukemia", "Leukemia Uns" }.Contains(x.System3)).GroupBy(x => x.N_CLINEXT).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System3 == "Testis").Select(x => x.N_CLINEXT).Count(), 1) }).OrderBy(i => i.Key == "Not Known").ThenBy(i => i.Key == "Treated outside").ThenBy(i => i.Key == "Too Advanced").ThenBy(i => i.Key == "Dist. Mets").ThenBy(i => i.Key == "Regional").ThenBy(i => i.Key == "Localised").ToList();

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
            ViewBag.malecount = db.fnl_alysis.Where(x => x.SEX == "M" && new[] { "Myeloid  Leukemia", "Lymphoid  Leukemia", "Leukemia Uns" }.Contains(x.System3)).Count();
            ViewBag.femalecount = db.fnl_alysis.Where(x => x.SEX == "F" && new[] { "Myeloid  Leukemia", "Lymphoid  Leukemia", "Leukemia Uns" }.Contains(x.System3)).Count();

            var obj = db.fnl_alysis.Where(x => new[] { "Myeloid  Leukemia", "Lymphoid  Leukemia", "Leukemia Uns" }.Contains(x.System3)).GroupBy(x => x.TMH_RX).Select(y => new { y.Key, Count = y.Count() }).ToList();

            ViewBag.objectname = obj.Where(x => x.Key != null);
            var list = db.fnl_alysis.Where(x => new[] { "Myeloid  Leukemia", "Lymphoid  Leukemia", "Leukemia Uns" }.Contains(x.System3)).GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => new[] { "Myeloid  Leukemia", "Lymphoid  Leukemia", "Leukemia Uns" }.Contains(y.System3)).Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Take(5);
            var list1 = db.fnl_alysis.Where(x => new[] { "Myeloid  Leukemia", "Lymphoid  Leukemia", "Leukemia Uns" }.Contains(x.System3)).GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => new[] { "Myeloid  Leukemia", "Lymphoid  Leukemia", "Leukemia Uns" }.Contains(y.System3)).Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Skip(5);

            ViewBag.other = list1.Sum(y => y.Statecount);
            ViewBag.Otherper = Math.Round(list1.Sum(x => x.Statecount) * 100 / db.fnl_alysis.Where(x => new[] { "Myeloid  Leukemia", "Lymphoid  Leukemia", "Leukemia Uns" }.Contains(x.System3)).Select(x => x.STATE).Count(), 1);
            // ViewBag.totalper = db.fnl_alysis.Where(x => x.System4 == sys4 ).Select(x => x.STATE).Count() * 100 / db.fnl_alysis.Where(x => x.System4 == sys4 ).Select(x => x.STATE).Count();
            ViewBag.TotalCount = db.fnl_alysis.Where(x => new[] { "Myeloid  Leukemia", "Lymphoid  Leukemia", "Leukemia Uns" }.Contains(x.System3)).Select(x => x.STATE).Count();

            ViewBag.newcase = db.fnl_alysis.Where(x => new[] { "Myeloid  Leukemia", "Lymphoid  Leukemia", "Leukemia Uns" }.Contains(x.System3)).Where(x => x.PRVTMTYN == "N").Count();
            ViewBag.newcaseper = Math.Round((ViewBag.newcase) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.oldcase = db.fnl_alysis.Where(x => new[] { "Myeloid  Leukemia", "Lymphoid  Leukemia", "Leukemia Uns" }.Contains(x.System3)).Where(x => x.PRVTMTYN == "Y").Count();
            ViewBag.oldcaseper = Math.Round((ViewBag.oldcase) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.unkcase = db.fnl_alysis.Where(x => new[] { "Myeloid  Leukemia", "Lymphoid  Leukemia", "Leukemia Uns" }.Contains(x.System3)).Where(x => x.PRVTMTYN == "U").Count();
            ViewBag.unkcaseper = Math.Round((ViewBag.unkcase) * 100.0 / (ViewBag.TotalCount), 1);

            ViewBag.NewTretedTmh = db.fnl_alysis.Where(x => new[] { "Myeloid  Leukemia", "Lymphoid  Leukemia", "Leukemia Uns" }.Contains(x.System3)).Where(x => x.PRVTMTYN == "N" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.NewTretedTmhper = Math.Round((ViewBag.NewTretedTmh) * 100.0 / (ViewBag.newcase), 1);
            ViewBag.oldtreatedtmh = db.fnl_alysis.Where(x => new[] { "Myeloid  Leukemia", "Lymphoid  Leukemia", "Leukemia Uns" }.Contains(x.System3)).Where(x => x.PRVTMTYN == "Y" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.oldtreatedtmhper = Math.Round((ViewBag.oldtreatedtmh) * 100.0 / (ViewBag.oldcase), 1);
            ViewBag.untreatedtmh = db.fnl_alysis.Where(x => new[] { "Myeloid  Leukemia", "Lymphoid  Leukemia", "Leukemia Uns" }.Contains(x.System3)).Where(x => x.PRVTMTYN == "U" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.untreatedtmhper = Math.Round((ViewBag.untreatedtmh) * 100.0 / (ViewBag.unkcase), 1);
            if (double.IsNaN(ViewBag.untreatedtmhper))
            {
                ViewBag.untreatedtmhper = 0;
            }
            ViewBag.totalcase = (ViewBag.newcase + ViewBag.oldcase + ViewBag.unkcase);
            ViewBag.totalcaseper = (ViewBag.newcaseper + ViewBag.oldcaseper + ViewBag.unkcaseper);
            ViewBag.tottreatmh = (ViewBag.NewTretedTmh + ViewBag.oldtreatedtmh + ViewBag.untreatedtmh);
            ViewBag.tottreatmhper = Math.Round((ViewBag.tottreatmh) * 100.0 / (ViewBag.totalcase), 1);

            ViewBag.nontretnewcases = ((ViewBag.newcase) - (ViewBag.NewTretedTmh));
            ViewBag.nontretnewcasesper = Math.Round((ViewBag.nontretnewcases) * 100.0 / (ViewBag.newcase), 1);
            ViewBag.nontretoldcases = ((ViewBag.oldcase) - (ViewBag.oldtreatedtmh));
            ViewBag.nontretoldcasesper = Math.Round((ViewBag.nontretoldcases) * 100.0 / (ViewBag.oldcase), 1);


            ViewBag.agedata = db.fnl_alysis.Where(x => new[] { "Myeloid  Leukemia", "Lymphoid  Leukemia", "Leukemia Uns" }.Contains(x.System3)).GroupBy(x => x.BROAD_AGE).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => new[] { "Myeloid  Leukemia", "Lymphoid  Leukemia", "Leukemia Uns" }.Contains(x.System3)).Select(x => x.BROAD_AGE).Count(), 1) }).ToList();
            var objj = db.fnl_alysis.Where(x => new[] { "Myeloid  Leukemia", "Lymphoid  Leukemia", "Leukemia Uns" }.Contains(x.System3)).GroupBy(x => x.final_tti).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => new[] { "Myeloid  Leukemia", "Lymphoid  Leukemia", "Leukemia Uns" }.Contains(x.System3)).Select(x => x.final_tti).Count(), 1) }).ToList();
            ViewBag.timedata = objj.Where(x => x.Key != null);

            ViewBag.discharge = db.fnl_alysis.Where(x => new[] { "Myeloid  Leukemia", "Lymphoid  Leukemia", "Leukemia Uns" }.Contains(x.System3)).GroupBy(x => x.n_dissts).Select(g => new { Key = g.Key, Count = g.Count() }).ToList()
             .Select(x => new {
                 x.Key,
                 x.Count,
                 Color =
             x.Key == "Palliative Care" ? "#FFF200" :
             x.Key == "Regression" ? "#9CD66B" :
             x.Key == "Progression" ? "#FF4C4C" :
             x.Key == "NED" ? "#E78AC3" :
             x.Key == "Lost to Follow-up" ? "#2F3E75" :
             x.Key == "Death in Hospital" ? "#4FC3F7" :
             "#CCCCCC"
             }).ToList();

            ViewBag.System3 = "Leukemia";
            return View(list);
        }

        [HttpGet]
        public PartialViewResult Hodgkins_Disease()
        {

            var data = db.fnl_alysis.Where(x => x.System3 == "Hodgkins Disease").GroupBy(x => x.N_CLINEXT).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System3 == "Hodgkins Disease").Select(x => x.N_CLINEXT).Count(), 1) }).OrderBy(i => i.Key == "Not Known").ThenBy(i => i.Key == "Treated outside").ThenBy(i => i.Key == "Too Advanced").ThenBy(i => i.Key == "Dist. Mets").ThenBy(i => i.Key == "Regional").ThenBy(i => i.Key == "Localised").ToList();

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
            ViewBag.malecount = db.fnl_alysis.Where(x => x.SEX == "M" && x.System3 == "Hodgkins Disease").Count();
            ViewBag.femalecount = db.fnl_alysis.Where(x => x.SEX == "F" && x.System3 == "Hodgkins Disease").Count();

            var obj = db.fnl_alysis.Where(x => x.System3 == "Hodgkins Disease").GroupBy(x => x.TMH_RX).Select(y => new { y.Key, Count = y.Count() }).ToList();

            ViewBag.objectname = obj.Where(x => x.Key != null);
            var list = db.fnl_alysis.Where(x => x.System3 == "Hodgkins Disease").GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.System3 == "Hodgkins Disease").Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Take(5);
            var list1 = db.fnl_alysis.Where(x => x.System3 == "Hodgkins Disease").GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.System3 == "Hodgkins Disease").Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Skip(5);

            ViewBag.other = list1.Sum(y => y.Statecount);
            ViewBag.Otherper = Math.Round(list1.Sum(x => x.Statecount) * 100 / db.fnl_alysis.Where(x => x.System3 == "Hodgkins Disease").Select(x => x.STATE).Count(), 1);
            // ViewBag.totalper = db.fnl_alysis.Where(x => x.System4 == sys4 ).Select(x => x.STATE).Count() * 100 / db.fnl_alysis.Where(x => x.System4 == sys4 ).Select(x => x.STATE).Count();
            ViewBag.TotalCount = db.fnl_alysis.Where(x => x.System3 == "Hodgkins Disease").Select(x => x.STATE).Count();

            ViewBag.newcase = db.fnl_alysis.Where(x => x.System3 == "Hodgkins Disease").Where(x => x.PRVTMTYN == "N").Count();
            ViewBag.newcaseper = Math.Round((ViewBag.newcase) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.oldcase = db.fnl_alysis.Where(x => x.System3 == "Hodgkins Disease").Where(x => x.PRVTMTYN == "Y").Count();
            ViewBag.oldcaseper = Math.Round((ViewBag.oldcase) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.unkcase = db.fnl_alysis.Where(x => x.System3 == "Hodgkins Disease").Where(x => x.PRVTMTYN == "U").Count();
            ViewBag.unkcaseper = Math.Round((ViewBag.unkcase) * 100.0 / (ViewBag.TotalCount), 1);

            ViewBag.NewTretedTmh = db.fnl_alysis.Where(x => x.System3 == "Hodgkins Disease").Where(x => x.PRVTMTYN == "N" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.NewTretedTmhper = Math.Round((ViewBag.NewTretedTmh) * 100.0 / (ViewBag.newcase), 1);
            ViewBag.oldtreatedtmh = db.fnl_alysis.Where(x => x.System3 == "Hodgkins Disease").Where(x => x.PRVTMTYN == "Y" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.oldtreatedtmhper = Math.Round((ViewBag.oldtreatedtmh) * 100.0 / (ViewBag.oldcase), 1);
            ViewBag.untreatedtmh = db.fnl_alysis.Where(x => x.System3 == "Hodgkins Disease").Where(x => x.PRVTMTYN == "U" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.untreatedtmhper = Math.Round((ViewBag.untreatedtmh) * 100.0 / (ViewBag.unkcase), 1);
            if (double.IsNaN(ViewBag.untreatedtmhper))
            {
                ViewBag.untreatedtmhper = 0;
            }
            ViewBag.totalcase = (ViewBag.newcase + ViewBag.oldcase + ViewBag.unkcase);
            ViewBag.totalcaseper = (ViewBag.newcaseper + ViewBag.oldcaseper + ViewBag.unkcaseper);
            ViewBag.tottreatmh = (ViewBag.NewTretedTmh + ViewBag.oldtreatedtmh + ViewBag.untreatedtmh);
            ViewBag.tottreatmhper = Math.Round((ViewBag.tottreatmh) * 100.0 / (ViewBag.totalcase), 1);

            ViewBag.nontretnewcases = ((ViewBag.newcase) - (ViewBag.NewTretedTmh));
            ViewBag.nontretnewcasesper = Math.Round((ViewBag.nontretnewcases) * 100.0 / (ViewBag.newcase), 1);
            ViewBag.nontretoldcases = ((ViewBag.oldcase) - (ViewBag.oldtreatedtmh));
            ViewBag.nontretoldcasesper = Math.Round((ViewBag.nontretoldcases) * 100.0 / (ViewBag.oldcase), 1);

            ViewBag.agedata = db.fnl_alysis.Where(x => x.System3 == "Hodgkins Disease").GroupBy(x => x.BROAD_AGE).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System3 == "Hodgkins Disease").Select(x => x.BROAD_AGE).Count(), 1) }).ToList();
            var objj = db.fnl_alysis.Where(x => x.System3 == "Hodgkins Disease").GroupBy(x => x.final_tti).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System3 == "Hodgkins Disease").Select(x => x.final_tti).Count(), 1) }).ToList();
            ViewBag.timedata = objj.Where(x => x.Key != null);

            ViewBag.discharge = db.fnl_alysis.Where(x => x.System3 == "Hodgkins Disease").GroupBy(x => x.n_dissts).Select(g => new { Key = g.Key, Count = g.Count() }).ToList()
             .Select(x => new {
                 x.Key,
                 x.Count,
                 Color =
             x.Key == "Palliative Care" ? "#FFF200" :
             x.Key == "Regression" ? "#9CD66B" :
             x.Key == "Progression" ? "#FF4C4C" :
             x.Key == "NED" ? "#E78AC3" :
             x.Key == "Lost to Follow-up" ? "#2F3E75" :
             x.Key == "Death in Hospital" ? "#4FC3F7" :
             "#CCCCCC"
             }).ToList();

            ViewBag.System3 = "Hodgkins Disease";
            return PartialView("~/Views/SharedTMH22/_Hodgkins_Disease.cshtml", list);
        }

        [HttpGet]
        public PartialViewResult Leukemia()
        {

            var data = db.fnl_alysis.Where(x => new[] { "Myeloid  Leukemia", "Lymphoid  Leukemia", "Leukemia Uns" }.Contains(x.System3)).GroupBy(x => x.N_CLINEXT).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System3 == "Testis").Select(x => x.N_CLINEXT).Count(), 1) }).OrderBy(i => i.Key == "Not Known").ThenBy(i => i.Key == "Treated outside").ThenBy(i => i.Key == "Too Advanced").ThenBy(i => i.Key == "Dist. Mets").ThenBy(i => i.Key == "Regional").ThenBy(i => i.Key == "Localised").ToList();

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
            ViewBag.malecount = db.fnl_alysis.Where(x => x.SEX == "M" && new[] { "Myeloid  Leukemia", "Lymphoid  Leukemia", "Leukemia Uns" }.Contains(x.System3)).Count();
            ViewBag.femalecount = db.fnl_alysis.Where(x => x.SEX == "F" && new[] { "Myeloid  Leukemia", "Lymphoid  Leukemia", "Leukemia Uns" }.Contains(x.System3)).Count();

            var obj = db.fnl_alysis.Where(x => new[] { "Myeloid  Leukemia", "Lymphoid  Leukemia", "Leukemia Uns" }.Contains(x.System3)).GroupBy(x => x.TMH_RX).Select(y => new { y.Key, Count = y.Count() }).ToList();

            ViewBag.objectname = obj.Where(x => x.Key != null);
            var list = db.fnl_alysis.Where(x => new[] { "Myeloid  Leukemia", "Lymphoid  Leukemia", "Leukemia Uns" }.Contains(x.System3)).GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => new[] { "Myeloid  Leukemia", "Lymphoid  Leukemia", "Leukemia Uns" }.Contains(y.System3)).Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Take(5);
            var list1 = db.fnl_alysis.Where(x => new[] { "Myeloid  Leukemia", "Lymphoid  Leukemia", "Leukemia Uns" }.Contains(x.System3)).GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => new[] { "Myeloid  Leukemia", "Lymphoid  Leukemia", "Leukemia Uns" }.Contains(y.System3)).Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Skip(5);

            ViewBag.other = list1.Sum(y => y.Statecount);
            ViewBag.Otherper = Math.Round(list1.Sum(x => x.Statecount) * 100 / db.fnl_alysis.Where(x => new[] { "Myeloid  Leukemia", "Lymphoid  Leukemia", "Leukemia Uns" }.Contains(x.System3)).Select(x => x.STATE).Count(), 1);
            // ViewBag.totalper = db.fnl_alysis.Where(x => x.System4 == sys4 ).Select(x => x.STATE).Count() * 100 / db.fnl_alysis.Where(x => x.System4 == sys4 ).Select(x => x.STATE).Count();
            ViewBag.TotalCount = db.fnl_alysis.Where(x => new[] { "Myeloid  Leukemia", "Lymphoid  Leukemia", "Leukemia Uns" }.Contains(x.System3)).Select(x => x.STATE).Count();

            ViewBag.newcase = db.fnl_alysis.Where(x => new[] { "Myeloid  Leukemia", "Lymphoid  Leukemia", "Leukemia Uns" }.Contains(x.System3)).Where(x => x.PRVTMTYN == "N").Count();
            ViewBag.newcaseper = Math.Round((ViewBag.newcase) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.oldcase = db.fnl_alysis.Where(x => new[] { "Myeloid  Leukemia", "Lymphoid  Leukemia", "Leukemia Uns" }.Contains(x.System3)).Where(x => x.PRVTMTYN == "Y").Count();
            ViewBag.oldcaseper = Math.Round((ViewBag.oldcase) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.unkcase = db.fnl_alysis.Where(x => new[] { "Myeloid  Leukemia", "Lymphoid  Leukemia", "Leukemia Uns" }.Contains(x.System3)).Where(x => x.PRVTMTYN == "U").Count();
            ViewBag.unkcaseper = Math.Round((ViewBag.unkcase) * 100.0 / (ViewBag.TotalCount), 1);

            ViewBag.NewTretedTmh = db.fnl_alysis.Where(x => new[] { "Myeloid  Leukemia", "Lymphoid  Leukemia", "Leukemia Uns" }.Contains(x.System3)).Where(x => x.PRVTMTYN == "N" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.NewTretedTmhper = Math.Round((ViewBag.NewTretedTmh) * 100.0 / (ViewBag.newcase), 1);
            ViewBag.oldtreatedtmh = db.fnl_alysis.Where(x => new[] { "Myeloid  Leukemia", "Lymphoid  Leukemia", "Leukemia Uns" }.Contains(x.System3)).Where(x => x.PRVTMTYN == "Y" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.oldtreatedtmhper = Math.Round((ViewBag.oldtreatedtmh) * 100.0 / (ViewBag.oldcase), 1);
            ViewBag.untreatedtmh = db.fnl_alysis.Where(x => new[] { "Myeloid  Leukemia", "Lymphoid  Leukemia", "Leukemia Uns" }.Contains(x.System3)).Where(x => x.PRVTMTYN == "U" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.untreatedtmhper = Math.Round((ViewBag.untreatedtmh) * 100.0 / (ViewBag.unkcase), 1);
            if (double.IsNaN(ViewBag.untreatedtmhper))
            {
                ViewBag.untreatedtmhper = 0;
            }
            ViewBag.totalcase = (ViewBag.newcase + ViewBag.oldcase + ViewBag.unkcase);
            ViewBag.totalcaseper = (ViewBag.newcaseper + ViewBag.oldcaseper + ViewBag.unkcaseper);
            ViewBag.tottreatmh = (ViewBag.NewTretedTmh + ViewBag.oldtreatedtmh + ViewBag.untreatedtmh);
            ViewBag.tottreatmhper = Math.Round((ViewBag.tottreatmh) * 100.0 / (ViewBag.totalcase), 1);

            ViewBag.nontretnewcases = ((ViewBag.newcase) - (ViewBag.NewTretedTmh));
            ViewBag.nontretnewcasesper = Math.Round((ViewBag.nontretnewcases) * 100.0 / (ViewBag.newcase), 1);
            ViewBag.nontretoldcases = ((ViewBag.oldcase) - (ViewBag.oldtreatedtmh));
            ViewBag.nontretoldcasesper = Math.Round((ViewBag.nontretoldcases) * 100.0 / (ViewBag.oldcase), 1);


            ViewBag.agedata = db.fnl_alysis.Where(x => new[] { "Myeloid  Leukemia", "Lymphoid  Leukemia", "Leukemia Uns" }.Contains(x.System3)).GroupBy(x => x.BROAD_AGE).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => new[] { "Myeloid  Leukemia", "Lymphoid  Leukemia", "Leukemia Uns" }.Contains(x.System3)).Select(x => x.BROAD_AGE).Count(), 1) }).ToList();
            var objj = db.fnl_alysis.Where(x => new[] { "Myeloid  Leukemia", "Lymphoid  Leukemia", "Leukemia Uns" }.Contains(x.System3)).GroupBy(x => x.final_tti).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => new[] { "Myeloid  Leukemia", "Lymphoid  Leukemia", "Leukemia Uns" }.Contains(x.System3)).Select(x => x.final_tti).Count(), 1) }).ToList();
            ViewBag.timedata = objj.Where(x => x.Key != null);

            ViewBag.discharge = db.fnl_alysis.Where(x => new[] { "Myeloid  Leukemia", "Lymphoid  Leukemia", "Leukemia Uns" }.Contains(x.System3)).GroupBy(x => x.n_dissts).Select(g => new { Key = g.Key, Count = g.Count() }).ToList()
             .Select(x => new {
                 x.Key,
                 x.Count,
                 Color =
             x.Key == "Palliative Care" ? "#FFF200" :
             x.Key == "Regression" ? "#9CD66B" :
             x.Key == "Progression" ? "#FF4C4C" :
             x.Key == "NED" ? "#E78AC3" :
             x.Key == "Lost to Follow-up" ? "#2F3E75" :
             x.Key == "Death in Hospital" ? "#4FC3F7" :
             "#CCCCCC"
             }).ToList();

            ViewBag.System3 = "Leukemia";
            return PartialView("~/Views/SharedTMH22/_Leukemia.cshtml", list);
        }
    }
}