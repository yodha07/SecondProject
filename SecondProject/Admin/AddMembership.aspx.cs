using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SecondProject.Admin
{
    public partial class AddMembership : System.Web.UI.Page
    {
        SqlConnection conn;
        protected void Page_Load(object sender, EventArgs e)
        {
            string cs = ConfigurationManager.ConnectionStrings["ELearning_Project"].ConnectionString;
            conn = new SqlConnection(cs);
            conn.Open();
            if (!IsPostBack)
            {
                loadPlanName();
            }
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            string masterplanId = DropDownList1.SelectedValue;
            int id = int.Parse(DropDownList2.SelectedValue);
            double price = double.Parse(TextBox1.Text);

            string query = $"exec el_AddMembershipPlan '{id}','{price}','{masterplanId}'";
            SqlCommand cmd = new SqlCommand(query, conn);
            int r = cmd.ExecuteNonQuery();

            int planId = 0;
            string query2 = $"exec el_FetchPlanId";
            SqlCommand cmd2 = new SqlCommand(query2, conn);
            SqlDataReader rdr2 = cmd2.ExecuteReader();
            if (rdr2.Read())
            {
                planId = int.Parse(rdr2[0].ToString());
            }
            int c = 0;
            foreach (ListItem item in CheckBoxList1.Items)
            {
                if (item.Selected)
                {
                    int subCourseId = int.Parse(item.Value);
                    string query3 = $"exec el_AddMembershipPlanSubcourse '{planId}','{subCourseId}'";
                    SqlCommand cmd3 = new SqlCommand(query3, conn);
                    int r3 = cmd3.ExecuteNonQuery();
                    if (r3 > 0)
                    {
                        c++;
                    }
                }
            }
            if (c > 0)
            {
                Response.Write("<script>alert('Membership Added')</script>");
            }
            else
            {
                Response.Write("<script>alert('Membership not Added')</script>");
            }
        }

        public void loadPlanName()
        {
            string query = $"exec el_GetMasterPlan";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader rdr = cmd.ExecuteReader();
            DropDownList1.Items.Add(new ListItem("--Select Tier--", "0"));
            DropDownList2.Items.Add(new ListItem("--Select Tier First--", "0"));
            CheckBoxList1.Items.Add("--Select Tier/Master Course First--");
            while (rdr.Read())
            {
                DropDownList1.Items.Add(new ListItem(rdr["PlanName"].ToString(), rdr["PlanId"].ToString()));
            }
            rdr.Close();
        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList2.Items.Clear();
            CheckBoxList1.Items.Clear();
            if (int.Parse(DropDownList1.SelectedValue) == 0)
            {
                DropDownList2.Items.Add(new ListItem("--Select Tier First--", "0"));
                CheckBoxList1.Items.Add("--Select Tier/Master Course First--");
            }
            else
            {
                string query = $"exec el_GetMasterCourse";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader rdr = cmd.ExecuteReader();
                DropDownList2.Items.Add(new ListItem("--Select Master Course--", "0"));
                while (rdr.Read())
                {
                    DropDownList2.Items.Add(new ListItem(rdr["Title"].ToString(), rdr["MasterCourseId"].ToString()));
                }
                rdr.Close();
                CheckBoxList1.Items.Add("--Select Master Course First--");
            }

        }
        protected void DropDownList2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (int.Parse(DropDownList2.SelectedValue) == 0)
            {
                CheckBoxList1.Items.Clear();
                CheckBoxList1.Items.Add("--Select Master Course First--");
            }
            else
            {
                int mcid = int.Parse(DropDownList2.SelectedValue);
                string query2 = $"exec el_GetSubCourses '{mcid}'";
                SqlCommand cmd2 = new SqlCommand(query2, conn);
                SqlDataReader rdr2 = cmd2.ExecuteReader();
                CheckBoxList1.Items.Clear();
                while (rdr2.Read())
                {
                    string title = rdr2["Title"].ToString();
                    string id = rdr2["SubCourseId"].ToString();
                    CheckBoxList1.Items.Add(new ListItem(title, id));
                }


                rdr2.Close();
            }
        }
    }

}