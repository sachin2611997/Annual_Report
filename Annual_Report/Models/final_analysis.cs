using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Annual_Report.Models
{
    
    // [Table("test")]               // 2021 report
     //[Table("Report2022")]   //testTMH 2022
    [Table("TMH_2022")]  //TMH_2022

    public class final_analysis
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
        public int SIXTMT { get; set; } //TMH-RX-TYPE
        public string STATE { get; set; }
        public string BROAD_AGE { get; set; }
        public string final_tti { get; set; }
        public string Lead_site { get; set; }
        public string Sys { get; set; }

        public string gynae_system { get; set; }
        //  public string SIXTMT { get; set; }
        public string color { get; set; }

        public string Year5Age { get; set; }
        public string System1 { get; set; }
        public string NMETHOD { get; set; }
        public string REL_DET { get; set; } //religion property
        public string MOTHER_TONGUE { get; set; } //mother tongue
        public string EDU_DET { get; set; } //education
        public string TREATMENT_MUM_All { get; set; }
        public string PRVTMT { get; set; }
        public string AGE_GRP { get; set; }
        public string MyProperty { get; set; }
        public string TREATMENT_Before { get; set; }
        public string INST { get; set; }

        public string n_dissts { get; set; }
    }


    public class final_analysisViewModel
    {
        public string System4 { get; set; }
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

        
    }

    public class System3StatsViewModel
    {
        public string Category { get; set; } // New property for Category
        public string System4 { get; set; }
        public string System { get; set; }
        public string System3 { get; set; }
        public string System2 { get; set; }   
        public int MaleCount { get; set; }
        public int FemaleCount { get; set; }
        public int TotalCount { get; set; }
        public int TotalFCount { get; set; }
        public double MalePercentage { get; set; }
        public double FemalePercentage { get; set; }
        public double TotalPercentage { get; set; }
        public int TotalNonHodCount { get; internal set; }
        public int TotalNosCount { get; internal set; }
        public string ICD10 { get; set; }
    }
    public class MethodDiagnosisVM
    {
        public List<System3StatsViewModel> Male { get; set; }
        public List<System3StatsViewModel> Female { get; set; }
    }

    [Table("OverviewTmcRegistration")]
    public class OverviewTmcRegistration
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Case File Registrations - TMH")]
        public int CaseFileReg_TMH { get; set; }

        [Display(Name = "Case File Registrations - ACTREC")]
        public int CaseFileReg_ACTREC { get; set; }

        [Display(Name = "Referral Card Registrations - TMH")]
        public int ReferralCardReg_TMH { get; set; }

        [Display(Name = "Referral Card Registrations - ACTREC")]
        public int ReferralCardReg_ACTREC { get; set; }

        [Display(Name = "Preventive Registrations - TMH")]
        public int PreventiveReg_TMH { get; set; }

       

        [Display(Name = "Teleconsultation - TMH")]
        public int Teleconsultation_TMH { get; set; }

       
        [Display(Name = "Total Registrations - TMH")]
        public int TotalReg_TMH { get; set; }

        [Display(Name = "Total Registrations - ACTREC")]
        public int TotalReg_ACTREC { get; set; }

        [Display(Name = "Total Registrations - Total")]
        public int TotalReg_Total { get; set; }

        // ---------------- Section 2: Cancer Cases ----------------

        [Display(Name = "Cancer Cases from Case File Reg - TMH")]
        public int CancerFromCaseFile_TMH { get; set; }

        [Display(Name = "Cancer Cases from Case File Reg - ACTREC")]
        public int CancerFromCaseFile_ACTREC { get; set; }

        [Display(Name = "Cancer Cases from Case File Reg - Total")]
        public int CancerFromCaseFile_Total { get; set; }

        [Display(Name = "Cancer Cases from Referral Card - Total")]
        public int CancerFromReferralCard_Total { get; set; }

        [Display(Name = "Total Cancer Cases in TMC - Total")]
        public int TotalCancerCasesTMC_Total { get; set; }

        // ---------------- Bottom summary ----------------

        [Display(Name = "Estimated Cancer Cases in India (Globocan 2022)")]
        public int EstimatedCancerCasesIndia { get; set; }

        [Display(Name = "% of cancer patients attending TMC Mumbai Centres")]
        [Range(0, 100)]
        public decimal PercentAttendingTMC { get; set; }
    }

    [Table("PediatricCancer")]
    public class PediatricCancer
    {
        [Key]
        public int Id { get; set; }

        public string CancerType { get; set; }

        public int Male_00_14 { get; set; }
        public int Female_00_14 { get; set; }
        public int Total_00_14 { get; set; }

        public int Male_15_19 { get; set; }
        public int Female_15_19 { get; set; }
        public int Total_15_19 { get; set; }

        public int GrandTotal { get; set; }

        public int DisplayOrder { get; set; }
    }

    [Table("KeyFindings")]
    public class KeyFinding
    {
        [Key]
        public int Id { get; set; }
        public string Heading { get; set; }

        [AllowHtml]
        public string Key { get; set; }
    }

    [Table("MagnitudeRegistration")]
    public class MagnitudeRegistration
    {
        [Key]
        public int Id { get; set; }

        public int Year { get; set; }

        public int TotalNewPatientsRegistered { get; set; }

        public string CancerCases { get; set; }

        public int MaleCases { get; set; }

        public int FemaleCases { get; set; }
    }

    public class Index
    {
        public string Number { get; set; } = "";   // "Chapter 3", "Chapter 7", etc. Empty for non-chapter rows.
        public string Title { get; set; } = "";
        public string PageNo { get; set; } = "";    // supports single pages ("1") and ranges ("3-10"); empty = no page yet
        public int Level { get; set; } = 1;
        public bool IsSectionDivider { get; set; } = false;
    }






}