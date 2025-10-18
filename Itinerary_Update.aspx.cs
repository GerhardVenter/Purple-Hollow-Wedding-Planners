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
            //string connStr = ConfigurationManager.ConnectionStrings["MySqlConn"].ConnectionString;
            //string username = Session["username"].ToString();
            //int guestID = int.Parse(Text1.Value);

            //getUserId(username);

            ////Assigning variables
            //String fName = Text2.Value;
            //String lName = Text3.Value;
            //String dlDS = ddlDS.SelectedValue;
            //String dlRS = ddlRS.SelectedValue;

            //if (string.IsNullOrEmpty(fName) && string.IsNullOrEmpty(lName) && dlDS == "Default" && dlRS == "Default")
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "showEditedNullErrorPopup", "showEditedNullError();", true);
            //}
            //else
            //{
            //    using (MySqlConnection conn = new MySqlConnection(connStr))
            //    {

            //        String editQuery = "";
            //        if (!string.IsNullOrEmpty(fName))
            //        {
            //            conn.Open();
            //            editQuery = "UPDATE guest SET guestFName = @guestFName WHERE guestID = @guestID AND userID = @userID";
            //            using (MySqlCommand cmd = new MySqlCommand(editQuery, conn))
            //            {
            //                cmd.Parameters.AddWithValue("@guestID", guestID);
            //                cmd.Parameters.AddWithValue("@userID", userID);
            //                cmd.Parameters.AddWithValue("@guestFName", fName);
            //                cmd.ExecuteNonQuery();
            //            }
            //            conn.Close();
            //        }

            //        if (!string.IsNullOrEmpty(lName))
            //        {
            //            conn.Open();
            //            editQuery = "UPDATE guest SET guestLName = @guestLName WHERE guestID = @guestID AND userID = @userID";
            //            using (MySqlCommand cmd = new MySqlCommand(editQuery, conn))
            //            {
            //                cmd.Parameters.AddWithValue("@guestID", guestID);
            //                cmd.Parameters.AddWithValue("@userID", userID);
            //                cmd.Parameters.AddWithValue("@guestLName", lName);
            //                cmd.ExecuteNonQuery();
            //            }
            //            conn.Close();
            //        }

            //        if (dlDS != "Default")
            //        {
            //            conn.Open();
            //            editQuery = "UPDATE guest SET guestDSelection = @guestDS WHERE guestID = @guestID AND userID = @userID";
            //            using (MySqlCommand cmd = new MySqlCommand(editQuery, conn))
            //            {
            //                cmd.Parameters.AddWithValue("@guestID", guestID);
            //                cmd.Parameters.AddWithValue("@userID", userID);
            //                cmd.Parameters.AddWithValue("@guestDS", dlDS);
            //                cmd.ExecuteNonQuery();
            //            }
            //            conn.Close();
            //        }

            //        if (dlRS != "Default")
            //        {
            //            conn.Open();
            //            editQuery = "UPDATE guest SET guestRSelection = @guestRS WHERE guestID = @guestID AND userID = @userID";
            //            using (MySqlCommand cmd = new MySqlCommand(editQuery, conn))
            //            {
            //                cmd.Parameters.AddWithValue("@guestID", guestID);
            //                cmd.Parameters.AddWithValue("@userID", userID);
            //                cmd.Parameters.AddWithValue("@guestRS", dlRS);
            //                cmd.ExecuteNonQuery();
            //            }
            //            conn.Close();
            //        }


            //    }
            //    ClientScript.RegisterStartupScript(this.GetType(), "showSuccess", "showDeleteSuccessPopupGuest();", true);
            //    fillGrid();
            //}

        }

        private void CheckIfUserExist(int itineraryID)
        {
            string connStr = ConfigurationManager.ConnectionStrings["MySqlConn"].ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                String username = Session["username"].ToString();
                int userID = getUserId(username);

                conn.Open();
                String query = ("SELECT itineraryID FROM guest WHERE userID = @userID AND itineraryID = @itiID");
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
            Response.Redirect("Guests.aspx");
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            Response.Redirect("Guest_Delete.aspx");
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Response.Redirect("Guest_Add.aspx");
        }

        protected void ddlFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            filter();
        }

        protected void filter()
        {
            String selected = ddlFilterBy.SelectedValue;
            String username = Session["username"].ToString();

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
                    String query = ("SELECT guestFName AS 'First Name', guestLName AS 'Last Name', guestDSelection AS 'Dietary Selection', guestRSelection AS 'RSVP', guestEmail AS 'Email' FROM guest WHERE userID = @userID AND (guestDSelection = @selected OR guestRSelection = @selected)");
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@userID", userID);
                    cmd.Parameters.AddWithValue("@selected", selected);

                    MySqlDataReader dbRdr = cmd.ExecuteReader();
                    gvGuests.DataSource = dbRdr;
                    gvGuests.DataBind();
                    conn.Close();
                }
            }
        }

        protected void ddlSortBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            sortBy();
        }

        void sortBy()
        {
            String filterSelected = ddlFilterBy.SelectedValue;
            String selected = ddlSortBy.SelectedValue;
            String username = Session["username"].ToString();
            String ascDesc = "ASC";
            String fL = "guestFName";
            String query = "";

            if (selected == "None" && filterSelected == "None")
            {
                fillGrid();
            }
            else if (selected == "None")
            {
                filter();
            }
            else
            {


                string connStr = ConfigurationManager.ConnectionStrings["MySqlConn"].ConnectionString;

                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    getUserId(username);

                    conn.Open();

                    if (filterSelected == "None")
                    {

                        if (selected[0] == 'A')
                        {
                            ascDesc = "ASC";
                            if (selected[8] == 'F')
                            {
                                fL = "guestFName";
                            }
                            else
                            {
                                fL = "guestLName";
                            }
                        }
                        else
                        {
                            ascDesc = "Desc";

                            if (selected[9] == 'F')
                            {
                                fL = "guestFName";
                            }
                            else
                            {
                                fL = "guestLName";
                            }
                        }

                        query = ($@"SELECT guestFName AS 'First Name', guestLName AS 'Last Name', guestDSelection AS 'Dietary Selection', guestRSelection AS 'RSVP', guestEmail AS 'Email' FROM guest WHERE userID = @userID ORDER BY {fL} {ascDesc}");
                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@userID", userID);

                        MySqlDataReader dbRdr = cmd.ExecuteReader();
                        gvGuests.DataSource = dbRdr;
                        gvGuests.DataBind();
                        conn.Close();
                    }
                    else
                    {
                        if (selected[0] == 'A')
                        {
                            ascDesc = "ASC";
                            if (selected[8] == 'F')
                            {
                                fL = "guestFName";
                            }
                            else
                            {
                                fL = "guestLName";
                            }
                        }
                        else
                        {
                            ascDesc = "Desc";

                            if (selected[9] == 'F')
                            {
                                fL = "guestFName";
                            }
                            else
                            {
                                fL = "guestLName";
                            }
                        }

                        query = ($@"SELECT guestFName AS 'First Name', guestLName AS 'Last Name', guestDSelection AS 'Dietary Selection', guestRSelection AS 'RSVP', guestEmail AS 'Email' FROM guest WHERE userID = @userID AND (guestDSelection = @selected OR guestRSelection = @selected) ORDER BY {fL} {ascDesc}");
                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@userID", userID);
                        cmd.Parameters.AddWithValue("@selected", filterSelected);
                        MySqlDataReader dbRdr = cmd.ExecuteReader();
                        gvGuests.DataSource = dbRdr;
                        gvGuests.DataBind();
                        conn.Close();
                    }


                }
            }
        }
    }
}