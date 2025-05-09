using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SecondProject.Login
{
    public partial class Login1 : System.Web.UI.Page
    {
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Project"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            conn.Open();

            SqlCommand cmd = new SqlCommand($"SELECT * FROM Users WHERE Email = '{email}' AND Password = '{password}' AND IsActive = 1", conn);
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                string role = reader["Role"].ToString();
                Session["Email"] = email;
                Session["Role"] = role;
                int userId = Convert.ToInt32(reader["UserID"]);

                reader.Close();

                SqlCommand updateCmd = new SqlCommand("UPDATE Users SET LastLoginAt = @Now WHERE UserID = @UserID", conn);
                updateCmd.Parameters.AddWithValue("@Now", DateTime.Now);
                updateCmd.Parameters.AddWithValue("@UserID", userId);
                updateCmd.ExecuteNonQuery();

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
            }

            conn.Close();

        }
    }
}