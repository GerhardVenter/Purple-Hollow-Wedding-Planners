using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Purple_Hollow_Wedding_Planners
{
    public partial class Itinerary_Share : System.Web.UI.Page
    {
        int userID = 0;

        private int getUserId(String username)
        {
            string connStr = ConfigurationManager.ConnectionStrings["MySqlConn"].ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                string query = ("SELECT userID FROM user WHERE username = @username");
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@username", username);



                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        userID = reader.GetInt32("userID");
                    }
                }

                conn.Close();

                return userID;
            }

        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            Response.Redirect("Itinerary.aspx");
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Response.Redirect("Itinerary_Add.aspx");
        }

        protected void btnRemoveGUest_Click(object sender, EventArgs e)
        {

            String username = Session["username"].ToString();
            int userID = getUserId(username);


            string connStr = ConfigurationManager.ConnectionStrings["MySqlConn"].ConnectionString;
            StringBuilder itineraryItemsDB = new StringBuilder();

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                String query = ("SELECT itineraryName, itineraryStartTime, itineraryEndTime FROM itinerary WHERE userID = @userID ORDER BY StartTime");
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@userID", userID);

                conn.Open();

                MySqlDataReader reader = cmd.ExecuteReader();

                itineraryItemsDB.AppendLine("Your Itinerary:\n");

                while (reader.Read())
                {
                    string name = reader["Name"].ToString();
                    int start = Convert.ToInt32(reader["StartTime"]);
                    int end = Convert.ToInt32(reader["EndTime"]);

                    itineraryItemsDB.AppendLine($"Name: {name}");
                    itineraryItemsDB.AppendLine($"Start Time: {start}");
                    itineraryItemsDB.AppendLine($"End Time: {end}");
                    itineraryItemsDB.AppendLine();
                }

                reader.Close();

                conn.Close();

                string itineraryString = itineraryItemsDB.ToString();
                String email = itineraryString;

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
                        Credentials = new NetworkCredential("gojo64831@gmail.com", "goii xxqm mqlx ofmh "),
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
            }
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            Response.Redirect("Itinerary_Update.aspx");
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            Response.Redirect("Itinerary_Delete.aspx");
        }
    }
}