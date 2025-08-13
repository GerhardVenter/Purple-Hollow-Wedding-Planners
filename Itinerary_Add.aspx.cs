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

        private void fillGrid()
        {
            string connStr = ConfigurationManager.ConnectionStrings["MySqlConn"].ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                String username = Session["username"].ToString();
                int userID = getUserId(username);

                conn.Open();
                String query = ("SELECT itineraryName AS 'Item name', itineraryStartTime AS 'Start time', itineraryEndTime AS 'End time', itineraryDescription AS 'Short description' FROM itinerary WHERE userID = @userID");
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
                ClientScript.RegisterStartupScript(this.GetType(), "showSuccess", "showErrorPopupGuest();", true);
            }
            else if (descr.Length > 128)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "showSuccess", "showItiNotNumPopup();", true);
            }

            else
            {
                try
                {
                    int sT = int.Parse(startTime);
                    int eT = int.Parse(endTime);

                    Add(itemName, descr, sT, eT);

                    ClientScript.RegisterStartupScript(this.GetType(), "showSuccess", "showAddedSuccessPopup();", true);
                }
                catch (Exception)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "showSuccess", "showItiNotNumPopup();", true);
                }


            }

        }

        private void Add(String itemName, String descr, int startTime, int endTime)
        {
            String rS = ddlRS.SelectedValue;
            String dS = ddlDS.SelectedValue;
            String username = Session["username"].ToString();
            userID = getUserId(username);

            String gFName = inpNam.Value;
            String gLName = Text2.Value;

            String connStr = ConfigurationManager.ConnectionStrings["MySqlConn"].ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                String query = ("INSERT INTO guest (`userID`, `guestFName`, `guestLName`, `guestDSelection`, `guestRSelection`) VALUES (@userID, @gFName, @gLName, @ddlDS, @ddlRS)");
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@userID", userID);
                cmd.Parameters.AddWithValue("@gFName", gFName);
                cmd.Parameters.AddWithValue("@gLName", gLName);
                cmd.Parameters.AddWithValue("@ddlDS", dS);
                cmd.Parameters.AddWithValue("@ddlRS", rS);

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
            Response.Redirect("Guest_Delete.aspx");
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            Response.Redirect("Guest_Edit.aspx");
        }
    }
}