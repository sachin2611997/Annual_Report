using Annual_Report.Models.testmodel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Annual_Report.Models
{
    public class DbEntity:DbContext
    {
        public DbEntity():base("DbEntity")
        {

        }
        public DbSet<final_analysis> fnl_alysis { get; set; }
        public DbSet<OverviewTmcRegistration> overview_tmc { get; set; }

       // public DbSet<Provisional_Report> Pro_Ret { get; set; }
       
      

        public DbSet<Models.testmodel.testmodel> test1 { get; set; }
        public DbSet<testmodel2> test2 { get; set; }
        public DbSet<Models.testmodel.testmodel3> test3 { get; set; }


        //practice
       

    }
}