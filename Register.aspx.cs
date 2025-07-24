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
            lblMessage.Visible = false;
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text.Trim();
            string confirmPassword = txtConfirmPassword.Text.Trim();

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

                    try
                    {
                        MailMessage emailMessage = new MailMessage();
                        emailMessage.From = new MailAddress("gojo64831@gmail.com");
                        emailMessage.To.Add(email);
                        emailMessage.Subject = "Your Purple Hollow Wedding Planner Account has been created successfully";
                        emailMessage.Body = $"Hello {username},\n\nYour Purple Hollow Wedding Planner Account has been created successfully!\n\nReady to go beyond infinity?";

                        SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
                        {
                            smtpClient.Port = 587;
                            smtpClient.Credentials = new NetworkCredential("gojo64831@gmail.com", "whfb dflr hgsw puxs ");
                            smtpClient.EnableSsl = true;
                        }

                        smtpClient.Send(emailMessage);
                    }
                    catch (Exception)
                    {

                        throw;
                    }

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
