using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Purple_Hollow_Wedding_Planners
{
    public partial class Itinerary_Update : System.Web.UI.Page
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

        private void fillGrid()
        {
            string connStr = ConfigurationManager.ConnectionStrings["MySqlConn"].ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                String username = Session["username"].ToString();
                int userID = getUserId(username);

                conn.Open();
                String query = @"
SELECT 
   itineraryID AS 'Itinerary ID' , itineraryName AS 'Item name',
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
        protected void btnShare_Click(object sender, EventArgs e)
        {
            Response.Redirect("Itinerary_Share.aspx");
        }

        private void fillSort()
        {
            ddlSortBy.Items.Add(new ListItem("None", "None"));
            ddlSortBy.Items.Add(new ListItem("StartTime Asc", "AscitineraryStartTime"));
            ddlSortBy.Items.Add(new ListItem("StartTime Desc", "DescitineraryStartTime"));

            ddlSortBy.Items.Add(new ListItem("EndTime Asc", "AscitineraryEndTime"));
            ddlSortBy.Items.Add(new ListItem("EndTime Desc", "DescitineraryEndTime"));
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

        protected void btnEditGuest_Click(object sender, EventArgs e)
        {
            try
            {
                int guestID = int.Parse(Text1.Value);

                CheckIfUserExist(guestID);


            }
            catch (Exception)
            {

                ClientScript.RegisterStartupScript(this.GetType(), "showSuccess", "showDeleteErrorNullEntryPopupGuest();", true);
            }
        }

        private void EditGuest()
        {
            string connStr = ConfigurationManager.ConnectionStrings["MySqlConn"].ConnectionString;
            string username = Session["username"].ToString();
            int itiID = int.Parse(Text1.Value);

            // Ensure userID is properly set
            int userID = getUserId(username);

            string iName = Text2.Value?.Trim();
            string iDescr = Text3.Value?.Trim();
            string startTime = inpST.Value?.Trim();
            string endTime = inpET.Value?.Trim();

            if (string.IsNullOrEmpty(iName) && string.IsNullOrEmpty(iDescr) && string.IsNullOrEmpty(startTime) && string.IsNullOrEmpty(endTime))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "showEditedNullErrorPopup", "showEditedNullError();", true);
                return;
            }

            int currentStart = 0, currentEnd = 0;

            // Get current times
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("SELECT itineraryStartTime, itineraryEndTime FROM itinerary WHERE itineraryID=@itiID AND userID=@userID", conn))
                {
                    cmd.Parameters.AddWithValue("@itiID", itiID);
                    cmd.Parameters.AddWithValue("@userID", userID);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            currentStart = reader.GetInt32("itineraryStartTime");
                            currentEnd = reader.GetInt32("itineraryEndTime");
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "showNotFound", "alert('Itinerary not found');", true);
                            return;
                        }
                    }
                }
                conn.Close();
            }

            // Duplicate name check
            if (!string.IsNullOrEmpty(iName))
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand("SELECT COUNT(*) FROM itinerary WHERE userID=@userID AND LOWER(TRIM(itineraryName))=@itiName AND itineraryID!=@itiID", conn))
                    {
                        cmd.Parameters.AddWithValue("@userID", userID);
                        cmd.Parameters.AddWithValue("@itiName", iName.ToLower());
                        cmd.Parameters.AddWithValue("@itiID", itiID);
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        if (count > 0)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "showSuccess", "showItiNotNumPopup();", true);
                            return;
                        }
                    }
                    conn.Close();
                }
            }

            // Parse times
            int? sT = string.IsNullOrEmpty(startTime) ? (int?)null : int.Parse(startTime);
            int? eT = string.IsNullOrEmpty(endTime) ? (int?)null : int.Parse(endTime);

            int finalStart = sT ?? currentStart;
            int finalEnd = eT ?? currentEnd;

            // Validate time
            if (finalEnd <= finalStart)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "showSuccess", "showAddedSuccessPopup();", true);
                return;
            }

            // Build single update query dynamically
            List<string> updates = new List<string>();
            if (!string.IsNullOrEmpty(iName)) updates.Add("itineraryName=@itiName");
            if (!string.IsNullOrEmpty(iDescr) && iDescr.Length <= 128) updates.Add("itineraryDescription=@itiDesc");
            if (sT.HasValue) updates.Add("itineraryStartTime=@itiST");
            if (eT.HasValue) updates.Add("itineraryEndTime=@itiET");

            if (updates.Count > 0)
            {
                string updateQuery = "UPDATE itinerary SET " + string.Join(", ", updates) + " WHERE itineraryID=@itiID AND userID=@userID";
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(updateQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@itiID", itiID);
                        cmd.Parameters.AddWithValue("@userID", userID);
                        if (!string.IsNullOrEmpty(iName)) cmd.Parameters.AddWithValue("@itiName", iName);
                        if (!string.IsNullOrEmpty(iDescr) && iDescr.Length <= 128) cmd.Parameters.AddWithValue("@itiDesc", iDescr);
                        if (sT.HasValue) cmd.Parameters.AddWithValue("@itiST", sT.Value);
                        if (eT.HasValue) cmd.Parameters.AddWithValue("@itiET", eT.Value);
                        cmd.ExecuteNonQuery();
                    }
                    conn.Close();
                }
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "showSuccess", "showDeleteSuccessPopupGuest();", true);
            fillGrid();
        }




        private void CheckIfUserExist(int itineraryID)
        {
            string connStr = ConfigurationManager.ConnectionStrings["MySqlConn"].ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                String username = Session["username"].ToString();
                int userID = getUserId(username);

                conn.Open();
                String query = ("SELECT itineraryID FROM itinerary WHERE userID = @userID AND itineraryID = @itiID");
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@userID", userID);
                cmd.Parameters.AddWithValue("@itiID", itineraryID);

                MySqlDataAdapter adapter = new MySqlDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet itinerary = new DataSet();
                adapter.Fill(itinerary);
                conn.Close();

                if (itinerary.Tables[0].Rows.Count != 0)
                {
                    EditGuest();
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "showSuccess", "showDeleteErrorNoMatchEntryPopupGuest();", true);
                }
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            Response.Redirect("Itinerary.aspx");
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            Response.Redirect("Itinerary_Delete.aspx");
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Response.Redirect("Itinerary_Add.aspx");
        }

        protected void ddlSortBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            sortBy();
        }

        protected void btnTimeLine_Click(object sender, EventArgs e)
        {
            Response.Redirect("Itinerary_Timeline.aspx");
        }

        void sortBy()
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

                    query = ($@"SELECT 
   itineraryID AS 'Itinerary ID' , itineraryName AS 'Item name',
    CONCAT(LPAD(FLOOR(itineraryStartTime / 100), 2, '0'), ':', LPAD(itineraryStartTime % 100, 2, '0')) AS 'Start time',
    CONCAT(LPAD(FLOOR(itineraryEndTime / 100), 2, '0'), ':', LPAD(itineraryEndTime % 100, 2, '0')) AS 'End time',
    itineraryDescription AS 'Short description' FROM itinerary WHERE userID = @userID ORDER BY {fL} {ascDesc}");
                    MySqlCommand cmd = new MySqlCommand(query, conn);
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