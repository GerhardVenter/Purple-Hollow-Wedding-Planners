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
    public partial class Itinerary_Add : System.Web.UI.Page
    {
        int userID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Grid
                fillGrid();


                //Sort
                fillSort();
            }
        }
        protected void btnShare_Click(object sender, EventArgs e)
        {
            Response.Redirect("Itinerary_Share.aspx");
        }
        private void fillGrid()
        {
            String username = Session["username"].ToString();
            int userID = getUserId(username);

            string connStr = ConfigurationManager.ConnectionStrings["MySqlConn"].ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
               

                conn.Open();
                String query = @"
SELECT 
    itineraryName AS 'Item name',
    CONCAT(LPAD(FLOOR(itineraryStartTime / 100), 2, '0'), ':', LPAD(itineraryStartTime % 100, 2, '0')) AS 'Start time',
    CONCAT(LPAD(FLOOR(itineraryEndTime / 100), 2, '0'), ':', LPAD(itineraryEndTime % 100, 2, '0')) AS 'End time',
    itineraryDescription AS 'Short description'
FROM itinerary 
WHERE userID = @userID";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@userID", userID);

                MySqlDataReader dbRdr = cmd.ExecuteReader();
                gvGuests.DataSource = dbRdr;
                gvGuests.DataBind();
                conn.Close();
            }
        }

        private void fillSort()
        {
            ddlSortBy.Items.Add(new ListItem("None", "None"));
            ddlSortBy.Items.Add(new ListItem("StartTime Asc", "AscitineraryStartTime"));
            ddlSortBy.Items.Add(new ListItem("StartTime Desc", "DescitineraryStartTime"));

            ddlSortBy.Items.Add(new ListItem("EndTime Asc", "AscitineraryEndTime"));
            ddlSortBy.Items.Add(new ListItem("EndTime Desc", "DescitineraryEndTime"));
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            Response.Redirect("Guests.aspx");
        }

        protected void ddlSortBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            String selected = ddlSortBy.SelectedValue;
            String username = Session["username"].ToString();
            String ascDesc = "ASC";
            String fL = "itineraryStartTime";
            String query = "";

            if (selected == "None")
            {
                fillGrid();
            }
            else
            {
                string connStr = ConfigurationManager.ConnectionStrings["MySqlConn"].ConnectionString;

                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    getUserId(username);

                    conn.Open();

                    if (selected[0] == 'A')
                    {
                        ascDesc = "ASC";
                        if (selected[12] == 'S')
                        {
                            fL = "itineraryStartTime";
                        }
                        else
                        {
                            fL = "itineraryEndTime";
                        }
                    }
                    else
                    {
                        ascDesc = "Desc";

                        if (selected[13] == 'S')
                        {
                            fL = "itineraryStartTime";
                        }
                        else
                        {
                            fL = "itineraryEndTime";
                        }
                    }

                    query = ($@"SELECT itineraryName AS 'Item name', itineraryStartTime AS 'Start time', itineraryEndTime AS 'End time', itineraryDescription AS 'Short description' FROM itinerary WHERE userID = @userID ORDER BY {fL} {ascDesc}");
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@userID", userID);

                    MySqlDataReader dbRdr = cmd.ExecuteReader();
                    gvGuests.DataSource = dbRdr;
                    gvGuests.DataBind();
                    conn.Close();
                }
            }
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            String itemName = inpNam.Value;
            String startTime = inpST.Value;
            String descr = inpDesc.Value;
            String endTime = inpET.Value;

            if (string.IsNullOrWhiteSpace(itemName) || string.IsNullOrWhiteSpace(startTime) || string.IsNullOrEmpty(endTime))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "showError", "showErrorPopupGuest();", true);
            }
            else if (descr.Length > 128)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "showError", "showItiTooLongPopup();", true);
            }
            else
            {
                int sT = int.Parse(startTime);
                int eT = int.Parse(endTime);

                // Check if endTime is smaller than startTime
                if (eT <= sT)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "showSuccess", "showDeleteErrorNoMatchEntryPopupGuest();", true);
                }
                else if (ItineraryNameExists(itemName))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "showSuccess", "showDeleteErrorNullEntryPopupGuest();", true);
                }
                else
                {
                    Add(itemName, descr, sT, eT);
                    fillGrid();
                    ClientScript.RegisterStartupScript(this.GetType(), "showSuccess", "showAddedSuccessPopup();", true);
                }
                    
            }


        }


        private bool ItineraryNameExists(string itemName)
        {
            string username = Session["username"].ToString();
            int userID = getUserId(username);
            string connStr = ConfigurationManager.ConnectionStrings["MySqlConn"].ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM itinerary WHERE userID = @userID AND itineraryName = @itiName";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@userID", userID);
                cmd.Parameters.AddWithValue("@itiName", itemName);

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
        }



        private void Add(String itemName, String descr, int startTime, int endTime)
        {
            String username = Session["username"].ToString();
            userID = getUserId(username);

            String connStr = ConfigurationManager.ConnectionStrings["MySqlConn"].ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                String query = ("INSERT INTO itinerary (`userID`, `itineraryName`, `itineraryStartTime`, `itineraryEndTime`, `itineraryDescription`) VALUES (@userID, @itiName, @itiST, @itiET, @desc)");
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@userID", userID);
                cmd.Parameters.AddWithValue("@itiName", itemName);
                cmd.Parameters.AddWithValue("@itiST", startTime);
                cmd.Parameters.AddWithValue("@itiET", endTime);
                cmd.Parameters.AddWithValue("@desc", descr);

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

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            Response.Redirect("Itinerary_Delete.aspx");
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            Response.Redirect("Itinerary_Update.aspx");
        }

        protected void btnView_Click1(object sender, EventArgs e)
        {
            Response.Redirect("Itinerary.aspx");
        }

        protected void btnTimeLine_Click(object sender, EventArgs e)
        {
            Response.Redirect("Itinerary_Timeline.aspx");
        }
    }
}