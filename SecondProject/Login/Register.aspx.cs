using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace SecondProject.Login
{
    public partial class Register1 : System.Web.UI.Page
    {
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ELearning_Project"].ConnectionString);

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text.Trim();
            string confirmPassword = txtConfirmPassword.Text.Trim();
            string name = txtName.Text.Trim();

            if (password != confirmPassword)
            {
                lblPasswordError.Text = "Passwords do not match.";
                return;
            }

            
            if (IsEmailExist(email))
            {
                lblEmailError.Text = "Email already registered.";
                return;
            }

            
            string hashedPassword = password;

            
            string query = "INSERT INTO [User] (FullName, Email, PasswordHash, IsActive, RegisteredAt, Role) VALUES (@FullName, @Email, @PasswordHash, 1, @RegisteredAt, 'User')";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@FullName", name); 
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@PasswordHash", hashedPassword);
            cmd.Parameters.AddWithValue("@RegisteredAt", DateTime.Now);

            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                lblSuccessMessage.Text = "Registration successful!";
            }
            catch (Exception ex)
            {
                lblSuccessMessage.Text = "Error: " + ex.Message;
            }
            finally
            {
                conn.Close();
            }
        }

        
        private bool IsEmailExist(string email)
        {
            string query = "SELECT COUNT(*) FROM [User] WHERE Email = @Email";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Email", email);

            try
            {
                conn.Open();
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
            catch
            {
                return false;
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
