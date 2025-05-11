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
    public partial class MyCourses : System.Web.UI.Page
    {
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ELearning_Project"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindMyCourses();
            }
        }

        private int GetLoggedInUserId()
        {
            int userID = -1;
            string email = Session["Email"]?.ToString();
            if (string.IsNullOrEmpty(email))
            {
                Response.Redirect("~/Login/Login.aspx");
            }
            else
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT UserId FROM [User] WHERE Email = @Email", conn);
                cmd.Parameters.AddWithValue("@Email", email);

                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    userID = Convert.ToInt32(rdr["UserId"]);
                    Session["UserId"] = userID;
                    Response.Write("UserId: " + userID);
                }
                conn.Close();
            }
            return userID;
        }


        private void BindMyCourses()
        {
            int userId = GetLoggedInUserId();
            string query = @"
        SELECT DISTINCT sc.SubCourseId, sc.Title, sc.Thumbnail
        FROM SubCourse sc
        INNER JOIN User_SubCourseAccess usa ON sc.SubCourseId = usa.SubCourseId
        WHERE usa.UserId = @UserId";

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ELearning_Project"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    try
                    {
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            List<SubCourse> subCourses = new List<SubCourse>();
                            while (reader.Read())
                            {
                                subCourses.Add(new SubCourse
                                {
                                    SubCourseId = Convert.ToInt32(reader["SubCourseId"]),
                                    Title = reader["Title"].ToString(),
                                    Thumbnail = reader["Thumbnail"].ToString()
                                });
                            }

                            rptSubCourses.DataSource = subCourses;
                            rptSubCourses.DataBind();
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle any exceptions (log it or show an error message)
                        Response.Write("Error: " + ex.Message);
                    }
                }
            }
        }

        public class SubCourse
        {
            public int SubCourseId { get; set; }
            public string Title { get; set; }
            public string Thumbnail { get; set; }
        }


        protected void ViewTopics_Click(object sender, CommandEventArgs e)
        {
            int subCourseId = Convert.ToInt32(e.CommandArgument);
            Response.Redirect($"Topics.aspx?scid={subCourseId}");
        }

        protected void Assignments_Click(object sender, CommandEventArgs e)
        {
            int subCourseId = Convert.ToInt32(e.CommandArgument);
            Response.Redirect($"Assignments.aspx?scid={subCourseId}");
        }

        protected void Mcq_Click(object sender, CommandEventArgs e)
        {
            int subCourseId = Convert.ToInt32(e.CommandArgument);
            Response.Redirect($"MCQs.aspx?scid={subCourseId}");
        }

        protected void Certificate_Click(object sender, CommandEventArgs e)
        {
            int subCourseId = Convert.ToInt32(e.CommandArgument);
            Response.Redirect($"Certificate.aspx?scid={subCourseId}");
        }


    }
}