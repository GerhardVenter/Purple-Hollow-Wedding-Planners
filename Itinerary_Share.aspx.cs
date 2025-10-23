using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Purple_Hollow_Wedding_Planners
{
    public partial class Itinerary_Share : System.Web.UI.Page
    {
        int userID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
           
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