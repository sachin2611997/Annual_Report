using Annual_Report.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Annual_Report.Controllers
{
    public class Report2024Controller : Controller
    {
        DbEntity db = new DbEntity();
        // GET: Report2024
        [HttpGet]
        public ActionResult Age_Site_Distri_Male24()
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
            { "0-04", g.FirstOrDefault(x => x.AgeGroup == "0-04")?.Count ?? 0 },
            { "05-09", g.FirstOrDefault(x => x.AgeGroup == "05-09")?.Count ?? 0 },
            { "10-14", g.FirstOrDefault(x => x.AgeGroup == "10-14")?.Count ?? 0 },
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
        public ActionResult Age_Site_Distribution_Female24()
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
            { "0-04", g.FirstOrDefault(x => x.AgeGroup == "0-04")?.Count ?? 0 },
            { "05-09", g.FirstOrDefault(x => x.AgeGroup == "05-09")?.Count ?? 0 },
            { "10-14", g.FirstOrDefault(x => x.AgeGroup == "10-14")?.Count ?? 0 },
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
    }
}