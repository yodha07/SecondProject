using iTextSharp.text.pdf;
using iTextSharp.text;
using iTextSharp.tool.xml;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
//using Microsoft.AspNet.Identity;
using SecondProject.MasterPage;
using static SecondProject.User.MyCourses;

namespace SecondProject.User
{
    public partial class Topics : System.Web.UI.Page
    {

        SqlConnection conn;

        [Serializable]
        public class TopicItem
        {
            public int TopicId { get; set; }
            public string Title { get; set; }
            public string VideoEmbedCode { get; set; }
            public int SubCourseId { get; set; }
        }


        protected void Page_Init(object sender, EventArgs e)
        {
            LoadTopics();
            BindTopics();
        }



        protected void Page_Load(object sender, EventArgs e)
        {
            string connStr = ConfigurationManager.ConnectionStrings["ELearning_Project"].ConnectionString;
            conn = new SqlConnection(connStr);
            conn.Open();
            //month_duration();
            if (Session["Email"] == null || Session["UserId"] == null)
            {
                Response.Redirect("~/Login/Login.aspx");
            }
            if (!IsPostBack)
            {
                //month_duration();
                BindPlaylist();
                UpdateNavigationButtons();

                var topics = ViewState["Topics"] as List<TopicItem>;
                if (topics != null && topics.Count > 0)
                {
                    int subCourseId = topics[0].SubCourseId;

                    btnCertificate.CommandArgument = subCourseId.ToString();
                    btnAssignments.CommandArgument = subCourseId.ToString();
                    btnMCQ.CommandArgument = subCourseId.ToString();
                    //Button1.CommandArgument = subCourseId.ToString(); // For second copy
                    //Button2.CommandArgument = subCourseId.ToString();
                    MarkVideoWatchedIfNeeded();
                }
            }
        }


        protected void Assignments_Click(object sender, CommandEventArgs e)
        {
            int subCourseId = Convert.ToInt32(e.CommandArgument);
            int activeIndex = mvTopics.ActiveViewIndex;

            int topicId = 0;
            string filePath = "";

            string connStr = ConfigurationManager.ConnectionStrings["ELearning_Project"].ConnectionString;

            SqlConnection conn = new SqlConnection(connStr);
            {
                conn.Open();
                SqlCommand cmd1 = new SqlCommand(
                    @"SELECT TopicId 
              FROM Topic 
              WHERE SubCourseId = @SubCourseId 
              ORDER BY TopicId ", conn);
                cmd1.Parameters.AddWithValue("@SubCourseId", subCourseId);

                object result = cmd1.ExecuteScalar();

                if (result != null)
                {
                    topicId = Convert.ToInt32(result);

                    SqlCommand cmd2 = new SqlCommand("SELECT FilePath FROM Assignment WHERE TopicId = @TopicId", conn);
                    cmd2.Parameters.AddWithValue("@TopicId", topicId);

                    using (SqlDataReader reader = cmd2.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            filePath = reader["FilePath"].ToString();

                            string physicalPath = Server.MapPath("~/" + filePath);
                            if (File.Exists(physicalPath))
                            {
                                Response.Clear();
                                Response.ContentType = "application/pdf";
                                Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(physicalPath));
                                Response.TransmitFile(physicalPath);
                                Response.End();
                            }
                            else
                            {
                                Response.Write("<script>alert('File not found on server.');</script>");
                            }
                        }
                        else
                        {
                            Response.Write("<script>alert('No assignment found for this topic.');</script>");
                        }
                    }
                }
                else
                {
                    Response.Write("<script>alert('Topic not found.');</script>");
                }
            }
        }


        protected void Mcq_Click(object sender, CommandEventArgs e)
        {
            if (int.TryParse(e.CommandArgument?.ToString(), out int subCourseId))
            {
                Response.Redirect($"MCQs.aspx?scid={subCourseId}");
            }
            else
            {
                Response.Write("Invalid or missing SubCourseId");
            }
        }


