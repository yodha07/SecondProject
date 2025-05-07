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
    public partial class Login : System.Web.UI.Page
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

            SqlCommand cmd = new SqlCommand($"SELECT * FROM Users WHERE Email='{email}' AND Password='{password}'", conn);
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                string role = reader["Role"].ToString();
                Session["Email"] = email;
                Session["Role"] = role;

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
                lblMessage.Text = "Invalid username or password!";
            }

            reader.Close();
            conn.Close();

        }
    }
}