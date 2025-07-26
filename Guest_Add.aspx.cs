using MySql.Data.MySqlClient;
using Mysqlx.Connection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Purple_Hollow_Wedding_Planners
{
    public partial class Guest_Add : System.Web.UI.Page
    {
        int userID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlRS.Items.Add(new ListItem("RSVP NA", "NA"));
                ddlRS.Items.Add(new ListItem("RSVP Reception Only", "Reception Only"));
                ddlRS.Items.Add(new ListItem("RSVP All Events", "All Events"));
                ddlRS.Items.Add(new ListItem("RSVP Ceremony Only", "Ceremony Only"));              

                ddlDS.Items.Add(new ListItem("NA", "NA"));
                ddlDS.Items.Add(new ListItem("Vegan", "Vegan"));
                ddlDS.Items.Add(new ListItem("Vegetarian", "Vegetarian"));
                ddlDS.Items.Add(new ListItem("Standard", "Standard"));
                ddlDS.Items.Add(new ListItem("Gluten-Free", "Gluten-Free"));
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            Response.Redirect("Guests.aspx");
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            String rS = ddlRS.SelectedValue;
            String dS = ddlDS.SelectedValue;
            String username = Session["username"].ToString();
            userID = getUserId(username);

            String gFName = Text1.Value;
            String gLName = Text2.Value;
            String email = Text3.Value;

            String connStr = ConfigurationManager.ConnectionStrings["MySqlConn"].ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                String query = ("INSERT INTO guest (`userID`, `guestFName`, `guestLName`, `guestDSelection`, `guestRSelection`, `guestEmail`) VALUES (@userID, @gFName, @gLName, @ddlDS, @ddlRS, @email)");
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@userID", userID);
                cmd.Parameters.AddWithValue("@gFName", gFName);
                cmd.Parameters.AddWithValue("@gLName", gLName);
                cmd.Parameters.AddWithValue("@ddlDS", rS);
                cmd.Parameters.AddWithValue("@ddlRS", dS);
                cmd.Parameters.AddWithValue("@email", email);

                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

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
    }
}