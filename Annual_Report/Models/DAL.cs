using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Annual_Report.Models
{
   
    public class DAL
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DbEntity"].ConnectionString);
        public DataSet Get_System4()
        {

            SqlCommand com = new SqlCommand("Select System4 from final_analysis_2020 group by System4", con);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;

        }
    }
}