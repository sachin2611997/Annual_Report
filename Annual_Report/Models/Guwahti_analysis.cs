using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Annual_Report.Models
{
    [Table("Guwahti_Test_2023")]
    public class Guwahti_analysis
    {[Key]
        public string System4 { get; set; }
        public string System3 { get; set; }
        public string System2 { get; set; }
        public string SEX { get; set; }
        public double AGE { get; set; }
        public string TGPR { get; set; }
        public string N_CLINEXT { get; set; }
        public string BROAD_AGE { get; set; }
        public string final_tti { get; set; }
        public string CANCER_DIRECTED_TREATMENT { get; set; }
        public string n_treatment { get; set; }
        public string n_status { get; set; }


    }
}