        private List<TopicItem> GetTopics(int subCourseId)
        {
            string connStr = ConfigurationManager.ConnectionStrings["ELearning_Project"].ConnectionString;

            SqlConnection conn = new SqlConnection(connStr);
            List<TopicItem> list = new List<TopicItem>();
            {
                string query = "SELECT TopicId, Title, VideoEmbedCode, SubCourseId FROM Topic WHERE SubCourseId = @SubCourseId ORDER BY TopicId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@SubCourseId", subCourseId);
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    list.Add(new TopicItem
                    {
                        TopicId = Convert.ToInt32(rdr["TopicId"]),
                        Title = rdr["Title"].ToString(),
                        VideoEmbedCode = rdr["VideoEmbedCode"].ToString(),
                        SubCourseId = Convert.ToInt32(rdr["SubCourseId"])
                    });
                }
            }
            return list;
        }

        private void LoadTopics()
        {
            if (ViewState["Topics"] == null && Request.QueryString["scid"] != null)
            {
                int subCourseId = Convert.ToInt32(Request.QueryString["scid"]);
                var topics = GetTopics(subCourseId);

                if (topics != null && topics.Count > 0)
                {
                    ViewState["Topics"] = topics;
                }
                else
                {

                    Response.Write("<script>alert('No topics found for this SubCourse.');</script>");
                }
            }
        }


        //protected void month_duration()
        //{
        //    int user = GetLoggedInUserId();
        //    int subcourseid = Convert.ToInt32(Request.QueryString["scid"]);
        //    string connStr = ConfigurationManager.ConnectionStrings["ELearning_Project"].ConnectionString;

        //    using (SqlConnection conn = new SqlConnection(connStr))
        //    {
        //        string query = $"exec month_limit '{user}' , '{subcourseid}'";
        //        using (SqlCommand cmd = new SqlCommand(query, conn))
        //        {
        //            conn.Open();
        //            SqlDataReader rdr = cmd.ExecuteReader();
        //            {
        //                if (rdr.Read())
        //                {
        //                    DateTime purchase_date = Convert.ToDateTime(rdr["PurchaseDate"]);
        //                    TimeSpan month = DateTime.Now - purchase_date;
        //                    if (month.TotalDays <= 30)
        //                    {
        //                        int current_days = (int)month.TotalDays;
        //                        int daysRemaining = 30 - current_days;
        //                        Label2.Text = $"You Have {daysRemaining} Days Remaining";
        //                    }
        //                    else
        //                    {
        //                        Label2.Text = "Your access to this subcourse has expired.";
        //                        Panel1.Visible = false;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}


        protected void btnSubmitReview_Click(object sender, EventArgs e)
        {
            string review = txtReview.Text;
            // Save it to DB or display as needed
        }


        private int GetLoggedInUserId()
        {
            string connStr = ConfigurationManager.ConnectionStrings["ELearning_Project"].ConnectionString;

            SqlConnection conn = new SqlConnection(connStr);
            int userID = -1;
            string email = Session["Email"]?.ToString();
            if (string.IsNullOrEmpty(email))
            {
                Response.Redirect("~/Login/Login.aspx");
            }
            else
            {
                SqlCommand cmd = new SqlCommand("SELECT UserId FROM [User] WHERE Email = @Email", conn);
                cmd.Parameters.AddWithValue("@Email", email);

                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    userID = Convert.ToInt32(rdr["UserId"]);
                    Session["UserId"] = userID;
                    Label1.Text = "UserId:" + Session["UserId"].ToString();
                }
            }
            return userID;
        }


        protected void Certificate_Click(object sender, CommandEventArgs e)
        {
            int subCourseId = Convert.ToInt32(e.CommandArgument);
            int userId = GetLoggedInUserId();
            string userName = "";
            string courseTitle = "";

            string connStr = ConfigurationManager.ConnectionStrings["ELearning_Project"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlCommand cmdUser = new SqlCommand("SELECT Fullname FROM [User] WHERE UserId = @UserId", conn);
                cmdUser.Parameters.AddWithValue("@UserId", userId);
                object userResult = cmdUser.ExecuteScalar();
                if (userResult != null)
                    userName = userResult.ToString();

                SqlCommand cmdCourse = new SqlCommand(@"SELECT sc.Title AS Title
        FROM SubCourse sc
        JOIN MasterCourse mc ON sc.MasterCourseId = mc.MasterCourseId
        WHERE sc.SubCourseId = @SubCourseId", conn);
                cmdCourse.Parameters.AddWithValue("@SubCourseId", subCourseId);
                object titleResult = cmdCourse.ExecuteScalar();
                if (titleResult != null)
                    courseTitle = titleResult.ToString();

                string fileName = $"Certificate_{userName}_{DateTime.Now:dd-MM-yyyy_HH-mm-ss}.pdf";
                string filePath = "Certificates/" + fileName;
                string fullPath = Server.MapPath("~/" + filePath);

                string certificateDir = Server.MapPath("~/Certificates");
                if (!Directory.Exists(certificateDir))
                {
                    Directory.CreateDirectory(certificateDir);
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
                </style>
            </head>
            <body>
                <h1>Certificate of Completion</h1>
                <p>This is to certify that</p>
                <h2>{userName}</h2>
                <p>has successfully completed the course</p>
                <h2>{courseTitle}</h2>
                <p>on {DateTime.Now:dd/MM/yyyy}</p>
            </body>
        </html>";

                byte[] pdfBytes = GeneratePdfFromHtml(htmlContent);
                File.WriteAllBytes(fullPath, pdfBytes);

                SqlCommand insertCmd = new SqlCommand(@"
            INSERT INTO Certificate (UserId, SubCourseId, IssuedDate, CertificatePath) 
            VALUES (@UserId, @SubCourseId, @IssuedDate, @CertificatePath)", conn);
                insertCmd.Parameters.AddWithValue("@UserId", userId);
                insertCmd.Parameters.AddWithValue("@SubCourseId", subCourseId);
                insertCmd.Parameters.AddWithValue("@IssuedDate", DateTime.Now);
                insertCmd.Parameters.AddWithValue("@CertificatePath", "/" + filePath);
                insertCmd.ExecuteNonQuery();

                if (System.IO.File.Exists(Server.MapPath("~/" + filePath)))
                {
                    Response.Clear();
                    Response.ContentType = "application/pdf";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(filePath));
                    Response.TransmitFile(Server.MapPath("~/" + filePath));
                    Response.End(); 
                }
                else
                {
                    Response.Write("Certificate file not found.");
                }
            }
        }


        private byte[] GeneratePdfFromHtml(string htmlContent)
        {
            string logoPath = Server.MapPath("~/Images/Logo.png");

            using (MemoryStream memoryStream = new MemoryStream())
            {
                Document document = new Document(PageSize.A4, 10f, 10f, 10f, 10f);
                PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                document.Open();


                if (File.Exists(logoPath))
                {
                    iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(logoPath);
                    logo.ScaleToFit(150f, 150f);
                    logo.Alignment = Element.ALIGN_CENTER;
                    document.Add(logo);
                }

                using (StringReader stringReader = new StringReader(htmlContent))
                {
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, stringReader);
                }

                document.Close();
                return memoryStream.ToArray();
            }
        }

        private void BindTopics()
        {
            var topics = ViewState["Topics"] as List<TopicItem>;
            if (topics == null || topics.Count == 0) return;

            mvTopics.Views.Clear();
            foreach (var topic in topics)
            {
                View view = new View();
                view.Controls.Add(new LiteralControl(topic.VideoEmbedCode));
                mvTopics.Views.Add(view);
            }

            if (mvTopics.Views.Count > 0)
            {
                mvTopics.ActiveViewIndex = (ActiveViewIndex >= 0 && ActiveViewIndex < mvTopics.Views.Count)
                    ? ActiveViewIndex
                    : 0;
            }
        }



        private void BindPlaylist()
        {
            rptPlaylist.DataSource = ViewState["Topics"] as List<TopicItem>;
            rptPlaylist.DataBind();
        }

        private void UpdateNavigationButtons()
        {
            var topics = ViewState["Topics"] as List<TopicItem>;
            if (topics != null && topics.Count > 0)
            {
                int currentIndex = mvTopics.ActiveViewIndex;

                btnPrev.Enabled = currentIndex > 0;
                btnNext.Enabled = currentIndex < topics.Count - 1;


                if (currentIndex == topics.Count - 1)
                {
                    btnCertificate.Enabled = true;
                    btnCertificate.CommandArgument = topics[0].SubCourseId.ToString();
                }
                else
                {
                    btnCertificate.Enabled = false;
                }
            }
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            if (ActiveViewIndex < mvTopics.Views.Count - 1)
                ActiveViewIndex++;

            BindTopics();
            BindPlaylist();
            UpdateNavigationButtons();
        }

        private void UpdateCertificateButton()
        {
            btnCertificate.Enabled = (mvTopics.ActiveViewIndex == mvTopics.Views.Count - 1);
        }
        protected void btnPrev_Click(object sender, EventArgs e)
        {
            if (ActiveViewIndex > 0)
                ActiveViewIndex--;

            BindTopics();
            BindPlaylist();
            UpdateNavigationButtons();
        }

        private void MarkVideoWatchedIfNeeded()
        {
            int userId = GetLoggedInUserId();
            int subCourseId = Convert.ToInt32(Request.QueryString["scid"]);

            {
                SqlCommand cmd = new SqlCommand($"INSERT INTO User_SubCourseProgress (UserId, SubCourseId, VideosWatched) VALUES ('{userId}', '{subCourseId}', 1)", conn);
                cmd.ExecuteNonQuery();
            }
        }


        protected void PlayVideo_Command(object sender, CommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            if (index >= 0 && index < mvTopics.Views.Count)
            {
                ActiveViewIndex = index;
            }

            BindTopics();
            BindPlaylist();
            UpdateNavigationButtons();
            MarkVideoWatchedIfNeeded();
        }


        private int ActiveViewIndex
        {
            get => ViewState["ActiveViewIndex"] != null ? (int)ViewState["ActiveViewIndex"] : 0;
            set => ViewState["ActiveViewIndex"] = value;
        }


    }
}
