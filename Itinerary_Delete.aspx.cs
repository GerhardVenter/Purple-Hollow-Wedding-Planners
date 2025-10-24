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
    public partial class Itinerary_Delete : System.Web.UI.Page
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

        protected void ddlFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
       
        }

        protected void ddlSortBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            sortBy();
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
        

        protected void btnView_Click(object sender, EventArgs e)
        {
            Response.Redirect("Itinerary.aspx");
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Response.Redirect("Itinerary_Add.aspx");
        }

        private void DeleteGuest()
        {
            string connStr = ConfigurationManager.ConnectionStrings["MySqlConn"].ConnectionString;
            string username = Session["username"].ToString();
            int itID = int.Parse(Text1.Value);

            getUserId(username);

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                String deleteQuery = "DELETE FROM itinerary WHERE itineraryID = @itiID AND userID = @userID";
                using (MySqlCommand cmd = new MySqlCommand(deleteQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@itiID", itID);
                    cmd.Parameters.AddWithValue("@userID", userID);
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            }

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
                    DeleteGuest();
                    ClientScript.RegisterStartupScript(this.GetType(), "showSuccess", "showDeleteSuccessPopupGuest();", true);
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "showSuccess", "showDeleteErrorNoMatchEntryPopupGuest();", true);
                }

                conn.Close();
            }
        }

        protected void btnRemoveGUest_Click(object sender, EventArgs e)
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

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            Response.Redirect("Itinerary_Update.aspx");
        }
    }
}