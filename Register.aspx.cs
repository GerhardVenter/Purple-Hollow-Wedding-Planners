using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;

namespace Purple_Hollow_Wedding_Planners
{
    public partial class Register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Hide message label on first load (not on postback)
            if (!IsPostBack)
            {
                lblMessage.Visible = false;
            }
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text.Trim();
            string confirmPassword = txtConfirmPassword.Text.Trim();

            // Reset error styling
            txtUsername.CssClass = txtUsername.CssClass.Replace(" error", "");
            txtEmail.CssClass = txtEmail.CssClass.Replace(" error", "");
            txtPassword.CssClass = txtPassword.CssClass.Replace(" error", "");
            txtConfirmPassword.CssClass = txtConfirmPassword.CssClass.Replace(" error", "");

            bool hasError = false;

            if (string.IsNullOrWhiteSpace(username))
            {
                txtUsername.CssClass += " error";
                hasError = true;
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                txtEmail.CssClass += " error";
                hasError = true;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                txtPassword.CssClass += " error";
                hasError = true;
            }

            if (string.IsNullOrWhiteSpace(confirmPassword))
            {
                txtConfirmPassword.CssClass += " error";
                hasError = true;
            }

            if (hasError)
            {
                lblMessage.Text = "Please fill in all fields.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Visible = true;
                return;
            }

            if (password != confirmPassword)
            {
                txtPassword.CssClass += " error";
                txtConfirmPassword.CssClass += " error";
                lblMessage.Text = "Passwords do not match!";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Visible = true;
                return;
            }

            // Input validation
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(confirmPassword))
            {
                lblMessage.Text = "Please fill in all fields.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Visible = true;
                return;
            }

            if (password != confirmPassword)
            {
                lblMessage.Text = "Passwords do not match!";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Visible = true;
                return;
            }

            string connStr = ConfigurationManager.ConnectionStrings["MySqlConn"].ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                try
                {
                    conn.Open();

                    string query = "INSERT INTO User (username, email, password) VALUES (@username, @email, @password)";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@password", password);
                    cmd.ExecuteNonQuery();

                    // Send confirmation email
                    try
                    {
                        MailMessage emailMessage = new MailMessage();
                        emailMessage.From = new MailAddress("gojo64831@gmail.com");
                        emailMessage.To.Add(email);
                        emailMessage.Subject = "Your Purple Hollow Wedding Planner Account";
                        emailMessage.Body = $"Hello {username},\n\nYour Purple Hollow Wedding Planner Account has been created successfully!\n\nReady to go beyond infinity?";

                        SmtpClient smtpClient = new SmtpClient("smtp.gmail.com")
                        {
                            Port = 587,
                            Credentials = new NetworkCredential("gojo64831@gmail.com", "whfb dflr hgsw puxs"),
                            EnableSsl = true
                        };

                        smtpClient.Send(emailMessage);
                    }
                    catch (Exception)
                    {
                        // You can choose to show a warning if the email fails (optional)
                        // lblMessage.Text = "Account created, but failed to send confirmation email.";
                        // lblMessage.ForeColor = System.Drawing.Color.Orange;
                    }

                    // Redirect with registration flag
                    Response.Redirect("Login.aspx?registered=true");
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Error: " + ex.Message;
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    lblMessage.Visible = true;
                }
            }
        }
    }
}
