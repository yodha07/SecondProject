using System;

namespace SecondProject.User
{
    public partial class UserGreet : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Change 'UserEmail' to 'Email' to match the key used in the login page
            string email = Session["Email"]?.ToString();

            if (string.IsNullOrEmpty(email))
            {
                Response.Redirect("~/Login/Login.aspx");
            }
            else
            {
                lblGreeting.Text = "Hello, " + " (" + email + ")";
            }
        }
    }
}
