using Annual_Report.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PdfiumViewer;
using System.Drawing;
using System.Web.Mvc;
using OfficeOpenXml;
using System.IO;


namespace Annual_Report.Controllers
{
    public class HomeController : Controller
    {
        //private readonly HBCRReportContext _context;

        //public FinalAnalysisController(HBCRReportContext context)
        //{
        //    _context = context;
        //}
        DbEntity db = new DbEntity();




        public ActionResult trend()
        {
            return View();
        }
        



        // GET: Home
        [HttpGet]
        public ActionResult map()
        {
            //var Data = db.StateData.ToDictionary(s => s.Abbreviation,
            // s => new
            // {
            //     tooltip = new { text = s.TooltipText, backgroundColor = s.TooltipBackgroundColor },
            //     backgroundColor = s.BackgroundColor,
            //     label = new { text = s.LabelText, fontSize = s.LabelFontSize, textAlign = s.LabelTextAlign, visible = s.LabelVisible , fontWeight = "bold" }
            // });
            //ViewBag.data = Data;


            return View();
            //return PartialView("_map");
        }
        [HttpGet]
        public ActionResult Index()
        {


            return View();
        }

        public ActionResult All_sites()
        {
            var System4 = db.fnl_alysis.Select(x => x.System4).Distinct().ToList();
            var System4list = System4.ToList();
            ViewBag.System4 = new SelectList(System4list, System4, "");

         
            var males = db.fnl_alysis.Where(x => x.SEX == "M").Average(x => x.AGE_AT_REG);
            ViewBag.male = Math.Round(males, 0);
            var females = db.fnl_alysis.Where(x => x.SEX == "F").Average(x => x.AGE_AT_REG);
            ViewBag.female = Math.Round(females, 0);
            var total = db.fnl_alysis.Count();
            ViewBag.total = total;
            ViewBag.malecount = db.fnl_alysis.Where(x => x.SEX == "M").Count();
            ViewBag.femalecount = db.fnl_alysis.Where(x => x.SEX == "F").Count();
            ViewBag.Totalcancers = db.fnl_alysis.Select(x => x.PRVTMTYN).Count();
            var list = db.fnl_alysis.GroupBy(x => x.PRVTMTYN).Select(x => new final_analysisViewModel() { NewCase = x.Key, NewCaseCount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Select(y => y.PRVTMTYN).Count(), 1) }).ToList();

            ViewBag.totalper = db.fnl_alysis.Select(x => x.PRVTMTYN).Count() * 100 / db.fnl_alysis.Select(x => x.PRVTMTYN).Count();

            //  ViewBag.data = db.fnl_alysis.GroupBy(x => x.N_CLINEXT).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Select(x => x.N_CLINEXT).Count(), 1) }).OrderBy(i => i.Key == "Treated Outside").ThenBy(i => i.Key == "Unk. Primary").ThenBy(i => i.Key == "Not Known").ThenBy(i => i.Key == "N.A.").ThenBy(i => i.Key == "Too Advanced").ThenBy(i => i.Key == "Dist. Mets").ThenBy(i => i.Key == "Regional").ThenBy(i => i.Key == "Localised").ToList();

