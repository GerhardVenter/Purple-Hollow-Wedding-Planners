using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Purple_Hollow_Wedding_Planners
{
    public partial class Itinerary_Timeline : System.Web.UI.Page
    {
        int userID;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                rptTimeline.DataSource = GetItinerary();
                rptTimeline.DataBind();
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            Response.Redirect("Itinerary.aspx");
        }
        public DataTable GetItinerary()
        {
            string connStr = ConfigurationManager.ConnectionStrings["MySqlConn"].ConnectionString;

            String username = Session["username"].ToString();
            int userID = getUserId(username);
            string query = @"
   SELECT 
    itineraryID AS ItineraryID,
    itineraryName AS ItemName,
    CONCAT(LPAD(FLOOR(itineraryStartTime / 100), 2, '0'), ':', LPAD(itineraryStartTime % 100, 2, '0')) AS StartTime,
    itineraryDescription AS Description
FROM itinerary
WHERE userID = @userID
ORDER BY itineraryStartTime ASC;";



            using (MySqlConnection conn = new MySqlConnection(connStr))
            using (MySqlCommand cmd = new MySqlCommand(query, conn))              
            using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
            {
                cmd.Parameters.AddWithValue("@userID", userID);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
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

        protected void btnShare_Click(object sender, EventArgs e)
        {
            Response.Redirect("Itinerary_Share.aspx");
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Response.Redirect("Itinerary_Add.aspx");
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