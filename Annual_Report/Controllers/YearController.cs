using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Annual_Report.Controllers
{
    public class YearController : Controller
    {
        // GET: Year
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ViewBag.year = "2022";
            base.OnActionExecuting(filterContext);
        }

    }
}