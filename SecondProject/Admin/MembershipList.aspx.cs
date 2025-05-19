using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SecondProject.Admin
{
    public partial class MembershipList : System.Web.UI.Page
    {
        SqlConnection conn;
        protected void Page_Load(object sender, EventArgs e)
        {
            string cs = ConfigurationManager.ConnectionStrings["ELearning_Project"].ConnectionString;
            conn = new SqlConnection(cs);
            conn.Open();
            if (!IsPostBack)
            {
                fetchPlan();
            }
        }

        public void fetchPlan()
        {
            string query = $"exec el_GetPlan";
            SqlDataAdapter da = new SqlDataAdapter(query, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            DataList1.DataSource = dt;
            DataList1.DataBind();

        }

        protected void DataList1_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            int PlanId = int.Parse(DataBinder.Eval(e.Item.DataItem, "PlanId").ToString());

            string query = $"exec el_GetPlanSubCourse '{PlanId}'";
            SqlDataAdapter ada = new SqlDataAdapter(query, conn);
            DataTable dt2 = new DataTable();
            ada.Fill(dt2);
            DataList innerList = (DataList)e.Item.FindControl("DataList2");
            innerList.DataSource = dt2;
            innerList.DataBind();

        }

        protected void DataList1_DeleteCommand(object source, DataListCommandEventArgs e)
        {
            try
            {
                int id = int.Parse(e.CommandArgument.ToString());
                string q2 = $"exec el_DeletePlan '{id}'";
                SqlCommand cmd2 = new SqlCommand(q2, conn);
                cmd2.ExecuteNonQuery();
                fetchPlan();
            }
            catch (SqlException e2)
            {
                Response.Write("<script>alert('User has bought this plan so unable to delete it')</script>");
            }
        }

        protected void DataList1_EditCommand(object source, DataListCommandEventArgs e)
        {
            int id = int.Parse(e.CommandArgument.ToString());
            string q = $"exec el_GetMembershipById '{id}'";
            SqlCommand cmd = new SqlCommand(q, conn);
            SqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                HiddenField1.Value = rdr["PlanId"].ToString();
                txtPrice.Text = rdr["Price"].ToString();

                string selectedPlanId = rdr["MasterPlanId"].ToString();
                getPlan(selectedPlanId);

                string selectedMcId = rdr["MasterCourseId"].ToString();
                DropDownList2.Items.Add(new ListItem(rdr["Title"].ToString(), rdr["MasterCourseId"].ToString()));


                List<int> selectedSubCourseIds = new List<int>();
                string subquery = $"exec el_GetPlanSubCourse '{id}'";
                SqlCommand subcmd = new SqlCommand(subquery, conn);
                SqlDataReader subrdr = subcmd.ExecuteReader();
                while (subrdr.Read())
                {
                    selectedSubCourseIds.Add(int.Parse(subrdr["SubCourseId"].ToString()));
                }
                subrdr.Close();

                getSc(int.Parse(selectedMcId), selectedSubCourseIds);
            }
            rdr.Close();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#exampleModal').modal('show');", true);
        }

        public void getPlan(string selectedPlanId)
        {
            DropDownList1.Items.Clear();
            string query = $"exec el_GetMasterPlan";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                string planId = rdr["PlanId"].ToString();
                string planName = rdr["PlanName"].ToString();
                DropDownList1.Items.Add(new ListItem(planName, planId));
            }
            rdr.Close();
            DropDownList1.SelectedValue = selectedPlanId;
        }
        public void getSc(int mcid, List<int> selectedSubCourseIds)
        {
            CheckBoxList1.Items.Clear();
            string query = $"exec el_GetSubCourses '{mcid}'";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                string title = rdr["Title"].ToString();
                string id = rdr["SubCourseId"].ToString();
                ListItem item = new ListItem(title, id);
                if (selectedSubCourseIds != null && selectedSubCourseIds.Contains(int.Parse(id)))
                {
                    item.Selected = true;
                }
                CheckBoxList1.Items.Add(item);
            }
            rdr.Close();
        }
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            int id = int.Parse(HiddenField1.Value);
            string masterplanId = DropDownList1.SelectedValue;
            double price = double.Parse(txtPrice.Text);

            string query = $"exec el_UpdateMembershipPlan '{id}','{price}','{masterplanId}'";
            SqlCommand cmd = new SqlCommand(query, conn);
            int r = cmd.ExecuteNonQuery();

            string query2 = $"exec el_DeleteMembershipPlanSubcourse '{id}'";
            SqlCommand cmd2 = new SqlCommand(query2, conn);
            int r2 = cmd2.ExecuteNonQuery();

            int planId = id;
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
            fetchPlan();
            if (c > 0)
            {
                Response.Write("<script>alert('Membership Updated')</script>");
            }
            else
            {
                Response.Write("<script>alert('Membership not Updated')</script>");
            }
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("MembershipList.aspx");
        }
    }

}