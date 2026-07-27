using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Annual_Report.Models.testmodel
{
    [Table("final_analysis_2020")]
    public class testmodel
    {
        [Key]
        public double year { get; set; }
        public string System4 { get; set; }
        public string System3 { get; set; }
        public string System2 { get; set; }
        public double AGE_AT_REG { get; set; }
        public string PRVTMTYN { get; set; }
        public string SEX { get; set; }
        public string N_CLINEXT { get; set; }
        public string TMH_RX { get; set; }
        public string SIXTMT { get; set; } //TMH-RX-TYPE
        public string STATE { get; set; }
        public string BROAD_AGE { get; set; }
        public string final_tti { get; set; }
        public string Lead_site { get; set; }
        public string Sys { get; set; }

        public string gynae_system { get; set; }
        //  public string SIXTMT { get; set; }
       // public string color { get; set; }

       // public string Year5Age { get; set; }
        public string System1 { get; set; }
        public string NMETHOD { get; set; }
        public string REL_DET { get; set; } //religion property
        public string MOTHER_TONGUE { get; set; } //mother tongue
        public string EDU_DET { get; set; } //education
        public string TREATMENT_MUM_All { get; set; }
        public string PRVTMT { get; set; }
        public string AGE_GRP { get; set; }
      //  public string MyProperty { get; set; }
        public string TREATMENT_Before { get; set; }
        public string INST { get; set; }
    }



  


    public class testViewModel
    {
        public string System4 { get; set; }
        public string System3 { get; set; }
        public string NewCase { get; set; }
        public double NewCaseCount { get; set; }
        [DisplayName("Nos")]
        public double Statecount { get; set; }

        [DisplayName("Region")]
        public string State { get; set; }
        public int othercount { get; set; }
        public double Percentage { get; set; }
        public string Unknown { get; set; }
        public string date { get; set; }
        public double duration { get; set; }
        public int count { get; set; }

    }
}