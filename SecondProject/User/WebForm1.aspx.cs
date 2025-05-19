using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SecondProject.User
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        int masterCourseId;
        SqlConnection conn;
        protected void Page_Load(object sender, EventArgs e)
        {
            conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ELearning_Project"].ConnectionString);
            conn.Open();
            //filldatalist();
            if (!IsPostBack)
            {
                filldatalist();

            }

        }

        public void filldatalist()
        {
            //string q = "select * from SubCourse join Topic on SubCourse.SubCourseId=Topic.SubCourseId";
            //string q = "SELECT   s.*, \r\n    COUNT(t.TopicId) AS TopicCount\r\nFROM \r\n    SubCourse s\r\nLEFT JOIN \r\n    Topic t ON s.SubCourseId = t.SubCourseId\r\nGROUP BY \r\n    s.SubCourseId, s.MasterCourseId, s.Title, s.Thumbnail, s.Price, s.Rating";
            string q = "exec GetSubCourseDetailsWithTopics";
            //string q = $"exec GetSubCourseSummaryByMasterCourse {masterCourseId}";
            SqlCommand cmd = new SqlCommand(q, conn);
            SqlDataReader rdr = cmd.ExecuteReader();
            DataList1.DataSource = rdr;
            DataList1.DataBind();

        }
        public string GetStarsHtml(object ratingObj)
        {
            double rating = double.Parse((ratingObj).ToString());
            string starsHtml = "";

            for (int i = 1; i <= 5; i++)
            {
                if (i <= Math.Floor(rating))
                {
                    starsHtml += "<i class='fa fa-star text-warning'></i>"; // full
                }
                else if (i - rating <= 0.5)
                {
                    starsHtml += "<i class='fa fa-star-half-o text-warning'></i>"; // half
                }
                else
                {
                    starsHtml += "<i class='fa fa-star-o text-warning'></i>"; // empty
                }
            }

            starsHtml += $" <span class='ml-1'>({rating:F1})</span>"; // show 1 decimal point
            return starsHtml;
        }

        protected void DataList1_ItemCommand(object source, DataListCommandEventArgs e)
        {
            int userid=int.Parse(Session["UserId"].ToString());
            if (e.CommandName == "addtocart")
            {

                int selectedsubcourseid = int.Parse(e.CommandArgument.ToString());
                string q = $"select * from Cart where UserId ={userid} and SubCourseID in ({selectedsubcourseid}) ";
                SqlCommand cmd = new SqlCommand(q, conn);
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    Response.Write("<script>alert('Course Is Already Added In The Cart')</script>");

                }
                else
                {
                    string q2 = "insert into Cart(UserId,SubCourseId)values('" + userid + "','" + selectedsubcourseid + "')";
                    SqlCommand cmd2 = new SqlCommand(q2, conn);
                    cmd2.ExecuteNonQuery();
                    Response.Write("<script>alert('Course Added In The Cart Succesfully')</script>");
                    Response.Redirect("Cart.aspx");
                }

            }



        }
        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            masterCourseId = int.Parse(DropDownList1.SelectedValue);

            //string q = "select * from SubCourse join Topic on SubCourse.SubCourseId=Topic.SubCourseId";
            //string q = "SELECT   s.*, \r\n    COUNT(t.TopicId) AS TopicCount\r\nFROM \r\n    SubCourse s\r\nLEFT JOIN \r\n    Topic t ON s.SubCourseId = t.SubCourseId\r\nGROUP BY \r\n    s.SubCourseId, s.MasterCourseId, s.Title, s.Thumbnail, s.Price, s.Rating";
            //string q = "exec GetSubCourseDetailsWithTopics";
            string q = $"exec GetSubCourseSummaryByMasterCourse {masterCourseId}";
            SqlCommand cmd = new SqlCommand(q, conn);
            SqlDataReader rdr = cmd.ExecuteReader();
            DataList1.DataSource = rdr;
            DataList1.DataBind();
        }


    }

}