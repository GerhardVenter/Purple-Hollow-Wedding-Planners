using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Purple_Hollow_Wedding_Planners
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Only run on initial page load
            if (!IsPostBack)
            {
                // Show success message after registration
                if (Request.QueryString["registered"] == "true")
                {
                    lblMessage.Text = "Registration successful! Please log in.";
                    lblMessage.ForeColor = System.Drawing.Color.Green;
                    lblMessage.Visible = true;
                }

                // Allow <%# IsPostBack ? ... %> to bind (if used — optional now that fade-in is removed)
                Page.DataBind();
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            // Reset previous error styles (if used)
            txtUsername.CssClass = txtUsername.CssClass.Replace(" error", "");
            txtPassword.CssClass = txtPassword.CssClass.Replace(" error", "");

            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();
            bool hasError = false;

            // Basic field validation
            if (string.IsNullOrWhiteSpace(username))
            {
                txtUsername.CssClass += " error";
                hasError = true;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                txtPassword.CssClass += " error";
                hasError = true;
            }

            if (hasError)
            {
                lblMessage.Text = "Please fill in all required fields.";
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
                    string query = "SELECT * FROM User WHERE username = @username AND password = @password";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password);

                    var reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        // Store session and redirect
                        Session["username"] = username;
                        Session["justLoggedIn"] = true; // For popup if desired
                        Response.Redirect("Home.aspx");
                    }
                    else
                    {
                        lblMessage.Text = "Invalid username or password.";
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                        lblMessage.Visible = true;
                    }
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