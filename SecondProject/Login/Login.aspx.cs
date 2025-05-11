using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Net;
using System.IO;
using ASPSnippets.GoogleAPI;
using System.Web.Script.Serialization;
using System.Data.SqlClient;
using System.Configuration;

namespace SecondProject.Login

{

    public partial class Login : System.Web.UI.Page

    {

        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ELearning_Project"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            GoogleConnect.ClientId = ConfigurationManager.AppSettings["GoogleClientId"];
            GoogleConnect.ClientSecret = ConfigurationManager.AppSettings["GoogleClientSecret"];
            GoogleConnect.RedirectUri = "https://localhost:44379/Login/Login.aspx";

            if (!this.IsPostBack)
            {
                string code = Request.QueryString["code"];
                if (!string.IsNullOrEmpty(code))
                {
                    GoogleConnect connect = new GoogleConnect();
                    string json = connect.Fetch("me", code);
                    GoogleProfile profile = new JavaScriptSerializer().Deserialize<GoogleProfile>(json);

                    string email = profile.Email;
                    string name = profile.Name;

                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT UserId, Role FROM [User] WHERE Email = @Email", conn);
                    cmd.Parameters.AddWithValue("@Email", email);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        // Existing user
                        int userId = Convert.ToInt32(reader["UserId"]);
                        string role = reader["Role"].ToString();

                        Session["Email"] = email;
                        Session["Role"] = role;
                        Session["UserId"] = userId;

                        reader.Close();

                        SqlCommand updateCmd = new SqlCommand("UPDATE [User] SET LastLogin = @Now WHERE UserId = @UserId", conn);
                        updateCmd.Parameters.AddWithValue("@Now", DateTime.Now);
                        updateCmd.Parameters.AddWithValue("@UserId", userId);
                        updateCmd.ExecuteNonQuery();
                    }
                    else
                    {
                        // New user - insert
                        reader.Close(); // IMPORTANT before reusing connection

                        SqlCommand insertCmd = new SqlCommand(@"
                    INSERT INTO [User] (FullName, Email, Role, LastLogin)
                    VALUES (@FullName, @Email, 'User', @Now);
                    SELECT SCOPE_IDENTITY();", conn);

                        insertCmd.Parameters.AddWithValue("@FullName", name);
                        insertCmd.Parameters.AddWithValue("@Email", email);
                        insertCmd.Parameters.AddWithValue("@Now", DateTime.Now);

                        int newUserId = Convert.ToInt32(insertCmd.ExecuteScalar());

                        Session["Email"] = email;
                        Session["Role"] = "User";
                        Session["UserId"] = newUserId;
                    }

                    conn.Close();

                    Response.Redirect("~/User/UserGreet.aspx");
                }

                if (Request.QueryString["error"] == "access_denied")
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Access denied.')", true);
                }
            }
        }



        public class GoogleProfile
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Picture { get; set; }
            public string Email { get; set; }
            public string Verified_Email { get; set; }
        }


        protected void LoginBtn(object sender, EventArgs e)
        {
            GoogleConnect.Authorize("profile", "email");
        }


        protected void btnLogin_Click(object sender, EventArgs e)

        {

            string email = txtUsername.Text.Trim();

            string password = txtPassword.Text.Trim();


            conn.Open();


            SqlCommand cmd = new SqlCommand("SELECT UserId, Role FROM [User] WHERE Email = @Email AND PasswordHash = @Password", conn);

            cmd.Parameters.AddWithValue("@Email", email);

            cmd.Parameters.AddWithValue("@Password", password);


            SqlDataReader reader = cmd.ExecuteReader();


            if (reader.Read())

            {

                int userId = Convert.ToInt32(reader["UserId"]);

                string role = reader["Role"].ToString();


                Session["Email"] = email;

                Session["Role"] = role;

                Session["UserId"] = userId;


                reader.Close();


                SqlCommand updateCmd = new SqlCommand("UPDATE [User] SET LastLogin = @Now WHERE UserId = @UserId", conn);

                updateCmd.Parameters.AddWithValue("@Now", DateTime.Now);

                updateCmd.Parameters.AddWithValue("@UserId", userId);

                updateCmd.ExecuteNonQuery();


                conn.Close();


                if (role == "Admin")

                {

                    Response.Redirect("~/Admin/AdminGreet.aspx");

                }

                else

                {

                    Response.Redirect("~/User/UserGreet.aspx");

                }

            }

            else

            {

                reader.Close();

                lblMessage.Text = "Invalid username or password!";

                conn.Close();

            }

        }

    }

}