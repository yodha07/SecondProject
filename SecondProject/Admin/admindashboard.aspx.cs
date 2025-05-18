using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.DataVisualization.Charting;


namespace SecondProject.Admin
{
    public partial class admindashboard : System.Web.UI.Page
    {
        SqlConnection conn;
        protected void Page_Load(object sender, EventArgs e)
        {

            string role = Session["Role"].ToString() ;
            if(role != "Admin")
            {
                Response.Redirect("~/Login/Login.aspx");
            }

            string cs = ConfigurationManager.ConnectionStrings["ELearning_Project"].ConnectionString;
            conn = new SqlConnection(cs);
            conn.Open();
            if (!IsPostBack)
            {
                status_active();
                status_inactive();
                master_course_count();
                sub_course_count();
                //chart();
                piechart();
                dropdown();
                sales_dropdown();


            }


        }
        protected void status_active()
        {
            string query = "exec count_is_Active";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader reader = cmd.ExecuteReader();
            //reader.Read();
            DataList1.DataSource = reader;
            DataList1.DataBind();
            reader.Read();

        }
        protected void status_inactive()
        {
            string query = "exec count_is_inActive";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader reader = cmd.ExecuteReader();
            //reader.Read();
            DataList2.DataSource = reader;
            DataList2.DataBind();
            reader.Read();
        }
        protected void master_course_count()
        {
            string query = "exec master_course_count";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader reader = cmd.ExecuteReader();
            //reader.Read();
            DataList3.DataSource = reader;
            DataList3.DataBind();
            reader.Read();
        }
        protected void sub_course_count()
        {
            string query = "exec sub_course_count";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader reader = cmd.ExecuteReader();
            //reader.Read();
            DataList4.DataSource = reader;
            DataList4.DataBind();
            reader.Read();
        }

        protected void piechart()
        {
            string query = "exec piechart";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader reader = cmd.ExecuteReader();
            //reader.Read();
            Chart2.DataSource = reader;
            Chart2.DataBind();
            reader.Read();
        }
        //protected void chart()
        //{
        //    int year = 2020;
        //    string query = $"exec sale_data '{year}'";
        //    SqlCommand cmd = new SqlCommand(query, conn);
        //    SqlDataReader reader = cmd.ExecuteReader();
        //    //reader.Read();
        //    Chart1.DataSource = reader;
        //    Chart1.DataBind();  
        //    reader.Read();
        //}
        protected void dropdown()
        {
            string query = "exec dropdown";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader rdr = cmd.ExecuteReader();
            //reader.Read();
            //DropDownList1.DataSource = rdr;
            //DropDownList1.DataBind();
            while (rdr.Read())
            {
                DropDownList1.Items.Add(new ListItem(rdr["Title"].ToString(), rdr["MasterCourseId"].ToString()));

            }

        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedId = int.Parse(DropDownList1.SelectedValue);

            string query = $"exec dropdown_grid'{selectedId}'";

            SqlCommand cmd = new SqlCommand(query, conn);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            GridView1.DataSource = dt;
            GridView1.DataBind();
            //chart();
            //piechart();

        }

        protected void sales_dropdown()
        {
            string query = "exec sales_dropdown";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader rdr = cmd.ExecuteReader();
            //reader.Read();
            DropDownList2.DataSource = rdr;
            DropDownList2.DataBind();
            //while (rdr.Read())
            //{
            //    DropDownList1.Items.Add(new ListItem(rdr["Title"].ToString(), rdr["MasterCourseId"].ToString()));

            //}

        }

        protected void DropDownList2_SelectedIndexChanged(object sender, EventArgs e)
        {
            int year = int.Parse(DropDownList2.SelectedValue);
            string query = $"exec sale_data '{year}'";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader reader = cmd.ExecuteReader();
            //reader.Read();
            Chart1.DataSource = reader;
            Chart1.DataBind();
            reader.Read();
        }
    }
}