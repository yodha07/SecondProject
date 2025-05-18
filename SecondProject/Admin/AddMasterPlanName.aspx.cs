using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace SecondProject.Admin
{
    public partial class AddMasterPlanName : System.Web.UI.Page
    {
        SqlConnection conn;
        protected void Page_Load(object sender, EventArgs e)
        {
            string cs = ConfigurationManager.ConnectionStrings["ELearning_Project"].ConnectionString;
            conn = new SqlConnection(cs);
            conn.Open();
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            string mpName = TextBox1.Text;

            string query = $"exec el_AddMasterPlan '{mpName}'";
            SqlCommand cmd = new SqlCommand(query, conn);
            int r = cmd.ExecuteNonQuery();
            if (r > 0)
            {
                Response.Write("<script>alert('Master Plan Name Added')</script>");
            }
            else
            {
                Response.Write("<script>alert('Master Plan Name Not Added')</script>");
            }
        }
    }

}