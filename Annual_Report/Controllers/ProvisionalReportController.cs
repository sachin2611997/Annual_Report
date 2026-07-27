using Annual_Report.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Web.Mvc;

namespace Annual_Report.Controllers
{
    public class ProvisionalReportController : Controller
    {
        DbEntity db = new DbEntity();

       
            
       

        public ActionResult Index()
        {
            return View();
        }
        // GET: ProvisionalReport
        public ActionResult AllCenter()
        {
            ViewBag.total = db.fnl_alysis.Count();
            ViewBag.malecount = db.fnl_alysis.Where(x => x.SEX == "M").Count();
            ViewBag.femalecount = db.fnl_alysis.Where(x => x.SEX == "F").Count();
            ViewBag.agedata = db.fnl_alysis.GroupBy(x => x.BROAD_AGE).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Select(x => x.BROAD_AGE).Count(), 1) }).OrderBy(i => i.Key == ">=60").ThenBy(i => i.Key == "46-59").ThenBy(i => i.Key == "20-45").ThenBy(i => i.Key == "0-19").ToList();
            var males = db.fnl_alysis.Where(x => x.SEX == "M").Average(x => x.AGE_AT_REG);
            ViewBag.male = Math.Round(males, 0);
            var females = db.fnl_alysis.Where(x => x.SEX == "F").Average(x => x.AGE_AT_REG);
            ViewBag.female = Math.Round(females, 0);
            var list = db.fnl_alysis.Where(x => x.System4 == "Head & Neck").GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.System4 == "Head & Neck").Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Take(5);

            return View(list);
        }

        public ActionResult HeadnNeck()
        {
            ViewBag.agedata = db.fnl_alysis.Where(x => x.System4 == "Head & Neck").GroupBy(x => x.BROAD_AGE).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System4 == "Head & Neck").Select(x => x.BROAD_AGE).Count(), 1) }).OrderBy(i => i.Key == ">=60").ThenBy(i => i.Key == "46-59").ThenBy(i => i.Key == "20-45").ThenBy(i => i.Key == "0-19").ToList();
            var list = db.fnl_alysis.Where(x => x.System4 == "Head & Neck").GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.System4 == "Head & Neck").Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Take(5);
            ViewBag.malecount = db.fnl_alysis.Where(x => x.SEX == "M" && x.System4 == "Head & Neck").Count();
            ViewBag.femalecount = db.fnl_alysis.Where(x => x.SEX == "F" && x.System4 == "Head & Neck").Count();
            ViewBag.TotalCount = db.fnl_alysis.Where(x => x.System4 == "Head & Neck").Select(x => x.STATE).Count();
            var list1 = db.fnl_alysis.Where(x => x.System4 == "Head & Neck").GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.System4 == "Head & Neck").Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Skip(5);

            ViewBag.other = list1.Sum(y => y.Statecount);
            ViewBag.Otherper = Math.Round(list1.Sum(x => x.Statecount) * 100 / db.fnl_alysis.Where(x => x.System4 == "Head & Neck").Select(x => x.STATE).Count(), 1);
            ViewBag.System4 = "Head & Neck";
            return View(list);
        }

        public ActionResult Lung()
        {
            ViewBag.agedata = db.fnl_alysis.Where(x => x.System4 == "Lung").GroupBy(x => x.BROAD_AGE).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System4 == "Lung").Select(x => x.BROAD_AGE).Count(), 1) }).OrderBy(i => i.Key == ">=60").ThenBy(i => i.Key == "46-59").ThenBy(i => i.Key == "20-45").ThenBy(i => i.Key == "0-19").ToList();
            var list = db.fnl_alysis.Where(x => x.System4 == "Lung").GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.System4 == "Lung").Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Take(5);
            ViewBag.malecount = db.fnl_alysis.Where(x => x.SEX == "M" && x.System4 == "Lung").Count();
            ViewBag.femalecount = db.fnl_alysis.Where(x => x.SEX == "F" && x.System4 == "Lung").Count();
            ViewBag.TotalCount = db.fnl_alysis.Where(x => x.System4 == "Lung").Select(x => x.STATE).Count();
            var list1 = db.fnl_alysis.Where(x => x.System4 == "Lung").GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.System4 == "Lung").Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Skip(5);

            ViewBag.other = list1.Sum(y => y.Statecount);
            ViewBag.Otherper = Math.Round(list1.Sum(x => x.Statecount) * 100 / db.fnl_alysis.Where(x => x.System4 == "Lung").Select(x => x.STATE).Count(), 1);
            ViewBag.System4 = "Lung & Trachea";
            return View(list);
        }

        public ActionResult Breast()
        {
            ViewBag.agedata = db.fnl_alysis.Where(x => x.System2 == "Female Breast").GroupBy(x => x.BROAD_AGE).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System2 == "Female Breast").Select(x => x.BROAD_AGE).Count(), 1) }).OrderBy(i => i.Key == ">=60").ThenBy(i => i.Key == "46-59").ThenBy(i => i.Key == "20-45").ThenBy(i => i.Key == "0-19").ToList();
            var list = db.fnl_alysis.Where(x => x.System2 == "Female Breast").GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.System2 == "Female Breast").Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Take(5);
            ViewBag.malecount = db.fnl_alysis.Where(x => x.SEX == "M" && x.System2 == "Female Breast").Count();
            ViewBag.femalecount = db.fnl_alysis.Where(x => x.SEX == "F" && x.System2 == "Female Breast").Count();
            ViewBag.TotalCount = db.fnl_alysis.Where(x => x.System2 == "Female Breast").Select(x => x.STATE).Count();
            var list1 = db.fnl_alysis.Where(x => x.System2 == "Female Breast").GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.System2 == "Female Breast").Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Skip(5);

            ViewBag.other = list1.Sum(y => y.Statecount);
            ViewBag.Otherper = Math.Round(list1.Sum(x => x.Statecount) * 100 / db.fnl_alysis.Where(x => x.System2 == "Female Breast").Select(x => x.STATE).Count(), 1);
            ViewBag.System4 = "Breast";
            return View(list);
        }

        public ActionResult Gastrointestinal_ProTable()
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

            }).ToList();

            ViewBag.tMcount = db.fnl_alysis.Where(x => x.System4 == "Gastro-intestinal" && x.SEX == "M").Count();
            ViewBag.tFcount = db.fnl_alysis.Where(x => x.System4 == "Gastro-intestinal" && x.SEX == "F").Count();
            return View();
        }  // table


        public ActionResult Gastrointestinal()
        {
            ViewBag.agedata = db.fnl_alysis.Where(x => x.System4 == "Gastro-intestinal").GroupBy(x => x.BROAD_AGE).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System4 == "Gastro-intestinal").Select(x => x.BROAD_AGE).Count(), 1) }).OrderBy(i => i.Key == ">=60").ThenBy(i => i.Key == "46-59").ThenBy(i => i.Key == "20-45").ThenBy(i => i.Key == "0-19").ToList();
            var list = db.fnl_alysis.Where(x => x.System4 == "Gastro-intestinal").GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.System4 == "Gastro-intestinal").Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Take(5);
            ViewBag.malecount = db.fnl_alysis.Where(x => x.SEX == "M" && x.System4 == "Gastro-intestinal").Count();
            ViewBag.femalecount = db.fnl_alysis.Where(x => x.SEX == "F" && x.System4 == "Gastro-intestinal").Count();
            ViewBag.TotalCount = db.fnl_alysis.Where(x => x.System4 == "Gastro-intestinal").Select(x => x.STATE).Count();
            var list1 = db.fnl_alysis.Where(x => x.System4 == "Gastro-intestinal").GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.System4 == "Gastro-intestinal").Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Skip(5);

            ViewBag.other = list1.Sum(y => y.Statecount);
            ViewBag.Otherper = Math.Round(list1.Sum(x => x.Statecount) * 100 / db.fnl_alysis.Where(x => x.System4 == "Gastro-intestinal").Select(x => x.STATE).Count(), 1);
          //  ViewBag.System4 = "Lung & Trachea";
            return View(list);
        }

        public ActionResult Bone()
        {
            ViewBag.agedata = db.fnl_alysis.Where(x => x.System4 == "Bone").GroupBy(x => x.BROAD_AGE).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System4 == "Bone").Select(x => x.BROAD_AGE).Count(), 1) }).OrderBy(i => i.Key == ">=60").ThenBy(i => i.Key == "46-59").ThenBy(i => i.Key == "20-45").ThenBy(i => i.Key == "0-19").ToList();
            var list = db.fnl_alysis.Where(x => x.System4 == "Bone").GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.System4 == "Bone").Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Take(5);
            ViewBag.malecount = db.fnl_alysis.Where(x => x.SEX == "M" && x.System4 == "Bone").Count();
            ViewBag.femalecount = db.fnl_alysis.Where(x => x.SEX == "F" && x.System4 == "Bone").Count();
            ViewBag.TotalCount = db.fnl_alysis.Where(x => x.System4 == "Bone").Select(x => x.STATE).Count();
            var list1 = db.fnl_alysis.Where(x => x.System4 == "Bone").GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.System4 == "Bone").Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Skip(5);

            ViewBag.other = list1.Sum(y => y.Statecount);
            ViewBag.Otherper = Math.Round(list1.Sum(x => x.Statecount) * 100 / db.fnl_alysis.Where(x => x.System4 == "Bone").Select(x => x.STATE).Count(), 1);
              ViewBag.System4 = "Bone";
            return View(list);
        }

        public ActionResult Peripheral_Nerves()
        {
            ViewBag.agedata = db.fnl_alysis.Where(x => x.System4 == "Peripheral Nerves & Soft Tissue").GroupBy(x => x.BROAD_AGE).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System4 == "Peripheral Nerves & Soft Tissue").Select(x => x.BROAD_AGE).Count(), 1) }).OrderBy(i => i.Key == ">=60").ThenBy(i => i.Key == "46-59").ThenBy(i => i.Key == "20-45").ThenBy(i => i.Key == "0-19").ToList();
            var list = db.fnl_alysis.Where(x => x.System4 == "Peripheral Nerves & Soft Tissue").GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.System4 == "Peripheral Nerves & Soft Tissue").Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Take(5);
            ViewBag.malecount = db.fnl_alysis.Where(x => x.SEX == "M" && x.System4 == "Peripheral Nerves & Soft Tissue").Count();
            ViewBag.femalecount = db.fnl_alysis.Where(x => x.SEX == "F" && x.System4 == "Peripheral Nerves & Soft Tissue").Count();
            ViewBag.TotalCount = db.fnl_alysis.Where(x => x.System4 == "Peripheral Nerves & Soft Tissue").Select(x => x.STATE).Count();
            var list1 = db.fnl_alysis.Where(x => x.System4 == "Peripheral Nerves & Soft Tissue").GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.System4 == "Peripheral Nerves & Soft Tissue").Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Skip(5);

            ViewBag.other = list1.Sum(y => y.Statecount);
            ViewBag.Otherper = Math.Round(list1.Sum(x => x.Statecount) * 100 / db.fnl_alysis.Where(x => x.System4 == "Peripheral Nerves & Soft Tissue").Select(x => x.STATE).Count(), 1);
            ViewBag.System4 = "Peripheral Nerves and Autonomic Nervous System and  Soft Tissue";
            return View(list);
        }
        public ActionResult Female_Gynae()
        {
            ViewBag.agedata = db.fnl_alysis.Where(x => x.gynae_system == "Female Gyaec Cancer").GroupBy(x => x.BROAD_AGE).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.gynae_system == "Female Gyaec Cancer").Select(x => x.BROAD_AGE).Count(), 1) }).OrderBy(i => i.Key == ">=60").ThenBy(i => i.Key == "46-59").ThenBy(i => i.Key == "20-45").ThenBy(i => i.Key == "0-19").ToList();
            var list = db.fnl_alysis.Where(x => x.gynae_system == "Female Gyaec Cancer").GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.gynae_system == "Female Gyaec Cancer").Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Take(5);
            ViewBag.malecount = db.fnl_alysis.Where(x => x.SEX == "M" && x.gynae_system == "Female Gyaec Cancer").Count();
            ViewBag.femalecount = db.fnl_alysis.Where(x => x.SEX == "F" && x.gynae_system == "Female Gyaec Cancer").Count();
            ViewBag.TotalCount = db.fnl_alysis.Where(x => x.gynae_system == "Female Gyaec Cancer").Select(x => x.STATE).Count();
            var list1 = db.fnl_alysis.Where(x => x.gynae_system == "Female Gyaec Cancer").GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.gynae_system == "Female Gyaec Cancer").Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Skip(5);

            ViewBag.other = list1.Sum(y => y.Statecount);
            ViewBag.Otherper = Math.Round(list1.Sum(x => x.Statecount) * 100 / db.fnl_alysis.Where(x => x.gynae_system == "Female Gyaec Cancer").Select(x => x.STATE).Count(), 1);
            ViewBag.System4 = "Female Gynaecological Cancers";

            //table
            ViewBag.TotalHaematCount = db.fnl_alysis.Where(x => x.gynae_system == "Female Gyaec Cancer").Count();

            ViewBag.System3Values = db.fnl_alysis
    .Where(x => x.gynae_system == "Female Gyaec Cancer")
    .GroupBy(x => x.System3)
    .Select(g => new System3StatsViewModel
    {
        System3 = g.Key,
        MaleCount = g.Where(x => x.SEX == "M").Count(),
        FemaleCount = g.Where(x => x.SEX == "F").Count(),
        TotalCount = db.fnl_alysis.Where(x => x.SEX == "M" && x.gynae_system == "Female Gyaec Cancer").Count(),
        TotalFCount = db.fnl_alysis.Where(x => x.SEX == "F" && x.gynae_system == "Female Gyaec Cancer").Count(),
        TotalNosCount = g.Count(x => x.SEX == "M") + g.Count(x => x.SEX == "F"),

    })
    .ToList();

            ViewBag.tMcount = db.fnl_alysis.Where(x => x.gynae_system == "Female Gyaec Cancer" && x.SEX == "M").Count();
            ViewBag.tFcount = db.fnl_alysis.Where(x => x.gynae_system == "Female Gyaec Cancer" && x.SEX == "F").Count();


            return View(list);
        }

        public ActionResult Male_Reproductive()
        {
            ViewBag.agedata = db.fnl_alysis.Where(x => x.gynae_system == "Uro and Male Reproductive Organs").GroupBy(x => x.BROAD_AGE).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.gynae_system == "Uro and Male Reproductive Organs").Select(x => x.BROAD_AGE).Count(), 1) }).OrderBy(i => i.Key == ">=60").ThenBy(i => i.Key == "46-59").ThenBy(i => i.Key == "20-45").ThenBy(i => i.Key == "0-19").ToList();
            var list = db.fnl_alysis.Where(x => x.gynae_system == "Uro and Male Reproductive Organs").GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.gynae_system == "Uro and Male Reproductive Organs").Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Take(5);
            ViewBag.malecount = db.fnl_alysis.Where(x => x.SEX == "M" && x.gynae_system == "Uro and Male Reproductive Organs").Count();
            ViewBag.femalecount = db.fnl_alysis.Where(x => x.SEX == "F" && x.gynae_system == "Uro and Male Reproductive Organs").Count();
            ViewBag.TotalCount = db.fnl_alysis.Where(x => x.gynae_system == "Uro and Male Reproductive Organs").Select(x => x.STATE).Count();
            var list1 = db.fnl_alysis.Where(x => x.gynae_system == "Uro and Male Reproductive Organs").GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.gynae_system == "Uro and Male Reproductive Organs").Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Skip(5);

            ViewBag.other = list1.Sum(y => y.Statecount);
            ViewBag.Otherper = Math.Round(list1.Sum(x => x.Statecount) * 100 / db.fnl_alysis.Where(x => x.gynae_system == "Uro and Male Reproductive Organs").Select(x => x.STATE).Count(), 1);
            ViewBag.System4 = "Urinary Tract and Male Genital Organ Cancers";

            //table
            ViewBag.TotalHaematCount = db.fnl_alysis.Where(x => x.gynae_system == "Uro and Male Reproductive Organs").Count();

            ViewBag.System3Values = db.fnl_alysis
    .Where(x => x.gynae_system == "Uro and Male Reproductive Organs")
    .GroupBy(x => x.System3)
    .Select(g => new System3StatsViewModel
    {
        System3 = g.Key,
        MaleCount = g.Where(x => x.SEX == "M").Count(),
        FemaleCount = g.Where(x => x.SEX == "F").Count(),
        TotalCount = db.fnl_alysis.Where(x => x.SEX == "M" && x.gynae_system == "Uro and Male Reproductive Organs").Count(),
        TotalFCount = db.fnl_alysis.Where(x => x.SEX == "F" && x.gynae_system == "Uro and Male Reproductive Organs").Count(),
        TotalNosCount = g.Count(x => x.SEX == "M") + g.Count(x => x.SEX == "F"),

    })
    .ToList();

            ViewBag.tMcount = db.fnl_alysis.Where(x => x.gynae_system == "Uro and Male Reproductive Organs" && x.SEX == "M").Count();
            ViewBag.tFcount = db.fnl_alysis.Where(x => x.gynae_system == "Uro and Male Reproductive Organs" && x.SEX == "F").Count();


            return View(list);
        }



        public ActionResult Haemat()
        {
            ViewBag.agedata = db.fnl_alysis.Where(x => x.System4 == "Haematopoetic Cancers").GroupBy(x => x.BROAD_AGE).Select(y => new { y.Key, Count = Math.Round(y.Count() * 100.0 / db.fnl_alysis.Where(x => x.System4 == "Haematopoetic Cancers").Select(x => x.BROAD_AGE).Count(), 1) }).OrderBy(i => i.Key == ">=60").ThenBy(i => i.Key == "46-59").ThenBy(i => i.Key == "20-45").ThenBy(i => i.Key == "0-19").ToList();
            var list = db.fnl_alysis.Where(x => x.System4 == "Haematopoetic Cancers").GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.System4 == "Haematopoetic Cancers").Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Take(5);
            ViewBag.malecount = db.fnl_alysis.Where(x => x.SEX == "M" && x.System4 == "Haematopoetic Cancers").Count();
            ViewBag.femalecount = db.fnl_alysis.Where(x => x.SEX == "F" && x.System4 == "Haematopoetic Cancers").Count();
            ViewBag.TotalCount = db.fnl_alysis.Where(x => x.System4 == "Haematopoetic Cancers").Select(x => x.STATE).Count();
            var list1 = db.fnl_alysis.Where(x => x.System4 == "Haematopoetic Cancers").GroupBy(x => x.STATE).Select(x => new final_analysisViewModel() { State = x.Key, Statecount = x.Count(), Percentage = Math.Round(x.Count() * 100.0 / db.fnl_alysis.Where(y => y.System4 == "Haematopoetic Cancers").Select(y => y.STATE).Count(), 1) }).ToList().OrderByDescending(r => r.Percentage).Skip(5);

            ViewBag.other = list1.Sum(y => y.Statecount);
            ViewBag.Otherper = Math.Round(list1.Sum(x => x.Statecount) * 100 / db.fnl_alysis.Where(x => x.System4 == "Haematopoetic Cancers").Select(x => x.STATE).Count(), 1);


            //table
            ViewBag.TotalHaematCount = db.fnl_alysis.Where(x => x.System4 == "Haematopoetic Cancers").Count();

            ViewBag.System3Values = db.fnl_alysis
    .Where(x => x.System4 == "Haematopoetic Cancers")
    .GroupBy(x => x.System3)
    .Select(g => new System3StatsViewModel
    {
        System3 = g.Key,
        MaleCount = g.Where(x => x.SEX == "M").Count(),
        FemaleCount = g.Where(x => x.SEX == "F").Count(),
        TotalCount = db.fnl_alysis.Where(x => x.SEX == "M" && x.System4 == "Haematopoetic Cancers").Count(),
        TotalFCount = db.fnl_alysis.Where(x => x.SEX == "F" && x.System4 == "Haematopoetic Cancers").Count(),
        TotalNosCount = g.Count(x => x.SEX == "M") + g.Count(x => x.SEX == "F"),

    })
    .ToList();

            ViewBag.tMcount = db.fnl_alysis.Where(x => x.System4 == "Haematopoetic Cancers" && x.SEX == "M").Count();
            ViewBag.tFcount = db.fnl_alysis.Where(x => x.System4 == "Haematopoetic Cancers" && x.SEX == "F").Count();
            ViewBag.System4 = "Haematopoetic Cancers";
            return View(list);
        }


        public ActionResult LeadSiteCancers()
        {
            return View();
        }
        public ActionResult TopLeadSites()
        {
            var totalLeadSiteMaleCount = db.fnl_alysis.Where(x => x.SEX == "M").Count();
            ViewBag.Male = db.fnl_alysis.Where(x => x.SEX == "M").GroupBy(x => x.Lead_site ).Select(x=>new {Key=x.Key, Count = x.Count(), Percentage = Math.Round((x.Count() * 100.0) / totalLeadSiteMaleCount, 1) }).OrderByDescending(x=>x.Count).Take(10).ToList();

            var totalLeadSiteFemaleCount = db.fnl_alysis.Where(x => x.SEX == "F").Count();
            ViewBag.Female = db.fnl_alysis.Where(x => x.SEX == "F").GroupBy(x => x.Lead_site).Select(x => new { Key = x.Key, Count = x.Count(), Percentage = Math.Round((x.Count() * 100.0) / totalLeadSiteFemaleCount, 1) }).OrderByDescending(x => x.Count).Take(10).ToList();

            return View();
        }

        public ActionResult LeadSite2045()
        {
            var totalLeadSite2045Count = db.fnl_alysis.Where(x => x.SEX == "M" && x.BROAD_AGE== "20-45").Count();                            //wrong condiion 
            ViewBag.Male2045 = db.fnl_alysis.Where(x=>x.SEX=="M" && x.BROAD_AGE == "20-45").GroupBy(f=>new { f.Lead_site, f.Sys })
                  .Select(g => new { Key = g.Key.Lead_site, Sys = g.Key.Sys, Count = g.Count(), Percentage = Math.Round((g.Count() * 100.0) / totalLeadSite2045Count, 1) })
                  .OrderByDescending(x => x.Count)
                .Take(10)
                .ToList();

            ViewBag.Female2045 = db.fnl_alysis.Where(x => x.SEX == "F" && x.BROAD_AGE == "20-45").GroupBy(f => new { f.Lead_site, f.Sys })
                 .Select(g => new { Key = g.Key.Lead_site, Sys = g.Key.Sys, Count = g.Count(), Percentage = Math.Round((g.Count() * 100.0) / totalLeadSite2045Count, 1) })
                 .OrderByDescending(x => x.Count)
               .Take(10)
               .ToList();


            var totalLeadSite4659Count = db.fnl_alysis.Where(x => x.SEX == "M" && x.BROAD_AGE == "46-59").Count();

            ViewBag.Male4659 = db.fnl_alysis.Where(x => x.SEX == "M" && x.BROAD_AGE == "46-59").GroupBy(f => new { f.Lead_site, f.Sys })
                  .Select(g => new { Key = g.Key.Lead_site, Sys = g.Key.Sys, Count = g.Count(), Percentage = Math.Round((g.Count() * 100.0) / totalLeadSite4659Count, 1) })
                  .OrderByDescending(x => x.Count)
                .Take(10)
                .ToList();

            ViewBag.Female4659 = db.fnl_alysis.Where(x => x.SEX == "F" && x.BROAD_AGE == "46-59").GroupBy(f => new { f.Lead_site, f.Sys })
                 .Select(g => new { Key = g.Key.Lead_site, Sys = g.Key.Sys, Count = g.Count(), Percentage = Math.Round((g.Count() * 100.0) / totalLeadSite4659Count, 1) })
                 .OrderByDescending(x => x.Count)
               .Take(10)
               .ToList();

            return View();
        }


        public ActionResult LeadSite60above()
        {
            var totalLeadSite60aboveCount = db.fnl_alysis.Where(x => x.SEX == "M" && x.BROAD_AGE == ">=60").Count();                            //wrong condiion 
            ViewBag.Male60above = db.fnl_alysis.Where(x => x.SEX == "M" && x.BROAD_AGE == ">=60").GroupBy(f => new { f.Lead_site, f.Sys })
                  .Select(g => new { Key = g.Key.Lead_site, Sys = g.Key.Sys, Count = g.Count(), Percentage = Math.Round((g.Count() * 100.0) / totalLeadSite60aboveCount, 1) })
                  .OrderByDescending(x => x.Count)
                .Take(10)
                .ToList();

            ViewBag.Female60above = db.fnl_alysis.Where(x => x.SEX == "F" && x.BROAD_AGE == ">=60").GroupBy(f => new { f.Lead_site, f.Sys })
                 .Select(g => new { Key = g.Key.Lead_site, Sys = g.Key.Sys, Count = g.Count(), Percentage = Math.Round((g.Count() * 100.0) / totalLeadSite60aboveCount, 1) })
                 .OrderByDescending(x => x.Count)
               .Take(10)
               .ToList();


           

            return View();
        }

        public ActionResult Age_Sys_regis_actrec()
        {
            return View();
        }

        public ActionResult barchart()
        {
            var totalLeadSiteCount = db.fnl_alysis.Where(x => x.SEX == "M" && x.BROAD_AGE == "20-45").Count();                            //wrong condiion 
            ViewBag.Male = db.fnl_alysis.Where(x => x.SEX == "M" && x.BROAD_AGE == "20-45").GroupBy(f => new { f.Lead_site, f.Sys })
                  .Select(g => new { Key = g.Key.Lead_site, Sys = g.Key.Sys, Count = g.Count(), Percentage = Math.Round((g.Count() * 100.0) / totalLeadSiteCount, 1) })
                  .OrderByDescending(x => x.Count)
                .Take(10)
                .ToList();
            return View();
        }
    }
}