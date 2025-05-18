using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SecondProject.Admin
{
    public partial class AddTopics : System.Web.UI.Page
    {
        SqlConnection conn;
        protected void Page_Load(object sender, EventArgs e)
        {
            string cs = ConfigurationManager.ConnectionStrings["ELearning_Project"].ConnectionString;
            conn = new SqlConnection(cs);
            conn.Open();
            if (!IsPostBack)
            {
                loadMcName();
            }
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            string topicName = TextBox1.Text;
            int id = int.Parse(DropDownList2.SelectedValue);
            string videoCode = TextBox2.Text;
            string duration = TextBox3.Text;

            string query = $"exec el_AddTopic '{id}','{topicName}','{videoCode}','{duration}'";
            SqlCommand cmd = new SqlCommand(query, conn);
            int r = cmd.ExecuteNonQuery();
            if (r > 0)
            {
                Response.Write("<script>alert('Topic Added')</script>");
            }
            else
            {
                Response.Write("<script>alert('Topic Not Added')</script>");
            }
        }

        public void loadMcName()
        {
            string query = $"exec el_GetMasterCourse";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader rdr = cmd.ExecuteReader();
            DropDownList1.Items.Add(new ListItem("--Select Master Course--", "0"));
            while (rdr.Read())
            {
                DropDownList1.Items.Add(new ListItem(rdr["Title"].ToString(), rdr["MasterCourseId"].ToString()));
            }
            rdr.Close();
            DropDownList2.Items.Add(new ListItem("--Select Master Course First--", "0"));
        }
        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList2.Items.Clear();
            if (int.Parse(DropDownList1.SelectedValue) == 0)
            {
                DropDownList2.Items.Add(new ListItem("--Select Master Course First--", "0"));
            }
            else
            {
                DropDownList2.Items.Add(new ListItem("--Select Sub Course--", "0"));
                int mcid = int.Parse(DropDownList1.SelectedValue);
                string query2 = $"exec  s '{mcid}'";
                SqlCommand cmd2 = new SqlCommand(query2, conn);
                SqlDataReader rdr2 = cmd2.ExecuteReader();
                while (rdr2.Read())
                {
                    DropDownList2.Items.Add(new ListItem(rdr2["Title"].ToString(), rdr2["SubCourseId"].ToString()));
                }
                rdr2.Close();
            }
        }
    }

}