           //// var data = db.fnl_alysis.GroupBy(x => x.N_CLINEXT).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Select(x => x.N_CLINEXT).Count(), 1) }).OrderBy(i => i.Key == "Not Known").ThenBy(i => i.Key == "Treated outside").ThenBy(i => i.Key == "Too Advanced").ThenBy(i => i.Key == "Dist. Mets").ThenBy(i => i.Key == "Regional").ThenBy(i => i.Key == "Localised").ToList();
            var data = db.fnl_alysis.GroupBy(x => x.N_CLINEXT).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Select(x => x.N_CLINEXT).Count(), 1) }).OrderBy(i => i.Key == "Treated outside").ThenBy(i => i.Key == "Unk. Primary").ThenBy(i => i.Key == "Not Known").ThenBy(i => i.Key == "Not applicable").ThenBy(i => i.Key == "Too Advanced").ThenBy(i => i.Key == "Dist. Mets").ThenBy(i => i.Key == "Regional").ThenBy(i => i.Key == "Localised").ToList();


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









            var obj = db.fnl_alysis.GroupBy(x => x.TMH_RX).Select(y => new { y.Key, Count = y.Count() }).ToList();
            ViewBag.objectname = obj.Where(x => x.Key != null);


            ViewBag.newtmc = db.fnl_alysis.Where(x => x.PRVTMTYN == "N" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.oldtmc = db.fnl_alysis.Where(x => x.PRVTMTYN == "Y" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.untmc = db.fnl_alysis.Where(x => x.PRVTMTYN == "U" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();


            ViewBag.totaltre = db.fnl_alysis.Where(x => x.SIXTMT > 0).Select(y => y.SIXTMT).Count();

            ViewBag.newc = db.fnl_alysis.Where(x => x.PRVTMTYN == "N").Count();
            ViewBag.newper = Math.Round(ViewBag.newc * 100.0 / total, 1);

            ViewBag.oldc = db.fnl_alysis.Where(x => x.PRVTMTYN == "Y").Count();
            ViewBag.oldper = Math.Round(ViewBag.oldc * 100.0 / total, 1);

            ViewBag.unkc = db.fnl_alysis.Where(x => x.PRVTMTYN == "U").Count();
            ViewBag.unkper = Math.Round(ViewBag.unkc * 100.0 / total, 1);
            return View();
        }


        [HttpGet]
        public PartialViewResult Head_Neck()
        {
            // var data = db.fnl_alysis.Where(x => x.System4 == "Head & Neck").GroupBy(x => x.N_CLINEXT).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System4 == "Head & Neck").Select(x => x.N_CLINEXT).Count(), 1) }).OrderBy(i => i.Key == "Not Known").ThenBy(i => i.Key == "Treated outside").ThenBy(i => i.Key == "Too Advanced").ThenBy(i => i.Key == "Dist. Mets").ThenBy(i => i.Key == "Regional").ThenBy(i => i.Key == "Localised").ToList();

            var filtered = db.fnl_alysis
                 .Where(x => x.System4 == "Head & Neck");

            var totalCount = filtered.Count();

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

            ViewBag.malecount = db.fnl_alysis.Where(x => x.SEX == "M" && x.System4 == "Head & Neck").Count();
            ViewBag.femalecount = db.fnl_alysis.Where(x => x.SEX == "F" && x.System4 == "Head & Neck").Count();

            var obj = db.fnl_alysis.Where(x => x.System4 == "Head & Neck").GroupBy(x => x.TMH_RX).Select(y => new { y.Key, Count = y.Count() }).ToList();

            ViewBag.objectname = obj.Where(x => x.Key != null);

          




            var list = db.fnl_alysis.Where(x => x.System4 == "Head & Neck").GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.System4 == "Head & Neck").Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Take(5);
            var list1 = db.fnl_alysis.Where(x => x.System4 == "Head & Neck").GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.System4 == "Head & Neck").Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Skip(5);

            ViewBag.other = list1.Sum(y => y.Statecount);
            ViewBag.Otherper = Math.Round(list1.Sum(x => x.Statecount) * 100 / db.fnl_alysis.Where(x => x.System4 == "Head & Neck").Select(x => x.STATE).Count(), 1);
            ViewBag.TotalCount = db.fnl_alysis.Where(x => x.System4 == "Head & Neck").Select(x => x.STATE).Count();


            ViewBag.newcase = db.fnl_alysis.Where(x => x.System4 == "Head & Neck").Where(x => x.PRVTMTYN == "N").Count();
            ViewBag.newcaseper = Math.Round((ViewBag.newcase) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.oldcase = db.fnl_alysis.Where(x => x.System4 == "Head & Neck").Where(x => x.PRVTMTYN == "Y").Count();
            ViewBag.oldcaseper = Math.Round((ViewBag.oldcase) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.unkcase = db.fnl_alysis.Where(x => x.System4 == "Head & Neck").Where(x => x.PRVTMTYN == "U").Count();

            ViewBag.unkcaseper = Math.Round((ViewBag.unkcase) * 100.0 / (ViewBag.TotalCount), 1);

            ViewBag.NewTretedTmh = db.fnl_alysis.Where(x => x.System4 == "Head & Neck").Where(x => x.PRVTMTYN == "N" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.NewTretedTmhper = Math.Round((ViewBag.NewTretedTmh) * 100.0 / (ViewBag.newcase), 1);
            ViewBag.oldtreatedtmh = db.fnl_alysis.Where(x => x.System4 == "Head & Neck").Where(x => x.PRVTMTYN == "Y" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.oldtreatedtmhper = Math.Round((ViewBag.oldtreatedtmh) * 100.0 / (ViewBag.oldcase), 1);
            ViewBag.untreatedtmh = db.fnl_alysis.Where(x => x.System4 == "Head & Neck").Where(x => x.PRVTMTYN == "U" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();



            ViewBag.untreatedtmhper = Math.Round((ViewBag.untreatedtmh) * 100.0 / (ViewBag.unkcase), 1);
            if (double.IsNaN(ViewBag.untreatedtmhper))
            {
                ViewBag.untreatedtmhper = 0;
            }
            ViewBag.totalcase = (ViewBag.newcase + ViewBag.oldcase + ViewBag.unkcase);
            ViewBag.totalcaseper = (ViewBag.newcaseper + ViewBag.oldcaseper + ViewBag.unkcaseper);
            ViewBag.tottreatmh = (ViewBag.NewTretedTmh + ViewBag.oldtreatedtmh + ViewBag.untreatedtmh);
            ViewBag.tottreatmhper = Math.Round((ViewBag.tottreatmh) * 100.0 / (ViewBag.totalcase), 1);


            ViewBag.totaltre = db.fnl_alysis.Where(x => x.System4 == "Head & Neck").Where(x => x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.notunderwent = (ViewBag.TotalCount - ViewBag.totaltre);
            ViewBag.Outside = (ViewBag.TotalCount - ViewBag.newtmc);
            ViewBag.Per = Math.Round((ViewBag.totaltre) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.Per1 = Math.Round((ViewBag.notunderwent) * 100.0 / (ViewBag.TotalCount), 1);


            ViewBag.agedata = db.fnl_alysis.Where(x => x.System4 == "Head & Neck").GroupBy(x => x.BROAD_AGE).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System4 == "Head & Neck").Select(x => x.BROAD_AGE).Count(), 1) }).OrderBy(i => i.Key == ">=60").ThenBy(i => i.Key == "46-59").ThenBy(i => i.Key == "20-45").ThenBy(i => i.Key == "0-19").ToList();
            var objj = db.fnl_alysis.Where(x => x.System4 == "Head & Neck" && x.final_tti != null).GroupBy(x => x.final_tti).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System4 == "Head & Neck" && x.final_tti != null).Select(x => x.final_tti).Count(), 1) }).OrderBy(i => i.Key == ">180 days").ThenBy(i => i.Key == "91-180 days").ThenBy(i => i.Key == "31-90 days").ThenBy(i => i.Key == "<30 days").ToList();
            ViewBag.timedata = objj.Where(x => x.Key != null);


            ViewBag.System4 = "Head & Neck";
            return PartialView("_Head_Neck", list);
        }            //main


       
          public ActionResult Head_Neck_Table()
        {
            var priorityOrder = new List<string> { "Oral Cavity",  "Oropharynx", "Larynx", "Hypopharynx", "Para Nasal Sinus", "Salivary Gland (Major)", "Nasopharynx" };

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

          [HttpGet]
        public PartialViewResult Buccal_Mucosa()
        {
            var data = db.fnl_alysis.Where(x => x.System2 == "Buccal Mucosa").GroupBy(x => x.N_CLINEXT).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System2 == "Buccal Mucosa").Select(x => x.N_CLINEXT).Count(), 1) }).OrderBy(i => i.Key == "Not Known").ThenBy(i => i.Key == "Treated outside").ThenBy(i => i.Key == "Too Advanced").ThenBy(i => i.Key == "Dist. Mets").ThenBy(i => i.Key == "Regional").ThenBy(i => i.Key == "Localised").ToList();
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


            ViewBag.malecount = db.fnl_alysis.Where(x => x.SEX == "M" && x.System2 == "Buccal Mucosa").Count();
            ViewBag.femalecount = db.fnl_alysis.Where(x => x.SEX == "F" && x.System2 == "Buccal Mucosa").Count();

            var obj = db.fnl_alysis.Where(x => x.System2 == "Buccal Mucosa").GroupBy(x => x.TMH_RX).Select(y => new { y.Key, Count = y.Count() }).ToList();

            ViewBag.objectname = obj.Where(x => x.Key != null);
            var list = db.fnl_alysis.Where(x => x.System2 == "Buccal Mucosa").GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.System2 == "Buccal Mucosa").Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Take(5);
            var list1 = db.fnl_alysis.Where(x => x.System2 == "Buccal Mucosa").GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.System2 == "Buccal Mucosa").Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Skip(5);

            ViewBag.other = list1.Sum(y => y.Statecount);
            ViewBag.Otherper = Math.Round(list1.Sum(x => x.Statecount) * 100 / db.fnl_alysis.Where(x => x.System2 == "Buccal Mucosa").Select(x => x.STATE).Count(), 1);
            // ViewBag.totalper = db.fnl_alysis.Where(x => x.System4 == sys4 ).Select(x => x.STATE).Count() * 100 / db.fnl_alysis.Where(x => x.System4 == sys4 ).Select(x => x.STATE).Count();
            ViewBag.TotalCount = db.fnl_alysis.Where(x => x.System2 == "Buccal Mucosa").Select(x => x.STATE).Count();

            ViewBag.newcase = db.fnl_alysis.Where(x => x.System2 == "Buccal Mucosa").Where(x => x.PRVTMTYN == "N").Count();
            ViewBag.newcaseper = Math.Round((ViewBag.newcase) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.oldcase = db.fnl_alysis.Where(x => x.System2 == "Buccal Mucosa").Where(x => x.PRVTMTYN == "Y").Count();
            ViewBag.oldcaseper = Math.Round((ViewBag.oldcase) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.unkcase = db.fnl_alysis.Where(x => x.System2 == "Buccal Mucosa").Where(x => x.PRVTMTYN == "U").Count();
            ViewBag.unkcaseper = Math.Round((ViewBag.unkcase) * 100.0 / (ViewBag.TotalCount), 1);

            ViewBag.NewTretedTmh = db.fnl_alysis.Where(x => x.System2 == "Buccal Mucosa").Where(x => x.PRVTMTYN == "N" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.NewTretedTmhper = Math.Round((ViewBag.NewTretedTmh) * 100.0 / (ViewBag.newcase), 1);
            ViewBag.oldtreatedtmh = db.fnl_alysis.Where(x => x.System2 == "Buccal Mucosa").Where(x => x.PRVTMTYN == "Y" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.oldtreatedtmhper = Math.Round((ViewBag.oldtreatedtmh) * 100.0 / (ViewBag.oldcase), 1);
            ViewBag.untreatedtmh = db.fnl_alysis.Where(x => x.System2 == "Buccal Mucosa").Where(x => x.PRVTMTYN == "U" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.untreatedtmhper = Math.Round((ViewBag.untreatedtmh) * 100.0 / (ViewBag.unkcase), 1);
            if (double.IsNaN(ViewBag.untreatedtmhper))
            {
                ViewBag.untreatedtmhper = 0;
            }
            ViewBag.totalcase = (ViewBag.newcase + ViewBag.oldcase + ViewBag.unkcase);
            ViewBag.totalcaseper = (ViewBag.newcaseper + ViewBag.oldcaseper + ViewBag.unkcaseper);
            ViewBag.tottreatmh = (ViewBag.NewTretedTmh + ViewBag.oldtreatedtmh + ViewBag.untreatedtmh);
            ViewBag.tottreatmhper = Math.Round((ViewBag.tottreatmh) * 100.0 / (ViewBag.totalcase), 1);


            ViewBag.agedata = db.fnl_alysis.Where(x => x.System2 == "Buccal Mucosa").GroupBy(x => x.BROAD_AGE).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System2 == "Buccal Mucosa").Select(x => x.BROAD_AGE).Count(), 1) }).ToList();
            var objj = db.fnl_alysis.Where(x => x.System2 == "Buccal Mucosa").GroupBy(x => x.final_tti).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System2 == "Buccal Mucosa").Select(x => x.final_tti).Count(), 1) }).ToList();
            ViewBag.timedata = objj.Where(x => x.Key != null);

            ViewBag.System2 = "Buccal Mucosa";
            return PartialView("_Buccal_Mucosa", list);

        }

        [HttpGet]
        public PartialViewResult Anterior_Tongue()
        {
            var data = db.fnl_alysis.Where(x => x.System2 == "Anterior Tongue").GroupBy(x => x.N_CLINEXT).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System2 == "Anterior Tongue").Select(x => x.N_CLINEXT).Count(), 1) }).OrderBy(i => i.Key == "Not Known").ThenBy(i => i.Key == "Treated outside").ThenBy(i => i.Key == "Too Advanced").ThenBy(i => i.Key == "Dist. Mets").ThenBy(i => i.Key == "Regional").ThenBy(i => i.Key == "Localised").ToList();
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


            ViewBag.malecount = db.fnl_alysis.Where(x => x.SEX == "M" && x.System2 == "Anterior Tongue").Count();
            ViewBag.femalecount = db.fnl_alysis.Where(x => x.SEX == "F" && x.System2 == "Anterior Tongue").Count();

            var obj = db.fnl_alysis.Where(x => x.System2 == "Anterior Tongue").GroupBy(x => x.TMH_RX).Select(y => new { y.Key, Count = y.Count() }).ToList();

            ViewBag.objectname = obj.Where(x => x.Key != null);
            var list = db.fnl_alysis.Where(x => x.System2 == "Anterior Tongue").GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.System2 == "Anterior Tongue").Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Take(5);
            var list1 = db.fnl_alysis.Where(x => x.System2 == "Anterior Tongue").GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.System2 == "Anterior Tongue").Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Skip(5);

            ViewBag.other = list1.Sum(y => y.Statecount);
            ViewBag.Otherper = Math.Round(list1.Sum(x => x.Statecount) * 100 / db.fnl_alysis.Where(x => x.System2 == "Anterior Tongue").Select(x => x.STATE).Count(), 1);
            // ViewBag.totalper = db.fnl_alysis.Where(x => x.System4 == sys4 ).Select(x => x.STATE).Count() * 100 / db.fnl_alysis.Where(x => x.System4 == sys4 ).Select(x => x.STATE).Count();
            ViewBag.TotalCount = db.fnl_alysis.Where(x => x.System2 == "Anterior Tongue").Select(x => x.STATE).Count();

            ViewBag.newcase = db.fnl_alysis.Where(x => x.System2 == "Anterior Tongue").Where(x => x.PRVTMTYN == "N").Count();
            ViewBag.newcaseper = Math.Round((ViewBag.newcase) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.oldcase = db.fnl_alysis.Where(x => x.System2 == "Anterior Tongue").Where(x => x.PRVTMTYN == "Y").Count();
            ViewBag.oldcaseper = Math.Round((ViewBag.oldcase) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.unkcase = db.fnl_alysis.Where(x => x.System2 == "Anterior Tongue").Where(x => x.PRVTMTYN == "U").Count();
            ViewBag.unkcaseper = Math.Round((ViewBag.unkcase) * 100.0 / (ViewBag.TotalCount), 1);

            ViewBag.NewTretedTmh = db.fnl_alysis.Where(x => x.System2 == "Anterior Tongue").Where(x => x.PRVTMTYN == "N" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.NewTretedTmhper = Math.Round((ViewBag.NewTretedTmh) * 100.0 / (ViewBag.newcase), 1);
            ViewBag.oldtreatedtmh = db.fnl_alysis.Where(x => x.System2 == "Anterior Tongue").Where(x => x.PRVTMTYN == "Y" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.oldtreatedtmhper = Math.Round((ViewBag.oldtreatedtmh) * 100.0 / (ViewBag.oldcase), 1);
            ViewBag.untreatedtmh = db.fnl_alysis.Where(x => x.System2 == "Anterior Tongue").Where(x => x.PRVTMTYN == "U" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.untreatedtmhper = Math.Round((ViewBag.untreatedtmh) * 100.0 / (ViewBag.unkcase), 1);
            if (double.IsNaN(ViewBag.untreatedtmhper))
            {
                ViewBag.untreatedtmhper = 0;
            }
            ViewBag.totalcase = (ViewBag.newcase + ViewBag.oldcase + ViewBag.unkcase);
            ViewBag.totalcaseper = (ViewBag.newcaseper + ViewBag.oldcaseper + ViewBag.unkcaseper);
            ViewBag.tottreatmh = (ViewBag.NewTretedTmh + ViewBag.oldtreatedtmh + ViewBag.untreatedtmh);
            ViewBag.tottreatmhper = Math.Round((ViewBag.tottreatmh) * 100.0 / (ViewBag.totalcase), 1);


            ViewBag.agedata = db.fnl_alysis.Where(x => x.System2 == "Anterior Tongue").GroupBy(x => x.BROAD_AGE).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System2 == "Anterior Tongue").Select(x => x.BROAD_AGE).Count(), 1) }).ToList();
            var objj = db.fnl_alysis.Where(x => x.System2 == "Anterior Tongue").GroupBy(x => x.final_tti).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System2 == "Anterior Tongue").Select(x => x.final_tti).Count(), 1) }).ToList();
            ViewBag.timedata = objj.Where(x => x.Key != null);

            ViewBag.System2 = "Anterior Tongue";
            return PartialView("_Anterior_Tongue", list);
        }

        [HttpGet]
        public PartialViewResult Base_Tongue()
        {
            var data = db.fnl_alysis.Where(x => x.System2 == "Base Tongue").GroupBy(x => x.N_CLINEXT).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System2 == "Base Tongue").Select(x => x.N_CLINEXT).Count(), 1) }).OrderBy(i => i.Key == "Not Known").ThenBy(i => i.Key == "Treated outside").ThenBy(i => i.Key == "Too Advanced").ThenBy(i => i.Key == "Dist. Mets").ThenBy(i => i.Key == "Regional").ThenBy(i => i.Key == "Localised").ToList();
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


            ViewBag.malecount = db.fnl_alysis.Where(x => x.SEX == "M" && x.System2 == "Base Tongue").Count();
            ViewBag.femalecount = db.fnl_alysis.Where(x => x.SEX == "F" && x.System2 == "Base Tongue").Count();

            var obj = db.fnl_alysis.Where(x => x.System2 == "Base Tongue").GroupBy(x => x.TMH_RX).Select(y => new { y.Key, Count = y.Count() }).ToList();

            ViewBag.objectname = obj.Where(x => x.Key != null);
            var list = db.fnl_alysis.Where(x => x.System2 == "Base Tongue").GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.System2 == "Base Tongue").Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Take(5);
            var list1 = db.fnl_alysis.Where(x => x.System2 == "Base Tongue").GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.System2 == "Base Tongue").Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Skip(5);

            ViewBag.other = list1.Sum(y => y.Statecount);
            ViewBag.Otherper = Math.Round(list1.Sum(x => x.Statecount) * 100 / db.fnl_alysis.Where(x => x.System2 == "Base Tongue").Select(x => x.STATE).Count(), 1);
            // ViewBag.totalper = db.fnl_alysis.Where(x => x.System4 == sys4 ).Select(x => x.STATE).Count() * 100 / db.fnl_alysis.Where(x => x.System4 == sys4 ).Select(x => x.STATE).Count();
            ViewBag.TotalCount = db.fnl_alysis.Where(x => x.System2 == "Base Tongue").Select(x => x.STATE).Count();

            ViewBag.newcase = db.fnl_alysis.Where(x => x.System2 == "Base Tongue").Where(x => x.PRVTMTYN == "N").Count();
            ViewBag.newcaseper = Math.Round((ViewBag.newcase) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.oldcase = db.fnl_alysis.Where(x => x.System2 == "Base Tongue").Where(x => x.PRVTMTYN == "Y").Count();
            ViewBag.oldcaseper = Math.Round((ViewBag.oldcase) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.unkcase = db.fnl_alysis.Where(x => x.System2 == "Base Tongue").Where(x => x.PRVTMTYN == "U").Count();
            ViewBag.unkcaseper = Math.Round((ViewBag.unkcase) * 100.0 / (ViewBag.TotalCount), 1);

            ViewBag.NewTretedTmh = db.fnl_alysis.Where(x => x.System2 == "Base Tongue").Where(x => x.PRVTMTYN == "N" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.NewTretedTmhper = Math.Round((ViewBag.NewTretedTmh) * 100.0 / (ViewBag.newcase), 1);
            ViewBag.oldtreatedtmh = db.fnl_alysis.Where(x => x.System2 == "Base Tongue").Where(x => x.PRVTMTYN == "Y" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.oldtreatedtmhper = Math.Round((ViewBag.oldtreatedtmh) * 100.0 / (ViewBag.oldcase), 1);
            ViewBag.untreatedtmh = db.fnl_alysis.Where(x => x.System2 == "Base Tongue").Where(x => x.PRVTMTYN == "U" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.untreatedtmhper = Math.Round((ViewBag.untreatedtmh) * 100.0 / (ViewBag.unkcase), 1);
            if (double.IsNaN(ViewBag.untreatedtmhper))
            {
                ViewBag.untreatedtmhper = 0;
            }
            ViewBag.totalcase = (ViewBag.newcase + ViewBag.oldcase + ViewBag.unkcase);
            ViewBag.totalcaseper = (ViewBag.newcaseper + ViewBag.oldcaseper + ViewBag.unkcaseper);
            ViewBag.tottreatmh = (ViewBag.NewTretedTmh + ViewBag.oldtreatedtmh + ViewBag.untreatedtmh);
            ViewBag.tottreatmhper = Math.Round((ViewBag.tottreatmh) * 100.0 / (ViewBag.totalcase), 1);


            ViewBag.agedata = db.fnl_alysis.Where(x => x.System2 == "Base Tongue").GroupBy(x => x.BROAD_AGE).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System2 == "Base Tongue").Select(x => x.BROAD_AGE).Count(), 1) }).ToList();
            var objj = db.fnl_alysis.Where(x => x.System2 == "Base Tongue").GroupBy(x => x.final_tti).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System2 == "Base Tongue").Select(x => x.final_tti).Count(), 1) }).ToList();
            ViewBag.timedata = objj.Where(x => x.Key != null);

            ViewBag.System2 = "Base Tongue";
            return PartialView("_Base_Tongue", list);
        }

        [HttpGet]
        public PartialViewResult Tonsil()
        {
            var data = db.fnl_alysis.Where(x => x.System2 == "Tonsil").GroupBy(x => x.N_CLINEXT).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System2 == "Tonsil").Select(x => x.N_CLINEXT).Count(), 1) }).OrderBy(i => i.Key == "Not Known").ThenBy(i => i.Key == "Treated outside").ThenBy(i => i.Key == "Too Advanced").ThenBy(i => i.Key == "Dist. Mets").ThenBy(i => i.Key == "Regional").ThenBy(i => i.Key == "Localised").ToList();
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


            ViewBag.malecount = db.fnl_alysis.Where(x => x.SEX == "M" && x.System2 == "Tonsil").Count();
            ViewBag.femalecount = db.fnl_alysis.Where(x => x.SEX == "F" && x.System2 == "Tonsil").Count();

            var obj = db.fnl_alysis.Where(x => x.System2 == "Tonsil").GroupBy(x => x.TMH_RX).Select(y => new { y.Key, Count = y.Count() }).ToList();

            ViewBag.objectname = obj.Where(x => x.Key != null);
            var list = db.fnl_alysis.Where(x => x.System2 == "Tonsil").GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.System2 == "Tonsil").Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Take(5);
            var list1 = db.fnl_alysis.Where(x => x.System2 == "Tonsil").GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.System2 == "Tonsil").Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Skip(5);

            ViewBag.other = list1.Sum(y => y.Statecount);
            ViewBag.Otherper = Math.Round(list1.Sum(x => x.Statecount) * 100 / db.fnl_alysis.Where(x => x.System2 == "Tonsil").Select(x => x.STATE).Count(), 1);
            // ViewBag.totalper = db.fnl_alysis.Where(x => x.System4 == sys4 ).Select(x => x.STATE).Count() * 100 / db.fnl_alysis.Where(x => x.System4 == sys4 ).Select(x => x.STATE).Count();
            ViewBag.TotalCount = db.fnl_alysis.Where(x => x.System2 == "Tonsil").Select(x => x.STATE).Count();

            ViewBag.newcase = db.fnl_alysis.Where(x => x.System2 == "Tonsil").Where(x => x.PRVTMTYN == "N").Count();
            ViewBag.newcaseper = Math.Round((ViewBag.newcase) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.oldcase = db.fnl_alysis.Where(x => x.System2 == "Tonsil").Where(x => x.PRVTMTYN == "Y").Count();
            ViewBag.oldcaseper = Math.Round((ViewBag.oldcase) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.unkcase = db.fnl_alysis.Where(x => x.System2 == "Tonsil").Where(x => x.PRVTMTYN == "U").Count();
            ViewBag.unkcaseper = Math.Round((ViewBag.unkcase) * 100.0 / (ViewBag.TotalCount), 1);

            ViewBag.NewTretedTmh = db.fnl_alysis.Where(x => x.System2 == "Tonsil").Where(x => x.PRVTMTYN == "N" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.NewTretedTmhper = Math.Round((ViewBag.NewTretedTmh) * 100.0 / (ViewBag.newcase), 1);
            ViewBag.oldtreatedtmh = db.fnl_alysis.Where(x => x.System2 == "Tonsil").Where(x => x.PRVTMTYN == "Y" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.oldtreatedtmhper = Math.Round((ViewBag.oldtreatedtmh) * 100.0 / (ViewBag.oldcase), 1);
            ViewBag.untreatedtmh = db.fnl_alysis.Where(x => x.System2 == "Tonsil").Where(x => x.PRVTMTYN == "U" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.untreatedtmhper = Math.Round((ViewBag.untreatedtmh) * 100.0 / (ViewBag.unkcase), 1);
            if (double.IsNaN(ViewBag.untreatedtmhper))
            {
                ViewBag.untreatedtmhper = 0;
            }
            ViewBag.totalcase = (ViewBag.newcase + ViewBag.oldcase + ViewBag.unkcase);
            ViewBag.totalcaseper = (ViewBag.newcaseper + ViewBag.oldcaseper + ViewBag.unkcaseper);
            ViewBag.tottreatmh = (ViewBag.NewTretedTmh + ViewBag.oldtreatedtmh + ViewBag.untreatedtmh);
            ViewBag.tottreatmhper = Math.Round((ViewBag.tottreatmh) * 100.0 / (ViewBag.totalcase), 1);


            ViewBag.agedata = db.fnl_alysis.Where(x => x.System2 == "Tonsil").GroupBy(x => x.BROAD_AGE).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System2 == "Tonsil").Select(x => x.BROAD_AGE).Count(), 1) }).ToList();
            var objj = db.fnl_alysis.Where(x => x.System2 == "Tonsil").GroupBy(x => x.final_tti).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System2 == "Tonsil").Select(x => x.final_tti).Count(), 1) }).ToList();
            ViewBag.timedata = objj.Where(x => x.Key != null);

            ViewBag.System2 = "Tonsil";
            return PartialView("_Tonsil", list);
        }

        [HttpGet]
        public PartialViewResult Pyriform_Fossa()
        {
            var data = db.fnl_alysis.Where(x => x.System2 == "Pyriform Fossa").GroupBy(x => x.N_CLINEXT).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System2 == "Pyriform Fossa").Select(x => x.N_CLINEXT).Count(), 1) }).OrderBy(i => i.Key == "Not Known").ThenBy(i => i.Key == "Treated outside").ThenBy(i => i.Key == "Too Advanced").ThenBy(i => i.Key == "Dist. Mets").ThenBy(i => i.Key == "Regional").ThenBy(i => i.Key == "Localised").ToList();
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


            ViewBag.malecount = db.fnl_alysis.Where(x => x.SEX == "M" && x.System2 == "Pyriform Fossa").Count();
            ViewBag.femalecount = db.fnl_alysis.Where(x => x.SEX == "F" && x.System2 == "Pyriform Fossa").Count();

            var obj = db.fnl_alysis.Where(x => x.System2 == "Pyriform Fossa").GroupBy(x => x.TMH_RX).Select(y => new { y.Key, Count = y.Count() }).ToList();

            ViewBag.objectname = obj.Where(x => x.Key != null);
            var list = db.fnl_alysis.Where(x => x.System2 == "Pyriform Fossa").GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.System2 == "Pyriform Fossa").Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Take(5);
            var list1 = db.fnl_alysis.Where(x => x.System2 == "Pyriform Fossa").GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.System2 == "Pyriform Fossa").Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Skip(5);

            ViewBag.other = list1.Sum(y => y.Statecount);
            ViewBag.Otherper = Math.Round(list1.Sum(x => x.Statecount) * 100 / db.fnl_alysis.Where(x => x.System2 == "Pyriform Fossa").Select(x => x.STATE).Count(), 1);
            // ViewBag.totalper = db.fnl_alysis.Where(x => x.System4 == sys4 ).Select(x => x.STATE).Count() * 100 / db.fnl_alysis.Where(x => x.System4 == sys4 ).Select(x => x.STATE).Count();
            ViewBag.TotalCount = db.fnl_alysis.Where(x => x.System2 == "Pyriform Fossa").Select(x => x.STATE).Count();

            ViewBag.newcase = db.fnl_alysis.Where(x => x.System2 == "Pyriform Fossa").Where(x => x.PRVTMTYN == "N").Count();
            ViewBag.newcaseper = Math.Round((ViewBag.newcase) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.oldcase = db.fnl_alysis.Where(x => x.System2 == "Pyriform Fossa").Where(x => x.PRVTMTYN == "Y").Count();
            ViewBag.oldcaseper = Math.Round((ViewBag.oldcase) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.unkcase = db.fnl_alysis.Where(x => x.System2 == "Pyriform Fossa").Where(x => x.PRVTMTYN == "U").Count();
            ViewBag.unkcaseper = Math.Round((ViewBag.unkcase) * 100.0 / (ViewBag.TotalCount), 1);

            ViewBag.NewTretedTmh = db.fnl_alysis.Where(x => x.System2 == "Pyriform Fossa").Where(x => x.PRVTMTYN == "N" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.NewTretedTmhper = Math.Round((ViewBag.NewTretedTmh) * 100.0 / (ViewBag.newcase), 1);
            ViewBag.oldtreatedtmh = db.fnl_alysis.Where(x => x.System2 == "Pyriform Fossa").Where(x => x.PRVTMTYN == "Y" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.oldtreatedtmhper = Math.Round((ViewBag.oldtreatedtmh) * 100.0 / (ViewBag.oldcase), 1);
            ViewBag.untreatedtmh = db.fnl_alysis.Where(x => x.System2 == "Pyriform Fossa").Where(x => x.PRVTMTYN == "U" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.untreatedtmhper = Math.Round((ViewBag.untreatedtmh) * 100.0 / (ViewBag.unkcase), 1);
            if (double.IsNaN(ViewBag.untreatedtmhper))
            {
                ViewBag.untreatedtmhper = 0;
            }
            ViewBag.totalcase = (ViewBag.newcase + ViewBag.oldcase + ViewBag.unkcase);
            ViewBag.totalcaseper = (ViewBag.newcaseper + ViewBag.oldcaseper + ViewBag.unkcaseper);
            ViewBag.tottreatmh = (ViewBag.NewTretedTmh + ViewBag.oldtreatedtmh + ViewBag.untreatedtmh);
            ViewBag.tottreatmhper = Math.Round((ViewBag.tottreatmh) * 100.0 / (ViewBag.totalcase), 1);


            ViewBag.agedata = db.fnl_alysis.Where(x => x.System2 == "Pyriform Fossa").GroupBy(x => x.BROAD_AGE).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System2 == "Pyriform Fossa").Select(x => x.BROAD_AGE).Count(), 1) }).ToList();
            var objj = db.fnl_alysis.Where(x => x.System2 == "Pyriform Fossa").GroupBy(x => x.final_tti).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System2 == "Pyriform Fossa").Select(x => x.final_tti).Count(), 1) }).ToList();
            ViewBag.timedata = objj.Where(x => x.Key != null);

            ViewBag.System2 = "Pyriform Fossa";
            return PartialView("_Pyriform_Fossa", list);
        }

        [HttpGet]
        public PartialViewResult Supra_Glottic_Larynx()
        {
            var data = db.fnl_alysis.Where(x => x.System2 == "Supra Glottic Larynx").GroupBy(x => x.N_CLINEXT).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System2 == "Supra Glottic Larynx").Select(x => x.N_CLINEXT).Count(), 1) }).OrderBy(i => i.Key == "Not Known").ThenBy(i => i.Key == "Treated outside").ThenBy(i => i.Key == "Too Advanced").ThenBy(i => i.Key == "Dist. Mets").ThenBy(i => i.Key == "Regional").ThenBy(i => i.Key == "Localised").ToList();
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


            ViewBag.malecount = db.fnl_alysis.Where(x => x.SEX == "M" && x.System2 == "Supra Glottic Larynx").Count();
            ViewBag.femalecount = db.fnl_alysis.Where(x => x.SEX == "F" && x.System2 == "Supra Glottic Larynx").Count();

            var obj = db.fnl_alysis.Where(x => x.System2 == "Supra Glottic Larynx").GroupBy(x => x.TMH_RX).Select(y => new { y.Key, Count = y.Count() }).ToList();

            ViewBag.objectname = obj.Where(x => x.Key != null);
            var list = db.fnl_alysis.Where(x => x.System2 == "Supra Glottic Larynx").GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.System2 == "Supra Glottic Larynx").Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Take(5);
            var list1 = db.fnl_alysis.Where(x => x.System2 == "Supra Glottic Larynx").GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.System2 == "Supra Glottic Larynx").Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Skip(5);

            ViewBag.other = list1.Sum(y => y.Statecount);
            ViewBag.Otherper = Math.Round(list1.Sum(x => x.Statecount) * 100 / db.fnl_alysis.Where(x => x.System2 == "Supra Glottic Larynx").Select(x => x.STATE).Count(), 1);
            // ViewBag.totalper = db.fnl_alysis.Where(x => x.System4 == sys4 ).Select(x => x.STATE).Count() * 100 / db.fnl_alysis.Where(x => x.System4 == sys4 ).Select(x => x.STATE).Count();
            ViewBag.TotalCount = db.fnl_alysis.Where(x => x.System2 == "Supra Glottic Larynx").Select(x => x.STATE).Count();

            ViewBag.newcase = db.fnl_alysis.Where(x => x.System2 == "Supra Glottic Larynx").Where(x => x.PRVTMTYN == "N").Count();
            ViewBag.newcaseper = Math.Round((ViewBag.newcase) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.oldcase = db.fnl_alysis.Where(x => x.System2 == "Supra Glottic Larynx").Where(x => x.PRVTMTYN == "Y").Count();
            ViewBag.oldcaseper = Math.Round((ViewBag.oldcase) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.unkcase = db.fnl_alysis.Where(x => x.System2 == "Supra Glottic Larynx").Where(x => x.PRVTMTYN == "U").Count();
            ViewBag.unkcaseper = Math.Round((ViewBag.unkcase) * 100.0 / (ViewBag.TotalCount), 1);

            ViewBag.NewTretedTmh = db.fnl_alysis.Where(x => x.System2 == "Supra Glottic Larynx").Where(x => x.PRVTMTYN == "N" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.NewTretedTmhper = Math.Round((ViewBag.NewTretedTmh) * 100.0 / (ViewBag.newcase), 1);
            ViewBag.oldtreatedtmh = db.fnl_alysis.Where(x => x.System2 == "Supra Glottic Larynx").Where(x => x.PRVTMTYN == "Y" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.oldtreatedtmhper = Math.Round((ViewBag.oldtreatedtmh) * 100.0 / (ViewBag.oldcase), 1);
            ViewBag.untreatedtmh = db.fnl_alysis.Where(x => x.System2 == "Supra Glottic Larynx").Where(x => x.PRVTMTYN == "U" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.untreatedtmhper = Math.Round((ViewBag.untreatedtmh) * 100.0 / (ViewBag.unkcase), 1);
            if (double.IsNaN(ViewBag.untreatedtmhper))
            {
                ViewBag.untreatedtmhper = 0;
            }
            ViewBag.totalcase = (ViewBag.newcase + ViewBag.oldcase + ViewBag.unkcase);
            ViewBag.totalcaseper = (ViewBag.newcaseper + ViewBag.oldcaseper + ViewBag.unkcaseper);
            ViewBag.tottreatmh = (ViewBag.NewTretedTmh + ViewBag.oldtreatedtmh + ViewBag.untreatedtmh);
            ViewBag.tottreatmhper = Math.Round((ViewBag.tottreatmh) * 100.0 / (ViewBag.totalcase), 1);


            ViewBag.agedata = db.fnl_alysis.Where(x => x.System2 == "Supra Glottic Larynx").GroupBy(x => x.BROAD_AGE).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System2 == "Pyriform Fossa").Select(x => x.BROAD_AGE).Count(), 1) }).ToList();
            var objj = db.fnl_alysis.Where(x => x.System2 == "Supra Glottic Larynx").GroupBy(x => x.final_tti).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System2 == "Pyriform Fossa").Select(x => x.final_tti).Count(), 1) }).ToList();
            ViewBag.timedata = objj.Where(x => x.Key != null);

            ViewBag.System2 = "Supra Glottic Larynx";
            return PartialView("_Supra_Glottic_Larynx", list);
        }

        [HttpGet]
        public PartialViewResult Vocal_Cord()
        {
            var data = db.fnl_alysis.Where(x => x.System2 == "Vocal Cord").GroupBy(x => x.N_CLINEXT).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System2 == "Vocal Cord").Select(x => x.N_CLINEXT).Count(), 1) }).OrderBy(i => i.Key == "Not Known").ThenBy(i => i.Key == "Treated outside").ThenBy(i => i.Key == "Too Advanced").ThenBy(i => i.Key == "Dist. Mets").ThenBy(i => i.Key == "Regional").ThenBy(i => i.Key == "Localised").ToList();
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


            ViewBag.malecount = db.fnl_alysis.Where(x => x.SEX == "M" && x.System2 == "Vocal Cord").Count();
            ViewBag.femalecount = db.fnl_alysis.Where(x => x.SEX == "F" && x.System2 == "Vocal Cord").Count();

            var obj = db.fnl_alysis.Where(x => x.System2 == "Vocal Cord").GroupBy(x => x.TMH_RX).Select(y => new { y.Key, Count = y.Count() }).ToList();

            ViewBag.objectname = obj.Where(x => x.Key != null);
            var list = db.fnl_alysis.Where(x => x.System2 == "Vocal Cord").GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.System2 == "Vocal Cord").Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Take(5);
            var list1 = db.fnl_alysis.Where(x => x.System2 == "Vocal Cord").GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.System2 == "Vocal Cord").Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Skip(5);

            ViewBag.other = list1.Sum(y => y.Statecount);
            ViewBag.Otherper = Math.Round(list1.Sum(x => x.Statecount) * 100 / db.fnl_alysis.Where(x => x.System2 == "Vocal Cord").Select(x => x.STATE).Count(), 1);
            // ViewBag.totalper = db.fnl_alysis.Where(x => x.System4 == sys4 ).Select(x => x.STATE).Count() * 100 / db.fnl_alysis.Where(x => x.System4 == sys4 ).Select(x => x.STATE).Count();
            ViewBag.TotalCount = db.fnl_alysis.Where(x => x.System2 == "Vocal Cord").Select(x => x.STATE).Count();

            ViewBag.newcase = db.fnl_alysis.Where(x => x.System2 == "Vocal Cord").Where(x => x.PRVTMTYN == "N").Count();
            ViewBag.newcaseper = Math.Round((ViewBag.newcase) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.oldcase = db.fnl_alysis.Where(x => x.System2 == "Vocal Cord").Where(x => x.PRVTMTYN == "Y").Count();
            ViewBag.oldcaseper = Math.Round((ViewBag.oldcase) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.unkcase = db.fnl_alysis.Where(x => x.System2 == "Vocal Cord").Where(x => x.PRVTMTYN == "U").Count();
            ViewBag.unkcaseper = Math.Round((ViewBag.unkcase) * 100.0 / (ViewBag.TotalCount), 1);

            ViewBag.NewTretedTmh = db.fnl_alysis.Where(x => x.System2 == "Vocal Cord").Where(x => x.PRVTMTYN == "N" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.NewTretedTmhper = Math.Round((ViewBag.NewTretedTmh) * 100.0 / (ViewBag.newcase), 1);
            ViewBag.oldtreatedtmh = db.fnl_alysis.Where(x => x.System2 == "Vocal Cord").Where(x => x.PRVTMTYN == "Y" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.oldtreatedtmhper = Math.Round((ViewBag.oldtreatedtmh) * 100.0 / (ViewBag.oldcase), 1);
            ViewBag.untreatedtmh = db.fnl_alysis.Where(x => x.System2 == "Vocal Cord").Where(x => x.PRVTMTYN == "U" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.untreatedtmhper = Math.Round((ViewBag.untreatedtmh) * 100.0 / (ViewBag.unkcase), 1);
            if (double.IsNaN(ViewBag.untreatedtmhper))
            {
                ViewBag.untreatedtmhper = 0;
            }
            ViewBag.totalcase = (ViewBag.newcase + ViewBag.oldcase + ViewBag.unkcase);
            ViewBag.totalcaseper = (ViewBag.newcaseper + ViewBag.oldcaseper + ViewBag.unkcaseper);
            ViewBag.tottreatmh = (ViewBag.NewTretedTmh + ViewBag.oldtreatedtmh + ViewBag.untreatedtmh);
            ViewBag.tottreatmhper = Math.Round((ViewBag.tottreatmh) * 100.0 / (ViewBag.totalcase), 1);


            ViewBag.agedata = db.fnl_alysis.Where(x => x.System2 == "Vocal Cord").GroupBy(x => x.BROAD_AGE).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System2 == "Vocal Cord").Select(x => x.BROAD_AGE).Count(), 1) }).ToList();
            var objj = db.fnl_alysis.Where(x => x.System2 == "Vocal Cord").GroupBy(x => x.final_tti).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System2 == "Vocal Cord").Select(x => x.final_tti).Count(), 1) }).ToList();
            ViewBag.timedata = objj.Where(x => x.Key != null);

            ViewBag.System2 = "Vocal Cord";
            return PartialView("_Vocal_Cord", list);
        }

        [HttpGet]
        public PartialViewResult Nasopharynx()
        {
            var data = db.fnl_alysis.Where(x => x.System2 == "Nasopharynx").GroupBy(x => x.N_CLINEXT).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System2 == "Nasopharynx").Select(x => x.N_CLINEXT).Count(), 1) }).OrderBy(i => i.Key == "Not Known").ThenBy(i => i.Key == "Treated outside").ThenBy(i => i.Key == "Too Advanced").ThenBy(i => i.Key == "Dist. Mets").ThenBy(i => i.Key == "Regional").ThenBy(i => i.Key == "Localised").ToList();
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


            ViewBag.malecount = db.fnl_alysis.Where(x => x.SEX == "M" && x.System2 == "Nasopharynx").Count();
            ViewBag.femalecount = db.fnl_alysis.Where(x => x.SEX == "F" && x.System2 == "Nasopharynx").Count();

            var obj = db.fnl_alysis.Where(x => x.System2 == "Nasopharynx").GroupBy(x => x.TMH_RX).Select(y => new { y.Key, Count = y.Count() }).ToList();

            ViewBag.objectname = obj.Where(x => x.Key != null);
            var list = db.fnl_alysis.Where(x => x.System2 == "Nasopharynx").GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.System2 == "Nasopharynx").Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Take(5);
            var list1 = db.fnl_alysis.Where(x => x.System2 == "Nasopharynx").GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.System2 == "Nasopharynx").Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Skip(5);

            ViewBag.other = list1.Sum(y => y.Statecount);
            ViewBag.Otherper = Math.Round(list1.Sum(x => x.Statecount) * 100 / db.fnl_alysis.Where(x => x.System2 == "Nasopharynx").Select(x => x.STATE).Count(), 1);
            // ViewBag.totalper = db.fnl_alysis.Where(x => x.System4 == sys4 ).Select(x => x.STATE).Count() * 100 / db.fnl_alysis.Where(x => x.System4 == sys4 ).Select(x => x.STATE).Count();
            ViewBag.TotalCount = db.fnl_alysis.Where(x => x.System2 == "Nasopharynx").Select(x => x.STATE).Count();

            ViewBag.newcase = db.fnl_alysis.Where(x => x.System2 == "Nasopharynx").Where(x => x.PRVTMTYN == "N").Count();
            ViewBag.newcaseper = Math.Round((ViewBag.newcase) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.oldcase = db.fnl_alysis.Where(x => x.System2 == "Nasopharynx").Where(x => x.PRVTMTYN == "Y").Count();
            ViewBag.oldcaseper = Math.Round((ViewBag.oldcase) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.unkcase = db.fnl_alysis.Where(x => x.System2 == "Nasopharynx").Where(x => x.PRVTMTYN == "U").Count();
            ViewBag.unkcaseper = Math.Round((ViewBag.unkcase) * 100.0 / (ViewBag.TotalCount), 1);

            ViewBag.NewTretedTmh = db.fnl_alysis.Where(x => x.System2 == "Nasopharynx").Where(x => x.PRVTMTYN == "N" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.NewTretedTmhper = Math.Round((ViewBag.NewTretedTmh) * 100.0 / (ViewBag.newcase), 1);
            ViewBag.oldtreatedtmh = db.fnl_alysis.Where(x => x.System2 == "Nasopharynx").Where(x => x.PRVTMTYN == "Y" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.oldtreatedtmhper = Math.Round((ViewBag.oldtreatedtmh) * 100.0 / (ViewBag.oldcase), 1);
            ViewBag.untreatedtmh = db.fnl_alysis.Where(x => x.System2 == "Nasopharynx").Where(x => x.PRVTMTYN == "U" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.untreatedtmhper = Math.Round((ViewBag.untreatedtmh) * 100.0 / (ViewBag.unkcase), 1);
            if (double.IsNaN(ViewBag.untreatedtmhper))
            {
                ViewBag.untreatedtmhper = 0;
            }
            ViewBag.totalcase = (ViewBag.newcase + ViewBag.oldcase + ViewBag.unkcase);
            ViewBag.totalcaseper = (ViewBag.newcaseper + ViewBag.oldcaseper + ViewBag.unkcaseper);
            ViewBag.tottreatmh = (ViewBag.NewTretedTmh + ViewBag.oldtreatedtmh + ViewBag.untreatedtmh);
            ViewBag.tottreatmhper = Math.Round((ViewBag.tottreatmh) * 100.0 / (ViewBag.totalcase), 1);


            ViewBag.agedata = db.fnl_alysis.Where(x => x.System2 == "Nasopharynx").GroupBy(x => x.BROAD_AGE).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System2 == "Nasopharynx").Select(x => x.BROAD_AGE).Count(), 1) }).ToList();
            var objj = db.fnl_alysis.Where(x => x.System2 == "Nasopharynx").GroupBy(x => x.final_tti).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System2 == "Nasopharynx").Select(x => x.final_tti).Count(), 1) }).ToList();
            ViewBag.timedata = objj.Where(x => x.Key != null);

            ViewBag.System2 = "Nasopharynx";
            return PartialView("_Nasopharynx", list);
        }

        [HttpGet]
        public PartialViewResult Lung()
        {
            var data = db.fnl_alysis.Where(x => x.System4 == "Lung").GroupBy(x => x.N_CLINEXT).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System4 == "Lung").Select(x => x.N_CLINEXT).Count(), 1) }).OrderBy(i => i.Key == "Not Known").ThenBy(i => i.Key == "Treated outside").ThenBy(i => i.Key == "Too Advanced").ThenBy(i => i.Key == "Dist. Mets").ThenBy(i => i.Key == "Regional").ThenBy(i => i.Key == "Localised").ToList();


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

            ViewBag.malecount = db.fnl_alysis.Where(x => x.SEX == "M" && x.System4 == "Lung").Count();
            ViewBag.femalecount = db.fnl_alysis.Where(x => x.SEX == "F" && x.System4 == "Lung").Count();

            var obj = db.fnl_alysis.Where(x => x.System4 == "Lung").GroupBy(x => x.TMH_RX).Select(y => new { y.Key, Count = y.Count() }).ToList();

            ViewBag.objectname = obj.Where(x => x.Key != null);
            var list = db.fnl_alysis.Where(x => x.System4 == "Lung").GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.System4 == "Lung").Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Take(5);
            var list1 = db.fnl_alysis.Where(x => x.System4 == "Lung").GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.System4 == "Lung").Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Skip(5);

            ViewBag.other = list1.Sum(y => y.Statecount);
            ViewBag.Otherper = Math.Round(list1.Sum(x => x.Statecount) * 100 / db.fnl_alysis.Where(x => x.System4 == "Lung").Select(x => x.STATE).Count(), 1);
            ViewBag.TotalCount = db.fnl_alysis.Where(x => x.System4 == "Lung").Select(x => x.STATE).Count();


            ViewBag.newcase = db.fnl_alysis.Where(x => x.System4 == "Lung").Where(x => x.PRVTMTYN == "N").Count();
            ViewBag.newcaseper = Math.Round((ViewBag.newcase) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.oldcase = db.fnl_alysis.Where(x => x.System4 == "Lung").Where(x => x.PRVTMTYN == "Y").Count();
            ViewBag.oldcaseper = Math.Round((ViewBag.oldcase) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.unkcase = db.fnl_alysis.Where(x => x.System4 == "Lung").Where(x => x.PRVTMTYN == "U").Count();

            ViewBag.unkcaseper = Math.Round((ViewBag.unkcase) * 100.0 / (ViewBag.TotalCount), 1);

            ViewBag.NewTretedTmh = db.fnl_alysis.Where(x => x.System4 == "Lung").Where(x => x.PRVTMTYN == "N" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.NewTretedTmhper = Math.Round((ViewBag.NewTretedTmh) * 100.0 / (ViewBag.newcase), 1);
            ViewBag.oldtreatedtmh = db.fnl_alysis.Where(x => x.System4 == "Lung").Where(x => x.PRVTMTYN == "Y" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.oldtreatedtmhper = Math.Round((ViewBag.oldtreatedtmh) * 100.0 / (ViewBag.oldcase), 1);
            ViewBag.untreatedtmh = db.fnl_alysis.Where(x => x.System4 == "Lung").Where(x => x.PRVTMTYN == "U" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();



            ViewBag.untreatedtmhper = Math.Round((ViewBag.untreatedtmh) * 100.0 / (ViewBag.unkcase), 1);
            if (double.IsNaN(ViewBag.untreatedtmhper))
            {
                ViewBag.untreatedtmhper = 0;
            }
            ViewBag.totalcase = (ViewBag.newcase + ViewBag.oldcase + ViewBag.unkcase);
            ViewBag.totalcaseper = (ViewBag.newcaseper + ViewBag.oldcaseper + ViewBag.unkcaseper);
            ViewBag.tottreatmh = (ViewBag.NewTretedTmh + ViewBag.oldtreatedtmh + ViewBag.untreatedtmh);
            ViewBag.tottreatmhper = Math.Round((ViewBag.tottreatmh) * 100.0 / (ViewBag.totalcase), 1);


            ViewBag.totaltre = db.fnl_alysis.Where(x => x.System4 == "Lung").Where(x => x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.notunderwent = (ViewBag.TotalCount - ViewBag.totaltre);
            ViewBag.Outside = (ViewBag.TotalCount - ViewBag.newtmc);
            ViewBag.Per = Math.Round((ViewBag.totaltre) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.Per1 = Math.Round((ViewBag.notunderwent) * 100.0 / (ViewBag.TotalCount), 1);


            ViewBag.agedata = db.fnl_alysis.Where(x => x.System4 == "Lung").GroupBy(x => x.BROAD_AGE).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System4 == "Lung").Select(x => x.BROAD_AGE).Count(), 1) }).OrderBy(i => i.Key == ">=60").ThenBy(i => i.Key == "46-59").ThenBy(i => i.Key == "20-45").ThenBy(i => i.Key == "0-19").ToList();



            var objj = db.fnl_alysis.Where(x => x.System4 == "Lung" && x.final_tti != null).GroupBy(x => x.final_tti).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System4 == "Lung" && x.final_tti != null).Select(x => x.final_tti).Count(), 1) }).OrderBy(i => i.Key == ">180 days").ThenBy(i => i.Key == "91-180 days").ThenBy(i => i.Key == "31-90 days").ThenBy(i => i.Key == "<=30 days").ToList();



            ViewBag.timedata = objj.Where(x => x.Key != null);


            ViewBag.System4 = "Lung & Trachea";
            return PartialView("_Lung", list);
        }   //main

        [HttpGet]
        public PartialViewResult Gastro_intestinal()
        {
            var data = db.fnl_alysis.Where(x => x.System4 == "Gastro-intestinal").GroupBy(x => x.N_CLINEXT).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System4 == "Gastro-intestinal").Select(x => x.N_CLINEXT).Count(), 1) }).OrderBy(i => i.Key == "Not Known").ThenBy(i => i.Key == "Treated outside").ThenBy(i => i.Key == "Too Advanced").ThenBy(i => i.Key == "Dist. Mets").ThenBy(i => i.Key == "Regional").ThenBy(i => i.Key == "Localised").ToList();


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

            ViewBag.malecount = db.fnl_alysis.Where(x => x.SEX == "M" && x.System4 == "Gastro-intestinal").Count();
            ViewBag.femalecount = db.fnl_alysis.Where(x => x.SEX == "F" && x.System4 == "Gastro-intestinal").Count();

            var obj = db.fnl_alysis.Where(x => x.System4 == "Gastro-intestinal").GroupBy(x => x.TMH_RX).Select(y => new { y.Key, Count = y.Count() }).ToList();

            ViewBag.objectname = obj.Where(x => x.Key != null);
            var list = db.fnl_alysis.Where(x => x.System4 == "Gastro-intestinal").GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.System4 == "Gastro-intestinal").Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Take(5);
            var list1 = db.fnl_alysis.Where(x => x.System4 == "Gastro-intestinal").GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.System4 == "Gastro-intestinal").Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Skip(5);

            ViewBag.other = list1.Sum(y => y.Statecount);
            ViewBag.Otherper = Math.Round(list1.Sum(x => x.Statecount) * 100 / db.fnl_alysis.Where(x => x.System4 == "Gastro-intestinal").Select(x => x.STATE).Count(), 1);
            ViewBag.TotalCount = db.fnl_alysis.Where(x => x.System4 == "Gastro-intestinal").Select(x => x.STATE).Count();


            ViewBag.newcase = db.fnl_alysis.Where(x => x.System4 == "Gastro-intestinal").Where(x => x.PRVTMTYN == "N").Count();
            ViewBag.newcaseper = Math.Round((ViewBag.newcase) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.oldcase = db.fnl_alysis.Where(x => x.System4 == "Gastro-intestinal").Where(x => x.PRVTMTYN == "Y").Count();
            ViewBag.oldcaseper = Math.Round((ViewBag.oldcase) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.unkcase = db.fnl_alysis.Where(x => x.System4 == "Gastro-intestinal").Where(x => x.PRVTMTYN == "U").Count();

            ViewBag.unkcaseper = Math.Round((ViewBag.unkcase) * 100.0 / (ViewBag.TotalCount), 1);

            ViewBag.NewTretedTmh = db.fnl_alysis.Where(x => x.System4 == "Gastro-intestinal").Where(x => x.PRVTMTYN == "N" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.NewTretedTmhper = Math.Round((ViewBag.NewTretedTmh) * 100.0 / (ViewBag.newcase), 1);
            ViewBag.oldtreatedtmh = db.fnl_alysis.Where(x => x.System4 == "Gastro-intestinal").Where(x => x.PRVTMTYN == "Y" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.oldtreatedtmhper = Math.Round((ViewBag.oldtreatedtmh) * 100.0 / (ViewBag.oldcase), 1);
            ViewBag.untreatedtmh = db.fnl_alysis.Where(x => x.System4 == "Gastro-intestinal").Where(x => x.PRVTMTYN == "U" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();



            ViewBag.untreatedtmhper = Math.Round((ViewBag.untreatedtmh) * 100.0 / (ViewBag.unkcase), 1);
            if (double.IsNaN(ViewBag.untreatedtmhper))
            {
                ViewBag.untreatedtmhper = 0;
            }
            ViewBag.totalcase = (ViewBag.newcase + ViewBag.oldcase + ViewBag.unkcase);
            ViewBag.totalcaseper = (ViewBag.newcaseper + ViewBag.oldcaseper + ViewBag.unkcaseper);
            ViewBag.tottreatmh = (ViewBag.NewTretedTmh + ViewBag.oldtreatedtmh + ViewBag.untreatedtmh);
            ViewBag.tottreatmhper = Math.Round((ViewBag.tottreatmh) * 100.0 / (ViewBag.totalcase), 1);


            ViewBag.totaltre = db.fnl_alysis.Where(x => x.System4 == "Gastro-intestinal").Where(x => x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.notunderwent = (ViewBag.TotalCount - ViewBag.totaltre);
            ViewBag.Outside = (ViewBag.TotalCount - ViewBag.newtmc);
            ViewBag.Per = Math.Round((ViewBag.totaltre) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.Per1 = Math.Round((ViewBag.notunderwent) * 100.0 / (ViewBag.TotalCount), 1);


            ViewBag.agedata = db.fnl_alysis.Where(x => x.System4 == "Gastro-intestinal").GroupBy(x => x.BROAD_AGE).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System4 == "Gastro-intestinal").Select(x => x.BROAD_AGE).Count(), 1) }).OrderBy(i => i.Key == ">=60").ThenBy(i => i.Key == "46-59").ThenBy(i => i.Key == "20-45").ThenBy(i => i.Key == "0-19").ToList();



            var objj = db.fnl_alysis.Where(x => x.System4 == "Gastro-intestinal" && x.final_tti != null).GroupBy(x => x.final_tti).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System4 == "Gastro-intestinal" && x.final_tti != null).Select(x => x.final_tti).Count(), 1) }).OrderBy(i => i.Key == ">180 days").ThenBy(i => i.Key == "91-180 days").ThenBy(i => i.Key == "31-90 days").ThenBy(i => i.Key == "<=30 days").ToList();



            ViewBag.timedata = objj.Where(x => x.Key != null);

            //table
            ViewBag.TotalGUSCount = db.fnl_alysis.Where(x => x.System4 == "Gastro-intestinal").Count();

            ViewBag.System3Values = db.fnl_alysis.Where(x => x.System4 == "Gastro-intestinal").GroupBy(x => x.System3).Select(g => new System3StatsViewModel
            {
                System3 = g.Key,
                MaleCount = g.Where(x => x.SEX == "M").Count(),
                FemaleCount = g.Where(x => x.SEX == "F").Count(),
                TotalCount = db.fnl_alysis.Where(x => x.SEX == "M" && x.System4 == "Gastro-intestinal").Count(),
                TotalFCount = db.fnl_alysis.Where(x => x.SEX == "F" && x.System4 == "Gastro-intestinal").Count(),
                TotalNosCount = g.Count(x => x.SEX == "M") + g.Count(x => x.SEX == "F"),

            })
  .ToList();

            ViewBag.tMcount = db.fnl_alysis.Where(x => x.System4 == "Gastro-intestinal" && x.SEX == "M").Count();
            ViewBag.tFcount = db.fnl_alysis.Where(x => x.System4 == "Gastro-intestinal" && x.SEX == "F").Count();






            ViewBag.System4 = "Gastrointestinal Tract Cancers";
            return PartialView("_Gastro_intestinal", list);
        }    //main


        public ActionResult Gastrointestinal_Table()
        {
            ViewBag.TotalGUSCount = db.fnl_alysis.Where(x => x.System4 == "Gastro-intestinal").Count();

            ViewBag.System3Values = db.fnl_alysis.Where(x => x.System4 == "Gastro-intestinal").GroupBy(x => x.System3).Select(g => new System3StatsViewModel
            {
                System3 = g.Key,
                MaleCount = g.Where(x => x.SEX == "M").Count(),
                FemaleCount = g.Where(x => x.SEX == "F").Count(),
                TotalCount = db.fnl_alysis.Where(x => x.SEX == "M" && x.System4 == "Gastro-intestinal").Count(),
                TotalFCount = db.fnl_alysis.Where(x => x.SEX == "F" && x.System4 == "Gastro-intestinal").Count(),
                TotalNosCount = g.Count(x => x.SEX == "M") + g.Count(x => x.SEX == "F"),

            })
  .ToList();

            ViewBag.tMcount = db.fnl_alysis.Where(x => x.System4 == "Gastro-intestinal" && x.SEX == "M").Count();
            ViewBag.tFcount = db.fnl_alysis.Where(x => x.System4 == "Gastro-intestinal" && x.SEX == "F").Count();
            return View();
        }  // table

        [HttpGet]
        public PartialViewResult Oesophagus()
        {

            var data = db.fnl_alysis.Where(x => new[] { "Oesophagus NOS", "Oesophagus Lower Third", "Oesophagus Middle Third", "Oesophagus Upper Third" }.Contains(x.System2)).GroupBy(x => x.N_CLINEXT).Select(g => new { g.Key, Count = Math.Round(g.Count() * 100.0 / db.fnl_alysis.Count(y => new[] { "Oesophagus NOS", "Oesophagus Lower Third", "Oesophagus Middle Third", "Oesophagus Upper Third" }.Contains(y.System2)), 1) }).OrderBy(x => x.Key == "Not Known").ThenBy(x => x.Key == "Treated outside").ThenBy(x => x.Key == "Too Advanced").ThenBy(x => x.Key == "Dist. Mets").ThenBy(x => x.Key == "Regional").ThenBy(x => x.Key == "Localised").ToList();

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
            ViewBag.malecount = db.fnl_alysis.Where(x => x.SEX == "M" && x.System3 == "Oesophagus").Count();
            ViewBag.femalecount = db.fnl_alysis.Where(x => x.SEX == "F" && x.System3 == "Oesophagus").Count();

            var obj = db.fnl_alysis.Where(x => x.System3 == "Oesophagus").GroupBy(x => x.TMH_RX).Select(y => new { y.Key, Count = y.Count() }).ToList();

            ViewBag.objectname = obj.Where(x => x.Key != null);
            var list = db.fnl_alysis.Where(x => x.System3 == "Oesophagus").GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.System3 == "Oesophagus").Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Take(5);
            var list1 = db.fnl_alysis.Where(x => x.System3 == "Oesophagus").GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.System3 == "Oesophagus").Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Skip(5);

            ViewBag.other = list1.Sum(y => y.Statecount);
            ViewBag.Otherper = Math.Round(list1.Sum(x => x.Statecount) * 100 / db.fnl_alysis.Where(x => x.System3 == "Oesophagus").Select(x => x.STATE).Count(), 1);
            // ViewBag.totalper = db.fnl_alysis.Where(x => x.System4 == sys4 ).Select(x => x.STATE).Count() * 100 / db.fnl_alysis.Where(x => x.System4 == sys4 ).Select(x => x.STATE).Count();
            ViewBag.TotalCount = db.fnl_alysis.Where(x => x.System3 == "Oesophagus").Select(x => x.STATE).Count();

            ViewBag.newcase = db.fnl_alysis.Where(x => x.System3 == "Oesophagus").Where(x => x.PRVTMTYN == "N").Count();
            ViewBag.newcaseper = Math.Round((ViewBag.newcase) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.oldcase = db.fnl_alysis.Where(x => x.System3 == "Oesophagus").Where(x => x.PRVTMTYN == "Y").Count();
            ViewBag.oldcaseper = Math.Round((ViewBag.oldcase) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.unkcase = db.fnl_alysis.Where(x => x.System3 == "Oesophagus").Where(x => x.PRVTMTYN == "U").Count();
            ViewBag.unkcaseper = Math.Round((ViewBag.unkcase) * 100.0 / (ViewBag.TotalCount), 1);

            ViewBag.NewTretedTmh = db.fnl_alysis.Where(x => x.System3 == "Oesophagus").Where(x => x.PRVTMTYN == "N" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.NewTretedTmhper = Math.Round((ViewBag.NewTretedTmh) * 100.0 / (ViewBag.newcase), 1);
            ViewBag.oldtreatedtmh = db.fnl_alysis.Where(x => x.System3 == "Oesophagus").Where(x => x.PRVTMTYN == "Y" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.oldtreatedtmhper = Math.Round((ViewBag.oldtreatedtmh) * 100.0 / (ViewBag.oldcase), 1);
            ViewBag.untreatedtmh = db.fnl_alysis.Where(x => x.System3 == "Oesophagus").Where(x => x.PRVTMTYN == "U" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.untreatedtmhper = Math.Round((ViewBag.untreatedtmh) * 100.0 / (ViewBag.unkcase), 1);
            if (double.IsNaN(ViewBag.untreatedtmhper))
            {
                ViewBag.untreatedtmhper = 0;
            }
            ViewBag.totalcase = (ViewBag.newcase + ViewBag.oldcase + ViewBag.unkcase);
            ViewBag.totalcaseper = (ViewBag.newcaseper + ViewBag.oldcaseper + ViewBag.unkcaseper);
            ViewBag.tottreatmh = (ViewBag.NewTretedTmh + ViewBag.oldtreatedtmh + ViewBag.untreatedtmh);
            ViewBag.tottreatmhper = Math.Round((ViewBag.tottreatmh) * 100.0 / (ViewBag.totalcase), 1);


            ViewBag.agedata = db.fnl_alysis.Where(x => x.System3 == "Oesophagus").GroupBy(x => x.BROAD_AGE).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System3 == "Oesophagus").Select(x => x.BROAD_AGE).Count(), 1) }).ToList();
            var objj = db.fnl_alysis.Where(x => x.System3 == "Oesophagus").GroupBy(x => x.final_tti).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System3 == "Oesophagus").Select(x => x.final_tti).Count(), 1) }).ToList();
            ViewBag.timedata = objj.Where(x => x.Key != null);

            ViewBag.System3 = "Oesophagus";
            return PartialView("_Oesophagus", list);

        }

        [HttpGet]
        public PartialViewResult Stomach()
        {

            var data = db.fnl_alysis.Where(x => x.System3 == "Stomach").GroupBy(x => x.N_CLINEXT).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System3 == "Stomach").Select(x => x.N_CLINEXT).Count(), 1) }).OrderBy(i => i.Key == "Not Known").ThenBy(i => i.Key == "Treated outside").ThenBy(i => i.Key == "Too Advanced").ThenBy(i => i.Key == "Dist. Mets").ThenBy(i => i.Key == "Regional").ThenBy(i => i.Key == "Localised").ToList();

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
            ViewBag.malecount = db.fnl_alysis.Where(x => x.SEX == "M" && x.System3 == "Stomach").Count();
            ViewBag.femalecount = db.fnl_alysis.Where(x => x.SEX == "F" && x.System3 == "Stomach").Count();

            var obj = db.fnl_alysis.Where(x => x.System3 == "Stomach").GroupBy(x => x.TMH_RX).Select(y => new { y.Key, Count = y.Count() }).ToList();

            ViewBag.objectname = obj.Where(x => x.Key != null);
            var list = db.fnl_alysis.Where(x => x.System3 == "Stomach").GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.System3 == "Stomach").Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Take(5);
            var list1 = db.fnl_alysis.Where(x => x.System3 == "Stomach").GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.System3 == "Stomach").Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Skip(5);

            ViewBag.other = list1.Sum(y => y.Statecount);
            ViewBag.Otherper = Math.Round(list1.Sum(x => x.Statecount) * 100 / db.fnl_alysis.Where(x => x.System3 == "Stomach").Select(x => x.STATE).Count(), 1);
            // ViewBag.totalper = db.fnl_alysis.Where(x => x.System4 == sys4 ).Select(x => x.STATE).Count() * 100 / db.fnl_alysis.Where(x => x.System4 == sys4 ).Select(x => x.STATE).Count();
            ViewBag.TotalCount = db.fnl_alysis.Where(x => x.System3 == "Stomach").Select(x => x.STATE).Count();

            ViewBag.newcase = db.fnl_alysis.Where(x => x.System3 == "Stomach").Where(x => x.PRVTMTYN == "N").Count();
            ViewBag.newcaseper = Math.Round((ViewBag.newcase) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.oldcase = db.fnl_alysis.Where(x => x.System3 == "Stomach").Where(x => x.PRVTMTYN == "Y").Count();
            ViewBag.oldcaseper = Math.Round((ViewBag.oldcase) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.unkcase = db.fnl_alysis.Where(x => x.System3 == "Stomach").Where(x => x.PRVTMTYN == "U").Count();
            ViewBag.unkcaseper = Math.Round((ViewBag.unkcase) * 100.0 / (ViewBag.TotalCount), 1);

            ViewBag.NewTretedTmh = db.fnl_alysis.Where(x => x.System3 == "Stomach").Where(x => x.PRVTMTYN == "N" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.NewTretedTmhper = Math.Round((ViewBag.NewTretedTmh) * 100.0 / (ViewBag.newcase), 1);
            ViewBag.oldtreatedtmh = db.fnl_alysis.Where(x => x.System3 == "Stomach").Where(x => x.PRVTMTYN == "Y" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.oldtreatedtmhper = Math.Round((ViewBag.oldtreatedtmh) * 100.0 / (ViewBag.oldcase), 1);
            ViewBag.untreatedtmh = db.fnl_alysis.Where(x => x.System3 == "Stomach").Where(x => x.PRVTMTYN == "U" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.untreatedtmhper = Math.Round((ViewBag.untreatedtmh) * 100.0 / (ViewBag.unkcase), 1);
            if (double.IsNaN(ViewBag.untreatedtmhper))
            {
                ViewBag.untreatedtmhper = 0;
            }
            ViewBag.totalcase = (ViewBag.newcase + ViewBag.oldcase + ViewBag.unkcase);
            ViewBag.totalcaseper = (ViewBag.newcaseper + ViewBag.oldcaseper + ViewBag.unkcaseper);
            ViewBag.tottreatmh = (ViewBag.NewTretedTmh + ViewBag.oldtreatedtmh + ViewBag.untreatedtmh);
            ViewBag.tottreatmhper = Math.Round((ViewBag.tottreatmh) * 100.0 / (ViewBag.totalcase), 1);


            ViewBag.agedata = db.fnl_alysis.Where(x => x.System3 == "Stomach").GroupBy(x => x.BROAD_AGE).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System3 == "Stomach").Select(x => x.BROAD_AGE).Count(), 1) }).ToList();
            var objj = db.fnl_alysis.Where(x => x.System3 == "Stomach").GroupBy(x => x.final_tti).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System3 == "Stomach").Select(x => x.final_tti).Count(), 1) }).ToList();
            ViewBag.timedata = objj.Where(x => x.Key != null);

            ViewBag.System3 = "Stomach";
            return PartialView("_Stomach", list);

        }

        [HttpGet]
        public PartialViewResult Colon()
        {

            var data = db.fnl_alysis.Where(x => x.System3 == "Colon").GroupBy(x => x.N_CLINEXT).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System3 == "Colon").Select(x => x.N_CLINEXT).Count(), 1) }).OrderBy(i => i.Key == "Not Known").ThenBy(i => i.Key == "Treated outside").ThenBy(i => i.Key == "Too Advanced").ThenBy(i => i.Key == "Dist. Mets").ThenBy(i => i.Key == "Regional").ThenBy(i => i.Key == "Localised").ToList();

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
            ViewBag.malecount = db.fnl_alysis.Where(x => x.SEX == "M" && x.System3 == "Colon").Count();
            ViewBag.femalecount = db.fnl_alysis.Where(x => x.SEX == "F" && x.System3 == "Colon").Count();

            var obj = db.fnl_alysis.Where(x => x.System3 == "Colon").GroupBy(x => x.TMH_RX).Select(y => new { y.Key, Count = y.Count() }).ToList();

            ViewBag.objectname = obj.Where(x => x.Key != null);
            var list = db.fnl_alysis.Where(x => x.System3 == "Colon").GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.System3 == "Colon").Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Take(5);
            var list1 = db.fnl_alysis.Where(x => x.System3 == "Colon").GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.System3 == "Colon").Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Skip(5);

            ViewBag.other = list1.Sum(y => y.Statecount);
            ViewBag.Otherper = Math.Round(list1.Sum(x => x.Statecount) * 100 / db.fnl_alysis.Where(x => x.System3 == "Colon").Select(x => x.STATE).Count(), 1);
            // ViewBag.totalper = db.fnl_alysis.Where(x => x.System4 == sys4 ).Select(x => x.STATE).Count() * 100 / db.fnl_alysis.Where(x => x.System4 == sys4 ).Select(x => x.STATE).Count();
            ViewBag.TotalCount = db.fnl_alysis.Where(x => x.System3 == "Colon").Select(x => x.STATE).Count();

            ViewBag.newcase = db.fnl_alysis.Where(x => x.System3 == "Colon").Where(x => x.PRVTMTYN == "N").Count();
            ViewBag.newcaseper = Math.Round((ViewBag.newcase) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.oldcase = db.fnl_alysis.Where(x => x.System3 == "Colon").Where(x => x.PRVTMTYN == "Y").Count();
            ViewBag.oldcaseper = Math.Round((ViewBag.oldcase) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.unkcase = db.fnl_alysis.Where(x => x.System3 == "Colon").Where(x => x.PRVTMTYN == "U").Count();
            ViewBag.unkcaseper = Math.Round((ViewBag.unkcase) * 100.0 / (ViewBag.TotalCount), 1);

            ViewBag.NewTretedTmh = db.fnl_alysis.Where(x => x.System3 == "Colon").Where(x => x.PRVTMTYN == "N" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.NewTretedTmhper = Math.Round((ViewBag.NewTretedTmh) * 100.0 / (ViewBag.newcase), 1);
            ViewBag.oldtreatedtmh = db.fnl_alysis.Where(x => x.System3 == "Colon").Where(x => x.PRVTMTYN == "Y" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.oldtreatedtmhper = Math.Round((ViewBag.oldtreatedtmh) * 100.0 / (ViewBag.oldcase), 1);
            ViewBag.untreatedtmh = db.fnl_alysis.Where(x => x.System3 == "Colon").Where(x => x.PRVTMTYN == "U" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.untreatedtmhper = Math.Round((ViewBag.untreatedtmh) * 100.0 / (ViewBag.unkcase), 1);
            if (double.IsNaN(ViewBag.untreatedtmhper))
            {
                ViewBag.untreatedtmhper = 0;
            }
            ViewBag.totalcase = (ViewBag.newcase + ViewBag.oldcase + ViewBag.unkcase);
            ViewBag.totalcaseper = (ViewBag.newcaseper + ViewBag.oldcaseper + ViewBag.unkcaseper);
            ViewBag.tottreatmh = (ViewBag.NewTretedTmh + ViewBag.oldtreatedtmh + ViewBag.untreatedtmh);
            ViewBag.tottreatmhper = Math.Round((ViewBag.tottreatmh) * 100.0 / (ViewBag.totalcase), 1);


            ViewBag.agedata = db.fnl_alysis.Where(x => x.System3 == "Colon").GroupBy(x => x.BROAD_AGE).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System3 == "Colon").Select(x => x.BROAD_AGE).Count(), 1) }).ToList();
            var objj = db.fnl_alysis.Where(x => x.System3 == "Colon").GroupBy(x => x.final_tti).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System3 == "Colon").Select(x => x.final_tti).Count(), 1) }).ToList();
            ViewBag.timedata = objj.Where(x => x.Key != null);

            ViewBag.System3 = "Colon";
            return PartialView("_Colon", list);

        }

        [HttpGet]
        public PartialViewResult Rectum()
        {

            var data = db.fnl_alysis.Where(x => x.System3 == "Rectum").GroupBy(x => x.N_CLINEXT).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System3 == "Rectum").Select(x => x.N_CLINEXT).Count(), 1) }).OrderBy(i => i.Key == "Not Known").ThenBy(i => i.Key == "Treated outside").ThenBy(i => i.Key == "Too Advanced").ThenBy(i => i.Key == "Dist. Mets").ThenBy(i => i.Key == "Regional").ThenBy(i => i.Key == "Localised").ToList();

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
            ViewBag.malecount = db.fnl_alysis.Where(x => x.SEX == "M" && x.System3 == "Rectum").Count();
            ViewBag.femalecount = db.fnl_alysis.Where(x => x.SEX == "F" && x.System3 == "Rectum").Count();

            var obj = db.fnl_alysis.Where(x => x.System3 == "Rectum").GroupBy(x => x.TMH_RX).Select(y => new { y.Key, Count = y.Count() }).ToList();

            ViewBag.objectname = obj.Where(x => x.Key != null);
            var list = db.fnl_alysis.Where(x => x.System3 == "Rectum").GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.System3 == "Rectum").Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Take(5);
            var list1 = db.fnl_alysis.Where(x => x.System3 == "Rectum").GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.System3 == "Rectum").Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Skip(5);

            ViewBag.other = list1.Sum(y => y.Statecount);
            ViewBag.Otherper = Math.Round(list1.Sum(x => x.Statecount) * 100 / db.fnl_alysis.Where(x => x.System3 == "Rectum").Select(x => x.STATE).Count(), 1);
            // ViewBag.totalper = db.fnl_alysis.Where(x => x.System4 == sys4 ).Select(x => x.STATE).Count() * 100 / db.fnl_alysis.Where(x => x.System4 == sys4 ).Select(x => x.STATE).Count();
            ViewBag.TotalCount = db.fnl_alysis.Where(x => x.System3 == "Rectum").Select(x => x.STATE).Count();

            ViewBag.newcase = db.fnl_alysis.Where(x => x.System3 == "Rectum").Where(x => x.PRVTMTYN == "N").Count();
            ViewBag.newcaseper = Math.Round((ViewBag.newcase) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.oldcase = db.fnl_alysis.Where(x => x.System3 == "Rectum").Where(x => x.PRVTMTYN == "Y").Count();
            ViewBag.oldcaseper = Math.Round((ViewBag.oldcase) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.unkcase = db.fnl_alysis.Where(x => x.System3 == "Rectum").Where(x => x.PRVTMTYN == "U").Count();
            ViewBag.unkcaseper = Math.Round((ViewBag.unkcase) * 100.0 / (ViewBag.TotalCount), 1);

            ViewBag.NewTretedTmh = db.fnl_alysis.Where(x => x.System3 == "Rectum").Where(x => x.PRVTMTYN == "N" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.NewTretedTmhper = Math.Round((ViewBag.NewTretedTmh) * 100.0 / (ViewBag.newcase), 1);
            ViewBag.oldtreatedtmh = db.fnl_alysis.Where(x => x.System3 == "Rectum").Where(x => x.PRVTMTYN == "Y" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.oldtreatedtmhper = Math.Round((ViewBag.oldtreatedtmh) * 100.0 / (ViewBag.oldcase), 1);
            ViewBag.untreatedtmh = db.fnl_alysis.Where(x => x.System3 == "Rectum").Where(x => x.PRVTMTYN == "U" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.untreatedtmhper = Math.Round((ViewBag.untreatedtmh) * 100.0 / (ViewBag.unkcase), 1);
            if (double.IsNaN(ViewBag.untreatedtmhper))
            {
                ViewBag.untreatedtmhper = 0;
            }
            ViewBag.totalcase = (ViewBag.newcase + ViewBag.oldcase + ViewBag.unkcase);
            ViewBag.totalcaseper = (ViewBag.newcaseper + ViewBag.oldcaseper + ViewBag.unkcaseper);
            ViewBag.tottreatmh = (ViewBag.NewTretedTmh + ViewBag.oldtreatedtmh + ViewBag.untreatedtmh);
            ViewBag.tottreatmhper = Math.Round((ViewBag.tottreatmh) * 100.0 / (ViewBag.totalcase), 1);


            ViewBag.agedata = db.fnl_alysis.Where(x => x.System3 == "Rectum").GroupBy(x => x.BROAD_AGE).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System3 == "Rectum").Select(x => x.BROAD_AGE).Count(), 1) }).ToList();
            var objj = db.fnl_alysis.Where(x => x.System3 == "Rectum").GroupBy(x => x.final_tti).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System3 == "Rectum").Select(x => x.final_tti).Count(), 1) }).ToList();
            ViewBag.timedata = objj.Where(x => x.Key != null);

            ViewBag.System3 = "Rectum";
            return PartialView("_Rectum", list);

        }

        [HttpGet]
        public PartialViewResult Liver()
        {

            var data = db.fnl_alysis.Where(x => x.System3 == "Liver").GroupBy(x => x.N_CLINEXT).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System3 == "Liver").Select(x => x.N_CLINEXT).Count(), 1) }).OrderBy(i => i.Key == "Not Known").ThenBy(i => i.Key == "Treated outside").ThenBy(i => i.Key == "Too Advanced").ThenBy(i => i.Key == "Dist. Mets").ThenBy(i => i.Key == "Regional").ThenBy(i => i.Key == "Localised").ToList();

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
            ViewBag.malecount = db.fnl_alysis.Where(x => x.SEX == "M" && x.System3 == "Liver").Count();
            ViewBag.femalecount = db.fnl_alysis.Where(x => x.SEX == "F" && x.System3 == "Liver").Count();

            var obj = db.fnl_alysis.Where(x => x.System3 == "Liver").GroupBy(x => x.TMH_RX).Select(y => new { y.Key, Count = y.Count() }).ToList();

            ViewBag.objectname = obj.Where(x => x.Key != null);
            var list = db.fnl_alysis.Where(x => x.System3 == "Liver").GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.System3 == "Liver").Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Take(5);
            var list1 = db.fnl_alysis.Where(x => x.System3 == "Liver").GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.System3 == "Liver").Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Skip(5);

            ViewBag.other = list1.Sum(y => y.Statecount);
            ViewBag.Otherper = Math.Round(list1.Sum(x => x.Statecount) * 100 / db.fnl_alysis.Where(x => x.System3 == "Liver").Select(x => x.STATE).Count(), 1);
            // ViewBag.totalper = db.fnl_alysis.Where(x => x.System4 == sys4 ).Select(x => x.STATE).Count() * 100 / db.fnl_alysis.Where(x => x.System4 == sys4 ).Select(x => x.STATE).Count();
            ViewBag.TotalCount = db.fnl_alysis.Where(x => x.System3 == "Liver").Select(x => x.STATE).Count();

            ViewBag.newcase = db.fnl_alysis.Where(x => x.System3 == "Liver").Where(x => x.PRVTMTYN == "N").Count();
            ViewBag.newcaseper = Math.Round((ViewBag.newcase) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.oldcase = db.fnl_alysis.Where(x => x.System3 == "Liver").Where(x => x.PRVTMTYN == "Y").Count();
            ViewBag.oldcaseper = Math.Round((ViewBag.oldcase) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.unkcase = db.fnl_alysis.Where(x => x.System3 == "Liver").Where(x => x.PRVTMTYN == "U").Count();
            ViewBag.unkcaseper = Math.Round((ViewBag.unkcase) * 100.0 / (ViewBag.TotalCount), 1);

            ViewBag.NewTretedTmh = db.fnl_alysis.Where(x => x.System3 == "Liver").Where(x => x.PRVTMTYN == "N" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.NewTretedTmhper = Math.Round((ViewBag.NewTretedTmh) * 100.0 / (ViewBag.newcase), 1);
            ViewBag.oldtreatedtmh = db.fnl_alysis.Where(x => x.System3 == "Liver").Where(x => x.PRVTMTYN == "Y" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.oldtreatedtmhper = Math.Round((ViewBag.oldtreatedtmh) * 100.0 / (ViewBag.oldcase), 1);
            ViewBag.untreatedtmh = db.fnl_alysis.Where(x => x.System3 == "Liver").Where(x => x.PRVTMTYN == "U" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.untreatedtmhper = Math.Round((ViewBag.untreatedtmh) * 100.0 / (ViewBag.unkcase), 1);
            if (double.IsNaN(ViewBag.untreatedtmhper))
            {
                ViewBag.untreatedtmhper = 0;
            }
            ViewBag.totalcase = (ViewBag.newcase + ViewBag.oldcase + ViewBag.unkcase);
            ViewBag.totalcaseper = (ViewBag.newcaseper + ViewBag.oldcaseper + ViewBag.unkcaseper);
            ViewBag.tottreatmh = (ViewBag.NewTretedTmh + ViewBag.oldtreatedtmh + ViewBag.untreatedtmh);
            ViewBag.tottreatmhper = Math.Round((ViewBag.tottreatmh) * 100.0 / (ViewBag.totalcase), 1);


            ViewBag.agedata = db.fnl_alysis.Where(x => x.System3 == "Liver").GroupBy(x => x.BROAD_AGE).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System3 == "Liver").Select(x => x.BROAD_AGE).Count(), 1) }).ToList();
            var objj = db.fnl_alysis.Where(x => x.System3 == "Liver").GroupBy(x => x.final_tti).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System3 == "Liver").Select(x => x.final_tti).Count(), 1) }).ToList();
            ViewBag.timedata = objj.Where(x => x.Key != null);

            ViewBag.System3 = "Liver";
            return PartialView("_Liver", list);
        }

        [HttpGet]
        public PartialViewResult Gall_Bladder()
        {

            var data = db.fnl_alysis.Where(x => x.System3 == "Gall Bladder").GroupBy(x => x.N_CLINEXT).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System3 == "Gall Bladder").Select(x => x.N_CLINEXT).Count(), 1) }).OrderBy(i => i.Key == "Not Known").ThenBy(i => i.Key == "Treated outside").ThenBy(i => i.Key == "Too Advanced").ThenBy(i => i.Key == "Dist. Mets").ThenBy(i => i.Key == "Regional").ThenBy(i => i.Key == "Localised").ToList();

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
            ViewBag.malecount = db.fnl_alysis.Where(x => x.SEX == "M" && x.System3 == "Gall Bladder").Count();
            ViewBag.femalecount = db.fnl_alysis.Where(x => x.SEX == "F" && x.System3 == "Gall Bladder").Count();

            var obj = db.fnl_alysis.Where(x => x.System3 == "Gall Bladder").GroupBy(x => x.TMH_RX).Select(y => new { y.Key, Count = y.Count() }).ToList();

            ViewBag.objectname = obj.Where(x => x.Key != null);
            var list = db.fnl_alysis.Where(x => x.System3 == "Gall Bladder").GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.System3 == "Gall Bladder").Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Take(5);
            var list1 = db.fnl_alysis.Where(x => x.System3 == "Gall Bladder").GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.System3 == "Gall Bladder").Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Skip(5);

            ViewBag.other = list1.Sum(y => y.Statecount);
            ViewBag.Otherper = Math.Round(list1.Sum(x => x.Statecount) * 100 / db.fnl_alysis.Where(x => x.System3 == "Gall Bladder").Select(x => x.STATE).Count(), 1);
            // ViewBag.totalper = db.fnl_alysis.Where(x => x.System4 == sys4 ).Select(x => x.STATE).Count() * 100 / db.fnl_alysis.Where(x => x.System4 == sys4 ).Select(x => x.STATE).Count();
            ViewBag.TotalCount = db.fnl_alysis.Where(x => x.System3 == "Gall Bladder").Select(x => x.STATE).Count();

            ViewBag.newcase = db.fnl_alysis.Where(x => x.System3 == "Gall Bladder").Where(x => x.PRVTMTYN == "N").Count();
            ViewBag.newcaseper = Math.Round((ViewBag.newcase) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.oldcase = db.fnl_alysis.Where(x => x.System3 == "Gall Bladder").Where(x => x.PRVTMTYN == "Y").Count();
            ViewBag.oldcaseper = Math.Round((ViewBag.oldcase) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.unkcase = db.fnl_alysis.Where(x => x.System3 == "Gall Bladder").Where(x => x.PRVTMTYN == "U").Count();
            ViewBag.unkcaseper = Math.Round((ViewBag.unkcase) * 100.0 / (ViewBag.TotalCount), 1);

            ViewBag.NewTretedTmh = db.fnl_alysis.Where(x => x.System3 == "Gall Bladder").Where(x => x.PRVTMTYN == "N" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.NewTretedTmhper = Math.Round((ViewBag.NewTretedTmh) * 100.0 / (ViewBag.newcase), 1);
            ViewBag.oldtreatedtmh = db.fnl_alysis.Where(x => x.System3 == "Gall Bladder").Where(x => x.PRVTMTYN == "Y" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.oldtreatedtmhper = Math.Round((ViewBag.oldtreatedtmh) * 100.0 / (ViewBag.oldcase), 1);
            ViewBag.untreatedtmh = db.fnl_alysis.Where(x => x.System3 == "Gall Bladder").Where(x => x.PRVTMTYN == "U" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.untreatedtmhper = Math.Round((ViewBag.untreatedtmh) * 100.0 / (ViewBag.unkcase), 1);
            if (double.IsNaN(ViewBag.untreatedtmhper))
            {
                ViewBag.untreatedtmhper = 0;
            }
            ViewBag.totalcase = (ViewBag.newcase + ViewBag.oldcase + ViewBag.unkcase);
            ViewBag.totalcaseper = (ViewBag.newcaseper + ViewBag.oldcaseper + ViewBag.unkcaseper);
            ViewBag.tottreatmh = (ViewBag.NewTretedTmh + ViewBag.oldtreatedtmh + ViewBag.untreatedtmh);
            ViewBag.tottreatmhper = Math.Round((ViewBag.tottreatmh) * 100.0 / (ViewBag.totalcase), 1);


            ViewBag.agedata = db.fnl_alysis.Where(x => x.System3 == "Gall Bladder").GroupBy(x => x.BROAD_AGE).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System3 == "Gall Bladder").Select(x => x.BROAD_AGE).Count(), 1) }).ToList();
            var objj = db.fnl_alysis.Where(x => x.System3 == "Gall Bladder").GroupBy(x => x.final_tti).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System3 == "Gall Bladder").Select(x => x.final_tti).Count(), 1) }).ToList();
            ViewBag.timedata = objj.Where(x => x.Key != null);

            ViewBag.System3 = "Gall Bladder";
            return PartialView("_GallBladder", list);
        }

        [HttpGet]
        public PartialViewResult Pancreas()
        {

            var data = db.fnl_alysis.Where(x => x.System3 == "Pancreas").GroupBy(x => x.N_CLINEXT).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System3 == "Pancreas").Select(x => x.N_CLINEXT).Count(), 1) }).OrderBy(i => i.Key == "Not Known").ThenBy(i => i.Key == "Treated outside").ThenBy(i => i.Key == "Too Advanced").ThenBy(i => i.Key == "Dist. Mets").ThenBy(i => i.Key == "Regional").ThenBy(i => i.Key == "Localised").ToList();

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
            ViewBag.malecount = db.fnl_alysis.Where(x => x.SEX == "M" && x.System3 == "Pancreas").Count();
            ViewBag.femalecount = db.fnl_alysis.Where(x => x.SEX == "F" && x.System3 == "Pancreas").Count();

            var obj = db.fnl_alysis.Where(x => x.System3 == "Pancreas").GroupBy(x => x.TMH_RX).Select(y => new { y.Key, Count = y.Count() }).ToList();

            ViewBag.objectname = obj.Where(x => x.Key != null);
            var list = db.fnl_alysis.Where(x => x.System3 == "Pancreas").GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.System3 == "Pancreas").Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Take(5);
            var list1 = db.fnl_alysis.Where(x => x.System3 == "Pancreas").GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.System3 == "Pancreas").Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Skip(5);

            ViewBag.other = list1.Sum(y => y.Statecount);
            ViewBag.Otherper = Math.Round(list1.Sum(x => x.Statecount) * 100 / db.fnl_alysis.Where(x => x.System3 == "Pancreas").Select(x => x.STATE).Count(), 1);
            // ViewBag.totalper = db.fnl_alysis.Where(x => x.System4 == sys4 ).Select(x => x.STATE).Count() * 100 / db.fnl_alysis.Where(x => x.System4 == sys4 ).Select(x => x.STATE).Count();
            ViewBag.TotalCount = db.fnl_alysis.Where(x => x.System3 == "Pancreas").Select(x => x.STATE).Count();

            ViewBag.newcase = db.fnl_alysis.Where(x => x.System3 == "Pancreas").Where(x => x.PRVTMTYN == "N").Count();
            ViewBag.newcaseper = Math.Round((ViewBag.newcase) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.oldcase = db.fnl_alysis.Where(x => x.System3 == "Pancreas").Where(x => x.PRVTMTYN == "Y").Count();
            ViewBag.oldcaseper = Math.Round((ViewBag.oldcase) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.unkcase = db.fnl_alysis.Where(x => x.System3 == "Pancreas").Where(x => x.PRVTMTYN == "U").Count();
            ViewBag.unkcaseper = Math.Round((ViewBag.unkcase) * 100.0 / (ViewBag.TotalCount), 1);

            ViewBag.NewTretedTmh = db.fnl_alysis.Where(x => x.System3 == "Pancreas").Where(x => x.PRVTMTYN == "N" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.NewTretedTmhper = Math.Round((ViewBag.NewTretedTmh) * 100.0 / (ViewBag.newcase), 1);
            ViewBag.oldtreatedtmh = db.fnl_alysis.Where(x => x.System3 == "Pancreas").Where(x => x.PRVTMTYN == "Y" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.oldtreatedtmhper = Math.Round((ViewBag.oldtreatedtmh) * 100.0 / (ViewBag.oldcase), 1);
            ViewBag.untreatedtmh = db.fnl_alysis.Where(x => x.System3 == "Pancreas").Where(x => x.PRVTMTYN == "U" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.untreatedtmhper = Math.Round((ViewBag.untreatedtmh) * 100.0 / (ViewBag.unkcase), 1);
            if (double.IsNaN(ViewBag.untreatedtmhper))
            {
                ViewBag.untreatedtmhper = 0;
            }
            ViewBag.totalcase = (ViewBag.newcase + ViewBag.oldcase + ViewBag.unkcase);
            ViewBag.totalcaseper = (ViewBag.newcaseper + ViewBag.oldcaseper + ViewBag.unkcaseper);
            ViewBag.tottreatmh = (ViewBag.NewTretedTmh + ViewBag.oldtreatedtmh + ViewBag.untreatedtmh);
            ViewBag.tottreatmhper = Math.Round((ViewBag.tottreatmh) * 100.0 / (ViewBag.totalcase), 1);


            ViewBag.agedata = db.fnl_alysis.Where(x => x.System3 == "Pancreas").GroupBy(x => x.BROAD_AGE).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System3 == "Pancreas").Select(x => x.BROAD_AGE).Count(), 1) }).ToList();
            var objj = db.fnl_alysis.Where(x => x.System3 == "Pancreas").GroupBy(x => x.final_tti).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System3 == "Pancreas").Select(x => x.final_tti).Count(), 1) }).ToList();
            ViewBag.timedata = objj.Where(x => x.Key != null);

            ViewBag.System3 = "Pancreas";
            return PartialView("_Pancreas", list);
        }


        [HttpGet]
        public PartialViewResult Female_Breast()
        {
            var data = db.fnl_alysis.Where(x => x.System2 == "Female Breast").GroupBy(x => x.N_CLINEXT).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System2 == "Female Breast").Select(x => x.N_CLINEXT).Count(), 1) }).OrderBy(i => i.Key == "Not Known").ThenBy(i => i.Key == "Treated outside").ThenBy(i => i.Key == "Too Advanced").ThenBy(i => i.Key == "Dist. Mets").ThenBy(i => i.Key == "Regional").ThenBy(i => i.Key == "Localised").ToList();
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


            // ViewBag.malecount = db.fnl_alysis.Where(x => x.SEX == "1M" && x.System2 == "Female Breast").Count();
            //  ViewBag.femalecount = db.fnl_alysis.Where(x => x.SEX == "2F" && x.System2 == "Female Breast").Count();

            var obj = db.fnl_alysis.Where(x => x.System2 == "Female Breast").GroupBy(x => x.TMH_RX).Select(y => new { y.Key, Count = y.Count() }).ToList();

            ViewBag.objectname = obj.Where(x => x.Key != null);
            var list = db.fnl_alysis.Where(x => x.System2 == "Female Breast").GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.System2 == "Female Breast").Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Take(5);
            var list1 = db.fnl_alysis.Where(x => x.System2 == "Female Breast").GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.System2 == "Female Breast").Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Skip(5);

            ViewBag.other = list1.Sum(y => y.Statecount);
            ViewBag.Otherper = Math.Round(list1.Sum(x => x.Statecount) * 100 / db.fnl_alysis.Where(x => x.System2 == "Female Breast").Select(x => x.STATE).Count(), 1);
            // ViewBag.totalper = db.fnl_alysis.Where(x => x.System4 == sys4 ).Select(x => x.STATE).Count() * 100 / db.fnl_alysis.Where(x => x.System4 == sys4 ).Select(x => x.STATE).Count();
            ViewBag.TotalCount = db.fnl_alysis.Where(x => x.System2 == "Female Breast").Select(x => x.STATE).Count();

            ViewBag.newcase = db.fnl_alysis.Where(x => x.System2 == "Female Breast").Where(x => x.PRVTMTYN == "N").Count();
            ViewBag.newcaseper = Math.Round((ViewBag.newcase) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.oldcase = db.fnl_alysis.Where(x => x.System2 == "Female Breast").Where(x => x.PRVTMTYN == "Y").Count();
            ViewBag.oldcaseper = Math.Round((ViewBag.oldcase) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.unkcase = db.fnl_alysis.Where(x => x.System2 == "Female Breast").Where(x => x.PRVTMTYN == "U").Count();
            ViewBag.unkcaseper = Math.Round((ViewBag.unkcase) * 100.0 / (ViewBag.TotalCount), 1);

            ViewBag.NewTretedTmh = db.fnl_alysis.Where(x => x.System2 == "Female Breast").Where(x => x.PRVTMTYN == "N" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.NewTretedTmhper = Math.Round((ViewBag.NewTretedTmh) * 100.0 / (ViewBag.newcase), 1);
            ViewBag.oldtreatedtmh = db.fnl_alysis.Where(x => x.System2 == "Female Breast").Where(x => x.PRVTMTYN == "Y" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.oldtreatedtmhper = Math.Round((ViewBag.oldtreatedtmh) * 100.0 / (ViewBag.oldcase), 1);
            ViewBag.untreatedtmh = db.fnl_alysis.Where(x => x.System2 == "Female Breast").Where(x => x.PRVTMTYN == "U" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.untreatedtmhper = Math.Round((ViewBag.untreatedtmh) * 100.0 / (ViewBag.unkcase), 1);
            if (double.IsNaN(ViewBag.untreatedtmhper))
            {
                ViewBag.untreatedtmhper = 0;
            }
            ViewBag.totalcase = (ViewBag.newcase + ViewBag.oldcase + ViewBag.unkcase);
            ViewBag.totalcaseper = (ViewBag.newcaseper + ViewBag.oldcaseper + ViewBag.unkcaseper);
            ViewBag.tottreatmh = (ViewBag.NewTretedTmh + ViewBag.oldtreatedtmh + ViewBag.untreatedtmh);
            ViewBag.tottreatmhper = Math.Round((ViewBag.tottreatmh) * 100.0 / (ViewBag.totalcase), 1);


            ViewBag.agedata = db.fnl_alysis.Where(x => x.System2 == "Female Breast").GroupBy(x => x.BROAD_AGE).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System2 == "Female Breast").Select(x => x.BROAD_AGE).Count(), 1) }).OrderBy(i => i.Key == ">=60").ThenBy(i => i.Key == "46-59").ThenBy(i => i.Key == "20-45").ThenBy(i => i.Key == "0-19").ToList();
            var objj = db.fnl_alysis.Where(x => x.System2 == "Female Breast" && x.final_tti != null).GroupBy(x => x.final_tti).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System2 == "Female Breast" && x.final_tti != null).Select(x => x.final_tti).Count(), 1) }).OrderBy(i => i.Key == ">180 days").ThenBy(i => i.Key == "91-180 days").ThenBy(i => i.Key == "31-90 days").ThenBy(i => i.Key == "<=30 days").ToList();

            ViewBag.timedata = objj.Where(x => x.Key != null);

            ViewBag.System2 = "Female Breast Cancer";
            return PartialView("_Female_Breast", list);
        }    //main

        [HttpGet]
        public PartialViewResult Bone()
        {

            var data = db.fnl_alysis.Where(x => x.System3 == "Bone").GroupBy(x => x.N_CLINEXT).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System3 == "Bone").Select(x => x.N_CLINEXT).Count(), 1) }).OrderBy(i => i.Key == "Not Known").ThenBy(i => i.Key == "Treated outside").ThenBy(i => i.Key == "Too Advanced").ThenBy(i => i.Key == "Dist. Mets").ThenBy(i => i.Key == "Regional").ThenBy(i => i.Key == "Localised").ToList();

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
            ViewBag.malecount = db.fnl_alysis.Where(x => x.SEX == "M" && x.System3 == "Bone").Count();
            ViewBag.femalecount = db.fnl_alysis.Where(x => x.SEX == "F" && x.System3 == "Bone").Count();

            var obj = db.fnl_alysis.Where(x => x.System3 == "Bone").GroupBy(x => x.TMH_RX).Select(y => new { y.Key, Count = y.Count() }).ToList();

            ViewBag.objectname = obj.Where(x => x.Key != null);
            var list = db.fnl_alysis.Where(x => x.System3 == "Bone").GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.System3 == "Bone").Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Take(5);
            var list1 = db.fnl_alysis.Where(x => x.System3 == "Bone").GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.System3 == "Bone").Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Skip(5);

            ViewBag.other = list1.Sum(y => y.Statecount);
            ViewBag.Otherper = Math.Round(list1.Sum(x => x.Statecount) * 100 / db.fnl_alysis.Where(x => x.System3 == "Bone").Select(x => x.STATE).Count(), 1);
            // ViewBag.totalper = db.fnl_alysis.Where(x => x.System4 == sys4 ).Select(x => x.STATE).Count() * 100 / db.fnl_alysis.Where(x => x.System4 == sys4 ).Select(x => x.STATE).Count();
            ViewBag.TotalCount = db.fnl_alysis.Where(x => x.System3 == "Bone").Select(x => x.STATE).Count();

            ViewBag.newcase = db.fnl_alysis.Where(x => x.System3 == "Bone").Where(x => x.PRVTMTYN == "N").Count();
            ViewBag.newcaseper = Math.Round((ViewBag.newcase) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.oldcase = db.fnl_alysis.Where(x => x.System3 == "Bone").Where(x => x.PRVTMTYN == "Y").Count();
            ViewBag.oldcaseper = Math.Round((ViewBag.oldcase) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.unkcase = db.fnl_alysis.Where(x => x.System3 == "Bone").Where(x => x.PRVTMTYN == "U").Count();
            ViewBag.unkcaseper = Math.Round((ViewBag.unkcase) * 100.0 / (ViewBag.TotalCount), 1);

            ViewBag.NewTretedTmh = db.fnl_alysis.Where(x => x.System3 == "Bone").Where(x => x.PRVTMTYN == "N" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.NewTretedTmhper = Math.Round((ViewBag.NewTretedTmh) * 100.0 / (ViewBag.newcase), 1);
            ViewBag.oldtreatedtmh = db.fnl_alysis.Where(x => x.System3 == "Bone").Where(x => x.PRVTMTYN == "Y" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.oldtreatedtmhper = Math.Round((ViewBag.oldtreatedtmh) * 100.0 / (ViewBag.oldcase), 1);
            ViewBag.untreatedtmh = db.fnl_alysis.Where(x => x.System3 == "Bone").Where(x => x.PRVTMTYN == "U" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.untreatedtmhper = Math.Round((ViewBag.untreatedtmh) * 100.0 / (ViewBag.unkcase), 1);
            if (double.IsNaN(ViewBag.untreatedtmhper))
            {
                ViewBag.untreatedtmhper = 0;
            }
            ViewBag.totalcase = (ViewBag.newcase + ViewBag.oldcase + ViewBag.unkcase);
            ViewBag.totalcaseper = (ViewBag.newcaseper + ViewBag.oldcaseper + ViewBag.unkcaseper);
            ViewBag.tottreatmh = (ViewBag.NewTretedTmh + ViewBag.oldtreatedtmh + ViewBag.untreatedtmh);
            ViewBag.tottreatmhper = Math.Round((ViewBag.tottreatmh) * 100.0 / (ViewBag.totalcase), 1);


            ViewBag.agedata = db.fnl_alysis.Where(x => x.System3 == "Bone").GroupBy(x => x.BROAD_AGE).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System3 == "Bone").Select(x => x.BROAD_AGE).Count(), 1) }).OrderBy(i => i.Key == ">=60").ThenBy(i => i.Key == "46-59").ThenBy(i => i.Key == "20-45").ThenBy(i => i.Key == "0-19").ToList();

            var objj = db.fnl_alysis.Where(x => x.System3 == "Bone" && x.final_tti != null).GroupBy(x => x.final_tti).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System3 == "Bone" && x.final_tti != null).Select(x => x.final_tti).Count(), 1) }).OrderBy(i => i.Key == ">180 days").ThenBy(i => i.Key == "91-180 days").ThenBy(i => i.Key == "31-90 days").ThenBy(i => i.Key == "<=30 days").ToList();
            ViewBag.timedata = objj.Where(x => x.Key != null);

            ViewBag.System3 = "Bone";
            return PartialView("_Bone", list);
        }         //main


        [HttpGet]
        public PartialViewResult Peripheral_Nerves_Soft_Tissue()
        {

            var data = db.fnl_alysis.Where(x => x.System3 == "Peripheral Nerves & Soft Tissue").GroupBy(x => x.N_CLINEXT).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System3 == "Peripheral Nerves & Soft Tissue").Select(x => x.N_CLINEXT).Count(), 1) }).OrderBy(i => i.Key == "Not Known").ThenBy(i => i.Key == "Treated outside").ThenBy(i => i.Key == "Too Advanced").ThenBy(i => i.Key == "Dist. Mets").ThenBy(i => i.Key == "Regional").ThenBy(i => i.Key == "Localised").ToList();

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
            ViewBag.malecount = db.fnl_alysis.Where(x => x.SEX == "M" && x.System3 == "Peripheral Nerves & Soft Tissue").Count();
            ViewBag.femalecount = db.fnl_alysis.Where(x => x.SEX == "F" && x.System3 == "Peripheral Nerves & Soft Tissue").Count();

            var obj = db.fnl_alysis.Where(x => x.System3 == "Peripheral Nerves & Soft Tissue").GroupBy(x => x.TMH_RX).Select(y => new { y.Key, Count = y.Count() }).ToList();

            ViewBag.objectname = obj.Where(x => x.Key != null);
            var list = db.fnl_alysis.Where(x => x.System3 == "Peripheral Nerves & Soft Tissue").GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.System3 == "Peripheral Nerves & Soft Tissue").Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Take(5);
            var list1 = db.fnl_alysis.Where(x => x.System3 == "Peripheral Nerves & Soft Tissue").GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.System3 == "Peripheral Nerves & Soft Tissue").Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Skip(5);

            ViewBag.other = list1.Sum(y => y.Statecount);
            ViewBag.Otherper = Math.Round(list1.Sum(x => x.Statecount) * 100 / db.fnl_alysis.Where(x => x.System3 == "Peripheral Nerves & Soft Tissue").Select(x => x.STATE).Count(), 1);
            // ViewBag.totalper = db.fnl_alysis.Where(x => x.System4 == sys4 ).Select(x => x.STATE).Count() * 100 / db.fnl_alysis.Where(x => x.System4 == sys4 ).Select(x => x.STATE).Count();
            ViewBag.TotalCount = db.fnl_alysis.Where(x => x.System3 == "Peripheral Nerves & Soft Tissue").Select(x => x.STATE).Count();

            ViewBag.newcase = db.fnl_alysis.Where(x => x.System3 == "Peripheral Nerves & Soft Tissue").Where(x => x.PRVTMTYN == "N").Count();
            ViewBag.newcaseper = Math.Round((ViewBag.newcase) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.oldcase = db.fnl_alysis.Where(x => x.System3 == "Peripheral Nerves & Soft Tissue").Where(x => x.PRVTMTYN == "Y").Count();
            ViewBag.oldcaseper = Math.Round((ViewBag.oldcase) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.unkcase = db.fnl_alysis.Where(x => x.System3 == "Peripheral Nerves & Soft Tissue").Where(x => x.PRVTMTYN == "U").Count();
            ViewBag.unkcaseper = Math.Round((ViewBag.unkcase) * 100.0 / (ViewBag.TotalCount), 1);

            ViewBag.NewTretedTmh = db.fnl_alysis.Where(x => x.System3 == "Peripheral Nerves & Soft Tissue").Where(x => x.PRVTMTYN == "N" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.NewTretedTmhper = Math.Round((ViewBag.NewTretedTmh) * 100.0 / (ViewBag.newcase), 1);
            ViewBag.oldtreatedtmh = db.fnl_alysis.Where(x => x.System3 == "Peripheral Nerves & Soft Tissue").Where(x => x.PRVTMTYN == "Y" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.oldtreatedtmhper = Math.Round((ViewBag.oldtreatedtmh) * 100.0 / (ViewBag.oldcase), 1);
            ViewBag.untreatedtmh = db.fnl_alysis.Where(x => x.System3 == "Peripheral Nerves & Soft Tissue").Where(x => x.PRVTMTYN == "U" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.untreatedtmhper = Math.Round((ViewBag.untreatedtmh) * 100.0 / (ViewBag.unkcase), 1);
            if (double.IsNaN(ViewBag.untreatedtmhper))
            {
                ViewBag.untreatedtmhper = 0;
            }
            ViewBag.totalcase = (ViewBag.newcase + ViewBag.oldcase + ViewBag.unkcase);
            ViewBag.totalcaseper = (ViewBag.newcaseper + ViewBag.oldcaseper + ViewBag.unkcaseper);
            ViewBag.tottreatmh = (ViewBag.NewTretedTmh + ViewBag.oldtreatedtmh + ViewBag.untreatedtmh);
            ViewBag.tottreatmhper = Math.Round((ViewBag.tottreatmh) * 100.0 / (ViewBag.totalcase), 1);


            ViewBag.agedata = db.fnl_alysis.Where(x => x.System3 == "Peripheral Nerves & Soft Tissue").GroupBy(x => x.BROAD_AGE).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System3 == "Peripheral Nerves & Soft Tissue").Select(x => x.BROAD_AGE).Count(), 1) }).OrderBy(i => i.Key == ">=60").ThenBy(i => i.Key == "46-59").ThenBy(i => i.Key == "20-45").ThenBy(i => i.Key == "0-19").ToList();

            var objj = db.fnl_alysis.Where(x => x.System3 == "Peripheral Nerves & Soft Tissue" && x.final_tti != null).GroupBy(x => x.final_tti).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System3 == "Peripheral Nerves & Soft Tissue" && x.final_tti != null).Select(x => x.final_tti).Count(), 1) }).OrderBy(i => i.Key == ">180 days").ThenBy(i => i.Key == "91-180 days").ThenBy(i => i.Key == "31-90 days").ThenBy(i => i.Key == "<=30 days").ToList();
            ViewBag.timedata = objj.Where(x => x.Key != null);

            ViewBag.System3 = "Peripheral Nerves & Autonomic Nervous System and Soft Tissue";
            return PartialView("_Peripheral_Nerves_Soft_Tissue", list);
        }         //main


        public ActionResult Genito_UrinarySystem()
        {
            var system3Values = db.fnl_alysis
                .Where(x => x.System4 == "Genito Urinary Organs")
                .GroupBy(x => x.System3)
                .Select(g => new System3StatsViewModel
                {
                    System3 = g.Key,
                    MaleCount = g.Count(x => x.SEX == "M"),
                    FemaleCount = g.Count(x => x.SEX == "F")
                })
                .ToList();

            // Compute totals
            int totalMale = system3Values.Sum(x => x.MaleCount);
            int totalFemale = system3Values.Sum(x => x.FemaleCount);
            int totalAll = totalMale + totalFemale;

            // Set percentages in the view model
            foreach (var item in system3Values)
            {
                item.TotalNosCount = item.MaleCount + item.FemaleCount;
                item.MalePercentage = totalMale == 0 ? 0 : (item.MaleCount * 100.0 / totalMale);
                item.FemalePercentage = totalFemale == 0 ? 0 : (item.FemaleCount * 100.0 / totalFemale);
                item.TotalPercentage = totalAll == 0 ? 0 : (item.TotalNosCount * 100.0 / totalAll);
            }

            // Order by TotalPercentage in descending order for both Male and Female
            var maleSites = new List<string> {
        "Other Male Genital Organs", "Penis", "Prostate",  "Testis", "Unspecified Urinary Organs",  
    };
            var femaleSites = new List<string> {
        "Cervix", "Placenta", "Other Female Genital Organs", "Corpus Uteri", "Vulva", "Vagina", "Ovary"
    };
            var genitoSites = new List<string>
            {
 "Kidney","Urinary Bladder","Ureter","Renal Pelvis","Unspecified Urinary Organs"
            };

            var maleRows = system3Values.Where(x => maleSites.Contains(x.System3)).OrderByDescending(x => x.TotalPercentage).ToList();
            var femaleRows = system3Values.Where(x => femaleSites.Contains(x.System3)).OrderByDescending(x => x.TotalPercentage).ToList();
            var genitoRows = system3Values.Where(x => genitoSites.Contains(x.System3)).OrderByDescending(x => x.TotalPercentage).ToList();
            // Pass sorted lists to ViewBag
            ViewBag.System3Values = system3Values;
            ViewBag.MaleRows = maleRows;
            ViewBag.FemaleRows = femaleRows;
            ViewBag.genitoRows = genitoRows;
            ViewBag.TotalMale = totalMale;
            ViewBag.TotalFemale = totalFemale;
            ViewBag.TotalOverall = totalAll;

            return View();
        }







        [HttpGet]
        public PartialViewResult Female_Gynaecological()
        {
            var data = db.fnl_alysis.Where(x => new[] { "C51", "C52", "C53", "C54-C55", "C56", "C57", "C58" }.Contains(x.Sys) && x.System4 == "Genito Urinary Organs")
                .GroupBy(x => x.N_CLINEXT).Select(g => new
                {
                    g.Key,
                    Count = Math.Round(g.Count() * 100.0 / db.fnl_alysis.Count(y => new[]
{ "C51", "C52", "C53", "C54-C55", "C56", "C57", "C58" }.Contains(y.Sys)), 1)
                })
                .OrderBy(x => x.Key == "Not Known").ThenBy(x => x.Key == "Treated outside").ThenBy(x => x.Key == "Too Advanced")
                .ThenBy(x => x.Key == "Dist. Mets").ThenBy(x => x.Key == "Regional").ThenBy(x => x.Key == "Localised").ToList();



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

            ViewBag.malecount = db.fnl_alysis.Where(x => x.SEX == "M" && new[] { "C51", "C52", "C53", "C54-C55", "C56", "C57", "C58" }.Contains(x.Sys)).Count();
            ViewBag.femalecount = db.fnl_alysis.Where(x => x.SEX == "F" && new[] { "C51", "C52", "C53", "C54-C55", "C56", "C57", "C58" }.Contains(x.Sys)).Count();

            var obj = db.fnl_alysis.Where(x => x.System4 == "Genito Urinary Organs" && new[] { "C51", "C52", "C53", "C54-C55", "C56", "C57", "C58" }.Contains(x.Sys)).GroupBy(x => x.TMH_RX).Select(y => new { y.Key, Count = y.Count() }).ToList();

            ViewBag.objectname = obj.Where(x => x.Key != null);
            var list = db.fnl_alysis.Where(x => x.System4 == "Genito Urinary Organs" && new[] { "C51", "C52", "C53", "C54-C55", "C56", "C57", "C58" }.Contains(x.Sys)).GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.System4 == "Genito Urinary Organs" && new[] { "C51", "C52", "C53", "C54-C55", "C56", "C57", "C58" }.Contains(y.Sys)).Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Take(5);
            var list1 = db.fnl_alysis.Where(x => x.System4 == "Genito Urinary Organs" && new[] { "C51", "C52", "C53", "C54-C55", "C56", "C57", "C58" }.Contains(x.Sys)).GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.System4 == "Genito Urinary Organs" && new[] { "C51", "C52", "C53", "C54-C55", "C56", "C57", "C58" }.Contains(y.Sys)).Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Skip(5);

            ViewBag.other = list1.Sum(y => y.Statecount);
            ViewBag.Otherper = Math.Round(list1.Sum(x => x.Statecount) * 100 / db.fnl_alysis.Where(x => x.System4 == "Genito Urinary Organs" && new[] { "C51", "C52", "C53", "C54-C55", "C56", "C57", "C58" }.Contains(x.Sys)).Select(x => x.STATE).Count(), 1);
            ViewBag.TotalCount = db.fnl_alysis.Where(x => x.System4 == "Genito Urinary Organs" && new[] { "C51", "C52", "C53", "C54-C55", "C56", "C57", "C58" }.Contains(x.Sys)).Select(x => x.STATE).Count();


            ViewBag.newcase = db.fnl_alysis.Where(x => x.System4 == "Genito Urinary Organs" && new[] { "C51", "C52", "C53", "C54-C55", "C56", "C57", "C58" }.Contains(x.Sys)).Where(x => x.PRVTMTYN == "N").Count();
            ViewBag.newcaseper = Math.Round((ViewBag.newcase) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.oldcase = db.fnl_alysis.Where(x => x.System4 == "Genito Urinary Organs" && new[] { "C51", "C52", "C53", "C54-C55", "C56", "C57", "C58" }.Contains(x.Sys)).Where(x => x.PRVTMTYN == "Y").Count();
            ViewBag.oldcaseper = Math.Round((ViewBag.oldcase) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.unkcase = db.fnl_alysis.Where(x => x.System4 == "Genito Urinary Organs" && new[] { "C51", "C52", "C53", "C54-C55", "C56", "C57", "C58" }.Contains(x.Sys)).Where(x => x.PRVTMTYN == "U").Count();

            ViewBag.unkcaseper = Math.Round((ViewBag.unkcase) * 100.0 / (ViewBag.TotalCount), 1);

            ViewBag.NewTretedTmh = db.fnl_alysis.Where(x => x.System4 == "Genito Urinary Organs" && new[] { "C51", "C52", "C53", "C54-C55", "C56", "C57", "C58" }.Contains(x.Sys)).Where(x => x.PRVTMTYN == "N" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.NewTretedTmhper = Math.Round((ViewBag.NewTretedTmh) * 100.0 / (ViewBag.newcase), 1);
            ViewBag.oldtreatedtmh = db.fnl_alysis.Where(x => x.System4 == "Genito Urinary Organs" && new[] { "C51", "C52", "C53", "C54-C55", "C56", "C57", "C58" }.Contains(x.Sys)).Where(x => x.PRVTMTYN == "Y" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.oldtreatedtmhper = Math.Round((ViewBag.oldtreatedtmh) * 100.0 / (ViewBag.oldcase), 1);
            ViewBag.untreatedtmh = db.fnl_alysis.Where(x => x.System4 == "Genito Urinary Organs" && new[] { "C51", "C52", "C53", "C54-C55", "C56", "C57", "C58" }.Contains(x.Sys)).Where(x => x.PRVTMTYN == "U" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();



            ViewBag.untreatedtmhper = Math.Round((ViewBag.untreatedtmh) * 100.0 / (ViewBag.unkcase), 1);
            if (double.IsNaN(ViewBag.untreatedtmhper))
            {
                ViewBag.untreatedtmhper = 0;
            }
            ViewBag.totalcase = (ViewBag.newcase + ViewBag.oldcase + ViewBag.unkcase);
            ViewBag.totalcaseper = (ViewBag.newcaseper + ViewBag.oldcaseper + ViewBag.unkcaseper);
            ViewBag.tottreatmh = (ViewBag.NewTretedTmh + ViewBag.oldtreatedtmh + ViewBag.untreatedtmh);
            ViewBag.tottreatmhper = Math.Round((ViewBag.tottreatmh) * 100.0 / (ViewBag.totalcase), 1);


            ViewBag.totaltre = db.fnl_alysis.Where(x => x.System4 == "Genito Urinary Organs" && new[] { "C51", "C52", "C53", "C54-C55", "C56", "C57", "C58" }.Contains(x.Sys)).Where(x => x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.notunderwent = (ViewBag.TotalCount - ViewBag.totaltre);
            ViewBag.Outside = (ViewBag.TotalCount - ViewBag.newtmc);
            ViewBag.Per = Math.Round((ViewBag.totaltre) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.Per1 = Math.Round((ViewBag.notunderwent) * 100.0 / (ViewBag.TotalCount), 1);


            ViewBag.agedata = db.fnl_alysis.Where(x => x.System4 == "Genito Urinary Organs" && new[] { "C51", "C52", "C53", "C54-C55", "C56", "C57", "C58" }.Contains(x.Sys))
                .GroupBy(x => x.BROAD_AGE).Select(y => new
                {
                    y.Key,
                    Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System4 == "Genito Urinary Organs" && new[] { "C51", "C52", "C53", "C54-C55", "C56", "C57", "C58" }.Contains(x.Sys))
                .Select(x => x.BROAD_AGE).Count(), 1)
                }).OrderBy(i => i.Key == ">=60").ThenBy(i => i.Key == "46-59").ThenBy(i => i.Key == "20-45").ThenBy(i => i.Key == "0-19").ToList();



            //  var objj = db.fnl_alysis.Where(x => x.System4 == "Genito Urinary Organs" && x.final_tti != null && new[] { "C51", "C52", "C53", "C54-C55", "C56", "C57", "C58" }.Contains(x.Sys)).GroupBy(x => x.final_tti).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System4 == "Genito Urinary Organs"  && x.final_tti != null && new[] { "C51", "C52", "C53", "C54-C55", "C56", "C57", "C58" }.Contains(x.Sys)).Select(x => x.final_tti).Count(), 1) }).OrderBy(i => i.Key == ">180 days").ThenBy(i => i.Key == "91-180 days").ThenBy(i => i.Key == "31-90 days").ThenBy(i => i.Key == "<30 days").ToList();
            var objj = db.fnl_alysis.Where(x => x.System4 == "Genito Urinary Organs" && x.final_tti != null).GroupBy(x => x.final_tti).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System4 == "Genito Urinary Organs" && x.final_tti != null).Select(x => x.final_tti).Count(), 1) }).OrderBy(i => i.Key == ">180 days").ThenBy(i => i.Key == "91-180 days").ThenBy(i => i.Key == "31-90 days").ThenBy(i => i.Key == "<=30 days").ToList();



            ViewBag.timedata = objj.Where(x => x.Key != null);


            ViewBag.System4 = "Female Gynaecological Cancers";
            return PartialView("_Genito_Urinary_Organs", list);
        }   //main

        [HttpGet]
        public PartialViewResult Cervix()
        {

            var data = db.fnl_alysis.Where(x => x.System3 == "Cervix").GroupBy(x => x.N_CLINEXT).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System3 == "Cervix").Select(x => x.N_CLINEXT).Count(), 1) }).OrderBy(i => i.Key == "Not Known").ThenBy(i => i.Key == "Treated outside").ThenBy(i => i.Key == "Too Advanced").ThenBy(i => i.Key == "Dist. Mets").ThenBy(i => i.Key == "Regional").ThenBy(i => i.Key == "Localised").ToList();

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
            ViewBag.malecount = db.fnl_alysis.Where(x => x.SEX == "M" && x.System3 == "Cervix").Count();
            ViewBag.femalecount = db.fnl_alysis.Where(x => x.SEX == "F" && x.System3 == "Cervix").Count();

            var obj = db.fnl_alysis.Where(x => x.System3 == "Cervix").GroupBy(x => x.TMH_RX).Select(y => new { y.Key, Count = y.Count() }).ToList();

            ViewBag.objectname = obj.Where(x => x.Key != null);
            var list = db.fnl_alysis.Where(x => x.System3 == "Cervix").GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.System3 == "Cervix").Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Take(5);
            var list1 = db.fnl_alysis.Where(x => x.System3 == "Cervix").GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.System3 == "Cervix").Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Skip(5);

            ViewBag.other = list1.Sum(y => y.Statecount);
            ViewBag.Otherper = Math.Round(list1.Sum(x => x.Statecount) * 100 / db.fnl_alysis.Where(x => x.System3 == "Cervix").Select(x => x.STATE).Count(), 1);
            // ViewBag.totalper = db.fnl_alysis.Where(x => x.System4 == sys4 ).Select(x => x.STATE).Count() * 100 / db.fnl_alysis.Where(x => x.System4 == sys4 ).Select(x => x.STATE).Count();
            ViewBag.TotalCount = db.fnl_alysis.Where(x => x.System3 == "Cervix").Select(x => x.STATE).Count();

            ViewBag.newcase = db.fnl_alysis.Where(x => x.System3 == "Cervix").Where(x => x.PRVTMTYN == "N").Count();
            ViewBag.newcaseper = Math.Round((ViewBag.newcase) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.oldcase = db.fnl_alysis.Where(x => x.System3 == "Cervix").Where(x => x.PRVTMTYN == "Y").Count();
            ViewBag.oldcaseper = Math.Round((ViewBag.oldcase) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.unkcase = db.fnl_alysis.Where(x => x.System3 == "Cervix").Where(x => x.PRVTMTYN == "U").Count();
            ViewBag.unkcaseper = Math.Round((ViewBag.unkcase) * 100.0 / (ViewBag.TotalCount), 1);

            ViewBag.NewTretedTmh = db.fnl_alysis.Where(x => x.System3 == "Cervix").Where(x => x.PRVTMTYN == "N" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.NewTretedTmhper = Math.Round((ViewBag.NewTretedTmh) * 100.0 / (ViewBag.newcase), 1);
            ViewBag.oldtreatedtmh = db.fnl_alysis.Where(x => x.System3 == "Cervix").Where(x => x.PRVTMTYN == "Y" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.oldtreatedtmhper = Math.Round((ViewBag.oldtreatedtmh) * 100.0 / (ViewBag.oldcase), 1);
            ViewBag.untreatedtmh = db.fnl_alysis.Where(x => x.System3 == "Cervix").Where(x => x.PRVTMTYN == "U" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.untreatedtmhper = Math.Round((ViewBag.untreatedtmh) * 100.0 / (ViewBag.unkcase), 1);
            if (double.IsNaN(ViewBag.untreatedtmhper))
            {
                ViewBag.untreatedtmhper = 0;
            }
            ViewBag.totalcase = (ViewBag.newcase + ViewBag.oldcase + ViewBag.unkcase);
            ViewBag.totalcaseper = (ViewBag.newcaseper + ViewBag.oldcaseper + ViewBag.unkcaseper);
            ViewBag.tottreatmh = (ViewBag.NewTretedTmh + ViewBag.oldtreatedtmh + ViewBag.untreatedtmh);
            ViewBag.tottreatmhper = Math.Round((ViewBag.tottreatmh) * 100.0 / (ViewBag.totalcase), 1);


            ViewBag.agedata = db.fnl_alysis.Where(x => x.System3 == "Cervix").GroupBy(x => x.BROAD_AGE).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System3 == "Cervix").Select(x => x.BROAD_AGE).Count(), 1) }).ToList();
            var objj = db.fnl_alysis.Where(x => x.System3 == "Cervix").GroupBy(x => x.final_tti).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System3 == "Cervix").Select(x => x.final_tti).Count(), 1) }).ToList();
            ViewBag.timedata = objj.Where(x => x.Key != null);

            ViewBag.System3 = "Cervix Cancer";
            return PartialView("_Cervix", list);
        }

        [HttpGet]
        public PartialViewResult Corpus_Uteri()
        {

            var data = db.fnl_alysis.Where(x => x.System3 == "Corpus Uteri").GroupBy(x => x.N_CLINEXT).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System3 == "Corpus Uteri").Select(x => x.N_CLINEXT).Count(), 1) }).OrderBy(i => i.Key == "Not Known").ThenBy(i => i.Key == "Treated outside").ThenBy(i => i.Key == "Too Advanced").ThenBy(i => i.Key == "Dist. Mets").ThenBy(i => i.Key == "Regional").ThenBy(i => i.Key == "Localised").ToList();

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
            ViewBag.malecount = db.fnl_alysis.Where(x => x.SEX == "M" && x.System3 == "Corpus Uteri").Count();
            ViewBag.femalecount = db.fnl_alysis.Where(x => x.SEX == "F" && x.System3 == "Corpus Uteri").Count();

            var obj = db.fnl_alysis.Where(x => x.System3 == "Corpus Uteri").GroupBy(x => x.TMH_RX).Select(y => new { y.Key, Count = y.Count() }).ToList();

            ViewBag.objectname = obj.Where(x => x.Key != null);
            var list = db.fnl_alysis.Where(x => x.System3 == "Corpus Uteri").GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.System3 == "Corpus Uteri").Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Take(5);
            var list1 = db.fnl_alysis.Where(x => x.System3 == "Corpus Uteri").GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.System3 == "Corpus Uteri").Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Skip(5);

            ViewBag.other = list1.Sum(y => y.Statecount);
            ViewBag.Otherper = Math.Round(list1.Sum(x => x.Statecount) * 100 / db.fnl_alysis.Where(x => x.System3 == "Corpus Uteri").Select(x => x.STATE).Count(), 1);
            // ViewBag.totalper = db.fnl_alysis.Where(x => x.System4 == sys4 ).Select(x => x.STATE).Count() * 100 / db.fnl_alysis.Where(x => x.System4 == sys4 ).Select(x => x.STATE).Count();
            ViewBag.TotalCount = db.fnl_alysis.Where(x => x.System3 == "Corpus Uteri").Select(x => x.STATE).Count();

            ViewBag.newcase = db.fnl_alysis.Where(x => x.System3 == "Corpus Uteri").Where(x => x.PRVTMTYN == "N").Count();
            ViewBag.newcaseper = Math.Round((ViewBag.newcase) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.oldcase = db.fnl_alysis.Where(x => x.System3 == "Corpus Uteri").Where(x => x.PRVTMTYN == "Y").Count();
            ViewBag.oldcaseper = Math.Round((ViewBag.oldcase) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.unkcase = db.fnl_alysis.Where(x => x.System3 == "Corpus Uteri").Where(x => x.PRVTMTYN == "U").Count();
            ViewBag.unkcaseper = Math.Round((ViewBag.unkcase) * 100.0 / (ViewBag.TotalCount), 1);

            ViewBag.NewTretedTmh = db.fnl_alysis.Where(x => x.System3 == "Corpus Uteri").Where(x => x.PRVTMTYN == "N" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.NewTretedTmhper = Math.Round((ViewBag.NewTretedTmh) * 100.0 / (ViewBag.newcase), 1);
            ViewBag.oldtreatedtmh = db.fnl_alysis.Where(x => x.System3 == "Corpus Uteri").Where(x => x.PRVTMTYN == "Y" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.oldtreatedtmhper = Math.Round((ViewBag.oldtreatedtmh) * 100.0 / (ViewBag.oldcase), 1);
            ViewBag.untreatedtmh = db.fnl_alysis.Where(x => x.System3 == "Corpus Uteri").Where(x => x.PRVTMTYN == "U" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.untreatedtmhper = Math.Round((ViewBag.untreatedtmh) * 100.0 / (ViewBag.unkcase), 1);
            if (double.IsNaN(ViewBag.untreatedtmhper))
            {
                ViewBag.untreatedtmhper = 0;
            }
            ViewBag.totalcase = (ViewBag.newcase + ViewBag.oldcase + ViewBag.unkcase);
            ViewBag.totalcaseper = (ViewBag.newcaseper + ViewBag.oldcaseper + ViewBag.unkcaseper);
            ViewBag.tottreatmh = (ViewBag.NewTretedTmh + ViewBag.oldtreatedtmh + ViewBag.untreatedtmh);
            ViewBag.tottreatmhper = Math.Round((ViewBag.tottreatmh) * 100.0 / (ViewBag.totalcase), 1);


            ViewBag.agedata = db.fnl_alysis.Where(x => x.System3 == "Corpus Uteri").GroupBy(x => x.BROAD_AGE).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System3 == "Corpus Uteri").Select(x => x.BROAD_AGE).Count(), 1) }).ToList();
            var objj = db.fnl_alysis.Where(x => x.System3 == "Corpus Uteri").GroupBy(x => x.final_tti).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System3 == "Corpus Uteri").Select(x => x.final_tti).Count(), 1) }).ToList();
            ViewBag.timedata = objj.Where(x => x.Key != null);

            ViewBag.System3 = "Corpus Uteri";
            return PartialView("_Corpus_Uteri", list);
        }


        [HttpGet]
        public PartialViewResult Ovary()
        {

            var data = db.fnl_alysis.Where(x => x.System3 == "Ovary").GroupBy(x => x.N_CLINEXT).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System3 == "Ovary").Select(x => x.N_CLINEXT).Count(), 1) }).OrderBy(i => i.Key == "Not Known").ThenBy(i => i.Key == "Treated outside").ThenBy(i => i.Key == "Too Advanced").ThenBy(i => i.Key == "Dist. Mets").ThenBy(i => i.Key == "Regional").ThenBy(i => i.Key == "Localised").ToList();

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
            ViewBag.malecount = db.fnl_alysis.Where(x => x.SEX == "M" && x.System3 == "Ovary").Count();
            ViewBag.femalecount = db.fnl_alysis.Where(x => x.SEX == "F" && x.System3 == "Ovary").Count();

            var obj = db.fnl_alysis.Where(x => x.System3 == "Ovary").GroupBy(x => x.TMH_RX).Select(y => new { y.Key, Count = y.Count() }).ToList();

            ViewBag.objectname = obj.Where(x => x.Key != null);
            var list = db.fnl_alysis.Where(x => x.System3 == "Ovary").GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.System3 == "Ovary").Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Take(5);
            var list1 = db.fnl_alysis.Where(x => x.System3 == "Ovary").GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.System3 == "Ovary").Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Skip(5);

            ViewBag.other = list1.Sum(y => y.Statecount);
            ViewBag.Otherper = Math.Round(list1.Sum(x => x.Statecount) * 100 / db.fnl_alysis.Where(x => x.System3 == "Ovary").Select(x => x.STATE).Count(), 1);
            // ViewBag.totalper = db.fnl_alysis.Where(x => x.System4 == sys4 ).Select(x => x.STATE).Count() * 100 / db.fnl_alysis.Where(x => x.System4 == sys4 ).Select(x => x.STATE).Count();
            ViewBag.TotalCount = db.fnl_alysis.Where(x => x.System3 == "Ovary").Select(x => x.STATE).Count();

            ViewBag.newcase = db.fnl_alysis.Where(x => x.System3 == "Ovary").Where(x => x.PRVTMTYN == "N").Count();
            ViewBag.newcaseper = Math.Round((ViewBag.newcase) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.oldcase = db.fnl_alysis.Where(x => x.System3 == "Ovary").Where(x => x.PRVTMTYN == "Y").Count();
            ViewBag.oldcaseper = Math.Round((ViewBag.oldcase) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.unkcase = db.fnl_alysis.Where(x => x.System3 == "Ovary").Where(x => x.PRVTMTYN == "U").Count();
            ViewBag.unkcaseper = Math.Round((ViewBag.unkcase) * 100.0 / (ViewBag.TotalCount), 1);

            ViewBag.NewTretedTmh = db.fnl_alysis.Where(x => x.System3 == "Ovary").Where(x => x.PRVTMTYN == "N" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.NewTretedTmhper = Math.Round((ViewBag.NewTretedTmh) * 100.0 / (ViewBag.newcase), 1);
            ViewBag.oldtreatedtmh = db.fnl_alysis.Where(x => x.System3 == "Ovary").Where(x => x.PRVTMTYN == "Y" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.oldtreatedtmhper = Math.Round((ViewBag.oldtreatedtmh) * 100.0 / (ViewBag.oldcase), 1);
            ViewBag.untreatedtmh = db.fnl_alysis.Where(x => x.System3 == "Ovary").Where(x => x.PRVTMTYN == "U" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.untreatedtmhper = Math.Round((ViewBag.untreatedtmh) * 100.0 / (ViewBag.unkcase), 1);
            if (double.IsNaN(ViewBag.untreatedtmhper))
            {
                ViewBag.untreatedtmhper = 0;
            }
            ViewBag.totalcase = (ViewBag.newcase + ViewBag.oldcase + ViewBag.unkcase);
            ViewBag.totalcaseper = (ViewBag.newcaseper + ViewBag.oldcaseper + ViewBag.unkcaseper);
            ViewBag.tottreatmh = (ViewBag.NewTretedTmh + ViewBag.oldtreatedtmh + ViewBag.untreatedtmh);
            ViewBag.tottreatmhper = Math.Round((ViewBag.tottreatmh) * 100.0 / (ViewBag.totalcase), 1);


            ViewBag.agedata = db.fnl_alysis.Where(x => x.System3 == "Ovary").GroupBy(x => x.BROAD_AGE).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System3 == "Ovary").Select(x => x.BROAD_AGE).Count(), 1) }).ToList();
            var objj = db.fnl_alysis.Where(x => x.System3 == "Ovary").GroupBy(x => x.final_tti).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System3 == "Ovary").Select(x => x.final_tti).Count(), 1) }).ToList();
            ViewBag.timedata = objj.Where(x => x.Key != null);

            ViewBag.System3 = "Ovary";
            return PartialView("_Ovary", list);
        }


        [HttpGet]
        public PartialViewResult Vagina()
        {

            var data = db.fnl_alysis.Where(x => x.System3 == "Vagina").GroupBy(x => x.N_CLINEXT).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System3 == "Vagina").Select(x => x.N_CLINEXT).Count(), 1) }).OrderBy(i => i.Key == "Not Known").ThenBy(i => i.Key == "Treated outside").ThenBy(i => i.Key == "Too Advanced").ThenBy(i => i.Key == "Dist. Mets").ThenBy(i => i.Key == "Regional").ThenBy(i => i.Key == "Localised").ToList();

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
            ViewBag.malecount = db.fnl_alysis.Where(x => x.SEX == "M" && x.System3 == "Vagina").Count();
            ViewBag.femalecount = db.fnl_alysis.Where(x => x.SEX == "F" && x.System3 == "Vagina").Count();

            var obj = db.fnl_alysis.Where(x => x.System3 == "Vagina").GroupBy(x => x.TMH_RX).Select(y => new { y.Key, Count = y.Count() }).ToList();

            ViewBag.objectname = obj.Where(x => x.Key != null);
            var list = db.fnl_alysis.Where(x => x.System3 == "Vagina").GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.System3 == "Vagina").Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Take(5);
            var list1 = db.fnl_alysis.Where(x => x.System3 == "Vagina").GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.System3 == "Vagina").Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Skip(5);

            ViewBag.other = list1.Sum(y => y.Statecount);
            ViewBag.Otherper = Math.Round(list1.Sum(x => x.Statecount) * 100 / db.fnl_alysis.Where(x => x.System3 == "Vagina").Select(x => x.STATE).Count(), 1);
            // ViewBag.totalper = db.fnl_alysis.Where(x => x.System4 == sys4 ).Select(x => x.STATE).Count() * 100 / db.fnl_alysis.Where(x => x.System4 == sys4 ).Select(x => x.STATE).Count();
            ViewBag.TotalCount = db.fnl_alysis.Where(x => x.System3 == "Vagina").Select(x => x.STATE).Count();

            ViewBag.newcase = db.fnl_alysis.Where(x => x.System3 == "Vagina").Where(x => x.PRVTMTYN == "N").Count();
            ViewBag.newcaseper = Math.Round((ViewBag.newcase) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.oldcase = db.fnl_alysis.Where(x => x.System3 == "Vagina").Where(x => x.PRVTMTYN == "Y").Count();
            ViewBag.oldcaseper = Math.Round((ViewBag.oldcase) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.unkcase = db.fnl_alysis.Where(x => x.System3 == "Vagina").Where(x => x.PRVTMTYN == "U").Count();
            ViewBag.unkcaseper = Math.Round((ViewBag.unkcase) * 100.0 / (ViewBag.TotalCount), 1);

            ViewBag.NewTretedTmh = db.fnl_alysis.Where(x => x.System3 == "Vagina").Where(x => x.PRVTMTYN == "N" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.NewTretedTmhper = Math.Round((ViewBag.NewTretedTmh) * 100.0 / (ViewBag.newcase), 1);
            ViewBag.oldtreatedtmh = db.fnl_alysis.Where(x => x.System3 == "Vagina").Where(x => x.PRVTMTYN == "Y" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.oldtreatedtmhper = Math.Round((ViewBag.oldtreatedtmh) * 100.0 / (ViewBag.oldcase), 1);
            ViewBag.untreatedtmh = db.fnl_alysis.Where(x => x.System3 == "Vagina").Where(x => x.PRVTMTYN == "U" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.untreatedtmhper = Math.Round((ViewBag.untreatedtmh) * 100.0 / (ViewBag.unkcase), 1);
            if (double.IsNaN(ViewBag.untreatedtmhper))
            {
                ViewBag.untreatedtmhper = 0;
            }
            ViewBag.totalcase = (ViewBag.newcase + ViewBag.oldcase + ViewBag.unkcase);
            ViewBag.totalcaseper = (ViewBag.newcaseper + ViewBag.oldcaseper + ViewBag.unkcaseper);
            ViewBag.tottreatmh = (ViewBag.NewTretedTmh + ViewBag.oldtreatedtmh + ViewBag.untreatedtmh);
            ViewBag.tottreatmhper = Math.Round((ViewBag.tottreatmh) * 100.0 / (ViewBag.totalcase), 1);


            ViewBag.agedata = db.fnl_alysis.Where(x => x.System3 == "Vagina").GroupBy(x => x.BROAD_AGE).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System3 == "Vagina").Select(x => x.BROAD_AGE).Count(), 1) }).ToList();
            var objj = db.fnl_alysis.Where(x => x.System3 == "Vagina").GroupBy(x => x.final_tti).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System3 == "Vagina").Select(x => x.final_tti).Count(), 1) }).ToList();
            ViewBag.timedata = objj.Where(x => x.Key != null);

            ViewBag.System3 = "Vagina";
            return PartialView("_Vagina", list);
        }


        [HttpGet]
        public PartialViewResult Uro_Male_Reproductive()
        {
            var data = db.fnl_alysis.Where(x => x.gynae_system == "Uro and Male Reproductive Organs").GroupBy(x => x.N_CLINEXT).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.gynae_system == "Uro and Male Reproductive Organs").Select(x => x.N_CLINEXT).Count(), 1) }).OrderBy(i => i.Key == "Not Known").ThenBy(i => i.Key == "Treated outside").ThenBy(i => i.Key == "Too Advanced").ThenBy(i => i.Key == "Dist. Mets").ThenBy(i => i.Key == "Regional").ThenBy(i => i.Key == "Localised").ToList();


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

            ViewBag.malecount = db.fnl_alysis.Where(x => x.SEX == "M" && x.gynae_system == "Uro and Male Reproductive Organs").Count();
            ViewBag.femalecount = db.fnl_alysis.Where(x => x.SEX == "F" && x.gynae_system == "Uro and Male Reproductive Organs").Count();

            var obj = db.fnl_alysis.Where(x => x.gynae_system == "Uro and Male Reproductive Organs").GroupBy(x => x.TMH_RX).Select(y => new { y.Key, Count = y.Count() }).ToList();

            ViewBag.objectname = obj.Where(x => x.Key != null);
            var list = db.fnl_alysis.Where(x => x.gynae_system == "Uro and Male Reproductive Organs").GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.gynae_system == "Uro and Male Reproductive Organs").Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Take(5);
            var list1 = db.fnl_alysis.Where(x => x.gynae_system == "Uro and Male Reproductive Organs").GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.gynae_system == "Uro and Male Reproductive Organs").Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Skip(5);

            ViewBag.other = list1.Sum(y => y.Statecount);
            ViewBag.Otherper = Math.Round(list1.Sum(x => x.Statecount) * 100 / db.fnl_alysis.Where(x => x.gynae_system == "Uro and Male Reproductive Organs").Select(x => x.STATE).Count(), 1);
            ViewBag.TotalCount = db.fnl_alysis.Where(x => x.gynae_system == "Uro and Male Reproductive Organs").Select(x => x.STATE).Count();


            ViewBag.newcase = db.fnl_alysis.Where(x => x.gynae_system == "Uro and Male Reproductive Organs").Where(x => x.PRVTMTYN == "N").Count();
            ViewBag.newcaseper = Math.Round((ViewBag.newcase) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.oldcase = db.fnl_alysis.Where(x => x.gynae_system == "Uro and Male Reproductive Organs").Where(x => x.PRVTMTYN == "Y").Count();
            ViewBag.oldcaseper = Math.Round((ViewBag.oldcase) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.unkcase = db.fnl_alysis.Where(x => x.gynae_system == "Uro and Male Reproductive Organs").Where(x => x.PRVTMTYN == "U").Count();

            ViewBag.unkcaseper = Math.Round((ViewBag.unkcase) * 100.0 / (ViewBag.TotalCount), 1);

            ViewBag.NewTretedTmh = db.fnl_alysis.Where(x => x.gynae_system == "Uro and Male Reproductive Organs").Where(x => x.PRVTMTYN == "N" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.NewTretedTmhper = Math.Round((ViewBag.NewTretedTmh) * 100.0 / (ViewBag.newcase), 1);
            ViewBag.oldtreatedtmh = db.fnl_alysis.Where(x => x.gynae_system == "Uro and Male Reproductive Organs").Where(x => x.PRVTMTYN == "Y" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.oldtreatedtmhper = Math.Round((ViewBag.oldtreatedtmh) * 100.0 / (ViewBag.oldcase), 1);
            ViewBag.untreatedtmh = db.fnl_alysis.Where(x => x.gynae_system == "Uro and Male Reproductive Organs").Where(x => x.PRVTMTYN == "U" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();



            ViewBag.untreatedtmhper = Math.Round((ViewBag.untreatedtmh) * 100.0 / (ViewBag.unkcase), 1);
            if (double.IsNaN(ViewBag.untreatedtmhper))
            {
                ViewBag.untreatedtmhper = 0;
            }
            ViewBag.totalcase = (ViewBag.newcase + ViewBag.oldcase + ViewBag.unkcase);
            ViewBag.totalcaseper = (ViewBag.newcaseper + ViewBag.oldcaseper + ViewBag.unkcaseper);
            ViewBag.tottreatmh = (ViewBag.NewTretedTmh + ViewBag.oldtreatedtmh + ViewBag.untreatedtmh);
            ViewBag.tottreatmhper = Math.Round((ViewBag.tottreatmh) * 100.0 / (ViewBag.totalcase), 1);


            ViewBag.totaltre = db.fnl_alysis.Where(x => x.gynae_system == "Uro and Male Reproductive Organs").Where(x => x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.notunderwent = (ViewBag.TotalCount - ViewBag.totaltre);
            ViewBag.Outside = (ViewBag.TotalCount - ViewBag.newtmc);
            ViewBag.Per = Math.Round((ViewBag.totaltre) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.Per1 = Math.Round((ViewBag.notunderwent) * 100.0 / (ViewBag.TotalCount), 1);


            ViewBag.agedata = db.fnl_alysis.Where(x => x.gynae_system == "Uro and Male Reproductive Organs").GroupBy(x => x.BROAD_AGE).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.gynae_system == "Uro and Male Reproductive Organs").Select(x => x.BROAD_AGE).Count(), 1) }).OrderBy(i => i.Key == ">=60").ThenBy(i => i.Key == "46-59").ThenBy(i => i.Key == "20-45").ThenBy(i => i.Key == "0-19").ToList();



            var objj = db.fnl_alysis.Where(x => x.gynae_system == "Uro and Male Reproductive Organs" && x.final_tti != null).GroupBy(x => x.final_tti).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.gynae_system == "Uro and Male Reproductive Organs" && x.final_tti != null).Select(x => x.final_tti).Count(), 1) }).OrderBy(i => i.Key == ">180 days").ThenBy(i => i.Key == "91-180 days").ThenBy(i => i.Key == "31-90 days").ThenBy(i => i.Key == "<=30 days").ToList();



            ViewBag.timedata = objj.Where(x => x.Key != null);


            ViewBag.gynae_system = "Uro-Oncology and Male Reproductive Cancers";
            return PartialView("_Uro_Male_Repro_Organs", list);
        }    //main

        [HttpGet]
        public PartialViewResult Kidney()
        {

            var data = db.fnl_alysis.Where(x => x.System3 == "Kidney").GroupBy(x => x.N_CLINEXT).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System3 == "Kidney").Select(x => x.N_CLINEXT).Count(), 1) }).OrderBy(i => i.Key == "Not Known").ThenBy(i => i.Key == "Treated outside").ThenBy(i => i.Key == "Too Advanced").ThenBy(i => i.Key == "Dist. Mets").ThenBy(i => i.Key == "Regional").ThenBy(i => i.Key == "Localised").ToList();

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
            ViewBag.malecount = db.fnl_alysis.Where(x => x.SEX == "M" && x.System3 == "Kidney").Count();
            ViewBag.femalecount = db.fnl_alysis.Where(x => x.SEX == "F" && x.System3 == "Kidney").Count();

            var obj = db.fnl_alysis.Where(x => x.System3 == "Kidney").GroupBy(x => x.TMH_RX).Select(y => new { y.Key, Count = y.Count() }).ToList();

            ViewBag.objectname = obj.Where(x => x.Key != null);
            var list = db.fnl_alysis.Where(x => x.System3 == "Kidney").GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.System3 == "Kidney").Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Take(5);
            var list1 = db.fnl_alysis.Where(x => x.System3 == "Kidney").GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.System3 == "Kidney").Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Skip(5);

            ViewBag.other = list1.Sum(y => y.Statecount);
            ViewBag.Otherper = Math.Round(list1.Sum(x => x.Statecount) * 100 / db.fnl_alysis.Where(x => x.System3 == "Kidney").Select(x => x.STATE).Count(), 1);
            // ViewBag.totalper = db.fnl_alysis.Where(x => x.System4 == sys4 ).Select(x => x.STATE).Count() * 100 / db.fnl_alysis.Where(x => x.System4 == sys4 ).Select(x => x.STATE).Count();
            ViewBag.TotalCount = db.fnl_alysis.Where(x => x.System3 == "Kidney").Select(x => x.STATE).Count();

            ViewBag.newcase = db.fnl_alysis.Where(x => x.System3 == "Kidney").Where(x => x.PRVTMTYN == "N").Count();
            ViewBag.newcaseper = Math.Round((ViewBag.newcase) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.oldcase = db.fnl_alysis.Where(x => x.System3 == "Kidney").Where(x => x.PRVTMTYN == "Y").Count();
            ViewBag.oldcaseper = Math.Round((ViewBag.oldcase) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.unkcase = db.fnl_alysis.Where(x => x.System3 == "Kidney").Where(x => x.PRVTMTYN == "U").Count();
            ViewBag.unkcaseper = Math.Round((ViewBag.unkcase) * 100.0 / (ViewBag.TotalCount), 1);

            ViewBag.NewTretedTmh = db.fnl_alysis.Where(x => x.System3 == "Kidney").Where(x => x.PRVTMTYN == "N" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.NewTretedTmhper = Math.Round((ViewBag.NewTretedTmh) * 100.0 / (ViewBag.newcase), 1);
            ViewBag.oldtreatedtmh = db.fnl_alysis.Where(x => x.System3 == "Kidney").Where(x => x.PRVTMTYN == "Y" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.oldtreatedtmhper = Math.Round((ViewBag.oldtreatedtmh) * 100.0 / (ViewBag.oldcase), 1);
            ViewBag.untreatedtmh = db.fnl_alysis.Where(x => x.System3 == "Kidney").Where(x => x.PRVTMTYN == "U" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.untreatedtmhper = Math.Round((ViewBag.untreatedtmh) * 100.0 / (ViewBag.unkcase), 1);
            if (double.IsNaN(ViewBag.untreatedtmhper))
            {
                ViewBag.untreatedtmhper = 0;
            }
            ViewBag.totalcase = (ViewBag.newcase + ViewBag.oldcase + ViewBag.unkcase);
            ViewBag.totalcaseper = (ViewBag.newcaseper + ViewBag.oldcaseper + ViewBag.unkcaseper);
            ViewBag.tottreatmh = (ViewBag.NewTretedTmh + ViewBag.oldtreatedtmh + ViewBag.untreatedtmh);
            ViewBag.tottreatmhper = Math.Round((ViewBag.tottreatmh) * 100.0 / (ViewBag.totalcase), 1);


            ViewBag.agedata = db.fnl_alysis.Where(x => x.System3 == "Kidney").GroupBy(x => x.BROAD_AGE).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System3 == "Kidney").Select(x => x.BROAD_AGE).Count(), 1) }).ToList();
            var objj = db.fnl_alysis.Where(x => x.System3 == "Kidney").GroupBy(x => x.final_tti).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System3 == "Kidney").Select(x => x.final_tti).Count(), 1) }).ToList();
            ViewBag.timedata = objj.Where(x => x.Key != null);

            ViewBag.System3 = "Kidney";
            return PartialView("_Kidney", list);
        }

        [HttpGet]
        public PartialViewResult Prostate()
        {

            var data = db.fnl_alysis.Where(x => x.System3 == "Prostate").GroupBy(x => x.N_CLINEXT).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System3 == "Prostate").Select(x => x.N_CLINEXT).Count(), 1) }).OrderBy(i => i.Key == "Not Known").ThenBy(i => i.Key == "Treated outside").ThenBy(i => i.Key == "Too Advanced").ThenBy(i => i.Key == "Dist. Mets").ThenBy(i => i.Key == "Regional").ThenBy(i => i.Key == "Localised").ToList();

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
            ViewBag.malecount = db.fnl_alysis.Where(x => x.SEX == "M" && x.System3 == "Prostate").Count();
            ViewBag.femalecount = db.fnl_alysis.Where(x => x.SEX == "F" && x.System3 == "Prostate").Count();

            var obj = db.fnl_alysis.Where(x => x.System3 == "Prostate").GroupBy(x => x.TMH_RX).Select(y => new { y.Key, Count = y.Count() }).ToList();

            ViewBag.objectname = obj.Where(x => x.Key != null);
            var list = db.fnl_alysis.Where(x => x.System3 == "Prostate").GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.System3 == "Prostate").Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Take(5);
            var list1 = db.fnl_alysis.Where(x => x.System3 == "Prostate").GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.System3 == "Prostate").Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Skip(5);

            ViewBag.other = list1.Sum(y => y.Statecount);
            ViewBag.Otherper = Math.Round(list1.Sum(x => x.Statecount) * 100 / db.fnl_alysis.Where(x => x.System3 == "Prostate").Select(x => x.STATE).Count(), 1);
            // ViewBag.totalper = db.fnl_alysis.Where(x => x.System4 == sys4 ).Select(x => x.STATE).Count() * 100 / db.fnl_alysis.Where(x => x.System4 == sys4 ).Select(x => x.STATE).Count();
            ViewBag.TotalCount = db.fnl_alysis.Where(x => x.System3 == "Prostate").Select(x => x.STATE).Count();

            ViewBag.newcase = db.fnl_alysis.Where(x => x.System3 == "Prostate").Where(x => x.PRVTMTYN == "N").Count();
            ViewBag.newcaseper = Math.Round((ViewBag.newcase) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.oldcase = db.fnl_alysis.Where(x => x.System3 == "Prostate").Where(x => x.PRVTMTYN == "Y").Count();
            ViewBag.oldcaseper = Math.Round((ViewBag.oldcase) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.unkcase = db.fnl_alysis.Where(x => x.System3 == "Prostate").Where(x => x.PRVTMTYN == "U").Count();
            ViewBag.unkcaseper = Math.Round((ViewBag.unkcase) * 100.0 / (ViewBag.TotalCount), 1);

            ViewBag.NewTretedTmh = db.fnl_alysis.Where(x => x.System3 == "Prostate").Where(x => x.PRVTMTYN == "N" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.NewTretedTmhper = Math.Round((ViewBag.NewTretedTmh) * 100.0 / (ViewBag.newcase), 1);
            ViewBag.oldtreatedtmh = db.fnl_alysis.Where(x => x.System3 == "Prostate").Where(x => x.PRVTMTYN == "Y" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.oldtreatedtmhper = Math.Round((ViewBag.oldtreatedtmh) * 100.0 / (ViewBag.oldcase), 1);
            ViewBag.untreatedtmh = db.fnl_alysis.Where(x => x.System3 == "Prostate").Where(x => x.PRVTMTYN == "U" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.untreatedtmhper = Math.Round((ViewBag.untreatedtmh) * 100.0 / (ViewBag.unkcase), 1);
            if (double.IsNaN(ViewBag.untreatedtmhper))
            {
                ViewBag.untreatedtmhper = 0;
            }
            ViewBag.totalcase = (ViewBag.newcase + ViewBag.oldcase + ViewBag.unkcase);
            ViewBag.totalcaseper = (ViewBag.newcaseper + ViewBag.oldcaseper + ViewBag.unkcaseper);
            ViewBag.tottreatmh = (ViewBag.NewTretedTmh + ViewBag.oldtreatedtmh + ViewBag.untreatedtmh);
            ViewBag.tottreatmhper = Math.Round((ViewBag.tottreatmh) * 100.0 / (ViewBag.totalcase), 1);


            ViewBag.agedata = db.fnl_alysis.Where(x => x.System3 == "Prostate").GroupBy(x => x.BROAD_AGE).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System3 == "Prostate").Select(x => x.BROAD_AGE).Count(), 1) }).ToList();
            var objj = db.fnl_alysis.Where(x => x.System3 == "Prostate").GroupBy(x => x.final_tti).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System3 == "Prostate").Select(x => x.final_tti).Count(), 1) }).ToList();
            ViewBag.timedata = objj.Where(x => x.Key != null);

            ViewBag.System3 = "Prostate";
            return PartialView("_Prostate", list);
        }

        [HttpGet]
        public PartialViewResult Testis()
        {

            var data = db.fnl_alysis.Where(x => x.System3 == "Testis").GroupBy(x => x.N_CLINEXT).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System3 == "Testis").Select(x => x.N_CLINEXT).Count(), 1) }).OrderBy(i => i.Key == "Not Known").ThenBy(i => i.Key == "Treated outside").ThenBy(i => i.Key == "Too Advanced").ThenBy(i => i.Key == "Dist. Mets").ThenBy(i => i.Key == "Regional").ThenBy(i => i.Key == "Localised").ToList();

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
            ViewBag.malecount = db.fnl_alysis.Where(x => x.SEX == "M" && x.System3 == "Testis").Count();
            ViewBag.femalecount = db.fnl_alysis.Where(x => x.SEX == "F" && x.System3 == "Testis").Count();

            var obj = db.fnl_alysis.Where(x => x.System3 == "Testis").GroupBy(x => x.TMH_RX).Select(y => new { y.Key, Count = y.Count() }).ToList();

            ViewBag.objectname = obj.Where(x => x.Key != null);
            var list = db.fnl_alysis.Where(x => x.System3 == "Testis").GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.System3 == "Testis").Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Take(5);
            var list1 = db.fnl_alysis.Where(x => x.System3 == "Testis").GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.System3 == "Testis").Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Skip(5);

            ViewBag.other = list1.Sum(y => y.Statecount);
            ViewBag.Otherper = Math.Round(list1.Sum(x => x.Statecount) * 100 / db.fnl_alysis.Where(x => x.System3 == "Testis").Select(x => x.STATE).Count(), 1);
            // ViewBag.totalper = db.fnl_alysis.Where(x => x.System4 == sys4 ).Select(x => x.STATE).Count() * 100 / db.fnl_alysis.Where(x => x.System4 == sys4 ).Select(x => x.STATE).Count();
            ViewBag.TotalCount = db.fnl_alysis.Where(x => x.System3 == "Testis").Select(x => x.STATE).Count();

            ViewBag.newcase = db.fnl_alysis.Where(x => x.System3 == "Testis").Where(x => x.PRVTMTYN == "N").Count();
            ViewBag.newcaseper = Math.Round((ViewBag.newcase) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.oldcase = db.fnl_alysis.Where(x => x.System3 == "Testis").Where(x => x.PRVTMTYN == "Y").Count();
            ViewBag.oldcaseper = Math.Round((ViewBag.oldcase) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.unkcase = db.fnl_alysis.Where(x => x.System3 == "Testis").Where(x => x.PRVTMTYN == "U").Count();
            ViewBag.unkcaseper = Math.Round((ViewBag.unkcase) * 100.0 / (ViewBag.TotalCount), 1);

            ViewBag.NewTretedTmh = db.fnl_alysis.Where(x => x.System3 == "Testis").Where(x => x.PRVTMTYN == "N" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.NewTretedTmhper = Math.Round((ViewBag.NewTretedTmh) * 100.0 / (ViewBag.newcase), 1);
            ViewBag.oldtreatedtmh = db.fnl_alysis.Where(x => x.System3 == "Testis").Where(x => x.PRVTMTYN == "Y" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.oldtreatedtmhper = Math.Round((ViewBag.oldtreatedtmh) * 100.0 / (ViewBag.oldcase), 1);
            ViewBag.untreatedtmh = db.fnl_alysis.Where(x => x.System3 == "Testis").Where(x => x.PRVTMTYN == "U" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.untreatedtmhper = Math.Round((ViewBag.untreatedtmh) * 100.0 / (ViewBag.unkcase), 1);
            if (double.IsNaN(ViewBag.untreatedtmhper))
            {
                ViewBag.untreatedtmhper = 0;
            }
            ViewBag.totalcase = (ViewBag.newcase + ViewBag.oldcase + ViewBag.unkcase);
            ViewBag.totalcaseper = (ViewBag.newcaseper + ViewBag.oldcaseper + ViewBag.unkcaseper);
            ViewBag.tottreatmh = (ViewBag.NewTretedTmh + ViewBag.oldtreatedtmh + ViewBag.untreatedtmh);
            ViewBag.tottreatmhper = Math.Round((ViewBag.tottreatmh) * 100.0 / (ViewBag.totalcase), 1);


            ViewBag.agedata = db.fnl_alysis.Where(x => x.System3 == "Testis").GroupBy(x => x.BROAD_AGE).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System3 == "Testis").Select(x => x.BROAD_AGE).Count(), 1) }).ToList();
            var objj = db.fnl_alysis.Where(x => x.System3 == "Testis").GroupBy(x => x.final_tti).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System3 == "Testis").Select(x => x.final_tti).Count(), 1) }).ToList();
            ViewBag.timedata = objj.Where(x => x.Key != null);

            ViewBag.System3 = "Testis";
            return PartialView("_Testis", list);
        }

        [HttpGet]
        public PartialViewResult Urinary_Bladder()
        {

            var data = db.fnl_alysis.Where(x => x.System3 == "Urinary Bladder").GroupBy(x => x.N_CLINEXT).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System3 == "Urinary Bladder").Select(x => x.N_CLINEXT).Count(), 1) }).OrderBy(i => i.Key == "Not Known").ThenBy(i => i.Key == "Treated outside").ThenBy(i => i.Key == "Too Advanced").ThenBy(i => i.Key == "Dist. Mets").ThenBy(i => i.Key == "Regional").ThenBy(i => i.Key == "Localised").ToList();

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
            ViewBag.malecount = db.fnl_alysis.Where(x => x.SEX == "M" && x.System3 == "Urinary Bladder").Count();
            ViewBag.femalecount = db.fnl_alysis.Where(x => x.SEX == "F" && x.System3 == "Urinary Bladder").Count();

            var obj = db.fnl_alysis.Where(x => x.System3 == "Urinary Bladder").GroupBy(x => x.TMH_RX).Select(y => new { y.Key, Count = y.Count() }).ToList();

            ViewBag.objectname = obj.Where(x => x.Key != null);
            var list = db.fnl_alysis.Where(x => x.System3 == "Urinary Bladder").GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.System3 == "Urinary Bladder").Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Take(5);
            var list1 = db.fnl_alysis.Where(x => x.System3 == "Urinary Bladder").GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.System3 == "Urinary Bladder").Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Skip(5);

            ViewBag.other = list1.Sum(y => y.Statecount);
            ViewBag.Otherper = Math.Round(list1.Sum(x => x.Statecount) * 100 / db.fnl_alysis.Where(x => x.System3 == "Urinary Bladder").Select(x => x.STATE).Count(), 1);
            // ViewBag.totalper = db.fnl_alysis.Where(x => x.System4 == sys4 ).Select(x => x.STATE).Count() * 100 / db.fnl_alysis.Where(x => x.System4 == sys4 ).Select(x => x.STATE).Count();
            ViewBag.TotalCount = db.fnl_alysis.Where(x => x.System3 == "Urinary Bladder").Select(x => x.STATE).Count();

            ViewBag.newcase = db.fnl_alysis.Where(x => x.System3 == "Urinary Bladder").Where(x => x.PRVTMTYN == "N").Count();
            ViewBag.newcaseper = Math.Round((ViewBag.newcase) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.oldcase = db.fnl_alysis.Where(x => x.System3 == "Urinary Bladder").Where(x => x.PRVTMTYN == "Y").Count();
            ViewBag.oldcaseper = Math.Round((ViewBag.oldcase) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.unkcase = db.fnl_alysis.Where(x => x.System3 == "Urinary Bladder").Where(x => x.PRVTMTYN == "U").Count();
            ViewBag.unkcaseper = Math.Round((ViewBag.unkcase) * 100.0 / (ViewBag.TotalCount), 1);

            ViewBag.NewTretedTmh = db.fnl_alysis.Where(x => x.System3 == "Urinary Bladder").Where(x => x.PRVTMTYN == "N" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.NewTretedTmhper = Math.Round((ViewBag.NewTretedTmh) * 100.0 / (ViewBag.newcase), 1);
            ViewBag.oldtreatedtmh = db.fnl_alysis.Where(x => x.System3 == "Urinary Bladder").Where(x => x.PRVTMTYN == "Y" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.oldtreatedtmhper = Math.Round((ViewBag.oldtreatedtmh) * 100.0 / (ViewBag.oldcase), 1);
            ViewBag.untreatedtmh = db.fnl_alysis.Where(x => x.System3 == "Urinary Bladder").Where(x => x.PRVTMTYN == "U" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.untreatedtmhper = Math.Round((ViewBag.untreatedtmh) * 100.0 / (ViewBag.unkcase), 1);
            if (double.IsNaN(ViewBag.untreatedtmhper))
            {
                ViewBag.untreatedtmhper = 0;
            }
            ViewBag.totalcase = (ViewBag.newcase + ViewBag.oldcase + ViewBag.unkcase);
            ViewBag.totalcaseper = (ViewBag.newcaseper + ViewBag.oldcaseper + ViewBag.unkcaseper);
            ViewBag.tottreatmh = (ViewBag.NewTretedTmh + ViewBag.oldtreatedtmh + ViewBag.untreatedtmh);
            ViewBag.tottreatmhper = Math.Round((ViewBag.tottreatmh) * 100.0 / (ViewBag.totalcase), 1);


            ViewBag.agedata = db.fnl_alysis.Where(x => x.System3 == "Urinary Bladder").GroupBy(x => x.BROAD_AGE).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System3 == "Urinary Bladder").Select(x => x.BROAD_AGE).Count(), 1) }).ToList();
            var objj = db.fnl_alysis.Where(x => x.System3 == "Urinary Bladder").GroupBy(x => x.final_tti).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System3 == "Urinary Bladder").Select(x => x.final_tti).Count(), 1) }).ToList();
            ViewBag.timedata = objj.Where(x => x.Key != null);

            ViewBag.System3 = "Urinary Bladder";
            return PartialView("_Urinary_Bladder", list);
        }

        [HttpGet]
        public PartialViewResult Haematopoetic()
        {
            var data = db.fnl_alysis.Where(x => x.System4 == "Haematopoetic Cancers").GroupBy(x => x.N_CLINEXT).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System4 == "Haematopoetic Cancers").Select(x => x.N_CLINEXT).Count(), 1) }).OrderBy(i => i.Key == "Not Known").ThenBy(i => i.Key == "Treated outside").ThenBy(i => i.Key == "Too Advanced").ThenBy(i => i.Key == "Dist. Mets").ThenBy(i => i.Key == "Regional").ThenBy(i => i.Key == "Localised").ToList();


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

            ViewBag.malecount = db.fnl_alysis.Where(x => x.SEX == "M" && x.System4 == "Haematopoetic Cancers").Count();
            ViewBag.femalecount = db.fnl_alysis.Where(x => x.SEX == "F" && x.System4 == "Haematopoetic Cancers").Count();

            var obj = db.fnl_alysis.Where(x => x.System4 == "Haematopoetic Cancers").GroupBy(x => x.TMH_RX).Select(y => new { y.Key, Count = y.Count() }).ToList();

            ViewBag.objectname = obj.Where(x => x.Key != null);
            var list = db.fnl_alysis.Where(x => x.System4 == "Haematopoetic Cancers").GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.System4 == "Haematopoetic Cancers").Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Take(5);
            var list1 = db.fnl_alysis.Where(x => x.System4 == "Haematopoetic Cancers").GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.System4 == "Haematopoetic Cancers").Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Skip(5);

            ViewBag.other = list1.Sum(y => y.Statecount);
            ViewBag.Otherper = Math.Round(list1.Sum(x => x.Statecount) * 100 / db.fnl_alysis.Where(x => x.System4 == "Haematopoetic Cancers").Select(x => x.STATE).Count(), 1);
            ViewBag.TotalCount = db.fnl_alysis.Where(x => x.System4 == "Haematopoetic Cancers").Select(x => x.STATE).Count();


            ViewBag.newcase = db.fnl_alysis.Where(x => x.System4 == "Haematopoetic Cancers").Where(x => x.PRVTMTYN == "N").Count();
            ViewBag.newcaseper = Math.Round((ViewBag.newcase) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.oldcase = db.fnl_alysis.Where(x => x.System4 == "Haematopoetic Cancers").Where(x => x.PRVTMTYN == "Y").Count();
            ViewBag.oldcaseper = Math.Round((ViewBag.oldcase) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.unkcase = db.fnl_alysis.Where(x => x.System4 == "Haematopoetic Cancers").Where(x => x.PRVTMTYN == "U").Count();

            ViewBag.unkcaseper = Math.Round((ViewBag.unkcase) * 100.0 / (ViewBag.TotalCount), 1);

            ViewBag.NewTretedTmh = db.fnl_alysis.Where(x => x.System4 == "Haematopoetic Cancers").Where(x => x.PRVTMTYN == "N" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.NewTretedTmhper = Math.Round((ViewBag.NewTretedTmh) * 100.0 / (ViewBag.newcase), 1);
            ViewBag.oldtreatedtmh = db.fnl_alysis.Where(x => x.System4 == "Haematopoetic Cancers").Where(x => x.PRVTMTYN == "Y" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.oldtreatedtmhper = Math.Round((ViewBag.oldtreatedtmh) * 100.0 / (ViewBag.oldcase), 1);
            ViewBag.untreatedtmh = db.fnl_alysis.Where(x => x.System4 == "Haematopoetic Cancers").Where(x => x.PRVTMTYN == "U" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();



            ViewBag.untreatedtmhper = Math.Round((ViewBag.untreatedtmh) * 100.0 / (ViewBag.unkcase), 1);
            if (double.IsNaN(ViewBag.untreatedtmhper))
            {
                ViewBag.untreatedtmhper = 0;
            }
            ViewBag.totalcase = (ViewBag.newcase + ViewBag.oldcase + ViewBag.unkcase);
            ViewBag.totalcaseper = (ViewBag.newcaseper + ViewBag.oldcaseper + ViewBag.unkcaseper);
            ViewBag.tottreatmh = (ViewBag.NewTretedTmh + ViewBag.oldtreatedtmh + ViewBag.untreatedtmh);
            ViewBag.tottreatmhper = Math.Round((ViewBag.tottreatmh) * 100.0 / (ViewBag.totalcase), 1);


            ViewBag.totaltre = db.fnl_alysis.Where(x => x.System4 == "Haematopoetic Cancers").Where(x => x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.notunderwent = (ViewBag.TotalCount - ViewBag.totaltre);
            ViewBag.Outside = (ViewBag.TotalCount - ViewBag.newtmc);
            ViewBag.Per = Math.Round((ViewBag.totaltre) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.Per1 = Math.Round((ViewBag.notunderwent) * 100.0 / (ViewBag.TotalCount), 1);


            ViewBag.agedata = db.fnl_alysis.Where(x => x.System4 == "Haematopoetic Cancers").GroupBy(x => x.BROAD_AGE).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System4 == "Haematopoetic Cancers").Select(x => x.BROAD_AGE).Count(), 1) }).OrderBy(i => i.Key == ">=60").ThenBy(i => i.Key == "46-59").ThenBy(i => i.Key == "20-45").ThenBy(i => i.Key == "0-19").ToList();



            var objj = db.fnl_alysis.Where(x => x.System4 == "Haematopoetic Cancers" && x.final_tti != null).GroupBy(x => x.final_tti).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System4 == "Haematopoetic Cancers" && x.final_tti != null).Select(x => x.final_tti).Count(), 1) }).OrderBy(i => i.Key == ">180 days").ThenBy(i => i.Key == "91-180 days").ThenBy(i => i.Key == "31-90 days").ThenBy(i => i.Key == "<=30 days").ToList();



            ViewBag.timedata = objj.Where(x => x.Key != null);


            ViewBag.System4 = "Haematopoetic Cancers";
            return PartialView("_Haematopoetic", list);
        }   //main


        [HttpGet]
        public PartialViewResult Malignant_Lymphoma()
        {

            var data = db.fnl_alysis.Where(x => x.System3 == "Non-Hodgkins Lymphoma").GroupBy(x => x.N_CLINEXT).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System3 == "Non-Hodgkins Lymphoma").Select(x => x.N_CLINEXT).Count(), 1) }).OrderBy(i => i.Key == "Not Known").ThenBy(i => i.Key == "Treated outside").ThenBy(i => i.Key == "Too Advanced").ThenBy(i => i.Key == "Dist. Mets").ThenBy(i => i.Key == "Regional").ThenBy(i => i.Key == "Localised").ToList();

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
            ViewBag.malecount = db.fnl_alysis.Where(x => x.SEX == "M" && x.System3 == "Non-Hodgkins Lymphoma").Count();
            ViewBag.femalecount = db.fnl_alysis.Where(x => x.SEX == "F" && x.System3 == "Non-Hodgkins Lymphoma").Count();

            var obj = db.fnl_alysis.Where(x => x.System3 == "Non-Hodgkins Lymphoma").GroupBy(x => x.TMH_RX).Select(y => new { y.Key, Count = y.Count() }).ToList();

            ViewBag.objectname = obj.Where(x => x.Key != null);
            var list = db.fnl_alysis.Where(x => x.System3 == "Non-Hodgkins Lymphoma").GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.System3 == "Non-Hodgkins Lymphoma").Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Take(5);
            var list1 = db.fnl_alysis.Where(x => x.System3 == "Non-Hodgkins Lymphoma").GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.System3 == "Non-Hodgkins Lymphoma").Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Skip(5);

            ViewBag.other = list1.Sum(y => y.Statecount);
            ViewBag.Otherper = Math.Round(list1.Sum(x => x.Statecount) * 100 / db.fnl_alysis.Where(x => x.System3 == "Non-Hodgkins Lymphoma").Select(x => x.STATE).Count(), 1);
            // ViewBag.totalper = db.fnl_alysis.Where(x => x.System4 == sys4 ).Select(x => x.STATE).Count() * 100 / db.fnl_alysis.Where(x => x.System4 == sys4 ).Select(x => x.STATE).Count();
            ViewBag.TotalCount = db.fnl_alysis.Where(x => x.System3 == "Non-Hodgkins Lymphoma").Select(x => x.STATE).Count();

            ViewBag.newcase = db.fnl_alysis.Where(x => x.System3 == "Non-Hodgkins Lymphoma").Where(x => x.PRVTMTYN == "N").Count();
            ViewBag.newcaseper = Math.Round((ViewBag.newcase) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.oldcase = db.fnl_alysis.Where(x => x.System3 == "Non-Hodgkins Lymphoma").Where(x => x.PRVTMTYN == "Y").Count();
            ViewBag.oldcaseper = Math.Round((ViewBag.oldcase) * 100.0 / (ViewBag.TotalCount), 1);
            ViewBag.unkcase = db.fnl_alysis.Where(x => x.System3 == "Non-Hodgkins Lymphoma").Where(x => x.PRVTMTYN == "U").Count();
            ViewBag.unkcaseper = Math.Round((ViewBag.unkcase) * 100.0 / (ViewBag.TotalCount), 1);

            ViewBag.NewTretedTmh = db.fnl_alysis.Where(x => x.System3 == "Non-Hodgkins Lymphoma").Where(x => x.PRVTMTYN == "N" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.NewTretedTmhper = Math.Round((ViewBag.NewTretedTmh) * 100.0 / (ViewBag.newcase), 1);
            ViewBag.oldtreatedtmh = db.fnl_alysis.Where(x => x.System3 == "Non-Hodgkins Lymphoma").Where(x => x.PRVTMTYN == "Y" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.oldtreatedtmhper = Math.Round((ViewBag.oldtreatedtmh) * 100.0 / (ViewBag.oldcase), 1);
            ViewBag.untreatedtmh = db.fnl_alysis.Where(x => x.System3 == "Non-Hodgkins Lymphoma").Where(x => x.PRVTMTYN == "U" && x.SIXTMT > 0).Select(y => y.SIXTMT).Count();
            ViewBag.untreatedtmhper = Math.Round((ViewBag.untreatedtmh) * 100.0 / (ViewBag.unkcase), 1);
            if (double.IsNaN(ViewBag.untreatedtmhper))
            {
                ViewBag.untreatedtmhper = 0;
            }
            ViewBag.totalcase = (ViewBag.newcase + ViewBag.oldcase + ViewBag.unkcase);
            ViewBag.totalcaseper = (ViewBag.newcaseper + ViewBag.oldcaseper + ViewBag.unkcaseper);
            ViewBag.tottreatmh = (ViewBag.NewTretedTmh + ViewBag.oldtreatedtmh + ViewBag.untreatedtmh);
            ViewBag.tottreatmhper = Math.Round((ViewBag.tottreatmh) * 100.0 / (ViewBag.totalcase), 1);


            ViewBag.agedata = db.fnl_alysis.Where(x => x.System3 == "Non-Hodgkins Lymphoma").GroupBy(x => x.BROAD_AGE).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System3 == "Non-Hodgkins Lymphoma").Select(x => x.BROAD_AGE).Count(), 1) }).ToList();
            var objj = db.fnl_alysis.Where(x => x.System3 == "Non-Hodgkins Lymphoma").GroupBy(x => x.final_tti).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System3 == "Non-Hodgkins Lymphoma").Select(x => x.final_tti).Count(), 1) }).ToList();
            ViewBag.timedata = objj.Where(x => x.Key != null);


            //ViewBag.System3Values = db.fnl_alysis.Where(x => x.System4 == "Haematopoetic Cancers" && x.SEX=="1M").GroupBy(x =>new { x.System3,x.SEX }).Select(x =>new { x.Key, Count =x.Count()}).ToList();
            var totalHaematCount = db.fnl_alysis.Count(x => x.System4 == "Haematopoetic Cancers");
            var totalMaleCount = db.fnl_alysis.Count(x => x.System4 == "Haematopoetic Cancers" && x.SEX == "M");
            var totalFemaleCount = db.fnl_alysis.Count(x => x.System4 == "Haematopoetic Cancers" && x.SEX == "F");

            ViewBag.TotalHaematCount = totalHaematCount;
            ViewBag.tMcount = totalMaleCount;
            ViewBag.tFcount = totalFemaleCount;

            ViewBag.system3Values = db.fnl_alysis
                .Where(x => x.System4 == "Haematopoetic Cancers")
                .GroupBy(x => x.System3)
                .Select(g => new System3StatsViewModel
                {
                    System3 = g.Key,
                    MaleCount = g.Count(x => x.SEX == "M"),
                    FemaleCount = g.Count(x => x.SEX == "F"),
                    TotalCount = totalMaleCount,
                    TotalFCount = totalFemaleCount,
                    TotalNosCount = g.Count(),
                    TotalPercentage = (g.Count() * 100.0) / totalHaematCount
                })
                .OrderByDescending(x => x.TotalPercentage) // 👈 Sort here
                .ToList();

            ViewBag.System3 = "Malignant Lymphoma";
            return PartialView("_Non_Hodgkins_Lymphoma", list);
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


            ViewBag.agedata = db.fnl_alysis.Where(x => x.System3 == "Hodgkins Disease").GroupBy(x => x.BROAD_AGE).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System3 == "Hodgkins Disease").Select(x => x.BROAD_AGE).Count(), 1) }).ToList();
            var objj = db.fnl_alysis.Where(x => x.System3 == "Hodgkins Disease").GroupBy(x => x.final_tti).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System3 == "Hodgkins Disease").Select(x => x.final_tti).Count(), 1) }).ToList();
            ViewBag.timedata = objj.Where(x => x.Key != null);

            ViewBag.System3 = "Hodgkins Disease";
            return PartialView("_Hodgkins_Disease", list);
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


            ViewBag.agedata = db.fnl_alysis.Where(x => new[] { "Myeloid  Leukemia", "Lymphoid  Leukemia", "Leukemia Uns" }.Contains(x.System3)).GroupBy(x => x.BROAD_AGE).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => new[] { "Myeloid  Leukemia", "Lymphoid  Leukemia", "Leukemia Uns" }.Contains(x.System3)).Select(x => x.BROAD_AGE).Count(), 1) }).ToList();
            var objj = db.fnl_alysis.Where(x => new[] { "Myeloid  Leukemia", "Lymphoid  Leukemia", "Leukemia Uns" }.Contains(x.System3)).GroupBy(x => x.final_tti).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => new[] { "Myeloid  Leukemia", "Lymphoid  Leukemia", "Leukemia Uns" }.Contains(x.System3)).Select(x => x.final_tti).Count(), 1) }).ToList();
            ViewBag.timedata = objj.Where(x => x.Key != null);

            ViewBag.System3 = "Leukemia";
            return PartialView("_Leukemia", list);
        }


        // ICD-10 TABLE :-
        public ActionResult ICD_10Page()
        {
            return View();
        }

        public ActionResult ICD_10TableIndex()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Age_Site_Distribution()
        {

            ViewBag.Year5Age = db.fnl_alysis.GroupBy(x => x.AGE_GRP).OrderBy(group => group.Key).Select(group => group.Key).ToList();

            ViewBag.Sys = db.fnl_alysis.GroupBy(x => x.Sys).Select(y => y.Key).OrderBy(Sys => Sys).ToList();
            var siteList = db.fnl_alysis.GroupBy(x => new { x.Sys, x.System1 }).OrderBy(g => g.Key.Sys).Select(g => g.Key.System1).ToList();
            ViewBag.Site = siteList;



            var groupedData = db.fnl_alysis.Where(x => x.SEX == "M").GroupBy(x => new { x.System1, x.Sys, x.AGE_GRP }).Select(g => new
            {
                SysValue = g.Key.Sys,   // ICD-10 Code
                SiteName = g.Key.System1 == "Soft Tissue" || g.Key.System1 == "Soft tissue & PNS"
            ? "Soft Tissue & PNS" // Merge both
            : g.Key.System1,      // Keep original for other sites

                AgeGroup = g.Key.AGE_GRP,  // Age Group
                Count = g.Count()           // Count of cases
            }).GroupBy(x => new
            {
                x.SysValue,
                x.SiteName,
                x.AgeGroup
            })
    .Select(g => new
    {
        SysValue = g.Key.SysValue,
        SiteName = g.Key.SiteName,
        AgeGroup = g.Key.AgeGroup,
        Count = g.Sum(x => x.Count) // Sum up counts for merged SiteNames
    })
    .ToList();


            var transformedData = groupedData.GroupBy(g => new { g.SysValue, g.SiteName })
                .Select(g => new
                {
                    SysValue = g.Key.SysValue,
                    SiteName = g.Key.SiteName,
                    AgeGroups = new Dictionary<string, int>
                    {
            { "0-4", g.FirstOrDefault(x => x.AgeGroup == "0-4")?.Count ?? 0 },
            { "05_09", g.FirstOrDefault(x => x.AgeGroup == "05_09")?.Count ?? 0 },
            { "10_14", g.FirstOrDefault(x => x.AgeGroup == "10_14")?.Count ?? 0 },
            { "15-19", g.FirstOrDefault(x => x.AgeGroup == "15-19")?.Count ?? 0 },

            { "20-24", g.FirstOrDefault(x => x.AgeGroup == "20-24")?.Count ?? 0 },
            { "25-29", g.FirstOrDefault(x => x.AgeGroup == "25-29")?.Count ?? 0 },
            { "30-34", g.FirstOrDefault(x => x.AgeGroup == "30-34")?.Count ?? 0 },
            { "35-39", g.FirstOrDefault(x => x.AgeGroup == "35-39")?.Count ?? 0 },

            { "40-44", g.FirstOrDefault(x => x.AgeGroup == "40-44")?.Count ?? 0 },
            { "45-49", g.FirstOrDefault(x => x.AgeGroup == "45-49")?.Count ?? 0 },
            { "50-54", g.FirstOrDefault(x => x.AgeGroup == "50-54")?.Count ?? 0 },
            { "55-59", g.FirstOrDefault(x => x.AgeGroup == "55-59")?.Count ?? 0 },

            { "60-64", g.FirstOrDefault(x => x.AgeGroup == "60-64")?.Count ?? 0 },
            { "65-69", g.FirstOrDefault(x => x.AgeGroup == "65-69")?.Count ?? 0 },
            { "70-74", g.FirstOrDefault(x => x.AgeGroup == "70-74")?.Count ?? 0 },
            { "75+",   g.FirstOrDefault(x => x.AgeGroup == "75+")?.Count ?? 0 },
             },
                    TotalCount = g.Sum(x => x.Count),

                })
                .ToList();
            ViewBag.Year5AgeCounts = transformedData;

            var tc = db.fnl_alysis.Where(x => x.SEX == "M").Count();
            ViewBag.TotalCount = tc;

            return View();
        }


        [HttpGet]
        public ActionResult Age_Site_Distribution_Female()
        {

            ViewBag.Year5Age = db.fnl_alysis.GroupBy(x => x.AGE_GRP).OrderBy(group => group.Key).Select(group => group.Key).ToList();

            ViewBag.Sys = db.fnl_alysis.GroupBy(x => x.Sys).Select(y => y.Key).OrderBy(Sys => Sys).ToList();
            var siteList = db.fnl_alysis.GroupBy(x => new { x.Sys, x.System1 }).OrderBy(g => g.Key.Sys).Select(g => g.Key.System1).ToList();
            ViewBag.Site = siteList;



            var groupedData = db.fnl_alysis.Where(x => x.SEX == "F").GroupBy(x => new { x.System1, x.Sys, x.AGE_GRP }).Select(g => new
            {
                SysValue = g.Key.Sys,   // ICD-10 Code
                SiteName = g.Key.System1 == "Soft Tissue" || g.Key.System1 == "Soft tissue & PNS"
            ? "Soft Tissue & PNS" // Merge both
            : g.Key.System1,      // Keep original for other sites

                AgeGroup = g.Key.AGE_GRP,  // Age Group
                Count = g.Count()           // Count of cases
            }).GroupBy(x => new
            {
                x.SysValue,
                x.SiteName,
                x.AgeGroup
            })
    .Select(g => new
    {
        SysValue = g.Key.SysValue,
        SiteName = g.Key.SiteName,
        AgeGroup = g.Key.AgeGroup,
        Count = g.Sum(x => x.Count) // Sum up counts for merged SiteNames
    })
    .ToList();


            var transformedData = groupedData.GroupBy(g => new { g.SysValue, g.SiteName })
                .Select(g => new
                {
                    SysValue = g.Key.SysValue,
                    SiteName = g.Key.SiteName,
                    AgeGroups = new Dictionary<string, int>
                    {
            { "0-4", g.FirstOrDefault(x => x.AgeGroup == "0-4")?.Count ?? 0 },
            { "05_09", g.FirstOrDefault(x => x.AgeGroup == "05_09")?.Count ?? 0 },
            { "10_14", g.FirstOrDefault(x => x.AgeGroup == "10_14")?.Count ?? 0 },
            { "15-19", g.FirstOrDefault(x => x.AgeGroup == "15-19")?.Count ?? 0 },

            { "20-24", g.FirstOrDefault(x => x.AgeGroup == "20-24")?.Count ?? 0 },
            { "25-29", g.FirstOrDefault(x => x.AgeGroup == "25-29")?.Count ?? 0 },
            { "30-34", g.FirstOrDefault(x => x.AgeGroup == "30-34")?.Count ?? 0 },
            { "35-39", g.FirstOrDefault(x => x.AgeGroup == "35-39")?.Count ?? 0 },

            { "40-44", g.FirstOrDefault(x => x.AgeGroup == "40-44")?.Count ?? 0 },
            { "45-49", g.FirstOrDefault(x => x.AgeGroup == "45-49")?.Count ?? 0 },
            { "50-54", g.FirstOrDefault(x => x.AgeGroup == "50-54")?.Count ?? 0 },
            { "55-59", g.FirstOrDefault(x => x.AgeGroup == "55-59")?.Count ?? 0 },

            { "60-64", g.FirstOrDefault(x => x.AgeGroup == "60-64")?.Count ?? 0 },
            { "65-69", g.FirstOrDefault(x => x.AgeGroup == "65-69")?.Count ?? 0 },
            { "70-74", g.FirstOrDefault(x => x.AgeGroup == "70-74")?.Count ?? 0 },
            { "75+",   g.FirstOrDefault(x => x.AgeGroup == "75+")?.Count ?? 0 },
             },
                    TotalCount = g.Sum(x => x.Count),

                })
                .ToList();
            ViewBag.Year5AgeCounts = transformedData;

            var tc = db.fnl_alysis.Where(x => x.SEX == "F").Count();
            ViewBag.TotalCount = tc;

            return View();
        }

        [HttpGet]
        public ActionResult Method_Dig_Male()
        {

           // ViewBag.Year5Age = db.fnl_alysis.GroupBy(x => x.AGE_GRP).OrderBy(group => group.Key).Select(group => group.Key).ToList();

            ViewBag.Sys = db.fnl_alysis.GroupBy(x => x.Sys).Select(y => y.Key).OrderBy(Sys => Sys).ToList();
            var siteList = db.fnl_alysis.GroupBy(x => new { x.Sys, x.System1 }).OrderBy(g => g.Key.Sys).Select(g => g.Key.System1).ToList();
            ViewBag.Site = siteList;



            var groupedData = db.fnl_alysis.Where(x => x.SEX == "M").GroupBy(x => new { x.System1, x.Sys, x.NMETHOD }).Select(g => new
            {
                SysValue = g.Key.Sys,   // ICD-10 Code
                SiteName = g.Key.System1 == "Soft Tissue" || g.Key.System1 == "Soft tissue & PNS"
            ? "Soft Tissue & PNS" // Merge both
            : g.Key.System1,      // Keep original for other sites

                Method = g.Key.NMETHOD,  // method
                Count = g.Count()           // Count of cases
            }).GroupBy(x => new
            {
                x.SysValue,
                x.SiteName,
                x.Method
            })
    .Select(g => new
    {
        SysValue = g.Key.SysValue,
        SiteName = g.Key.SiteName,
        Method = g.Key.Method,
        Count = g.Sum(x => x.Count) // Sum up counts for merged SiteNames
    })
    .ToList();


            var transformedData = groupedData.GroupBy(g => new { g.SysValue, g.SiteName })
                .Select(g => new
                {
                    SysValue = g.Key.SysValue,
                    SiteName = g.Key.SiteName,
                    Methods = new Dictionary<string, int>
                    {
            { "Clinical", g.FirstOrDefault(x => x.Method == "Clinical")?.Count ?? 0 },
            { "Microscopic", g.FirstOrDefault(x => x.Method == "Microscopic")?.Count ?? 0 },
            { "Radiology", g.FirstOrDefault(x => x.Method == "Radiology")?.Count ?? 0 },
            { "Based on Outside Reports", g.FirstOrDefault(x => x.Method == "Based on Outside Reports")?.Count ?? 0 },
            { "Others", g.FirstOrDefault(x => x.Method == "Others")?.Count ?? 0 },

                   },

                    TotalCount = g.Sum(x => x.Count),

                })
                .ToList();
            ViewBag.Year5AgeCounts = transformedData;

            var tc = db.fnl_alysis.Where(x => x.SEX == "M").Count();
            ViewBag.TotalCount = tc;

            return View();
        }

        [HttpGet]
        public ActionResult Method_Dig_Female()
        {

           // ViewBag.Year5Age = db.fnl_alysis.GroupBy(x => x.AGE_GRP).OrderBy(group => group.Key).Select(group => group.Key).ToList();

            ViewBag.Sys = db.fnl_alysis.GroupBy(x => x.Sys).Select(y => y.Key).OrderBy(Sys => Sys).ToList();
            var siteList = db.fnl_alysis.GroupBy(x => new { x.Sys, x.System1 }).OrderBy(g => g.Key.Sys).Select(g => g.Key.System1).ToList();
            ViewBag.Site = siteList;



            var groupedData = db.fnl_alysis.Where(x => x.SEX == "F").GroupBy(x => new { x.System1, x.Sys, x.NMETHOD }).Select(g => new
            {
                SysValue = g.Key.Sys,   // ICD-10 Code
                SiteName = g.Key.System1 == "Soft Tissue" || g.Key.System1 == "Soft tissue & PNS"
            ? "Soft Tissue & PNS" // Merge both
            : g.Key.System1,      // Keep original for other sites

                Method = g.Key.NMETHOD,  // method
                Count = g.Count()           // Count of cases
            }).GroupBy(x => new
            {
                x.SysValue,
                x.SiteName,
                x.Method
            })
    .Select(g => new
    {
        SysValue = g.Key.SysValue,
        SiteName = g.Key.SiteName,
        Method = g.Key.Method,
        Count = g.Sum(x => x.Count) // Sum up counts for merged SiteNames
    })
    .ToList();


            var transformedData = groupedData.GroupBy(g => new { g.SysValue, g.SiteName })
                .Select(g => new
                {
                    SysValue = g.Key.SysValue,
                    SiteName = g.Key.SiteName,
                    Methods = new Dictionary<string, int>
                    {
            { "Clinical", g.FirstOrDefault(x => x.Method == "Clinical")?.Count ?? 0 },
            { "Microscopic", g.FirstOrDefault(x => x.Method == "Microscopic")?.Count ?? 0 },
            { "Radiology", g.FirstOrDefault(x => x.Method == "Radiology")?.Count ?? 0 },
            { "Based on Outside Reports", g.FirstOrDefault(x => x.Method == "Based on Outside Reports")?.Count ?? 0 },
            { "Others", g.FirstOrDefault(x => x.Method == "Others")?.Count ?? 0 },

                   },

                    TotalCount = g.Sum(x => x.Count),

                })
                .ToList();
            ViewBag.Year5AgeCounts = transformedData;

            var tc = db.fnl_alysis.Where(x => x.SEX == "F").Count();
            ViewBag.TotalCount = tc;

            return View();
        }


        [HttpGet]
        public ActionResult MotherTongueSitewiseMale()
        {
            //   ViewBag.Year5Age = db.fnl_alysis.GroupBy(x => x.AGE_GRP).OrderBy(group => group.Key).Select(group => group.Key).ToList();

            ViewBag.Sys = db.fnl_alysis.GroupBy(x => x.Sys).Select(y => y.Key).OrderBy(Sys => Sys).ToList();
            var siteList = db.fnl_alysis.GroupBy(x => new { x.Sys, x.System1 }).OrderBy(g => g.Key.Sys).Select(g => g.Key.System1).ToList();
            ViewBag.Site = siteList;



            var groupedData = db.fnl_alysis.Where(x => x.SEX == "M").GroupBy(x => new { x.System1, x.Sys, x.MOTHER_TONGUE }).Select(g => new
            {
                SysValue = g.Key.Sys,   // ICD-10 Code
                SiteName = g.Key.System1 == "Soft Tissue" || g.Key.System1 == "Soft tissue & PNS"
            ? "Soft Tissue & PNS" // Merge both
            : g.Key.System1,      // Keep original for other sites

                Method = g.Key.MOTHER_TONGUE,  // method
                Count = g.Count()           // Count of cases
            }).GroupBy(x => new
            {
                x.SysValue,
                x.SiteName,
                x.Method
            })
    .Select(g => new
    {
        SysValue = g.Key.SysValue,
        SiteName = g.Key.SiteName,
        Method = g.Key.Method,
        Count = g.Sum(x => x.Count) // Sum up counts for merged SiteNames
    })
    .ToList();


            var transformedData = groupedData.GroupBy(g => new { g.SysValue, g.SiteName })
                .Select(g => new
                {
                    SysValue = g.Key.SysValue,
                    SiteName = g.Key.SiteName,
                    Methods = new Dictionary<string, int>
                    {
            { "Assamese", g.FirstOrDefault(x => x.Method == "Assamese")?.Count ?? 0 },
            { "Bengali", g.FirstOrDefault(x => x.Method == "Bengali")?.Count ?? 0 },
            { "Gujarathi", g.FirstOrDefault(x => x.Method == "Gujarathi")?.Count ?? 0 },
            { "Hindi", g.FirstOrDefault(x => x.Method == "Hindi")?.Count ?? 0 },
            { "Kannada", g.FirstOrDefault(x => x.Method == "Kannada")?.Count ?? 0 },
            { "Kashmiri", g.FirstOrDefault(x => x.Method == "Kashmiri")?.Count ?? 0 },
            { "Malayalam", g.FirstOrDefault(x => x.Method == "Malayalam")?.Count ?? 0 },
            { "Marathi", g.FirstOrDefault(x => x.Method == "Marathi")?.Count ?? 0 },
            { "Oriya", g.FirstOrDefault(x => x.Method == "Oriya")?.Count ?? 0 },
            { "Punjabi", g.FirstOrDefault(x => x.Method == "Punjabi")?.Count ?? 0 },
            { "Sanskrit", g.FirstOrDefault(x => x.Method == "Sanskrit")?.Count ?? 0 },
            { "Sindhi", g.FirstOrDefault(x => x.Method == "Sindhi")?.Count ?? 0 },

            { "Tamil", g.FirstOrDefault(x => x.Method == "Tamil")?.Count ?? 0 },
            { "Telugu", g.FirstOrDefault(x => x.Method == "Telugu")?.Count ?? 0 },
            { "Urdu", g.FirstOrDefault(x => x.Method == "Urdu")?.Count ?? 0 },
            { "English", g.FirstOrDefault(x => x.Method == "English")?.Count ?? 0 },

            { "Tulu", g.FirstOrDefault(x => x.Method == "Tulu")?.Count ?? 0 },
            { "Konkani", g.FirstOrDefault(x => x.Method == "Konkani")?.Count ?? 0 },
            { "Rajasthani", g.FirstOrDefault(x => x.Method == "Rajasthani")?.Count ?? 0 },
            { "Marwari", g.FirstOrDefault(x => x.Method == "Marwari")?.Count ?? 0 },
            { "Nepali", g.FirstOrDefault(x => x.Method == "Nepali")?.Count ?? 0 },
             { "Others", g.FirstOrDefault(x => x.Method == "Others")?.Count ?? 0 },
            { "Unknown", g.FirstOrDefault(x => x.Method == "Unknown")?.Count ?? 0 },
                   },

                    TotalCount = g.Sum(x => x.Count),

                })
                .ToList();
            ViewBag.Year5AgeCounts = transformedData;

            var tc = db.fnl_alysis.Where(x => x.SEX == "M").Count();
            ViewBag.TotalCount = tc;

            return View();
        }

        [HttpGet]
        public ActionResult MotherTongueSitewiseFemale()
        {
            //ViewBag.Year5Age = db.fnl_alysis.GroupBy(x => x.AGE_GRP).OrderBy(group => group.Key).Select(group => group.Key).ToList();

            ViewBag.Sys = db.fnl_alysis.GroupBy(x => x.Sys).Select(y => y.Key).OrderBy(Sys => Sys).ToList();
            var siteList = db.fnl_alysis.GroupBy(x => new { x.Sys, x.System1 }).OrderBy(g => g.Key.Sys).Select(g => g.Key.System1).ToList();
            ViewBag.Site = siteList;



            var groupedData = db.fnl_alysis.Where(x => x.SEX == "F").GroupBy(x => new { x.System1, x.Sys, x.MOTHER_TONGUE }).Select(g => new
            {
                SysValue = g.Key.Sys,   // ICD-10 Code
                SiteName = g.Key.System1 == "Soft Tissue" || g.Key.System1 == "Soft tissue & PNS"
            ? "Soft Tissue & PNS" // Merge both
            : g.Key.System1,      // Keep original for other sites

                Method = g.Key.MOTHER_TONGUE,  // method
                Count = g.Count()           // Count of cases
            }).GroupBy(x => new
            {
                x.SysValue,
                x.SiteName,
                x.Method
            })
    .Select(g => new
    {
        SysValue = g.Key.SysValue,
        SiteName = g.Key.SiteName,
        Method = g.Key.Method,
        Count = g.Sum(x => x.Count) // Sum up counts for merged SiteNames

    })
    .ToList();


            var transformedData = groupedData.GroupBy(g => new { g.SysValue, g.SiteName })
                .Select(g => new
                {
                    SysValue = g.Key.SysValue,
                    SiteName = g.Key.SiteName,
                    Methods = new Dictionary<string, int>
                    {
            { "Assamese", g.FirstOrDefault(x => x.Method == "Assamese")?.Count ?? 0 },
            { "Bengali", g.FirstOrDefault(x => x.Method == "Bengali")?.Count ?? 0 },
            { "Gujarathi", g.FirstOrDefault(x => x.Method == "Gujarathi")?.Count ?? 0 },
            { "Hindi", g.FirstOrDefault(x => x.Method == "Hindi")?.Count ?? 0 },
            { "Kannada", g.FirstOrDefault(x => x.Method == "Kannada")?.Count ?? 0 },
            { "Kashmiri", g.FirstOrDefault(x => x.Method == "Kashmiri")?.Count ?? 0 },
            { "Malayalam", g.FirstOrDefault(x => x.Method == "Malayalam")?.Count ?? 0 },
            { "Marathi", g.FirstOrDefault(x => x.Method == "Marathi")?.Count ?? 0 },
            { "Oriya", g.FirstOrDefault(x => x.Method == "Oriya")?.Count ?? 0 },
            { "Punjabi", g.FirstOrDefault(x => x.Method == "Punjabi")?.Count ?? 0 },
            { "Sanskrit", g.FirstOrDefault(x => x.Method == "Sanskrit")?.Count ?? 0 },
            { "Sindhi", g.FirstOrDefault(x => x.Method == "Sindhi")?.Count ?? 0 },

            { "Tamil", g.FirstOrDefault(x => x.Method == "Tamil")?.Count ?? 0 },
            { "Telugu", g.FirstOrDefault(x => x.Method == "Telugu")?.Count ?? 0 },
            { "Urdu", g.FirstOrDefault(x => x.Method == "Urdu")?.Count ?? 0 },
            { "English", g.FirstOrDefault(x => x.Method == "English")?.Count ?? 0 },

            { "Tulu", g.FirstOrDefault(x => x.Method == "Tulu")?.Count ?? 0 },
            { "Konkani", g.FirstOrDefault(x => x.Method == "Konkani")?.Count ?? 0 },
            { "Rajasthani", g.FirstOrDefault(x => x.Method == "Rajasthani")?.Count ?? 0 },
            { "Marwari", g.FirstOrDefault(x => x.Method == "Marwari")?.Count ?? 0 },
            { "Nepali", g.FirstOrDefault(x => x.Method == "Nepali")?.Count ?? 0 },
             { "Others", g.FirstOrDefault(x => x.Method == "Others")?.Count ?? 0 },
            { "Unknown", g.FirstOrDefault(x => x.Method == "Unknown")?.Count ?? 0 },
                   },

                    TotalCount = g.Sum(x => x.Count),

                })
                .ToList();
            ViewBag.Year5AgeCounts = transformedData;

            var tc = db.fnl_alysis.Where(x => x.SEX == "F").Count();
            ViewBag.TotalCount = tc;

            return View();
        }

        [HttpGet]
        public ActionResult EducationSitewiseMale()
        {
            // ViewBag.Year5Age = db.fnl_alysis.GroupBy(x => x.AGE_GRP).OrderBy(group => group.Key).Select(group => group.Key).ToList();

            ViewBag.Sys = db.fnl_alysis.GroupBy(x => x.Sys).Select(y => y.Key).OrderBy(Sys => Sys).ToList();
            var siteList = db.fnl_alysis.GroupBy(x => new { x.Sys, x.System1 }).OrderBy(g => g.Key.Sys).Select(g => g.Key.System1).ToList();
            ViewBag.Site = siteList;



            var groupedData = db.fnl_alysis.Where(x => x.SEX == "M").GroupBy(x => new { x.System1, x.Sys, x.EDU_DET }).Select(g => new
            {
                SysValue = g.Key.Sys,   // ICD-10 Code
                SiteName = g.Key.System1 == "Soft Tissue" || g.Key.System1 == "Soft tissue & PNS"
            ? "Soft Tissue & PNS" // Merge both
            : g.Key.System1,      // Keep original for other sites

                Method = g.Key.EDU_DET,  // method
                Count = g.Count()           // Count of cases
            }).GroupBy(x => new
            {
                x.SysValue,
                x.SiteName,
                x.Method
            })
    .Select(g => new
    {
        SysValue = g.Key.SysValue,
        SiteName = g.Key.SiteName,
        Method = g.Key.Method,
        Count = g.Sum(x => x.Count) // Sum up counts for merged SiteNames
    })
    .ToList();


            var transformedData = groupedData.GroupBy(g => new { g.SysValue, g.SiteName })
                .Select(g => new
                {
                    SysValue = g.Key.SysValue,
                    SiteName = g.Key.SiteName,
                    Methods = new Dictionary<string, int>
                    {
            { "Illiterate", g.FirstOrDefault(x => x.Method == "Illiterate")?.Count ?? 0 },
            { "Literate", g.FirstOrDefault(x => x.Method == "Literate")?.Count ?? 0 },
            { "Primary", g.FirstOrDefault(x => x.Method == "Primary")?.Count ?? 0 },
            { "Middle", g.FirstOrDefault(x => x.Method == "Middle")?.Count ?? 0 },
            { "Secondary", g.FirstOrDefault(x => x.Method == "Secondary")?.Count ?? 0 },
            { "Technical", g.FirstOrDefault(x => x.Method == "Technical")?.Count ?? 0 },
            { "College & Above", g.FirstOrDefault(x => x.Method == "College & Above")?.Count ?? 0 },
            { "<5Yrs Old", g.FirstOrDefault(x => x.Method == "<5Yrs Old")?.Count ?? 0 },
            { "Others", g.FirstOrDefault(x => x.Method == "Others")?.Count ?? 0 },
            { "Unknown", g.FirstOrDefault(x => x.Method == "Unknown")?.Count ?? 0 },
                   },

                    TotalCount = g.Sum(x => x.Count),

                })
                .ToList();
            ViewBag.Year5AgeCounts = transformedData;

            var tc = db.fnl_alysis.Where(x => x.SEX == "M").Count();
            ViewBag.TotalCount = tc;

            return View();
        }

        [HttpGet]
        public ActionResult EducationSitewiseFemale()
        {
            // ViewBag.Year5Age = db.fnl_alysis.GroupBy(x => x.AGE_GRP).OrderBy(group => group.Key).Select(group => group.Key).ToList();

            ViewBag.Sys = db.fnl_alysis.GroupBy(x => x.Sys).Select(y => y.Key).OrderBy(Sys => Sys).ToList();
            var siteList = db.fnl_alysis.GroupBy(x => new { x.Sys, x.System1 }).OrderBy(g => g.Key.Sys).Select(g => g.Key.System1).ToList();
            ViewBag.Site = siteList;



            var groupedData = db.fnl_alysis.Where(x => x.SEX == "F").GroupBy(x => new { x.System1, x.Sys, x.EDU_DET }).Select(g => new
            {
                SysValue = g.Key.Sys,   // ICD-10 Code
                SiteName = g.Key.System1 == "Soft Tissue" || g.Key.System1 == "Soft tissue & PNS"
            ? "Soft Tissue & PNS" // Merge both
            : g.Key.System1,      // Keep original for other sites

                Method = g.Key.EDU_DET,  // method
                Count = g.Count()           // Count of cases
            }).GroupBy(x => new
            {
                x.SysValue,
                x.SiteName,
                x.Method
            })
    .Select(g => new
    {
        SysValue = g.Key.SysValue,
        SiteName = g.Key.SiteName,
        Method = g.Key.Method,
        Count = g.Sum(x => x.Count) // Sum up counts for merged SiteNames
    })
    .ToList();


            var transformedData = groupedData.GroupBy(g => new { g.SysValue, g.SiteName })
                .Select(g => new
                {
                    SysValue = g.Key.SysValue,
                    SiteName = g.Key.SiteName,
                    Methods = new Dictionary<string, int>
                    {
            { "Illiterate", g.FirstOrDefault(x => x.Method == "Illiterate")?.Count ?? 0 },
            { "Literate", g.FirstOrDefault(x => x.Method == "Literate")?.Count ?? 0 },
            { "Primary", g.FirstOrDefault(x => x.Method == "Primary")?.Count ?? 0 },
            { "Middle", g.FirstOrDefault(x => x.Method == "Middle")?.Count ?? 0 },
            { "Secondary", g.FirstOrDefault(x => x.Method == "Secondary")?.Count ?? 0 },
            { "Technical", g.FirstOrDefault(x => x.Method == "Technical")?.Count ?? 0 },
            { "College & Above", g.FirstOrDefault(x => x.Method == "College & Above")?.Count ?? 0 },
            { "<5Yrs Old", g.FirstOrDefault(x => x.Method == "<5Yrs Old")?.Count ?? 0 },
            { "Others", g.FirstOrDefault(x => x.Method == "Others")?.Count ?? 0 },
            { "Unknown", g.FirstOrDefault(x => x.Method == "Unknown")?.Count ?? 0 },
                   },

                    TotalCount = g.Sum(x => x.Count),

                })
                .ToList();
            ViewBag.Year5AgeCounts = transformedData;

            var tc = db.fnl_alysis.Where(x => x.SEX == "F").Count();
            ViewBag.TotalCount = tc;

            return View();
        }

        [HttpGet]
        public ActionResult ReligionSitewiseMale()
        {
          //  ViewBag.Year5Age = db.fnl_alysis.GroupBy(x => x.AGE_GRP).OrderBy(group => group.Key).Select(group => group.Key).ToList();

            ViewBag.Sys = db.fnl_alysis.GroupBy(x => x.Sys).Select(y => y.Key).OrderBy(Sys => Sys).ToList();
            var siteList = db.fnl_alysis.GroupBy(x => new { x.Sys, x.System1 }).OrderBy(g => g.Key.Sys).Select(g => g.Key.System1).ToList();
            ViewBag.Site = siteList;



            var groupedData = db.fnl_alysis.Where(x => x.SEX == "M").GroupBy(x => new { x.System1, x.Sys, x.REL_DET }).Select(g => new
            {
                SysValue = g.Key.Sys,   // ICD-10 Code
                SiteName = g.Key.System1 == "Soft Tissue" || g.Key.System1 == "Soft tissue & PNS"
            ? "Soft Tissue & PNS" // Merge both
            : g.Key.System1,      // Keep original for other sites

                Method = g.Key.REL_DET,  // method
                Count = g.Count()           // Count of cases
            }).GroupBy(x => new
            {
                x.SysValue,
                x.SiteName,
                x.Method
            })
    .Select(g => new
    {
        SysValue = g.Key.SysValue,
        SiteName = g.Key.SiteName,
        Method = g.Key.Method,
        Count = g.Sum(x => x.Count) // Sum up counts for merged SiteNames
    })
    .ToList();


            var transformedData = groupedData.GroupBy(g => new { g.SysValue, g.SiteName })
                .Select(g => new
                {
                    SysValue = g.Key.SysValue,
                    SiteName = g.Key.SiteName,
                    Methods = new Dictionary<string, int>
                    {
            { "Hindu", g.FirstOrDefault(x => x.Method == "Hindu")?.Count ?? 0 },
            { "Muslim", g.FirstOrDefault(x => x.Method == "Muslim")?.Count ?? 0 },
            { "Christian", g.FirstOrDefault(x => x.Method == "Christian")?.Count ?? 0 },
            { "Sikh", g.FirstOrDefault(x => x.Method == "Sikh")?.Count ?? 0 },
            { "Jain", g.FirstOrDefault(x => x.Method == "Jain")?.Count ?? 0 },
             { "Neo-Buddhist", g.FirstOrDefault(x => x.Method == "Neo-Buddhist")?.Count ?? 0 },
            { "Parsi", g.FirstOrDefault(x => x.Method == "Parsi")?.Count ?? 0 },
            { "Jew", g.FirstOrDefault(x => x.Method == "Jew")?.Count ?? 0 },
            { "Anglo Indian", g.FirstOrDefault(x => x.Method == "Anglo Indian")?.Count ?? 0 },
            { "Others", g.FirstOrDefault(x => x.Method == "Others")?.Count ?? 0 },
             { "Unknown", g.FirstOrDefault(x => x.Method == "Unknown")?.Count ?? 0 },
                   },

                    TotalCount = g.Sum(x => x.Count),

                })
                .ToList();
            ViewBag.Year5AgeCounts = transformedData;

            var tc = db.fnl_alysis.Where(x => x.SEX == "M").Count();
            ViewBag.TotalCount = tc;

            return View();
        }

        [HttpGet]
        public ActionResult ReligionSitewiseFemale()
        {
           // ViewBag.Year5Age = db.fnl_alysis.GroupBy(x => x.AGE_GRP).OrderBy(group => group.Key).Select(group => group.Key).ToList();

            ViewBag.Sys = db.fnl_alysis.GroupBy(x => x.Sys).Select(y => y.Key).OrderBy(Sys => Sys).ToList();
            var siteList = db.fnl_alysis.GroupBy(x => new { x.Sys, x.System1 }).OrderBy(g => g.Key.Sys).Select(g => g.Key.System1).ToList();
            ViewBag.Site = siteList;



            var groupedData = db.fnl_alysis.Where(x => x.SEX == "F").GroupBy(x => new { x.System1, x.Sys, x.REL_DET }).Select(g => new
            {
                SysValue = g.Key.Sys,   // ICD-10 Code
                SiteName = g.Key.System1 == "Soft Tissue" || g.Key.System1 == "Soft tissue & PNS"
            ? "Soft Tissue & PNS" // Merge both
            : g.Key.System1,      // Keep original for other sites

                Method = g.Key.REL_DET,  // method
                Count = g.Count()           // Count of cases
            }).GroupBy(x => new
            {
                x.SysValue,
                x.SiteName,
                x.Method
            })
    .Select(g => new
    {
        SysValue = g.Key.SysValue,
        SiteName = g.Key.SiteName,
        Method = g.Key.Method,
        Count = g.Sum(x => x.Count) // Sum up counts for merged SiteNames
    })
    .ToList();


            var transformedData = groupedData.GroupBy(g => new { g.SysValue, g.SiteName })
                .Select(g => new
                {
                    SysValue = g.Key.SysValue,
                    SiteName = g.Key.SiteName,
                    Methods = new Dictionary<string, int>
                    {
            { "Hindu", g.FirstOrDefault(x => x.Method == "Hindu")?.Count ?? 0 },
            { "Muslim", g.FirstOrDefault(x => x.Method == "Muslim")?.Count ?? 0 },
            { "Christian", g.FirstOrDefault(x => x.Method == "Christian")?.Count ?? 0 },
            { "Sikh", g.FirstOrDefault(x => x.Method == "Sikh")?.Count ?? 0 },
            { "Jain", g.FirstOrDefault(x => x.Method == "Jain")?.Count ?? 0 },
             { "Neo-Buddhist", g.FirstOrDefault(x => x.Method == "Neo-Buddhist")?.Count ?? 0 },
            { "Parsi", g.FirstOrDefault(x => x.Method == "Parsi")?.Count ?? 0 },
            { "Jew", g.FirstOrDefault(x => x.Method == "Jew")?.Count ?? 0 },
            { "Anglo Indian", g.FirstOrDefault(x => x.Method == "Anglo Indian")?.Count ?? 0 },
            { "Others", g.FirstOrDefault(x => x.Method == "Others")?.Count ?? 0 },
             { "Unknown", g.FirstOrDefault(x => x.Method == "Unknown")?.Count ?? 0 },
                   },

                    TotalCount = g.Sum(x => x.Count),

                })
                .ToList();
            ViewBag.Year5AgeCounts = transformedData;

            var tc = db.fnl_alysis.Where(x => x.SEX == "F").Count();
            ViewBag.TotalCount = tc;

            return View();
        }

        [HttpGet]
        public ActionResult TreatmentReceivedBeforeRegisMale()
        {


            var groupedData = db.fnl_alysis.Where(x => x.PRVTMTYN == "Y" && x.SEX == "M").GroupBy(x => new { x.System1, x.Sys, x.TREATMENT_Before }).Select(g => new
            {
                SysValue = g.Key.Sys,   // ICD-10 Code
                SiteName = g.Key.System1 == "Soft Tissue" || g.Key.System1 == "Soft tissue & PNS"
                   ? "Soft Tissue & PNS" // Merge both
                   : g.Key.System1,      // Keep original for other sites

                AgeGroup = g.Key.TREATMENT_Before,  // Age Group
                Count = g.Count()           // Count of cases
            }).GroupBy(x => new
            {
                x.SysValue,
                x.SiteName,
                x.AgeGroup
            })
    .Select(g => new
    {
        SysValue = g.Key.SysValue,
        SiteName = g.Key.SiteName,
        AgeGroup = g.Key.AgeGroup,
        Count = g.Sum(x => x.Count) // Sum up counts for merged SiteNames
    })
    .ToList();


            var transformedData = groupedData.GroupBy(g => new { g.SysValue, g.SiteName })
                .Select(g => new
                {
                    SysValue = g.Key.SysValue,
                    SiteName = g.Key.SiteName,
                    AgeGroups = new Dictionary<string, int>
                    {
            { "S", g.FirstOrDefault(x => x.AgeGroup == "S")?.Count ?? 0 },
            { "R", g.FirstOrDefault(x => x.AgeGroup == "R")?.Count ?? 0 },
            { "C", g.FirstOrDefault(x => x.AgeGroup == "C")?.Count ?? 0 },
            { "SR", g.FirstOrDefault(x => x.AgeGroup == "SR")?.Count ?? 0 },

            { "SC", g.FirstOrDefault(x => x.AgeGroup == "SC")?.Count ?? 0 },
            { "RC", g.FirstOrDefault(x => x.AgeGroup == "RC")?.Count ?? 0 },
            { "SRC", g.FirstOrDefault(x => x.AgeGroup == "SRC")?.Count ?? 0 },
            { "HT", g.FirstOrDefault(x => x.AgeGroup == "HT")?.Count ?? 0 },

            { "SH", g.FirstOrDefault(x => x.AgeGroup == "SH")?.Count ?? 0 },
            { "RH", g.FirstOrDefault(x => x.AgeGroup == "RH")?.Count ?? 0 },
            { "CH", g.FirstOrDefault(x => x.AgeGroup == "CH")?.Count ?? 0 },
            { "SRH", g.FirstOrDefault(x => x.AgeGroup == "SRH")?.Count ?? 0 },

            { "SCH", g.FirstOrDefault(x => x.AgeGroup == "SCH")?.Count ?? 0 },
            { "RCH", g.FirstOrDefault(x => x.AgeGroup == "RCH")?.Count ?? 0 },
            { "SRCH", g.FirstOrDefault(x => x.AgeGroup == "SRCH")?.Count ?? 0 },

             },
                    TotalCount = g.Sum(x => x.Count),

                })
                .ToList();
            ViewBag.Year5AgeCounts = transformedData;

            var tc = db.fnl_alysis.Where(x => x.SEX == "M" && x.PRVTMTYN == "Y").Count();
            ViewBag.TotalCount = tc;

            return View();
        }

        [HttpGet]
        public ActionResult TreatmentReceivedBeforeRegisFemale()
        {


            var groupedData = db.fnl_alysis.Where(x => x.PRVTMTYN == "Y" && x.SEX == "F").GroupBy(x => new { x.System1, x.Sys, x.TREATMENT_Before }).Select(g => new
            {
                SysValue = g.Key.Sys,   // ICD-10 Code
                SiteName = g.Key.System1 == "Soft Tissue" || g.Key.System1 == "Soft tissue & PNS"
                   ? "Soft Tissue & PNS" // Merge both
                   : g.Key.System1,      // Keep original for other sites

                AgeGroup = g.Key.TREATMENT_Before,  // Age Group
                Count = g.Count()           // Count of cases
            }).GroupBy(x => new
            {
                x.SysValue,
                x.SiteName,
                x.AgeGroup
            })
    .Select(g => new
    {
        SysValue = g.Key.SysValue,
        SiteName = g.Key.SiteName,
        AgeGroup = g.Key.AgeGroup,
        Count = g.Sum(x => x.Count) // Sum up counts for merged SiteNames
    })
    .ToList();


            var transformedData = groupedData.GroupBy(g => new { g.SysValue, g.SiteName })
                .Select(g => new
                {
                    SysValue = g.Key.SysValue,
                    SiteName = g.Key.SiteName,
                    AgeGroups = new Dictionary<string, int>
                    {
            { "S", g.FirstOrDefault(x => x.AgeGroup == "S")?.Count ?? 0 },
            { "R", g.FirstOrDefault(x => x.AgeGroup == "R")?.Count ?? 0 },
            { "C", g.FirstOrDefault(x => x.AgeGroup == "C")?.Count ?? 0 },
            { "SR", g.FirstOrDefault(x => x.AgeGroup == "SR")?.Count ?? 0 },

            { "SC", g.FirstOrDefault(x => x.AgeGroup == "SC")?.Count ?? 0 },
            { "RC", g.FirstOrDefault(x => x.AgeGroup == "RC")?.Count ?? 0 },
            { "SRC", g.FirstOrDefault(x => x.AgeGroup == "SRC")?.Count ?? 0 },
            { "HT", g.FirstOrDefault(x => x.AgeGroup == "HT")?.Count ?? 0 },

            { "SH", g.FirstOrDefault(x => x.AgeGroup == "SH")?.Count ?? 0 },
            { "RH", g.FirstOrDefault(x => x.AgeGroup == "RH")?.Count ?? 0 },
            { "CH", g.FirstOrDefault(x => x.AgeGroup == "CH")?.Count ?? 0 },
            { "SRH", g.FirstOrDefault(x => x.AgeGroup == "SRH")?.Count ?? 0 },

            { "SCH", g.FirstOrDefault(x => x.AgeGroup == "SCH")?.Count ?? 0 },
            { "RCH", g.FirstOrDefault(x => x.AgeGroup == "RCH")?.Count ?? 0 },
            { "SRCH", g.FirstOrDefault(x => x.AgeGroup == "SRCH")?.Count ?? 0 },

             },
                    TotalCount = g.Sum(x => x.Count),

                })
                .ToList();
            ViewBag.Year5AgeCounts = transformedData;

            var tc = db.fnl_alysis.Where(x => x.SEX == "F" && x.PRVTMTYN == "Y").Count();
            ViewBag.TotalCount = tc;

            return View();
        }

        [HttpGet]
        public ActionResult TreatmentMumbaiNewCases()
        {
            // ViewBag.Year5Age = db.fnl_alysis.GroupBy(x => x.AGE_GRP).OrderBy(group => group.Key).Select(group => group.Key).ToList();

            ViewBag.Sys = db.fnl_alysis.GroupBy(x => x.Sys).Select(y => y.Key).OrderBy(Sys => Sys).ToList();
            var siteList = db.fnl_alysis.GroupBy(x => new { x.Sys, x.System1 }).OrderBy(g => g.Key.Sys).Select(g => g.Key.System1).ToList();
            ViewBag.Site = siteList;



            var groupedData = db.fnl_alysis.Where(x => x.SEX == "M" && x.PRVTMTYN == "N" && x.SIXTMT != 0).GroupBy(x => new { x.System1, x.Sys, x.TREATMENT_MUM_All }).Select(g => new
            {
                SysValue = g.Key.Sys,   // ICD-10 Code
                SiteName = g.Key.System1 == "Soft Tissue" || g.Key.System1 == "Soft tissue & PNS"
                ? "Soft Tissue & PNS" // Merge both
                : g.Key.System1,      // Keep original for other sites

                AgeGroup = g.Key.TREATMENT_MUM_All,  // Age Group
                Count = g.Count()           // Count of cases
            }).GroupBy(x => new
            {
                x.SysValue,
                x.SiteName,
                x.AgeGroup
            })
    .Select(g => new
    {
        SysValue = g.Key.SysValue,
        SiteName = g.Key.SiteName,
        AgeGroup = g.Key.AgeGroup,
        Count = g.Sum(x => x.Count) // Sum up counts for merged SiteNames
    })
    .ToList();


            var transformedData = groupedData.GroupBy(g => new { g.SysValue, g.SiteName })
                .Select(g => new
                {
                    SysValue = g.Key.SysValue,
                    SiteName = g.Key.SiteName,
                    AgeGroups = new Dictionary<string, int>
                    {
            { "S", g.FirstOrDefault(x => x.AgeGroup == "S")?.Count ?? 0 },
            { "R", g.FirstOrDefault(x => x.AgeGroup == "R")?.Count ?? 0 },
            { "C", g.FirstOrDefault(x => x.AgeGroup == "C")?.Count ?? 0 },
            { "SR", g.FirstOrDefault(x => x.AgeGroup == "SR")?.Count ?? 0 },

            { "SC", g.FirstOrDefault(x => x.AgeGroup == "SC")?.Count ?? 0 },
            { "RC", g.FirstOrDefault(x => x.AgeGroup == "RC")?.Count ?? 0 },
            { "SRC", g.FirstOrDefault(x => x.AgeGroup == "SRC")?.Count ?? 0 },
            { "HT", g.FirstOrDefault(x => x.AgeGroup == "HT")?.Count ?? 0 },

            { "SH", g.FirstOrDefault(x => x.AgeGroup == "SH")?.Count ?? 0 },
            { "RH", g.FirstOrDefault(x => x.AgeGroup == "RH")?.Count ?? 0 },
            { "CH", g.FirstOrDefault(x => x.AgeGroup == "CH")?.Count ?? 0 },
            { "SRH", g.FirstOrDefault(x => x.AgeGroup == "SRH")?.Count ?? 0 },

            { "SCH", g.FirstOrDefault(x => x.AgeGroup == "SCH")?.Count ?? 0 },
            { "RCH", g.FirstOrDefault(x => x.AgeGroup == "RCH")?.Count ?? 0 },
            { "SRCH", g.FirstOrDefault(x => x.AgeGroup == "SRCH")?.Count ?? 0 },
            { "BMT", g.FirstOrDefault(x => x.AgeGroup == "BMT")?.Count ?? 0 },
             },
                    TotalCount = g.Sum(x => x.Count),

                })
                .ToList();
            ViewBag.Year5AgeCounts = transformedData;

            var tc = db.fnl_alysis.Where(x => x.SEX == "M" && x.PRVTMTYN == "N" && x.SIXTMT != 0).Count();
            ViewBag.TotalCount = tc;

            return View();
        }

        [HttpGet]
        public ActionResult TreatmentMumbaiNewCasesFemale()
        {
            // ViewBag.Year5Age = db.fnl_alysis.GroupBy(x => x.AGE_GRP).OrderBy(group => group.Key).Select(group => group.Key).ToList();

            ViewBag.Sys = db.fnl_alysis.GroupBy(x => x.Sys).Select(y => y.Key).OrderBy(Sys => Sys).ToList();
            var siteList = db.fnl_alysis.GroupBy(x => new { x.Sys, x.System1 }).OrderBy(g => g.Key.Sys).Select(g => g.Key.System1).ToList();
            ViewBag.Site = siteList;



            var groupedData = db.fnl_alysis.Where(x => x.SEX == "F" && x.PRVTMTYN == "N" && x.SIXTMT != 0).GroupBy(x => new { x.System1, x.Sys, x.TREATMENT_MUM_All }).Select(g => new
            {
                SysValue = g.Key.Sys,   // ICD-10 Code
                SiteName = g.Key.System1 == "Soft Tissue" || g.Key.System1 == "Soft tissue & PNS"
                ? "Soft Tissue & PNS" // Merge both
                : g.Key.System1,      // Keep original for other sites

                AgeGroup = g.Key.TREATMENT_MUM_All,  // Age Group
                Count = g.Count()           // Count of cases
            }).GroupBy(x => new
            {
                x.SysValue,
                x.SiteName,
                x.AgeGroup
            })
    .Select(g => new
    {
        SysValue = g.Key.SysValue,
        SiteName = g.Key.SiteName,
        AgeGroup = g.Key.AgeGroup,
        Count = g.Sum(x => x.Count) // Sum up counts for merged SiteNames
    })
    .ToList();


            var transformedData = groupedData.GroupBy(g => new { g.SysValue, g.SiteName })
                .Select(g => new
                {
                    SysValue = g.Key.SysValue,
                    SiteName = g.Key.SiteName,
                    AgeGroups = new Dictionary<string, int>
                    {
            { "S", g.FirstOrDefault(x => x.AgeGroup == "S")?.Count ?? 0 },
            { "R", g.FirstOrDefault(x => x.AgeGroup == "R")?.Count ?? 0 },
            { "C", g.FirstOrDefault(x => x.AgeGroup == "C")?.Count ?? 0 },
            { "SR", g.FirstOrDefault(x => x.AgeGroup == "SR")?.Count ?? 0 },

            { "SC", g.FirstOrDefault(x => x.AgeGroup == "SC")?.Count ?? 0 },
            { "RC", g.FirstOrDefault(x => x.AgeGroup == "RC")?.Count ?? 0 },
            { "SRC", g.FirstOrDefault(x => x.AgeGroup == "SRC")?.Count ?? 0 },
            { "HT", g.FirstOrDefault(x => x.AgeGroup == "HT")?.Count ?? 0 },

            { "SH", g.FirstOrDefault(x => x.AgeGroup == "SH")?.Count ?? 0 },
            { "RH", g.FirstOrDefault(x => x.AgeGroup == "RH")?.Count ?? 0 },
            { "CH", g.FirstOrDefault(x => x.AgeGroup == "CH")?.Count ?? 0 },
            { "SRH", g.FirstOrDefault(x => x.AgeGroup == "SRH")?.Count ?? 0 },

            { "SCH", g.FirstOrDefault(x => x.AgeGroup == "SCH")?.Count ?? 0 },
            { "RCH", g.FirstOrDefault(x => x.AgeGroup == "RCH")?.Count ?? 0 },
            { "SRCH", g.FirstOrDefault(x => x.AgeGroup == "SRCH")?.Count ?? 0 },
            { "BMT", g.FirstOrDefault(x => x.AgeGroup == "BMT")?.Count ?? 0 },
             },
                    TotalCount = g.Sum(x => x.Count),

                })
                .ToList();
            ViewBag.Year5AgeCounts = transformedData;

            var tc = db.fnl_alysis.Where(x => x.SEX == "F" && x.PRVTMTYN == "N" && x.SIXTMT != 0).Count();
            ViewBag.TotalCount = tc;

            return View();
        }



        [HttpGet]
        public ActionResult TreatmentMumbaiAllCases()
        {
           // ViewBag.Year5Age = db.fnl_alysis.GroupBy(x => x.AGE_GRP).OrderBy(group => group.Key).Select(group => group.Key).ToList();

            ViewBag.Sys = db.fnl_alysis.GroupBy(x => x.Sys).Select(y => y.Key).OrderBy(Sys => Sys).ToList();
            var siteList = db.fnl_alysis.GroupBy(x => new { x.Sys, x.System1 }).OrderBy(g => g.Key.Sys).Select(g => g.Key.System1).ToList();
            ViewBag.Site = siteList;



            var groupedData = db.fnl_alysis.Where(x => x.SEX == "M").GroupBy(x => new { x.System1, x.Sys, x.TREATMENT_MUM_All }).Select(g => new
            {
                SysValue = g.Key.Sys,   // ICD-10 Code
                SiteName = g.Key.System1 == "Soft Tissue" || g.Key.System1 == "Soft tissue & PNS"
            ? "Soft Tissue & PNS" // Merge both
            : g.Key.System1,      // Keep original for other sites

                AgeGroup = g.Key.TREATMENT_MUM_All,  // Age Group
                Count = g.Count()           // Count of cases
            }).GroupBy(x => new
            {
                x.SysValue,
                x.SiteName,
                x.AgeGroup
            })
    .Select(g => new
    {
        SysValue = g.Key.SysValue,
        SiteName = g.Key.SiteName,
        AgeGroup = g.Key.AgeGroup,
        Count = g.Sum(x => x.Count) // Sum up counts for merged SiteNames
    })
    .ToList();


            var transformedData = groupedData.GroupBy(g => new { g.SysValue, g.SiteName })
                .Select(g => new
                {
                    SysValue = g.Key.SysValue,
                    SiteName = g.Key.SiteName,
                    AgeGroups = new Dictionary<string, int>
                    {
            { "S", g.FirstOrDefault(x => x.AgeGroup == "S")?.Count ?? 0 },
            { "R", g.FirstOrDefault(x => x.AgeGroup == "R")?.Count ?? 0 },
            { "C", g.FirstOrDefault(x => x.AgeGroup == "C")?.Count ?? 0 },
            { "SR", g.FirstOrDefault(x => x.AgeGroup == "SR")?.Count ?? 0 },

            { "SC", g.FirstOrDefault(x => x.AgeGroup == "SC")?.Count ?? 0 },
            { "RC", g.FirstOrDefault(x => x.AgeGroup == "RC")?.Count ?? 0 },
            { "SRC", g.FirstOrDefault(x => x.AgeGroup == "SRC")?.Count ?? 0 },
            { "HT", g.FirstOrDefault(x => x.AgeGroup == "HT")?.Count ?? 0 },

            { "SH", g.FirstOrDefault(x => x.AgeGroup == "SH")?.Count ?? 0 },
            { "RH", g.FirstOrDefault(x => x.AgeGroup == "RH")?.Count ?? 0 },
            { "CH", g.FirstOrDefault(x => x.AgeGroup == "CH")?.Count ?? 0 },
            { "SRH", g.FirstOrDefault(x => x.AgeGroup == "SRH")?.Count ?? 0 },

            { "SCH", g.FirstOrDefault(x => x.AgeGroup == "SCH")?.Count ?? 0 },
            { "RCH", g.FirstOrDefault(x => x.AgeGroup == "RCH")?.Count ?? 0 },
            { "SRCH", g.FirstOrDefault(x => x.AgeGroup == "SRCH")?.Count ?? 0 },
            { "BMT", g.FirstOrDefault(x => x.AgeGroup == "BMT")?.Count ?? 0 },
             },
                    TotalCount = g.Sum(x => x.Count),

                })
                .ToList();
            ViewBag.Year5AgeCounts = transformedData;

            var tc = db.fnl_alysis.Where(x => x.SEX == "M" && x.SIXTMT != 0).Count();
            ViewBag.TotalCount = tc;

            return View();
        }


        [HttpGet]
        public ActionResult TreatmentMumbaiAllCasesFemale()
        {
          //  ViewBag.Year5Age = db.fnl_alysis.GroupBy(x => x.AGE_GRP).OrderBy(group => group.Key).Select(group => group.Key).ToList();

            ViewBag.Sys = db.fnl_alysis.GroupBy(x => x.Sys).Select(y => y.Key).OrderBy(Sys => Sys).ToList();
            var siteList = db.fnl_alysis.GroupBy(x => new { x.Sys, x.System1 }).OrderBy(g => g.Key.Sys).Select(g => g.Key.System1).ToList();
            ViewBag.Site = siteList;



            var groupedData = db.fnl_alysis.Where(x => x.SEX == "F").GroupBy(x => new { x.System1, x.Sys, x.TREATMENT_MUM_All }).Select(g => new
            {
                SysValue = g.Key.Sys,   // ICD-10 Code
                SiteName = g.Key.System1 == "Soft Tissue" || g.Key.System1 == "Soft tissue & PNS"
            ? "Soft Tissue & PNS" // Merge both
            : g.Key.System1,      // Keep original for other sites

                AgeGroup = g.Key.TREATMENT_MUM_All,  // Age Group
                Count = g.Count()           // Count of cases
            }).GroupBy(x => new
            {
                x.SysValue,
                x.SiteName,
                x.AgeGroup
            })
    .Select(g => new
    {
        SysValue = g.Key.SysValue,
        SiteName = g.Key.SiteName,
        AgeGroup = g.Key.AgeGroup,
        Count = g.Sum(x => x.Count) // Sum up counts for merged SiteNames
    })
    .ToList();


            var transformedData = groupedData.GroupBy(g => new { g.SysValue, g.SiteName })
                .Select(g => new
                {
                    SysValue = g.Key.SysValue,
                    SiteName = g.Key.SiteName,
                    AgeGroups = new Dictionary<string, int>
                    {
            { "S", g.FirstOrDefault(x => x.AgeGroup == "S")?.Count ?? 0 },
            { "R", g.FirstOrDefault(x => x.AgeGroup == "R")?.Count ?? 0 },
            { "C", g.FirstOrDefault(x => x.AgeGroup == "C")?.Count ?? 0 },
            { "SR", g.FirstOrDefault(x => x.AgeGroup == "SR")?.Count ?? 0 },

            { "SC", g.FirstOrDefault(x => x.AgeGroup == "SC")?.Count ?? 0 },
            { "RC", g.FirstOrDefault(x => x.AgeGroup == "RC")?.Count ?? 0 },
            { "SRC", g.FirstOrDefault(x => x.AgeGroup == "SRC")?.Count ?? 0 },
            { "HT", g.FirstOrDefault(x => x.AgeGroup == "HT")?.Count ?? 0 },

            { "SH", g.FirstOrDefault(x => x.AgeGroup == "SH")?.Count ?? 0 },
            { "RH", g.FirstOrDefault(x => x.AgeGroup == "RH")?.Count ?? 0 },
            { "CH", g.FirstOrDefault(x => x.AgeGroup == "CH")?.Count ?? 0 },
            { "SRH", g.FirstOrDefault(x => x.AgeGroup == "SRH")?.Count ?? 0 },

            { "SCH", g.FirstOrDefault(x => x.AgeGroup == "SCH")?.Count ?? 0 },
            { "RCH", g.FirstOrDefault(x => x.AgeGroup == "RCH")?.Count ?? 0 },
            { "SRCH", g.FirstOrDefault(x => x.AgeGroup == "SRCH")?.Count ?? 0 },
            { "BMT", g.FirstOrDefault(x => x.AgeGroup == "BMT")?.Count ?? 0 },
             },
                    TotalCount = g.Sum(x => x.Count),

                })
                .ToList();
            ViewBag.Year5AgeCounts = transformedData;

            var tc = db.fnl_alysis.Where(x => x.SEX == "F" && x.SIXTMT != 0).Count();
            ViewBag.TotalCount = tc;

            return View();
        }



        //ACTREC PART:-

        //public ActionResult Actrec_registration()
        //{
        //    ViewBag.male = db.fnl_alysis.Where(x => x.INST == "ACTREC" && x.SEX == "M").Count();
        //    ViewBag.female = db.fnl_alysis.Where(x => x.INST == "ACTREC" && x.SEX == "F").Count();
        //    ViewBag.total = db.fnl_alysis.Where(x => x.INST == "ACTREC").Count();
        //    ViewBag.mper = Math.Round((ViewBag.male) * 100.0 / (ViewBag.total), 1);
        //    ViewBag.feper = Math.Round((ViewBag.female) * 100.0 / (ViewBag.total), 1);
        //    var list = db.fnl_alysis.Where(x => x.INST == "ACTREC").GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.INST == "ACTREC").Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Take(5);
        //    var list1 = db.fnl_alysis.Where(x => x.INST == "ACTREC").GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.INST == "ACTREC").Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Skip(5);
        //    ViewBag.other = list1.Sum(y => y.Statecount);
        //    ViewBag.Otherper = Math.Round(list1.Sum(x => x.Statecount) * 100 / db.fnl_alysis.Where(x => x.INST == "ACTREC").Select(x => x.STATE).Count(), 1);
        //    ViewBag.TotalCount = db.fnl_alysis.Where(x => x.INST == "ACTREC").Select(x => x.STATE).Count();

        //    return View(list);
        //}


        //public ActionResult Actrac_Treatment()
        //{
        //   // Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.System3 == "Peripheral Nerves & Soft Tissue").Select(y => y.STATE).Count(), 1) })
        //    var nos = db.fnl_alysis.Where(x => x.INST == "ACTREC" && x.TMH_RX != null).Count();
        //    var list = db.fnl_alysis.Where(x => x.INST == "ACTREC" && x.TMH_RX!=null).GroupBy(y=>y.TMH_RX).
        //    Select(y => new final_analysisViewModel() { State = y.Key, Statecount = y.Count(),Percentage=Math.Round(y.Count()*100.0 / nos,1 )}).ToList();
        //    ViewBag.nos = nos;
        //    var obj = db.fnl_alysis.Where(x => x.INST == "ACTREC" && x.TMH_RX != null).GroupBy(x => x.TMH_RX).Select(y => new { y.Key, Count = y.Count() }).ToList();

        //    ViewBag.objectname = obj.Where(x => x.Key != null);
        //    ViewBag.newactrigs = db.fnl_alysis.Where(x => x.INST == "ACTREC" && x.TMH_RX != null && x.PRVTMTYN == "N").Count();
        //    ViewBag.newactrigsper = Math.Round((ViewBag.newactrigs) * 100.0 / (nos) ,1);
          
        //    ViewBag.prioractrigs = db.fnl_alysis.Where(x => x.INST == "ACTREC" && x.TMH_RX != null && x.PRVTMTYN == "Y").Count();
        //    ViewBag.prioractrigsper = Math.Round((ViewBag.prioractrigs) * 100.0 / (nos), 1);
        //    return View(list);
        //}

      





    }
}