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
    public partial class Guests : System.Web.UI.Page
    {
        int userID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) {
                string connStr = ConfigurationManager.ConnectionStrings["MySqlConn"].ConnectionString;

                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    String username = Session["username"].ToString();
                    conn.Open();
                    string query = ("SELECT userID FROM user WHERE username = @username");
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@username", username);



                    using (MySqlDataReader reader = cmd.ExecuteReader()) {
                        if (reader.Read())
                        {
                            userID = reader.GetInt32("userID");
                        }                    
                    }

                    conn.Close();

                    conn.Open();
                    query = ("SELECT * FROM guest WHERE userID = @userID");
                    cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@userID", userID);

                    MySqlDataReader dbRdr = cmd.ExecuteReader();
                    gvGuests.DataSource = dbRdr;
                    gvGuests.DataBind();
                    conn.Close();
                }
            }
        }
    }
}