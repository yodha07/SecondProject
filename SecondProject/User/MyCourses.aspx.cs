using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using SecondProject.MasterPage;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

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

            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ELearning_Project"].ConnectionString);
            {
                SqlCommand cmd = new SqlCommand(query, conn);
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
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ELearning_Project"].ConnectionString);
            LinkButton btn = (LinkButton)sender;
            int subCourseId = Convert.ToInt32(e.CommandArgument);
            int userId = GetLoggedInUserId();
            string userName = "";
            string courseTitle = "";

            SqlCommand cmdUser = new SqlCommand("SELECT Fullname FROM [User] WHERE UserId = @UserId", conn);
            conn.Open();
            {
                cmdUser.Parameters.AddWithValue("@UserId", userId);
                userName = cmdUser.ExecuteScalar()?.ToString();
            }
            SqlCommand cmdCourse = new SqlCommand(@"SELECT mc.Title AS MasterCourseTitle
    FROM SubCourse sc
    JOIN MasterCourse mc ON sc.MasterCourseId = mc.MasterCourseId
    WHERE sc.SubCourseId = @SubCourseId", conn);
            {
                cmdCourse.Parameters.AddWithValue("@SubCourseId", subCourseId);
                courseTitle = cmdCourse.ExecuteScalar()?.ToString();
            }

            string fileName = $"Certificate_{userName}_{DateTime.Now:dd-MM-yyyy_HH-mm-ss}.pdf";
            string filePath = "Certificates/" + fileName;
            string fullPath = Server.MapPath("~/" + filePath);

            string certificateDir = Server.MapPath("~/Certificates");
            if (!Directory.Exists(certificateDir))
            {
                Directory.CreateDirectory(certificateDir);
            }

            string logo = Server.MapPath("~/logo.jpg");
            string base64 = "";
            if(File.Exists(logo))
            {
                byte[] imageBytes = File.ReadAllBytes(logo);
                base64 = Convert.ToBase64String(imageBytes);
            }

            string htmlContent = $@"
                <html>
                    <head>
                        <style>
                            body {{
                                font-family: Arial, sans-serif;
                                text-align: center;
                            }}
                            h1 {{
                                color: #4CAF50;
                            }}
                            p {{
                                font-size: 20px;
                            }}
                            img{{
                                width: 200px;
                                height: 200px;
                            }}
                        </style>
                    </head>
                    <body>
                        <h1>Certificate of Completion</h1>
                        <img src='data:image/png;base64,{base64}'/>
                        <p>This is to certify that</p>
                        <h2>{userName}</h2>
                        <p>has successfully completed the course</p>
                        <h2>{courseTitle}</h2>
                        <p>on {DateTime.Now.ToString("dd/MM/yyyy")}</p>
                    </body>
                </html>";

            byte[] pdfBytes = GeneratePdfFromHtml(htmlContent);

            File.WriteAllBytes(fullPath, pdfBytes);

            string IssuedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            SqlCommand insertCmd = new SqlCommand($"INSERT INTO Certificate (UserId, SubCourseId, IssuedDate, CertificatePath) VALUES ('{userId}', '{subCourseId}',  '{IssuedDate}', @CertificatePath)", conn);
            {
                insertCmd.Parameters.AddWithValue("@CertificatePath", "/" + filePath);  

                insertCmd.ExecuteNonQuery();
            }

            if(System.IO.File.Exists(fullPath))
            {
                Response.ContentType = "application/pdf";
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(fullPath));
                Response.TransmitFile(fullPath);
                Response.End();
            }
            else
            {
                Response.Write("Not found");
            }
        }

        private byte[] GeneratePdfFromHtml(string htmlContent)
        {
            MemoryStream memoryStream = new MemoryStream();
            {
                Document document = new Document(PageSize.A4, 10f, 10f, 10f, 10f);
                PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                document.Open();

                StringReader stringReader = new StringReader(htmlContent);
                {
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, stringReader);
                }

                document.Close();
                return memoryStream.ToArray();
            }
        }
    }
}